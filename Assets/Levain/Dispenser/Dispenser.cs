using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour {

   public CollectibleType type;
   public List<Material> materials = new List<Material>();
   public GameObject collectible;
   private MeshRenderer mesh;

   // Start is called before the first frame update
   void Start() {

      mesh = gameObject.GetComponent<MeshRenderer>();
      if (type == CollectibleType.Flour) {
         mesh.material = materials[0];
      } else if (type == CollectibleType.Water) {
         mesh.material = materials[1];
      }
   }

   // Update is called once per frame
   void Update() {

   }

   public void Dispense(){
      Vector3 pos = ((gameObject.transform.right * 2f) + gameObject.transform.position) + new Vector3(0, 1, 0);
      GameObject newCollectible = Instantiate(collectible, pos, Quaternion.identity);
      newCollectible.GetComponent<Rigidbody>().AddForce(0, 0, -5f, ForceMode.Impulse);
      newCollectible.GetComponent<Collectible>().type = type;
   }
}