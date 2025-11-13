using UnityEngine;
using UnityEngine.Events; 

[System.Serializable]
public class ShopItem
{
    [Tooltip("ID único do item (ex: 'sword_01', 'potion_hp').")]
    public string itemID;

    [Tooltip("Nome exibido na loja.")]
    public string itemName;

    [Tooltip("Descrição do item.")]
    [TextArea(3, 5)] 
    public string itemDescription;

    [Tooltip("Ícone do item (Sprite para a UI).")]
    public Sprite itemIcon;

    [Tooltip("Preço do item.")]
    public int itemPrice;

    [Tooltip("Mensagem exibida quando o item é comprado com sucesso.")]
    public string purchaseSuccessMessage = "Item comprado com sucesso!";

    [Tooltip("Mensagem exibida quando o jogador não tem dinheiro suficiente.")]
    public string insufficientFundsMessage = "Dinheiro insuficiente!";

    [Tooltip("Eventos disparados quando este item é comprado com sucesso.")]
    public UnityEvent OnItemPurchased;

    public ShopItem(string id, string name, string desc, Sprite icon, int price)
    {
        itemID = id;
        itemName = name;
        itemDescription = desc;
        itemIcon = icon;
        itemPrice = price;
    }
}