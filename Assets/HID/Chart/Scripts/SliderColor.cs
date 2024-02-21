using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderColor : MonoBehaviour
{
    public GameObject sliderHandle;
    public Color color = new Color();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float value = this.GetComponent<Slider>().value;
        sliderHandle.GetComponent<Image>().color = Color.HSVToRGB(value, 1, 1);
        color = sliderHandle.GetComponent<Image>().color;
    }
}
