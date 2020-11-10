using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoven : MonoBehaviour {
   public PlayerController playerController;
   public GameObject collectibleBread;
   public HUDScript Hud;

   public int doughNeeded = 1;

   // Start is called before the first frame update
   void Start() {
      playerController = GameObject.FindObjectOfType<PlayerController>();
      Hud = GameObject.FindObjectOfType<HUDScript>();
   }

   // Update is called once per frame
   void Update() {

   }

   public void PutDoughHoven() {
      if (doughNeeded > 0) {
         doughNeeded -= 1;
         Hud.ChangeHovenNeed();
         StartCoroutine(playerController.Consume());
      }
   }

   public void Collecting() {
      if (doughNeeded == 0) {
         Hud.ResetHovenNeed();
         Vector3 pos = ((gameObject.transform.right * 2f) + gameObject.transform.position) + new Vector3(0, 1, 0);
         GameObject newCollectible = Instantiate(collectibleBread, pos, Quaternion.identity);
         newCollectible.GetComponent<Rigidbody>().AddForce(gameObject.transform.right * 3f, ForceMode.Impulse);
         newCollectible.GetComponent<Collectible>().type = CollectibleType.Bread;
         doughNeeded = 1;
      }
   }
}