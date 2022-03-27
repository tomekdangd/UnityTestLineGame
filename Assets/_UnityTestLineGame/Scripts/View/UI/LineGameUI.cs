namespace UnityTestLineGame
{
   using System.Collections;
   using System.Collections.Generic;
   using UnityEngine;
   using UnityEngine.UI;

   public class LineGameUI : MonoBehaviour
   {
      public TMPro.TMP_Text pub_highestScoreText;
      public TMPro.TMP_Text pub_currentScoreText;
      public TMPro.TMP_Text pub_timerText;

      public Button pub_saveButton;

      public Button pub_bombButton;

      private void Start()
      {
         pub_saveButton.onClick.AddListener(() => SaveBoard());
         pub_bombButton.onClick.AddListener(() => UseBomb());
      }

      private void SaveBoard()
      {
         if(GameStateStaticData.GAME_STATE_MACHINE.GetCurrentState() == GameStateStaticData.GAME_STATE_START_GAME)
         {
            (GameStateStaticData.GAME_STATE_MACHINE.GetCurrentState() as GameStateStartGame).SaveBoard();
         }
      }

      private void UseBomb()
      {
         if (GameStateStaticData.GAME_STATE_MACHINE.GetCurrentState() == GameStateStaticData.GAME_STATE_START_GAME)
         {
            (GameStateStaticData.GAME_STATE_MACHINE.GetCurrentState() as GameStateStartGame).UseBomb();
         }
      }

      public void SetHighestScore(int score)
      {
         pub_highestScoreText.text = score.ToString();
      }

      public void SetCurrentScore(int score)
      {
         pub_currentScoreText.text = score.ToString();
      }

      public void SetTimer(int currentTime)
      {
         int hours = currentTime / 3600;
         int minutes = (currentTime - (hours * 3600)) / 60;
         int seconds = currentTime - (hours * 3600) - (minutes * 60);

         pub_timerText.text = string.Format("{0}:{1}:{2}", hours.ToString("00"), minutes.ToString("00"), seconds.ToString("00"));
      }
   }
}

