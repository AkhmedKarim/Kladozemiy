using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour,
    IDamageable, ICanAttack, ICanTilt
{
    public static PlayerController instance;

    AttackManager attackManager;


    [SerializeField] int _health;
    int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = (value < 0 ? 0 : value);
        }
    }

    Animator ICanAttack.GetAnimator => GetComponent<Animator>();

    bool ICanTilt.freezeRotation {
        get => rb.freezeRotation;
        set => rb.freezeRotation = value;
    }
    float ICanTilt.rotation {
        get => rb.rotation;
    }
    void ICanTilt.SetRotation(float angle)
    {
        rb.SetRotation(angle);
    }
    



    Weapon[] ICanAttack.GetAllWeapon()
    {
        return new Weapon[] { leftHand, rightHand};
    }
    void ICanAttack.StartCoroutine(IEnumerator enumerator)
    {
        StartCoroutine(enumerator);
    }

    [SerializeField] float
        walkSpeedX = 1,
        acceleration = 1,
        jumpForce = 6.6f,
        jumpAttackForse = 6.6f,
        runAttackForce = 10;

    float _direction = 1;

    [SerializeField] float actuallyspeedX;

    bool jumpKeyPressed;

    [SerializeField] int
        jumpCount = 0,
        jumpCountMax = 2;

    Weapon leftHand,
        rightHand;

    //components
    Rigidbody2D rb;
    GroundCheker gndCheker;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Debug.Log("Player is Start");
        gndCheker = GetComponentInChildren<GroundCheker>();
        Health = _health;
        rb = GetComponent<Rigidbody2D>();

        SetupHends();

        attackManager = new AttackManager((ICanAttack)this);
        attackManager.HideAllWeapon();
    }

    void Update()
    {
        if (gndCheker.IsGrounded)
        {
            jumpCount = 0;
        }

        MoveController();
        AttackControler();


        GetComponent<Animator>().SetFloat("movingX", Mathf.Abs(rb.velocity.x));
        GetComponent<Animator>().SetBool("isGrounded", gndCheker.IsGrounded);
    }

    void SetupHends()
    {
        leftHand = transform.Find("LeftHand").GetComponent<Weapon>();
        rightHand = transform.Find("RightHand").GetComponent<Weapon>();
    }

    void MoveController()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = 0;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            actuallyspeedX = walkSpeedX + acceleration;
        }
        else {
            actuallyspeedX = walkSpeedX;
        }

        if (Input.GetKey(KeyCode.D))
        {
            //velocity += new Vector2(actuallyspeedX, 0);//new Vector2(actuallyspeedX * Mathf.Cos(rb.rotation * Mathf.PI / 180), actuallyspeedX * Mathf.Sin(rb.rotation * Mathf.PI / 180));
            velocity.x = actuallyspeedX;
            if (!flipByAttackController)
                FlipDirection(false);

        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity.x = -actuallyspeedX;
            
            if (!flipByAttackController)
                FlipDirection(true);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount < jumpCountMax)
            {
                jumpKeyPressed = true;
                GetComponent<Animator>().SetTrigger("jump");
            }
        }


        //var mgn = velocity.magnitude;
        //float angle = rb.rotation * Mathf.Deg2Rad;
        //var velocity1 = new Vector2(mgn * Mathf.Cos(angle), mgn * Mathf.Sin(angle));

        rb.velocity = velocity;

    }

    void FlipDirection(bool isFlip)
    {
        _direction = (isFlip ? -1 : 1);
        transform.localScale = new Vector3(_direction, 1,1);
    }

    Vector2 mousPositionLastClick;
    void AttackControler()
    {
        if (Input.GetMouseButtonDown(0) && !attackManager.IsSomeAttackPlaying)
        {
            var mousPositionClick = Input.mousePosition;
            mousPositionLastClick = Camera.main.ScreenToWorldPoint(mousPositionClick);

            //if ()
            {
                var slimAttacksGetter = new SlimAttacksGetter();

                slimAttacksGetter.LastDirection = (int)_direction;

                if (mousPositionLastClick.x - transform.position.x < 0)
                {
                    FlipDirection(true);
                }
                else
                {
                    FlipDirection(false);
                }

                slimAttacksGetter.CurrentDirection = (int)_direction;

                attackManager.Attacking(slimAttacksGetter);

                StartCoroutine("WaitForAttackEnd");
            }
        }


       
    }

    //AnimationEvent
    public void AnimationEvent_Jump()
    {
        if (jumpKeyPressed)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
        }

        jumpKeyPressed = false;

    }
    public void AnimationEvent_StartAttackInRun()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpAttackForse);
        StartCoroutine("CHLEN");
    }

    //Coroutines
    IEnumerator CHLEN()
    {
        yield return new WaitForSeconds(.3f);
        rb.AddForce(Vector2.down * runAttackForce, ForceMode2D.Impulse);
    }

    

    bool flipByAttackController = false;
    IEnumerator WaitForAttackEnd()
    {
        float delay = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length;

        flipByAttackController = true;

        //SetActiveHand(true);

        yield return new WaitForSeconds(delay);

        //SetActiveHand(false);

        flipByAttackController = false;
    }

    bool isInvulnerable = false;
    IEnumerator Invulnerable()
    {
        isInvulnerable = true;
        GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.5f);

        GetComponent<SpriteRenderer>().color = Color.white;
        isInvulnerable = false;
    }



    //override methods
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.transform.CompareTag("Slope"))
    //    {
    //        if (rb.rotation >= 50)
    //        {
    //            rb.SetRotation(50);
    //        }
    //        else if (rb.rotation <= -50)
    //        {
    //            rb.SetRotation(-50);
    //        }
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.transform.CompareTag("Slope"))
    //    {
    //        rb.SetRotation(0);
    //        rb.freezeRotation = true;
    //    }
    //}
    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.transform.CompareTag("Slope"))
    //        rb.freezeRotation = false;
    //}

    public void GetHurt(int damage)
    {
        if (isInvulnerable)
            return;

        Health -= damage;
        StartCoroutine("Invulnerable");
    }

    
}
