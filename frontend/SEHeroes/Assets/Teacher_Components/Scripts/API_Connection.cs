﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Linq;

public class API_Connection
{
    private readonly string baseURL = "https://seheroes.herokuapp.com/";
    // Start is called before the first frame update
    public IEnumerator GetData(string apiEndpoint, Dictionary<string, string> queryParams, System.Action<string> callback = null)   //Verified: Working
    {
        Debug.Log("Enter get data");
        string fullURL = baseURL + apiEndpoint;
        if(queryParams != null)
        {
            fullURL += "?";
            foreach(var param in queryParams)
            {
                fullURL += (param.Key + "=" + param.Value + "&");
            }
            fullURL += "\b";
        }
        UnityWebRequest response = UnityWebRequest.Get(fullURL);
        Debug.Log(response.url);
        yield return response.SendWebRequest();
        
        if (response.isNetworkError || response.isHttpError)
        {
            //Debug.LogError(assignmentInfoRequest.error);
            yield break;
        }
        if(callback != null){
            Debug.Log("response callback is not null");
            callback(response.downloadHandler.text);
        }
    }
    public IEnumerator PostData(string apiEndpoint, string json_toAdd, System.Action<string> callback = null){
        string fullURL = baseURL + apiEndpoint;
        // byte[] bytes_toAdd = System.Text.Encoding.UTF8.GetBytes(json_toAdd);
        Debug.Log(fullURL);

        var addRequest = new UnityWebRequest(fullURL, "POST");
        byte[] bytes_toAdd = System.Text.Encoding.UTF8.GetBytes(json_toAdd);
        addRequest.uploadHandler = (UploadHandler) new UploadHandlerRaw(bytes_toAdd);
        addRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        addRequest.SetRequestHeader("Content-Type", "application/json");
        yield return addRequest.SendWebRequest();
        Debug.Log("Status Code for post: " + addRequest.responseCode);
        if (addRequest.isNetworkError){
            Debug.Log(addRequest.error);
        }
        else{
            Debug.Log(addRequest.downloadHandler.text);
        }
        Debug.Log("post successful");
        if(callback != null){
            Debug.Log("response callback is not null");
            callback(addRequest.downloadHandler.text);
        }
    }
    public IEnumerator PutData(string apiEndpoint, string json_toModify, System.Action<string> callback = null)
    {   // PUT method: as extra compiler must be installed for dynamic type, pass json instead of whole object
        string fullURL = baseURL + apiEndpoint;
        UnityWebRequest putRequest;
        //creating instance of class
            // TutorialClass studentRemove = new TutorialClass();
            // studentRemove.matricNo = matricNum;

        //use the JsonUtility.ToJson method to serialize it (convert it) to the JSON format
        //string json_toModify = JsonUtility.ToJson(toModify);    
            // string json_remove_student = JsonUtility.ToJson(studentRemove);
        // json now contains: '{"matricNo":<student_matric_number>}'

        //To convert the JSON back into an object, use JsonUtility.FromJson:
        // myObject = JsonUtility.FromJson<MyClass>(json);

        //Convert the json format to byte form to prepare to pass to put api
        byte[] bytes_toModify = System.Text.Encoding.UTF8.GetBytes(json_toModify);

        //Using UnityWebRequest to do a put request to the database
        using (putRequest = UnityWebRequest.Put(fullURL, bytes_toModify))
        {
            putRequest.SetRequestHeader("Content-Type", "application/json");
            yield return putRequest.SendWebRequest();
            if (putRequest.isNetworkError)
            {
                Debug.Log(putRequest.error);
            }
            else
            {
                Debug.Log(putRequest.downloadHandler.text);
            }
        }
        // Debug.Log("Status Code for post: " + putRequest.responseCode);
        Debug.Log("put successful");
        if(callback != null){
            Debug.Log("response callback is not null");
            callback(putRequest.downloadHandler.text);
        }
    }

    public IEnumerator DeleteData(string apiEndpoint){  // Verified: Working
        string fullURL = baseURL + apiEndpoint;
        UnityWebRequest response = UnityWebRequest.Delete(fullURL);
        Debug.Log(response.url);
        yield return response.SendWebRequest();
        if (response.isNetworkError || response.isHttpError)
        {
            Debug.LogError(response.error);
            yield break;
        }
        Debug.Log("Delete successfully");
    }
}