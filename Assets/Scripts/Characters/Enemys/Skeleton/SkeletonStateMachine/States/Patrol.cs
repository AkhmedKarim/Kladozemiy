using System;
using UnityEngine;

namespace SkeletonStateMachine
{
    public class Patrol : State
    {
        public Patrol(ISkeletonStateMachine skeletonStateMachine) : base(skeletonStateMachine)
        {
        }

        public override void Setup()
        {
            if (!skeleton.IsBetweenTargets())
            {
                skeleton.Stop();
                skeleton.TeleportateOnTargetsContainerPosition();
                //тут можна зробити івент, і коли
                //телепортація відбудеться, тільки тоді переключеться стан???
            }
        }

        public override void StateUpdate()
        {
            float distanseToCurrentTarget =
                Vector2.Distance(skeleton.CurrentTarget.position, skeleton.Position);

            skeleton.LookAt(skeleton.CurrentTarget);

            if (distanseToCurrentTarget < 0.4f)
            {
                skeleton.ChangeCurrentTarget();
            }
            if (skeleton.IsWallAhead())
            {
                skeleton.Jump();
            }

            skeleton.Walk();

            //transitions:
            if (skeleton.IsDead())
            {
                SetNextState(skeleton.StateDict[typeof(Death)]); 
                return;
            }
            if (skeleton.IsPlayerInFrontInRange(0.5f, 0.5f))
            {
                SetNextState(skeleton.StateDict[typeof(AttackState)]);
                return;
            }

            if (!skeleton.IsPatrolable || skeleton.IsPlayerBetweenTargets())
            {
                SetNextState(skeleton.StateDict[typeof(MoveToPlayer)]); 
                return;
            }
        }

        
    }
}
