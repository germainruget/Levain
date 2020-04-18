using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour {
   private bool panelOpen;

   [Header("Pick Up Panels")]
   public GameObject PickUpPanel;
   public TextMeshProUGUI pickUpChangingText;

   [Header("Dispenser Panels")]
   public GameObject DispenserPanel;
   public TextMeshProUGUI dispenserChangingText;

   [Header("Levain Panels")]
   public GameObject LevainPanel;
   public TextMeshProUGUI levainChangingText;

   [Header("Sliders")]
   public Slider slider;

   [Header("HUD Levain")]
   public Image HUDLevain;
   public Sprite goodLevain;
   public Sprite hmmmLevain;
   public Sprite badLevain;

   [Header("HUD Levain Needs")]
   public TextMeshProUGUI waterNeed;
   public TextMeshProUGUI wheatNeed;

   // Start is called before the first frame update
   void Start() {

   }

   // Update is called once per frame
   void Update() { }

   public void UpdateLevainIcon(Mood mood) {
      switch (mood) {
         case Mood.Happy:
            HUDLevain.GetComponent<Image>().sprite = goodLevain;
            break;
         case Mood.Hmmmm:
            HUDLevain.GetComponent<Image>().sprite = hmmmLevain;
            break;
         case Mood.Bad:
            HUDLevain.GetComponent<Image>().sprite = badLevain;
            break;
         default:
            HUDLevain.GetComponent<Image>().sprite = goodLevain;
            break;
      }
   }

   public void UpdateLife(float value) {
      slider.value = value;
   }

   public void ChangeWaterNeed(int need) {
      waterNeed.text = need.ToString();
   }

   public void ChangeWheatNeed(int need) {
      wheatNeed.text = need.ToString();
   }

   public void OpenPickUpPanel(string text) {
      if (!panelOpen) {
         pickUpChangingText.text = text;
         PickUpPanel.SetActive(true);
         panelOpen = true;
      }
   }

   public void OpenDispenserPanel(string text) {
      Debug.Log(text);
      if (!panelOpen) {
         dispenserChangingText.text = text;
         DispenserPanel.SetActive(true);
         panelOpen = true;
      }
   }

   public void OpenLevainPanel(string text) {
      if (!panelOpen) {
         panelOpen = true;
         levainChangingText.text = text;
         LevainPanel.SetActive(true);
      }
   }

   public void ClosePanels() {
      panelOpen = false;
      PickUpPanel.SetActive(false);
      DispenserPanel.SetActive(false);
      LevainPanel.SetActive(false);
   }

}