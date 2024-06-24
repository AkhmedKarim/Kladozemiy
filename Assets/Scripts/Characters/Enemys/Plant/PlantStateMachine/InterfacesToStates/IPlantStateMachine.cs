using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlantStateMachine
{
    public interface IPlantStateMachine
    {
        void Move();
        Transform CurrentTarget { get; }
        void SwitchTarget();
        bool IsPlayerBetweenTargets();
        bool BetweenTargets();
        void Stop();
        void LookAt(Transform target);
        Trigger HurtTrig { get; }
        bool IsPatrolable { get; }
        bool IsPlayerInFrontInRange(float maxX, float maxY);
        void GetingHurt();
        void Dead();
        int Health { get; }
        AttackManager AttackManager { get; }
        AttackGetter CurrentAttackGetter { get; }
        void TeleportateOnTargetsContainerPosition();
        Animator GetAnimator { get; }
        float DistanseToPlayer();


        Dictionary<Type, State> StateDict { get; }
        State CurrentState { set; }


        //щоб розуміти як далеко я від гравця і щоб можна було перейти в стан атаки
        //Vector2 PlayerPosition { get; }
        Vector2 Position  { get; }
    }
}

