using System;
using UnityEngine;
using System.Collections;

namespace SkeletonStateMachine
{
    public class Death : State
    {
        public Death(ISkeletonStateMachine skeletonStateMachine) : base(skeletonStateMachine)
        {
        }

        public override void StateUpdate()
        {
            skeleton.Dead();
        }

        public override void Setup()
        {
            skeleton.Die();
        }
    }
}
