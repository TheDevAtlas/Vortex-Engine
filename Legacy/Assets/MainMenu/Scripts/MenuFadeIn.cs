using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuFadeIn : MonoBehaviour
{
    public RawImage image;
    public Color targetColor = Color.clear;
    public float smooth;

    private Color initialColor;

    private void Start()
    {
        initialColor = image.color;
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < smooth)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / smooth);
            image.color = Color.Lerp(initialColor, targetColor, t);
            yield return null;
        }

        image.color = targetColor;
        if (Mathf.Approximately(targetColor.a, 0f))
        {
            Destroy(gameObject);
        }
    }
}
