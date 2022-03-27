namespace UnityTestLineGame
{
   using System.Collections;
   using System.Collections.Generic;
   using UnityEngine;

   public class Line98View : MonoBehaviour
   {
      public Camera pub_cam;

      // Ball Material
      public Material pub_ballMaterial;

      // Board Prefab use to Intantiate BoardView
      public BoardView pub_boardViewPrefab;

      public int pub_numberOfBallAtStart;

      public LineGameUI pub_lineGameUI;

      public List<Color> pub_colorType = new List<Color>();
      public List<Texture> pub_specialType = new List<Texture>();

      public float pub_spawnChance;

      // Width and Height of the board
      public int pub_boardSize;

      private void Awake()
      {
         Line98StaticData.LINE_98 = this;
      }

      // Start is called before the first frame update
      void Start()
      {
         Line98StaticData.PLAYER_CONTROLLER.Cam = pub_cam;

         GameStateStaticData.GAME_STATE_MACHINE.SetState(GameStateStaticData.GAME_STATE_INIT_GAME);
      }

      // Update is called once per frame
      void Update()
      {
         GameStateStaticData.GAME_STATE_MACHINE.OnExecuteState(Time.deltaTime);
      }
   }
}

