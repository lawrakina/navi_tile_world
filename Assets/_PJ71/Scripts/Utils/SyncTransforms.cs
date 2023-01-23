using System;
using UnityEngine;

namespace NavySpade._PJ71.Utils
{
    public class SyncTransforms : MonoBehaviour
    {
        public Transform SyncWith;

        private void Update()
        {
            if(SyncWith == null)
                return;
            
            transform.position = SyncWith.position;
        }
    }
}