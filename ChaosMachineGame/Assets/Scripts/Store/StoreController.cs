using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class StoreController : MonoBehaviour
{

    public static StoreController Instance { get; private set; }

    [Header("Shop Items")]
    [Tooltip("Lista de todos os itens disponíveis na loja.")]
    public List<ShopItem> availableItems = new List<ShopItem>();

    [Header("UI References")]
    [Tooltip("O container (GameObject pai) onde os slots de item da loja serão instanciados.")]
    public Transform shopItemsParent;
    [Tooltip("O Prefab de UI que representa um único item na loja (com ShopItemUI script).")]
    public GameObject shopItemUIPrefab;

    [Header("Events")]
    [Tooltip("Evento disparado quando o saldo do jogador é atualizado.")]
    public UnityEvent<int> OnCurrencyUpdated; 
    [Tooltip("Evento disparado para exibir uma mensagem ao jogador (ex: 'Item comprado!').")]
    public UnityEvent<string> OnMessageDisplayed; 
    private int _playerCurrency = 1000; 

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
        OnCurrencyUpdated?.Invoke(_playerCurrency);
    }

    /// <summary>
    /// Inicializa a UI da loja, instanciando os prefabs de item.
    /// </summary>
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
        foreach (ShopItem item in availableItems)
        {
            GameObject itemGO = Instantiate(shopItemUIPrefab, shopItemsParent);
            ShopItemUI itemUI = itemGO.GetComponent<ShopItemUI>();

            if (itemUI != null)
            {
                itemUI.SetupItem(item);
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
    public void PurchaseItem(ShopItem item)
    {
        if (item == null)
        {
            Debug.LogError("Tentativa de comprar item nulo.");
            return;
        }

        if (_playerCurrency >= item.itemPrice)
        {
            _playerCurrency -= item.itemPrice;

            item.OnItemPurchased?.Invoke();

            OnCurrencyUpdated?.Invoke(_playerCurrency);

            OnMessageDisplayed?.Invoke(item.purchaseSuccessMessage);

            Debug.Log($"Item '{item.itemName}' comprado por {item.itemPrice}. Saldo restante: {_playerCurrency}");
        }
        else
        {
            OnMessageDisplayed?.Invoke(item.insufficientFundsMessage);
            Debug.LogWarning($"Dinheiro insuficiente para comprar '{item.itemName}'. Preço: {item.itemPrice}, Saldo: {_playerCurrency}");
        }
    }

    /// <summary>
    /// Adiciona dinheiro ao saldo do jogador.
    /// </summary>
    /// <param name="amount">Quantidade de dinheiro a adicionar.</param>
    public void AddCurrency(int amount)
    {
        _playerCurrency += amount;
        OnCurrencyUpdated?.Invoke(_playerCurrency);
        OnMessageDisplayed?.Invoke($"+{amount} dinheiro!");
        Debug.Log($"Adicionado {amount} dinheiro. Novo saldo: {_playerCurrency}");
    }

    /// <summary>
    /// Retorna o saldo atual do jogador.
    /// </summary>
    public int GetPlayerCurrency()
    {
        return _playerCurrency;
    }

    /// <summary>
    /// Verifica se o jogador pode pagar por um item sem realmente comprá-lo.
    /// </summary>
    public bool CanAffordItem(ShopItem item)
    {
        return _playerCurrency >= item.itemPrice;
    }

    /// <summary>
    /// Recarrega os itens da loja (útil se itens mudam dinamicamente).
    /// </summary>
    public void ReloadShopItems()
    {
        InitializeShopUI();
    }

    /// <summary>
    /// Ativa/desativa a UI da loja.
    /// </summary>
    public void ToggleShopUI(bool activate)
    {
        if (shopItemsParent != null && shopItemsParent.parent != null)
        {
            shopItemsParent.parent.gameObject.SetActive(activate);
            OnMessageDisplayed?.Invoke(activate ? "Loja Aberta!" : "Loja Fechada!");
        }
    }

    /// <summary>
    /// Retorna um item pelo seu ID.
    /// </summary>
    public ShopItem GetItemByID(string itemID)
    {
        return availableItems.Find(item => item.itemID == itemID);
    }
}