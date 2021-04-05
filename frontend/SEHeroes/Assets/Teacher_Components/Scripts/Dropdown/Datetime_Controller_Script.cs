﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Datetime_Controller_Script : MonoBehaviour
{
    const int RANGE_YEAR = 5;

    public GameObject HandleDate;
    public GameObject HandleTime;
    //public GameObject mainContentPanel;
    private List<Dropdown> dropdowns = new List<Dropdown>();
    //private GameObject popUp;

    void Awake()
    {
        dropdowns.Add(HandleDate.transform.Find("Dropdown_Year").GetComponent<Dropdown>());
        dropdowns.Add(HandleDate.transform.Find("Dropdown_Month").GetComponent<Dropdown>());
        dropdowns.Add(HandleDate.transform.Find("Dropdown_Day").GetComponent<Dropdown>());
        dropdowns.Add(HandleTime.transform.Find("Dropdown_Hour").GetComponent<Dropdown>());
        dropdowns.Add(HandleTime.transform.Find("Dropdown_Minute").GetComponent<Dropdown>());

        List<List<string>> options = new List<List<string>>(5);
        for (int i = 0; i < 5; i++) options.Add(new List<string>());

        DateTime current = DateTime.Now;
        int year = int.Parse(current.ToString("yyyy"));
        int month = int.Parse(current.ToString("MM"));

        for (int i = 0; i <= RANGE_YEAR; i++)
        {
            options[0].Add((year + i).ToString());
        }
        for (int i = 0; i < 12; i++)
        {
            options[1].Add(current.AddMonths(i).ToString("MMM"));
        }
        for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
        {
            options[2].Add(i.ToString());
        }
        for (int i = 0; i < 24; i++)
        {
            options[3].Add(i.ToString("D2"));
        }
        for (int i = 0; i < 60; i += 5)
        {
            options[4].Add(i.ToString("D2"));
        }

        for(int i = 0; i < options.Count; i++)
        {
            dropdowns[i].ClearOptions();
            dropdowns[i].AddOptions(options[i]);
        }
    }
    public void FocusValues(AsgDate time){
        dropdowns[0].value = time.year - int.Parse(DateTime.Now.ToString("yyyy"));
        dropdowns[1].value = time.month;
        dropdowns[2].value = time.day - 1;
        dropdowns[3].value = time.hour;
        dropdowns[4].value = time.minute/5;
    }
}