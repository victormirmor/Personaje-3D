using UnityEngine;
using CrossInput;
namespace CompleteProject
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerRotation : MonoBehaviour
    {
        [Header("Rotación")]
        public float turnSpeed = 120f; // Grados por segundo

        void Update()
        {
            // Lee D-Pad Izquierda/Derecha o Flechas del teclado
            float turnInput = InputManager.GetAxis(InputConstants.AXIS_HORIZONTAL);

            Turn(turnInput);
        }

        void Turn(float turnInput)
        {
            if (turnInput != 0f)
            {
                float turnAmount = turnInput * turnSpeed * Time.deltaTime;
                transform.Rotate(0f, turnAmount, 0f);
            }
        }
    }
}