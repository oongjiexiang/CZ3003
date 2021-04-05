﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;
using System.Linq;


public class Assignment_Entry_Script : MonoBehaviour
{
    private Transform entryTemplate;
    private Transform entryContainer;
    private GameObject popUp;
    List<Assignment> assignmentList;

    public Text noRecordLabel;
    public GameObject mainContentPanel;
    public GameObject content;

    public static string assignmentName;
    public static string assignmentId;

    IEnumerator Start()
    {
        popUp = mainContentPanel.transform.Find("Panel_Messages").gameObject;
        popUp.SetActive(false);
        yield return StartCoroutine(setAssignmentList());
        tableInitialize();
    }
    IEnumerator setAssignmentList()
    {
        API_Connection conn = new API_Connection();
        JSONNode jsonNode = null;
        // print("in main: " + json_receivedData.Count);
        yield return StartCoroutine(conn.GetData("Assignment", null, s => {
            jsonNode = JSON.Parse(s);
        }));
        assignmentList = new List<Assignment>();
        for(int i = 0; i < jsonNode.Count; i++){
            assignmentList.Add(new Assignment(jsonNode[i]));
        }
    }
    void tableInitialize()
    {
        entryContainer = content.transform.Find("Assignment_Entry_Container");
        entryTemplate = entryContainer.Find("Assignment_Entry_Template").transform;
        entryTemplate.gameObject.SetActive(false);
        float templateHeight = 50f;
        for (int i = 0; i < assignmentList.Count; i++)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryTransform.gameObject.SetActive(true);
            entryTransform.Find("Text_No").GetComponent<Text>().text = (i+1).ToString();
            entryTransform.Find("Text_Name").GetComponent<Text>().text = assignmentList[i].assignmentName;
            if(assignmentList[i].dueDate.time() <= DateTime.Now){
                entryTransform.Find("Text_Status").GetComponent<Text>().text = "Passed";
                entryTransform.Find("Text_Status").GetComponent<Text>().color = Color.red;    
            }
            else if(assignmentList[i].startDate.time() <= DateTime.Now){
                entryTransform.Find("Text_Status").GetComponent<Text>().text = "Ongoing till " + assignmentList[i].dueDate.printTime();
                entryTransform.Find("Text_Status").GetComponent<Text>().color = new Color(0.11f, 0.51f, 0.04f, 1.0f);
            }
            else{
                entryTransform.Find("Text_Status").GetComponent<Text>().text = "From " + assignmentList[i].startDate.printTime();
            }
            entryTransform.Find("Text_Name").Find("Button_Edit").Find("Text").GetComponent<Text>().text = "View";
            entryTransform.Find("Text_Name").Find("Button_Edit").GetComponent<Button>().onClick.AddListener(() => {
                assignmentName = entryTransform.Find("Text_Name").GetComponent<Text>().text;
                viewAssignment();
            });
            entryTransform.Find("Text_Name").Find("Button_Delete").Find("Text").GetComponent<Text>().text = "Delete";
            entryTransform.Find("Text_Name").Find("Button_Delete").GetComponent<Button>().onClick.AddListener(() => {
                assignmentName = entryTransform.Find("Text_Name").GetComponent<Text>().text;
                viewAssignment();
            });
            entryTransform.localScale = new Vector2(1, 1);
            entryTransform.localPosition = new Vector2(0, -templateHeight * i);
        }
        if (assignmentList.Count == 0)
        {
            noRecordLabel.gameObject.SetActive(true);
        }
        else
        {
            noRecordLabel.gameObject.SetActive(false);
        }
    }
    public void viewAssignment()
    {
        SceneManager.LoadScene("Assignments_View");
    }
    public void scheduleAssignment()
    {
        SceneManager.LoadScene("Assignments_Schedule");
    }
    public void deleteAssignment()
    {
        popUp.SetActive(true);
        popUp.transform.Find("Popup_Delete").gameObject.SetActive(true);   
        SceneManager.LoadScene("Assignments");
    }
    public void confirmDelete()
    {
        //
        SceneManager.LoadScene("Assignments");
    }
    public void exitDelete()
    {
        popUp.gameObject.SetActive(false);
    }
}
