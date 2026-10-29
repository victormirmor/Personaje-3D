using UnityEngine;
using CrossInput;

namespace CompleteProject
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movimiento")]
        public float speed = 6f;

        [Header("Físicas / Gravedad")]
        public float gravity = -19.62f;

        private CharacterController controller;
        private Vector3 verticalVelocity;

        void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            // Mantiene pegado al personaje al suelo si no está cayendo
            if (controller.isGrounded && verticalVelocity.y < 0)
            {
                verticalVelocity.y = -2f;
            }

            // Movimiento hacia adelante / atrás (D-Pad Arriba/Abajo o Eje Vertical)
            float forwardInput = InputManager.GetAxis(InputConstants.AXIS_VERTICAL);

            // Movimiento lateral / Strafe mapeado a los gatillos (LB / RB)
            float strafeInput = 0f;

            if (InputManager.GetButtonDown(InputConstants.BUTTON_STRAFE_RIGHT))
            {
                strafeInput += 1f;
            }
            if (InputManager.GetButtonDown(InputConstants.BUTTON_STRAFE_LEFT))
            {
                strafeInput -= 1f;
            }

            Move(strafeInput, forwardInput);
            ApplyGravity();
        }

        void Move(float strafe, float forward)
        {
            // Transform.right es el desplazamiento lateral (LB/RB)
            // Transform.forward es el avance/retroceso (Arriba/Abajo)
            Vector3 moveInput = transform.right * strafe + transform.forward * forward;

            if (moveInput.magnitude > 1f)
            {
                moveInput.Normalize();
            }

            controller.Move(moveInput * speed * Time.deltaTime);
        }

        void ApplyGravity()
        {
            verticalVelocity.y += gravity * Time.deltaTime;
            controller.Move(verticalVelocity * Time.deltaTime);
        }
    }
}