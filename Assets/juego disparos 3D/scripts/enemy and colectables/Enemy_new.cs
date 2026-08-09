using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_new: MonoBehaviour{
  #region 
    [SerializeField] string TagPlayer="Jugador",tagEspada="espada";
    [SerializeField][Range(1,100)] int Value=1;
    [SerializeField]bool IsCoin;
    [SerializeField]Animator anim;
    public EnemyGrunt_ia  ScriptIA;
    bool IsDead=false;
    [SerializeField] CapsuleCollider ColiderEnemy;
    [SerializeField] AudioClip _Coin,_Dead;
    [SerializeField] Rigidbody RB;
    [SerializeField]int _Sound;
#endregion

    GameManager GameManager;
  SoundFXManagerv FXManager;
    [SerializeField] float waitTime;

    void Update(){
      if(GameManager.IsCoin==true){
       IsCoin=true;
      }else{
         IsCoin=false;
      }
    }
    void Awake(){
        //IsDead=false;
        SearchManagers();
        }
      
       void SearchManagers() {      
           if(GameManager==null){
        GameManager=FindObjectOfType<GameManager>();
        if(FXManager==null){
        FXManager=FindObjectOfType<SoundFXManagerv>();
        } 
      }   
    }

    
    void OnTriggerEnter(Collider other){
		if (other.tag == TagPlayer&&IsCoin==false){
      FXManager.SoundPlay(_Coin, _Sound);
            //GameManager.Hearts-=100;
            StartCoroutine(VidaEnemy());
            GameManager.Points-=(Value/2);
	   }
       if(other.tag == tagEspada&&IsDead==false){
         Is_dead();
        }       
  }
  private void Is_dead()
  {
    IsCoin = false;
    IsDead = true;
    FXManager.SoundPlay(_Dead, _Sound);
    GameManager.Points += Value;
    ScriptIA.isDead();
    ColiderEnemy.isTrigger = false;
    ColiderEnemy.radius = 0.1f;
    ColiderEnemy.height = 0.1f;
    ScriptIA.enabled = false;
    RB.useGravity = true;
    Value = 0;
  }
        
        IEnumerator VidaEnemy(){
      yield return new WaitForSeconds(waitTime);
      GameManager.Hearts-=100;
      }
}
