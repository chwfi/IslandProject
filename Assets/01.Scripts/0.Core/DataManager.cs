using System.Collections;
using UnityEngine;
using Firebase.Database;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

public class DataManager : MonoSingleton<DataManager>
{
    public string userId;

    public void OnDeleteData(string root)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        reference.Child(userId).Child(root).RemoveValueAsync();
    }

    public void OnSaveData<T>(T data, string id, string baseRoot)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        string json = JsonConvert.SerializeObject(data, new JsonSerializerSettings 
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        });

        reference.Child(userId).Child(baseRoot).Child(id).SetRawJsonValueAsync(json);

        Debug.Log($"saved: {json}");
    }

    public void OnLoadData<T>(string id, string baseRoot, Action<T> callback, Action failed)
    {
        StartCoroutine(LoadDataCoroutine(id, baseRoot, callback, failed));
    }

    public void OnLoadFromDatabase<T>(string baseRoot, Action<List<T>> callback)
    {
        StartCoroutine(LoadFromDatabaseCoroutine(baseRoot, callback));
    }

    private IEnumerator LoadFromDatabaseCoroutine<T>(string baseRoot, Action<List<T>> callback)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        var DBTask = reference.Child(userId).Child(baseRoot).GetValueAsync();

        yield return new WaitUntil(() => DBTask.IsCompleted);

        if (DBTask.Result.Value != null)
        {
            List<T> loadedDataList = new List<T>();
            foreach (DataSnapshot childSnapshot in DBTask.Result.Children)
            {
                string jsonData = childSnapshot.GetRawJsonValue();
                T loadedData = JsonConvert.DeserializeObject<T>(jsonData);
                loadedDataList.Add(loadedData);
            }

            callback?.Invoke(loadedDataList);
            Debug.Log($"Loaded {loadedDataList.Count} items from {baseRoot}");
        }
        else
        {
            Debug.LogWarning($"No data found at {baseRoot}");
            callback?.Invoke(null);
        }
    }

    private IEnumerator LoadDataCoroutine<T>(string id, string baseRoot, Action<T> callback, Action failed)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        var DBTask = reference.Child(userId).Child(baseRoot).Child(id).GetValueAsync();

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
        else
        {
            failed?.Invoke();
        }
    }
}