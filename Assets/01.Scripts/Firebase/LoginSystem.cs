using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginSystem : MonoBehaviour
{
    public InputField email;
    public InputField password;

    public Text outputText;

    private void Start() 
    {
        AuthManager.Instance.LoginState += OnChangedState;
        AuthManager.Instance.Init();
    }

    private void OnChangedState(bool sgin)
    {
        outputText.text = sgin ? "로그인: " : "로그아웃: ";
        outputText.text += AuthManager.Instance.UserID;
    }

    public void Create()
    {
        string e = email.text;
        string p = password.text;

        AuthManager.Instance.Create(e, p);
    }

    public void LogIn()
    {
        AuthManager.Instance.Login(email.text, password.text);
    }

    public void LogOut()
    {
        AuthManager.Instance.Logout();
    }
}
