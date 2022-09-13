using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class controlScript : MonoBehaviour
{

    // sql Script
    public sqlTest sqlscr;

    // Vectors representing previous camera position and targeted camera position
    // Linearly Interpolates between them to create animation
    public Vector3 oldCampos;
    public Vector3 newCampos;


    // Main Camera Object
    public Camera cam;

    // Mode/Page being rendered
    public int mode = 1;

    // Frame of movement animation
    public int animCounter;


    // Date of current day selected
    public DateTime selectedDate;
    // Date of current month selected
    public DateTime monSelDate;
    // List of Entry Objects
    public List<Entry> entries = new List<Entry>();

    // Textbox displaying current day in day page
    public TextMeshProUGUI dayText;
    // Textbox displaying current month of day in day page
    public TextMeshProUGUI monthText;
    // Textbox displaying current value of day in day page
    public TextMeshProUGUI valText;
    // Textbox displaying current notes of day in day page
    public TMP_InputField notesText;
    // Value slider controller
    public sliderScr slider;

    // Parent object for the Day Page
    public GameObject dayPage;
    // Parent obect for the Calendar Page
    public GameObject calPage;
    // Parent object for the graph page
    public GameObject graphPage;
    // Parent object for the stats page
    public GameObject statsPage;

    // Array of Day Objects for the calendar
    public dayscr[] days;

    // Textbox displaying current month in calendar page
    public TextMeshProUGUI calMonth;
    // Textbox displaying current year in calendar page
    public TextMeshProUGUI calYear;

    // Grapher Script for the graph page
    public graphScr grapher;

    // Stats Script for the stats page
    public statscr stats;


    // Light source object
    public Light sun;
    // Logo object
    public GameObject logo;
    // Logo object script
    public LogoScript logoscr;

    // Current frame of logo animation
    private int logoanim = 0; 

    // Music player Audiosource object
    public AudioSource musicPlayer;

    // Array of music clips for day and night playlists
    public AudioClip[] dayMusic;
    public AudioClip[] nightMusic;

    public AudioClip[] dreamplaylist;
    public AudioClip[] dayplaylist;
    public AudioClip[] nightplaylist;

    // Queue of music clips for the music player
    private Queue<AudioClip> q = new Queue<AudioClip>();

    // Current counter in day playlist
    private int dayq = 0;
    // Current counter in the night playlist
    private int nightq = 0;

    public bool dreaming;

    // Start is called before the first frame update
    void Start()
    {

        dreaming = false;
        // Sets the overall max-frame-rate of the renderer
        Application.targetFrameRate = 60;

        // Runs the shuffle method which suffles both playlists
        dayMusic = shuffle(dayplaylist);
        nightMusic = shuffle(nightplaylist);


        // Sets the logoscreen active
        logo.SetActive(true);
        // Sets it as opaque and sets animation frame counter for 4 seconds
        logoscr.setOpacicty(1);
        logoanim = 200;

        // Sets target position for the Camera at default position
        newCampos = new Vector3(-1.5f, 2.4f, -1.3f);

        // Hides all page parent classes except for the day page
        dayPage.SetActive(true);
        calPage.SetActive(false);
        graphPage.SetActive(false);
        statsPage.SetActive(false);


        // Starts the connection and setup scripts for the sql script
        sqlscr.sqlStart();

        // Sets the selected date as the current date
        selectedDate = DateTime.Now;

        // Moves to the day page position
        moveDay();


    }

    // Fixed Update is called 50 times a second
    void FixedUpdate()
    {
        // Creates a percent of how far from noon the day is: 12pm being 0, 12am being 1
        double percent = (double)Math.Abs(((DateTime.Now.Hour * 3600) + (DateTime.Now.Minute * 60) + DateTime.Now.Second) - 43200d) / 43200d;


        // Linearly interpolates rgb values from the percent to make a gradient between day-mode and night-mode based on time of day.
        double red = Mathf.Lerp(0.9568f, 0.3528f,(float)percent);
        double blue = Mathf.Lerp(0.6588f, 0.0794f, (float)percent);
        double green = Mathf.Lerp(0.6117f, 0.5849f, (float)percent);

        // Sets the light source as this color

        if (dreaming)
        {
            sun.color = new Color(0.32f, 0.9f, 0.32f);
        }
        else
        {
            sun.color = new Color((float)red, (float)blue, (float)green);
        }


        // If the queue has a song in it
        if (q.Count == 0)
        {
            // If the day is closer to noon, queues the next song from the day playlist
            if (percent < 0.5)
            {
                q.Enqueue(dayMusic[dayq]);

                // If it finishes the day playlist, reset the counter
                if (dayq >= dayMusic.Length - 1)
                {
                    dayq = 0;
                }
                else
                {
                    dayq++;
                }
            }
            // If the day is closer to night, queues the next song from the night playlist
            else
            {
                q.Enqueue(nightMusic[nightq]);

                // If it finishes the night playlist, reset the counter
                if (nightq >= nightMusic.Length - 1)
                {
                    nightq = 0;
                }
                else
                {
                    nightq++;
                }
            }
        }

        // Unpauses the music just in case
        musicPlayer.UnPause();

        // If the song finished
        if (musicPlayer.isPlaying == false)
        {
            // Dequeue the next song and start playing it
            musicPlayer.clip = q.Dequeue();
            musicPlayer.Play();
        }
        

        // If the between page animation counter is running
        if (animCounter > 0)
        {
            animCounter--;
        }
        // If the animation just finished
        if(animCounter == 1)
        {
            // Sets the parent page matching the new mode active
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

        // If the target position is not the current position
        if (newCampos != cam.transform.position)
        {
            // Linearly interpolate to the target postion by the animation frame
            cam.transform.position = new Vector3(Mathf.Lerp(oldCampos.x, newCampos.x, (20-animCounter)*0.05f), Mathf.Lerp(oldCampos.y, newCampos.y, (20 - animCounter) * 0.05f), Mathf.Lerp(oldCampos.z, newCampos.z, (20 - animCounter) * 0.05f));
        }


        // If the logo animation is running
        if (logoanim > 0)
        {
            logo.SetActive(true);
            // if it is in the first three seconds
            if (logoanim > 50)
            {
                if(logoanim > 150)
                {
                    // Linearly interpolate the brightness to increase to 1
                    logoscr.setBrightness(Mathf.Lerp(1, 0, (logoanim - 150f)/50f));
                }
            }
            else
            {
                // Linearly interpolates the opacity to fade out over a second
                logoscr.setOpacicty(Mathf.Lerp(0, 1, (float)logoanim / 50f));
            }
            logoanim--;
        }
        // If the logo animation finished
        else
        {
            // Hide the logo
            logo.SetActive(false);
        }


    }


    // Shuffle Method which shuffles an array of music clips
    private AudioClip[] shuffle(AudioClip[] au)
    {
        // Length of array
        int n = au.Length;
        // For each item
        while (n > 1)
        {
            // Finds a random song in the remaining items
            n--;
            int k = UnityEngine.Random.Range(0, n+1);
            AudioClip temp = au[k];
            // Removes it and adds it too a new list
            au[k] = au[n];
            au[n] = temp;
        }
        return au;

    }

    public void dream()
    {
        if (dreaming)
        {
            dayq = 0;
            nightq = 0;
            dayMusic = new AudioClip[8];
            nightMusic = new AudioClip[9];
            dayMusic = shuffle(dayplaylist);
            nightMusic = shuffle(nightplaylist);
            q.Clear();
            musicPlayer.Stop();
            dreaming = false;
        }
        else
        {
            dayMusic = new AudioClip[4];
            nightMusic = new AudioClip[4];
            dayMusic = shuffle(dreamplaylist);
            nightMusic = shuffle(dreamplaylist);
            q.Clear();
            musicPlayer.Stop();
            dreaming = true;
        }
    }


    // Method called when navigating to the day page
    public void moveDay()
    {
        // Sets the target position
        newCampos = new Vector3(-1.5f, 2.4f, -1.3f);

        // Sets text boxes to match the selected day
        dayText.SetText(selectedDate.ToString("dddd dd"));
        monthText.SetText(selectedDate.ToString("MMMM"));
        valText.SetText(searchfor(selectedDate).Item1.ToString().Equals("0") ? "?" : searchfor(selectedDate).Item1.ToString());
        notesText.text = (searchfor(selectedDate).Item2.ToString());
        slider.setValue((int)(searchfor(selectedDate).Item1 * 2f));

        // Starts animation method
        moveTo(3);

    }

    // Method called when navigating to the calendar page
    public void moveCal()
    {
        // Sets the target position
        newCampos = new Vector3(-1.5f, 2.4f, 1.3f);

        // Starts animation method
        moveTo(1);
        // Sets the selected month to match the selected year
        monSelDate = new DateTime(selectedDate.Year, selectedDate.Month, 1);

        // Runs the calendar generation method with the selected month
        generateCal(monSelDate);

    }

    // Method called when navigating to the graphing page
    public void moveGraph()
    {
        // Sets the target position
        newCampos = new Vector3(1.5f, 2.4f, 1.3f);

        // Resets buttons
        grapher.leftButt.SetActive(false);
        grapher.rightButt.SetActive(true);

        // Reads the sql table into the entries list
        sqlscr.read();

        // Sets the grapher mode and running the graphing method
        grapher.mode = 1;
        grapher.graph();

        // Starts the animation method
        moveTo(2);
    }

    // Method called when navigating to the stats page
    public void moveStats()
    {
        // Reads the sql table into the entries list
        sqlscr.read();

        // Sets the target position
        newCampos = new Vector3(1.5f, 2.4f, -1.3f);

        // Resets buttons
        stats.leftbutt.SetActive(false);
        stats.rightbutt.SetActive(true);

        // Starts animation method
        moveTo(4);

        // Sets the stats mode and runs the stats method
        stats.mode = 1;
        stats.updateStats();
    }

    // Animation method creates animation frames and sets mode and camera position vectors
    public void moveTo(int i)
    {
        oldCampos = cam.transform.position;
        animCounter = 20;
        mode = i;

    }

    // Method called when searching for a certain date within the database
    // Returns the rows values if they exist
    // Returns default values if it does not exist in the database
    public (float, string) searchfor(DateTime date)
    {

        // Reads the sql table into the entries list
        sqlscr.read();

        // Sets default values
        float val = 0f;
        string notes = "";


        // For each item in the entries list
        foreach (Entry e in entries)
        {

            // If entry's date is the same number of full days away from 1/1/2022 as the searched for day
            if (e.getDateSince2022() == (date - (new DateTime(2022, 1, 1).Date)).Days)
            {
                // Sets the values to match the date in the database
                val = e.getVal();
                notes = e.getNotes();
            }
        }

        // Returns the new values
        return (val, notes);
    }


    // Method called when a day is changed
    public void add()
    {

        // Calles the add method sql which adds the new date to the database
        sqlscr.add(selectedDate, slider.getValue(), notesText.text);

        // Updates textboxes to match the new data
        valText.SetText(searchfor(selectedDate).Item1.ToString().Equals("0")? "?": searchfor(selectedDate).Item1.ToString());
        notesText.text = (searchfor(selectedDate).Item2.ToString());
    }

    // Method called when navigating to a previous day
    public void navBack()
    {
        // Lowers the selected date by one day and updates text boxes
        selectedDate = selectedDate.Subtract(TimeSpan.FromDays(1));
        dayText.SetText(selectedDate.ToString("dddd dd"));
        monthText.SetText(selectedDate.ToString("MMMM"));
        valText.SetText(searchfor(selectedDate).Item1.ToString().Equals("0") ? "?" : searchfor(selectedDate).Item1.ToString());
        notesText.text = (searchfor(selectedDate).Item2.ToString());
        slider.setValue((int)(searchfor(selectedDate).Item1 * 2f));
    }

    // Method called when navigating to a future day
    public void navForwards()
    {
        // Raises the selected date by one day and updates the text boxes
        selectedDate = selectedDate.AddDays(1);
        dayText.SetText(selectedDate.ToString("dddd dd"));
        monthText.SetText(selectedDate.ToString("MMMM"));
        valText.SetText(searchfor(selectedDate).Item1.ToString().Equals("0") ? "?" : searchfor(selectedDate).Item1.ToString());
        notesText.text = (searchfor(selectedDate).Item2.ToString());
        slider.setValue((int)(searchfor(selectedDate).Item1*2f));
    }
    
    // Method called when navigating to a previous month in the calendar page
    public void navMonBack()
    {
        monSelDate = monSelDate.AddMonths(-1);
        generateCal(monSelDate);

    }

    // Method called when navigating to a future month in the calendar page
    public void navMonForwards()
    {
        monSelDate = monSelDate.AddMonths(1);
        generateCal(monSelDate);

    }

    // Method called when deleting the current day in the day page
    public void deleteCurrent()
    {

        // Runs the delete method withe the selected date
        sqlscr.delete(selectedDate);

        // Resets the textboxes and sliders
        dayText.SetText(selectedDate.ToString("dddd dd"));
        monthText.SetText(selectedDate.ToString("MMMM"));
        valText.SetText(searchfor(selectedDate).Item1.ToString().Equals("0") ? "?" : searchfor(selectedDate).Item1.ToString());
        notesText.text = (searchfor(selectedDate).Item2.ToString());
        slider.setValue((int)(searchfor(selectedDate).Item1 * 2f));
    }


    // Method called when generating a calendar for a selected month
    public void generateCal(DateTime seldate)
    {
        // Sets teh textboxes to match the selected month and year
        calYear.text = seldate.ToString("yyy");
        calMonth.text = seldate.ToString("MMMM");

        // Finds the length of the month and the day of the first day
        int monthLen = DateTime.DaysInMonth(seldate.Year, seldate.Month);
        string startingday = new DateTime(seldate.Year, seldate.Month, 1).ToString("ddd");

        int startNum = 0;

        // Sets the start number based off which day the first day of the month is
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

        // For each possible box
        for (int i = 0; i < 42; i++)
        {
            // For days before the first day in the month but in the same week
            if (i < startNum)
            {
                // Sets default transparent values and disables buttons
                days[i].date = new DateTime();
                days[i].num.text = "";
                days[i].dayIntl = 0;
                days[i].butt.interactable = false;
                days[i].im.color = new Color(1, 1, 1, 1);
            }

            // For days in the month
            else if (i < startNum + monthLen)
            {
                
                // Sets the Day object to have matching date values and sets color and text of the button
                days[i].date = new DateTime(seldate.Year, seldate.Month, (i - startNum + 1));
                days[i].num.text = (i - startNum + 1).ToString();
                days[i].dayIntl = searchfor(days[i].date).Item1;
                days[i].updateColor();
                days[i].butt.interactable = true;
            }
            else
            {
                // Sets default transparent values and disables buttons
                days[i].date = new DateTime();
                days[i].num.text = "";
                days[i].dayIntl = 0;
                days[i].butt.interactable = false;
                days[i].im.color = new Color(1, 1, 1, 1);
            }
        }
    }
}
