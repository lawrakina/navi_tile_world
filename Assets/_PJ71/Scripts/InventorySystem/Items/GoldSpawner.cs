using NavySpade._PJ71.UI.UITween;
using NavySpade.Meta.Runtime.Economic.Currencies;
using NS.Core.Utils.Pool.Example;
using UnityEngine;

namespace NavySpade._PJ71.InventorySystem.Items
{
    public class GoldSpawner : MonoBehaviour
    {
        [SerializeField] private Currency _currency;
        [SerializeField] private Sprite _icon;
        [SerializeField] private GoldPool _pool;
        
        public void DropGold(Vector3 position)
        {
            var gold = _pool.GetObject();
            gold.transform.parent = null;
            gold.transform.SetPositionAndRotation(position, Quaternion.identity);
        }

        public void ReturnToPool()
        {
            _pool.ReturnAll();
        }
        
        public void Receive(Gold gold)
        {
            _currency.Count += gold.Amount;
            Gui.Instance.UITweener.StartTween(gold.transform.position, TweenTargetType.Gold, () =>
            {
                _currency.Count += gold.Amount;
            }, new TweenExtraData() {Icon = _icon});
            
            _pool.Return(gold);
        }
    }
}