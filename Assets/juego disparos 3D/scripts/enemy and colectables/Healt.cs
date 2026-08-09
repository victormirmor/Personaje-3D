using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healt : MonoBehaviour{
    [SerializeField] string TagPlayer="Jugador";
    [SerializeField][Range(10,100)] int Value=10;
    [SerializeField] SphereCollider ColiderCoin;
    [SerializeField] GameObject thisCoin;
    [SerializeField] AudioClip _Healt;
    [SerializeField] int _Sound;
    
    GameManager GameManager;
    SoundFXManagerv FXManager;
    
     void Update() {
      SearchManagers();
    }
    void Awake(){
      SearchManagers();
    }  
    void OnTriggerEnter(Collider other){
		if (other.tag == TagPlayer){         
           GameManager.Hearts+=Value;
           GameManager.Points+=(Value/2);
            thisCoin.SetActive(false);
            ColiderCoin.enabled=false;
            FXManager.SoundPlay(_Healt, _Sound);           
      }
  } 
  void SearchManagers() {
           if(GameManager==null){
        GameManager=FindObjectOfType<GameManager>();
        if(FXManager==null){
        FXManager=FindObjectOfType<SoundFXManagerv>();
        }   
      }
    }




}
