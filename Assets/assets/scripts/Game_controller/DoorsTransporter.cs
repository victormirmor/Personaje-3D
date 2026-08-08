using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsTransporter : MonoBehaviour{
    [SerializeField] string TagPlayer;

    public GameObject Player;
    public Transform Destino;
    public Vector3 Offset;
    void Awake(){
        searchPlayer(); 
      }
      private void Update() {
         searchPlayer();
        
      } 

void searchPlayer(){
    if (Player == null)
 Player=GameObject.FindGameObjectWithTag(TagPlayer);
} 
    void OnTriggerEnter(Collider other){
		if (other.tag == TagPlayer ){
       other.transform.position=Destino.transform.position+Offset;

        }
    } 
}
