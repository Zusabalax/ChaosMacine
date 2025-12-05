using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(PlayerInput))]
    public class FirstPersonController : MonoBehaviour
    {
        [Header("Jogador")]
        [Tooltip("Velocidade de movimento do personagem")]
        public float MoveSpeed = 4.0f;
        [Tooltip("Velocidade de corrida do personagem")]
        public float SprintSpeed = 6.0f;
        [Tooltip("Taxa de aceleração e desaceleração")]
        public float SpeedChangeRate = 10.0f;

        [Space(10)]
        [Tooltip("A altura que o personagem pode pular")]
        public float JumpHeight = 1.2f;
        [Tooltip("O personagem usa seu próprio valor de gravidade")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Tempo necessário para esperar antes de poder pular novamente")]
        public float JumpTimeout = 0.1f;
        [Tooltip("Tempo necessário para entrar no estado de queda")]
        public float FallTimeout = 0.15f;

        [Header("Verificação de Chão")]
        [Tooltip("Se o personagem está no chão ou não")]
        public bool Grounded = true;
        [Tooltip("Útil para terrenos irregulares")]
        public float GroundedOffset = -0.14f;
        [Tooltip("O raio da verificação de chão")]
        public float GroundedRadius = 0.5f;
        [Tooltip("Quais camadas o personagem usa como chão")]
        public LayerMask GroundLayers;
       

        // player
        private float _speed;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        private PlayerInput _playerInput;
        private Rigidbody2D _rigidbody;
        private StarterAssetsInputs _input;
        private bool _isJumping = false;
        private Tween _movementTween;
        private Tween _jumpTween;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _input = GetComponent<StarterAssetsInputs>();
            _playerInput = GetComponent<PlayerInput>();

            // Configure Rigidbody2D para física 2D
            _rigidbody.gravityScale = 0; // Vamos controlar a gravidade manualmente
            _rigidbody.freezeRotation = true; // Congela rotação

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            GroundedCheck();
            JumpAndGravity();
            Move();
        }

        private void OnDestroy()
        {
            // Limpar tweens quando o objeto for destruído
            _movementTween?.Kill();
            _jumpTween?.Kill();
        }

        private void GroundedCheck()
        {
            // Verifica se está no chão usando um círculo 2D
            Vector2 circlePosition = new Vector2(transform.position.x, transform.position.y - GroundedOffset);
            Grounded = Physics2D.OverlapCircle(circlePosition, GroundedRadius, GroundLayers);

            // Se está no chão e não está pulando, resetar estado de pulo
            if(Grounded)
                     _isJumping = false;

            if (Grounded && _verticalVelocity <= 0f)
            {
                _isJumping = false;
                _verticalVelocity = 0f;
            }
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // Se não há input de movimento, parar o movimento
            if (_input.move.x == 0)
            {
                _movementTween?.Kill();
                return;
            }

            // Calcular direção do movimento
            Vector3 direction = new Vector3(_input.move.x, 0, 0).normalized;

            // Calcular posição alvo
            Vector3 targetPosition = transform.position + direction * targetSpeed * Time.deltaTime;

            // Se já existe um tween de movimento, matá-lo
            _movementTween?.Kill();

            // Criar novo tween de movimento suave
            _movementTween = transform.DOMoveX(targetPosition.x, 0.1f)
                .SetEase(Ease.Linear);
        }

        private void JumpAndGravity()
        {
            // Verificar input de pulo
            if (_input.jump && Grounded && !_isJumping)
            {
                Jump();
                _input.jump = false; // Resetar input de pulo
            }
            _input.jump = false;

            // Aplicar gravidade se não estiver no chão
            if (!Grounded)
            {
                // Aplicar gravidade
                _verticalVelocity += Gravity * Time.deltaTime;

                // Limitar velocidade vertical
                _verticalVelocity = Mathf.Max(_verticalVelocity, -_terminalVelocity);

                // Aplicar velocidade vertical ao rigidbody
                _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _verticalVelocity);
            }
        }

        private void Jump()
        {
            if (Grounded && !_isJumping)
            {
                _isJumping = true;

                // Calcular velocidade de pulo baseada na altura desejada
                float jumpVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                _verticalVelocity = jumpVelocity;

                // Matar qualquer tween de pulo anterior
                _jumpTween?.Kill();

                // Aplicar força de pulo
                _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _verticalVelocity);

                // Timer para permitir próximo pulo
                _jumpTimeoutDelta = JumpTimeout;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // Desenhar gizmo para visualizar a área de verificação de chão
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
        }
    }
}