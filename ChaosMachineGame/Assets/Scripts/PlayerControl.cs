using UnityEngine;
using System.Collections;

using UnityEngine.Events;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl Instance;
    public static bool fly;
    public static bool Jump;

  

    [SerializeField]
    private Rigidbody2D Player;
    [SerializeField]
    private float Forca, FlyForce, Jumptime;
    [SerializeField]
    private float speed;

    [SerializeField]
    private Animator _animator;

    // Unity Events
    public UnityEvent OnJump;
    public UnityEvent OnDash;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Jump = false;
        fly = false;
    }

    void Update()
    {
        if (!fly)
        {
            if (Jump)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ExecuteJump();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ExecuteFlyJump();
            }
        }

        // Input para dash (exemplo com Left Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ExecuteDash();
        }
    }

    public void JumpButton()
    {
        if (!fly)
        {
            if (Jump)
            {
                ExecuteJump();
            }
        }
        else
        {
            ExecuteFlyJump();
        }
    }

    public void DashButton()
    {
        ExecuteDash();
    }

    private void ExecuteJump()
    {
        Player.AddForce(Vector2.up * Forca, ForceMode2D.Impulse);
        Jump = false;
        _animator.SetBool("Jump", true);
        OnJump.Invoke(); // Dispara o evento de pulo
    }

    private void ExecuteFlyJump()
    {
        Player.AddForce(Vector2.up * FlyForce, ForceMode2D.Impulse);
        StartCoroutine(Cdfly());
        OnJump.Invoke(); // Dispara o evento de pulo
    }

    private void ExecuteDash()
    {
        // Implementação básica do dash - ajuste conforme necessário
        // Exemplo: dash rápido para a direita
        Player.AddForce(Vector2.right * Forca * 1.5f, ForceMode2D.Impulse);

        OnDash.Invoke(); // Dispara o evento de dash
    }

    void GroundCheck()
    {
        
    }

    IEnumerator Cdfly()
    {
        Jump = false;
        yield return new WaitForSeconds(Jumptime);
        Jump = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer== 3)
        {
            _animator.SetBool("Jump", false);
            Jump = true;
        }
    }
    
}