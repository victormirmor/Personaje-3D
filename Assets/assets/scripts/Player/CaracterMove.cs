using UnityEngine;

public class CaracterMove : MonoBehaviour {
	#region Variables

	private CharacterController _controller;
	[Header("Ajustes de Rotación")]
	[Range(1.0f, 12.0f)]public float rotationSpeed = 10.0f;
	[Header("Ajustes de Fisica")]
	[Range(1.0f, 30.0f)]public float gravityForce = 20.0f;     // Fuerza cuando cae
	[Range(0.1f, 3.0f)]public float groundedGravity = 2.0f;  // Fuerza para mantenerse pegado

	[Header("Ajustes de Movimiento")]
	[Range(1.0f, 12.0f)] public float movementSpeed = 5.0f;
	[Range(0.1f, 2.0f)]public float rotationThreshold = 0.1f;

	[Header("Dependencias")]
	public C_Animation _animScript;
	public C_Sound _soundScript;

	private float _verticalVelocity; 
	#endregion

	#region Metodos de Unity
	void Start() {
		_controller = GetComponent<CharacterController>();
		_animScript = GetComponent<C_Animation>();
		_soundScript = GetComponent<C_Sound>();
	}

	void Update() {
		if (InputDataMap.Instance == null) return;

		ManejarMovimiento();
		AplicarGravedad();   
	}
	#endregion

	#region Logica de Movimiento
	void ManejarMovimiento() {

		// IMPORTANTE: Leemos los ejes ya filtrados del mapa central
		float h = InputDataMap.Instance.horizontal;
		float v = InputDataMap.Instance.vertical;

		Vector3 move = new Vector3(h, 0, v);

		// El CharacterController solo se mueve si los valores superan la zona muerta

		//float inputMagnitude = move.sqrMagnitude;
		_controller.Move(move * Time.deltaTime * movementSpeed);

		// Lógica de rotación
		if (move.sqrMagnitude > rotationThreshold) {
			Quaternion targetRotation = Quaternion.LookRotation(move);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
		}
		// Sincronización con animaciones
		if (_animScript != null) {
			_animScript.ActualizarBotas(h, v);
			//_soundScript.IntentarReproducirPasos(inputMagnitude);
		}
	}


	void AplicarGravedad() {
		if (_controller.isGrounded==false) { // ESTADO: En el aire
			_verticalVelocity -= gravityForce * Time.deltaTime;
		} 
		else if (_controller.isGrounded==true){// ESTADO: En el suelo
			_verticalVelocity = -groundedGravity;
		}
		Vector3 moveGravity = new Vector3(0, _verticalVelocity, 0);
		_controller.Move(moveGravity * Time.deltaTime);
	}
	#endregion
}