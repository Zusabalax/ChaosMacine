using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl Instance;
    public static bool fly, guita;
    private bool _jump, _cdDash, _stopDash;

    [SerializeField]
    private Transform _originalPosition;

    public Rigidbody2D Player;
    [SerializeField]
    private float Forca, FlyForce, _dashForce, Jumptime;
    [SerializeField]
    private float speed;

    [SerializeField]
    private Animator _animator;

    // CORREÇÃO: Mudar de AnimatorController para RuntimeAnimatorController
    public RuntimeAnimatorController _gargulaController, _catController, _gatoGuita;

    // Unity Events
    public UnityEvent OnJump;
    public UnityEvent OnDash;

    private bool skill, cdskillsss;

    [SerializeField]
    Transform skillPoint;

    [SerializeField]
    GameObject bulletAtak, bulletAtak2;

    [SerializeField]
    private TextMeshProUGUI lifeText;

    public int life = 9;

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
        _jump = false;
        fly = false;
        _cdDash = true;
        _stopDash = false;
        guita = false;
        cdskillsss = true;
    }

    IEnumerator CdSkillssssss()
    {
        cdskillsss = false;
        yield return new WaitForSeconds(1);
        cdskillsss = true;
    }

    public void UpgrateSkill()
    {
        if (fly)
        {
            _animator.runtimeAnimatorController = _gatoGuita;
            skill = true;
        }
    }

    public void SkillAtak()
    {
        if (fly)
        {
            if (cdskillsss)
            {
                if (!skill)
                {
                    Instantiate(bulletAtak, skillPoint.position, skillPoint.rotation);
                }
                else
                {
                    Instantiate(bulletAtak2, skillPoint.position, skillPoint.rotation);
                }
                StartCoroutine(CdSkillssssss());
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SkillAtak();
        }

        if (life <= 0)
        {
            StateMachine.Instance.SetState(StateMachine.State.GameOver);
        }

        if (!fly)
        {
            if (_jump)
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
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (fly)
                ExecuteFlyDASH();
            else
                ExecuteDash();
        }
    }

    public void JumpButton()
    {
        if (!fly)
        {
            if (_jump)
            {
                ExecuteJump();
            }
        }
        else
        {
            ExecuteFlyJump();
        }
    }

    public void JumpDash()
    {
        if (!fly)
        {
            if (_jump)
            {
                ExecuteDash();
            }
        }
        else
        {
            ExecuteFlyDASH();
        }
    }

    public void DashButton()
    {
        ExecuteDash();
    }

    private void ExecuteJump()
    {
        _animator.SetBool("Jump", true);
        _stopDash = false;
        Player.transform.SetParent(_originalPosition);
        Player.AddForce(new Vector2(0.2f, 1) * Forca, ForceMode2D.Impulse);
        _jump = false;
        OnJump?.Invoke(); // Dispara o evento de pulo
    }

    private void ExecuteFlyJump()
    {
        transform.DORotate(new Vector3(0, 180, 0), 0f);
        Player.AddForce(new Vector2(-1, 5) * FlyForce, ForceMode2D.Impulse);
        StartCoroutine(Cdfly());
        OnJump?.Invoke(); // Dispara o evento de pulo
    }

    private void ExecuteFlyDASH()
    {
        transform.DORotate(new Vector3(0, 0, 0), 0f);
        Player.AddForce(new Vector2(1, 5) * FlyForce, ForceMode2D.Impulse);
        StartCoroutine(Cdfly());
        OnJump?.Invoke(); // Dispara o evento de pulo
    }

    private void ExecuteDash()
    {
        if (_cdDash)
        {
            if (!_stopDash)
            {
                StartCoroutine(CdDash());
                if (_jump)
                {
                    _animator.SetBool("Jump", false);
                    _animator.SetBool("Dash", true);
                }
                else
                {
                    _animator.SetBool("Jump", true);
                    _animator.SetBool("Dash", true);
                }
                Player.AddForce(Vector2.right * _dashForce, ForceMode2D.Impulse);
            }
            else
            {
                _animator.SetBool("Jump", false);
                _animator.SetBool("Dash", false);
                Player.linearVelocity = Vector2.zero;
                Player.angularVelocity = 0f;
            }
            _stopDash = !_stopDash;
            StartCoroutine(CdDash());
            OnDash?.Invoke(); // Dispara o evento de dash
        }
    }

    IEnumerator Cdfly()
    {
        _jump = false;
        yield return new WaitForSeconds(Jumptime);
        _jump = true;
    }

    IEnumerator CdDash()
    {
        _cdDash = false;
        yield return new WaitForEndOfFrame();
        _cdDash = true;
        _animator.SetBool("Dash", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3 && !fly)
        {
            _animator.SetBool("Jump", false);
            Player.linearVelocity = Vector2.zero;
            Player.angularVelocity = 0f;
            Player.transform.SetParent(collision.transform);
            _jump = true;
        }

        if (collision.gameObject.layer == 6)
        {
            life--;
            lifeText.text = life.ToString();
        }
    }
}