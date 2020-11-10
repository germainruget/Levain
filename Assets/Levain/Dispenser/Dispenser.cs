using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour {

   public CollectibleType type;
   public GameObject collectible;
   private AudioManager audioManager;
   private PlayerController playerController;
   private HUDScript Hud;

   void Start() {
      audioManager = GameObject.FindObjectOfType<AudioManager>();
      playerController = GameObject.FindObjectOfType<PlayerController>();
      Hud = GameObject.FindObjectOfType<HUDScript>();
   }

   void Update() {

   }

   public void Dispense() {
      GameObject.FindObjectOfType<AudioManager>().PlaySound(CollectibleType.None, "Cash Out");
      playerController.money -= 2f;
      Hud.UpdateMoney(playerController.money);
      Vector3 pos = ((gameObject.transform.right * 2f) + gameObject.transform.position) + new Vector3(0, 1, 0);
      GameObject newCollectible = Instantiate(collectible, pos, Quaternion.identity);
      newCollectible.GetComponent<Rigidbody>().AddForce(gameObject.transform.right * 3f, ForceMode.Impulse);
      newCollectible.GetComponent<Collectible>().type = type;
   }
}