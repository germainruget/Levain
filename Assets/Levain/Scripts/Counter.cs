using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour {
   public PlayerController playerController;
   private int NeedBread = 1;
   //HUD
   public HUDScript Hud;

   // Start is called before the first frame update
   void Start() {
      playerController = GameObject.FindObjectOfType<PlayerController>();
      Hud.UpdateMoney(playerController.money);
   }

   // Update is called once per frame
   void Update() {

   }

   public void Sell(){
      if(NeedBread > 0){
         StartCoroutine(playerController.Consume());
         playerController.money += 5f;
         Hud.UpdateMoney(playerController.money);
         NeedBread = 1;
      }
   }
}