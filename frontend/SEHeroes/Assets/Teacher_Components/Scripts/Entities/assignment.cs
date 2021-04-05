using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;
using System.Linq;

public class Assignment{
    public string assignmentName;
    [System.NonSerialized]
    public string assignmentId;
    public int tries;
    public AsgDate startDate;      // will change
    public AsgDate dueDate;        // will change
    public List<string> questions;  // represents list of questionId 
    public Assignment(){
        assignmentName = "Testing";
        startDate = new AsgDate();
        dueDate = new AsgDate();
        tries = 2;
        questions = new List<string>();
    }
    public Assignment(JSONNode jsonAsg){
        assignmentName = jsonAsg["assignmentName"];
        assignmentId = jsonAsg["assignmentId"];
        tries = jsonAsg["tries"];
        startDate = new AsgDate(jsonAsg["startDate"]);
        dueDate = new AsgDate(jsonAsg["dueDate"]);
        questions = new List<string>();
        for(int j = 0; j < jsonAsg["questions"].Count; j++)
            questions.Add(jsonAsg["questions"][j]);
    }
}