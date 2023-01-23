using System;
using UnityEngine;

namespace NavySpade._PJ71.Positions
{
    public interface IArea
    {
        public Vector2 RectSize { get; }
        
        public Vector3 Center { get; }

        public event Action SizeChanged;
    }
}