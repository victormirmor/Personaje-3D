using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colectar : MonoBehaviour{
    
    public PlayerShooting playerShooting;
    public GameObject This, PlayerGun;

    public bool IsGus;

    void Update(){
    }
    void OnTriggerEnter(Collider Other){
                    
        if(IsGus==true){
        PlayerGun.SetActive(true);
        playerShooting.enabled=true;
        }
    This.SetActive(false);
        

    }
}
