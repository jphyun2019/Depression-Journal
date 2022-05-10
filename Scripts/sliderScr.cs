using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sliderScr : MonoBehaviour
{
    public Slider slider;
    public Image image;


    public void setValue(int i)
    {
        slider.value = i;
    }

    public void setFloat(float f)
    {
        slider.value = f;
    }

    public float getValue()
    {
        return (float)slider.value / 2f;
    }

    public void updateColor(float val)
    {
        if (val > 0)
        {
            float red = 1;
            float green = 1;

            if (val < 5)
            {
                green = 0.5f + 0.1f * val;
            }
            else if (val > 5)
            {
                red = 1f - 0.1f * (val - 5f);
            }

            image.color = new Color(red, green, 0.6f, 1);
        }
        else
        {
            image.color = new Color(1, 1, 1, 1);
        }


    }

}
