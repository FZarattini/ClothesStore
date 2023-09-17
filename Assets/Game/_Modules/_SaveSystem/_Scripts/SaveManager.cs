using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public static Action OnGameLoaded;

    [Title("References")]
    [SerializeField] ItemListSO _playerInventory;
    [SerializeField] ItemListSO _storeStock;
    [SerializeField] ItemListSO _allItems;
    [SerializeField, ReadOnly] int currentCurrency;

    private const string PLAYER_CURRENCY_SAVED = "PLAYER_CURRENCY_SAVED";
    private const string PLAYER_CURRENCY = "PLAYER_CURRENCY";
    private const string PLAYER_INVENTORY_QUANTITY = "PLAYER_INVENTORY_QUANTITY";
    private const string STORE_STOCK_QUANTITY = "STORE_STOCK_QUANTITY";
    private const string PLAYER_INVENTORY_ITEM = "PLAYER_INVENTORY_ITEM";
    private const string STORE_STOCK_ITEM = "STORE_STOCK_ITEM";
    private const string PLAYER_INVENTORY_SAVED = "PLAYER_INVENTORY_SAVED";
    private const string STORE_STOCK_SAVED = "STORE_STOCK_SAVED";

    public int CurrentCurrency => currentCurrency;

    public bool SavedInventory
    {
        get => PlayerPrefs.GetInt(PLAYER_INVENTORY_SAVED, 0) == 1; 
    }

    public bool SavedStoreStock
    {
        get => PlayerPrefs.GetInt(STORE_STOCK_SAVED, 0) == 1;
    }

    public bool SavedCurrency
    {
        get => PlayerPrefs.GetInt(PLAYER_CURRENCY_SAVED, 0) == 1;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentCurrency = 0;
        LoadGameData();
    }

    private void OnEnable()
    {
        PlayerController.OnCurrencyChanged += UpdateCurrentCurrency;
        PauseController.OnSaveGame += SaveGameData;
        PauseController.OnClearGameData += ClearGameData;
    }

    private void OnDisable()
    {
        PlayerController.OnCurrencyChanged -= UpdateCurrentCurrency;
        PauseController.OnSaveGame -= SaveGameData;
        PauseController.OnClearGameData -= ClearGameData;
    }

    void UpdateCurrentCurrency(int value)
    {
        currentCurrency = value;
    }

    void SaveGameData()
    {
        SavePlayerCurrency();
        SaveInventory();
        SaveStoreStock();
    }

    void ClearGameData()
    {
        PlayerPrefs.DeleteAll();
    }

    void LoadGameData()
    {
        LoadPlayerCurrency();
        LoadInventory();
        LoadStoreStock();

        OnGameLoaded?.Invoke();
    }

    #region Player Currency Methods
    void SavePlayerCurrency()
    {
        PlayerPrefs.SetInt(PLAYER_CURRENCY, currentCurrency);
        PlayerPrefs.SetInt(PLAYER_CURRENCY_SAVED, 1);
    }

    public void  LoadPlayerCurrency()
    {
        currentCurrency = PlayerPrefs.GetInt(PLAYER_CURRENCY);
    }

    #endregion Player Currency Methods

    #region Inventory Methods

    void SaveInventory()
    {
        ClearInventoryData();

        var itemQuantity = _playerInventory.ItemsList.Count;

        PlayerPrefs.SetInt(PLAYER_INVENTORY_QUANTITY, itemQuantity);

        for (int i = 0; i < itemQuantity; i++)
        {
            PlayerPrefs.SetString($"{PLAYER_INVENTORY_ITEM}_{i}", _playerInventory.ItemsList[i].id);
        }

        PlayerPrefs.SetInt(PLAYER_INVENTORY_SAVED, 1);
    }

    void ClearInventoryData()
    {
        var quantity = PlayerPrefs.GetInt(PLAYER_INVENTORY_QUANTITY);

        PlayerPrefs.DeleteKey(PLAYER_INVENTORY_QUANTITY);

        for (int i = 0; i < quantity; i++)
        {
            PlayerPrefs.DeleteKey($"{PLAYER_INVENTORY_ITEM}_{i}");
        }
    }


    void LoadInventory()
    {
        List<ClothingItemData> items = new List<ClothingItemData>();
        var quantity = PlayerPrefs.GetInt(PLAYER_INVENTORY_QUANTITY);

        for (int i = 0; i < quantity; i++)
        {
            var id = PlayerPrefs.GetString($"{PLAYER_INVENTORY_ITEM}_{i}", "");

            if (id == "") return;

            for (int j = 0; j < _allItems.ItemsList.Count; j++)
            {
                if (_allItems.ItemsList[j].id == id)
                {
                    items.Add(_allItems.ItemsList[j]);
                }
            }
        }

        _playerInventory.ItemsList = new List<ClothingItemData>(items);
    }

    #endregion Inventory Methods

    #region Store Stock Methods

    void SaveStoreStock()
    {
        ClearStoreStockData();

        var itemQuantity = _storeStock.ItemsList.Count;

        PlayerPrefs.SetInt(STORE_STOCK_QUANTITY, itemQuantity);

        for (int i = 0; i < itemQuantity; i++)
        {
            PlayerPrefs.SetString($"{STORE_STOCK_ITEM}_{i}", _storeStock.ItemsList[i].id);
        }

        PlayerPrefs.SetInt(STORE_STOCK_SAVED, 1);
    }

    void LoadStoreStock()
    {
        List<ClothingItemData> items = new List<ClothingItemData>();
        var quantity = PlayerPrefs.GetInt(STORE_STOCK_QUANTITY);

        for (int i = 0; i < quantity; i++)
        {
            var id = PlayerPrefs.GetString($"{STORE_STOCK_ITEM}_{i}", "");

            if (id == "") return;

            for (int j = 0; j < _allItems.ItemsList.Count; j++)
            {
                if (_allItems.ItemsList[j].id == id)
                {
                    items.Add(_allItems.ItemsList[j]);
                }
            }
        }

        _storeStock.ItemsList = new List<ClothingItemData>(items);
    }

    void ClearStoreStockData()
    {
        var quantity = PlayerPrefs.GetInt(STORE_STOCK_QUANTITY);

        PlayerPrefs.DeleteKey(STORE_STOCK_QUANTITY);

        for (int i = 0; i < quantity; i++)
        {
            PlayerPrefs.DeleteKey($"{STORE_STOCK_ITEM}_{i}");
        }
    }

    #endregion Store Stock Methods
}
