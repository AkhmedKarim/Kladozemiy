using System;
using SkeletonStateMachine;

namespace PlantStateMachine
{
    public class Hurt : State
    {
        public Hurt(IPlantStateMachine plant) : base(plant)
        {
        }

        public override void Start()
        {
            plant.Stop();
        }
        public override void Update()
        {
            plant.GetingHurt();


            //conditions
            if (plant.Health <= 0)
            {
                SetState(typeof(Dead));
                return;
            }
            if (true)
            {
                SetState(typeof(MoveToPlayer));
                return;
            }
        }
        
    }
}

