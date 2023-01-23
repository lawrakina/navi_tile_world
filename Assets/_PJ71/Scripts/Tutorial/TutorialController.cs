using System;
using System.Collections;
using NaughtyAttributes;
using NavySpade.Modules.Saving.Runtime;
using NavySpade.Modules.Saving.Runtime.Interfaces;
using NavySpade.Modules.Utils.Singletons.Runtime.Unity;
using UnityEngine;
using UnityEngine.Events;


namespace NavySpade.pj77.Tutorial
{
    public class TutorialController : MonoSingleton<TutorialController>, ISaveable
    {
        [Serializable]
        public struct TutorStep
        {
            public TutorAction TutorAction;
            public UnityEvent OnStartAction;
            public UnityEvent OnEndAction;
        }

        [SerializeField] private int levelIndex;
        [SerializeField] private TutorStep[] _tutorSteps;

        private TutorSaveData _tutorData;
        
        public bool TutorDone
        {
            get => _tutorData.TutorDone;
            set => _tutorData.TutorDone = value;
        }

        public int CurrentStepIndex
        {
            get => _tutorData.StepIndex;
            set => _tutorData.StepIndex = value;
        }

        public TutorAction CurrentTutorAction => _tutorSteps[_tutorData.StepIndex].TutorAction;

        public static void InvokeAction(TutorAction action)
        {
            if (InstanceExists == false)
                return;

            if (Instance.TutorDone)
                return;

            Instance.CheckProgress(action);
        }
        
        public static void ForcedAction(TutorAction action)
        {
            foreach (var step in Instance._tutorSteps)
            {
                if (step.TutorAction == action)
                {
                    step.OnStartAction?.Invoke();
                    Instance.TutorDone = true;
                }
            }
        }

        public void Init()
        {
            SaveManager.Register(this);
            RestoreState(null);
            
            if (TutorDone == false)
            {
                TutorStep currentStep = _tutorSteps[CurrentStepIndex];
                currentStep.OnStartAction?.Invoke();
            }
        }

        private void CheckProgress(TutorAction action)
        {
            TutorStep currentStep = _tutorSteps[CurrentStepIndex];
            if (currentStep.TutorAction == action)
            {
                UpdateTutorProgress();
            }
        }

        private void UpdateTutorProgress()
        {
            TutorStep currentStep = _tutorSteps[CurrentStepIndex];
            currentStep.OnEndAction?.Invoke();

            CurrentStepIndex++;
            if (CurrentStepIndex >= _tutorSteps.Length)
            {
                TutorDone = true;
                return;
            }

            TutorStep nextStep = _tutorSteps[CurrentStepIndex];
            nextStep.OnStartAction?.Invoke();
        }
        
        public void ShowTutorClick()
        {
            TutorClickHandler.Show();
        }

        public void HideTutorClick()
        {
            TutorClickHandler.Hide();
        }
        
        public void ShowText(string text)
        {
            TutorialUIController.Instance.ShowText(text);
        }

        public void HideText()
        {
            TutorialUIController.Instance.HideText();
        }
        
        public void ShowTutorObjects()
        {
            TutorialObjects.Show();
        }

        public void HideTutorObjects()
        {
            TutorialObjects.Hide();
        }

        public void FinishStepAfter(float time)
        {
            StartCoroutine(WaitForFinishProgress(time));
        }

        private IEnumerator WaitForFinishProgress(float time)
        {
            yield return new WaitForSeconds(time);
            CheckProgress(_tutorSteps[CurrentStepIndex].TutorAction);
        }

        public object CaptureState()
        {
            SaveManager.Save("Tutor" + levelIndex, _tutorData);
            return null;
        }

        public void RestoreState(object state)
        {
            _tutorData = SaveManager.Load("Tutor" + levelIndex, new TutorSaveData());
        }

        public void ClearSave() {}
        
        [Serializable]
        private struct TutorSaveData
        {
            public bool TutorDone;
            public int StepIndex;
        }
    }
}