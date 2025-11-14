
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoreController : MonoBehaviour
{
    public static StoreController Instance { get; private set; }

    [Header("Shop Items")]
    public List<ShopItem> availableItems = new List<ShopItem>();

    [Header("UI References")]
    public Transform shopItemsParent;
    public GameObject shopItemUIPrefab;

    private List<ShopItemUI> _instantiatedShopItems = new List<ShopItemUI>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        InitializeShopUI();
    }

    private void InitializeShopUI()
    {
        if (shopItemsParent == null || shopItemUIPrefab == null)
        {
            Debug.LogError("StoreController: shopItemsParent ou shopItemUIPrefab não está definido na UI.");
            return;
        }

        foreach (Transform child in shopItemsParent)
        {
            Destroy(child.gameObject);
        }
        _instantiatedShopItems.Clear();

        foreach (ShopItem item in availableItems)
        {
            GameObject itemGO = Instantiate(shopItemUIPrefab, shopItemsParent);
            ShopItemUI itemUI = itemGO.GetComponent<ShopItemUI>();

            if (itemUI != null)
            {
                itemUI.SetupItem(item);
                _instantiatedShopItems.Add(itemUI);
            }
            else
            {
                Debug.LogWarning($"StoreController: shopItemUIPrefab não tem um componente ShopItemUI. Item: {item.itemName}");
            }
        }
    }

    /// <summary>
    /// Tenta comprar um item específico.
    /// </summary>
    /// <param name="item">O item a ser comprado.</param>
    /// <returns>True se a compra foi bem-sucedida, false caso contrário.</returns>
    public bool PurchaseItem(ShopItem item) 
    {
        if (item == null)
        {
            Debug.LogError("Tentativa de comprar item nulo.");
            return false;
        }

        if (EconomyManager.Instance != null && EconomyManager.Instance.SpendCurrency(item.itemPrice))
        {
            item.OnItemPurchased?.Invoke();
            Debug.Log($"Item '{item.itemName}' comprado por {item.itemPrice}. Saldo restante: {EconomyManager.Instance.GetCurrentCurrency()}");
            return true;
        }
        else
        {
            Debug.LogWarning($"Dinheiro insuficiente para comprar '{item.itemName}'. Preço: {item.itemPrice}, Saldo: {EconomyManager.Instance.GetCurrentCurrency()}");
            return false; 
        }
    }

    public bool CanAffordItem(ShopItem item)
    {
        return EconomyManager.Instance != null && EconomyManager.Instance.CanAfford(item.itemPrice);
    }

    public void ReloadShopItems()
    {
        InitializeShopUI();
    }

    public void ToggleShopUI(bool activate)
    {
        if (shopItemsParent != null && shopItemsParent.parent != null)
        {
            shopItemsParent.parent.gameObject.SetActive(activate);

        }
    }

    public ShopItem GetItemByID(string itemID)
    {
        return availableItems.Find(item => item.itemID == itemID);
    }

}