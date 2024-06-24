using System;
using System.Collections;
using System.Collections.Generic;
using PlantStateMachine;
using UnityEngine;

public class PlantBehaviour : MonoBehaviour, IPlantStateMachine, ICanAttack
{
    Animator ICanAttack.GetAnimator => GetComponent<Animator>();
    Weapon[] ICanAttack.GetAllWeapon()
    {
        return new Weapon[] { tentacle };
    }
    void ICanAttack.StartCoroutine(IEnumerator enumerator)
    {
        StartCoroutine(enumerator);
    }

    //-=-=-==-=-=-==-=-=-=-=-=StateMachine-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    [SerializeField] State currentState;
    Dictionary<Type, State> statesDict;

    Vector2 IPlantStateMachine.Position { get => transform.position; }
    Dictionary<Type, State> IPlantStateMachine.StateDict { get => statesDict; }
    State IPlantStateMachine.CurrentState { set => currentState = value; }
    Transform IPlantStateMachine.CurrentTarget { get => patrollController.CurrentTarget; }
    Trigger IPlantStateMachine.HurtTrig { get => hurtTrig; }
    Animator IPlantStateMachine.GetAnimator { get => animator; }
    void IPlantStateMachine.Stop()
    {
        this.Stop();
    }
    float IPlantStateMachine.DistanseToPlayer()
    {
        return Vector2.Distance(transform.position, player.transform.position);
    }
    void IPlantStateMachine.SwitchTarget()
    {
        patrollController.SwitchTarget();
    }
    bool IPlantStateMachine.IsPlayerBetweenTargets()
    {
        return patrollController.IsBetweenTargets(player.transform);
    }
    bool IPlantStateMachine.BetweenTargets()
    {
        return patrollController.IsBetweenTargets(this.transform);
    }
    void IPlantStateMachine.GetingHurt()
    {
        //анімація отримання шкоди, можливо треба все переробити під карутіну
    }
    int IPlantStateMachine.Health { get => health; }
    AttackManager IPlantStateMachine.AttackManager => this.attackManager;
    AttackGetter IPlantStateMachine.CurrentAttackGetter => attackGetter;
    bool IPlantStateMachine.IsPatrolable { get => isPatrolable; }
    void IPlantStateMachine.TeleportateOnTargetsContainerPosition()
    {
        transform.position = patrollController.DefaultPositionBetweenTargets;
    }
    bool IPlantStateMachine.IsPlayerInFrontInRange(float maxX, float maxY)
    {
        float localPlayerPosition_x = player.transform.position.x - transform.position.x;
        float distanseToPlayer = Mathf.Abs(player.transform.position.y - transform.position.y);

        if (distanseToPlayer < maxY)
            switch (direction)
            {
                default:
                case 1:
                    return localPlayerPosition_x < maxX && localPlayerPosition_x > 0;
                case -1:
                    return localPlayerPosition_x < 0 && localPlayerPosition_x > -maxX;

            }

        return false;
    }
    //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    Trigger hurtTrig;

    Rigidbody2D rb;
    Animator animator;

    PlayerController player;

    float actualSpeed_x;
    [SerializeField] float walkSpeed;
    int direction = 1;
    int health;

    Weapon tentacle;

    AttackManager attackManager;
    AttackGetter attackGetter;
    public void SetAttackGetter(AttackGetter attackGetter)
    {
        this.attackGetter = attackGetter;
    }

    [SerializeField] bool isPatrolable;
    [SerializeField] Transform[] targets = new Transform[2];
    PatrolBetweenTargetsCreator patrollController;

    void Start()
    {

        //воно буде встановлюватися автоматично по ходу гри
        {
            SetAttackGetter(new PlantSerialAttack_AttacksGetter());
        }

        player = PlayerController.instance;

        statesDict = new Dictionary<Type, State>()
        {
            { typeof(PatrolBehaviour), new PatrolBehaviour(this) },
            { typeof(MoveToPlayer), new MoveToPlayer(this)},
            { typeof(Dead), new Dead(this)},
            { typeof(Disguise), new Disguise(this)},
            { typeof(DisguiseAttack), new DisguiseAttack(this)},
            { typeof(Hurt), new Hurt(this)},
            { typeof(PoisonAttack), new PoisonAttack(this)},
            { typeof(TentaclesAttack), new TentaclesAttack(this)},
        };
        currentState = statesDict[typeof(PatrolBehaviour)];


        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();



        patrollController = new PatrolBetweenTwoTargets(carrier: this.transform, targets);
        patrollController.SetupTargets();
        SetupWeapon();


        currentState.Start();


        attackManager = new AttackManager(attacker: this);
        attackManager.HideAllWeapon();
    }

    void SetupWeapon()
    {
        tentacle = transform.Find("Tentacles").GetComponent<Weapon>();
    }

    void Update()
    {
        if (currentState is MoveToPlayer)
        {
            Debug.Log("MoveToPlayer");
        }
        else if (currentState is PatrolBehaviour)
        {
            Debug.Log("PatrolBehaviour");
        }
        else
        {
            Debug.Log("else state");
        }

        
        currentState.Update();

        UpdateVelocity();
        UpdateAnimatorsParameters();
    }

    void UpdateVelocity()
    {
        rb.velocity = new Vector2(actualSpeed_x * direction, rb.velocity.y);
    }
    void UpdateAnimatorsParameters()
    {
        animator.SetFloat("Moving_X", actualSpeed_x);
    }

    

    public void Move()
    {
        actualSpeed_x = walkSpeed;
    }
    private void Stop()
    {
        actualSpeed_x = 0;
    }
   
    private void Hurt(int damage)
    {
        animator.SetTrigger("Hurt");
        Stop();
        health -= damage;
    }
    public void Dead()
    {
        Stop();
        animator.SetBool("Dead", true);
    }
    public void PoisonAttack()
    {
        Stop();
        animator.SetTrigger("PoisonAttack");
        //instantiate poison cloud
    }
    public void DisguiseIn()
    {
        Stop();
        animator.SetTrigger("DisguiseIn");
    }
    public void DisguiseOut()
    {
        animator.SetTrigger("DisguiseOut");
    }
    public void LookAt(Transform target)
    {
        const float minDistanseToCahngeDirection = 0.1f;

        float localCurrentTargetPos_x = target.position.x - transform.position.x;

        if (localCurrentTargetPos_x < minDistanseToCahngeDirection)
            direction = -1;
        else if (localCurrentTargetPos_x > minDistanseToCahngeDirection)
            direction = 1;

        transform.localScale = new Vector3(direction, 1, 1);
    }


}
