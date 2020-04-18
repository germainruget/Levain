using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TransitionParameter {
   Move,
   Holding,
}

public class PlayerController : MonoBehaviour {
   public GameObject Player;
   public LevainScript Levain;

   private PlayerInputActions inputActions;
   private Vector2 movementInput;
   private bool IsMoving;

   [SerializeField]
   private float moveSpeed = 10f;

   private Vector3 inputDirection;
   private Vector3 moveVector;
   private Quaternion currentRotation;
   private Animator animator;

   [Header("Picking Up")]
   public bool canPickUp;
   public GameObject Pickable;
   [Header("Order")]
   public bool canOrder;
   public GameObject Dispenser;
   [Header("Holding")]
   public bool isHolding;
   public CollectibleType currentCollectibeType;
   public GameObject Holding;
   [Header("Feeding")]
   public bool canFeed;
   public bool canCollect;

   //HUD
   public HUDScript Hud;

   void Start() {
      Player = gameObject;
      Levain = GameObject.FindObjectOfType<LevainScript>();
      Hud = GameObject.FindObjectOfType<HUDScript>();
   }

   void Awake() {
      inputActions = new PlayerInputActions();
      inputActions.Player.Movement.performed += context => movementInput = context.ReadValue<Vector2>();

      animator = GetComponentInChildren<Animator>();
   }

   void Update() {
      if (canPickUp && !isHolding) {
         PickUp();
      } else if (!canFeed && isHolding) {
         PutDown();
      } else if (canFeed) {
         Feeding();
      } else if (canOrder && Input.GetKeyDown(KeyCode.E)) {
         Dispenser.GetComponent<Dispenser>().Dispense();
      } else if (canCollect) {
         Collecting();
      }
   }

   void FixedUpdate() {
      float h = movementInput.x;
      float v = movementInput.y;

      Vector3 targetInput = new Vector3(h, 0, v);

      inputDirection = Vector3.Lerp(inputDirection, targetInput, Time.deltaTime * 10f);

      Vector3 camForward = Camera.main.transform.forward;
      Vector3 camRight = Camera.main.transform.right;
      camForward.y = 0f;
      camRight.y = 0f;

      Vector3 desiredDirection = camForward * inputDirection.z + camRight * inputDirection.x;

      if (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0) {
         Move(desiredDirection);
         animator.SetBool(TransitionParameter.Move.ToString(), true);
      } else {
         animator.SetBool(TransitionParameter.Move.ToString(), false);
      }

      Turn(desiredDirection);
   }

   void Move(Vector3 desiredDirection) {
      moveVector.Set(desiredDirection.x, 0f, desiredDirection.z);
      moveVector = moveVector * moveSpeed * Time.deltaTime;
      transform.position += moveVector;
   }

   void Turn(Vector3 desiredDirection) {
      if ((desiredDirection.x > 0.1f || desiredDirection.x < -0.1f) || (desiredDirection.z > 0.1f || desiredDirection.z < -0.1f)) {
         currentRotation = Quaternion.LookRotation(desiredDirection);
         transform.rotation = currentRotation;
      } else {
         transform.rotation = currentRotation;
      }
   }

   private void OnEnable() {
      inputActions.Enable();
   }

   private void OnDisable() {
      inputActions.Disable();
   }

   private void OnTriggerEnter(Collider other) {
      Collectible collectible = other.GetComponent<Collectible>();
      LevainScript levain = other.GetComponent<LevainScript>();
      Dispenser dispenser = other.GetComponent<Dispenser>();

      if (levain != null) {
         if (isHolding) {
            canFeed = true;
            Hud.OpenLevainPanel("Press E to feed");
         } else {
            Hud.OpenLevainPanel("Press E to collect");
            canCollect = true;
         }
      }

      if (collectible != null) {
         Pickable = collectible.gameObject;
         canPickUp = true;
         Hud.OpenPickUpPanel(collectible.type.ToString());
      }

      if (dispenser != null) {
         Dispenser = dispenser.gameObject;
         canOrder = true;
         Hud.OpenDispenserPanel(dispenser.type.ToString());
      }

   }

   private void OnTriggerExit(Collider other) {
      Pickable = null;
      canPickUp = false;

      Dispenser = null;
      canOrder = false;

      canFeed = false;
      canCollect = false;
      Hud.ClosePanels();
   }

   private void PickUp() {
      if (Input.GetKeyDown(KeyCode.E) && Pickable != null) {
         Holding = Pickable;
         currentCollectibeType = Pickable.GetComponent<Collectible>().type;
         Pickable.SetActive(false);
         isHolding = true;
         animator.SetBool(TransitionParameter.Holding.ToString(), true);

         //Remove pickable
         Pickable = null;
         canPickUp = false;
         Hud.ClosePanels();

         //Slow Down player
         moveSpeed = 4f;
      }
   }

   private void PutDown() {
      if (Input.GetKeyDown(KeyCode.E) && Holding != null && !canFeed) {
         //Place Object
         Holding.transform.position = ((Player.transform.forward * 1f) + Player.transform.position) + new Vector3(0, 2, 0);
         Holding.SetActive(true);
         Holding.GetComponent<Rigidbody>().AddForce(Player.transform.forward * 1f, ForceMode.Impulse);

         LetGo();

      }
   }

   private void LetGo() {
      //Let Go
      Holding = null;
      currentCollectibeType = CollectibleType.None;
      isHolding = false;
      animator.SetBool(TransitionParameter.Holding.ToString(), false);

      //Put back player speed;
      moveSpeed = 8f;
   }

   private void Feeding() {
      if (Input.GetKeyDown(KeyCode.E)) {
         Levain.Feeding(currentCollectibeType);
      }
   }

   private void Collecting() {
      if (Input.GetKeyDown(KeyCode.E)) {
         Levain.Collecting();
      }
   }

   public void Consume() {
      Destroy(Holding);
      LetGo();
   }
}