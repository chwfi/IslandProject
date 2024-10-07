using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class DatabaseTest : MonoBehaviour
{
    DatabaseReference m_Reference;

    void Start()
    {
        m_Reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SetData()
    {
        string json = "hi";
        m_Reference.Child("participants").SetValueAsync(json);
    }
}