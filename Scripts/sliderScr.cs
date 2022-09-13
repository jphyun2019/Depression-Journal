using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sliderScr : MonoBehaviour
{

    // Slider UI Object
    public Slider slider;
    // Sprite of the Slider
    public Image image;


    // Getters and Setters
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

    // Changes the color based off the value
    public void updateColor(float val)
    {

        // If the value is not default
        if (val > 0)
        {
            float red = 1;
            float green = 1;

            // Lowers Green value if lower than 5
            if (val < 5)
            {
                green = 0.5f + 0.1f * val;
            }
            // Lowers red value if greater than 5
            else if (val > 5)
            {
                red = 1f - 0.1f * (val - 5f);
            }

            // Sets the sprite to the new color
            image.color = new Color(red, green, 0.6f, 1);
        }
        else
        {
            // Sets the sprite to white
            image.color = new Color(1, 1, 1, 1);
        }


    }

}
