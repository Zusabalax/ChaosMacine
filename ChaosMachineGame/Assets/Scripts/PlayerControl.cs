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

    private CharacterController _characterControler;
    [SerializeField]
    
    private LayerMask GroundLayers;

    bool Grounded;
    private float _radios;
    private float _circle = 0.7f;


    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 4.0f;
    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 6.0f;
    [Tooltip("Rotation speed of the character")]
    public float RotationSpeed = 1.0f;
    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.1f;
    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

  
    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.5f;
   

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 90.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -90.0f;

    private float _cinemachineTargetPitch;

    // player
    private float _speed;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;


  
  

    private GameObject _mainCamera;

    private const float _threshold = 0.01f;

    Vector3 _direction = Vector3.zero;


    bool _jumpbutton;


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


        _characterControler = GetComponent<CharacterController>();
        _characterControler.detectCollisions = true;
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

        #region testeMove
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
        #endregion

        GraundCheck();
  
        JumpAndGravity();
        Move();
        _jump = Grounded;



    }

    private void Move()
    {
        _characterControler.Move( _direction * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }

    private void JumpAndGravity()
    {
        if (Grounded)
        {
            _direction = Vector3.zero;
           // _verticalVelocity= 0.0f;    
            // reset the fall timeout timer

        }
        else
        {
            _direction = Vector3.right;
           
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
       
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
        _verticalVelocity = Forca*Time.deltaTime;
    //    Player.transform.SetParent(_originalPosition);
      //  Player.AddForce(new Vector2(0.2f, 1) * Forca, ForceMode2D.Impulse);
    //  _characterControler.Move(new Vector3(speed * Time.deltaTime, Forca * Time.deltaTime, 0));

       
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


    private void GraundCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y -_circle, transform.position.z);
        Grounded = Physics2D.OverlapCircle(spherePosition, _radios, GroundLayers);
        _animator.SetBool("Jump", false);
        //   Player.linearVelocity = Vector2.zero;
        //  Player.angularVelocity = 0f;
       // Player.transform.SetParent(collision.transform);
        _jump = Grounded;

        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAGRAUDED"+Grounded);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3 && !fly)
        {
            _animator.SetBool("Jump", false);
         //   Player.linearVelocity = Vector2.zero;
          //  Player.angularVelocity = 0f;
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