using System;
using NavySpade._PJ71.UI;
using NavySpade._PJ71.UI.UITween;

namespace NS.Core.Utils.Pool.Example
{
    public class UITweenPool : PoolInGame<TweenContainer>
    {
        private void Awake()
        {
            Initialize();
        }
    }
}