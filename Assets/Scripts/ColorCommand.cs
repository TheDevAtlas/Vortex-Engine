using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Windows;

public class ColorCommand : MonoBehaviour
{
    private PortalDevice portalDevice;
    public Color portalColor;
    public float portalColorDelay;

    public bool isActivated;
    public bool isReady;
    public byte testValue;

    private void Awake()
    {
        // Attempt to find an existing PortalDevice.
        // portalDevice = InputSystem.devices.FirstOrDefault(x => x is PortalDevice) as PortalDevice;
        portalDevice = (PortalDevice)GetComponent<PlayerInput>().devices[0];
        StartCoroutine(ActivatePortal());
    }

    private void Update()
    {
        print(Convert.ToChar((int)(portalDevice._01Byte.magnitude * 255f)));
    }

    IEnumerator ActivatePortal()
    {
        if(isActivated)
        {
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(ReadyPortal());
        }
        else
        {
            //portalDevice = (PortalDevice)GetComponent<PlayerInput>().devices[0];

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
            yield return new WaitForSeconds(0.01f);

            
            StartCoroutine(ActivatePortal());
        }
    }

    IEnumerator ReadyPortal()
    {
        if (isReady)
        {
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(UpdatePortal());
        }
        else
        {
            //portalDevice = (PortalDevice)GetComponent<PlayerInput>().devices[0];

            byte[] data = new byte[33];
            data[0] = 0x00;
            data[1] = Convert.ToByte('R');
            data[2] = 0x01;

            var command = PortalCommand.Create(data);

            // Send the command to the device.
            print(portalDevice.ExecuteCommand(ref command));

            if (Convert.ToChar((int)(portalDevice._01Byte.magnitude * 255f)) == Convert.ToByte('R'))
            {
                isReady = true;
            }

            yield return new WaitForSeconds(0.01f);
            StartCoroutine(ReadyPortal());
        }
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

            //portalDevice = (PortalDevice)GetComponent<PlayerInput>().devices[0];

            byte[] data = new byte[33];
            /*            data[0] = 0x00;
                        data[1] = Convert.ToByte('J');
                        data[2] = 0x00;
                        data[3] = (byte)(portalColor.r * 255f);
                        data[4] = (byte)(portalColor.g * 255f);
                        data[5] = (byte)(portalColor.b * 255f);
                        data[6] = (byte)Mathf.Clamp(portalColorDelay,0f,255f);
                        data[7] = 0x00;*/

            data = new byte[33];
            data[0] = 0x00;
            data[1] = Convert.ToByte('C');
            data[2] = (byte)(portalColor.r * 255f);
            data[3] = (byte)(portalColor.g * 255f);
            data[4] = (byte)(portalColor.b * 255f);

            var command = PortalCommand.Create(data);
            

            // Send the command to the device.
            print(portalDevice.ExecuteCommand(ref command));
            StartCoroutine(UpdatePortal());
        }

        
    }
}
