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
    public int dayIntl;
    public Button butt;
    public controlScript controller;


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

}
