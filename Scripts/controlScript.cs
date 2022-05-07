using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class controlScript : MonoBehaviour
{
    public sqlTest sqlscr;

    public Vector3 oldCampos;
    public Vector3 newCampos;
    public Vector3 newCamrot;
    public float newCamFOV;
    public Camera cam;

    public int mode;
    public int animCounter;


    public DateTime selectedDate;
    public DateTime monSelDate;
    public List<Entry> entries = new List<Entry>();

    public TextMeshProUGUI dayText;
    public TextMeshProUGUI monthText;
    public TextMeshProUGUI valText;
    public TMP_InputField notesText;
    public sliderScr slider;


    public GameObject dayPage;
    public GameObject calPage;
    public GameObject graphPage;
    public GameObject statsPage;

    public dayscr[] days;
    public TextMeshProUGUI calMonth;
    public TextMeshProUGUI calYear;

    public graphScr grapher;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        //newCampos = new Vector3(0, 6.5f, 0);


        newCampos = new Vector3(-1.5f, 2.4f, -1.3f);
        newCamrot = new Vector3(90, 0, 0);

        dayPage.SetActive(true);
        calPage.SetActive(false);
        graphPage.SetActive(false);
        statsPage.SetActive(false);


        newCamFOV = 60;
        sqlscr.sqlStart();
        selectedDate = DateTime.Now;
        moveDay();


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(animCounter > 0)
        {
            animCounter--;
        }
        if(animCounter == 1)
        {
            switch (mode)
            {
                case 1:
                    calPage.SetActive(true);
                    break;
                case 2:
                    graphPage.SetActive(true);
                    break;
                case 3:
                    dayPage.SetActive(true);
                    break;
                case 4:
                    statsPage.SetActive(true);
                    break;

            }


        }
        if (newCampos != cam.transform.position)
        {
            cam.transform.position = new Vector3(Mathf.Lerp(oldCampos.x, newCampos.x, (20-animCounter)*0.05f), Mathf.Lerp(oldCampos.y, newCampos.y, (20 - animCounter) * 0.05f), Mathf.Lerp(oldCampos.z, newCampos.z, (20 - animCounter) * 0.05f));
        }
        if (newCamrot != cam.transform.eulerAngles)
        {
            cam.transform.eulerAngles = new Vector3(Mathf.Lerp(cam.transform.eulerAngles.x, newCamrot.x, 0.05f), Mathf.Lerp(cam.transform.eulerAngles.y, newCamrot.y, 0.05f), Mathf.Lerp(cam.transform.eulerAngles.z, newCamrot.z, 0.05f));
        }
        if(newCamFOV != cam.fieldOfView)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newCamFOV, 0.05f);
        }

    }

    public void moveDay()
    {
        newCampos = new Vector3(-1.5f, 2.4f, -1.3f);
        dayText.SetText(selectedDate.ToString("dddd dd"));
        monthText.SetText(selectedDate.ToString("MMMM"));
        valText.SetText(searchfor(selectedDate).Item1.ToString().Equals("0") ? "?" : searchfor(selectedDate).Item1.ToString());
        notesText.text = (searchfor(selectedDate).Item2.ToString());
        slider.setValue((int)(searchfor(selectedDate).Item1 * 2f));
        moveTo(3);

    }
    public void moveCal()
    {
        newCampos = new Vector3(-1.5f, 2.4f, 1.3f);
        moveTo(1);
        monSelDate = new DateTime(selectedDate.Year, selectedDate.Month, 1);
        generateCal(monSelDate);

    }
    public void moveGraph()
    {
        newCampos = new Vector3(1.5f, 2.4f, 1.3f);
        sqlscr.read();
        grapher.graph(1);
        moveTo(2);
    }
    public void moveStats()
    {
        newCampos = new Vector3(1.5f, 2.4f, -1.3f);
        moveTo(4);
    }



    public void moveTo(int i)
    {
        oldCampos = cam.transform.position;
        animCounter = 20;
        mode = i;

    }

    public (float, string) searchfor(DateTime date)
    {
        sqlscr.read();
        float val = 0f;
        string notes = "";


        foreach (Entry e in entries)
        {

            if (e.getDateSince2022() == (date - (new DateTime(2022, 1, 1).Date)).Days)
            {
                val = e.getVal();
                notes = e.getNotes();
            }
        }


        return (val, notes);
    }

    public void add()
    {
        sqlscr.add(selectedDate, slider.getValue(), notesText.text);
        valText.SetText(searchfor(selectedDate).Item1.ToString().Equals("0")? "?": searchfor(selectedDate).Item1.ToString());
        notesText.text = (searchfor(selectedDate).Item2.ToString());
    }

    public void navBack()
    {
        selectedDate = selectedDate.Subtract(TimeSpan.FromDays(1));
        dayText.SetText(selectedDate.ToString("dddd dd"));
        monthText.SetText(selectedDate.ToString("MMMM"));
        valText.SetText(searchfor(selectedDate).Item1.ToString().Equals("0") ? "?" : searchfor(selectedDate).Item1.ToString());
        notesText.text = (searchfor(selectedDate).Item2.ToString());
        slider.setValue((int)(searchfor(selectedDate).Item1 * 2f));
    }
    public void navForwards()
    {
        selectedDate = selectedDate.AddDays(1);
        dayText.SetText(selectedDate.ToString("dddd dd"));
        monthText.SetText(selectedDate.ToString("MMMM"));
        valText.SetText(searchfor(selectedDate).Item1.ToString().Equals("0") ? "?" : searchfor(selectedDate).Item1.ToString());
        notesText.text = (searchfor(selectedDate).Item2.ToString());
        slider.setValue((int)(searchfor(selectedDate).Item1*2f));
    }
    public void navMonBack()
    {
        monSelDate = monSelDate.AddMonths(-1);
        generateCal(monSelDate);

    }
    public void navMonForwards()
    {
        monSelDate = monSelDate.AddMonths(1);
        generateCal(monSelDate);

    }





    public void deleteCurrent()
    {
        sqlscr.delete(selectedDate);
        dayText.SetText(selectedDate.ToString("dddd dd"));
        monthText.SetText(selectedDate.ToString("MMMM"));
        valText.SetText(searchfor(selectedDate).Item1.ToString().Equals("0") ? "?" : searchfor(selectedDate).Item1.ToString());
        notesText.text = (searchfor(selectedDate).Item2.ToString());
        slider.setValue((int)(searchfor(selectedDate).Item1 * 2f));
    }

    public void generateCal(DateTime seldate)
    {
        calYear.text = seldate.ToString("yyy");
        calMonth.text = seldate.ToString("MMMM");
        int monthLen = DateTime.DaysInMonth(seldate.Year, seldate.Month);
        string startingday = new DateTime(seldate.Year, seldate.Month, 1).ToString("ddd");

        int startNum = 0;
        switch (startingday)
        {
            case "Sun": startNum = 0; break;
            case "Mon": startNum = 1; break;
            case "Tue": startNum = 2; break;
            case "Wed": startNum = 3; break;
            case "Thu": startNum = 4; break;
            case "Fri": startNum = 5; break;
            case "Sat": startNum = 6; break;
        }


        for (int i = 0; i < 42; i++)
        {
            if (i < startNum)
            {
                days[i].date = new DateTime();
                days[i].num.text = "";
                days[i].dayIntl = 0;
                days[i].butt.interactable = false;
            }
            else if (i < startNum + monthLen)
            {
                
                days[i].date = new DateTime(seldate.Year, seldate.Month, (i - startNum + 1));
                days[i].num.text = (i - startNum + 1).ToString();
                days[i].dayIntl = searchfor(days[i].date).Item1;
                days[i].updateColor();
                days[i].butt.interactable = true;
            }
            else
            {
                days[i].date = new DateTime();
                days[i].num.text = "";
                days[i].dayIntl = 0;
                days[i].butt.interactable = false;
            }
        }




    }
}
