using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mood{
   Happy, 
   Hmmmm,
   Bad,
}
public class LevainScript : MonoBehaviour {
   public Mood mood;
   public PlayerController playerController;
   public GameObject collectibleLevain;

   [Header("Need")]
   public float maxLifePoint = 100f;
   public float currentLifePoint;
   public Mood currentMood = Mood.Happy;
   public float healingPoint = 5f;
   public int WaterNeed;
   public int FloorNeed;

   //HUD
   public HUDScript Hud;

   // Start is called before the first frame update
   void Start() {
      currentLifePoint = maxLifePoint;
      mood = currentMood;
      RandomNeed(mood);
   }

   // Update is called once per frame
   void Update() {
      //Random need at each bader mood
      if(mood != currentMood){
         if((int) mood < (int) currentMood) RandomNeed(currentMood);
         mood = currentMood;
      }

      //Life point over time
      if (maxLifePoint > 0) {
         currentLifePoint -= Time.deltaTime;
         Hud.UpdateLife(currentLifePoint / maxLifePoint);
      }

      //mood Changer
      if (currentLifePoint > (maxLifePoint / 3) * 2) {
         currentMood = Mood.Happy;
      } else if (currentLifePoint > (maxLifePoint / 3)) {
         currentMood = Mood.Hmmmm;
      } else {
         currentMood = Mood.Bad;
      }

      //Update levain icon for each mood
      Hud.UpdateLevainIcon(currentMood);
   }

   public void RandomNeed(Mood currentMood){
      Debug.Log((int) currentMood);
      int NewWaterNeed = Random.Range(1, 2) * ((int) currentMood + 1);
      int NewFloorNeed = Random.Range(1, 2) * ((int) currentMood + 1);

      WaterNeed += NewWaterNeed;
      FloorNeed += NewFloorNeed;

      Hud.ChangeWaterNeed(WaterNeed);
      Hud.ChangeFloorNeed(FloorNeed);

   }

   public void Feeding(CollectibleType type){
      if(type == CollectibleType.Flour && FloorNeed > 0) {
         FloorNeed -= 1;
         if(currentLifePoint < maxLifePoint){
            float newlifeAmount = currentLifePoint + healingPoint;
            if(newlifeAmount > maxLifePoint){
               currentLifePoint = maxLifePoint;
            }
            else{
               currentLifePoint = newlifeAmount;
            }
         }
         Hud.ChangeFloorNeed(FloorNeed);
         playerController.Consume();
         
      }
      if(type == CollectibleType.Water && WaterNeed > 0){
         WaterNeed -= 1;
         if(currentLifePoint < maxLifePoint){
            float newlifeAmount = currentLifePoint + healingPoint;
            if(newlifeAmount > maxLifePoint){
               currentLifePoint = maxLifePoint;
            }
            else{
               currentLifePoint = newlifeAmount;
            }
         }
         Hud.ChangeWaterNeed(WaterNeed);
         playerController.Consume();
      }
   }

   public void Collecting(){
      // if(currentMood == Mood.Happy){
      //    Debug.Log("Collecting");
      // }
      Vector3 pos = ((gameObject.transform.right * 2f) + gameObject.transform.position) + new Vector3(0, 1, 0);
      GameObject newCollectible = Instantiate(collectibleLevain, pos, Quaternion.identity);
      newCollectible.GetComponent<Rigidbody>().AddForce(0, 0, -5f, ForceMode.Impulse);
      newCollectible.GetComponent<Collectible>().type = CollectibleType.Levain;

      currentLifePoint -= 10f;

   }
}