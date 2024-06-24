using System;
using System.Linq;
using UnityEngine;

namespace PlantStateMachine
{
    public class PatrolBehaviour : State
    {
        public PatrolBehaviour(IPlantStateMachine plant) : base(plant)
        {
        }
        bool randomlyWeakeUp = false;

        public override void Start()
        {
            randomlyWeakeUp = false;

            if (!plant.BetweenTargets())
            {
                plant.Stop();
                plant.TeleportateOnTargetsContainerPosition();
                //тут можна зробити івент, і коли
                //телепортація відбудеться, тільки тоді переключеться стан???
            }

            PeriodickRandom.Reset();
        }

        public override void Update()
        {
            plant.Move();
            plant.LookAt(plant.CurrentTarget);

            float distanceToTarget_x = Mathf.Abs(plant.Position.x - plant.CurrentTarget.position.x);
            if (distanceToTarget_x < 0.4f)
            {
                plant.SwitchTarget();
            }

            //transitions:
            if (plant.HurtTrig)
            {
                SetState(typeof(Hurt));
                return;
            }
            if (plant.IsPlayerBetweenTargets())
            {
                SetState(typeof(MoveToPlayer));
                return;
            }
            if (!plant.IsPatrolable)
            {
                SetState(typeof(MoveToPlayer));
                return;
            }
            PeriodickRandom.SetRandomValueEvery_3_seconds(ref randomlyWeakeUp);
            if (randomlyWeakeUp)
            {
                SetState(typeof(Disguise));
                return;
            }
        }
    }
}

