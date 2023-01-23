using UnityEngine;

namespace pj33
{
    public abstract class SquadPositions : MonoBehaviour
    {
        public abstract Vector3[] LocalPositions { get; }


        // public static Vector3 GetWorldPosition(SquadJoiner joiner)
        // {
        //     var center = joiner.AttachedSquad.transform.position;
        //
        //     return joiner.AttachedSquad.SquadPositions.LocalPositions[joiner.IndexInSquad] + center;
        // }
        //
        // public static Vector3 GetDestanationPoint(SquadJoiner joiner)
        // {
        //     return GetWorldPosition(joiner);
        // }

        private void OnDrawGizmosSelected()
        {
            if (LocalPositions == null)
                return;

            foreach (var position in LocalPositions)
            {
                Gizmos.DrawSphere(transform.position + position, .3f);
            }
        }
    }
}