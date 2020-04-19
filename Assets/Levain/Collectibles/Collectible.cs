using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType {
   None,
   Flour,
   Water,
   Levain,
   Dough,
   Bread,
}

public class Collectible : MonoBehaviour {
   public CollectibleType type;
   // public List<Material> materials = new List<Material>();
   // private MeshRenderer mesh;
   public List<GameObject> Item = new List<GameObject>();
   public GameObject newCollectible;

   // Start is called before the first frame update
   void Start() {
      // mesh = gameObject.GetComponent<MeshRenderer>();
      if (type == CollectibleType.Flour) {
         // mesh.material = materials[0];
         newCollectible = Instantiate(Item[0], transform.position, Quaternion.identity);
      } else if (type == CollectibleType.Water) {
         // mesh.material = materials[1];
         newCollectible = Instantiate(Item[1], transform.position, Quaternion.identity);
      } else if (type == CollectibleType.Levain) {
         // mesh.material = materials[2];
         newCollectible = Instantiate(Item[2], transform.position, Quaternion.identity);
      } else if (type == CollectibleType.Dough) {
         // mesh.material = materials[3];
         newCollectible = Instantiate(Item[3], transform.position, Quaternion.identity);
      } else if (type == CollectibleType.Bread) {
         // mesh.material = materials[4];
         newCollectible = Instantiate(Item[4], transform.position, Quaternion.identity);
      }

      newCollectible.transform.parent = this.gameObject.transform;
   }

   // Update is called once per frame
   void Update() {

   }
}