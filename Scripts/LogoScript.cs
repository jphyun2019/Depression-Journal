using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoScript : MonoBehaviour
{
    public Image image;

    public void setOpacicty(float opacity)
    {
        image.color = new Color(1, 1, 1, opacity);
    }

    public void setBrightness(float brightness)
    {
        image.color = new Color(brightness, brightness, brightness, 1);
    }


}
