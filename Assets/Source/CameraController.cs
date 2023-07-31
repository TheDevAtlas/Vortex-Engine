using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PortalController portal;

    public Camera MainCamera;
    public Camera VortexCamera;

    public float transitionDuration = 2f; // Time taken for the transition

    private float transitionTimer = 0f;
    private float initialFOV = 60f; // Starting FOV of the active camera
    private float targetFOV = 180f;  // Target FOV for the transition

    public GameObject join;
    public GameObject place;
    public GameObject reading;

    private void Awake()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        VortexCamera = GameObject.FindGameObjectWithTag("VortexCamera").GetComponent<Camera>();

        MainCamera.gameObject.SetActive(true);
        VortexCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (portal.placeSkylander)
        {
            MainCamera.gameObject.SetActive(false);
            VortexCamera.gameObject.SetActive(true);
            portal.playerUI.SetActive(false);

            if (portal.players.Count == 0)
            {
                join.SetActive(true);
                place.SetActive(false);
                reading.SetActive(false);
            } else if (portal.isReading)
            {
                join.SetActive(false);
                place.SetActive(false);
                reading.SetActive(true);
            }
            else
            {
                join.SetActive(false);
                place.SetActive(true);
                reading.SetActive(false);
            }
        }
        else 
        {
            MainCamera.gameObject.SetActive(true);
            VortexCamera.gameObject.SetActive(false);
            portal.playerUI.SetActive(true);
        }
    }
}
