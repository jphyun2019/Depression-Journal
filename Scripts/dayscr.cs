using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class dayscr : MonoBehaviour
{

    // Day Number textbox
    public TextMeshProUGUI num;

    // Assigned Date
    public DateTime date;
    // Happiness value
    public float dayIntl = 0;

    // Button Component
    public Button butt;
    // Main control script
    public controlScript controller;
    // Image Component
    public Image im;


    // Start is called before the first frame update
    void Start()
    {
        // Gets the button component and assigns method when clicked
        Button btn = butt.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

    }

    // When the button is clicked
    void TaskOnClick()
    {
        // Sets the selected date and moves to the day page
        controller.selectedDate = date;
        controller.moveDay();
    }

    // Udpates the color of the button to match the value of happiness at a that day
    public void updateColor()
    {
        // If the value exists
        if (dayIntl > 0)
        {
            float red = 1;
            float green = 1;

            // Sets the reds and greens based off the happiness value
            if(dayIntl < 5)
            {
                green = 0.5f + 0.1f * dayIntl;
            }
            else if (dayIntl > 5)
            {
                red = 1f- 0.1f*(dayIntl-5f);
            }

            // Sets the image to that new color
            im.color = new Color(red, green, 0.6f, 0.8f);
        }
        else
        {
            // Sets the image to white
            im.color = new Color(1, 1, 1, 0.8f);
        }


    }

}
