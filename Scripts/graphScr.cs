using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


public class graphScr : MonoBehaviour
{

    public GameObject pen;
    public TrailRenderer pentrail;
    public controlScript main;

    public List<Coordinate> points = new List<Coordinate>();
    int anim = 0;
    int totalanim = 0;
    int bufferFrames = 0;



    public float x;
    public float y;

    public TextMeshProUGUI modeText;
    public GameObject leftButt;
    public GameObject rightButt;

    public int mode = 1;
    public GameObject origin;

    public GameObject[] extralines;
    public TextMeshProUGUI[] dateTexts;




    // Start is called before the first frame update
    void Start()
    {
        mode = 1;
        leftButt.SetActive(false);
        rightButt.SetActive(true);
        pen.transform.localPosition = new Vector3(0, 0, 0);
        pentrail.emitting = false;
    }


    public void graph()
    {




        switch (mode)
        {
            case 1:
                foreach(GameObject g in extralines) { g.SetActive(false); }
                origin.transform.localScale = new Vector3(1, 1, 1);
                draw(11, 1);
                break;

            case 2:
                foreach (GameObject g in extralines) { g.SetActive(false); }
                origin.transform.localScale = new Vector3(1, 1, 1);
                draw(11, 3);
                break;

            case 3:
                foreach (GameObject g in extralines) { g.SetActive(true); }
                origin.transform.localScale = new Vector3(0.5f, 1, 1);
                draw(21, 5);
                break;

        }
    }

    private void draw(int pointCount, int groupSize)
    {
        pentrail.emitting = false;
        points.Clear();

        DateTime temp = DateTime.Now;

        for(int i = 0; i < 4; i++)
        {
            dateTexts[i].SetText(temp.AddDays(-groupSize * 2f*(i+1) * (((float)pointCount-1f)/10f)).ToString("M/d"));
        }
        


        modeText.SetText(((pointCount - 1)*groupSize) + " days");


        for(int i = 0; i < pointCount; i ++)
        {
            float total = 0;
            int counter = 0;
            

            for(int j = 0; j < groupSize; j++)
            {
                if (!(main.searchfor(temp).Item1 == 0))
                {
                    total += main.searchfor(temp).Item1;
                    counter++;
                }
                temp = temp.AddDays(-1);
            }

            if(counter > 0)
            {
                float average = total / counter;
                points.Add(new Coordinate(i, average));



            }

        }
        
        foreach (Coordinate c in points)
        {
            c.printCoord();
        }

        pentrail.emitting = false;
        pen.transform.localPosition = new Vector3(points[0].getX(), 0, points[0].getY());
        pentrail.emitting = false;


        anim = 0;
        totalanim = (points.Count-1) * 10;
        bufferFrames = 10;

        pentrail.Clear();

        //points.Add(new Coordinate(-1, 5));
        //points.Add(new Coordinate(11, 5));



    }

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


        if (anim < totalanim)
        {
            if(bufferFrames == 0)
            {
                Debug.Log(points[(int)(anim / 10)].getX());
                Debug.Log(points[(int)(anim / 10)].getY());
                Debug.Log(((float)anim % 10f) / 10f);

                x = Mathf.Lerp(points[(int)(anim / 10)].getX(), points[(int)(anim / 10) + 1].getX(), ((float)anim % 10f) / 10f);

                y = Mathf.Lerp(points[(int)(anim / 10)].getY(), points[(int)(anim / 10) + 1].getY(), ((float)anim % 10f) / 10f);


                pentrail.emitting = true;
                pen.transform.localPosition = new Vector3(x, 0, y);


                anim++;
            }
            else
            {
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

    public void moveRight()
    {

        leftButt.SetActive(true);
        mode++;
        graph();
        if(mode == 3)
        {
            rightButt.SetActive(false);
        }


    }
    
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
