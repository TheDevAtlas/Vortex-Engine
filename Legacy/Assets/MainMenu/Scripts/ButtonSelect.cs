using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;
using System;
using Unity.VisualScripting;

public class ButtonSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Button button;
    public TextMeshProUGUI buttonText;
    public Color unselected;
    public Color selected;
    public GameObject selectedAnimation;
    public RawImage background;
    public Texture image;
    public MainMenuAudioManager audioManager;

    private bool isSelected = false;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
        UpdateButtonColor();
        selectedAnimation.SetActive(true);
        if(image != null)
        {
            background.texture = image;
            float aspect = (float)image.width / (float)image.height;
            background.gameObject.GetComponent<AspectRatioFitter>().aspectRatio = aspect;
        }
        audioManager.PlaySelect();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        UpdateButtonColor();
        selectedAnimation.SetActive(false);
    }

    private void UpdateButtonColor()
    {
        buttonText.color = isSelected ? selected : unselected;
    }
}
