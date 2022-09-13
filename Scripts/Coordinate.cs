using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinate
{

    // X and Y value of the coordinate
    private int x;
    private float y;


    // Constructor
    public Coordinate(int x, float y)
    {
        this.x = x;
        this.y = y;
    }

    // Prints the variables
    public void printCoord()
    {
        Debug.Log(x + "," + y);
    }


    // Getters and Setters
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
