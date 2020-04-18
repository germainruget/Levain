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
   public List<Material> materials = new List<Material>();
   private MeshRenderer mesh;

   // Start is called before the first frame update
   void Start() {
      mesh = gameObject.GetComponent<MeshRenderer>();
      if (type == CollectibleType.Flour) {
         mesh.material = materials[0];
      } else if (type == CollectibleType.Water) {
         mesh.material = materials[1];
      } else if (type == CollectibleType.Levain) {
         mesh.material = materials[2];
      } else if (type == CollectibleType.Dough) {
         mesh.material = materials[3];
      } else if (type == CollectibleType.Bread) {
         mesh.material = materials[4];
      }
   }

   // Update is called once per frame
   void Update() {

   }
}