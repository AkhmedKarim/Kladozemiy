using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkeletonStateMachine;
using System;

public class SkeletonBehaviour : MonoBehaviour,
ISkeletonStateMachine, IDamageable, ICanAttack
{
   //-=-=-=-=-=-=-=-=-=-=-=-=-StateMachine-=-=-=-=-=-=-=-=-=-=-=-=-
   [SerializeField] State currentState;
    Dictionary<Type, State> stateDict = new Dictionary<Type, State>();

    AttackManager ISkeletonStateMachine.AttackManager { get => attackManager; }
    int ISkeletonStateMachine.Direction { get => direction; }
    bool ISkeletonStateMachine.IsRun { get => isRun; }
    Vector2 ISkeletonStateMachine.Position { get => (Vector2)transform.position; }
    State ISkeletonStateMachine.CurrentState { set => currentState = value; }
    Dictionary<Type, State> ISkeletonStateMachine.StateDict { get => stateDict; }
    Transform ISkeletonStateMachine.CurrentTarget { get => patrolController.CurrentTarget; }
    bool ISkeletonStateMachine.IsInvulnerable { get => isInvulnerable; }
    bool ISkeletonStateMachine.IsDead()
    {
        return (health <= 0);
    }
    bool ISkeletonStateMachine.IsPlayerInFrontInRange(float maxX, float maxY)
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
    void ISkeletonStateMachine.Stop()
    {
        actuallySpeedX = 0;
    }
    bool ISkeletonStateMachine.IsPlayerBetweenTargets()
    {
        return patrolController.IsBetweenTargets(player.transform);
    }
    void ISkeletonStateMachine.LookAt(Transform target)
    {
        LookAt(target);
    }
    bool ISkeletonStateMachine.IsWallAhead()
    {
        return IsWallAhead();
    }
    bool ISkeletonStateMachine.IsPatrolable { get => patrolState; }
    bool ISkeletonStateMachine.IsBetweenTargets()
    {
        return patrolController.IsBetweenTargets(this.transform);
    }
    void ISkeletonStateMachine.TeleportateOnTargetsContainerPosition()
    {                       
        transform.position = patrolController.DefaultPositionBetweenTargets;
    }
    void ISkeletonStateMachine.Run()
    {
        float distanse_x_ToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);

        actuallySpeedX = runSpeed;
        isRun = (distanse_x_ToPlayer > 0.5f);
    }
    void ISkeletonStateMachine.Jump()
    {
        Jump();
    }
    void ISkeletonStateMachine.Walk()
    {
        Walk();
    }
    void ISkeletonStateMachine.ChangeCurrentTarget()
    {
        patrolController.SwitchTarget();
    }
    void ISkeletonStateMachine.Die()
    {
        StartCoroutine(Disappearance());

        IEnumerator Disappearance()
        {
            while (GetComponent<SpriteRenderer>().color.a > 0)
            {
                float a_channel = GetComponent<SpriteRenderer>().color.a;
                GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, a_channel - 0.04f);

                yield return new WaitForSeconds(0.1f);
            }
            Destroy(gameObject);
        }
    }
    void ISkeletonStateMachine.Dead()
    {
        actuallySpeedX = 0;
    }
    //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

    Animator ICanAttack.GetAnimator => animator;
    Weapon[] ICanAttack.GetAllWeapon()
    {
        return new Weapon[] { sword };
    }
    void ICanAttack.StartCoroutine(IEnumerator enumerator)
    {
        StartCoroutine(enumerator);
    }


    AttackManager attackManager;


    void IDamageable.GetHurt(int damage)
    {
        if (isInvulnerable)
            return;

        animator.SetTrigger("Hurt");
        health -= damage;
        StartCoroutine(Invulnerable());
    }


    Weapon sword;

    PlayerController player;

    [SerializeField]
    float
        defaultSpeedX = 1,
        runSpeed = 2,
        jumpForce;

    [SerializeField] int health;
    float actuallySpeedX;
    int direction = 1;
    bool isRun = false;
    int bodyDamage = 1;

    RaycastHit2D hit;
    LayerMask watchAbleLayer;

    [SerializeField] Transform[] targets = new Transform[2];

    PatrolBetweenTargetsCreator patrolController;

    //-=-=-=-=-components-=-=-=-=-=-
    Animator animator;
    Rigidbody2D rb;
    GroundCheker gndCheker;

    void Start()
    {
        stateDict.Add(typeof(MoveToPlayer), new MoveToPlayer(this));
        stateDict.Add(typeof(Death), new Death(this));
        stateDict.Add(typeof(Patrol), new Patrol(this));
        stateDict.Add(typeof(AttackState), new AttackState(this));
        currentState = stateDict[typeof(MoveToPlayer)];


        gndCheker = GetComponentInChildren<GroundCheker>();
        rb = GetComponent<Rigidbody2D>();
        player = PlayerController.instance;
        animator = GetComponent<Animator>();
        watchAbleLayer = LayerMask.GetMask("Ground");
        SetupWeapon();

        actuallySpeedX = defaultSpeedX;


        patrolController = new PatrolBetweenTwoTargets(carrier: transform, targets);
        patrolController.SetupTargets();



       
        attackManager = new AttackManager((ICanAttack)this);
        attackManager.HideAllWeapon();
    }
    void Update()
    {
        hit = UpdateRaycastHit2D();
        UpdateVelocity();
        UpdateAnimatorParametrs();
        BodyHitPlayer();


        currentState.StateUpdate();
    }

    void SetupWeapon()
    {
        sword = transform.Find("Sword").GetComponent<Weapon>();
    }
    void UpdateVelocity()
    {
        rb.velocity = new Vector2(actuallySpeedX * direction, rb.velocity.y);
    }
    void UpdateAnimatorParametrs()
    {
        animator.SetFloat("movingX", Mathf.Abs(rb.velocity.x));
        animator.SetBool("isGrounded", gndCheker.IsGrounded);
        animator.SetInteger("Health", health);
    }
    RaycastHit2D UpdateRaycastHit2D()
    {
        Ray2D ray = new Ray2D((Vector2)transform.position, Vector2.right * direction);
        float rayLength = actuallySpeedX < defaultSpeedX ? 0.4f : actuallySpeedX;
        Debug.DrawRay(ray.origin, ray.direction * rayLength);
        return Physics2D.Raycast(ray.origin, ray.direction, rayLength, watchAbleLayer);
    }
    float timerOfBodeHit = 0,
        timerMaxOfBodeHit = 1;
    void BodyHitPlayer()
    {
        float distanseToPlayer = Vector2.Distance(player.transform.position, transform.position);
        float skeletonSizeRadius = .4f;

        if (distanseToPlayer < skeletonSizeRadius)
        {
            timerOfBodeHit += Time.deltaTime;

            if (timerOfBodeHit > timerMaxOfBodeHit)
            {
                timerOfBodeHit = 0;
                player.GetHurt(bodyDamage);
            }
        }
        else
            timerOfBodeHit = timerMaxOfBodeHit;
    }

    private void OnDestroy()
    {
        if (targets[0] != null)
            foreach (var target in targets)
            {
                Destroy(target.gameObject);
            }
    }


    [SerializeField] bool patrolState;
   
    bool isInvulnerable = false;
    IEnumerator Invulnerable()
    {
        isInvulnerable = true;
        for (int i = 0; i < 2; i++)
        {
            GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 0.3f);
            yield return new WaitForSeconds(0.2f);

            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
        isInvulnerable = false;
    }

    void AnimatorEvent_Jump()
    {
        if (gndCheker.IsGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.ResetTrigger("jump");
        }
    }

    void LookAt(Transform target)
    {
        const float minDistanseToCahngeDirection = 0.1f;

        float localCurrentTargetPos_x = target.position.x - transform.position.x;

        actuallySpeedX = 0;

        if (localCurrentTargetPos_x < minDistanseToCahngeDirection)
            direction = -1;
        else if (localCurrentTargetPos_x > minDistanseToCahngeDirection)
            direction = 1;

        transform.localScale = new Vector3(direction, 1, 1);
    }
    bool IsWallAhead()
    {
        if (hit && hit.transform.CompareTag("Platform"))
        {
            return true;
        }

        return false;
    }
    
    private void Jump()
    {
        if (gndCheker.IsGrounded)
            animator.SetTrigger("jump");
    }
    void Walk()
    {
        actuallySpeedX = defaultSpeedX;
        isRun = false;

    }
    
}
