using System;

namespace PlantStateMachine
{
    public class PoisonAttack : State
    {
        public PoisonAttack(IPlantStateMachine plant) : base(plant)
        {
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
