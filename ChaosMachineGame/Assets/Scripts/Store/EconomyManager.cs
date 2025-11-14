using UnityEngine;
using UnityEngine.Events;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; set; }

    [Header("Configurações de Moeda")]
    [Tooltip("Dinheiro inicial do jogador.")]
    [SerializeField] private int _playerCurrency = 1000;

    [Tooltip("Evento disparado quando o saldo do jogador é atualizado.")]
    public UnityEvent<int> OnCurrencyUpdated;

    private const string MONEY = "Money";
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
    }

    private void Start()
    {
        //_playerCurrency = PlayerPrefs.GetInt(MONEY);
        OnCurrencyUpdated?.Invoke(_playerCurrency);
    }

    /// <summary>
    /// Adiciona uma quantia de dinheiro ao saldo do jogador.
    /// </summary>
    /// <param name="amount">A quantia de dinheiro a ser adicionada. Deve ser positiva.</param>
    public void AddCurrency(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("EconomyManager: Tentativa de adicionar uma quantia negativa. Use SpendCurrency para remover.");
            return;
        }

        _playerCurrency += amount;
        OnCurrencyUpdated?.Invoke(_playerCurrency);
        PlayerPrefs.SetInt(MONEY, _playerCurrency);
        Debug.Log($"Adicionado {amount} dinheiro. Novo saldo: {_playerCurrency}");
    }

    /// <summary>
    /// Tenta remover uma quantia de dinheiro do saldo do jogador.
    /// </summary>
    /// <param name="amount">A quantia de dinheiro a ser gasta. Deve ser positiva.</param>
    /// <returns>True se o dinheiro foi gasto com sucesso, false caso contrário (ex: saldo insuficiente).</returns>
    public bool SpendCurrency(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("EconomyManager: Tentativa de gastar uma quantia negativa. Use AddCurrency para adicionar.");
            return false;
        }

        if (_playerCurrency >= amount)
        {
            _playerCurrency -= amount;
            OnCurrencyUpdated?.Invoke(_playerCurrency);
            PlayerPrefs.SetInt(MONEY, _playerCurrency);
            Debug.Log($"Gastos {amount} dinheiro. Saldo restante: {_playerCurrency}");
            return true;
        }
        else
        {
            Debug.Log($"Dinheiro insuficiente. Saldo atual: {_playerCurrency}, Necessário: {amount}");
            return false;
        }
    }

    /// <summary>
    /// Retorna o saldo atual do jogador.
    /// </summary>
    public int GetCurrentCurrency()
    {
        return _playerCurrency;
    }

    /// <summary>
    /// Verifica se o jogador pode pagar por uma quantia específica.
    /// </summary>
    /// <param name="amount">A quantia a ser verificada.</param>
    /// <returns>True se o jogador tiver dinheiro suficiente, false caso contrário.</returns>
    public bool CanAfford(int amount)
    {
        return _playerCurrency >= amount;
    }
}