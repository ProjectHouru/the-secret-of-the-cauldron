using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarUI
   : BasePanel
{
   private List<VisualElement> _healthIcons;
   
   public HealthBarUI(VisualElement panel): base (panel)
   {
      _healthIcons = panel.Q<VisualElement>("HealthValues").Query<VisualElement>("Heart").ToList();
   }
   
   public void Display(int value)
   {
      int i = 0;
      foreach (var healthIcon in _healthIcons)
      {
         if (value > i)
         {
            healthIcon.transform.scale = Vector3.one;
         }
         else
         {
            healthIcon.transform.scale = Vector3.zero;
         }

         i++;
      }
   }
}