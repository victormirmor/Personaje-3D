using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GamepadDetector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;

    private void OnEnable()
    {
        // Suscribirse al evento de cambio de dispositivos
        InputSystem.onDeviceChange += OnDeviceChange;
        
        // Verificar el estado inicial al activar el objeto
        UpdateGamepadStatus();
    }

    private void OnDisable()
    {
        // Desuscribirse para evitar fugas de memoria
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        // Solo actualizar si el cambio corresponde a un Gamepad
        if (device is Gamepad)
        {
            UpdateGamepadStatus();
        }
    }

    private void UpdateGamepadStatus()
    {
        // Comprobar si hay al menos un mando presente en el sistema
        if (Gamepad.current != null)
        {
            statusText.text = $"Mando Detectado: {Gamepad.current.displayName}";
            statusText.color = Color.green;
        }
        else
        {
            statusText.text = "Sin Mando Conectado";
            statusText.color = Color.red;
        }
    }
}