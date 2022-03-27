namespace UnityTestLineGame
{
   using System.Collections;
   using System.Collections.Generic;
   using UnityEngine;

   public static class Line98StaticData
   {
      public static PlayerController PLAYER_CONTROLLER = new PlayerController();
      public static Line98View LINE_98;

      // Use to set obj a as child of b with resetted position, rotation, and scale
      public static void ResetInstantiatedObj(Transform a, Transform b)
      {
         a.SetParent(b);
         a.localPosition = new Vector3(0, 0, 0);
         a.localRotation = Quaternion.identity;
         a.localScale = new Vector3(1, 1, 1);
      }

      public enum BallColor
      {
         ColorType0,
         ColorType1,
         ColorType2,
         ColorType3,
         ColorType4,
         ColorType5,
         ColorType6
      }

      public enum BallType
      {
         Normal,
         Ghost,
         Freeze,
         Flexible
      }
   }
}

