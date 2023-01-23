using System;
using System.Collections;
using System.Collections.Generic;
using NavySpade.PJ70.Core.Ammunition;
using UnityEngine;

public class BulletPool : PoolInGame<AmmoHandler>
{
    private void Awake()
    {
        Initialize();
    }
}
