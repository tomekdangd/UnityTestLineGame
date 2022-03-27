namespace UnityTestLineGame
{
   using System.Collections;
   using System.Collections.Generic;
   using System.IO;
   using System.Runtime.Serialization.Formatters.Binary;
   using UnityEngine;

   public class SaveLoadData
   {
      public void SaveHighestScore(int highestScore)
      {
         BinaryFormatter bf = new BinaryFormatter();
         FileStream fs = new FileStream(Application.persistentDataPath + "/score.dat", FileMode.Create);

         HighestScore dat = new HighestScore();

         dat.pub_highestScore = highestScore;

         bf.Serialize(fs, dat);

         fs.Close();
      }
      
      public int LoadHighestScore()
      {
         int highestScore = 0;
         BinaryFormatter bf = new BinaryFormatter();
         
         if (File.Exists(Application.persistentDataPath + "/score.dat"))
         {
            FileStream fs = new FileStream(Application.persistentDataPath + "/score.dat", FileMode.Open);
            if (fs.Length > 0)
            {
               HighestScore dat = (HighestScore)bf.Deserialize(fs);

               highestScore = dat.pub_highestScore;
            }

            fs.Close();
         }

         return highestScore;
      }

      public void SaveBoard(int score, float timer, Ball[] boardTiles)
      {
         BinaryFormatter bf = new BinaryFormatter();
         FileStream fs = new FileStream(Application.persistentDataPath + "/board.dat", FileMode.Create);

         BoardData dat = new BoardData();

         dat.pub_currentScore = score;
         dat.pub_timer = timer;
         for(int i = 0; i < boardTiles.Length; i++)
         {
            if(boardTiles[i] != null)
            {
               dat.AddBall(boardTiles[i]);
            }
         }

         bf.Serialize(fs, dat);

         fs.Close();
      }

      public void LoadBoard(Board board)
      {
         BinaryFormatter bf = new BinaryFormatter();

         if (File.Exists(Application.persistentDataPath + "/board.dat"))
         {
            FileStream fs = new FileStream(Application.persistentDataPath + "/board.dat", FileMode.Open);
            if (fs.Length > 0)
            {
               BoardData dat = (BoardData)bf.Deserialize(fs);

               board.pub_score = dat.pub_currentScore;
               board.pub_timer = dat.pub_timer;
               Line98StaticData.LINE_98.pub_lineGameUI.SetCurrentScore(board.pub_score);
               Line98StaticData.LINE_98.pub_lineGameUI.SetTimer((int)board.pub_timer);
               board.ClearBoard();
               for (int i = 0; i < dat.pub_ballDataList.Count; i++)
               {
                  board.SetBallAtIdx(dat.pub_ballDataList[i].pub_ballIdx, dat.pub_ballDataList[i].pub_ballColor, dat.pub_ballDataList[i].pub_ballType, dat.pub_ballDataList[i].pub_isQueuing);
               }
            }

            fs.Close();
         }
      }
   }

   [System.Serializable]
   public class HighestScore
   {
      public int pub_highestScore;
   }

   [System.Serializable]
   public class BoardData
   {
      public int pub_currentScore;
      public float pub_timer;
      public List<BallData> pub_ballDataList = new List<BallData>();

      public void AddBall(Ball ball)
      {
         BallData dat = new BallData();
         dat.pub_ballColor = ball.pub_ballColor;
         dat.pub_ballType = ball.pub_ballType;
         dat.pub_ballIdx = ball.pub_idx;
         if(ball.pub_ballStateMachine.GetCurrentState() == ball.pub_ballStateQueue)
         {
            dat.pub_isQueuing = true;
         }
         pub_ballDataList.Add(dat);
      }
   }

   [System.Serializable]
   public class BallData
   {
      public int pub_ballColor;
      public int pub_ballType;
      public int pub_ballIdx;
      public bool pub_isQueuing;
   }
}

