using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entry
{

    private DateTime date;
    private float val;
    private string notes;
    private int dateSince2022;


    // Constructors
    public Entry()
    {
        this.date = new DateTime(2022, 1, 1);
        this.val = 0;
        this.notes = "Enter Here: ";
        this.dateSince2022 = 0;
    }
    public Entry(DateTime date, float val, string notes)
    {
        this.date = date;
        this.val = val;
        this.notes = notes;
        this.dateSince2022 = (date - (new DateTime(2022, 1, 1).Date)).Days;

    }

    public void printEntry()
    {
        Debug.Log($"Date: {this.date.ToString("MM/dd/yyyy")}, Value: {this.val}, Notes: {this.notes}, Days Since 2022: {dateSince2022}");
    }



    // Getters and Setters
    public DateTime getDate()
    {
        return this.date;
    }
    public void setDate(DateTime d)
    {
        this.date = d;
    }

    public float getVal()
    {
        return this.val;
    }
    public void setVal(float f)
    {
        this.val = f;
    }

    public string getNotes()
    {
        return this.notes;
    }
    public void setNotes(string n)
    {
        this.notes = n;
    }

    public int getDateSince2022()
    {
        return this.dateSince2022;
    }
    public void setDateSince2022(int i)
    {
        this.dateSince2022 = i;
    }



}
