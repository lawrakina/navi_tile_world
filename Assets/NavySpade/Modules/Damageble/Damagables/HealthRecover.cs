using NavySpade.NavySpade.Modules.Utils.Timers;
using UnityEngine;

namespace Core.Damagables
{
    public class HealthRecover : MonoBehaviour
    {
        [SerializeField] private DamageableLight _damageable;
        [SerializeField] private float _recoverTime;
        [SerializeField] private float _recoverHp;
        
        private Timer _recoverTimer;
        
        private void OnEnable()
        {
            _damageable.OnHPChange += ResetRecover;
        }
        
        private void OnDisable()
        {
            _damageable.OnHPChange -= ResetRecover;
        }

        private void Start()
        {
            _recoverTimer = new Timer(_recoverTime);
        }

        private void Update()
        {
            if(_damageable.HP >= _damageable.MaxHp)
                return;
            
            _recoverTimer.Update(Time.deltaTime);
            if (_recoverTimer.IsFinish())
            {
                _damageable.Heal(_recoverHp);
                _recoverTimer.Reload();
            }
        }
        
        private void ResetRecover(float obj)
        {
            _recoverTimer.Reload();
        }
    }
}