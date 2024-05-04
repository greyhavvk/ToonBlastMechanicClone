using System;
using UnityEngine;

namespace Core.BlockSystem.Block
{
    public class ObstacleBlock : BlockEntity
    {
        [SerializeField] private int hitPoint;

        public void ReduceHitPoint(int damage)
        {
            hitPoint = Mathf.Clamp(hitPoint - damage, 0, int.MaxValue);
        }

        public bool IsBroken => hitPoint == 0;
    }
}