using System;
using UnityEngine;

namespace NavySpade._PJ71.InventorySystem
{
    public interface IRequirementsHolder
    {
        event Action ProgressUpdated;

        ResourceRequirements Requirement { get; }
        
        int LeftResources { get; }
    }
}