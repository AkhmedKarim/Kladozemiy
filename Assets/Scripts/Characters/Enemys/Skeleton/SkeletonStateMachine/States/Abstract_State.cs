using System;

namespace SkeletonStateMachine
{
    public abstract class State
    {
        protected ISkeletonStateMachine skeleton;

        public State(ISkeletonStateMachine skeletonStateMachine)
        {
            this.skeleton = skeletonStateMachine;
        }

        public virtual void Setup() { }

        public abstract void StateUpdate();

        public void SetNextState(State nextState)
        {
            nextState.Setup();
            skeleton.CurrentState = nextState;
        }

    }
}
