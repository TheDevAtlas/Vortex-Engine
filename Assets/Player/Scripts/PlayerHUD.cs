using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public PlayerController pc;

    [Header("Health Bar")]
    public float health;
    float lerpTimer;
    float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;

    [Header("XP Bar")]
    public float xp;
    float lerpTimerXP;
    float maxXP = 100f;
    public float chipSpeedXP = 2f;
    public Image frontXPBar;
    public Image backXPBar;

    [Header("Text Boxes")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI healthTextBack;
    public TextMeshProUGUI nametagText;
    public TextMeshProUGUI nameTagTextBack;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI moneyText;

    [Header("Sprites")]
    public RawImage skylanderIcon;
    public RawImage skylanderElement;
    public Texture[] elements;

    void Start()
    {
        //maxHealth = pc.playerMaxHealth;

        health = maxHealth;
        xp = maxXP;
    }

    void Update()
    {
        if(pc.figure.skylander != null)
        {
            nametagText.text = pc.figure.skylander.name.ToString();
            nameTagTextBack.text = pc.figure.skylander.name.ToString();
            xpText.text = ((int)((float)pc.figure.xp / 1000f)).ToString();
            moneyText.text = pc.figure.coin.ToString();
            maxHealth = pc.figure.skylander.MAX_HEALTH;

            health = Mathf.Clamp(health, 0, maxHealth);
            xp = Mathf.Clamp(xp, 0, maxXP);
            UpdateHealthXPUI();

            skylanderIcon.texture = pc.figure.skylander.FIGURE_SPRITE;
            skylanderElement.texture = GetElementTexture(pc.figure.skylander.ELEMENT);
        }
    }

    void UpdateHealthXPUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        if(fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }

        if(fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);
        }

        healthText.text = health.ToString();
        healthTextBack.text = health.ToString();

        float fillFXP = frontXPBar.fillAmount;
        float fillBXP = backXPBar.fillAmount;
        float hFractionXP = xp / maxXP;

        if (fillBXP > hFractionXP)
        {
            frontXPBar.fillAmount = hFractionXP;
            backXPBar.color = Color.red;
            lerpTimerXP += Time.deltaTime;
            float percentComplete = lerpTimerXP / chipSpeedXP;
            percentComplete *= percentComplete;
            backXPBar.fillAmount = Mathf.Lerp(fillBXP, hFractionXP, percentComplete);
        }

        if (fillFXP < hFractionXP)
        {
            backXPBar.fillAmount = hFractionXP;
            backXPBar.color = Color.green;
            lerpTimerXP += Time.deltaTime;
            float percentComplete = lerpTimerXP / chipSpeedXP;
            percentComplete *= percentComplete;
            frontXPBar.fillAmount = Mathf.Lerp(fillFXP, hFractionXP, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
    }

    public void RestoreHealth(float heal)
    {
        health += heal;
        lerpTimer = 0f;
    }

    public void XPDamage(float damage)
    {
        xp -= damage;
        lerpTimerXP = 0f;
    }

    public void RestoreXP(float heal)
    {
        xp += heal;
        lerpTimerXP = 0f;
    }

    public Texture GetElementTexture(Element e)
    {
        if(e == Element.Air)
        {
            return elements[0];
        }

        if (e == Element.Dark)
        {
            return elements[1];
        }

        if (e == Element.Earth)
        {
            return elements[2];
        }

        if (e == Element.Fire)
        {
            return elements[3];
        }

        if (e == Element.Life)
        {
            return elements[4];
        }

        if (e == Element.Light)
        {
            return elements[5];
        }

        if (e == Element.Magic)
        {
            return elements[6];
        }

        if (e == Element.Tech)
        {
            return elements[7];
        }

        if (e == Element.Undead)
        {
            return elements[8];
        }

        if (e == Element.Water)
        {
            return elements[9];
        }

        return null;
    }
}
