using UnityEngine;
using System.Collections;

namespace SkeletonStateMachine
{
    public class AttackState : State
    {
        readonly Vector2 minDistanceToAttack = new Vector2(1.25f, 0.45f);

        public AttackState(ISkeletonStateMachine skeletonStateMachine) : base(skeletonStateMachine)
        {
        }

        public override void StateUpdate()
        {
            skeleton.Stop();

            //transitions:
            if (skeleton.IsDead())
            {
                SetNextState(skeleton.StateDict[typeof(Death)]);
                return;
            }
            if (!skeleton.AttackManager.IsSomeAttackPlaying &&
                !skeleton.IsPlayerInFrontInRange(minDistanceToAttack.x, minDistanceToAttack.y))
            {
                SetNextState(skeleton.StateDict[typeof(MoveToPlayer)]);
                return;
            }
            //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-


            if (!skeleton.AttackManager.IsSomeAttackPlaying && !skeleton.IsInvulnerable)
            {
                skeleton.AttackManager.Attacking(new SkeletonsDefaultAttacksGetter());
            }

        }

        public override void Setup()
        {

        }

    }
}
