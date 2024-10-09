using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;
using Newtonsoft.Json;

public class DataManager : MonoSingleton<DataManager>
{
    private DatabaseReference _ref;
    public string userId;

    void Start()
    {
        //_ref = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void OnSaveData<T>(T data, string id)
    {
        string json = JsonConvert.SerializeObject(data, new JsonSerializerSettings 
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        });

        _ref.Child(userId).Child(id).SetRawJsonValueAsync(json);

        Debug.Log($"saved: {json}");
    }

    public void OnLoadData<T>(string id, Action<T> callback) where T : new()
    {
        StartCoroutine(LoadDataCoroutine(id, callback));
    }

    private IEnumerator LoadDataCoroutine<T>(string id, Action<T> callback) where T : new()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        var DBTask = reference.Child(userId).Child(id).GetValueAsync();

        yield return new WaitUntil(() => DBTask.IsCompleted);
        
        if (DBTask.Result.Value != null)
        {
            DataSnapshot snapshot = DBTask.Result;
            string jsonData = snapshot.GetRawJsonValue();

            if (jsonData != null)
            {
                Debug.Log($"data found: {jsonData}");
                T loadedData = JsonConvert.DeserializeObject<T>(jsonData); 
                callback?.Invoke(loadedData);
            }
        }
    }
}