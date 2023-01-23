using UnityEngine;

namespace NavySpade._PJ71.UI.UITween
{
    public class UiTweenTarget : MonoBehaviour
    {
        [field: SerializeField] public TweenTargetType TargetType { get; private set; }

        public Vector3 Pos => transform.position;
    }
}