using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using System;


public class Control : MonoBehaviour
{
 
    public Slider sliderColor;                  //The slider to control LEDs ring
    public Slider sliderSpeed;                  //The slider tor control the motor speed       
    public Toggle toggleControlStick;            //The button to switch the control mode
    public Text textTemp;                       //The text to display the temperature
    public Text textHumidity;                   //The text to display the humidity
    public Text textSpeed;                      //The text to display the speed of motor
    public Text textControlType;                //The text to display the mode of motor control
    public Image imageLEDRing;                  //The image to show the color of LEDs ring
    public GameObject chartSpeedData;           //The chart to display the data of motor speed
    public GameObject ringMotor;                //The image to show the speed
    public GameObject moveJoystick;             //The image to show the joystick move
    public GameObject pressJoystick;            //The image to show the leftstick press

    private ZTGameCtrl bleGamepad;              //currentGamepad        
    private uint timeCount = 0;                 //Time counter
    private int motorSpeed;                     //Motor speed
    private UInt32 outputData0;                 //The output data0 of HID outputreport
    private UInt32 outputData1;                 //The output data1 of HID outputreport
    private bool sendDataEnable = false;

    // Start is called before the first frame update
    void Start()
    {
        sliderSpeed.interactable = false;
        sendDataEnable = true;
        chartSpeedData.GetComponent<ChartDisplay>().AddSeries("Speed",Color.red,true);
    }

    void FixedUpdate()
    { 
        if(bleGamepad == null)
        {
            InputSystem.Update();

           //Findout the gamepad with "ZTGameCtrl"
            for (byte i = 0; i < Gamepad.all.Count; i++)
            {
                if (Gamepad.all[i].layout.Contains("ZTGameCtrl"))
                {
                    bleGamepad = (ZTGameCtrl)Gamepad.all[i];
                    break;
                }
            }

            if (bleGamepad == null) //No gamepad for "ZTGameCtrl"
                return;

        }

        if (timeCount == 0)//Timer 0.02S*5 = 0.1S
        {
            int data0 = bleGamepad.dData0.ReadValue();  //read the custom data from input report
            int data1 = bleGamepad.dData1.ReadValue();

            short temperature = (short)((((data0 >> 8) & 0xff)<<8) + ((data0) & 0xff) );
            textTemp.text = "T:" + (temperature/10f).ToString("F1") + "C";
            textHumidity.text = "H:" + ((data0 >> 16)&0xff) + "%";
            

            short speed = (short)((((data1 >> 8) & 0xff) << 8) + ((data1) & 0xff));
            motorSpeed = speed;
            chartSpeedData.GetComponent<ChartDisplay>().ChartAddPoints(0, speed);

            textSpeed.text = "S:" + speed.ToString();

            byte sliderSpeedValue = (byte)(sliderSpeed.value * 255);

            byte[] colorRGB = new byte[3];
            Color colorData = Color.HSVToRGB(sliderColor.value, 1, 1);

            colorRGB[0] = (byte)(colorData.r * 255);
            colorRGB[1] = (byte)(colorData.g * 255);
            colorRGB[2] = (byte)(colorData.b * 255);

            outputData0 = (UInt32)((colorRGB[0] << 0) + (colorRGB[1] << 8) + (colorRGB[2] << 16));
            if (sliderSpeedValue >= 127)
            {   //motor forward
                outputData1 = (UInt32)((sliderSpeed.value - 0.5f) * 511) + ((UInt32)0x01 << 8);
            }
            else if (sliderSpeedValue < 127)
            {   //motor reverse
                outputData1 = (UInt32)((0.5f - sliderSpeed.value) * 511);
            }

            if (sendDataEnable)
            {   //Set the output report data
                var command = MyControlCommand.Create(0x0A, (byte)(toggleControlStick.isOn ? 1 : 0), outputData0, outputData1);
                bleGamepad.device.ExecuteCommand(ref command);
                sendDataEnable = false;
            }
        }
             
        if (timeCount < 5)
            timeCount++;
        else
            timeCount = 0;
    }


    // Update is called once per frame
    void Update()
    {

        if (bleGamepad == null)
            return;

        ringMotor.transform.Rotate(motorSpeed * Vector3.forward * Time.deltaTime);

        //Left stick button pressed
        if (bleGamepad.leftStickButton.ReadValue() == 0)
            pressJoystick.SetActive(false);
        else
            pressJoystick.SetActive(true);

        //Show the joystick position
        Vector3 positionCentre = moveJoystick.transform.parent.transform.position;
        Vector3 moveVector = new Vector3(bleGamepad.leftStick.ReadValue().x * 120, bleGamepad.leftStick.ReadValue().y * 120, 0);
        moveJoystick.transform.position = positionCentre + moveVector;

    }

    //Motor speed control, joystick or slider
    public void SetSpeedControl()
    {
        textControlType.text = toggleControlStick.isOn ? "Speed Control Joystick" : "Speed Control Slider";
        
        if (!toggleControlStick.isOn)
            sliderSpeed.value = 0.5f;

        sliderSpeed.interactable = !toggleControlStick.isOn;
        sendDataEnable = true;
    }

    public void SetSpeed()
    {
        sendDataEnable = true;
    }

    public void SetLightColor()
    {
        Color colorData = Color.HSVToRGB(sliderColor.value, 1, 1);
        imageLEDRing.color = colorData;
        sendDataEnable = true;
    }

    public void SetSpeedZero()
    {
        sliderSpeed.value = 0.5f;
        sendDataEnable = true;
    }

}
