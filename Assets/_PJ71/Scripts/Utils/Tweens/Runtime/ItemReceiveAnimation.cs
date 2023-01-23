using NavySpade._PJ71.InventorySystem.Items;

namespace pj40.Core.Tweens.Runtime
{
    public class ItemReceiveAnimation : ReceivingAnimation<ParabolaMovement, ItemObject>
    {
        protected override ParabolaMovement InitializeMovementTypeBase()
        {
            ParabolaMovement bezierMovement = new ParabolaMovement();
            return bezierMovement;
        }
    }
}