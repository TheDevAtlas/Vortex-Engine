using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure
{
    public int id;
    public int vid;
    public string nickname;
    public int coin;
    public int xp;
    public int hat;
    public string upgrades;
    public byte[][] data = new byte[0x40][];
    public byte[] decryptedData = new byte[1024];
    public byte[][] finalData = new byte[0x40][];

    public FigureObject skylander;
}

public class PlayerController : MonoBehaviour
{
    public byte toyID;

    public PortalController portal;
    public PlayerHUD hud;
    public Figure figure;
    public int expectedToys = 1;
    public int playerID;
    public byte[] ReadCommands = new byte[]{
    0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F,
0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F,
0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F,
0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F,
0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F,
0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F,
0x60, 0x61, 0x62, 0x63, 0x64, 0x65
    };

    //

    public FigureObject[] allSkylanders;
    void Start()
    {
        ReadCommands = new byte[]{
    0x00, 0x01, 0x08, 0x09, 0x0A, 0x0C, 0x0D, 0x10, 0x24, 0x25, 0x26, 0x28, 0x29, 0x2C
    };
        figure = new Figure();
        toyID = 0xFF;
        portal = GameObject.FindGameObjectWithTag("GameController").GetComponent<PortalController>();
        hud = portal.AddPlayer(this);

        figure.data = new byte[0x40][];
        figure.decryptedData = new byte[1024];
        figure.finalData = new byte[0x40][];

        for (int i = 0; i < figure.data.Length; i++)
        {
            figure.data[i] = new byte[0x10];
            figure.finalData[i] = new byte[0x10];
        }

        figure.skylander = null;
    }

    public void SubmitData()
    {
        

        figure.id = figure.data[0x01][0x00] + (figure.data[0x01][0x01] << 8);
        figure.vid = figure.data[0x01][0x0C] + (figure.data[0x01][0x0D] << 8);

        // Get The Skylander //
        for(int i = 0; i < allSkylanders.Length; i++)
        {
            if (allSkylanders[i].ID == figure.id && allSkylanders[i].VID == figure.vid)
            {
                figure.skylander = allSkylanders[i];
                break;
            }
        }

        // Get An Older Skylander That Matchs The Toy //
        if(figure.skylander == null)
        {
            for (int i = 0; i < allSkylanders.Length; i++)
            {
                if (allSkylanders[i].ID == figure.id)
                {
                    figure.skylander = allSkylanders[i];
                    break;
                }
            }
        }

        // GET THE LAST AREA SAVED, BIGGER IS THE ONE TO READ, SMALLER IS THE ONE TO WRITE TO// - 0x08 (0x09) || 0x24 (0x09)
        int area0 = LogDecryptedData(0x08, 0x09);
        int area1 = LogDecryptedData(0x24, 0x09);
        print("AREA VALUE: " + FormatBytes((byte)area0, (byte)area1));

        if (area0 < area1)
        {

            // GET MONEY // - 0x08 (0x03 + 0x04)
            int money = BitConverter.ToUInt16(new byte[] { LogDecryptedData(0x08, 0x03), LogDecryptedData(0x08, 0x04) }, 0);
            figure.coin = money;
            print("Money: " + money);

            // GET XP // - 0x08 (0x00 + 0x01 + 0x02)
            int xp = BitConverter.ToUInt16(new byte[] { LogDecryptedData(0x08, 0x00), LogDecryptedData(0x08, 0x01), LogDecryptedData(0x08, 0x02) }, 0);
            figure.xp = xp;
            print("XP: " + xp);

            // GET LE OF UPGRADES // - 0x09 (0x00 + 0x01)
            int up1 = LogDecryptedData(0x09, 0x00);
            int up2 = LogDecryptedData(0x09, 0x01);
            figure.upgrades = FormatBytes((byte)up1, (byte)up2);

            // GET HAT ID // - 0x09 (0x04 + 0x05)
            int hat = BitConverter.ToUInt16(new byte[] { LogDecryptedData(0x09, 0x04), LogDecryptedData(0x09, 0x05) }, 0);
            figure.hat = hat;
            print("HAT ID: " + hat);

            // GET NICKNAME ONE + TWO // - 0x26 (0x00 +/+ 0x0F)
            byte[] nickBytes =
            {
                LogDecryptedData(0x26, 0x00), LogDecryptedData(0x26, 0x01),
                LogDecryptedData(0x26, 0x02), LogDecryptedData(0x26, 0x03),
                LogDecryptedData(0x26, 0x04), LogDecryptedData(0x26, 0x05),
                LogDecryptedData(0x26, 0x06), LogDecryptedData(0x26, 0x07),
                LogDecryptedData(0x26, 0x08), LogDecryptedData(0x26, 0x09),
                LogDecryptedData(0x26, 0x0A), LogDecryptedData(0x26, 0x0B),
                LogDecryptedData(0x26, 0x0C), LogDecryptedData(0x26, 0x0D),
                LogDecryptedData(0x26, 0x0E), LogDecryptedData(0x26, 0x0F),
                LogDecryptedData(0x28, 0x00), LogDecryptedData(0x28, 0x01),
                LogDecryptedData(0x28, 0x02), LogDecryptedData(0x28, 0x03),
                LogDecryptedData(0x28, 0x04), LogDecryptedData(0x28, 0x05),
                LogDecryptedData(0x28, 0x06), LogDecryptedData(0x28, 0x07),
                LogDecryptedData(0x28, 0x08), LogDecryptedData(0x28, 0x09),
                LogDecryptedData(0x28, 0x0A), LogDecryptedData(0x28, 0x0B),
                LogDecryptedData(0x28, 0x0C), LogDecryptedData(0x28, 0x0D),
                LogDecryptedData(0x28, 0x0E), LogDecryptedData(0x28, 0x0F)
            };

            string nickname = ConvertBytesToString(nickBytes);
            figure.nickname = nickname;
            print("NICKNAME: " + nickname);

        }
        else
        {

            // GET MONEY // - 0x08 (0x03 + 0x04)
            int money = BitConverter.ToUInt16(new byte[] { LogDecryptedData(0x24, 0x03), LogDecryptedData(0x24, 0x04) }, 0);
            figure.coin = money;
            print("Money: " + money);

            // GET XP // - 0x08 (0x00 + 0x01 + 0x02)
            int xp = BitConverter.ToUInt16(new byte[] { LogDecryptedData(0x24, 0x00), LogDecryptedData(0x24, 0x01), LogDecryptedData(0x24, 0x02) }, 0);
            figure.xp = xp;
            print("XP: " + xp);

            // GET LE OF UPGRADES // - 0x09 (0x00 + 0x01)
            byte up1 = LogDecryptedData(0x25, 0x00);
            byte up2 = LogDecryptedData(0x25, 0x01);
            figure.upgrades = FormatBytes((byte)up1, (byte)up2);

            // GET HAT ID // - 0x25 (0x04 + 0x05)
            int hat = BitConverter.ToUInt16(new byte[] { LogDecryptedData(0x25, 0x04), LogDecryptedData(0x25, 0x05) }, 0);
            figure.hat = hat;
            print("HAT ID: " + hat);

            // GET NICKNAME ONE + TWO // - 0x28 (0x00 +/+ 0x0F)
            byte[] nickBytes =
            {
                LogDecryptedData(0x26, 0x00), LogDecryptedData(0x26, 0x01),
                LogDecryptedData(0x26, 0x02), LogDecryptedData(0x26, 0x03),
                LogDecryptedData(0x26, 0x04), LogDecryptedData(0x26, 0x05),
                LogDecryptedData(0x26, 0x06), LogDecryptedData(0x26, 0x07),
                LogDecryptedData(0x26, 0x08), LogDecryptedData(0x26, 0x09),
                LogDecryptedData(0x26, 0x0A), LogDecryptedData(0x26, 0x0B),
                LogDecryptedData(0x26, 0x0C), LogDecryptedData(0x26, 0x0D),
                LogDecryptedData(0x26, 0x0E), LogDecryptedData(0x26, 0x0F),
                LogDecryptedData(0x28, 0x00), LogDecryptedData(0x28, 0x01),
                LogDecryptedData(0x28, 0x02), LogDecryptedData(0x28, 0x03),
                LogDecryptedData(0x28, 0x04), LogDecryptedData(0x28, 0x05),
                LogDecryptedData(0x28, 0x06), LogDecryptedData(0x28, 0x07),
                LogDecryptedData(0x28, 0x08), LogDecryptedData(0x28, 0x09),
                LogDecryptedData(0x28, 0x0A), LogDecryptedData(0x28, 0x0B),
                LogDecryptedData(0x28, 0x0C), LogDecryptedData(0x28, 0x0D),
                LogDecryptedData(0x28, 0x0E), LogDecryptedData(0x28, 0x0F)
            };

            string nickname = ConvertBytesToString(nickBytes);
            figure.nickname = nickname;
            print("NICKNAME: " + nickname);
        }

        print("ID: " + figure.id);
        print("VID: " + figure.vid);

        hud.health = figure.skylander.MAX_HEALTH;

        portal.placeSkylander = false;

        string fullData = "";

        for (int i = 0; i < 8; i++)
        {
            for (int z = 0; z < figure.data[i].Length; z++)
            {
                fullData += figure.data[i][z].ToString("X2") + " ";
            }
            fullData += "\n";
        }

        for (int i = 8; i < figure.data.Length; i++)
        {
            for (int z = 0; z < figure.data[i].Length; z++)
            {
                fullData += LogDecryptedData((byte)i, (byte)z).ToString("X2") + " ";
            }
            fullData += "\n";
        }

        print(fullData);
    }

    public string FormatBytes(byte byte1, byte byte2)
    {
        string formattedByte1 = Convert.ToString(byte1, 2).PadLeft(8, '0');
        string formattedByte2 = Convert.ToString(byte2, 2).PadLeft(8, '0');
        return $"{formattedByte1}:{formattedByte2}";
    }

    public void ResetData()
    {
        figure = new Figure();
        toyID = 0xFF;

        figure.data = new byte[0x40][];
        figure.decryptedData = new byte[1024];
        figure.finalData = new byte[0x40][];

        for (int i = 0; i < figure.data.Length; i++)
        {
            figure.data[i] = new byte[0x10];
            figure.finalData[i] = new byte[0x10];
        }

        figure.skylander = null;
    }

    public byte LogDecryptedData(byte block, byte location)
    {
        DecryptBuffer(figure.data);

        //int newLocation = (block * 64) + location;

        return figure.finalData[block][location];
    }

    public void DecryptBuffer(byte[][] b)
    {
        byte[] buffer = TransformBuffer(b);

        uint blockIndex;
        byte[] blockData;
        byte[] tagBlocks0and1;

        tagBlocks0and1 = buffer;
        for (blockIndex = 0x08; blockIndex < 0x40; blockIndex++)
        {
            blockData = buffer[(int)(blockIndex * 0x10)..((int)(blockIndex * 0x10) + 16)];
            Crypt.DecryptTagBlock(blockData, blockIndex, tagBlocks0and1);
            Buffer.BlockCopy(blockData, 0, figure.decryptedData, (int)(blockIndex * 0x10), 16);
        }

        figure.finalData = new byte[64][];
        for (int z = 0; z < 64; z++)
        {
            figure.finalData[z] = new byte[16];
        }

        int c = 0;
        for (int i = 0; i < 64; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                figure.finalData[i][j] = figure.decryptedData[c];
                c++;
            }
        }
    }

    public byte[] TransformBuffer(byte[][] b)
    {
        byte[] buffer = new byte[1024];
        int c = 0;

        for (int i = 0; i < b.Length; i++)
        {
            for (int j = 0; j < b[i].Length; j++)
            {
                buffer[c] = b[i][j];
                c++;
            }
        }

        return buffer;
    }

    public string ConvertBytesToString(byte[] input)
    {
        // NAME ENDS WHEN BYTE = 0x00 0x00 //
        string output = "";

        for (int i = 0; i < input.Length - 1; i += 2)
        {
            if (input[i] == 0x00 && input[i + 1] == 0x00)
                break;

            char character = BitConverter.ToChar(input, i);
            output += character;
        }

        return output;
    }
}
