using System;
using Firebase.Auth;
using UnityEngine;

public class AuthManager : MonoSingleton<AuthManager>
{
    private FirebaseAuth _auth;
    private FirebaseUser _user;

    public string UserID => _user.UserId;
    
    public Action<bool> LoginState;

    public void Init()
    {
        _auth = FirebaseAuth.DefaultInstance;

        if (_auth.CurrentUser != null)
        {
            Logout();
        }

        _auth.StateChanged += OnChanged;
    }

    private void OnChanged(object sender, EventArgs e)
    {
        if (_auth.CurrentUser != _user)
        {
            bool signed = _auth.CurrentUser != _user && _auth.CurrentUser != null;
            if (!signed && _user != null)
            {
                Debug.Log("Log out");
                LoginState?.Invoke(false);
            }

            _user = _auth.CurrentUser;
            if (signed)
            {
                Debug.Log("Log in");
                LoginState?.Invoke(true);
            }
        }
    }

    public void Create(string email, string password)
    {
        _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Create cancel");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to login");
                return;
            }
;
            AuthResult authResult = task.Result;
            FirebaseUser user = authResult.User;
            Debug.Log("로그인 완료");
        });
    }

    public void Login(string email, string password)
    {
        _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Cancel login");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to login");
                return;
            }

            AuthResult authResult = task.Result;
            FirebaseUser user = authResult.User;
            Debug.Log("로그인 완료");
        });
    }

    public void Logout()
    {
        _auth.SignOut();
        Debug.Log("Sign Out");
    }
}
