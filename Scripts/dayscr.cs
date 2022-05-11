using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class dayscr : MonoBehaviour
{
    public TextMeshProUGUI num;
    public DateTime date;
    public float dayIntl = 0;
    public Button butt;
    public controlScript controller;
    public Image im;


    void Start()
    {
        Button btn = butt.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

    }

    void TaskOnClick()
    {
        controller.selectedDate = date;
        controller.moveDay();
    }
    public void updateColor()
    {
        if (dayIntl > 0)
        {
            float red = 1;
            float green = 1;

            if(dayIntl < 5)
            {
                green = 0.5f + 0.1f * dayIntl;
            }
            else if (dayIntl > 5)
            {
                red = 1f- 0.1f*(dayIntl-5f);
            }

            im.color = new Color(red, green, 0.6f, 0.8f);
        }
        else
        {
            im.color = new Color(1, 1, 1, 0.8f);
        }


    }

}
