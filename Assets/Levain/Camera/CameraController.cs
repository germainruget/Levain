using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
   Vector3 velocity;
   public float SmoothTimeZ;
   public float SmoothTimeX;
   public float offsetZ = -20f;
   public PlayerController playerController;
   private Vector3 originPos;

   private void Start() {
      playerController = GameObject.FindObjectOfType<PlayerController>();
      originPos = transform.position;
   }

   void FixedUpdate() {
      float posX = Mathf.SmoothDamp(transform.position.x, playerController.transform.position.x, ref velocity.x, SmoothTimeX);
      float posZ = Mathf.SmoothDamp(transform.position.z, playerController.transform.position.z + offsetZ, ref velocity.z, SmoothTimeZ);

      transform.position = new Vector3(posX, transform.position.y, posZ);
   }
}