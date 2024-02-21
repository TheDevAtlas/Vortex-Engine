using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System;

using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ChartDisplay : MonoBehaviour
{
    [System.Serializable]
    public class Resources  
    {
        public GameObject buttonPrefab;             //Config button gameObject prefab
        public float ButtonH = 45f;                 //Config button height
        public Color ButtonForeColor = Color.white; //Button text color
        public GameObject chartPointsPrefab;        //Chart points data gameObject prefab
    }

    [System.Serializable]
    public class ChartAX
    {
        public Text textAX_YMaxA;   //AX Y max display text A
        public Text textAX_YMinA;   //AX Y min display text A
        public Text textAX_YMaxB;   //AX Y max display text B
        public Text textAX_YMinB;   //AX Y min display text B
        public Text textAX_XMax;    //AX X display text
        public Text textAX_XMin;    //AX X display text
        public byte AXFontSize = 20;
    }

    [System.Serializable]
    public class Config
    {
        public UInt16 pointsMax = 500;          //The max count for the chart display
        public UInt32 chartXMax = 1000000;
        public float chartMargin = 20;          //Chart display area margin on the screen
        public float textGap = 5;
    }

    [System.Serializable]
    private class Chart
    {
        public Color color;         //Chart line color
        public float lineThickness; //Chart line points thickness
        public List<float> points;  //Chart line points data array
        public float YMax;          //Data array max for AX Y display
        public float YMin;          //Data array min for AX Y display
        public UInt32 chartX;       //Chart AX X data
        public bool AX_ANoB;        //Chart AX is A or B
        public GameObject dataUI;   //Chart UI gameObject
        public Button btnConfig;    //Chart UI config button
        public bool visible;
    }

    public ChartAX AX;       //Config data
    public Resources resources;     //Ext resources
    public Config config;



    private GameObject buttonArea;  //Config button display area
    private GameObject displayArea; //Chart display area
    private GameObject AX_UI;       //Chart AX display area
    private GameObject pointsUI;    //Chart line points display area
    private GameObject chartAXLine; //Chart AX line 

    private GameObject configArea;  //Config panel area
    private Slider sliderColor;     //Chart line color control slider
    private Slider sliderThickness; //Chart line thickness control slider
    private Toggle toggleVisible;   //Chart line visible control toggle

    private float chartAX_YMaxA = 0;//Chart points data Y max A
    private float chartAX_YMinA = 0;//Chart points data Y min A
    private float chartAX_YMaxB = 0;//Chart points data Y max B
    private float chartAX_YMinB = 0;//Chart points data Y min B
    private bool chartAX_Aset;
    private bool chartAX_Bset;
    private bool chartVisibleSet;

    private byte configIndex = 0;   //Current config chart index
    private List<Chart> chartData = new List<Chart>();//Chart lists

    public UInt16 pointsMax
    {
        set { config.pointsMax = value; }
        get { return config.pointsMax; }
    }
    public UInt32 chartXMax
    {
        set { config.chartXMax = value; }
        get { return config.chartXMax; }
    }

    public void Clear()
    {
        for(byte i=0; i< chartData.Count; i++)
        {   //Max AX X value, clear the chart data
            chartData[i].points.Clear();
            List<Vector2> points = new List<Vector2>();
            chartData[i].dataUI.GetComponent<UILineRenderer>().Points = points.ToArray();
            chartData[i].chartX = 0;
            chartData[i].YMax = 0;
            chartData[i].YMin = 0;
            configIndex = 0;
        }
        ChartAdjust();
    }

    private void Awake()
    {
        GetGameObjects();
    }
    // Start is called before the first frame update
    void Start()
    {  
        ChartAdjust();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (chartVisibleSet)
        {   //Config the lines, than update other lines AX and display
            for (byte i = 0; i < chartData.Count; i++)
            {
                if(chartData[i].AX_ANoB == chartData[configIndex].AX_ANoB)
                    chartLineUpdate(i);
                chartVisibleSet = false;
            }                    
        }     
    }

    //Get the chart AX Y max and min data of A and B
    private void GetAXMaxMin()
    {
        if (chartData.Count < 1)//No data 
            return;
        chartAX_Aset = false;
        chartAX_Bset = false; 
        for (byte i = 0; i < chartData.Count; i++)
        {
            if (chartData[i].AX_ANoB && (chartData[i].visible == true))
            {
                if (!chartAX_Aset)
                {   //Find the first type A visible line, and set the AX data
                    chartAX_YMaxA = chartData[i].YMax;
                    chartAX_YMinA = chartData[i].YMin;
                    chartAX_Aset = true;
                    AX.textAX_YMaxA.color = chartData[i].color;
                    AX.textAX_YMinA.color = chartData[i].color;
                }
                else
                {   //Find the AX max and min data
                    chartAX_YMaxA = chartAX_YMaxA > chartData[i].YMax ? chartAX_YMaxA : chartData[i].YMax;
                    chartAX_YMinA = chartAX_YMinA < chartData[i].YMin ? chartAX_YMinA : chartData[i].YMin;
                }
            }
            else if( (!chartData[i].AX_ANoB) && (chartData[i].visible == true))
            {
                if (!chartAX_Bset)
                {   //find the first type B visible line, and set the AX data
                    chartAX_YMaxB = chartData[i].YMax;
                    chartAX_YMinB = chartData[i].YMin;
                    chartAX_Bset = true;
                    AX.textAX_YMaxB.color = chartData[i].color;
                    AX.textAX_YMinB.color = chartData[i].color;
                }
                else
                {   //Find the AX max and min data
                    chartAX_YMaxB = chartAX_YMaxB > chartData[i].YMax ? chartAX_YMaxB : chartData[i].YMax;
                    chartAX_YMinB = chartAX_YMinB < chartData[i].YMin ? chartAX_YMinB : chartData[i].YMin;
                }
            }
        
        }
        //Set the AX text string 
        AX.textAX_YMaxA.text = chartAX_YMaxA.ToString();
        AX.textAX_YMinA.text = chartAX_YMinA.ToString();
        AX.textAX_YMaxB.text = chartAX_YMaxB.ToString();
        AX.textAX_YMinB.text = chartAX_YMinB.ToString();

        //AX.textAX_YMaxA.text = String.Format("{0, 8}", chartAX_YMaxA);
        //AX.textAX_YMinA.text = String.Format("{0, 8}", chartAX_YMinA);
    }

    //Line series add points data
    public void ChartAddPoints(byte chartIndex, float pointY)
    {
        if ((chartIndex + 1) > chartData.Count)//Index out of range
            return;

        if (chartData[chartIndex].points.Count >= config.pointsMax)
        {   //Max points display, remove first element
            chartData[chartIndex].points.RemoveAt(0);
        }
        chartData[chartIndex].points.Add(pointY);//List of points add data
        chartData[chartIndex].YMax = chartData[chartIndex].points.Max();
        chartData[chartIndex].YMin = chartData[chartIndex].points.Min();

        chartLineUpdate(chartIndex);

        AX.textAX_XMax.text = chartData[chartIndex].chartX.ToString();//Display the chart AX X data
        chartData[chartIndex].chartX++;

        if (chartData[chartIndex].chartX > config.chartXMax)
        {   //Max AX X value, clear the chart data
            chartData[chartIndex].points.Clear();
            chartData[chartIndex].chartX = 0;
        }
    }

    private void chartLineUpdate(byte chartIndex)
    {
        ChartAdjust();
        //Get the width and height of the points display area
        float PosW = pointsUI.GetComponent<RectTransform>().rect.width;
        float PosH = pointsUI.GetComponent<RectTransform>().rect.height;

        List<Vector2> points = new List<Vector2>();

        for (int i = 0; i < chartData[chartIndex].points.Count; i++)
        {
            
            if (chartData[chartIndex].chartX < config.pointsMax)
            {   //Chart AX X value is less than the points max length
                AX.textAX_XMin.text = "0";
                if (chartData[chartIndex].points.Count == 1)
                {   //Just one point in the line
                    if (chartData[chartIndex].AX_ANoB)//Chart AX Y is A
                    {
                        if (chartAX_YMaxA == chartAX_YMinA)//AX Y max = min
                            points.Add(new Vector2(0, 0.5f * PosH));//Point is in the middle at the left
                        else
                            points.Add(new Vector2(0, (chartData[chartIndex].points[i] - chartAX_YMinA) / (chartAX_YMaxA - chartAX_YMinA) * PosH));

                    }
                    else
                    {   //Chart AX Y is B
                        if (chartAX_YMaxB == chartAX_YMinB)
                            points.Add(new Vector2(0, 0.5f * PosH));
                        else
                            points.Add(new Vector2(0, (chartData[chartIndex].points[i] - chartAX_YMinB) / (chartAX_YMaxB - chartAX_YMinB) * PosH));
                    }
                }
                else
                {   //More than one point in the line
                    if (chartData[chartIndex].AX_ANoB)
                    {   //Chart AX Y is A
                        if (chartAX_YMaxA == chartAX_YMinA)//AX Y max = min
                            //Add the point X, i/count, i is from 0 to count, Y is in the middle
                            points.Add(new Vector2((((float)i / (float)(chartData[chartIndex].points.Count - 1)) * PosW), 0.5f * PosH));
                        else
                            points.Add(new Vector2((((float)i / (float)(chartData[chartIndex].points.Count - 1)) * PosW), (chartData[chartIndex].points[i] - chartAX_YMinA) / (chartAX_YMaxA - chartAX_YMinA) * PosH));
                    }
                    else
                    {   //Chart AX Y is B
                        if (chartAX_YMaxB == chartAX_YMinB)
                            points.Add(new Vector2((((float)i / (float)(chartData[chartIndex].points.Count - 1)) * PosW), 0.5f * PosH));
                        else
                            points.Add(new Vector2((((float)i / (float)(chartData[chartIndex].points.Count - 1)) * PosW), (chartData[chartIndex].points[i] - chartAX_YMinB) / (chartAX_YMaxB - chartAX_YMinB) * PosH));
                    }
                }
            }
            else
            {   //Chart AX X value is more than the points max length
                //Chart AX X position is from 0 to X max
                //Display the AX X Min value
                AX.textAX_XMin.text = (chartData[chartIndex].chartX - config.pointsMax).ToString();
                Debug.Log(chartData[chartIndex].chartX);
                if (chartData[chartIndex].AX_ANoB)
                {   //Chart AX Y is A
                    if (chartAX_YMaxA == chartAX_YMinA)
                        points.Add(new Vector2((((float)i / (float)config.pointsMax) * PosW), 0.5f * PosH));
                    else
                        points.Add(new Vector2((((float)i / (float)config.pointsMax) * PosW), (chartData[chartIndex].points[i] - chartAX_YMinA) / (chartAX_YMaxA - chartAX_YMinA) * PosH));
                }
                else
                {   //Chart AX Y is B
                    if (chartAX_YMaxB == chartAX_YMinB)
                        points.Add(new Vector2((((float)i / (float)config.pointsMax) * PosW), 0.5f * PosH));
                    else
                        points.Add(new Vector2((((float)i / (float)config.pointsMax) * PosW), (chartData[chartIndex].points[i] - chartAX_YMinB) / (chartAX_YMaxB - chartAX_YMinB) * PosH));
                }
            }
        }

        if ( chartIndex < points.Count)
        {   //Index is in the range of chart series
            chartData[chartIndex].dataUI.GetComponent<UILineRenderer>().Points = points.ToArray();
        }   
    }

    //Config panel display, get the parameter, input the button/chart index
    private void ConfigAreaDisplay(byte buttonIndex)
    {
        configArea.SetActive(true);
        configIndex = buttonIndex;//Save the configIndex

        Color colorButton = chartData[buttonIndex].color;//Get the char line color
        Vector3 colorHSV;
        //Get the HSV color data from RGB color data
        Color.RGBToHSV(colorButton, out colorHSV.x, out colorHSV.y, out colorHSV.z);
        //Set the color slider value
        sliderColor.value = colorHSV.x;
        //Set the thickness slider value
        sliderThickness.value = (chartData[buttonIndex].lineThickness - 1) / 20;
        //Set the chart line visible data
        toggleVisible.isOn = chartData[buttonIndex].dataUI.activeSelf;
    }

    private void ChartPointVisible()
    {
        if (buttonArea.activeSelf == false)
            return;
        //Debug.Log(toggleVisible.isOn.ToString());
        if (configIndex < chartData.Count)
        {
            chartData[configIndex].visible = toggleVisible.isOn;
            chartData[configIndex].dataUI.SetActive(toggleVisible.isOn);
            chartVisibleSet = true;          
        }         
    }

    private void ConfigColor()
    {
        if (buttonArea.activeSelf == false)
            return;
        
        if (configIndex < chartData.Count)
        {   //Get the color data from HSV to RGB
            Color color = Color.HSVToRGB(sliderColor.value, 1, 1);
            //Config the button backcolor
            chartData[configIndex].btnConfig.GetComponent<Image>().color = color;
            //Config the points line color
            chartData[configIndex].dataUI.GetComponent<UILineRenderer>().color = color;
            chartData[configIndex].color = color;
        }
    }


    private void ConfigThickness()
    {
        if (configIndex < chartData.Count)
        {
            chartData[configIndex].lineThickness = sliderThickness.value * 20 + 1;
            chartData[configIndex].dataUI.GetComponent<UILineRenderer>().LineThickness = chartData[configIndex].lineThickness;
        }          
    }

    private void ChartAdjust()
    {
        GetAXMaxMin();
        List<Vector2> pointTemp = new List<Vector2>();      
        //Get the width and height of the chart display area
        float chartPanelW = displayArea.GetComponent<RectTransform>().rect.width;
        float chartPanelH = displayArea.GetComponent<RectTransform>().rect.height;

        //Get the AX text size
        Vector2 textSize = new Vector2();

        //AX Y text length max size
        textSize.x = (AX.AXFontSize * AX.textAX_YMaxA.text.Length) / 2;    
        if ( ((AX.textAX_YMaxA.text.Length < AX.textAX_YMaxB.text.Length) && (chartAX_Bset==true)) || (chartAX_Aset == false))
            textSize.x = (AX.AXFontSize * AX.textAX_YMaxB.text.Length) / 2;
        textSize.y = AX.AXFontSize;
        //Set the AX Y max text size on the UI
        AX.textAX_YMaxA.gameObject.GetComponent<RectTransform>().sizeDelta = textSize;
        AX.textAX_YMaxB.gameObject.GetComponent<RectTransform>().sizeDelta = textSize;
       
        Vector2 textAXPosition = new Vector2();
        //Get the AX Y max text position
        textAXPosition.x = config.chartMargin + textSize.x / 2 - chartPanelW / 2;
        textAXPosition.y = chartPanelH / 2 - config.chartMargin - textSize.y / 2;
        //Set the AX Y max A text position
        AX.textAX_YMaxA.gameObject.GetComponent<RectTransform>().anchoredPosition = textAXPosition;
        //AX Y max A is set, B is under the A
        if (chartAX_Aset)
            textAXPosition.y -= textSize.y;
        AX.textAX_YMaxB.gameObject.GetComponent<RectTransform>().anchoredPosition = textAXPosition;

        //AX Y max A or B, visible config
        AX.textAX_YMaxA.gameObject.SetActive(chartAX_Aset);
        AX.textAX_YMaxB.gameObject.SetActive(chartAX_Bset);

        //Set the AX Y min text size on the UI
        AX.textAX_YMinA.gameObject.GetComponent<RectTransform>().sizeDelta = textSize;
        AX.textAX_YMinB.gameObject.GetComponent<RectTransform>().sizeDelta = textSize;

        //Set the AX Y min text position
        textAXPosition.y = config.chartMargin + textSize.y * 1.5f - chartPanelH / 2;
        AX.textAX_YMinA.gameObject.GetComponent<RectTransform>().anchoredPosition = textAXPosition;
        if (chartAX_Aset)
            textAXPosition.y += textSize.y;
        AX.textAX_YMinB.gameObject.GetComponent<RectTransform>().anchoredPosition = textAXPosition;

        AX.textAX_YMinA.gameObject.SetActive(chartAX_Aset);
        AX.textAX_YMinB.gameObject.SetActive(chartAX_Bset);

        //Get the AX line position and size
        pointTemp.Add(new Vector2(textSize.x + config.chartMargin + config.textGap, chartPanelH - config.chartMargin));
        pointTemp.Add(new Vector2(textSize.x + config.chartMargin + config.textGap, config.chartMargin + textSize.y + config.textGap));
        pointTemp.Add(new Vector2(chartPanelW - config.chartMargin, config.chartMargin + textSize.y + config.textGap));

        chartAXLine.GetComponentInChildren<UILineRenderer>().Points = pointTemp.ToArray();

        //Chart AX X max and min text
        textSize.x = (AX.AXFontSize * AX.textAX_XMax.text.Length) / 2;

        AX.textAX_XMax.gameObject.GetComponent<RectTransform>().sizeDelta = textSize;
        AX.textAX_XMax.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(chartPanelW / 2 - textSize.x / 2 - config.chartMargin, config.chartMargin + textSize.y / 2 - chartPanelH / 2);

        textSize.x = (AX.AXFontSize * AX.textAX_XMin.text.Length) / 2;

        AX.textAX_XMin.gameObject.GetComponent<RectTransform>().sizeDelta = textSize;
        AX.textAX_XMin.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(pointTemp[0].x + textSize.x / 2 - chartPanelW / 2, config.chartMargin + textSize.y / 2 - chartPanelH / 2);

        //Chart points line display area
        pointsUI.GetComponent<RectTransform>().offsetMax = new Vector2(chartPanelW / 2 - config.chartMargin, chartPanelH / 2 - config.chartMargin);
        pointsUI.GetComponent<RectTransform>().offsetMin = new Vector2(pointTemp[1].x - chartPanelW / 2, pointTemp[1].y - chartPanelH / 2);

        float W = pointsUI.GetComponent<RectTransform>().rect.width;
        float H = pointsUI.GetComponent<RectTransform>().rect.height;

        for (byte i = 0; i < chartData.Count; i++)
        {
            chartData[i].dataUI.GetComponent<RectTransform>().sizeDelta = new Vector2(W, H);
            chartData[i].dataUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            chartData[i].dataUI.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }

    }

    //Chart add series, input: chart name, chart color, chart AX is A or B
    public void AddSeries(string chartName, Color chartColor, bool AX_ANoB)
    {
        List<float> pointsData = new List<float>();
        Chart chart = new Chart();
        pointsData.Clear();
        chart.points = pointsData;
        chart.color = chartColor;
        chart.chartX = 0;
        chart.AX_ANoB = AX_ANoB;
        chart.visible = true;

        //Get the gameObject from the chart points prefab
        GameObject pointsLine = Instantiate(resources.chartPointsPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        pointsLine.GetComponent<UILineRenderer>().Points.ToList().Clear();
        //Set the points line name
        pointsLine.name = "PointLine" + (chartData.Count).ToString();
        //The gameObject is the child of "pointsUI"
        pointsLine.transform.SetParent(pointsUI.transform);
        pointsLine.GetComponent<UILineRenderer>().color = chartColor;
        chart.lineThickness = pointsLine.GetComponent<UILineRenderer>().LineThickness;
        chart.dataUI = pointsLine;
        chart.dataUI.SetActive(true);    
        chartData.Add(chart);
        //Add the config button of the chart line
        AddChartButton(chartName, chartColor, (byte)(chartData.Count - 1));
    }

    //Add config button
    public void AddChartButton(string chartName, Color ChartColor, byte buttonIndex)
    {
        GameObject ButtonChart = Instantiate(resources.buttonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        ButtonChart.name = "Button" + buttonIndex.ToString();
        //Debug.Log(ButtonChart.name);
        ButtonChart.transform.SetParent(buttonArea.transform);
        //Set the button display text 
        ButtonChart.transform.GetChild(0).GetComponent<Text>().text = chartName;
        //Set the text color
        ButtonChart.transform.GetChild(0).GetComponent<Text>().color = resources.ButtonForeColor;
        //Set the backcolor
        ButtonChart.GetComponent<Image>().color = ChartColor;
        //Set the button position and size
        float W = buttonArea.GetComponent<RectTransform>().rect.width;
        float H = buttonArea.GetComponent<RectTransform>().rect.height;
        ButtonChart.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(W - config.textGap * 5, resources.ButtonH - config.textGap);
        ButtonChart.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, H / 2 - resources.ButtonH / 2 - (float)(chartData.Count-1) * resources.ButtonH);
        ButtonChart.GetComponent<Button>().onClick.AddListener(delegate ()
        {   //Add the button function of button click
            this.ConfigAreaDisplay(buttonIndex);
        });
        chartData[buttonIndex].btnConfig = ButtonChart.GetComponent<Button>();
    }

    private void GetGameObjects()
    {
        buttonArea = this.transform.Find("ButtonArea").gameObject;
        displayArea = this.transform.Find("DisplayArea").gameObject;
        pointsUI = displayArea.transform.Find("PointsUI").gameObject;
        AX_UI = displayArea.transform.Find("AX_UI").gameObject;
        chartAXLine = AX_UI.transform.Find("ChartAXLine").gameObject;

        AX.textAX_YMaxA = AX_UI.transform.Find("TextAX_YMaxA").GetComponent<Text>();
        AX.textAX_YMaxB = AX_UI.transform.Find("TextAX_YMaxB").GetComponent<Text>();
        AX.textAX_YMinA = AX_UI.transform.Find("TextAX_YMinA").GetComponent<Text>();
        AX.textAX_YMinB = AX_UI.transform.Find("TextAX_YMinB").GetComponent<Text>();
        AX.textAX_XMax = AX_UI.transform.Find("TextAX_XMax").GetComponent<Text>();
        AX.textAX_XMin = AX_UI.transform.Find("TextAX_XMin").GetComponent<Text>();

        configArea = this.transform.Find("ConfigArea").gameObject;
        sliderColor = configArea.transform.Find("SliderColor").GetComponent<Slider>();
        sliderThickness = configArea.transform.Find("SliderThickness").GetComponent<Slider>();
        toggleVisible = configArea.transform.Find("ToggleVisible").GetComponent<Toggle>();

    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (chartData.Count == 0)
        {
            //AddChartData("PID", Color.blue);
            GetGameObjects();
            chartAX_Aset = true;
            chartAX_Bset = true;
            ChartAdjust();

        //    AddChartData("PID Data1", Color.blue);

        //    float PosW = ChartData.GetComponent<RectTransform>().rect.width;
        //    float PosH = ChartData.GetComponent<RectTransform>().rect.height;
        //    List<Vector2> PointTemp = new List<Vector2>();
        //    PointTemp.Add(new Vector2(PosW * 0, PosH * 0));
        //    PointTemp.Add(new Vector2(PosW * 0.2f, PosH * 0.2f));
        //    PointTemp.Add(new Vector2(PosW * 0.4f, PosH * 0.6f));
        //    PointTemp.Add(new Vector2(PosW * 0.6f, PosH * 0.1f));
        //    PointTemp.Add(new Vector2(PosW * 0.8f, PosH * 1f));
        //    PointTemp.Add(new Vector2(PosW * 1f, PosH * 0.4f));

        //    PointData[0].GetComponentInChildren<UILineRenderer>().Points = PointTemp.ToArray();
        //    ChartAdjust();
        }

    }
#endif



}
