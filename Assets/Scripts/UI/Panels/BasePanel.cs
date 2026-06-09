using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class BasePanel
{
   protected VisualElement _panel;

   public VisualElement Panel => _panel;
   
   public BasePanel(VisualElement panel)
   {
      _panel = panel;
   }

   public void Show()
   {
      _panel.visible = true;
   }
   
   public void Hide()
   {
      _panel.visible = false;
   }
}