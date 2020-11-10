using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

   public Sound[] sounds;

   public static  AudioManager instance;

   // Start is called before the first frame update
   void Awake() {

      if(instance == null){
         instance = this;
      }
      else{
         Destroy(gameObject);
         return;
      }
      
      DontDestroyOnLoad(gameObject);

      foreach(Sound s in sounds){
         s.source = gameObject.AddComponent<AudioSource>();
         s.source.clip = s.clip;
         s.source.volume = s.volume;
         s.source.pitch = s.pitch;
         s.source.loop = s.loop;
      }
   }

   void Start(){
      PlaySound(CollectibleType.None, "MainTheme");
   }

   public void PlaySound(CollectibleType type, string name = ""){
      Sound s;

      if(type != CollectibleType.None){
         s = Array.Find(sounds, sound => sound.type == type);
      }
      else{
         s = Array.Find(sounds, sound => sound.name == name);
      }

      s.source.Play();
   }
}