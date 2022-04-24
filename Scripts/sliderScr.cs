using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sliderScr : MonoBehaviour
{
    public Slider slider;


    public void setValue(int i)
    {
        slider.value = i;
    }

    public float getValue()
    {
        return (float)slider.value / 2f;
    }

}
