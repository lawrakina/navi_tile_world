using System.Collections;
using NavySpade._PJ71.Extensions;
using UnityEngine;


namespace NavySpade._PJ71.InventorySystem {
    public abstract class ItemReceiver : MonoBehaviour {
        private Coroutine _pickupItemsRoutine;
        private WaitForSeconds _delayToPickupItem;

        private void Awake() {
            _delayToPickupItem = new WaitForSeconds(ItemManagementConfig.Instance.DelayBetweenResourceRequest);
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag(StringManager.UNIT_PLAYER) &&
                !other.CompareTag(StringManager.UNIT_PLAYER_SQUAD)) return;
            if (other.TryGetComponent<IInventoryHandler>(out var inventory))
                OnEnter(inventory);
        }

        private void OnTriggerExit(Collider other) {
            if (!other.CompareTag(StringManager.UNIT_PLAYER) &&
                !other.CompareTag(StringManager.UNIT_PLAYER_SQUAD)) return;
            if (other.TryGetComponent(out IInventoryHandler inventory))
                OnExit(inventory);

            if (_pickupItemsRoutine == null) return;
            StopCoroutine(_pickupItemsRoutine);
            _pickupItemsRoutine = null;
        }

        protected void StartPickupItems(IInventoryHandler inventory) {
            _pickupItemsRoutine ??= StartCoroutine(PickupItemsForWorkRoutine(inventory));
        }

        private IEnumerator PickupItemsForWorkRoutine(IInventoryHandler playerInventory) {
            while (true)
            {
                yield return _delayToPickupItem;
                PickupItemsFrom(playerInventory);
            }
        }

        protected abstract void OnEnter(IInventoryHandler inventory);

        protected abstract void OnExit(IInventoryHandler inventory);

        protected abstract void PickupItemsFrom(IInventoryHandler playerInventory);
    }
}