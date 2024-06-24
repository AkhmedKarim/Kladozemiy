using System;

namespace PlantStateMachine
{
	public abstract class State
	{
        protected IPlantStateMachine plant;

        public State(IPlantStateMachine plant)
        {
            this.plant = plant;
        }
        public virtual void Start() { }

        public virtual void Quit() { }

        public abstract void Update();

        //public void _Update()
        //{
        //    Update();
        //}

        public void SetState(Type state)
        {
            this.Quit();
            var nextState = plant.StateDict[state];
            nextState.Start();
            plant.CurrentState = nextState;
        }
        //public void DontUpdate(int seconds)
        //{

        //}
    }
}

