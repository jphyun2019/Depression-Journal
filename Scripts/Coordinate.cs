using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinate
{
    private int x;
    private float y;


    public Coordinate(int x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public void printCoord()
    {
        Debug.Log(x + "," + y);
    }


    public int getX()
    {
        return this.x;
    }

    public float getY()
    {
        return this.y;
    }

    public void setX(int x)
    {
        this.x = x;
    }

    public void setY(float y)
    {
        this.y = y;
    }

}
