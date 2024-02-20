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
    public Color portalColor;

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

            byte[] data = new byte[33];
            data[0] = 0x00;
            data[1] = Convert.ToByte('J');
            data[2] = 0x00;
            data[3] = (byte)(portalColor.r * 255f);
            data[4] = (byte)(portalColor.g * 255f);
            data[5] = (byte)(portalColor.b * 255f);
            data[6] = 0x00;
            data[7] = 0x00;

            print(data[4]);

            var command = PortalCommand.Create(data);

            // Send the command to the device.
            print(portalDevice.ExecuteCommand(ref command));
            StartCoroutine(UpdatePortal());
        }

        
    }
}
