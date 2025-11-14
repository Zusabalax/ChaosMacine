using UnityEngine;
using TMPro; 

public class MoneyDisplayUI : MonoBehaviour
{
    [Tooltip("Referência ao TextMeshProUGUI que exibirá o dinheiro.")]
    public TextMeshProUGUI currencyText;

    private void Start()
    {
        EconomyManager.Instance.OnCurrencyUpdated.AddListener(UpdateCurrencyDisplay);
        UpdateCurrencyDisplay(EconomyManager.Instance.GetCurrentCurrency());
    }
    private void OnEnable()
    {
        if (currencyText == null)
        {
            Debug.LogError("MoneyDisplayUI: currencyText não está atribuído. Por favor, atribua um TextMeshProUGUI no Inspector.");
            enabled = false;
            return;
        }
    }

    private void OnDisable()
    {
        EconomyManager.Instance.OnCurrencyUpdated.RemoveListener(UpdateCurrencyDisplay);
    }

    /// <summary>
    /// Atualiza o texto da UI com o valor da moeda.
    /// Este método será chamado pelo evento OnCurrencyUpdated.
    /// </summary>
    /// <param name="newCurrencyAmount">A nova quantia de dinheiro do jogador.</param>
    public void UpdateCurrencyDisplay(int newCurrencyAmount)
    {
        if (currencyText != null)
        {
            currencyText.text = newCurrencyAmount.ToString();
            currencyText.text = $"Moeda: {newCurrencyAmount}";
        }
    }
}