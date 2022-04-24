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
    public List<Entry> entries = new List<Entry>();

    public TextMeshProUGUI dayText;
    public TextMeshProUGUI monthText;
    public TextMeshProUGUI valText;
    public TMP_InputField notesText;
    public sliderScr slider;
    

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        //newCampos = new Vector3(0, 6.5f, 0);
        newCampos = new Vector3(-1.5f, 2.4f, -1.3f);
        newCamrot = new Vector3(90, 0, 0);
        newCamFOV = 60;
        sqlscr.sqlStart();

        selectedDate = DateTime.Now;
        dayText.SetText(selectedDate.ToString("dddd dd"));
        valText.SetText(searchfor(selectedDate).Item1.ToString());
        notesText.text = (searchfor(selectedDate).Item2.ToString());
        slider.setValue((int)(searchfor(selectedDate).Item1 * 2f));


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown("1"))
        {
            newCampos = new Vector3(-1.5f, 2.4f, 1.3f);
            moveTo(1);
        }
        else if (Input.GetKeyDown("2"))
        {
            newCampos = new Vector3(1.5f, 2.4f, 1.3f);
            moveTo(2);
        }
        else if (Input.GetKeyDown("3"))
        {
            newCampos = new Vector3(-1.5f, 2.4f, -1.3f);
            moveTo(3);
        }
        else if (Input.GetKeyDown("4"))
        {
            newCampos = new Vector3(1.5f, 2.4f, -1.3f);
            moveTo(4);
        }


        if(animCounter > 0)
        {
            animCounter--;
        }
        if(animCounter == 1)
        {
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

    public void deleteCurrent()
    {
        sqlscr.delete(selectedDate);
        dayText.SetText(selectedDate.ToString("dddd dd"));
        monthText.SetText(selectedDate.ToString("MMMM"));
        valText.SetText(searchfor(selectedDate).Item1.ToString().Equals("0") ? "?" : searchfor(selectedDate).Item1.ToString());
        notesText.text = (searchfor(selectedDate).Item2.ToString());
        slider.setValue((int)(searchfor(selectedDate).Item1 * 2f));
    }


}
