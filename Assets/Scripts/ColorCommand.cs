using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.LowLevel;

public class ColorCommand : MonoBehaviour
{
    private PortalDevice portalDevice;

    private void Awake()
    {
        // Attempt to find an existing PortalDevice.
        portalDevice = InputSystem.devices.FirstOrDefault(x => x is PortalDevice) as PortalDevice;

        StartCoroutine(UpdatePortal());
    }

    // Update is called once per frame
    IEnumerator UpdatePortal()
    {
        if (portalDevice == null)
        {
            Debug.LogWarning("PortalDevice not found.");
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(UpdatePortal());
        }
        else
        {
            yield return new WaitForSeconds(0.01f);

            portalDevice = (PortalDevice)GetComponent<PlayerInput>().devices[0];

            // Create and populate your command here.
            /*var command = new PortalCommand
            {
                // Populate your 32 bytes here.
                _00Byte = 0x00,
                _01Byte = Convert.ToByte('C'), // color command
                _02Byte = 0xFF, // red
                _03Byte = 0x00,
                _04Byte = 0x00,
                _05Byte = 0x00,
                _06Byte = 0x00,
                _07Byte = 0x00,
                _08Byte = 0x00,
                _09Byte = 0x00,
                _0AByte = 0x00,
                _0BByte = 0x00,
                _0CByte = 0x00,
                _0DByte = 0x00,
                _0EByte = 0x00,
                _0FByte = 0x00,

                _10Byte = 0x00,
                _11Byte = 0x00,
                _12Byte = 0x00,
                _13Byte = 0x00,
                _14Byte = 0x00,
                _15Byte = 0x00,
                _16Byte = 0x00,
                _17Byte = 0x00,
                _18Byte = 0x00,
                _19Byte = 0x00,
                _1AByte = 0x00,
                _1BByte = 0x00,
                _1CByte = 0x00,
                _1DByte = 0x00,
                _1EByte = 0x00,
                _1FByte = 0x00,
                _20Byte = 0x00,
            };*/

            byte[] data = new byte[33];
            data[0] = 0x00;
            data[1] = Convert.ToByte('J');
            data[2] = 0x00;
            data[3] = 0xFF;
            data[4] = 0x00;
            data[5] = 0x00;
            data[6] = 0x00;
            data[7] = 0x00;

            var command = PortalCommand.Create(data);

            // Send the command to the device.
            print(portalDevice._01Byte.magnitude);
            print(portalDevice.ExecuteCommand(ref command));
            StartCoroutine(UpdatePortal());
        }

        
    }

    private object WaitForSeconds(float v)
    {
        throw new NotImplementedException();
    }
}
