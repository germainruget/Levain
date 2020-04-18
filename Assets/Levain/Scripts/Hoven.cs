using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoven : MonoBehaviour {
   public PlayerController playerController;
   public GameObject collectibleBread;

   // Start is called before the first frame update
   void Start() {

   }

   // Update is called once per frame
   void Update() {

   }

   public void PutDoughHoven() {
      playerController.Consume();
   }

   public void Collecting() {
      Vector3 pos = ((gameObject.transform.right * 2f) + gameObject.transform.position) + new Vector3(0, 1, 0);
      GameObject newCollectible = Instantiate(collectibleBread, pos, Quaternion.identity);
      newCollectible.GetComponent<Rigidbody>().AddForce(0, 0, -5f, ForceMode.Impulse);
      newCollectible.GetComponent<Collectible>().type = CollectibleType.Bread;
   }
}