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
        // Get The Portal Device From The Player Input Manager \\
        portalDevice = (PortalDevice)GetComponent<PlayerInput>().devices[0];

        // Activate The Portal / Starts The Polling Of The Portal \\
        StartCoroutine(ActivatePortal());

        // Set Up The UI Objects \\
        layoutObject = GameObject.FindGameObjectWithTag("Respawn").transform;
        colorPickerObject = Instantiate(colorPickerObject);
        colorPickerObject.transform.SetParent(layoutObject,false);
    }

    private void Update()
    {
        // Prints The Portals Current Command That It is Sending Back \\
        print(Convert.ToChar((int)(portalDevice._01Byte.magnitude * 255f)));
    }

    IEnumerator ActivatePortal()
    {
        // If Activated Move Onto Ready Command \\
        if(isActivated)
        {
            yield return new WaitForSeconds(1f/pollingRate);
            StartCoroutine(ReadyPortal());
        }
        else
        {
            // If Not Activated Create Activated Command \\

            byte[] data = new byte[33];
            data[0] = 0x00;
            data[1] = Convert.ToByte('A');
            data[2] = 0x01;
            var command = PortalCommand.Create(data);

            // Send the command to the device \\
            print(portalDevice.ExecuteCommand(ref command));

            // Check If Portal Returns \\
            if (Convert.ToChar((int)(portalDevice._01Byte.magnitude * 255f)) == Convert.ToByte('A'))
            {
                isActivated = true;
            }
            yield return new WaitForSeconds(1f/pollingRate);

            // Try Again \\
            StartCoroutine(ActivatePortal());
        }
    }

    IEnumerator ReadyPortal()
    {
        // If Ready Move Onto Update / Game Loop That You Create \\
        if (isReady)
        {
            yield return new WaitForSeconds(1f/pollingRate);
            StartCoroutine(UpdatePortal());
        }
        else
        {
            // If Not Ready Create Ready Command \\

            byte[] data = new byte[33];
            data[0] = 0x00;
            data[1] = Convert.ToByte('R');
            data[2] = 0x01;

            var command = PortalCommand.Create(data);

            // Send the command to the device \\
            print(portalDevice.ExecuteCommand(ref command));

            // Check Response \\
            if (Convert.ToChar((int)(portalDevice._01Byte.magnitude * 255f)) == Convert.ToByte('R'))
            {
                isReady = true;
            }

            yield return new WaitForSeconds(1f/pollingRate);

            // Try Again \\
            StartCoroutine(ReadyPortal());
        }
    }

    IEnumerator UpdatePortal()
    {
        // Error check for removal \\
        // TODO: Add Device Hotswap - Should Be Supported By Defualt, Just Destroy The Scripts / Player Using It \\
        if (portalDevice == null)
        {
            Debug.LogWarning("PortalDevice not found.");
            yield return new WaitForSeconds(1f/pollingRate);
            StartCoroutine(UpdatePortal());
        }
        else
        {
            yield return new WaitForSeconds(1f/pollingRate);

            // Get The Color User Wants \\
            Color c = colorPickerObject.GetComponent<FlexibleColorPicker>().GetColor();

            // J Command \\
            /*            data[0] = 0x00;
                        data[1] = Convert.ToByte('J');
                        data[2] = 0x00;
                        data[3] = (byte)(portalColor.r * 255f);
                        data[4] = (byte)(portalColor.g * 255f);
                        data[5] = (byte)(portalColor.b * 255f);
                        data[6] = (byte)Mathf.Clamp(portalColorDelay,0f,255f);
                        data[7] = 0x00;*/

            // Color Command \\
            byte[] data = new byte[33];
            data[0] = 0x00;
            data[1] = Convert.ToByte('C');
            data[2] = (byte)(c.r * 255f);
            data[3] = (byte)(c.g * 255f);
            data[4] = (byte)(c.b * 255f);

            var command = PortalCommand.Create(data);
            

            // Send the command to the device \\
            print(portalDevice.ExecuteCommand(ref command));
            StartCoroutine(UpdatePortal());
        }

        
    }
}
