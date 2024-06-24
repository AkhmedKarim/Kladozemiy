using System;
using UnityEngine;

namespace PlantStateMachine
{
    public class Dead : State
    {
        public Dead(IPlantStateMachine plant) : base(plant)
        {
        }

        public override void Update()
        {
        }
        public override void Start()
        {
            plant.Stop();
            plant.Dead();

        }
        public override void Quit()
        {
            Debug.Log("Dead State Was Quit?");
        }
    }
}

