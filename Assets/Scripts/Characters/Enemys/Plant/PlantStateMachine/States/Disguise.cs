using UnityEngine;
using System;
using Unity.VisualScripting;

namespace PlantStateMachine
{
	public class Disguise : State
	{
        const bool PATROLABLE = true, NOT_PATROLABLE = false;
        public Disguise(IPlantStateMachine plant) : base(plant)
        {
        }

        bool randomlyWeakeUp = false;
        Type nextState;
        bool IsNextStateHasBeenSet = false;

        public override void Start()
        {
            PeriodickRandom.Reset();
            randomlyWeakeUp = false;
            IsNextStateHasBeenSet = false;

            plant.Stop();
            plant.GetAnimator.SetTrigger("Disguise_In");
        }

        public override void Update()
        {
            plant.Stop();

            if (IsNextStateHasBeenSet)
            {
                while (plant.GetAnimator.GetCurrentAnimatorStateInfo(0).IsName("Plant_Disguise_Out"))
                    return;
                SetState(nextState);
                return;
            }

            //transitions:
            switch (plant.IsPatrolable)
            {
                case NOT_PATROLABLE:
                    if (plant.IsPlayerInFrontInRange(1.15f, 0.39f))//1.15f, 0.39f - я їх підібрав в редакторі юніті
                    {
                        SetState(typeof(DisguiseAttack));
                        return;
                    }
                    return;

                case PATROLABLE:
                    if (plant.IsPlayerInFrontInRange(2f, 2f))//(2f, 2f) - треба підбрати значення
                    {
                        _Quite(nextState: typeof(PoisonAttack));
                        return;
                    }
                    PeriodickRandom.SetRandomValueEvery_3_seconds(ref randomlyWeakeUp);
                    if (randomlyWeakeUp)
                    {
                        _Quite(nextState: typeof(PatrolBehaviour));
                        return;
                    }
                    return;
            }
            //-=-=-=-=-=-=
        }

        private void _Quite(Type nextState)
        {
            this.nextState = nextState;
            plant.GetAnimator.SetTrigger("Disguise_Out");
            IsNextStateHasBeenSet = true;
        }

        public override void Quit()
        {
            //мій метод Quit() тупо нюхає
        }

    }
}

