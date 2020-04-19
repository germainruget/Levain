using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TransitionParameter {
   Move,
   Holding,
}

public class PlayerController : MonoBehaviour {
   public GameObject player;
   public LevainScript levain;
   public DoughMaker doughMaker;
   public Hoven hoven;
   public Counter counter;

   private PlayerInputActions inputActions;
   private Vector2 movementInput;
   private bool IsMoving;

   [SerializeField]
   private float normalSpeed = 8f;
   [SerializeField]
   private float slowSpeed = 6f;

   private float moveSpeed;

   private Vector3 inputDirection;
   private Vector3 moveVector;
   private Quaternion currentRotation;
   private Animator animator;

   [Header("Money")]
   public float money = 100.0f;
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
   [Header("Bread Making")]
   public bool canAddIngredient;
   public bool canCollectDough;
   public bool canPutDoughHoven;
   public bool canCollectBread;
   [Header("Counter")]
   public bool canSell;

   //HUD
   public HUDScript Hud;

   void Start() {
      player = gameObject;
      levain = GameObject.FindObjectOfType<LevainScript>();
      doughMaker = GameObject.FindObjectOfType<DoughMaker>();
      hoven = GameObject.FindObjectOfType<Hoven>();
      counter = GameObject.FindObjectOfType<Counter>();
      Hud = GameObject.FindObjectOfType<HUDScript>();
      moveSpeed = normalSpeed;
   }

   void Awake() {
      inputActions = new PlayerInputActions();
      inputActions.Player.Movement.performed += context => movementInput = context.ReadValue<Vector2>();

      animator = GetComponentInChildren<Animator>();
   }

   void Update() {
      if (canPickUp && !isHolding) {
         PickUp();
      } else if (!canFeed && !canAddIngredient && !canPutDoughHoven && !canSell && isHolding) {
         PutDown();
      } else if (canFeed) {
         Feeding();
      } else if (canOrder && Input.GetKeyDown(KeyCode.E)) {
         Dispenser.GetComponent<Dispenser>().Dispense();
      } else if (canCollect) {
         CollectingLevain();
      } else if (canCollectDough) {
         CollectingDough();
      } else if (canAddIngredient) {
         AddIngredient();
      } else if (canPutDoughHoven) {
         PutDoughHoven();
      } else if (canCollectBread) {
         CollectingBread();
      } else if (canSell) {
         SellBread();
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
      Collectible collectibleCol = other.GetComponent<Collectible>();
      LevainScript levainCol = other.GetComponent<LevainScript>();
      DoughMaker doughMakerCol = other.GetComponent<DoughMaker>();
      Hoven hovenCol = other.GetComponent<Hoven>();
      Dispenser dispenserCol = other.GetComponent<Dispenser>();
      Counter counterCol = other.GetComponent<Counter>();

      //LEVAIN
      if (levainCol != null) {
         if (isHolding) {
            canFeed = true;
            Hud.OpenLevainPanel("Press E to feed " + currentCollectibeType);
         } else {
            Hud.OpenLevainPanel("Press E to collect starter");
            canCollect = true;
         }
      }

      //DOUGH
      if (doughMakerCol != null) {
         if (isHolding) {
            canAddIngredient = true;
            Hud.OpenBreadMakerPanel("Press E to add " + currentCollectibeType);
         } else {
            Hud.OpenBreadMakerPanel("Press E to collect dough");
            canCollectDough = true;
         }
      }

      //HOVEN
      if (hovenCol != null) {
         if (isHolding) {
            canPutDoughHoven = true;
            Hud.OpenHovenMakerPanel("Press E to add bread");
         } else {
            Hud.OpenHovenMakerPanel("Press E to collect bread");
            canCollectBread = true;
         }
      }

      //COUNTER
      if (counterCol != null) {
         if (isHolding) {
            canSell = true;
            Hud.OpenCounterPanel();
         }
      }

      //COLLECT
      if (collectibleCol != null) {
         Pickable = collectibleCol.gameObject;
         canPickUp = true;
         Hud.OpenPickUpPanel(collectibleCol.type.ToString());
      }

      //DISPENSER
      if (dispenserCol != null) {
         Dispenser = dispenserCol.gameObject;
         canOrder = true;
         Hud.OpenDispenserPanel(dispenserCol.type.ToString());
      }

   }

   private void OnTriggerExit(Collider other) {
      Pickable = null;
      canPickUp = false;

      Dispenser = null;
      canOrder = false;

      canFeed = false;
      canCollect = false;

      canAddIngredient = false;
      canCollectDough = false;

      canPutDoughHoven = false;
      canCollectBread = false;

      canSell = false;
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
         moveSpeed = slowSpeed;
      }
   }

   private void PutDown() {
      if (Input.GetKeyDown(KeyCode.E) && Holding != null && !canFeed) {
         //Place Object
         Holding.transform.position = ((player.transform.forward * 1f) + player.transform.position) + new Vector3(0, 2, 0);
         Holding.SetActive(true);
         Holding.GetComponent<Rigidbody>().AddForce(player.transform.forward * 1f, ForceMode.Impulse);

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
      moveSpeed = normalSpeed;
   }

   //LEVAIN
   private void Feeding() {
      if (Input.GetKeyDown(KeyCode.E)) {
         levain.Feeding(currentCollectibeType);
      }
   }

   private void CollectingLevain() {
      if (Input.GetKeyDown(KeyCode.E)) {
         levain.Collecting();
      }
   }

   //DOUGH
   private void AddIngredient() {
      if (Input.GetKeyDown(KeyCode.E)) {
         doughMaker.AddIngredient(currentCollectibeType);
      }
   }

   private void CollectingDough() {
      if (Input.GetKeyDown(KeyCode.E)) {
         doughMaker.Collecting();
      }
   }

   //BREAD
   private void PutDoughHoven() {
      if (Input.GetKeyDown(KeyCode.E)) {
         hoven.PutDoughHoven();
      }
   }

   private void CollectingBread() {
      if (Input.GetKeyDown(KeyCode.E)) {
         hoven.Collecting();
      }
   }

   //COUNTER
   private void SellBread() {
      if (Input.GetKeyDown(KeyCode.E)) {
         counter.Sell();
      }
   }

   //OTHER
   public void Consume() {
      Destroy(Holding);
      LetGo();
   }
}