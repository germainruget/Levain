using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoughMaker : MonoBehaviour {
   public PlayerController playerController;
   public GameObject collectibleDough;
   public HUDScript Hud;

   [Header("Need")]
   public int WaterNeed = 1;
   public int FloorNeed = 1;
   public int LevainNeed = 1;
   // Start is called before the first frame update
   void Start() {
      playerController = GameObject.FindObjectOfType<PlayerController>();
      Hud = GameObject.FindObjectOfType<HUDScript>();
   }

   // Update is called once per frame
   void Update() {
   
   }

   public void AddIngredient(CollectibleType type){
      if(type == CollectibleType.Flour && FloorNeed > 0) {
         FloorNeed -= 1;
         Hud.ChangeDoughNeed(type);
         StartCoroutine(playerController.Consume());
      }
      if(type == CollectibleType.Water && WaterNeed > 0){
         WaterNeed -= 1;
         Hud.ChangeDoughNeed(type);
         StartCoroutine(playerController.Consume());
      }
      if(type == CollectibleType.Levain && LevainNeed > 0){
         LevainNeed -= 1;
         Hud.ChangeDoughNeed(type);
         StartCoroutine(playerController.Consume());
      }
      if(WaterNeed == 0 && FloorNeed == 0 && LevainNeed == 0){
         Hud.ChangeDoughNeed(CollectibleType.Dough);
      }
   }

   public void Collecting(){
      if(WaterNeed == 0 && FloorNeed == 0 && LevainNeed == 0){
         Vector3 pos = ((gameObject.transform.right * 2f) + gameObject.transform.position) + new Vector3(0, 1, 0);
         GameObject newCollectible = Instantiate(collectibleDough, pos, Quaternion.identity);
         newCollectible.GetComponent<Rigidbody>().AddForce(gameObject.transform.right * 3f, ForceMode.Impulse);
         newCollectible.GetComponent<Collectible>().type = CollectibleType.Dough;

         WaterNeed = 1;
         FloorNeed = 1;
         LevainNeed = 1;
         Hud.ResetDoughNeed();
      }
   }
}