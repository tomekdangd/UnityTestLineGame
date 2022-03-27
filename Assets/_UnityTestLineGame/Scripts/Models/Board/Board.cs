namespace UnityTestLineGame
{
   using System.Collections;
   using System.Collections.Generic;
   using UnityEngine;

   [System.Serializable]
   public class Board
   {
      private int pri_size;

      public int pub_score;

      public float pub_timer;

      public int pub_highestScore;

      public Ball[] BoardTiles { get; set; }

      public BoardView pub_boardView;

      private List<int> pri_emptyTileList = new List<int>();

      public Ball[] QueueBall = new Ball[3];

      private List<PathNode> pri_openList = new List<PathNode>();
      private List<PathNode> pri_closeList = new List<PathNode>();

      private List<PathNode> pri_allNode = new List<PathNode>();

      public void Init(int size, int ballNum)
      {
         pri_size = size;

         pub_boardView = GameObject.Instantiate<BoardView>(Line98StaticData.LINE_98.pub_boardViewPrefab);
         Line98StaticData.ResetInstantiatedObj(pub_boardView.transform, Line98StaticData.LINE_98.transform);

         InitBoard();

         pub_boardView.InitBoardView(pri_size);

         for (int i = 0; i < Line98StaticData.LINE_98.pub_numberOfBallAtStart; i++)
         {
            SpawnRandomBallAtInit();
         }

         GetQueueBall();

         pub_timer = 0;
         pub_score = 0;
         Line98StaticData.LINE_98.pub_lineGameUI.SetTimer((int)pub_timer);
         Line98StaticData.LINE_98.pub_lineGameUI.SetCurrentScore(pub_score);

         //ShowList();
      }

      private void InitBoard()
      {
         pri_emptyTileList.Clear();
         pri_allNode.Clear();
         BoardTiles = new Ball[pri_size * pri_size];
         for (int i = 0; i < BoardTiles.Length; i++)
         {
            BoardTiles[i] = null;
            pri_emptyTileList.Add(i);
            PathNode node = new PathNode(i);
            node.pub_gCost = int.MaxValue;
            node.CalculateFCost();
            node.pub_cameFrom = null;
            pri_allNode.Add(node);
         }
      }

      public void UseBomb()
      {
         if(pub_score >= 50)
         {
            pub_score -= 50;
            Line98StaticData.LINE_98.pub_lineGameUI.SetCurrentScore(pub_score);
         }
         else
         {
            return;
         }
         for(int i = 0; i < 10; i++)
         {
            int randomIdx = Random.Range(0, BoardTiles.Length);
            if(BoardTiles[randomIdx] != null)
            {
               Ball ball = BoardTiles[randomIdx];
               ball.pub_ballStateDisable.SetBall(ball);
               ball.pub_ballStateMachine.SetState(ball.pub_ballStateDisable);
               BoardTiles[randomIdx] = null;
               pri_emptyTileList.Add(randomIdx);

               GameObject bomb = pub_boardView.GetUnusedBomb();
               bomb.transform.position = new Vector3(pub_boardView.BoardTilesView[randomIdx].transform.position.x, bomb.transform.position.y, pub_boardView.BoardTilesView[randomIdx].transform.position.z);
               bomb.SetActive(true);
            }
         }
      }

      public void SetBallAtIdx(int idx, int ballColor, int ballType, bool isQueuing)
      {
         Ball ball = new Ball();
         BallView ballView = pub_boardView.GetUnusedBall();

         int specialBall = ballType;
         int normalBall = ballColor;

         MaterialPropertyBlock block = new MaterialPropertyBlock();
         ballView.pub_meshRenderer.sharedMaterial = Line98StaticData.LINE_98.pub_ballMaterial;
         ballView.pub_meshRenderer.GetPropertyBlock(block);
         Color color = Line98StaticData.LINE_98.pub_colorType[0];
         if (specialBall != (int)Line98StaticData.BallType.Flexible)
         {
            color = Line98StaticData.LINE_98.pub_colorType[normalBall];
         }

         if (Line98StaticData.LINE_98.pub_specialType[specialBall] != null)
         {
            block.SetTexture("_BaseMap", Line98StaticData.LINE_98.pub_specialType[specialBall]);
         }

         block.SetColor("_BaseColor", color);
         ballView.pub_meshRenderer.SetPropertyBlock(block);

         ball.Init(idx, ballView, specialBall, normalBall);

         ballView.transform.position = pub_boardView.BoardTilesView[idx].transform.position;
         ballView.gameObject.SetActive(true);
         if(isQueuing)
         {
            ball.pub_ballStateQueue.SetBall(ball);
            ball.pub_ballStateMachine.SetState(ball.pub_ballStateQueue);
            for(int i = 0; i < QueueBall.Length; i++)
            {
               if(QueueBall[i] == null)
               {
                  QueueBall[i] = ball;
                  break;
               }
            }
         }

         BoardTiles[idx] = ball;

         pri_emptyTileList.Remove(idx);
      }

      public void ClearBoard()
      {
         pri_emptyTileList.Clear();
         for(int i = 0; i < BoardTiles.Length; i++)
         {
            if(BoardTiles[i] != null)
            {
               BoardTiles[i].pub_ballView.gameObject.SetActive(false);
            }
            BoardTiles[i] = null;
            pri_emptyTileList.Add(i);
            PathNode node = pri_allNode[i];
            node.pub_gCost = int.MaxValue;
            node.CalculateFCost();
            node.pub_cameFrom = null;
         }

         for(int i = 0; i < QueueBall.Length; i++)
         {
            QueueBall[i] = null;
         }

         //ShowList();
      }

      public void ShowList()
      {
         string display = "";
         int currentRow = 0;
         for(int i = 0; i < BoardTiles.Length; i++)
         {
            int row = i / pri_size;

            if (currentRow != row)
            {
               currentRow = row;
               Debug.Log(display);
               display = "";
            }
            display += "[";
            
            if (BoardTiles[i] == null)
            {
               display += "-1]";
            }
            else
            {
               display += BoardTiles[i].pub_ballColor.ToString() + "]";
            }
         }
         Debug.Log(display);
      }

      private void GetQueueBall()
      {
         if(pri_emptyTileList.Count <= 1)
         {
            // Lose Game
            SaveLoadData saveLoadData = new SaveLoadData();
            saveLoadData.SaveHighestScore(pub_highestScore);
            ClearBoard();

            for (int i = 0; i < Line98StaticData.LINE_98.pub_numberOfBallAtStart; i++)
            {
               SpawnRandomBallAtInit();
            }

            GetQueueBall();

            pub_timer = 0;
            pub_score = 0;
            Line98StaticData.LINE_98.pub_lineGameUI.SetTimer((int)pub_timer);
            Line98StaticData.LINE_98.pub_lineGameUI.SetCurrentScore(pub_score);

            return;
         }
         for(int i = 0; i < 3; i++)
         {
            if(pri_emptyTileList.Count > 0)
            {
               int randomIdx = Random.Range(0, pri_emptyTileList.Count);
               Ball ball = GetRandomBall(randomIdx);
               ball.pub_ballStateQueue.SetBall(ball);
               ball.pub_ballStateMachine.SetState(ball.pub_ballStateQueue);
               QueueBall[i] = ball;
               BoardTiles[pri_emptyTileList[randomIdx]] = ball;
               pri_emptyTileList.RemoveAt(randomIdx);
            }
         }
      }

      public void CheckEatAble(int idx, bool addQueueBall = true)
      {
         List<Ball> leftRightList = new List<Ball>();
         List<Ball> upDownList = new List<Ball>();
         List<Ball> diagLeftList = new List<Ball>();
         List<Ball> diagRightList = new List<Ball>();
         Ball movedBall = BoardTiles[idx];

         GetRightTileBall(idx, movedBall, leftRightList);
         GetLeftTileBall(idx, movedBall, leftRightList);

         GetUpTileBall(idx, movedBall, upDownList);
         GetDownTileBall(idx, movedBall, upDownList);

         GetUpLeftTileBall(idx, movedBall, diagLeftList);
         GetLowRightTileBall(idx, movedBall, diagLeftList);

         GetUpRightTileBall(idx, movedBall, diagRightList);
         GetLowLeftTileBall(idx, movedBall, diagRightList);

         GetPointAndDestroyBall(leftRightList);
         GetPointAndDestroyBall(upDownList);
         GetPointAndDestroyBall(diagLeftList);
         GetPointAndDestroyBall(diagRightList);

         if(leftRightList.Count >= 4 || upDownList.Count >= 4 || diagLeftList.Count >= 4 || diagRightList.Count >= 4)
         {
            pub_score += 1;
            movedBall.pub_ballStateDisable.SetBall(movedBall);
            movedBall.pub_ballStateMachine.SetState(movedBall.pub_ballStateDisable);
            BoardTiles[movedBall.pub_idx] = null;
            pri_emptyTileList.Add(movedBall.pub_idx);

            Line98StaticData.LINE_98.pub_lineGameUI.SetCurrentScore(pub_score);
            if(pub_score > pub_highestScore)
            {
               pub_highestScore = pub_score;
               Line98StaticData.LINE_98.pub_lineGameUI.SetHighestScore(pub_highestScore);
            }
         }
         else
         {
            if(addQueueBall)
            {
               for (int i = 0; i < QueueBall.Length; i++)
               {
                  if (BoardTiles[QueueBall[i].pub_idx] != null && BoardTiles[QueueBall[i].pub_idx].pub_ballStateMachine.GetCurrentState() == BoardTiles[QueueBall[i].pub_idx].pub_ballStateQueue)
                  {
                     QueueBall[i].pub_ballStateIdle.SetBall(QueueBall[i]);
                     QueueBall[i].pub_ballStateMachine.SetState(QueueBall[i].pub_ballStateIdle);
                     CheckEatAble(QueueBall[i].pub_idx, false);
                  }
                  else
                  {
                     QueueBall[i].pub_ballView.gameObject.SetActive(false);
                  }
               }
               GetQueueBall();
            }
         }
      }

      private void GetPointAndDestroyBall(List<Ball> ballList)
      {
         if(ballList.Count >= 4)
         {
            for(int i = 0; i < ballList.Count; i++)
            {
               ballList[i].pub_ballStateDisable.SetBall(ballList[i]);
               ballList[i].pub_ballStateMachine.SetState(ballList[i].pub_ballStateDisable);
               BoardTiles[ballList[i].pub_idx] = null;
               pri_emptyTileList.Add(ballList[i].pub_idx);
            }
            pub_score += ballList.Count;
         }
      }

      private void GetRightTileBall(int idx, Ball movedBall, List<Ball> ballList)
      {
         int checkIdx = GetRightTileIdx(idx);
         Ball checkBall;
         if (checkIdx != -1)
         {
            checkBall = BoardTiles[checkIdx];
         }
         else
         {
            checkBall = null;
         }

         while (checkIdx != -1 && checkBall != null && checkBall.pub_ballStateMachine.GetCurrentState() != checkBall.pub_ballStateQueue && 
                movedBall.pub_ballColor == checkBall.pub_ballColor)
         {
            ballList.Add(checkBall);
            checkIdx = GetRightTileIdx(checkIdx);
            if (checkIdx != -1)
            {
               checkBall = BoardTiles[checkIdx];
            }
            else
            {
               checkBall = null;
            }
         }
      }

      private void GetLeftTileBall(int idx, Ball movedBall, List<Ball> ballList)
      {
         int checkIdx = GetLeftTileIdx(idx);
         Ball checkBall;
         if (checkIdx != -1)
         {
            checkBall = BoardTiles[checkIdx];
         }
         else
         {
            checkBall = null;
         }

         while (checkIdx != -1 && checkBall != null && checkBall.pub_ballStateMachine.GetCurrentState() != checkBall.pub_ballStateQueue && 
                movedBall.pub_ballColor == checkBall.pub_ballColor)
         {
            ballList.Add(checkBall);
            checkIdx = GetLeftTileIdx(checkIdx);
            if (checkIdx != -1)
            {
               checkBall = BoardTiles[checkIdx];
            }
            else
            {
               checkBall = null;
            }
         }
      }

      private void GetUpTileBall(int idx, Ball movedBall, List<Ball> ballList)
      {
         int checkIdx = GetUpperTileIdx(idx);
         Ball checkBall;
         if (checkIdx != -1)
         {
            checkBall = BoardTiles[checkIdx];
         }
         else
         {
            checkBall = null;
         }

         while (checkIdx != -1 && checkBall != null && checkBall.pub_ballStateMachine.GetCurrentState() != checkBall.pub_ballStateQueue && 
                movedBall.pub_ballColor == checkBall.pub_ballColor)
         {
            ballList.Add(checkBall);
            checkIdx = GetUpperTileIdx(checkIdx);
            if (checkIdx != -1)
            {
               checkBall = BoardTiles[checkIdx];
            }
            else
            {
               checkBall = null;
            }
         }
      }

      private void GetDownTileBall(int idx, Ball movedBall, List<Ball> ballList)
      {
         int checkIdx = GetLowerTileIdx(idx);
         Ball checkBall;
         if (checkIdx != -1)
         {
            checkBall = BoardTiles[checkIdx];
         }
         else
         {
            checkBall = null;
         }

         while (checkIdx != -1 && checkBall != null && checkBall.pub_ballStateMachine.GetCurrentState() != checkBall.pub_ballStateQueue && 
                movedBall.pub_ballColor == checkBall.pub_ballColor)
         {
            ballList.Add(checkBall);
            checkIdx = GetLowerTileIdx(checkIdx);
            if (checkIdx != -1)
            {
               checkBall = BoardTiles[checkIdx];
            }
            else
            {
               checkBall = null;
            }
         }
      }
      
      private void GetUpLeftTileBall(int idx, Ball movedBall, List<Ball> ballList)
      {
         int checkIdx = GetDiagTileUpLeftIdx(idx);
         Ball checkBall;
         if (checkIdx != -1)
         {
            checkBall = BoardTiles[checkIdx];
         }
         else
         {
            checkBall = null;
         }

         while (checkIdx != -1 && checkBall != null && checkBall.pub_ballStateMachine.GetCurrentState() != checkBall.pub_ballStateQueue && 
                movedBall.pub_ballColor == checkBall.pub_ballColor)
         {
            ballList.Add(checkBall);
            checkIdx = GetDiagTileUpLeftIdx(checkIdx);
            if (checkIdx != -1)
            {
               checkBall = BoardTiles[checkIdx];
            }
            else
            {
               checkBall = null;
            }
         }
      }

      private void GetLowRightTileBall(int idx, Ball movedBall, List<Ball> ballList)
      {
         int checkIdx = GetDiagTileLowRightIdx(idx);
         Ball checkBall;
         if (checkIdx != -1)
         {
            checkBall = BoardTiles[checkIdx];
         }
         else
         {
            checkBall = null;
         }

         while (checkIdx != -1 && checkBall != null && checkBall.pub_ballStateMachine.GetCurrentState() != checkBall.pub_ballStateQueue && 
                movedBall.pub_ballColor == checkBall.pub_ballColor)
         {
            ballList.Add(checkBall);
            checkIdx = GetDiagTileLowRightIdx(checkIdx);
            if (checkIdx != -1)
            {
               checkBall = BoardTiles[checkIdx];
            }
            else
            {
               checkBall = null;
            }
         }
      }

      private void GetUpRightTileBall(int idx, Ball movedBall, List<Ball> ballList)
      {
         int checkIdx = GetDiagTileUpRightIdx(idx);
         Ball checkBall;
         if (checkIdx != -1)
         {
            checkBall = BoardTiles[checkIdx];
         }
         else
         {
            checkBall = null;
         }

         while (checkIdx != -1 && checkBall != null && checkBall.pub_ballStateMachine.GetCurrentState() != checkBall.pub_ballStateQueue && 
                movedBall.pub_ballColor == checkBall.pub_ballColor)
         {
            ballList.Add(checkBall);
            checkIdx = GetDiagTileUpRightIdx(checkIdx);
            if (checkIdx != -1)
            {
               checkBall = BoardTiles[checkIdx];
            }
            else
            {
               checkBall = null;
            }
         }
      }

      private void GetLowLeftTileBall(int idx, Ball movedBall, List<Ball> ballList)
      {
         int checkIdx = GetDiagTileLowLeftIdx(idx);
         Ball checkBall;
         if (checkIdx != -1)
         {
            checkBall = BoardTiles[checkIdx];
         }
         else
         {
            checkBall = null;
         }

         while (checkIdx != -1 && checkBall != null && checkBall.pub_ballStateMachine.GetCurrentState() != checkBall.pub_ballStateQueue && 
                movedBall.pub_ballColor == checkBall.pub_ballColor)
         {
            ballList.Add(checkBall);
            checkIdx = GetDiagTileLowLeftIdx(checkIdx);
            if (checkIdx != -1)
            {
               checkBall = BoardTiles[checkIdx];
            }
            else
            {
               checkBall = null;
            }
         }
      }

      public void MoveBall(int startIdx, int endIdx)
      {
         if(!pri_emptyTileList.Contains(startIdx))
         {
            pri_emptyTileList.Add(startIdx);
         }
         if(pri_emptyTileList.Contains(endIdx))
         {
            pri_emptyTileList.Remove(endIdx);
         }
      }

      private void SpawnRandomBallAtInit()
      {
         int randomIdx = Random.Range(0, pri_emptyTileList.Count);

         Ball ball = GetRandomBall(randomIdx);

         BoardTiles[pri_emptyTileList[randomIdx]] = ball;

         pri_emptyTileList.RemoveAt(randomIdx);
      }

      private Ball GetRandomBall(int randomIdx)
      {
         Ball ball = new Ball();
         BallView ballView = pub_boardView.GetUnusedBall();

         int specialBall = GetRandomSpecial();
         int normalBall = GetRandomNormal();

         MaterialPropertyBlock block = new MaterialPropertyBlock();
         ballView.pub_meshRenderer.sharedMaterial = Line98StaticData.LINE_98.pub_ballMaterial;
         ballView.pub_meshRenderer.GetPropertyBlock(block);
         Color color = Line98StaticData.LINE_98.pub_colorType[0];
         if (specialBall != (int)Line98StaticData.BallType.Flexible)
         {
            color = Line98StaticData.LINE_98.pub_colorType[normalBall];
         }

         if (Line98StaticData.LINE_98.pub_specialType[specialBall] != null)
         {
            block.SetTexture("_BaseMap", Line98StaticData.LINE_98.pub_specialType[specialBall]);
         }

         block.SetColor("_BaseColor", color);
         ballView.pub_meshRenderer.SetPropertyBlock(block);

         ball.Init(pri_emptyTileList[randomIdx], ballView, specialBall, normalBall);

         ballView.transform.position = pub_boardView.BoardTilesView[pri_emptyTileList[randomIdx]].transform.position;
         ballView.gameObject.SetActive(true);

         return ball;
      }

      public int GetRandomSpecial()
      {
         int random = 0;
         float randomNum = Random.Range(0.0f, 100.0f);
         if (randomNum <= Line98StaticData.LINE_98.pub_spawnChance)
         {
            random = Random.Range(1, System.Enum.GetNames(typeof(Line98StaticData.BallType)).Length);
         }
         return random;
      }

      public int GetRandomNormal()
      {
         int random = -1;
         random = Random.Range(0, System.Enum.GetNames(typeof(Line98StaticData.BallColor)).Length);
         return random;
      }

      public List<PathNode> FindPath(int startIdx, int endIdx)
      {
         pri_openList.Clear();
         pri_closeList.Clear();
         // Reset All Node
         for (int i = 0; i < pri_allNode.Count; i++)
         {
            PathNode node = pri_allNode[i];
            node.pub_isWalkable = true;
            node.pub_gCost = int.MaxValue;
            node.CalculateFCost();
            node.pub_cameFrom = null;
         }

         // FirstNode
         PathNode startNode = pri_allNode[startIdx];
         PathNode endNode = pri_allNode[endIdx];

         pri_openList.Add(startNode);

         startNode.pub_gCost = 0;
         startNode.pub_hCost = CalculateDistance(startNode, endNode);
         startNode.CalculateFCost();

         while(pri_openList.Count > 0)
         {
            PathNode currentNode = GetLowestFCostNode(pri_openList);
            if(currentNode.pub_idx == endNode.pub_idx)
            {
               return CalculatePath(endNode);
            }

            pri_openList.Remove(currentNode);
            pri_closeList.Add(currentNode);

            List<PathNode> neighbourList = GetNeighbourList(currentNode);
            for(int i = 0; i < neighbourList.Count; i++)
            {
               for(int n = 0; n < pri_closeList.Count; n++)
               {
                  if(pri_closeList.Contains(neighbourList[i]))
                  {
                     continue;
                  }
                  if(!neighbourList[i].pub_isWalkable && BoardTiles[startIdx].pub_ballType != (int)Line98StaticData.BallType.Ghost)
                  {
                     pri_closeList.Add(neighbourList[i]);
                     continue;
                  }
                  int newGCost = currentNode.pub_gCost + CalculateDistance(currentNode, neighbourList[i]);
                  if(newGCost < neighbourList[i].pub_gCost)
                  {
                     neighbourList[i].pub_cameFrom = currentNode;
                     neighbourList[i].pub_gCost = newGCost;
                     neighbourList[i].pub_hCost = CalculateDistance(neighbourList[i], endNode);
                     neighbourList[i].CalculateFCost();
                     if(!pri_openList.Contains(neighbourList[i]))
                     {
                        pri_openList.Add(neighbourList[i]);
                     }
                  }
               }
            }
         }

         return null;
      }

      private List<PathNode> GetNeighbourList(PathNode currentNode)
      {
         List<PathNode> neighbourList = new List<PathNode>();
         int idx = GetUpperTileIdx(currentNode.pub_idx);
         AddNodeToNeighbourList(idx, neighbourList);

         idx = GetRightTileIdx(currentNode.pub_idx);
         AddNodeToNeighbourList(idx, neighbourList);

         idx = GetLowerTileIdx(currentNode.pub_idx);
         AddNodeToNeighbourList(idx, neighbourList);

         idx = GetLeftTileIdx(currentNode.pub_idx);
         AddNodeToNeighbourList(idx, neighbourList);

         return neighbourList;
      }

      private void AddNodeToNeighbourList(int idx, List<PathNode> neighbourList)
      {
         if (idx != -1)
         {
            PathNode node = pri_allNode[idx];
            if (BoardTiles[idx] != null && 
                (BoardTiles[idx].pub_ballStateMachine.GetCurrentState() == BoardTiles[idx].pub_ballStateIdle ||
                BoardTiles[idx].pub_ballStateMachine.GetCurrentState() == BoardTiles[idx].pub_ballStateSelected))
            {
               node.pub_isWalkable = false;
            }
            neighbourList.Add(node);
         }
      }

      private List<PathNode> CalculatePath(PathNode endNode)
      {
         List<PathNode> path = new List<PathNode>();
         path.Add(endNode);
         PathNode currentNode = endNode;
         while (currentNode.pub_cameFrom != null)
         {
            path.Add(currentNode.pub_cameFrom);
            currentNode = currentNode.pub_cameFrom;
         }
         path.Reverse();
         return path;
      }

      private int CalculateDistance(PathNode a, PathNode b)
      {
         int aRow = a.pub_idx / pri_size;
         int aCol = a.pub_idx % pri_size;
         int bRow = b.pub_idx / pri_size;
         int bCol = b.pub_idx % pri_size;

         // The cost default is 1 so do not need to multiply to get the cost
         int totalDistance = Mathf.Abs(aRow - bRow) + Mathf.Abs(aCol - bCol);

         return totalDistance;
      }

      private PathNode GetLowestFCostNode(List<PathNode> openNodeList)
      {
         PathNode lowestCostNode = openNodeList[0];
         for(int i = 1; i < openNodeList.Count; i++)
         {
            if(openNodeList[i].pub_fCost < lowestCostNode.pub_fCost)
            {
               lowestCostNode = openNodeList[i];
            }
         }

         return lowestCostNode;
      }

      public void MapTick(float time)
      {
         pub_timer += time;
         Line98StaticData.LINE_98.pub_lineGameUI.SetTimer((int)pub_timer);
         for(int i = 0; i < BoardTiles.Length; i++)
         {
            if(BoardTiles[i] != null)
            {
               BoardTiles[i].BallTick(time);
            }
         }
      }

      public Ball GetBallAtTile(int idx)
      {
         return BoardTiles[idx];
      }

      public int GetUpperTileIdx(int idx)
      {
         int upperTileIdx = idx + pri_size;
         if (upperTileIdx >= BoardTiles.Length)
         {
            upperTileIdx = -1;
         }
         return upperTileIdx;
      }

      public int GetLowerTileIdx(int idx)
      {
         int lowerTileIdx = idx - pri_size;
         if (lowerTileIdx < 0)
         {
            lowerTileIdx = -1;
         }
         return lowerTileIdx;
      }

      public int GetLeftTileIdx(int idx)
      {
         int leftTileIdx = idx - 1;
         int col = idx % pri_size;
         col -= 1;
         if (col < 0)
         {
            leftTileIdx = -1;
         }
         return leftTileIdx;
      }

      public int GetRightTileIdx(int idx)
      {
         int rightTileIdx = idx + 1;
         int col = idx % pri_size;
         col += 1;
         if (col >= pri_size)
         {
            rightTileIdx = -1;
         }
         return rightTileIdx;
      }

      public int GetDiagTileUpLeftIdx(int idx)
      {
         int upLeftIdx = GetUpperTileIdx(idx);
         if(upLeftIdx != -1)
         {
            upLeftIdx = GetLeftTileIdx(upLeftIdx);
         }
         
         return upLeftIdx;
      }

      public int GetDiagTileLowLeftIdx(int idx)
      {
         int lowLeftIdx = GetLowerTileIdx(idx);
         if(lowLeftIdx != -1)
         {
            lowLeftIdx = GetLeftTileIdx(lowLeftIdx);
         }
         

         return lowLeftIdx;
      }

      public int GetDiagTileUpRightIdx(int idx)
      {
         int upRightIdx = GetUpperTileIdx(idx);
         if(upRightIdx != -1)
         {
            upRightIdx = GetRightTileIdx(upRightIdx);
         }
         
         return upRightIdx;
      }

      public int GetDiagTileLowRightIdx(int idx)
      {
         int lowRightIdx = GetLowerTileIdx(idx);
         if(lowRightIdx != -1)
         {
            lowRightIdx = GetRightTileIdx(lowRightIdx);
         }
         
         return lowRightIdx;
      }
   }
}

