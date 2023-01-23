using System;
using UnityEngine;

namespace NavySpade._PJ71.Scripts.Utils
{
    public static class GridUtils
    {
        public static Vector2Int IndexToCoord(int index, int width, int height)
        {
            int size = width * height;
            int x = index % size % width;
            int y = index % size / width;
            return new Vector2Int(x, y);
        }
        
        public static int CoordToIndex(int x, int y, int width)
        {
            return y * width + x;
        }
        
        public static int CoordToIndex(Vector2Int coord, int width)
        {
            return coord.y * width + coord.x;
        }

        public static void For2D(int width, int height, Action<int, int> func)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    func.Invoke(width, height);
                }
            }
        }
        
        public static Vector3 GetOriginOffset(AlignmentType alignment, Vector2Int size, Vector3 gaps)
        {
            switch (alignment)
            {
                case AlignmentType.LeftBottomCorner:
                    return new Vector3(gaps.x / 2, 0, gaps.y / 2) * -1;
                case AlignmentType.CenterBottomCorner:
                    return new Vector3(
                        (size.x - 1) * (gaps.x) / 2,
                        0,
                        -gaps.y / 2);
                case AlignmentType.Center:
                    return new Vector3(
                        (size.x - 1) * (gaps.x) / 2,
                        0,
                        (size.y - 1) * (gaps.y) / 2);
            }
            
            return Vector3.zero;
        }
    }
}