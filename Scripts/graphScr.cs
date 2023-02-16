using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


public class graphScr : MonoBehaviour
{

    // Pen Object
    public GameObject pen;
    // Pen Trail Renderer
    public TrailRenderer pentrail;
    // Main Control Script
    public controlScript main;

    // List of Coordinate Objects
    public List<Coordinate> points = new List<Coordinate>();

    // Graphing Animation Frame
    int anim = 0;
    // Final Animation Frame
    int totalanim = 0;
    // Buffer Frames before graphing starts
    int bufferFrames = 0;


    // X and Y value of pen
    public float x;
    public float y;

    // Mode Text Box
    public TextMeshProUGUI modeText;

    // Navigation Buttons
    public GameObject leftButt;
    public GameObject rightButt;

    // Current Mode
    public int mode = 1;
    // Origin of Graph
    public GameObject origin;

    // Array of extra lines
    public GameObject[] extralines;
    // Array of Date Text boxes
    public TextMeshProUGUI[] dateTexts;
    public GameObject[] extradates;




    // Start is called before the first frame update
    void Start()
    {

        mode = 1;
        leftButt.SetActive(false);
        rightButt.SetActive(true);
        pen.transform.localPosition = new Vector3(0, 0, 0);
        pentrail.emitting = false;
    }


    // Accesses draw method and sets parameters
    public void graph()
    {
        // Switches based off mode
        switch (mode)
        {
            case 1:
                // Calls draw function with 11,1
                foreach(GameObject g in extralines) { g.SetActive(false); }
                foreach (GameObject g in extradates) { g.SetActive(false); }
                origin.transform.localScale = new Vector3(1, 1, 1);
                draw(11, 1);
                break;

            case 2:
                // Calls draw function with 11,3
                foreach (GameObject g in extralines) { g.SetActive(false); }
                foreach (GameObject g in extradates) { g.SetActive(false); }
                origin.transform.localScale = new Vector3(1, 1, 1);
                draw(11, 3);
                break;

            case 3:
                // Calls draw function with 21,5
                // Also renders extra lines
                foreach (GameObject g in extralines) { g.SetActive(true); }
                foreach (GameObject g in extradates) { g.SetActive(true); }
                origin.transform.localScale = new Vector3(0.5f, 1, 1);
                draw(21, 5);
                break;
            case 4:
                // Calls draw function with 41,10
                // Also renders extra lines
                foreach (GameObject g in extralines) { g.SetActive(true); }
                foreach (GameObject g in extradates) { g.SetActive(true); }

                origin.transform.localScale = new Vector3((0.4f), 1, 1);
                draw(26, 8);
                break;
            case 5:
                // Calls draw function with 41,10
                // Also renders extra lines
                foreach (GameObject g in extralines) { g.SetActive(true); }
                foreach (GameObject g in extradates) { g.SetActive(true); }

                origin.transform.localScale = new Vector3((0.25f), 1, 1);
                draw(41, 10);
                break;
        }
    }

    // Draws the graph
    private void draw(int pointCount, int groupSize)
    {
        // Clears graph
        pentrail.emitting = false;
        points.Clear();

        DateTime temp = DateTime.Now;

        // Sets the x-axis dates
        for(int i = 0; i < 9; i++)
        {
            dateTexts[i].SetText(temp.AddDays(-groupSize * 1f*(i+1) * (((float)pointCount-1f)/10f)).ToString("M/d"));
        }
        
        // Sets the mode text
        modeText.SetText(((pointCount - 1)*groupSize) + " days");


        // For each keyframe location
        for(int i = 0; i < pointCount; i ++)
        {
            float total = 0;
            int counter = 0;
            
            // For all dates entries within each location
            for(int j = 0; j < groupSize; j++)
            {
                // If the date entry has a value
                if (!(main.searchfor(temp).Item1 == 0))
                {
                    // Add up the values
                    total += main.searchfor(temp).Item1;
                    counter++;
                }
                temp = temp.AddDays(-1);
            }

            // Finds the average of the values and creates a Coordinate object
            if(counter > 0)
            {
                float average = total / counter;
                points.Add(new Coordinate(i, average));

            }
        }
        
        // For each coordinate
        foreach (Coordinate c in points)
        {
            c.printCoord();
        }

        // If there are coordinates
        pentrail.emitting = false;
        if(points.Count > 0)
        {
            // Moves the pen to the first coordinate
            pen.transform.localPosition = new Vector3(points[0].getX(), 0, points[0].getY());
            pentrail.emitting = false;


            anim = 0;
            totalanim = (points.Count - 1) * 10;
            bufferFrames = 10;

            pentrail.Clear();
        }

    }

    // Lagrange Formulas
    public static float GetValue(List<Coordinate> controlPoints, float x)
    {
        var result = 0.0f;
        for (var j = 0; j < controlPoints.Count; j++)
        {
            
            result += Inner(controlPoints, x, j);
        }
        return result;
    }

    private static float Inner(List<Coordinate> controlPoints, float x, int j)
    {
        var dividend = 1.0f;
        var divisor = 1.0f;
        for (var k = 0; k < controlPoints.Count; k++)
        {
            if (k == j)
            {
                continue;
            }
            dividend *= (x - controlPoints[k].getX());
            divisor *= (controlPoints[j].getX() - controlPoints[k].getX());
        }
        return (dividend / divisor) * controlPoints[j].getY();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // If it is animating
        if (anim < totalanim)
        {
            // If it finished buffering to let pen clear
            if(bufferFrames == 0)
            {
                Debug.Log(points[(int)(anim / 10)].getX());
                Debug.Log(points[(int)(anim / 10)].getY());
                Debug.Log(((float)anim % 10f) / 10f);

                // Set x and y values between coordinate points based off animation frame
                x = Mathf.Lerp(points[(int)(anim / 10)].getX(), points[(int)(anim / 10) + 1].getX(), ((float)anim % 10f) / 10f);

                y = Mathf.Lerp(points[(int)(anim / 10)].getY(), points[(int)(anim / 10) + 1].getY(), ((float)anim % 10f) / 10f);


                // Moves the pen to the location
                pentrail.emitting = true;
                pen.transform.localPosition = new Vector3(x, 0, y);

                anim++;
            }
            else
            {
                // Move the pen to the origin
                pen.transform.localPosition = new Vector3(points[0].getX(), 0, points[0].getY());
                bufferFrames--;
            }
        }
        else
        {
            anim = 0;
            totalanim = 0;
        }


    }

    // Navigating to a larger mode
    public void moveRight()
    {

        leftButt.SetActive(true);
        mode++;
        graph();
        if(mode == 5)
        {
            rightButt.SetActive(false);
        }


    }
    
    // Navigating to a smaller mdoe
    public void moveLeft()
    {

        rightButt.SetActive(true);
        mode--;
        graph();
        if(mode == 1)
        {
            leftButt.SetActive(false);
        }
    }

}
