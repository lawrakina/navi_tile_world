using System;
using System.Collections;
using Cinemachine;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Events;

namespace NavySpade._PJ71
{
    public class OpeningCutScene : MonoBehaviour
    {
        [Serializable]
        public struct Step
        {
            public UnityEvent OnStartStep;
            public UnityEvent OnEndStep;
        }

        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private GameEvent _endEvent;
        [SerializeField] private Step[] _steps;
        
        private int _currentStep;
        
        public void PlayCutScene()
        {
            _currentStep = 0;
            PlayStep(_currentStep);
        }
        
        public void StepDuration(float time)
        {
            if(gameObject.activeSelf)
                StartCoroutine(WaitForFinishProgress(time));
        }

        private IEnumerator WaitForFinishProgress(float time)
        {
            yield return new WaitForSeconds(time);
            _steps[_currentStep].OnEndStep?.Invoke();
            _currentStep++;
            PlayStep(_currentStep);
        }

        private void PlayStep(int stepIndex)
        {
            if (stepIndex >= _steps.Length)
            {
                _endEvent.Raise();
                return;
            }
            
            _steps[stepIndex].OnStartStep?.Invoke();
        }

        public void SetTCameraFollow(Transform follow)
        {
            _virtualCamera.Follow = follow;
        }
    }
}