using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public bool jump;
        public bool sprint;

        [Header("Movement Settings")]
        public bool analogMovement;

        private void Update()
        {
            // Resetar input de pulo após ser processado
            // (o controller principal já faz isso)
        }

        // Métodos chamados pelo Player Input
        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        public void OnJump(InputValue value)
        {
            jump = value.isPressed;
        }

        public void OnSprint(InputValue value)
        {
            sprint = value.isPressed;
        }
    }
}