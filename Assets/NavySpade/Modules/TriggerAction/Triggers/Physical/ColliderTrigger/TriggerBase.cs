using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.SR;
using Utils.TriggerAction.Triggers.Physical.Conditions;
using Object = System.Object;

namespace Utils.TriggerAction.Triggers.Physical.ColliderTrigger
{
    [RequireComponent(typeof(Collider))]
    public abstract class TriggerBase<T> : MonoBehaviour where T : Component

    {
    [SR] [SerializeReference] private PhysicalCondition[] _conditions;

    public List<T> VisibleObjects { get; private set; } = new List<T>();

    public event Action ColliderEntered;

    private void OnTriggerEnter(Collider other)
    {
        Process(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (TryGetComponent(other, out T component))
        {
            VisibleObjects.Remove(component);
        }
    }

    private void Process(Collider other)
    {
        if (_conditions.Any(condition => condition.IsMet(other) == false))
            return;

        if (TryGetComponent(other, out T component) == false)
            return;

        if (VisibleObjects.Contains(component))
            return;

        VisibleObjects.Add(component);
        ColliderEntered?.Invoke();
    }

    protected abstract bool TryGetComponent(Collider other, out T component);
    }
}