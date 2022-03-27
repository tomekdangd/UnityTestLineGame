namespace UnityTestLineGame
{
   using System.Collections;
   using System.Collections.Generic;
   using UnityEngine;

   public class BoardView : MonoBehaviour
   {
      public GameObject pub_bombEffectPrefab;

      // The number of ball Spawned at start of the game
      public int pub_numBallsStart;

      // Hold all the tile of the board
      public Transform pub_tilesHolder;

      // Hold all the ball of the board
      public Transform pub_ballsHolder;

      // Hold All the bomb on the board;
      public Transform pub_bombsHolder;

      // Hold the tile prefab use to instantiate at the begining of the game
      public TileView pub_tilePrefab;

      // Hold the ball prefab use to instantiate new ball if needed
      public BallView pub_ballPrefab;

      // Hold all the tile object
      [HideInInspector]
      public TileView[] BoardTilesView;

      // Hold all the ball object
      private List<BallView> pri_ballList = new List<BallView>();

      // Hold All the bomb effect Obj
      private List<GameObject> pri_bombList = new List<GameObject>();

      public void InitBoardView(int size)
      {
         BoardTilesView = new TileView[size * size];
         
         // in case width and height is not equal
         float tileWidth = pub_tilePrefab.pub_tileCollider.size.x;
         float tileHeight = pub_tilePrefab.pub_tileCollider.size.z;
         float centerWidth = (int)(size * 0.5f) * tileWidth ;
         float centerHeight = (int)(size * 0.5f) * tileHeight;
         if(size % 2 == 0)
         {
            centerWidth = centerWidth - 1;
            centerHeight = centerHeight - 1;
         }

         for (int i = 0; i < BoardTilesView.Length; i++)
         {
            TileView tile = GameObject.Instantiate<TileView>(pub_tilePrefab);
            tile.pub_tileCollider.name = i.ToString();
            Line98StaticData.ResetInstantiatedObj(tile.transform, pub_tilesHolder);
            SetTilePos(i, tile, tileWidth, tileHeight, size, centerWidth, centerHeight);
            BoardTilesView[i] = tile;
         }
      }

      private void SetTilePos(int idx, TileView tileView, float tileWidth, float tileHeight, int size, float centerWidth, float centerHeight)
      {
         int row = idx / size;
         int col = idx % size;
         float posX = col * tileWidth;
         float posZ = row * tileHeight;

         posX -= centerWidth;
         posZ -= centerHeight;

         tileView.transform.localPosition = new Vector3(posX, 0, posZ);
      }

      public BallView GetUnusedBall()
      {
         BallView ball = null;
         for(int i = 0; i < pri_ballList.Count; i++)
         {
            if(!pri_ballList[i].gameObject.activeInHierarchy)
            {
               ball = pri_ballList[i];
               break;
            }
         }

         if(ball == null)
         {
            ball = GameObject.Instantiate<BallView>(pub_ballPrefab);
            Line98StaticData.ResetInstantiatedObj(ball.transform, pub_ballsHolder);
         }
         ball.pub_scaling = false;
         return ball;
      }

      public GameObject GetUnusedBomb()
      {
         GameObject bomb = null;

         for (int i = 0; i < pri_bombList.Count; i++)
         {
            if (!pri_bombList[i].gameObject.activeInHierarchy)
            {
               bomb = pri_bombList[i];
               break;
            }
         }

         if (bomb == null)
         {
            bomb = GameObject.Instantiate<GameObject>(pub_bombEffectPrefab);
            bomb.transform.SetParent(pub_bombsHolder);
            pri_bombList.Add(bomb);
         }

         return bomb;
      }

      public void Clear()
      {
         for(int i = 0; i < pri_ballList.Count; i++)
         {
            GameObject.Destroy(pri_ballList[i].gameObject);
         }
         pri_ballList.Clear();
      }

      // Start is called before the first frame update
      void Start()
      {
         
      }

      // Update is called once per frame
      void Update()
      {

      }
   }
}

