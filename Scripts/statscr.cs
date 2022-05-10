using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class statscr : MonoBehaviour
{
    public controlScript controller;

    public TextMeshProUGUI range;
    public GameObject rightbutt;
    public GameObject leftbutt;

    public TextMeshProUGUI average;
    public TextMeshProUGUI turbulence;
    public TextMeshProUGUI overall;

    public dayscr bestday;
    public dayscr worstday;

    public TextMeshProUGUI weekday;
    public TextMeshProUGUI weekend;

    public sliderScr[] bars;

    public int mode;
 


    public void updateStats()
    {

        drawChart();
        switch (mode)
        {

            case 1:

                writeStats(10);
                break;

            case 2:

                writeStats(30);
                break;

            case 3:

                writeStats(100);
                break;

        }
    }

    public void writeStats(int r)
    {
        range.SetText((r) + " days");


        List<Entry> subset = new List<Entry>();
        int temp = (DateTime.Now - (new DateTime(2022, 1, 1).Date)).Days;

        float total = 0;
        int counter = 0;


        for (int i = 0; i < r; i++)
        {
            foreach (Entry e in controller.entries)
            {
                if (e.getDateSince2022() == temp - i)
                {

                    subset.Add(e);
                    total += e.getVal();
                    counter++;
                }
            }
        }

        float ave = total / (float)counter;
        average.SetText(Math.Round(ave, 2).ToString());


        total = 0;
        foreach(Entry e in subset)
        {
            total += Math.Abs(e.getVal()-ave);
        }

        turbulence.SetText(Math.Round(total/(subset.Count), 2).ToString());

        float totalstart = 0;
        float totalend = 0;
        counter = subset.Count / 3;
        for(int i = counter; i>=0; i--)
        {
            totalend += subset[i].getVal();
            totalstart += subset[subset.Count - 1 - i].getVal();
        }

        Debug.Log(totalstart);
        Debug.Log(totalend);
        Debug.Log(counter);


        float percent = (float)Math.Round((100f*((totalend - totalstart) / ((float)counter)) / (totalstart / (float)counter)), 2);
        overall.SetText(percent + "% " + ((percent > 0)? "Increase":"Decrease"));


        Entry bes = new Entry();
        Entry wor = new Entry();
        wor.setVal(10);

        for (int i = subset.Count-1; i>=0; i--)
        {
            if(subset[i].getVal() > bes.getVal())
            {
                bes = subset[i];
            }
            if(subset[i].getVal() < wor.getVal())
            {
                wor = subset[i];
            }
        }


  
        bestday.date = bes.getDate();
        bestday.dayIntl = bes.getVal();
        bestday.updateColor();
        bestday.num.SetText(bes.getDate().ToString("M/d") + " (" + bes.getVal() + ")" + (bes.getNotes().Equals("")? "": "*"));




        worstday.date = wor.getDate();
        worstday.dayIntl = wor.getVal();
        worstday.updateColor();
        worstday.num.SetText(wor.getDate().ToString("M/d") + " (" + wor.getVal() + ")" + (wor.getNotes().Equals("")? "" : "*"));



    }

    public void drawChart()
    {


        float[] totals = { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
        int[] counters = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };


        foreach (Entry e in controller.entries)
        {

            int startNum = 0;


            switch (e.getDate().ToString("ddd"))
            {
                case "Sun": startNum = 0; break;
                case "Mon": startNum = 1; break;
                case "Tue": startNum = 2; break;
                case "Wed": startNum = 3; break;
                case "Thu": startNum = 4; break;
                case "Fri": startNum = 5; break;
                case "Sat": startNum = 6; break;
            }
            totals[startNum] += e.getVal();
            counters[startNum]++;

            if((startNum == 0)||(startNum == 6))
            {
                totals[7] += e.getVal();
                counters[7]++;
            }
            else
            {
                totals[8] += e.getVal();
                counters[8]++;
            }
        }

        int counter = 0;
        foreach (sliderScr s in bars)
        {
            s.setFloat((totals[counter] / (float)counters[counter]));


            if ((totals[counter] / (float)counters[counter]) > 0)
            {
                float red = 1;
                float green = 1;

                if ((totals[counter] / (float)counters[counter]) < 5)
                {
                    green = 0.5f + 0.1f * (totals[counter] / (float)counters[counter]);
                }
                else if ((totals[counter] / (float)counters[counter]) > 5)
                {
                    red = 1f - 0.1f * ((totals[counter] / (float)counters[counter]) - 5f);
                }

                s.image.color = new Color(red, green, 0.6f, 1);
            }
            else
            {
                s.image.color = new Color(1, 1, 1, 1);
            }

            counter++;
        }


        weekday.SetText("Weekday: " + Math.Round((float)totals[8] / (float)counters[8], 3));
        weekend.SetText("Weekend: " + Math.Round((float)totals[7] / (float)counters[7], 3));








    }

    public void moveRight()
    {

        leftbutt.SetActive(true);
        mode++;
        updateStats();
        if (mode == 3)
        {
            rightbutt.SetActive(false);
        }


    }

    public void moveLeft()
    {

        rightbutt.SetActive(true);
        mode--;
        updateStats();
        if (mode == 1)
        {
            leftbutt.SetActive(false);
        }


    }


}
