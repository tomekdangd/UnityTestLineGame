namespace UnityTestLineGame
{
   using System.Collections;
   using System.Collections.Generic;
   using UnityEngine;

   public class GameStateStartGame : GameState
   {
      private Ball pri_selectedBall;
      private bool pri_isMovingBall;
      private Ball pri_movingBall;
      private List<PathNode> pri_pathList = new List<PathNode>();
      private int pri_currentPathIdx;

      public void SaveBoard()
      {
         SaveLoadData saveLoadData = new SaveLoadData();
         saveLoadData.SaveBoard(pro_board.pub_score, pro_board.pub_timer, pro_board.BoardTiles);
      }

      public void UseBomb()
      {
         pro_board.UseBomb();
      }

      public override void OnEnterState()
      {
         pri_selectedBall = null;
         pri_isMovingBall = false;
      }

      public override void OnExecuteState(float time)
      {
         pro_board.MapTick(time);
         if(pri_isMovingBall)
         {
            MoveBall(time);
         }
      }

      private void MoveBall(float time)
      {
         Vector3 currentBallPos = pri_movingBall.pub_ballView.transform.position;
         Vector3 currentTilePos = pro_board.pub_boardView.BoardTilesView[pri_pathList[pri_currentPathIdx].pub_idx].transform.position;
         float speed = pri_movingBall.pub_ballView.pub_movingSpeed * time;
         pri_movingBall.pub_ballView.transform.position = Vector3.MoveTowards(currentBallPos, currentTilePos, speed);
         if(pri_movingBall.pub_ballView.transform.position == pro_board.pub_boardView.BoardTilesView[pri_pathList[pri_currentPathIdx].pub_idx].transform.position)
         {
            pri_currentPathIdx++;
            if (pri_currentPathIdx >= pri_pathList.Count)
            {
               // Set Ball Pos to new Pos in tile
               pro_board.BoardTiles[pri_pathList[0].pub_idx] = null;
               pro_board.BoardTiles[pri_pathList[pri_pathList.Count - 1].pub_idx] = pri_movingBall;
               pri_movingBall.pub_idx = pri_pathList[pri_pathList.Count - 1].pub_idx;
               pro_board.MoveBall(pri_pathList[0].pub_idx, pri_pathList[pri_pathList.Count - 1].pub_idx);
               pro_board.CheckEatAble(pri_movingBall.pub_idx);
               // reached the end
               pri_isMovingBall = false;
               pri_movingBall = null;
               //pro_board.ShowList();
            }
         }
      }

      public override void OnExitState()
      {
         base.OnExitState();
         pri_selectedBall = null;
      }

      public override void OnHandleInput(float time)
      {
         if(pri_isMovingBall)
         {
            return;
         }
         GameObject hitObj = Line98StaticData.PLAYER_CONTROLLER.PlayerInput();
         if(hitObj != null)
         {
            int idx = int.Parse(hitObj.name);
            if(pri_selectedBall == null)
            {
               HandleClickedBoardTile(idx);
            }
            else
            {
               HandleSelectedBall(idx);
            }
         }
      }

      private void HandleClickedBoardTile(int idx)
      {
         pri_selectedBall = pro_board.GetBallAtTile(idx);
         if(pri_selectedBall != null)
         {
            if(pri_selectedBall.pub_ballStateMachine.GetCurrentState() != pri_selectedBall.pub_ballStateQueue &&
               pri_selectedBall.pub_ballType != (int)Line98StaticData.BallType.Freeze)
            {
               pri_selectedBall.pub_ballStateSelected.SetBall(pri_selectedBall);
               pri_selectedBall.pub_ballStateMachine.SetState(pri_selectedBall.pub_ballStateSelected);
            }
            else
            {
               pri_selectedBall = null;
            }
         }
      }

      private void HandleSelectedBall(int idx)
      {
         Ball ball = pro_board.GetBallAtTile(idx);
         // if the tile got no ball or the ball still in queue can check path
         if (ball == null || ball.pub_ballStateMachine.GetCurrentState() == ball.pub_ballStateQueue)
         {
            pri_pathList = pro_board.FindPath(pri_selectedBall.GetCurrentIdx(), idx);

            if (pri_pathList != null)
            {
               pri_isMovingBall = true;
               pri_movingBall = pri_selectedBall;
               pri_currentPathIdx = 0;
            }
         }
         pri_selectedBall.pub_ballView.transform.rotation = Quaternion.identity;
         pri_selectedBall.pub_ballStateIdle.SetBall(pri_selectedBall);
         pri_selectedBall.pub_ballStateMachine.SetState(pri_selectedBall.pub_ballStateIdle);
         pri_selectedBall = null;
      }
   }
}

