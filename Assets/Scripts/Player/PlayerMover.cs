using System.Collections;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    //singleton pattern
    #region singleton
    public static PlayerMover singleton { get; private set; }
    private void Awake()
    {
        singleton = this;
    }
    #endregion
    #region MoveFields
    [Header("MoveSettings")]
    [SerializeField] private float speed;
    [SerializeField] private float speedInAir;
    [SerializeField] private float acceleration;
    [SerializeField] private float accelerationInAir;

    //X axis move input
    private float xMoveInput;
    //Stop moving
    private bool isMovingStopped = false;
    #endregion
    #region JumpFields
    [Header("JumpSettings")]
    [SerializeField] private float jumpForce;
    //count of jumps (for multijump)
    [SerializeField] private int maxJumpCount;
    [SerializeField] private float fallSpeed;

    private int jumpCount;
    #endregion
    #region GroundCheckFields
    [Header("GroundCheckSettings")]
    [SerializeField] private float radius;
    [SerializeField] private Vector2 offset;
    [SerializeField] private LayerMask ground;

    [HideInInspector] public bool IsGrounded { get; private set; }
    //Stopping ground check
    private bool isGroundCheckStopped = false;
    private float groundCheckStopTime = 0.15f;
    #endregion
    #region Events
    //jump event
    public delegate void JumpEvent();
    public event JumpEvent OnJump;
    #endregion
    //state of player 
    public State PlayerState;
    //bool for side check and rotation
    public bool isFacingRight;
    //X axis input settings
    public float XMoveInput
    {
        get
        {
            return xMoveInput;
        }
        set
        {
            if (XMoveInput > 1)
                XMoveInput = 1;
            else if (XMoveInput < -1)
                XMoveInput = -1;
            xMoveInput = value;
        }
    }
    //all types of state
    public enum State
    {
        Idle,
        Running,
        Jumping,
        Falling,
    }
    //rigidbody of player
    private Rigidbody2D rb;
    //X component of rigidbody
    private float rbX;
    //Y component of rigidbody
    private float rbY;
    private void Start()
    {
        //get rigidbody component
        rb = GetComponent<Rigidbody2D>();
        //when player repair - stop moving
        BrokenMachine.OnRepair += PlayerStop;
    }
    private void OnDisable()
    {
        BrokenMachine.OnRepair -= PlayerStop;
    }
    //moving stop method
    private void PlayerStop(float time)
    {
        StartCoroutine(PlayerStopCoroutine(time));
    }
    //moving stop coroutine
    private IEnumerator PlayerStopCoroutine(float time)
    {
        rb.simulated = false;
        yield return new WaitForSeconds(time);
        rb.simulated = true;
    }

    private void Update()
    {
        GroundCheck();
    }
    private void FixedUpdate()
    {
        Move();
        RbLimitation();
        StateCheck();
    }
    //jump method for button input
    public void JumpAction()
    {
        if (jumpCount > 0)
        {
            OnJump?.Invoke();
            Jump();
        }
    }
    #region MoveMethods
    //move method
    private void Move()
    {
        if (!isMovingStopped)
        {
            if (IsGrounded)
                rb.velocity += new Vector2(xMoveInput * acceleration, 0);
            else
                rb.velocity += new Vector2(xMoveInput * accelerationInAir, 0);
        }
    }
    #endregion
    #region JumpMethods
    //jump method
    private void Jump()
    {
        if (jumpCount > 0)
        {
            isGroundCheckStopped = true;
            //add force
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            StartCoroutine(GroundCheckStop());
        }
    }
    //ground check method
    private void GroundCheck()
    {
        if (!isGroundCheckStopped)
        {
            Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
            //ground check zone
            IsGrounded = Physics2D.OverlapCircle(playerPos - offset, radius, ground);
            if (IsGrounded)
            {
                jumpCount = maxJumpCount;
            }
            else
            {
                jumpCount = 0;
            }
        }
    }
    //ground check stop
    private IEnumerator GroundCheckStop()
    {
        isGroundCheckStopped = true;
        IsGrounded = false;
        yield return new WaitForSeconds(groundCheckStopTime);
        isGroundCheckStopped = false;
    }

    #endregion
    //state check and update
    private void StateCheck()
    {
        if (IsGrounded && Mathf.Abs(xMoveInput) < 0.01f)
            PlayerState = State.Idle;
        else if (IsGrounded && Mathf.Abs(xMoveInput) > 0)
            PlayerState = State.Running;
        else if (!IsGrounded && rbY > 0)
            PlayerState = State.Jumping;
        else if (!IsGrounded && rbY < 0)
            PlayerState = State.Falling;
        #region Rotation
        //rotation update 
        if (IsGrounded)
        {
            if (xMoveInput > 0.05f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                isFacingRight = true;
            }
            else if (xMoveInput < 0.05f && xMoveInput != 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
                isFacingRight = false;
            }
        }
        else
        {
            if (rbX < 0.1f && rbX > -0.1f)
            {
                //stay on current euler
            }
            else if (rbX > 0.1f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                isFacingRight = true;
            }
            else if (rbX < 0.1f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
                isFacingRight = false;
            }
        }
        #endregion
    }
    //rigidbody limitation
    private void RbLimitation()
    {
        rbX = rb.velocity.x;
        rbY = rb.velocity.y;

        if (rbY < -fallSpeed)
            rb.velocity = new Vector2(rb.velocity.x, -fallSpeed);


        if (IsGrounded)
        {
            if (rbX > speed)
                rb.velocity = new Vector2(speed, rb.velocity.y);
            else if (rbX < -speed)
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            if (xMoveInput == 0)
                rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            if (rbX > speedInAir)
                rb.velocity = new Vector2(speedInAir, rb.velocity.y);
            else if (rbX < -speedInAir)
                rb.velocity = new Vector2(-speedInAir, rb.velocity.y);
        }


    }
    #region Gizmos
    private void OnDrawGizmos()
    {
        //ground check zone draw
        #region GroundCheck
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
        Gizmos.DrawWireSphere(playerPos - offset, radius);
        #endregion
    }
    #endregion
}
