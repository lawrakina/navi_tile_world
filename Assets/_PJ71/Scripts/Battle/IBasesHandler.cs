namespace NavySpade._PJ71.Battle
{
    public interface IBasesHandler
    {
        public bool TryGetTargetFlag(Flag flag, out Flag destinationFlag);
    }
}