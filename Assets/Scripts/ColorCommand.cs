using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColorCommand : MonoBehaviour
{
    private PortalDevice portalDevice;
    public Color portalColor;
    public float portalColorDelay;

    public bool isActivated;
    public bool isReady;

    public float pollingRate;

    public GameObject colorPickerObject;
    public Transform layoutObject;

    private void Awake()
    {
        // Attempt to find an existing PortalDevice.
        // portalDevice = InputSystem.devices.FirstOrDefault(x => x is PortalDevice) as PortalDevice;
        portalDevice = (PortalDevice)GetComponent<PlayerInput>().devices[0];
        StartCoroutine(ActivatePortal());

        layoutObject = GameObject.FindGameObjectWithTag("Respawn").transform;

        colorPickerObject = Instantiate(colorPickerObject);
        colorPickerObject.transform.SetParent(layoutObject,false);
    }

    private void Update()
    {
        print(Convert.ToChar((int)(portalDevice._01Byte.magnitude * 255f)));
    }

    IEnumerator ActivatePortal()
    {
        if(isActivated)
        {
            yield return new WaitForSeconds(1f/pollingRate);
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
            yield return new WaitForSeconds(1f/pollingRate);

            
            StartCoroutine(ActivatePortal());
        }
    }

    IEnumerator ReadyPortal()
    {
        if (isReady)
        {
            yield return new WaitForSeconds(1f/pollingRate);
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

            yield return new WaitForSeconds(1f/pollingRate);
            StartCoroutine(ReadyPortal());
        }
    }

    // Update is called once per frame
    IEnumerator UpdatePortal()
    {
        if (portalDevice == null)
        {
            Debug.LogWarning("PortalDevice not found.");
            yield return new WaitForSeconds(1f/pollingRate);
            StartCoroutine(UpdatePortal());
        }
        else
        {
            yield return new WaitForSeconds(1f/pollingRate);

            Color c = colorPickerObject.GetComponent<FlexibleColorPicker>().GetColor();

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
            data[2] = (byte)(c.r * 255f);
            data[3] = (byte)(c.g * 255f);
            data[4] = (byte)(c.b * 255f);

            var command = PortalCommand.Create(data);
            

            // Send the command to the device.
            print(portalDevice.ExecuteCommand(ref command));
            StartCoroutine(UpdatePortal());
        }

        
    }
}
