using System;
using UnityEngine;

public class ItemSaveData
{
    public int coinAmount;
    public int crystalAmount;
    public int popularityAmount;
}

public class ItemManager : MonoSingleton<ItemManager>
{
    public Action<int> OnCoinUpdateUI;
    public Action<int> OnCrystalUpdateUI;

    private int _coin;
    private int _crystal;
    private int _popularity;

    public int Coin
    {
        get { return _coin; }
        set
        {
            _coin = value;
            OnCoinUpdateUI?.Invoke(_coin);
        }
    }
    public int Crystal
    {
        get { return _crystal; }
        set
        {
            _crystal = value;
            OnCrystalUpdateUI?.Invoke(_crystal);
        }
    }
    public int Popularity 
    {
        get { return _popularity; }
        set
        {
            _popularity = value;
            // action
        }
    }

    [SerializeField] private string _root;

    private void Start() 
    {
        OnLoadCostData();
    }
    
    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Coin += 500;
        }
    }

    private void OnApplicationQuit() 
    {
        Save();    
    }

    public void RecieveItem(RewardTypeEnum rewardType, int amount)
    {
        switch (rewardType)
        {
            case RewardTypeEnum.Coin:
                Coin += amount;
                break;
            case RewardTypeEnum.Popularity:
                Popularity += amount;
                break;
            case RewardTypeEnum.Crystal:
                Crystal += amount;
                break;
        }
    }

    public void UseCoin(int amount, Action success, Action fail)
    {
        if (Coin < amount)
        {
            PopupUIManager.Instance.AccessPopupUI("WarningPanel", true);
            fail?.Invoke();
            return;
        }
        else
        {
            Coin -= amount;
            success?.Invoke();
        }
    }

    public void Save()
    {
        DataManager.Instance.OnDeleteData(_root);
        DataManager.Instance.OnSaveData(ToSaveData(), "items", _root);
    }

    public void OnLoadCostData()
    {
        DataManager.Instance.OnLoadData<ItemSaveData>("items", _root, (loadedData) =>
        {
            if (loadedData != null)
            {
                LoadFrom(loadedData);
                OnCoinUpdateUI?.Invoke(_coin);
                OnCrystalUpdateUI?.Invoke(_crystal);
            }
            else
            {
                Debug.Log("Failed to load data");
            }
        }, () => Save());            
    }

    public ItemSaveData ToSaveData()
    {
        return new ItemSaveData
        {
            coinAmount = Coin,
            crystalAmount = Crystal   
        };
    }

    public void LoadFrom(ItemSaveData saveData)
    {
        _coin = saveData.coinAmount;
        _coin = saveData.crystalAmount;
    }
}
