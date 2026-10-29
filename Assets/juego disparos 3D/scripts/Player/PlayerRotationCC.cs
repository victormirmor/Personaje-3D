using UnityEngine;
using CrossInput;

public class PlayerRotationCC : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    public float tapAngle = 15f;            // Grados de giro por toque corto (Tap)
    public float turnSpeed = 15f;           // Velocidad de la interpolación (Slerp)
    public float holdRotationSpeed = 180f;  // Velocidad de giro continuo al mantener (grados/seg)
    public float holdThreshold = 0.18f;     // Tiempo en segundos para considerar botón "mantenido"

    private Quaternion targetRotation;
    private float lbTimer = 0f;
    private float rbTimer = 0f;

    void Awake()
    {
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // 1. Quick Turn de 180° (si presiona ambos botones a la vez)
        if ((InputManager.GetButtonDown(InputConstants.BUTTON_ROTATE_LEFT) && InputManager.GetButton(InputConstants.BUTTON_ROTATE_RIGHT)) ||
            (InputManager.GetButtonDown(InputConstants.BUTTON_ROTATE_RIGHT) && InputManager.GetButton(InputConstants.BUTTON_ROTATE_LEFT)))
        {
            targetRotation *= Quaternion.Euler(0f, 180f, 0f);
            lbTimer = 0f;
            rbTimer = 0f;
        }
        else
        {
            // 2. Rotación Izquierda (LB)
            HandleRotationButton(InputConstants.BUTTON_ROTATE_LEFT, ref lbTimer, -1f);

            // 3. Rotación Derecha (RB)
            HandleRotationButton(InputConstants.BUTTON_ROTATE_RIGHT, ref rbTimer, 1f);
        }

        // Interpola suavemente hacia la rotación objetivo
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    void HandleRotationButton(string buttonName, ref float buttonTimer, float direction)
    {
        // Toque corto instantáneo (Tap): Añade 15° al targetRotation
        if (InputManager.GetButtonDown(buttonName))
        {
            targetRotation *= Quaternion.Euler(0f, tapAngle * direction, 0f);
            buttonTimer = 0f;
        }

        // Mantener presionado (Hold): Desplaza progresivamente el targetRotation
        if (InputManager.GetButton(buttonName))
        {
            buttonTimer += Time.deltaTime;

            if (buttonTimer > holdThreshold)
            {
                float step = holdRotationSpeed * direction * Time.deltaTime;
                targetRotation *= Quaternion.Euler(0f, step, 0f);
            }
        }

        // Reinicia el contador al soltar el botón
       // if (InputManager.GetButtonUp(buttonName))
        //{
        //    buttonTimer = 0f;
        //}
    }
}