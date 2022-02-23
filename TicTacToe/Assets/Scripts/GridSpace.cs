using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
   public Button button;
   public Text buttonText;

   private GameController gameController;

   public void SetSpace()
   {
       if(gameController.playerMove == true)
       {
            buttonText.text = gameController.GetPlayerSide();
            button.interactable = false;
            gameController.EndTurn();
       }
   }

   public void SetGameControllerReference(GameController controller)
   {
       gameController = controller;
   }
}