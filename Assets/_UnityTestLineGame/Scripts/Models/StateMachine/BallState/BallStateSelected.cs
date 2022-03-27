namespace UnityTestLineGame
{
   using System.Collections;
   using System.Collections.Generic;
   using UnityEngine;

   public class BallStateSelected : BallState
   {
      public override void OnEnterState()
      {
         // Play Selected Animation?
      }

      public override void OnExecuteState(float time)
      {
         pro_ball.PlayAnimation(time);
      }

      public override void OnExitState()
      {
         base.OnExitState();
      }

      public override void OnHandleInput(float time)
      {
         
      }
   }
}

