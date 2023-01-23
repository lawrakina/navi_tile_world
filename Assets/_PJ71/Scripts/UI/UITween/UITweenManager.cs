using System;
using System.Linq;
using NS.Core.Utils.Pool.Example;
using Pool;
using UnityEngine;

namespace NavySpade._PJ71.UI.UITween
{
    public enum TweenTargetType
    {
        Wood,
        Gold
    }
    
    public class UITweenManager : MonoBehaviour
    {
        [SerializeField] private UITweenPool _tweenPool;
        [SerializeField] private Transform _inGameContainer;
        [SerializeField] private UiTweenTarget[] _uiTweenTargets;
        [SerializeField] private float _speed = 1;
        
        public void StartTween(Vector3 worldFrom, TweenTargetType targetType, Action endCallback, TweenExtraData extraData)
        {
            var screenPoint = Camera.main.WorldToScreenPoint(worldFrom);
            var tweener = _tweenPool.GetObject();
            tweener.transform.SetParent(_inGameContainer);
            tweener.transform.position = screenPoint;

            var target = _uiTweenTargets.FirstOrDefault((t) => t.TargetType == targetType);
            extraData.Speed = _speed;
            tweener.StartTween(target.Pos, extraData, () =>
            {
                endCallback?.Invoke();
                _tweenPool.Return(tweener);
            });
        }
    }
}