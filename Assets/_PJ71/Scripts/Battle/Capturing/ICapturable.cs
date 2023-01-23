using System;
using Core.Damagables;
using NavySpade._PJ71.Scripts.Actors.Runtime;
using UnityEngine;

namespace NavySpade._PJ71.Battle
{
    public interface ICapturable
    {
        Team CurrentTeam { get; }
        
        Team CapturedTeam { get; }
        
        float CaptureProgress { get; }
        
        event Action<Team> IsCaptured;

        event Action<float> CaptureProgressChanged;
        
        int AddToCapture(ICapturer capturer);

        void RemoveFromCapture(ICapturer capturer);

        Vector3 GetPositionForCapture(int index);

    }
}