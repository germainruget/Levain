using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TransitionParameter {
   Move,
   Holding,
   Pouring,
   GameOver,
   GameWin
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

   [Header("Timer")]
   public float maxTimer = 60f;
   public float currentTimer;

   [Header("Money")]
   public float money = 100.0f;
   [Header("Picking Up")]
   public bool canPickUp;
   public GameObject Pickable;
   [Header("Order")]
   public bool canOrder;
   public GameObject Dispenser;
   [Header("Holding")]
   public GameObject HoldingPos;
   public bool isHolding;
   public CollectibleType currentCollectibeType;
   public GameObject Holding;
   private HoldingPos holdPos;

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
   [Header("Audio")]
   public AudioManager audioManager;

   public bool isPourring;
   private bool isGameOver;

   //HUD
   public HUDScript Hud;

   void Start() {
      //Pause Game on Start
      Time.timeScale = 0;

      player = gameObject;
      levain = GameObject.FindObjectOfType<LevainScript>();
      doughMaker = GameObject.FindObjectOfType<DoughMaker>();
      hoven = GameObject.FindObjectOfType<Hoven>();
      counter = GameObject.FindObjectOfType<Counter>();
      Hud = GameObject.FindObjectOfType<HUDScript>();
      audioManager = GameObject.FindObjectOfType<AudioManager>();
      moveSpeed = normalSpeed;
      currentTimer = 0;
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
      } else if (canOrder && Input.GetKeyDown(KeyCode.E) && money > 0) {
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

      //LEVAIN LIFE POINTS
      if (levain.currentLifePoint <= 0) {
         GameOver();
      }

      //TIMER
      if (currentTimer <= maxTimer) {
         currentTimer += Time.deltaTime;
         Hud.UpdateTimer(currentTimer / maxTimer);
      } else {
         GameWin();
      }

      //TUTORIAL
      if (!isPourring && !isGameOver) {
         if (Input.GetKeyDown(KeyCode.F)) {
            Hud.ShowTutorial();
         }
         if (Input.GetKeyUp(KeyCode.F)) {
            Hud.HideTutorial();
         }
      }

      if (Input.GetKeyDown(KeyCode.Escape)) {
         Hud.ShowPauseMenu();
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

      if ((Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0) && !isPourring && !isGameOver) {
         Move(desiredDirection);
         Turn(desiredDirection);
         animator.SetBool(TransitionParameter.Move.ToString(), true);
      } else {
         animator.SetBool(TransitionParameter.Move.ToString(), false);
      }

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

   private void OnTriggerStay(Collider other) {
      Collectible collectibleCol = other.GetComponent<Collectible>();
      LevainScript levainCol = other.GetComponent<LevainScript>();
      DoughMaker doughMakerCol = other.GetComponent<DoughMaker>();
      Hoven hovenCol = other.GetComponent<Hoven>();
      Dispenser dispenserCol = other.GetComponent<Dispenser>();
      Counter counterCol = other.GetComponent<Counter>();

      //LEVAIN
      if (levainCol != null) {
         if (isHolding && !isPourring) {
            canFeed = true;
            Hud.OpenLevainPanel("Press E to feed " + currentCollectibeType);
         } else {
            Hud.OpenLevainPanel("Press E to collect starter");
            canCollect = true;
         }
      }

      //DOUGH
      if (doughMakerCol != null) {
         if (isHolding && !isPourring) {
            canAddIngredient = true;
            Hud.OpenDoughMakerPanel("Press E to add " + currentCollectibeType);
         } else {
            Hud.OpenDoughMakerPanel("Press E to collect dough");
            canCollectDough = true;
         }
      }

      //HOVEN
      if (hovenCol != null) {
         if (isHolding && !isPourring && currentCollectibeType == CollectibleType.Dough) {
            canPutDoughHoven = true;
            Hud.OpenHovenMakerPanel("Press E to add bread");
         } else {
            Hud.OpenHovenMakerPanel("Press E to collect bread");
            canCollectBread = true;
         }
      }

      //COUNTER
      if (counterCol != null) {
         if (isHolding && !isPourring) {
            canSell = true;
            Hud.OpenCounterPanel();
         }
      }

      //COLLECT
      if (collectibleCol != null) {
         Pickable = collectibleCol.gameObject;
         canPickUp = true;
         Hud.OpenPickUpPanel(collectibleCol.type.ToString(), collectibleCol.gameObject);
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

         HoldingPos[] holdingObjects = HoldingPos.GetComponent<HoldingType>().holdingObjects;
         holdPos = Array.Find(holdingObjects, hold => hold.type == currentCollectibeType);
         holdPos.obj.SetActive(true);

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
         holdPos.obj.SetActive(false);
         Holding.transform.position = ((player.transform.forward * 1f) + player.transform.position) + new Vector3(0, 2, 0);
         Holding.SetActive(true);
         Holding.GetComponent<Rigidbody>().AddForce(player.transform.forward * 3f, ForceMode.Impulse);

         LetGo();

      }
   }

   private void LetGo() {
      //Let Go
      Holding = null;
      currentCollectibeType = CollectibleType.None;

      holdPos.obj.SetActive(false);

      isHolding = false;
      isPourring = false;

      animator.SetBool(TransitionParameter.Holding.ToString(), false);
      animator.SetBool(TransitionParameter.Pouring.ToString(), false);

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
         canPutDoughHoven = false;
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
         canSell = false;
         counter.Sell();
      }
   }

   public IEnumerator Consume() {
      isPourring = true;
      animator.SetBool(TransitionParameter.Pouring.ToString(), true);
      GameObject.FindObjectOfType<AudioManager>().PlaySound(currentCollectibeType);

      currentCollectibeType = CollectibleType.None;

      Destroy(Holding);

      yield return new WaitForSeconds(1.5f);
      Hud.ClosePanels();
      LetGo();
   }

   private void GameOver() {
      Hud.ShowLoseMenu();
      isGameOver = true;
      animator.SetBool(TransitionParameter.Pouring.ToString(), false);
      animator.SetBool(TransitionParameter.Move.ToString(), false);
      animator.SetBool(TransitionParameter.Holding.ToString(), false);

      animator.SetBool(TransitionParameter.GameOver.ToString(), true);
   }

   private void GameWin() {
      Hud.ShowWinMenu();
      isGameOver = true;
      animator.SetBool(TransitionParameter.Pouring.ToString(), false);
      animator.SetBool(TransitionParameter.Move.ToString(), false);
      animator.SetBool(TransitionParameter.Holding.ToString(), false);

      animator.SetBool(TransitionParameter.GameWin.ToString(), true);
   }
}