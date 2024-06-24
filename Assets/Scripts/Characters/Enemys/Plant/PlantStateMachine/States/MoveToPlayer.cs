using UnityEngine;
namespace PlantStateMachine
{
	public class MoveToPlayer : State
	{
        Transform player = PlayerController.instance.transform;

		public MoveToPlayer(IPlantStateMachine plant):base(plant)
		{
		}

        public override void Start()
        {
            plant.Move();
        }

        public override void Update()
        {
            plant.LookAt(player);


            //transitions:
            if (plant.HurtTrig)
            {
                SetState(typeof(Hurt));
                return;
            }
            if (!plant.IsPlayerBetweenTargets() && plant.IsPatrolable)
            {
                SetState(typeof(PatrolBehaviour));
                return;
            }
            if (plant.IsPlayerInFrontInRange(0.5f, 0.5f))// test value (0.5f, 0.5f)
            {
                SetState(typeof(TentaclesAttack));
                return;
            }
            if (plant.DistanseToPlayer() > 14 && !plant.IsPatrolable)//14 - підібрав в редакторі
            {
                SetState(typeof(Disguise));
            }    
            //-=-=-=-=-=-=
        }
    }
}

