using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalReadWrite : MonoBehaviour
{
    private PortalDevice portalDevice;
    public Color portalColor;

    public bool isActivated;
    public bool isReady;
    public float pollingRate;

    public bool isActive;

    public GameObject readWriteUI;
    public Transform layoutObject;

    public TextMeshProUGUI textInput;
    public TextMeshProUGUI textOutput;

    void Awake()
    {
        // Get Device \\
        portalDevice = (PortalDevice)GetComponent<PlayerInput>().devices[0];
        StartCoroutine(ActivatePortal());

        // UI Setup \\
        layoutObject = GameObject.FindGameObjectWithTag("Respawn").transform;
        readWriteUI.transform.SetParent(layoutObject, false);
    }

    public void WriteToToy()
    {
        // Create Command To Write To Toy \\
        byte[] textBytes = Encoding.UTF8.GetBytes(textInput.text);

        byte[] data = new byte[33];
        data[0] = 0x00;
        data[1] = Convert.ToByte('W');
        data[2] = 0x01;
        data[3] = 0x00;

        for (int i = 0; i < textBytes.Length; i++)
        {
            data[4 + i] = textBytes[i];
        }

        var command = PortalCommand.Create(data);

        // Send command \\
        print("Write To Toy : " + portalDevice.ExecuteCommand(ref command));
    }

    public void ReadFromToy()
    {
        // Create command To Read From Toy \\
        byte[] data = new byte[33];
        data[0] = 0x00;
        data[1] = Convert.ToByte('Q');
        data[2] = 0x01; // first toy?
        data[3] = 0x00; // first block?
        var command = PortalCommand.Create(data);

        // Send Command \\
        print("Read From Toy : " + portalDevice.ExecuteCommand(ref command));

        // Check Response \\
        if (Convert.ToChar((int)(portalDevice._01Byte.magnitude * 255f)) == Convert.ToByte('Q'))
        {
            print("Reading Success, Getting Bytes");

            byte[] byteList = new byte[16];

            byteList[0] = (byte)(portalDevice._04Byte.magnitude * 255f);
            byteList[1] = (byte)(portalDevice._05Byte.magnitude * 255f);
            byteList[2] = (byte)(portalDevice._06Byte.magnitude * 255f);
            byteList[3] = (byte)(portalDevice._07Byte.magnitude * 255f);
            byteList[4] = (byte)(portalDevice._08Byte.magnitude * 255f);
            byteList[5] = (byte)(portalDevice._09Byte.magnitude * 255f);
            byteList[6] = (byte)(portalDevice._0AByte.magnitude * 255f);
            byteList[7] = (byte)(portalDevice._0BByte.magnitude * 255f);
            byteList[8] = (byte)(portalDevice._0CByte.magnitude * 255f);
            byteList[9] = (byte)(portalDevice._0DByte.magnitude * 255f);
            byteList[10] = (byte)(portalDevice._0EByte.magnitude * 255f);
            byteList[11] = (byte)(portalDevice._0FByte.magnitude * 255f);
            byteList[12] = (byte)(portalDevice._10Byte.magnitude * 255f);
            byteList[13] = (byte)(portalDevice._11Byte.magnitude * 255f);
            byteList[14] = (byte)(portalDevice._12Byte.magnitude * 255f);
            byteList[15] = (byte)(portalDevice._13Byte.magnitude * 255f);

            // Use UTF8 To Convert Byte To Text \\
            textOutput.text = Encoding.UTF8.GetString(byteList);

            // Output Data \\
            print(textOutput.text);
            print("Raw Bytes : " + string.Join(", ", byteList));
        }
    }

    // SAME AS COLOR COMMAND SCRIPT \\
    IEnumerator ActivatePortal()
    {
        if (isActivated)
        {
            yield return new WaitForSeconds(1f / pollingRate);
            StartCoroutine(ReadyPortal());
        }
        else
        {
            byte[] data = new byte[33];
            data[0] = 0x00;
            data[1] = Convert.ToByte('A');
            data[2] = 0x01;
            var command = PortalCommand.Create(data);

            // Send the command to the device.
            print(portalDevice.ExecuteCommand(ref command));

            if (Convert.ToChar((int)(portalDevice._01Byte.magnitude * 255f)) == Convert.ToByte('A'))
            {
                isActivated = true;
            }
            yield return new WaitForSeconds(1f / pollingRate);


            StartCoroutine(ActivatePortal());
        }
    }

    // SAME AS COLOR COMMAND SCRIPT \\
    IEnumerator ReadyPortal()
    {
        if (isReady)
        {
            yield return new WaitForSeconds(1f / pollingRate);
            StartCoroutine(ColorPortal());
        }
        else
        {
            byte[] data = new byte[33];
            data[0] = 0x00;
            data[1] = Convert.ToByte('R');
            data[2] = 0x01;

            var command = PortalCommand.Create(data);

            print(portalDevice.ExecuteCommand(ref command));

            if (Convert.ToChar((int)(portalDevice._01Byte.magnitude * 255f)) == Convert.ToByte('R'))
            {
                isReady = true;
            }

            yield return new WaitForSeconds(1f / pollingRate);
            StartCoroutine(ReadyPortal());
        }
    }

    // SAME AS COLOR COMMAND SCRIPT \\
    IEnumerator ColorPortal()
    {
        if (portalDevice == null || !isActive)
        {
            yield return new WaitForSeconds(1f / pollingRate);
            StartCoroutine(ColorPortal());
        }
        else
        {
            yield return new WaitForSeconds(1f / pollingRate);

            Color c = portalColor;

            byte[] data = new byte[33];
            data[0] = 0x00;
            data[1] = Convert.ToByte('C');
            data[2] = (byte)(c.r * 255f);
            data[3] = (byte)(c.g * 255f);
            data[4] = (byte)(c.b * 255f);

            var command = PortalCommand.Create(data);


            // Send the command to the device.
            print(portalDevice.ExecuteCommand(ref command));
            StartCoroutine(ColorPortal());
        }
    }
}
