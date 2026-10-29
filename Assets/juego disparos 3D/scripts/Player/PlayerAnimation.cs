using UnityEngine;
using CrossInput;

    
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator anim;

        void Awake ()
        {
            anim = GetComponent<Animator>();
        }

        void Update (){
            ani_mation();
        }

        void ani_mation(){
            // Leer ejes de movimiento
            float h = InputManager.GetAxis(InputConstants.AXIS_HORIZONTAL);
            float v = InputManager.GetAxis(InputConstants.AXIS_VERTICAL);

            // 1. Enviar valores al Blend Tree 2D
            anim.SetFloat(InputConstants.ANIM_SPEED_X, h);
            anim.SetFloat(InputConstants.ANIM_SPEED_Y, v);
        }
}
