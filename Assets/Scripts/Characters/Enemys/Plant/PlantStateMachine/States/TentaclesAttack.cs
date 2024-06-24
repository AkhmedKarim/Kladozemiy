using UnityEngine;

namespace PlantStateMachine
{
    public class TentaclesAttack : State
    {
        Vector2 affectedArea = new Vector2(1,1);//(1, 1) - зона у раження
        Transform player = PlayerController.instance.transform;

        public TentaclesAttack(IPlantStateMachine plant) : base(plant)
        {

        }
        public override void Start()
        {
            plant.Stop();
            //plant.StartTentaclesAttacking();
        }
        public override void Update()
        {
            plant.LookAt(player);

            if (plant.HurtTrig)
            {
                Debug.Log("Plant Become to Angry!!!");
            }
            

            //transition:
            if (!plant.IsPlayerInFrontInRange(affectedArea.x, affectedArea.y) &&
                    !plant.AttackManager.IsSomeAttackPlaying)
            {
                SetState(typeof(MoveToPlayer));
                return;
            }
            //-=-=-=-=-=-=-=-

            if (!plant.AttackManager.IsSomeAttackPlaying)
            {
                plant.AttackManager.Attacking(plant.CurrentAttackGetter);
            }
        }
        public override void Quit()
        {
            //plant.StopTentaclesAttacking();
        }

    }
}
