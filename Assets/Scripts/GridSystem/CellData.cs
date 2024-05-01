using UnityEngine;

namespace GridSystem
{
   public class CellData
   {
      public Vector2 Position;
      public bool IsEmpty= true;
      public GameObject HoldingObject;

      public CellData(Vector2 position)
      {
         Position = position;
      }
   }
}
