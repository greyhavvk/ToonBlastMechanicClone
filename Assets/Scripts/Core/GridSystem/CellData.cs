using UnityEngine;

namespace Core.GridSystem
{
   public class CellData
   {
      public Vector2 Position{ get; private set; }
      public bool IsEmpty { get; private set; }= true;

      public CellData(Vector2 position)
      {
         Position = position;
      }

      public void FillCell()
      {
         IsEmpty = false;
      }

      public void EmptyCell()
      {
         IsEmpty = true;
      }
   }
}
