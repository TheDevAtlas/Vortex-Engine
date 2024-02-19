using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PortalSide : byte
{
    RIGHT = 0x00,
    BOTH = 0x01,
    LEFT = 0x02,
}

public class PortalController : MonoBehaviour
{
    // A Public Portal That Can Be Accessed By Any Script //
    public static PortalConnection portal;

    // Portal Attributes //
    public float sampleRate = 50f; // Sample Rate - Reduces Load On Computer At The Cost Of Responsiveness //
    public float querySampleRate = 100f;
    public bool isReading = false; // Reading State - Stops Other Functions From Interupting A Read Sequence //
    public bool placeSkylander = true;

    // Portal Color //
    public Color portalColor;
    Color lastPortalColor;

    // Character Indexes - What Figure Is On The Portal //
    public List<byte> characterIndexes = new List<byte>();

    // Players In The Game //
    public List<PlayerController> players = new List<PlayerController>();

    // Player HUDs //
    public PlayerHUD[] huds;
    public GameObject playerUI;

    // Attempt To Create A New Portal Connection On Start //
    void Start()
    {
        print(Convert.ToByte('R').ToString("X2"));
        StartCoroutine(StartPortal());
    }

    // Change The Portal Color As Needed //
    private void Update()
    {
        if(portalColor != lastPortalColor)
        {
            lastPortalColor = portalColor;
            SetColor((byte)(portalColor.r * 255f), (byte)(portalColor.g * 255f), (byte)(portalColor.b * 255f));
        }
    }

    // Create A New Portal - If No Portal - Keep Trying //
    public IEnumerator StartPortal()
    {
        // New Portal Connection //
        portal = new PortalConnection();

        // Check If Portal Exists //
        if(portal.devicePtr == IntPtr.Zero)
        {
            // Portal Does Not Exist //
            print("Please Connect Your Portal Of Power");

            yield return new WaitForSecondsRealtime(1f / sampleRate);

            // Do It Again //
            StartCoroutine(StartPortal());
        }
        else
        {
            // Portal Exists //
            print("Portal Connected");

            yield return new WaitForSecondsRealtime(1f / sampleRate);

            // Attempt To Ready The Portal //
            StartCoroutine(ReadyPortal());
        }
    }

    public IEnumerator ReadyPortal()
    {
        // Send The Ready Portal Command //
        Ready();

        if (portal.Read()[0] == Convert.ToByte('R'))
        {
            // Portal Is Ready //
            print("Portal Ready");

            yield return null;

            // Attempt To Activate The Portal //
            StartCoroutine(ActivatePortal());
        }
        else
        {
            // Portal Is Not Ready //

            yield return new WaitForSeconds(1f / sampleRate);

            // Do It Again //
            StartCoroutine(ReadyPortal());
        }
    }

    public IEnumerator ActivatePortal()
    {
        // Send The Activate Portal Command //
        Activate();

        if (portal.Read()[0] == Convert.ToByte('A'))
        {
            // Portal Is Activated //
            print("Portal Activated");

            yield return null;

            // Start Getting Data From Portal //
            StartCoroutine(GetStatus());
        }
        else
        {
            // Portal Is Not Activated //

            yield return new WaitForSeconds(1f / sampleRate);

            // Do It Again //
            StartCoroutine(ActivatePortal());
        }
    }

    public IEnumerator GetStatus()
    {
        if(players.Count == 0)
        {
            // No Players Are In The Game //
            print("Press The Join Button");

            yield return new WaitForSecondsRealtime(1f / sampleRate);

            // Get Another Update //
            StartCoroutine(GetStatus());
        }
        else if (isReading)
        {
            // Portal Is Currently Doing Something //
            print("Portal Is Reading");

            yield return new WaitForSecondsRealtime(1f / sampleRate);

            // Get Another Update //
            StartCoroutine(GetStatus());
        }
        else
        {
            Status();

            byte[] output = portal.Read();

            if (output[0] == Convert.ToByte('S'))
            {
                string printOut = "";
                for(int i = 0; i < 5; i++)
                {
                    printOut += output[i].ToString("X2") + " ";
                }
                print(printOut);

                var bits = new BitArray(new int[] { output[1] + (output[2] << 8) + (output[3] << 16) + (output[4] << 24) });

                List<byte> result = new List<byte>();
                for (int i = 0; i < bits.Length; i += 2)
                {
                    if (bits.Get(i))
                    {
                        result.Add(Convert.ToByte(i / 2));
                    }
                }

                // Update the characterIndexes list with the current result list
                characterIndexes = result;

                // Find the toys that were removed from the list
                List<byte> removedToys = new List<byte>();
                for (int i = 0; i < players.Count; i++)
                {
                    if (!characterIndexes.Contains(players[i].toyID))
                    {
                        players[i].toyID = 0xFF;
                        players[i].ResetData();
                    }
                }

                // Find unused toys
                List<byte> unusedToys = new List<byte>();
                for (int i = 0; i < result.Count; i++)
                {
                    bool hasPlayer = false;
                    for (int j = 0; j < players.Count; j++)
                    {
                        if (result[i] == players[j].toyID)
                        {
                            print("Has A Player");
                            hasPlayer = true;
                        }
                    }
                    if (!hasPlayer)
                    {
                        unusedToys.Add(result[i]);
                    }
                }

                // Assign unused toys to players with toyID 0xFF
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].toyID == 0xFF)
                    {
                        if(unusedToys.Count != 0)
                        {
                            players[i].toyID = unusedToys[0];
                            StartCoroutine(QueryPortal(players[i].toyID, players[i], 0));
                            isReading = true;
                            unusedToys.RemoveAt(0);
                            break;
                        }
                        else
                        {
                            placeSkylander = true;
                            print("Please Place A Skylander On The Portal Of Power");
                        }
                    }
                }



                /*
                        

                            if (!skyTaken)
                            {
                                players[i].figure.toyID = characterIndexes[k];
                                hasSky = true;
                                index = characterIndexes[k];
                                break;
                            }
                        }
                        if (!hasSky)
                        {
                            print("Please Place A Skylander On The Portal Of Power");
                        }
                        else
                        {
                            // characterIndexes[index], players[i], 0
                            StartCoroutine(QueryPortal(characterIndexes[index], players[i], 0));
                            isReading = true;
                            break;
                        }
                    }
                }*/

                yield return new WaitForSecondsRealtime(1f / sampleRate);
                StartCoroutine(GetStatus());
            }
            else
            {
                yield return new WaitForSecondsRealtime(1f / sampleRate);
                StartCoroutine(GetStatus());
            }
        }
    }

    public IEnumerator QueryPortal(byte characterIndex, PlayerController pc, int currentBlockIndex)
    {
        byte block = pc.ReadCommands[currentBlockIndex];

        Query(characterIndex, block);

        byte[] output = portal.Read();

        if ((output[0] != Convert.ToByte('Q') || (output[1] != characterIndex && output[1] != characterIndex + 0x10) || output[2] != block))
        {
            yield return new WaitForSecondsRealtime(1f / querySampleRate);
            StartCoroutine(QueryPortal(characterIndex, pc, currentBlockIndex));
        }
        else
        {
            print(output[0]);
            Array.Copy(output, 3, pc.figure.data[block], 0, 16);
            print("Block Done: " + block);

            if(currentBlockIndex < pc.ReadCommands.Length - 1)
            {
                yield return new WaitForSecondsRealtime(1f / querySampleRate);
                StartCoroutine(QueryPortal(characterIndex, pc, currentBlockIndex + 1));
            }
            else
            {
                isReading = false;

                pc.SubmitData();

                yield return new WaitForSecondsRealtime(1f / querySampleRate);
            }
            
        }
    }

    public void SetColor(byte r, byte g, byte b)
    {
        byte[] colorCommand = new byte[0x21];
        colorCommand[1] = Convert.ToByte('C');
        colorCommand[2] = r;
        colorCommand[3] = g;
        colorCommand[4] = b;

        StartCoroutine(portal.Write(colorCommand));
    }

    public void Ready()
    {
        byte[] readyCommand = new byte[0x21];
        readyCommand[1] = Convert.ToByte('R');
        StartCoroutine(portal.Write(readyCommand));
    }

    public void Activate()
    {
        byte[] activationCommand = new byte[0x21];
        activationCommand[1] = Convert.ToByte('A');
        activationCommand[2] = 0x01;
        StartCoroutine(portal.Write(activationCommand));
    }

    public void Status()
    {
        byte[] statusCommand = new byte[0x21];
        statusCommand[1] = Convert.ToByte('S');
        StartCoroutine(portal.Write(statusCommand));
    }

    public void Query(byte characterIndex, byte block)
    {
        byte[] queryCommand = new byte[0x21];

        queryCommand[1] = Convert.ToByte('Q');
        queryCommand[2] = characterIndex;
        queryCommand[3] = block;

        StartCoroutine(portal.Write(queryCommand));
    }

    public void ButtonStatus()
    {
        StartCoroutine(GetStatus());
    }

    public PlayerHUD AddPlayer(PlayerController player)
    {
        players.Add(player);
        huds[players.Count - 1].gameObject.SetActive(true);
        huds[players.Count - 1].pc = player;
        player.playerID = players.Count;
        return huds[players.Count - 1];
    }
}
