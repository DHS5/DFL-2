using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Gives the user control of the player
/// The user can accelerate, decelerate, go on the sides and look on the sides (without changing the running orientation)
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Tooltip("Player script")]
    private Player player;


    public PlayerState CurrentState { get; private set; }



    [Tooltip("Rigidbody of the player")]
    public Rigidbody PlayerRigidbody { get; private set; }


    [Tooltip("Gravity multiplier constant")]
    readonly float gravityScale = 6f;
    [Tooltip("Jump multiplier constant")]
    readonly float jumpCst = 1.25f;



    public PlayerAttributesSO playerAtt;
    public PlayerUniversalDataSO playerUD;

    // Control parameters
    private float dirSensitivity;
    private float dirGravity;
    private float accSensitivity;
    private float accGravity;
    private float snap;



    private float realDir;
    private float realAcc;


    /// <summary>
    /// Velocity of the player
    /// </summary>
    public Vector3 Velocity { get; private set; }

    [Tooltip("Current speed of the player")]
    private float speed;

    [Tooltip("Current side speed of the player")]
    private float sideSpeed;


    [Tooltip("Bonus speed attribute of the player (changed by the bonus)")]
    [HideInInspector] public float bonusSpeed = 0f;
    [Tooltip("Bonus jump attribute of the player (changed by the bonus)")]
    [HideInInspector] public Vector3 bonusJump = Vector3.zero;



    // Player state variables
    public bool OnGround { get; private set; }
    public bool CanAccelerate { get; set; }
    public bool AlreadySlide { get; private set; }
    public bool Sprinting { get; private set; }
    public float SprintStartTime { get; private set; }



    // ### Properties ###

    public float Direction
    {
        get { return realDir; }
    }
    public float Acceleration
    {
        get { return realAcc; }
    }

    public float Speed
    {
        get { return speed + bonusSpeed; }
        set { speed = value; }
    }

    public float FSpeed
    {
        get { return Mathf.Sqrt(Speed * Speed - SideSpeed * SideSpeed); }
    }

    public float SideSpeed
    {
        get { return sideSpeed; }
        set { sideSpeed = value; }
    }

    /// <summary>
    /// Jump power of the player
    /// </summary>
    public Vector3 JumpPower
    {
        get { return new Vector3(0, Mathf.Sqrt(playerAtt.JumpHeight * jumpCst * -2 * (Physics.gravity.y * gravityScale)), 0); ; }
    }


    private void Awake()
    {
        player = GetComponent<Player>();

        PlayerRigidbody = GetComponent<Rigidbody>();

        GetControlParams();
    }


    private void Start()
    {
        PlayerRescale();

        CurrentState = new RunPS(player);

        CanAccelerate = true;
        AlreadySlide = false;
    }

    private void Update()
    {
        FilterDir();
        FilterAcc();

        if (!player.gameplay.freeze)
            CurrentState = CurrentState.Process();

        Velocity = ( Vector3.forward * FSpeed + Vector3.right * sideSpeed ) * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        // Keep the gravity constant
        PlayerRigidbody.AddForce(Physics.gravity * gravityScale);
    }

    void LateUpdate()
    {
        if (!player.gameplay.freeze)
        {
            transform.Translate(Velocity); // Makes the player run
        }
    }


    // ### Functions ###
    private void PlayerRescale()
    {
        var fpScale = player.fPPlayer.gameObject.transform.localScale;
        var tpScale = player.tPPlayer.gameObject.transform.localScale;
        player.fPPlayer.gameObject.transform.localScale = new Vector3(fpScale.x * playerAtt.size.x, fpScale.y * playerAtt.size.y, fpScale.z * playerAtt.size.z);
        player.tPPlayer.gameObject.transform.localScale = new Vector3(tpScale.x * playerAtt.size.x, tpScale.y * playerAtt.size.y, tpScale.z * playerAtt.size.z);
    }

    private void GetControlParams()
    {
        dirSensitivity = playerAtt.DirSensitivity;
        dirGravity = playerAtt.DirGravity;
        accSensitivity = playerAtt.AccSensitivity;
        accGravity = playerAtt.AccGravity;
        snap = playerAtt.snap;
    }


    private void FilterDir()
    {
        float rawDir = Input.GetAxisRaw("Horizontal");

        if (rawDir != 0)
            realDir += rawDir * dirSensitivity * Time.deltaTime;
        else if (rawDir == 0)
        {
            if (realDir > 0)
            {
                realDir -= dirGravity * Time.deltaTime;
                if (realDir < 0) realDir = 0f;
            }
            else if (realDir < 0)
            {
                realDir += dirGravity * Time.deltaTime;
                if (realDir > 0) realDir = 0f;
            }
        }

        if (Mathf.Abs(realDir) <= snap) SnapDir();

        realDir = Mathf.Clamp(realDir, -1, 1);
    }
    private void FilterAcc()
    {
        float rawAcc = Input.GetAxisRaw("Vertical");

        if (rawAcc != 0)
            realAcc += rawAcc * accSensitivity * Time.deltaTime;
        else if (rawAcc == 0)
        {
            if (realAcc > 0)
            {
                realAcc -= accGravity * Time.deltaTime;
                if (realAcc < 0) realAcc = 0f;
            }
            else if (realAcc < 0)
            {
                realAcc += accGravity * Time.deltaTime;
                if (realAcc > 0) realAcc = 0f;
            }
        }

        if (Mathf.Abs(realAcc) <= snap) SnapAcc();

        realAcc = Mathf.Clamp(realAcc, -1, 1);
    }

    public void SnapDir() { realDir = 0f; }
    public void SnapAcc() { realAcc = 0f; }

    public void FullDir(float side) { realDir = side / Mathf.Abs(side); }

    /// <summary>
    /// Detects a collision with the ground to know if the player is on the ground
    /// </summary>
    /// <param name="collision">Collider of the colliding game object</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            OnGround = true;
    }

    public void Jump()
    {
        PlayerRigidbody.AddForce(JumpPower + bonusJump, ForceMode.Impulse);
        OnGround = false;
    }


    public void Sprint() { if (!Sprinting) { Sprinting = true; SprintStartTime = Time.time; Invoke(nameof(Rest), playerAtt.accelerationTime); } }
    private void Rest() { Sprinting = false; CanAccelerate = false; Invoke(nameof(Rested) , playerAtt.accelerationRestTime) ; }
    private void Rested() { CanAccelerate = true; AlreadySlide = false; }
    public void Slide() { AlreadySlide = true; }


    public void Rain()
    {
        dirGravity /= 2;
        dirSensitivity /= 2;

        accGravity /= 2;
        accSensitivity /= 2;
    }
}
