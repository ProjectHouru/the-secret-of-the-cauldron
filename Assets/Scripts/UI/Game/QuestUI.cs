using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestUI
   : BasePanel
{
   private VisualElement _currentQuest;
   
   public VisualElement QuestVisualElement => _currentQuest;
   
   public QuestUI(VisualElement panel): base (panel)
   {
      _currentQuest = panel.Q<VisualElement>("QuestList").Q<VisualElement>("Quest");
   }
   
   public void Display(string questText)
   {
      _currentQuest.Q<Label>().text = questText;
   }
}