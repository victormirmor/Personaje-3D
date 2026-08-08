#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
public class C_Controller : MonoBehaviour{
    #region Variables
	[Header ("Grabity")]
	public float gravity = 20.0F;
	private Vector3 gravityDirection = Vector3.zero; 
    [SerializeField]bool CursorIsvisble;

    [Header ("Movement")]
    [SerializeField]float Speed;
	[SerializeField]string Horizontal="Horizontal",Vertical="Vertical";

	private CharacterController _controller;
    private float Axis_Horizontal, Axis_Vertical;    
    private Vector3 _move;    
    private Animator _anim;
    #endregion
    #region Unity voids
    private void Start() {
        _anim=gameObject.GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }
    void Update(){
        Cursor.visible = CursorIsvisble;
        Move();
		Gravity();
    } 
    #endregion
    #region Move  
        void Move(){
		Axis_Horizontal=Input.GetAxis(Horizontal);
		Axis_Vertical=Input.GetAxis(Vertical);

		_move=new Vector3(Axis_Vertical,0,Axis_Horizontal);

		_controller.Move(_move * Time.deltaTime * Speed);       
                          
             _anim.SetFloat("SpeedX",_move.x);
             _anim.SetFloat("SpeedY",_move.z);
			
		Rotate(_move);
    } 
    #endregion  
	#region create voids
    void Rotate(Vector3 move){
        Vector3 Rotation=new Vector3 (move.x,0, move.z);        
         if (Axis_Horizontal!=0 || Axis_Vertical!=0){
          transform.rotation= Quaternion.LookRotation(Rotation);
        }
    }

	void Gravity() {      
		gravityDirection.y -= gravity * Time.deltaTime;
		_controller.Move(gravityDirection * Time.deltaTime);
	}
    #endregion
}
