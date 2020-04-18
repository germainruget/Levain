using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour {
   public PlayerController playerController;

   //HUD
   public HUDScript Hud;

   // Start is called before the first frame update
   void Start() {
      Hud.UpdateMoney(playerController.money);
   }

   // Update is called once per frame
   void Update() {

   }

   public void Sell(){
      playerController.Consume();
      playerController.money += 5f;
      Hud.UpdateMoney(playerController.money);
   }
}