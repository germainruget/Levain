using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour {
   private bool panelOpen;

   [Header("Pick Up Panel")]
   public Sprite NeedOK;
   public Sprite NeedNOK;

   [Header("Pick Up Panel")]
   public GameObject PickUpPanel;
   public TextMeshProUGUI pickUpChangingText;

   [Header("Dispenser Panel")]
   public GameObject DispenserPanel;
   public TextMeshProUGUI dispenserChangingText;

   [Header("Levain Panel")]
   public GameObject LevainPanel;
   public TextMeshProUGUI levainChangingText;
   public GameObject LevainUIPlacer;

   [Header("Dough Maker Panel")]
   public GameObject DoughMakerPanel;
   public TextMeshProUGUI doughMakerChangingText;
   public GameObject DoughMakerUIPlacer;

   [Header("Dough Need")]
   public GameObject LevainNeed;
   public GameObject FlourNeed;
   public GameObject WaterNeed;
   public GameObject DoughNeed;

   [Header("Hoven Panel")]
   public GameObject HovenPanel;
   public TextMeshProUGUI hovenChangingText;
   public GameObject HovenUIPlacer;

   [Header("Hoven Need")]
   public GameObject HovenDoughNeed;
   public GameObject HovenBreadNeed;

   [Header("Money Panel")]
   public TextMeshProUGUI moneyChangingText;

   [Header("Counter Panel")]
   public GameObject CounterPanel;
   public GameObject CounterUIPlacer;

   [Header("Sliders")]
   public Image lifePoints;
   public Image timer;

   [Header("HUD Levain")]
   public Image HUDLevain;
   public Sprite goodLevain;
   public Sprite hmmmLevain;
   public Sprite badLevain;

   [Header("HUD Levain Needs")]
   public TextMeshProUGUI waterNeed;
   public TextMeshProUGUI wheatNeed;

   [Header("HUD Message")]
   public GameObject message;
   public GameObject tutorial;
   public GameObject winMenu;
   public GameObject loseMenu;
   public GameObject pauseMenu;

   private GameObject CurrentUIPos;
   private GameObject CurrentUIPanel;

   // Start is called before the first frame update
   void Start() {
      message.SetActive(true);
   }

   // Update is called once per frame
   void Update() {
      if (CurrentUIPos != null && CurrentUIPanel != null) {
         positionUIPanel(CurrentUIPos, CurrentUIPanel);
      }
   }

   private void positionUIPanel(GameObject UIPostion, GameObject UIPanel) {
      Vector3 UIPos = Camera.main.WorldToScreenPoint(UIPostion.transform.position);
      UIPanel.transform.position = UIPos;
   }

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
      // slider.value = value;
      if (value > 0.66f) {
         lifePoints.color = new Color(80f / 255f, 169f / 255f, 93f / 255f);
      } else if (value > 0.33f) {
         lifePoints.color = new Color(211f / 255f, 161f / 255f, 0f / 255f);
      } else {
         lifePoints.color = new Color(211f / 255f, 54f / 255f, 0f / 255f);
      }
      lifePoints.fillAmount = value;
   }

   public void UpdateTimer(float value) {
      timer.fillAmount = value;
   }

   public void UpdateMoney(float value) {
      moneyChangingText.text = value.ToString();
   }

   public void ChangeWaterNeed(int need) {
      waterNeed.text = need.ToString();
   }

   public void ChangeFloorNeed(int need) {
      wheatNeed.text = need.ToString();
   }

   public void ChangeDoughNeed(CollectibleType type) {
      if (type == CollectibleType.Levain) {
         LevainNeed.GetComponent<Image>().sprite = NeedOK;
         LevainNeed.GetComponentInChildren<TextMeshProUGUI>().text = "";
      }
      if (type == CollectibleType.Flour) {
         FlourNeed.GetComponent<Image>().sprite = NeedOK;
         FlourNeed.GetComponentInChildren<TextMeshProUGUI>().text = "";

      }
      if (type == CollectibleType.Water) {
         WaterNeed.GetComponent<Image>().sprite = NeedOK;
         WaterNeed.GetComponentInChildren<TextMeshProUGUI>().text = "";
      }
      if (type == CollectibleType.Dough) {
         DoughNeed.GetComponent<Image>().sprite = NeedOK;
         DoughNeed.GetComponentInChildren<TextMeshProUGUI>().text = "";
      }
   }

   public void ResetDoughNeed() {
      LevainNeed.GetComponent<Image>().sprite = NeedNOK;
      LevainNeed.GetComponentInChildren<TextMeshProUGUI>().text = "1";
      FlourNeed.GetComponent<Image>().sprite = NeedNOK;
      FlourNeed.GetComponentInChildren<TextMeshProUGUI>().text = "1";
      WaterNeed.GetComponent<Image>().sprite = NeedNOK;
      WaterNeed.GetComponentInChildren<TextMeshProUGUI>().text = "1";
      DoughNeed.GetComponent<Image>().sprite = NeedNOK;
      DoughNeed.GetComponentInChildren<TextMeshProUGUI>().text = "1";
   }

   public void ChangeHovenNeed() {
      HovenDoughNeed.GetComponent<Image>().sprite = NeedOK;
      HovenDoughNeed.GetComponentInChildren<TextMeshProUGUI>().text = "";
      HovenBreadNeed.GetComponent<Image>().sprite = NeedOK;
      HovenBreadNeed.GetComponentInChildren<TextMeshProUGUI>().text = "";
   }

   public void ResetHovenNeed() {
      HovenDoughNeed.GetComponent<Image>().sprite = NeedNOK;
      HovenDoughNeed.GetComponentInChildren<TextMeshProUGUI>().text = "1";
      HovenBreadNeed.GetComponent<Image>().sprite = NeedNOK;
      HovenBreadNeed.GetComponentInChildren<TextMeshProUGUI>().text = "1";
   }

   public void OpenPickUpPanel(string text, GameObject currentPickUp) {
      if (!panelOpen) {
         pickUpChangingText.text = text;

         CurrentUIPos = currentPickUp.transform.Find("CollectibleUIPlacer").gameObject;
         CurrentUIPanel = PickUpPanel;

         PickUpPanel.SetActive(true);
         panelOpen = true;
      }
   }

   public void OpenDispenserPanel(string text) {
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

         CurrentUIPos = LevainUIPlacer;
         CurrentUIPanel = LevainPanel;

         LevainPanel.SetActive(true);
      }
   }

   public void OpenDoughMakerPanel(string text) {
      if (!panelOpen) {
         panelOpen = true;
         doughMakerChangingText.text = text;

         CurrentUIPos = DoughMakerUIPlacer;
         CurrentUIPanel = DoughMakerPanel;

         DoughMakerPanel.SetActive(true);
      }
   }

   public void OpenHovenMakerPanel(string text) {
      if (!panelOpen) {
         panelOpen = true;
         hovenChangingText.text = text;

         CurrentUIPos = HovenUIPlacer;
         CurrentUIPanel = HovenPanel;

         HovenPanel.SetActive(true);
      }
   }

   public void OpenCounterPanel() {
      if (!panelOpen) {
         panelOpen = true;

         CurrentUIPos = CounterUIPlacer;
         CurrentUIPanel = CounterPanel;

         CounterPanel.SetActive(true);
      }
   }

   public void ClosePanels() {
      CurrentUIPos = null;
      CurrentUIPanel = null;

      panelOpen = false;
      PickUpPanel.SetActive(false);
      DispenserPanel.SetActive(false);
      LevainPanel.SetActive(false);
      DoughMakerPanel.SetActive(false);
      HovenPanel.SetActive(false);
      CounterPanel.SetActive(false);
   }

   public void CloseMessage() {
      Time.timeScale = 1;
      message.SetActive(false);
   }

   public void ShowTutorial() {
      tutorial.SetActive(true);
   }

   public void HideTutorial() {
      tutorial.SetActive(false);
   }

   public void ShowWinMenu() {
      winMenu.SetActive(true);
   }

   public void ShowLoseMenu() {
      loseMenu.SetActive(true);
   }

   public void ShowPauseMenu() {
      pauseMenu.SetActive(true);
      Time.timeScale = 0;
   }

   public void HidePauseMenu() {
      pauseMenu.SetActive(false);
      Time.timeScale = 1;
   }

   public void Restart() {
      SceneManager.LoadScene(1);
   }

   public void BackMainMenu() {
      SceneManager.LoadScene(0);
   }

   public void QuitGame() {
      Debug.Log("Quit");
      Application.Quit();
   }

}