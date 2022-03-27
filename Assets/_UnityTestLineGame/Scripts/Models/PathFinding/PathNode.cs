namespace UnityTestLineGame
{
   using System.Collections;
   using System.Collections.Generic;
   using UnityEngine;

   public class PathNode
   {
      public int pub_idx;

      public int pub_gCost;
      public int pub_hCost;
      public int pub_fCost;

      public PathNode pub_cameFrom;

      public bool pub_isWalkable;

      public PathNode(int idx)
      {
         pub_idx = idx;
         pub_cameFrom = null;
         pub_isWalkable = true;
      }

      public void CalculateFCost()
      {
         pub_fCost = pub_gCost + pub_hCost;
      }
   }
}

