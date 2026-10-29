using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManagerv : MonoBehaviour{    
    
    [SerializeField]AudioSource Source1;
    [SerializeField]AudioSource Source2; 

    public void SoundPlay(AudioClip AudioPlay, int Source){
        if(Source==1){
        Source1.clip=AudioPlay;
        Source1.Play(); 
    }if(Source==2){
        Source2.clip=AudioPlay;
        Source2.Play(); 
    }
}
     public void SoundStop(AudioClip AudioPlay, int Source){
        if(Source==1){
        Source1.clip=AudioPlay;
        Source1.Stop(); 
    }if (Source == 2)
        {
            Source2.clip = AudioPlay;
            Source2.Stop();
        }
    }
}
