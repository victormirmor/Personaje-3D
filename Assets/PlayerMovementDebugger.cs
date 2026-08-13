using UnityEngine;

public class PlayerMovementDebugger : MonoBehaviour
{
    [Header("Configuración de Traza")]
    public float rayDistance = 1.5f;
    public Color rayColorFree = Color.green;
    public Color rayColorBlocked = Color.red;

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (characterController != null)
        {
            Debug.Log($"<color=cyan>[Debugger] CharacterController detectado. SkinWidth: {characterController.skinWidth} | Center: {characterController.center} | Radius: {characterController.radius}</color>");
        }
        else
        {
            Debug.LogWarning("[Debugger] No se encontró CharacterController en este GameObject. El script analizará solo la física de Raycast.");
        }
    }

    void Update()
    {
        // 1. Lectura de ejes desde InputDataMap o Input Estándar
        float h = 0f;
        float v = 0f;

        if (InputDataMap.Instance != null)
        {
            h = InputDataMap.Instance.horizontal;
            v = InputDataMap.Instance.vertical;
        }
        else
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }

        Vector3 inputDir = new Vector3(h, 0f, v);

        // 2. Si el jugador intenta moverse, lanzamos raycasts de prueba
        if (inputDir.sqrMagnitude > 0.01f)
        {
            // Raycast frontal según el frente actual del personaje
            CheckObstacle(transform.position + Vector3.up * 0.5f, transform.forward, "FRENTE");

            // Raycast hacia la dirección a la que apunta el input
            Vector3 worldInputDir = transform.TransformDirection(inputDir.normalized);
            CheckObstacle(transform.position + Vector3.up * 0.5f, worldInputDir, "INPUT_DIR");

            // Raycast hacia abajo para verificar si el piso lo entierra
            CheckFloor(transform.position + Vector3.up * 0.1f);
        }
    }

    private void CheckObstacle(Vector3 origin, Vector3 direction, string label)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, rayDistance))
        {
            Debug.DrawRay(origin, direction * hit.distance, rayColorBlocked);
            Debug.LogWarning($"<color=red>[Debugger BLOQUEO - {label}] Chocando con: '{hit.collider.name}' | Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)} | Tag: {hit.collider.tag} | Distancia: {hit.distance:F2}m</color>");
        }
        else
        {
            Debug.DrawRay(origin, direction * rayDistance, rayColorFree);
        }
    }

    private void CheckFloor(Vector3 origin)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.down, out hit, 0.5f))
        {
            if (hit.distance < 0.05f)
            {
                Debug.LogWarning($"<color=orange>[Debugger PISO] El personaje está demasiado enterrado en: '{hit.collider.name}'. Distancia al suelo: {hit.distance:F3}m (Sugerencia: subir el Center.Y del CharacterController)</color>");
            }
        }
    }

    // Registra colisiones físicas directas si se coloca en el personaje
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Ignorar el suelo directo cuando el normal apunta hacia arriba
        if (hit.normal.y > 0.8f) return;

        Debug.Log($"<color=magenta>[Debugger CONTACTO FISICO] Colisionando lateralmente con: '{hit.gameObject.name}' | Tag: '{hit.gameObject.tag}' | Normal: {hit.normal}</color>");
    }
}