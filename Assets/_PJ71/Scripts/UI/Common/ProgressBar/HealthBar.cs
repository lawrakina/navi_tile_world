using Core.Damagables;
using UnityEngine;

namespace NavySpade._PJ71.UI.Common.ProgressBar
{
    [RequireComponent(typeof(ProgressBarBase))]
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private DamageableMono _damageable;
        
        private ProgressBarBase _progressBar;

        private void Awake()
        {
            _progressBar = GetComponent<ProgressBarBase>();
        }

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            if(_damageable == null)
                Debug.Log("Damageable null", this);
            
            _damageable.OnHPChange += UpdateProgressBar;
            _progressBar.SetupProgressbar(_damageable.MaxHp, _damageable.HP);
        }

        private void OnDestroy()
        {
            _damageable.OnHPChange -= UpdateProgressBar;
        }
        
        private void UpdateProgressBar(float obj)
        {
            _progressBar.SetupProgressbar(_damageable.MaxHp, _damageable.HP);
        }
    }
}