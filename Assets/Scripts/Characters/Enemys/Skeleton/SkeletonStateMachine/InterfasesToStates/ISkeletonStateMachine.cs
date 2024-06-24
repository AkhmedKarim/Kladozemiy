using System;
using UnityEngine;
using System.Collections.Generic;


namespace SkeletonStateMachine
{
    public interface ISkeletonStateMachine
    {
        public void Die();
        void Dead();
        void Stop();
        bool IsInvulnerable { get; }
        bool IsDead();

        State CurrentState { set; }
        Dictionary<Type, State> StateDict { get; }

        bool IsPlayerInFrontInRange(float maxX, float maxY);
        public Transform CurrentTarget { get; }
        void ChangeCurrentTarget();
        int Direction { get; }
        bool IsPatrolable { get; }
        bool IsBetweenTargets();
        void TeleportateOnTargetsContainerPosition();

        bool IsRun { get; }
        void Run();
        Vector2 Position { get; }
        bool IsWallAhead();
        bool IsPlayerBetweenTargets();
        void Jump();
        void Walk();
        void LookAt(Transform transform);

        AttackManager AttackManager { get; }
    }
}

