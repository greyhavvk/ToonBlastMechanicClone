using UnityEngine;

namespace GridSystem
{
   public class CellData
   {
      public Vector2 Position{ get; private set; }
      public bool IsEmpty { get; private set; }= true;
      public GameObject HoldingObject { get; private set; } = null;

      public CellData(Vector2 position)
      {
         Position = position;
      }

      public void FillCell(GameObject holdingObject)
      {
         HoldingObject = holdingObject;
         IsEmpty = false;
      }

      public void EmptyCell()
      {
         HoldingObject = null;
         IsEmpty = true;
      }
   }
}
