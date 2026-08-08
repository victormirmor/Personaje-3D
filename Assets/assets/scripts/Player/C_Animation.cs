using UnityEngine;

[RequireComponent(typeof(Animator))]
public class C_Animation : MonoBehaviour {
    #region Variables
    [SerializeField]bool CursorIsvisble;
    private Animator _anim;
    private int _hashSpeedX;
    private int _hashSpeedY;
    #endregion

    void Start() {
        _anim = GetComponent<Animator>();
        _hashSpeedX = Animator.StringToHash("SpeedX");
        _hashSpeedY = Animator.StringToHash("SpeedY");
        Cursor.visible = CursorIsvisble;
    }

    // El script de movimiento llamará a esta función
    public void ActualizarBotas(float x, float y) {
        _anim.SetFloat(_hashSpeedX, x);
        _anim.SetFloat(_hashSpeedY, y);
    }
}