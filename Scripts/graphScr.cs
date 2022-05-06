using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class graphScr : MonoBehaviour
{

    public GameObject pen;
    public TrailRenderer pentrail;
    public controlScript main;

    public float x;
    public float y;


    // Start is called before the first frame update
    void Start()
    {
        pentrail.emitting = false;
        pen.transform.position = new Vector3(0, 0, 0);
    }


    public void graph(int mode)
    {
        switch (mode)
        {
            case 1:
                draw(10, 1);
                break;
            case 2:
                draw(10, 3);
                break;
            case 3:
                draw(20, 5);
                break;

        }
    }

    public void draw(int pointCount, int groupSize)
    {
        List<Coordinate> points = new List<Coordinate>();

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

            float average = total / counter;

            if(average != 0)
            {
                points.Add(new Coordinate(i, average));
            }
        }
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
