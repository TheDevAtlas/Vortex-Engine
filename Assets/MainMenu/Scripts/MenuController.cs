using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class MenuController : MonoBehaviour
{
    // Menu Game Objects //
    public GameObject mainMenu;
    public GameObject selectMenu;

    // Positions To Move The Menus Too //
    public Vector2 mainMenuTargetY;
    public Vector2 selectMenuTargetY;

    // Buttons To Set UI Manager Too When Change Screens //
    public Button startMenuButton;
    public Button selectStartButton;

    // Speed In Seconds To Transition //
    public float smoothedSpeed;

    // Control The Menu With States //
    bool switchMain;
    bool switchGame;

    // Background Images //
    public RawImage background;
    public RawImage background2;

    // Audio Manager For The Main Menu //
    public MainMenuAudioManager audioManager;

    public void SwitchToMain()
    {
        switchMain = true;
        selectMenu.SetActive(true);
        startMenuButton.interactable = false;
        selectStartButton.Select();
    }

    public void StartGame()
    {
        switchMain = false;
        switchGame = true;
    }

    private void FixedUpdate()
    {
        if (switchMain)
        {
            Vector2 desiredPosition = mainMenuTargetY;
            Vector2 smoothedPosition = Vector2.Lerp(mainMenu.GetComponent<RectTransform>().anchoredPosition, desiredPosition, Time.fixedDeltaTime / smoothedSpeed);
            mainMenu.GetComponent<RectTransform>().anchoredPosition = smoothedPosition;

            desiredPosition = selectMenuTargetY;
            smoothedPosition = Vector2.Lerp(selectMenu.GetComponent<RectTransform>().anchoredPosition, desiredPosition, Time.fixedDeltaTime / smoothedSpeed);
            selectMenu.GetComponent<RectTransform>().anchoredPosition = smoothedPosition;

            background.color = Color.Lerp(background.color, Color.clear, Time.fixedDeltaTime / smoothedSpeed);
        }

        if (switchGame)
        {
            Vector2 desiredPosition = mainMenuTargetY;
            Vector2 smoothedPosition = Vector2.Lerp(selectMenu.GetComponent<RectTransform>().anchoredPosition, desiredPosition, Time.fixedDeltaTime / smoothedSpeed);
            selectMenu.GetComponent<RectTransform>().anchoredPosition = smoothedPosition;

            background2.color = Color.Lerp(background2.color, Color.black, Time.fixedDeltaTime / smoothedSpeed);

            audioManager.musicSource.volume = Mathf.Lerp(audioManager.musicSource.volume, 0f, Time.fixedDeltaTime / smoothedSpeed);
        }
    }
}
