using System.Collections;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
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

    private float xMoveInput;
    //Stopping moving
    private bool isMovingStopped = false;
    private float movingStopTime = 0.15f;
    #endregion
    #region JumpFields
    [Header("JumpSettings")]
    [SerializeField] private float jumpForce;
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
    public delegate void JumpEvent();
    public event JumpEvent OnJump;
    #endregion
    public State PlayerState;
    public bool isFacingRight;
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
    public enum State
    {
        Idle,
        Running,
        Jumping,
        Falling,
    }

    private Rigidbody2D rb;
    private float rbX;
    private float rbY;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GroundCheck();
        MoveInput();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpAction();
        }
    }
    private void FixedUpdate()
    {
        Move();
        RbLimitation();
        StateCheck();
    }
    public void JumpAction()
    {
        if (jumpCount > 0)
        {
            OnJump?.Invoke();
            Jump();
        }
    }
    #region MoveMethods
    private void MoveInput()
    {
        xMoveInput = Input.GetAxisRaw("Horizontal");
    }
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
    private void Jump()
    {
        if (jumpCount > 0)
        {
            isGroundCheckStopped = true;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            StartCoroutine(GroundCheckStop());
        }
    }
    private void GroundCheck()
    {
        if (!isGroundCheckStopped)
        {
            Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
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
    private IEnumerator GroundCheckStop()
    {
        isGroundCheckStopped = true;
        IsGrounded = false;
        yield return new WaitForSeconds(groundCheckStopTime);
        isGroundCheckStopped = false;
    }

    #endregion
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
        #region GroundCheck
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
        Gizmos.DrawWireSphere(playerPos - offset, radius);
        #endregion
    }
    #endregion
}
