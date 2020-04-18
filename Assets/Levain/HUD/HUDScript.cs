using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour {
   private bool panelOpen;

   [Header("Pick Up Panel")]
   public GameObject PickUpPanel;
   public TextMeshProUGUI pickUpChangingText;

   [Header("Dispenser Panel")]
   public GameObject DispenserPanel;
   public TextMeshProUGUI dispenserChangingText;

   [Header("Levain Panel")]
   public GameObject LevainPanel;
   public TextMeshProUGUI levainChangingText;

   [Header("Dough Maker Panel")]
   public GameObject DoughMakerPanel;
   public TextMeshProUGUI doughMakerChangingText;

   [Header("Hoven Panel")]
   public GameObject HovenPanel;
   public TextMeshProUGUI hovenChangingText;

   [Header("Money Panel")]
   public TextMeshProUGUI moneyChangingText;

   [Header("Counter Panel")]
   public GameObject CounterPanel;

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

   public void UpdateMoney(float value) {
      moneyChangingText.text = value.ToString() + " €";
   }

   public void ChangeWaterNeed(int need) {
      waterNeed.text = need.ToString();
   }

   public void ChangeFloorNeed(int need) {
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

   public void OpenBreadMakerPanel(string text) {
      if (!panelOpen) {
         panelOpen = true;
         doughMakerChangingText.text = text;
         DoughMakerPanel.SetActive(true);
      }
   }

   public void OpenHovenMakerPanel(string text) {
      if (!panelOpen) {
         panelOpen = true;
         hovenChangingText.text = text;
         HovenPanel.SetActive(true);
      }
   }

   public void OpenCounterPanel() {
      if (!panelOpen) {
         panelOpen = true;
         CounterPanel.SetActive(true);
      }
   }

   public void ClosePanels() {
      panelOpen = false;
      PickUpPanel.SetActive(false);
      DispenserPanel.SetActive(false);
      LevainPanel.SetActive(false);
      DoughMakerPanel.SetActive(false);
      HovenPanel.SetActive(false);
      CounterPanel.SetActive(false);
   }

}