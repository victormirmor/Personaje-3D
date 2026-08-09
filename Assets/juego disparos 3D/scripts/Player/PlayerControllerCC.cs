using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerCC : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float speed = 6f;
    public float gravity = -19.62f; // Doble de la gravedad real (-9.81) para mejor sensación de peso

    private CharacterController controller;
    private Vector3 verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        // 1. Resetear la velocidad vertical si ya estamos tocando el suelo
        if (controller.isGrounded && verticalVelocity.y < 0)
        {
            // Se usa un valor negativo pequeño (-2f) para asegurar que el CharacterController 
            // se mantenga pegado al suelo al bajar pendientes o escalones.
            verticalVelocity.y = -2f; 
        }

        // 2. Obtener direcciones orientadas a la cámara
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Transform camTransform = Camera.main.transform;
        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * v + right * h).normalized;

        // 3. Mover horizontalmente
        controller.Move(moveDirection * speed * Time.deltaTime);

        // 4. Calcular la gravedad acumulada en el eje Y
        verticalVelocity.y += gravity * Time.deltaTime;

        // 5. Aplicar el movimiento vertical (gravedad)
        controller.Move(verticalVelocity * Time.deltaTime);
    }
}