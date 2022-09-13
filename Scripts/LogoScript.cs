using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoScript : MonoBehaviour
{
    // Logo Image Object
    public Image image;

    // Sets Opacity of the image
    public void setOpacicty(float opacity)
    {
        image.color = new Color(1, 1, 1, opacity);
    }

    // Sets Brightness of the image
    public void setBrightness(float brightness)
    {
        image.color = new Color(brightness, brightness, brightness, 1);
    }


}
