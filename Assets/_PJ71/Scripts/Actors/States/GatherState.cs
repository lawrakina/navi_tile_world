using MoreMountains.NiceVibrations;
using NavySpade._PJ71.BuildingSystem.Runtime;
using NavySpade._PJ71.Gathering;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.InventorySystem.Visualizer;
using NavySpade._PJ71.UI.UITween;
using NavySpade._PJ71.Utils;
using NavySpade._PJ71.Utils.AnimatorUtils;
using NavySpade.Modules.StateMachines.Runtime.Mono;
using NavySpade.NavySpade.Modules.Utils.Timers;
using UnityEngine;
using UnityEngine.EventSystems;


namespace NavySpade._PJ71.Actors.States
{
    public class GatherState : StateBehavior
                               // , IInventoryInitialize
    {
        [SerializeField] private UnitAnimatorController _animator;
        [SerializeField] private MinedObjectDetector _detector;
        [SerializeField] private InventoryVisualizer _inventoryVisualizer;
        [SerializeField] private float _timeForGather = 1f;

        private Timer _timer;

        private void Awake()
        {
            _timer = new Timer(_timeForGather);
        }

        public override void Enter()
        {
            base.Enter();
            _animator.SetMoveBool(false);
            _timer.Reload();
            Gather();
        }

        public override void Exit()
        {
            base.Exit();
            _animator.SetMoveBool(true);
        }
        
        private void Update()
        {
            _timer.Update(Time.deltaTime);
            if (_timer.IsFinish())
            {
                Gather();
                _timer.Reload();
            }
        }
        
        private void Gather()
        {
            AnimActionCallbackData actionCallbackData = new AnimActionCallbackData()
            {
                AnimEvent = AnimEvent.Gather,
                EventCallback = GetResourceFromVisibleMinedObjects
            };

            _animator.SetTrigger(UnitAnimatorController.AnimType.Gather, actionCallbackData);
        }
        
        private void GetResourceFromVisibleMinedObjects()
        {
            foreach (var minedObject in _detector.MinedObjects)
            {
                minedObject.ResetRecovering();
                if (minedObject.CanGather)
                {
                    var itemInfo =  minedObject.GatherResource();
                    Gui.Instance.UITweener.StartTween(minedObject.transform.position, TweenTargetType.Wood, () => {
                        if (_inventoryVisualizer.Inventory == null) return;
                        if (_inventoryVisualizer is UnitStackVisualizer stackVisualizer)
                            stackVisualizer.Owner.Inventory.AddItem(itemInfo.Item1.Type, itemInfo.Item2);
                        else
                            _inventoryVisualizer.Inventory.AddItem(itemInfo.Item1.Type, itemInfo.Item2);
                    }, new TweenExtraData(){ Icon = itemInfo.Item1.Icon});
                    
                    VibrationManager.Vibrate(HapticTypes.MediumImpact);
                }
            }
        }
    }
}