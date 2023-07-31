using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void Exit()
    {
        Invoke("ExitAfterTime", 0.5f);
    }

    void ExitAfterTime()
    {
        Application.Quit();
        print("Quit");
    }
}
