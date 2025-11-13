using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class ShopItemUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image itemIconImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI itemPriceText;
    public Button buyButton;

    private ShopItem _currentItem; 

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
        if (itemPriceText != null) itemPriceText.text = item.itemPrice.ToString();

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners(); 
            buyButton.onClick.AddListener(OnBuyButtonClicked);
        }
    }

    /// <summary>
    /// Chamado quando o botão de compra deste item é clicado.
    /// </summary>
    private void OnBuyButtonClicked()
    {
        if (_currentItem != null && StoreController.Instance != null)
        {
            StoreController.Instance.PurchaseItem(_currentItem);
        }
        else
        {
            Debug.LogError("ShopItemUI: Item nulo ou StoreController.Instance não encontrado!");
        }
    }

    /// <summary>
    /// Atualiza o estado da UI (ex: desabilitar botão se não houver dinheiro).
    /// Esta função pode ser chamada pelo StoreController quando o dinheiro muda.
    /// </summary>
    public void UpdateUIState()
    {
        if (_currentItem != null && StoreController.Instance != null && buyButton != null)
        {
            buyButton.interactable = StoreController.Instance.CanAffordItem(_currentItem);

            
        }
    }
}