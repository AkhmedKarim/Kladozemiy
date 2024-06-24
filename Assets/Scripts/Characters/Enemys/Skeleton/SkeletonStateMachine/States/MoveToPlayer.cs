using System;
using UnityEngine;

namespace SkeletonStateMachine
{
    public class MoveToPlayer : State
    {
        Transform player = PlayerController.instance.transform;

        public MoveToPlayer(ISkeletonStateMachine skeletonStateMachine) : base(skeletonStateMachine)
        {
        }

        public override void StateUpdate()
        {
            float distanse_x_ToPlayer = Mathf.Abs(skeleton.Position.x - player.position.x);


            skeleton.LookAt(player);
            if (distanse_x_ToPlayer > 4 || skeleton.IsRun)//4 - test value
            {
                skeleton.Run();
            }
            else if (distanse_x_ToPlayer > 0.3f)//1 - test value
            {
                skeleton.Walk();
            }

            if (skeleton.IsWallAhead())
            {
                skeleton.Jump();
            }


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
            if (skeleton.IsPatrolable && !skeleton.IsPlayerBetweenTargets())
            {
                SetNextState(skeleton.StateDict[typeof(Patrol)]);
                return;
            }
            //-=-=-=-=-=-=-=-=-=-=-=
        }


    }
}

