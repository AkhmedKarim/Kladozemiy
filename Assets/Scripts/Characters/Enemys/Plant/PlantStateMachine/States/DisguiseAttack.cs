using System;
using UnityEngine;
namespace PlantStateMachine
{
    public class DisguiseAttack : State
    {
        public DisguiseAttack(IPlantStateMachine plant) : base(plant)
        {
        }

        public override void Update()
        {
            Debug.LogError("ПОСАСИ, стан 'DisguiseAttack' не готовий!");
        }
        public override void Start()
        {
            plant.GetAnimator.SetTrigger("StartAttacking");
        }
    }
}

