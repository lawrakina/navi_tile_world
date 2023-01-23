using Core.Actors;
using Core.Damagables;

namespace NavySpade._PJ71.Battle
{
    public interface IShootable
    {
        ShootingActorPreset Data { get; }

        DamageableMono Damageable { get; }
    }
}