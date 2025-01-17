﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using SimpleJSON;
using System.Linq;

public class LoginController : MonoBehaviour
{
    private string matricNo;
    private string password;

    void getForm() {
        matricNo = InputFieldController.matriculationNo;
        password = InputFieldController.password;
    }

    public void register() {
        Debug.Log("Register Account Button Clicked!");
        SceneManager.LoadScene(sceneName:"CharacterSelection");
    }

    public void login() {
        Debug.Log("Login Button Clicked!");
        getForm();
        StartCoroutine(APIController.RequestLogin(matricNo, password));
    }

    public static void loginSuccessful() {
            SceneManager.LoadScene(sceneName:"MainMenu");
    }

    public void exitGame() {
        Application.Quit();
    }

}
