using System;
using Core.Input.Commands.Interfaces;
using UnityEngine;

namespace Core.Input.Commands
{
    public class MoveCommand : ICommand
    {
        public Vector3 Movement;
        public Action ReachCallback;

        public MoveCommand(Vector3 movement, Action onReachCallback = null)
        {
            Movement = movement;
            ReachCallback = onReachCallback;
        }

        public void Execute()
        {

        }
    }
}