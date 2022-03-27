namespace UnityTestLineGame
{
   using UnityEngine;

   public class PlayerController
   {
      public Camera Cam { get; set; }

      public bool StartDragging { get; set; }

      private float pri_tapCounter;

      public Vector3 HitPoint { get; set; }

      public GameObject HitObj { get; set; }

      public Vector3 mousePos;

      public void TapTick(float time)
      {
         pri_tapCounter += time;
      }

      public bool TapInput()
      {
         bool isTap = false;
         if (pri_tapCounter <= 0.2f)
         {
            isTap = true;
         }

         return isTap;
      }

      public GameObject PlayerInput()
      {
         if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
         {
            return PhoneInput();
         }
         else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
         {
            return PCInput();
         }
         else
         {
            return null;
         }
      }

      public GameObject PlayerInputButtonDown()
      {
         if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
         {
            return PhoneInputButtonDown();
         }
         else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
         {
            return PCInputButtonDown();
         }
         else
         {
            return null;
         }
      }

      public void PlayerInputDragging(GameObject obj)
      {
         if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
         {
            PhoneInputDragging(obj);
         }
         else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
         {
            PCInputDragging(obj);
         }
      }

      private GameObject PCInputButtonDown()
      {
         if (Input.GetMouseButtonDown(0))
         {
            pri_tapCounter = 0;
            Ray raycast = Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
               return raycastHit.collider.gameObject;
            }
         }
         return null;
      }

      private GameObject PhoneInputButtonDown()
      {
         for (int i = 0; i < Input.touchCount; i++)
         {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
               pri_tapCounter = 0;
               Ray raycast = Cam.ScreenPointToRay(Input.GetTouch(i).position);
               RaycastHit raycastHit;
               if (Physics.Raycast(raycast, out raycastHit))
               {
                  return raycastHit.collider.gameObject;
               }
            }
         }
         return null;
      }

      private void PCInputDragging(GameObject obj)
      {
         if (obj != null && Input.GetMouseButton(0))
         {
            Ray raycast = Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] raycastHits;
            raycastHits = Physics.RaycastAll(raycast);

            Vector3 hitPos = Vector3.zero;
            mousePos = Input.mousePosition;
            for (int i = 0; i < raycastHits.Length; i++)
            {
               if (raycastHits[i].collider != null)
               {
                  hitPos = raycastHits[i].point;
                  HitObj = raycastHits[i].collider.gameObject;
                  break;
               }
            }
            hitPos = new Vector3(hitPos.x, hitPos.y, hitPos.z);
            if (obj.transform.position.x >= hitPos.x - 0.2f && obj.transform.position.x <= hitPos.x + 0.2f &&
               obj.transform.position.z >= hitPos.z - 0.2f && obj.transform.position.z <= hitPos.z + 0.2f)
            {
               StartDragging = false;
            }
            else
            {
               StartDragging = true;
            }
            if (StartDragging)
            {
               obj.transform.position = new Vector3(hitPos.x, hitPos.y, hitPos.z);
            }
         }
      }

      private void PhoneInputDragging(GameObject obj)
      {
         for (int i = 0; i < Input.touchCount; i++)
         {
            if (obj != null && (Input.GetTouch(i).phase == TouchPhase.Moved || Input.GetTouch(i).phase == TouchPhase.Stationary))
            {
               Ray raycast = Cam.ScreenPointToRay(Input.GetTouch(i).position);
               RaycastHit[] raycastHits;
               raycastHits = Physics.RaycastAll(raycast);
               mousePos = Input.GetTouch(i).position;
               Vector3 hitPos = Vector3.zero;

               for (int idx = 0; idx < raycastHits.Length; idx++)
               {

                  if (raycastHits[idx].collider != null)
                  {
                     hitPos = raycastHits[idx].point;
                     HitObj = raycastHits[i].collider.gameObject;
                     break;
                  }
               }
               if (obj.transform.position.x >= hitPos.x - 0.2f && obj.transform.position.x <= hitPos.x + 0.2f &&
               obj.transform.position.z >= hitPos.z - 0.2f && obj.transform.position.z <= hitPos.z + 0.2f)
               {
                  StartDragging = false;
               }
               else
               {
                  StartDragging = true;
               }
               if (StartDragging)
               {
                  obj.transform.position = new Vector3(hitPos.x, hitPos.y, hitPos.z);
               }
            }
         }
      }

      private GameObject PCInput()
      {
         if (Input.GetMouseButtonUp(0))
         {
            Ray raycast = Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
               HitPoint = raycastHit.point;
               return raycastHit.collider.gameObject;
            }
         }
         return null;
      }

      private GameObject PhoneInput()
      {
         for (int i = 0; i < Input.touchCount; i++)
         {
            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {
               Ray raycast = Cam.ScreenPointToRay(Input.GetTouch(i).position);
               RaycastHit raycastHit;
               if (Physics.Raycast(raycast, out raycastHit))
               {
                  HitPoint = raycastHit.point;
                  return raycastHit.collider.gameObject;
               }
            }
         }
         return null;
      }
   }
}

