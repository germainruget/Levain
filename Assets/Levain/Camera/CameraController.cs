using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
   Vector3 velocity;
   public float SmoothTimeZ;
   public float SmoothTimeX;
   public float offsetZ = -20f;
   public GameObject player;
   private Vector3 originPos;

   private void Start() {
      originPos = transform.position;
   }

   void FixedUpdate() {
      float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, SmoothTimeX);
      float posZ = Mathf.SmoothDamp(transform.position.z, player.transform.position.z + offsetZ, ref velocity.z, SmoothTimeZ);

      transform.position = new Vector3(posX, transform.position.y, posZ);
   }
}