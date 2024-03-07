using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
   [SerializeField] Image loadingBarImage;
   [SerializeField] Health health;
   void Start()
   {
       health.currentHealth.OnValueChanged += UpdateUI;
   }

   private void UpdateUI(int previousValue, int newValue)
   {
       loadingBarImage.fillAmount = (float)newValue / 100;
   }
}
