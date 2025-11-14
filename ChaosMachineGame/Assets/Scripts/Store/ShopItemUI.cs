using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Collections.Generic;
using System.Collections;

public class ShopItemUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image itemIconImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI itemPriceText;
    public Button buyButton;

    [Header("Feedback UI (Opcional)")]
    [Tooltip("TextMeshProUGUI para exibir mensagens de sucesso/falha da compra. Pode ser o mesmo que itemPriceText se você preferir.")]
    public TextMeshProUGUI feedbackMessageText;
    [Tooltip("Cor do texto quando a compra é bem-sucedida.")]
    public Color successColor = Color.green;
    [Tooltip("Cor do texto quando não há dinheiro suficiente.")]
    public Color insufficientFundsColor = Color.red;
    [Tooltip("Cor padrão do texto do preço.")]
    public Color defaultPriceColor = Color.white;
    [Tooltip("Tempo em segundos que a mensagem de feedback ficará visível.")]
    public float feedbackDisplayTime = 2f;

    private ShopItem _currentItem;
    private EconomyManager _economyManager;
    private Coroutine _feedbackCoroutine; 

    private void OnEnable()
    {
        if (_economyManager == null)
        {
            _economyManager = EconomyManager.Instance;
        }

        if (_economyManager != null)
        {
            _economyManager.OnCurrencyUpdated.AddListener(OnCurrencyChanged);
        }
        UpdateUIState();
    }

    private void OnDisable()
    {
        if (_economyManager != null)
        {
            _economyManager.OnCurrencyUpdated.RemoveListener(OnCurrencyChanged);
        }
        if (_feedbackCoroutine != null)
        {
            StopCoroutine(_feedbackCoroutine);
            _feedbackCoroutine = null;
        }
        if (itemPriceText != null) itemPriceText.color = defaultPriceColor;
        if (feedbackMessageText != null) feedbackMessageText.text = ""; 
    }

    /// <summary>
    /// Configura a UI do slot com os dados de um ShopItem.
    /// </summary>
    /// <param name="item">O ShopItem a ser exibido.</param>
    public void SetupItem(ShopItem item)
    {
        _currentItem = item;

        if (itemIconImage != null) itemIconImage.sprite = item.itemIcon;
        if (itemNameText != null) itemNameText.text = item.itemName;
        if (itemDescriptionText != null) itemDescriptionText.text = item.itemDescription;

        if (itemPriceText != null)
        {
            itemPriceText.text = item.itemPrice.ToString();
            itemPriceText.color = defaultPriceColor; 
        }

        if (feedbackMessageText != null) feedbackMessageText.text = ""; 

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(OnBuyButtonClicked);
        }

        UpdateUIState(); 
    }

    /// <summary>
    /// Chamado quando o botão de compra deste item é clicado.
    /// </summary>
    private void OnBuyButtonClicked()
    {
        if (_currentItem != null && StoreController.Instance != null)
        {
            bool purchaseSuccessful = StoreController.Instance.PurchaseItem(_currentItem);

            if (purchaseSuccessful)
            {
                DisplayFeedbackMessage(_currentItem.purchaseSuccessMessage, successColor);
            }
            else
            {
                DisplayFeedbackMessage(_currentItem.insufficientFundsMessage, insufficientFundsColor);
            }
        }
        else
        {
            Debug.LogError("ShopItemUI: Item nulo ou StoreController.Instance não encontrado!");
        }
    }

    /// <summary>
    /// Atualiza o estado da UI (ex: desabilitar botão se não houver dinheiro).
    /// Esta função é chamada quando a moeda muda ou quando a UI da loja é atualizada.
    /// </summary>
    public void UpdateUIState()
    {
        if (_currentItem == null || _economyManager == null) return;

        bool canAfford = _economyManager.CanAfford(_currentItem.itemPrice);

        if (buyButton != null)
        {
            buyButton.interactable = canAfford;
        }
        if (itemPriceText != null)
        {
            itemPriceText.color = canAfford ? defaultPriceColor : insufficientFundsColor;
        }
    }

    /// <summary>
    /// Exibe uma mensagem de feedback na UI do item por um tempo limitado.
    /// </summary>
    /// <param name="message">A mensagem a ser exibida.</param>
    /// <param name="color">A cor da mensagem.</param>
    private void DisplayFeedbackMessage(string message, Color color)
    {
        if (feedbackMessageText == null)
        {
            Debug.LogWarning($"ShopItemUI: feedbackMessageText não atribuído no item '{_currentItem.itemName}'. Mensagem: {message}");
            return;
        }

        if (_feedbackCoroutine != null)
        {
            StopCoroutine(_feedbackCoroutine);
        }

        _feedbackCoroutine = StartCoroutine(ShowFeedbackAndHide(message, color));
    }

    private IEnumerator ShowFeedbackAndHide(string message, Color color)
    {
        string originalPriceText = itemPriceText != null ? itemPriceText.text : "";
        Color originalPriceColor = itemPriceText != null ? itemPriceText.color : defaultPriceColor;

        feedbackMessageText.text = message;
        feedbackMessageText.color = color;

        if (itemPriceText != null && itemPriceText != feedbackMessageText) 
        {
            itemPriceText.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(feedbackDisplayTime);

        feedbackMessageText.text = "";
        if (itemPriceText != null && itemPriceText != feedbackMessageText) 
        {
            itemPriceText.gameObject.SetActive(true);
            UpdateUIState();
        }
        else if (itemPriceText != null && itemPriceText == feedbackMessageText) 
        {
            itemPriceText.text = _currentItem.itemPrice.ToString();
            UpdateUIState(); 
        }

        _feedbackCoroutine = null;
    }


    /// <summary>
    /// Método chamado quando o evento OnCurrencyUpdated do EconomyManager é disparado.
    /// </summary>
    /// <param name="newCurrencyAmount">A nova quantia de dinheiro do jogador.</param>
    private void OnCurrencyChanged(int newCurrencyAmount)
    {
        UpdateUIState();
    }
}