using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class C_Sound : MonoBehaviour {
    #region Variables
    [Header("Ajustes de Pasos")]
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private float stepInterval = 0.5f; // Tiempo entre pasos
    
    private AudioSource _source;
    private float _stepTimer;
    #endregion

    void Start() {
        _source = GetComponent<AudioSource>();
        // Configuramos el AudioSource por código para evitar errores
        _source.playOnAwake = false;
        _source.loop = false;
    }

    public void IntentarReproducirPasos(float movimientoSqr) {
        // Si el personaje se está moviendo
        if (movimientoSqr > 0.01f) {
            _stepTimer -= Time.deltaTime;

            if (_stepTimer <= 0) {
                _source.PlayOneShot(footstepClip);
                _stepTimer = stepInterval;
            }
        } else {
            // Si se detiene, reseteamos el timer para que el primer paso suene instantáneo al volver a movernos
            _stepTimer = 0;
        }
    }
}
