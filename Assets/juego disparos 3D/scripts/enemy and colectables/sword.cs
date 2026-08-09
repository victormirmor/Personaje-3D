using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sword : MonoBehaviour{

    GameObject Espada;
    public string TagEspada, TagPlayer = "Jugador";
    
    [SerializeField] float waitTime;
    [SerializeField]int _Sound;
    [SerializeField] AudioClip _EnemyChange;
    [SerializeField]TextMeshProUGUI Texto1,Texto2;

    [SerializeField] GameObject colectable;
    Collider ColectableCollider, EspadaCollider;
    Renderer ColectableRenderer, EspadaRenderer;
  SoundFXManagerv FXManager;
  GameManager gameManager;

void Awake(){
        Espada = GameObject.FindGameObjectWithTag(TagEspada);
        EspadaCollider = Espada.GetComponent<Collider>();
        ColectableCollider = colectable.GetComponent<Collider>(); 
        EspadaRenderer = Espada.GetComponent<Renderer>();
        ColectableRenderer = colectable.GetComponent<Renderer>();
        FXManager = FindObjectOfType<SoundFXManagerv>();
        gameManager=FindObjectOfType<GameManager>();
      }

    private void OnTriggerEnter(Collider other)
    {
      if (other.tag == TagPlayer){
      FXManager.SoundPlay(_EnemyChange, _Sound);
      gameManager.IsCoin = true;
        StartCoroutine(VidaEnemy());
        EspadaCollider.enabled = true;
        EspadaRenderer.enabled = true;
        ColectableCollider.enabled = false;
        ColectableRenderer.enabled = false;
      }
             
    IEnumerator VidaEnemy(){
      Texto1.text = "Si";
      Texto2.text="Si";
      yield return new WaitForSeconds(waitTime);
      Texto1.text = "No";
      Texto2.text = "No";
      Debug.Log("tiempo acabado");
      EspadaCollider.enabled = false;
      EspadaRenderer.enabled = false;
      gameManager.IsCoin = false;
      Destroy(colectable);
      }

    }
    }
