using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class statscr : MonoBehaviour
{

    // Controller main script
    public controlScript controller;

    // Range text box
    public TextMeshProUGUI range;

    // Navigation buttons
    public GameObject rightbutt;
    public GameObject leftbutt;

    // Text boxes
    public TextMeshProUGUI average;
    public TextMeshProUGUI turbulence;
    public TextMeshProUGUI overall;

    // Day buttons
    public dayscr bestday;
    public dayscr worstday;

    // Day Type text boxes
    public TextMeshProUGUI weekday;
    public TextMeshProUGUI weekend;

    // Bar graph bars
    public sliderScr[] bars;

    public int mode;
 


    // Updates the stats page with current data
    public void updateStats()
    {
        Debug.Log(controller.entries.Count);

        // Resets the bar graph
        foreach (sliderScr s in bars)
        {
            s.setFloat(0);
        }

        // Draws the graph
        drawChart();

        // Switches based off mode
        switch (mode)
        {
            case 1:
                if (controller.entries.Count > 0)
                {
                    writeStats(10);
                }
                range.SetText("10 days");
                break;

            case 2:
                if (controller.entries.Count > 0)
                {
                    writeStats(30);
                }
                range.SetText("30 days");
                break;

            case 3:
                if (controller.entries.Count > 0)
                {
                    writeStats(100);
                }
                range.SetText("100 days");
                break;
            case 4:
                if (controller.entries.Count > 0)
                {
                    writeStats(200);
                }
                range.SetText("200 days");
                break;
            case 5:
                if (controller.entries.Count > 0)
                {
                    writeStats(400);
                }
                range.SetText("400 days");
                break;
            case 6:
                if (controller.entries.Count > 0)
                {
                    writeStats(800);
                }
                range.SetText("800 days");
                break;


        }
    }

    // Updates the stats that depend of time ranges
    public void writeStats(int r)
    {
        // Sets the range text box
        range.SetText((r) + " days");

        // Creates a list of entries
        List<Entry> subset = new List<Entry>();
        int temp = (DateTime.Now - (new DateTime(2022, 1, 1).Date)).Days;

        float total = 0;
        int counter = 0;

        // For each entry that fits within the date range
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

        // If there are entries within the range
        if(subset.Count > 0)
        {
            // Calculates average value
            float ave = total / (float)counter;
            average.SetText(Math.Round(ave, 2).ToString());


            total = 0;
            // Calculates mean absolute deviantion
            foreach (Entry e in subset)
            {
                total += Math.Abs(e.getVal() - ave);
            }

            turbulence.SetText(Math.Round(total / (subset.Count), 2).ToString());

            // Divides the entries into 3 parts
            float totalstart = 0;
            float totalend = 0;
            counter = subset.Count / 3;

            // Finds averages of first and last thirds
            for (int i = counter; i >= 0; i--)
            {
                totalend += subset[i].getVal();
                totalstart += subset[subset.Count - 1 - i].getVal();
            }

            Debug.Log(totalstart);
            Debug.Log(totalend);
            Debug.Log(counter);

            // Calculates the percent increase or decrease
            float percent = (float)Math.Round((100f * ((totalend - totalstart) / ((float)counter)) / (totalstart / (float)counter)), 2);
            overall.SetText(percent + "% " + ((percent > 0) ? "Increase" : "Decrease"));

            // Creates two new Entry Objects
            Entry bes = new Entry();
            Entry wor = new Entry();
            wor.setVal(10);

            // Finds the best and worst days of the time range, starting from the beginning of the range
            for (int i = subset.Count - 1; i >= 0; i--)
            {
                if (subset[i].getVal() > bes.getVal())
                {
                    bes = subset[i];
                }
                if (subset[i].getVal() < wor.getVal())
                {
                    wor = subset[i];
                }
            }

            // Updates text boxes and colors

            bestday.date = bes.getDate();
            bestday.dayIntl = bes.getVal();
            bestday.updateColor();
            bestday.num.SetText(bes.getDate().ToString("M/d/y") + " (" + bes.getVal() + ")" + (bes.getNotes().Equals("") ? "" : "*"));

            worstday.date = wor.getDate();
            worstday.dayIntl = wor.getVal();
            worstday.updateColor();
            worstday.num.SetText(wor.getDate().ToString("M/d/y") + " (" + wor.getVal() + ")" + (wor.getNotes().Equals("") ? "" : "*"));
        }

    }

    // Updates the stats that do not depend on time range
    public void drawChart()
    {
        
        // Creates two lists with each day of the week's values and totals
        float[] totals = { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
        int[] counters = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        // For each entry in the main entry list
        foreach (Entry e in controller.entries)
        {

            int startNum = 0;

            // Finds the day type for each entry
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
            // Adds values
            totals[startNum] += e.getVal();
            counters[startNum]++;

            // If a Saturday or Sunday
            if((startNum == 0)||(startNum == 6))
            {
                totals[7] += e.getVal();
                counters[7]++;
            }
            // If a weekday
            else
            {
                totals[8] += e.getVal();
                counters[8]++;
            }
        }

        int counter = 0;
        // For each bar in the bar graph
        foreach (sliderScr s in bars)
        {

            if ((totals[counter] / (float)counters[counter]) > 0)
            {
                s.setFloat((totals[counter] / (float)counters[counter]));
                float red = 1;
                float green = 1;

                if ((totals[counter] / (float)counters[counter]) < 5)
                {
                    green = 0.5f + 0.25f * (totals[counter] / (float)counters[counter]);
                }
                else if ((totals[counter] / (float)counters[counter]) > 5)
                {
                    red = 1f - 0.25f * ((totals[counter] / (float)counters[counter]) - 5f);
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

    // Navigating to a higher range
    public void moveRight()
    {

        leftbutt.SetActive(true);
        mode++;
        updateStats();
        if (mode == 6)
        {
            rightbutt.SetActive(false);
        }


    }

    // Navigating to a lower range
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
