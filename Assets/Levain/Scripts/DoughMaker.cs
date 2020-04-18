using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoughMaker : MonoBehaviour {
   public PlayerController playerController;
   public GameObject collectibleDough;

   [Header("Need")]
   public int WaterNeed = 1;
   public int FloorNeed = 1;
   public int LevainNeed = 1;
   // Start is called before the first frame update
   void Start() {

   }

   // Update is called once per frame
   void Update() {
   
   }

   public void AddIngredient(CollectibleType type){
      if(type == CollectibleType.Flour && FloorNeed > 0) {
         FloorNeed -= 1;
         playerController.Consume();
      }
      if(type == CollectibleType.Water && WaterNeed > 0){
         WaterNeed -= 1;
         playerController.Consume();
      }
      if(type == CollectibleType.Levain && LevainNeed > 0){
         LevainNeed -= 1;
         playerController.Consume();
      }
   }

   public void Collecting(){
      if(WaterNeed == 0 && FloorNeed == 0 && LevainNeed == 0){
         Vector3 pos = ((gameObject.transform.right * 2f) + gameObject.transform.position) + new Vector3(0, 1, 0);
         GameObject newCollectible = Instantiate(collectibleDough, pos, Quaternion.identity);
         newCollectible.GetComponent<Rigidbody>().AddForce(0, 0, -5f, ForceMode.Impulse);
         newCollectible.GetComponent<Collectible>().type = CollectibleType.Dough;

         WaterNeed = 1;
         FloorNeed = 1;
         LevainNeed = 1;
      }
   }
}