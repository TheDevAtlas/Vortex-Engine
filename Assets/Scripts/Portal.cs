using System;
using System.Collections;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.InputSystem.LowLevel.InputEventTrace;

public class Portal
{
    public string path;

    PortalConnection portalConnection;

    public bool isActive;

    public void Initialize()
    {
        portalConnection = new PortalConnection(path);
    }

    public void ReadyCommand()
    {
        byte[] input = new byte[0x21];
        input[1] = Convert.ToByte('R');

        byte[] output;

        do
        {
            portalConnection.Write(input);

            output = portalConnection.Read();
        } while (HandleResponse(output) != Convert.ToByte('R'));

        Debug.Log("Portal is Ready - " + path);
        Debug.Log(output[1] + " - " + output[2]);
    }

    public void ActivateCommand()
    {
        byte[] input = new byte[0x21];
        input[1] = Convert.ToByte('A');
        input[2] = 0x01;

        byte[] output;

        do
        {
            portalConnection.Write(input);

            output = portalConnection.Read();
        } while (HandleResponse(output) != Convert.ToByte('A') && output[1] == 1);

        Debug.Log("Portal is Active - " + path);
        Debug.Log(output[1] + " - " + output[2]);
    }

    public void StatusCommand()
    {
        byte[] input = new byte[0x21];
        input[1] = Convert.ToByte('S');

        byte[] output;

        do
        {
            portalConnection.Write(input);

            output = portalConnection.Read();

            // we do not handle status here - in HandleResponse//
        } while (HandleResponse(output) != Convert.ToByte('S'));
    }

    public void ColorCommand(byte r, byte g, byte b)
    {
        byte[] input = new byte[0x21];

        input[1] = Convert.ToByte('C');
        input[2] = r;
        input[3] = g;
        input[4] = b;

        portalConnection.Write(input);
    }

    public void Query(byte characterIndex, byte block)
    {
        byte[] input = new byte[0x21];

        input[1] = Convert.ToByte('Q');
        input[2] = characterIndex;
        input[3] = block;

        portalConnection.Write(input);
    }

    public byte HandleResponse(byte[] input)
    {
        if(input.Length != 0x20)
        {
            throw new Exception();
        }

        if (input[0] == Convert.ToByte('S'))
        {
            byte[] figureStatusData = input.Skip(1).Take(4).ToArray();
            int figureStatusInt = BitConverter.ToInt32(figureStatusData);

            var figureStatus = new BitArray(new int[] { figureStatusInt });

            for (int i = figureStatus.Length - 1; i >= 1; i-=2)
            {
                bool present = figureStatus.Get(i - 1);
                bool changed = figureStatus.Get(i);

                if (changed)
                {
                    Debug.Log("Changed");
                }
            }
        }

        return input[0];
    }
}
