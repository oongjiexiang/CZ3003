﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Assignment_Edit_Script : MonoBehaviour
{
    // variables
    private Assignment asg = Assignment_Edit_Meta_Script.editAsg;
    private AssignmentQuestion current_question;
    private List<AssignmentQuestion> asgQuestionList;
    private int cur;

    // display
    public GameObject panelObject;
    public GameObject mainContentPanel;
    private Transform entryContainer;
    private GameObject popUp;
    private Dropdown dropdownAnswer;
    

    // Start is called before the first frame update
//     void Awake()
//     {
//         // display
//         popUp = mainContentPanel.transform.Find("Panel_Messages").gameObject;
//         entryContainer = panelObject.transform.Find("Panel_Question_Creation");
//         dropdownAnswer = entryContainer.Find("Dropdown_Answer").GetComponent<Dropdown>();
//         panelObject.transform.Find("Button_Delete").GetComponent<Button>().interactable = false;
//         popUp.SetActive(false);
//         // panelObject.transform.Find("Button_Cancel").GetComponent<Button>().onClick.AddListener(() => clickCancel());

//         // variables: questions
//         asgQuestionList = editAsg.questions;    // need to fetch editAsg
//         current_question = asgQuestionList[0];
//         cur = 0;
//         populateFields(current_question, false);
//     }
//     public void ClickSaveAndComplete()
//     {
//         if(validateFields()){
//             popUp.SetActive(true);
//             popUp.transform.Find("Popup_Create").gameObject.SetActive(true);
//         }
//         else{
//             popupQuestionIncomplete();
//         }
//     }
//     public void ClickSaveAndPrevious(){
//         if(validateFields()){
//             current_question = asgQuestionList[--cur];
//             if(cur > 0) populateFields(current_question, true);
//             else populateFields(current_question, false);
//         }
//         else{
//             popupQuestionIncomplete();
//         }
//     }
//     public void ClickSaveAndNext(){
//         if(validateFields()){
//             try{
//                 current_question = asgQuestionList[cur+1];
//                 populateFields(current_question, true);
//                 cur++;
//             }
//             catch(ArgumentOutOfRangeException){
//                 current_question = new AssignmentQuestion();
//                 asgQuestionList.Add(current_question);
//                 cur++;
//                 populateFields(current_question, true);
//             }
//         }
//         else{
//             popupQuestionIncomplete();
//         }
//     }
//     public void ClickClear()
//     {
//         entryContainer.Find("InputField_Question").GetComponent<InputField>().text = "";
//         entryContainer.Find("InputField_A").GetComponent<InputField>().text = "";
//         entryContainer.Find("InputField_B").GetComponent<InputField>().text = "";
//         entryContainer.Find("InputField_C").GetComponent<InputField>().text = "";
//         entryContainer.Find("InputField_D").GetComponent<InputField>().text = "";
//         dropdownAnswer.value = 0;
//     }
//     public void ClickDelete(){
//         popUp.SetActive(true);
//         popUp.transform.Find("Popup_Delete").gameObject.SetActive(true);
//     }
//     public void confirmSaveAndCreate(){ // do at 7pm
//         API_Assignment asg_conn = new API_Assignment();
//         print(new API_Connection());
//         foreach(var asgQuestion in asgQuestionList){
//             StartCoroutine(asg_conn.saveBack(asgQuestion));
//             asg.questions.Add(asgQuestion.assignmentQuestionId);
//         }
//         StartCoroutine(asg_conn.saveBack(asg));
//         print("Whole process is successful");
//         SceneManager.LoadScene("Assignments");
//     }
//     public void exitSaveAndCreate(){
//         popUp.transform.Find("Popup_Create").gameObject.SetActive(false);
//         popUp.gameObject.SetActive(false);
//     }
//     public void popupQuestionIncompleteAcknowledge(){
//         popUp.transform.Find("Popup_Incomplete").gameObject.SetActive(false);
//         popUp.gameObject.SetActive(false);
//     }
//     public void confirmDelete(){ //
//         exitDelete();
//         if(cur < asgQuestionList.Count - 1){
//             asgQuestionList.RemoveAt(cur);
//             current_question = asgQuestionList[cur];
//             if(cur == 0) populateFields(current_question, false);
//             else populateFields(current_question, true);
//         }
//         else{
//             asgQuestionList.RemoveAt(cur);
//             current_question = asgQuestionList[--cur];
//             if(cur == 0) populateFields(current_question, false);
//             else populateFields(current_question, true);
//         }
//     }
//     public void exitDelete(){
//         popUp.transform.Find("Popup_Delete").gameObject.SetActive(false);
//         popUp.gameObject.SetActive(false);
//     }
//     private void setPopupMessage(string message){   // helper function to set popup's message easily
//         popUp.transform.Find("Popup_Incomplete").Find("Text").GetComponent<Text>().text = message;
//     }
//     private bool validateFields(){
//         current_question.question = entryContainer.Find("InputField_Question").GetComponent<InputField>().text;
//         current_question.answer[0] = entryContainer.Find("InputField_A").GetComponent<InputField>().text;
//         current_question.answer[1] = entryContainer.Find("InputField_B").GetComponent<InputField>().text;
//         current_question.answer[2] = entryContainer.Find("InputField_C").GetComponent<InputField>().text;
//         current_question.answer[3] = entryContainer.Find("InputField_D").GetComponent<InputField>().text;
        
//         // There must be a question string
//         if(current_question.question == ""){
//             setPopupMessage("There is no question to be saved");
//             return false;
//         }

//         // All four answers must be given
//         for(int i = 0; i < current_question.answer.Count; i++){
//             if(current_question.answer[i] == ""){
//                 setPopupMessage("All four answers must be given");
//                 return false;
//             }
//         }
//         for(int i = 0; i < current_question.answer.Count-1; i++){
//             for(int j = i+1; j < current_question.answer.Count; j++){
//                 if(current_question.answer[i].Equals(current_question.answer[j])){
//                     setPopupMessage("All answers must be unique");
//                     return false;
//                 }
//             }
//         }
//         // Score must be integer
//         try{
//             current_question.score = int.Parse(entryContainer.Find("InputField_Score").GetComponent<InputField>().text);
//         }
//         catch(FormatException){
//             setPopupMessage("Score must be a whole number");
//             return false;
//         }
//         if(dropdownAnswer.value == 0){
//             setPopupMessage("Please choose an answer");
//             return false;
//         }
//         current_question.correctAnswer = dropdownAnswer.value-1;
//         return true;
//     }
//     private void populateFields(AssignmentQuestion current_question, bool buttonInteractable)
//     {
//         entryContainer.Find("InputField_Question").GetComponent<InputField>().text = current_question.question;
//         entryContainer.Find("InputField_A").GetComponent<InputField>().text = current_question.answer[0];
//         entryContainer.Find("InputField_B").GetComponent<InputField>().text = current_question.answer[1];
//         entryContainer.Find("InputField_C").GetComponent<InputField>().text = current_question.answer[2];
//         entryContainer.Find("InputField_D").GetComponent<InputField>().text = current_question.answer[3];
//         entryContainer.Find("Text_Question").GetComponent<Text>().text = "Question " + (cur + 1).ToString();
//         entryContainer.Find("InputField_Score").GetComponent<InputField>().text = current_question.score.ToString();
//         if(current_question.correctAnswer == -1) dropdownAnswer.value = 0;
//         else dropdownAnswer.value = current_question.correctAnswer + 1;
//         if(asgQuestionList.Count > 1){
//             panelObject.transform.Find("Button_Delete").GetComponent<Button>().interactable = true;
//         }
//         else{
//             panelObject.transform.Find("Button_Delete").GetComponent<Button>().interactable = false;
//         }
//         panelObject.transform.Find("Button_PrevQ").GetComponent<Button>().interactable = buttonInteractable;
//     }
//     private void popupQuestionIncomplete(){
//         popUp.SetActive(true);
//         popUp.transform.Find("Popup_Incomplete").gameObject.SetActive(true);
//     }
// }

}