using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class graphScr : MonoBehaviour
{

    public GameObject pen;
    public TrailRenderer pentrail;
    public controlScript main;

    public List<Coordinate> points = new List<Coordinate>();
    int anim = 0;
    int totalanim = 0;




    public float x;
    public float y;

    private bool waitbool = false;


    // Start is called before the first frame update
    void Start()
    {

        pen.transform.localPosition = new Vector3(0, 0, 0);
        pentrail.emitting = false;
    }


    public void graph(int mode)
    {
        switch (mode)
        {
            case 1:
                draw(11, 1);
                break;
            case 2:
                draw(10, 3);
                break;
            case 3:
                draw(20, 5);
                break;

        }
    }

    private void draw(int pointCount, int groupSize)
    {
        points.Clear();

        DateTime temp = DateTime.Now;

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


            Debug.Log(points[(int)(anim / 10)].getX());
            Debug.Log(points[(int)(anim / 10)].getY());
            Debug.Log(((float)anim % 10f)/10f);

            x = Mathf.Lerp(points[(int)(anim / 10)].getX(), points[(int)(anim / 10) + 1].getX(), ((float)anim % 10f) / 10f);

            y = Mathf.Lerp(points[(int)(anim / 10)].getY(), points[(int)(anim / 10) + 1].getY(), ((float)anim % 10f) / 10f);


            pentrail.emitting = true;
            pen.transform.localPosition = new Vector3(x, 0, y);


            anim++;

            //if (!waitbool)
            //{
            //    Debug.Log(points.Count);

            //    pen.transform.localPosition = new Vector3(points[points.Count - anim].getX(), 0, points[points.Count - anim].getY());
            //    pentrail.emitting = true;
            //    anim--;

            //    waitbool = true;
            //}
            //else
            //{
            //    waitbool = false;
            //}
        }
        else
        {
            anim = 0;
            totalanim = 0;
        }


    }
}
