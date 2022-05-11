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

    public int mode = 1;
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

    public statscr stats;



    public Light sun;
    public GameObject logo;
    public LogoScript logoscr;

    private int logoanim = 0; 

    public AudioSource musicPlayer;
    public AudioClip[] dayMusic;
    public AudioClip[] nightMusic;

    private Queue<AudioClip> q = new Queue<AudioClip>();

    private int dayq = 0;
    private int nightq = 0;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        //newCampos = new Vector3(0, 6.5f, 0);

        dayMusic = shuffle(dayMusic);
        nightMusic = shuffle(nightMusic);


        logo.SetActive(true);
        logoscr.setOpacicty(1);
        logoanim = 200;


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
        double percent = (double)Math.Abs(((DateTime.Now.Hour * 3600) + (DateTime.Now.Minute * 60) + DateTime.Now.Second) - 43200d) / 43200d;


        double red = Mathf.Lerp(0.9568f, 0.3528f,(float)percent);
        double blue = Mathf.Lerp(0.6588f, 0.0794f, (float)percent);
        double green = Mathf.Lerp(0.6117f, 0.5849f, (float)percent);

        sun.color = new Color((float)red, (float)blue, (float)green);
        if (q.Count == 0)
        {
            if (percent < 0.5)
            {
                q.Enqueue(dayMusic[dayq]);
                if (dayq == dayMusic.Length - 1)
                {
                    dayq = 0;
                }
                else
                {
                    dayq++;
                }
            }
            else
            {
                q.Enqueue(nightMusic[nightq]);
                if (nightq == nightMusic.Length - 1)
                {
                    nightq = 0;
                }
                else
                {
                    nightq++;
                }
            }
        }

        musicPlayer.UnPause();
        if (musicPlayer.isPlaying == false)
        {
            musicPlayer.clip = q.Dequeue();
            musicPlayer.Play();
        }
        if (q.Count == 0)
        {
            if(percent < 0.5)
            {
                q.Enqueue(dayMusic[dayq]);
                if(dayq == dayMusic.Length - 1)
                {
                    dayq = 0;
                }
                else
                {
                    dayq++;
                }
            }
            else
            {
                q.Enqueue(nightMusic[nightq]);
                if (nightq == nightMusic.Length - 1)
                {
                    nightq = 0;
                }
                else
                {
                    nightq++;
                }
            }
        }




        if (animCounter > 0)
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

        if (logoanim > 0)
        {
            logo.SetActive(true);
            if (logoanim > 50)
            {
                if(logoanim > 150)
                {
                    logoscr.setBrightness(Mathf.Lerp(1, 0, (logoanim - 150f)/50f));
                }
            }
            else
            {
                logoscr.setOpacicty(Mathf.Lerp(0, 1, (float)logoanim / 50f));
            }
            logoanim--;
        }
        else
        {
            logo.SetActive(false);
        }


    }


    private AudioClip[] shuffle(AudioClip[] au)
    {
        int n = au.Length;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n+1);
            AudioClip temp = au[k];
            au[k] = au[n];
            au[n] = temp;
        }
        return au;

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

        //selectedDate = DateTime.Now;
        newCampos = new Vector3(-1.5f, 2.4f, 1.3f);
        moveTo(1);
        monSelDate = new DateTime(selectedDate.Year, selectedDate.Month, 1);
        generateCal(monSelDate);

    }
    public void moveGraph()
    {
        newCampos = new Vector3(1.5f, 2.4f, 1.3f);
        grapher.leftButt.SetActive(false);
        grapher.rightButt.SetActive(true);
        sqlscr.read();
        grapher.mode = 1;
        grapher.graph();
        moveTo(2);
    }
    public void moveStats()
    {
        sqlscr.read();
        newCampos = new Vector3(1.5f, 2.4f, -1.3f);
        stats.leftbutt.SetActive(false);
        stats.rightbutt.SetActive(true);
        moveTo(4);
        stats.mode = 1;
        stats.updateStats();
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
                days[i].im.color = new Color(1, 1, 1, 1);
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
                days[i].im.color = new Color(1, 1, 1, 1);
            }
        }




    }
}
