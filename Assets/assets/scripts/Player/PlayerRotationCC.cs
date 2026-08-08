using UnityEngine;

public class PlayerRotationCC : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    public float turnAngle = 90f;       // Grados de giro por pulsación.
    public float turnSpeed = 15f;       // Velocidad del suavizado de rotación.

    private Quaternion targetRotation;

    void Awake()
    {
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // RB: Girar a la derecha
        if (Input.GetButtonDown(InputConstants.BUTTON_ROTATE_RIGHT))
        {
            targetRotation *= Quaternion.Euler(0f, turnAngle, 0f);
        }

        // LB: Girar a la izquierda
        if (Input.GetButtonDown(InputConstants.BUTTON_ROTATE_LEFT))
        {
            targetRotation *= Quaternion.Euler(0f, -turnAngle, 0f);
        }

        // Interpola suavemente la rotación del Transform
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
}