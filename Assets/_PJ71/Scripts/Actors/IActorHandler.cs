using Core.Actors;
using UnityEngine;

namespace NS.Core.Actors
{
    public interface IActorHandler
    {
        void EnemyDestroyed(ActorHolder holder);
    }
}