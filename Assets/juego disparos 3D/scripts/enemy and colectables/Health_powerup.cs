using UnityEngine;

namespace CompleteProject
{
    public class Health_powerup : MonoBehaviour
    {
        public int live=10;
        public bool isCoin=false;
        HealthManager playerHealth;       // Reference to the player's health.
        GameObject player;


        void Awake ()
        {
            // Set up the reference.
            player = GameObject.FindGameObjectWithTag ("Player");
            playerHealth = FindAnyObjectByType <HealthManager> ();
        }

        void OnTriggerEnter (Collider other){
            if(other.gameObject == player){
                if(isCoin==false){

                playerHealth.currentHealth += live;
                Debug.Log("Sumaste "+live +" de vida");
                ScoreManager.score += live/2;
                Destroy(this.gameObject);
                }else{

                    ScoreManager.score += live;
                    Debug.Log("Sumaste "+live +" de dinero");
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
