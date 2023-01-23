using System;
using System.Collections;
using Core.Actors;
using Core.Damagables;
using DG.Tweening;
using Misc.Damagables.Effects;
using Misc.Physic;
using NavySpade._PJ71.Utils.AnimatorUtils;
using UnityEngine;
using UnityEngine.Events;

namespace Pj_61_Weapon_adv.Core.UnitStates
{
    public class ActorDeathAnimation : MonoBehaviour
    {
        [SerializeField] private Ragdoll _ragdoll;
        [SerializeField] private UnitAnimatorController _unitAnimator;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private UnityEvent _onEndAnimation;
        [SerializeField] private Renderer[] _rendererToChangeMaterial;
        [SerializeField] private RagdollDirectionDamagableEffect _effect;

        private ActorConfig _config;
        private bool _inDyingProcess;

        private Action _endCallback;

        private void Awake()
        {
            _config = ActorConfig.Instance;
        }

        public void PlayDeathAnimation()
        {
            if(_inDyingProcess)
                return;

            if (_unitAnimator)
                _unitAnimator.PlayAnimation(UnitAnimatorController.AnimType.Idle); 
     
            Array.ForEach(_rendererToChangeMaterial, (r) => r.material = _config.DeathMaterial);

            if (_ragdoll)
            {
                _ragdoll.IsRagdollActive = true;
                _ragdoll.AddForce(_effect.HitPosition, _effect.RagdollPower, ForceMode.Impulse);
            }

            _onDie.Invoke();
            
            StartCoroutine(Timer());
        }
        
        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(_config.DyingTime);

            if (_config.CheckMinVelocityForDeath)
            {
                while (_ragdoll.CenterBody.velocity.magnitude > _config.MINVelocityForDeath)
                {
                    yield return null;
                }
            }
            
            if(_ragdoll)
                _ragdoll.IsRagdollActive = false;
            
            transform.DOMoveY(_config.DisposeYPoint, _config.DisposeTime).OnComplete(() =>
            {
                _endCallback?.Invoke();
                _onEndAnimation?.Invoke();
            });
        }
    }
}