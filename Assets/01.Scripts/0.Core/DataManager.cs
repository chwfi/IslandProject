using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class DataManager : MonoSingleton<DataManager>
{
    private DatabaseReference _ref;
    public string userId;

    void Start()
    {
        _ref = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void OnSaveData<T>(T data)
    {
        string json = JsonUtility.ToJson(data);
        _ref.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

    public void OnLoadData<T>(T data)
    {
        StartCoroutine(LoadDataCoroutine(data));
    }

    private IEnumerator LoadDataCoroutine(dynamic dt)
    {
        var data = _ref.Child("users").Child(userId).GetValueAsync();
        yield return new WaitUntil(predicate: () => data.IsCompleted);

        print("process is completed");

        DataSnapshot snapshot = data.Result;
        string jsonData = snapshot.GetRawJsonValue();

        if (jsonData != null)
        {
            print("data found");
            dt = JsonUtility.FromJson<dynamic>(jsonData);
        }
        else
        {
            print("data not fonund");
        }
    }
}