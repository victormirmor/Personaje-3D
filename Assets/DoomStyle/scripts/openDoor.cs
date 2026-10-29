using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openDoor : MonoBehaviour{
    public bool Open=false;
    public Animator Anim;

    void Start(){
        Open=false;
         anim();
    }

    void OnTriggerEnter(Collider Other){
        Open=true;
         anim();

    }

    void OnTriggerExit(Collider Other){
        Open=false;
        anim();

    }

    void anim(){
         Anim.SetBool("OpenDoor", Open);
    }
}
