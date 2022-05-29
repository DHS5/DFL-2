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
    [Tooltip("Player Manager")]
    [HideInInspector] public PlayerManager playerManager;


    private PlayerState currentState;


    
    [Tooltip("Rigidbody of the player")]
    public Rigidbody PlayerRigidbody { get; private set; }


    [Tooltip("Gravity multiplier constant")]
    readonly float gravityScale = 10f;



    [SerializeField] private float dirSensitivity;
    [SerializeField] private float dirGravity;

    [SerializeField] private float accSensitivity;
    [SerializeField] private float accGravity;

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



    [Header("Normal speed variables of the player")]
    [Tooltip("Forward speed of the player when running forward")]
    [SerializeField] private float normalSpeed; public float NormalSpeed { get { return normalSpeed; } }

    [Tooltip("Side speed multiplier of the player")]
    [SerializeField] private float normalSideSpeed; public float NormalSideSpeed { get { return normalSideSpeed; } }



    [Header("Acceleration parameters")]
    [Tooltip("Acceleration multiplier of the player")]
    [SerializeField] private float accelerationM; public float AccelerationM { get { return accelerationM; } }

    [Tooltip("Time during which the player is able to accelerate")]
    [SerializeField] private float accelerationTime;

    [Tooltip("Time during which the player need to rest to accelerate again")]
    [SerializeField] private float accelerationRestTime;

    [Tooltip("Side speed during an acceleration of the player")]
    [SerializeField] private float accSideSpeed; public float AccSideSpeed { get { return accSideSpeed; } }



    [Header("Slowrun parameters")]
    [Tooltip("Slowrun multiplier of the player")]
    [SerializeField] private float slowM; public float SlowM { get { return slowM; } }

    [Tooltip("Side speed during a slowrun of the player")]
    [SerializeField] private float slowSideSpeed; public float SlowSideSpeed { get { return slowSideSpeed; } }



    [Header("Jump parameters")]
    [Tooltip("Height the player is reaching when jumping")]
    [SerializeField] private float jumpHeight;

    [Tooltip("Hang time when the player's jumping")]
    [SerializeField] private float hangTime;



    [Header("Skill moves speed multipliers")]
    [Tooltip("Juke side speed of the player")]
    [SerializeField] private float jukeSideSpeed; public float JukeSideSpeed { get { return jukeSideSpeed; } }

    [Tooltip("Juke speed of the player")]
    [SerializeField] private float jukeSpeed; public float JukeSpeed { get { return jukeSpeed; } }

    [Tooltip("Spin side speed of the player")]
    [SerializeField] private float spinSideSpeed; public float SpinSideSpeed { get { return spinSideSpeed; } }

    [Tooltip("Spin peed of the player")]
    [SerializeField] private float spinSpeed; public float SpinSpeed { get { return spinSpeed; } }

    [Tooltip("Feint side speed of the player")]
    [SerializeField] private float feintSideSpeed; public float FeintSideSpeed { get { return feintSideSpeed; } }



    [Tooltip("Bonus speed attribute of the player (changed by the bonus)")]
    [HideInInspector] public float bonusSpeed = 0f;
    [Tooltip("Bonus jump attribute of the player (changed by the bonus)")]
    [HideInInspector] public Vector3 bonusJump = new Vector3(0, 0, 0);



    // Player state variables
    public bool OnGround { get; private set; }
    public bool CanAccelerate { get; private set; }
    public bool Sprinting { get; private set; }



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
        get { return new Vector3(0, Mathf.Sqrt(jumpHeight * -2 * (Physics.gravity.y * gravityScale)), 0); }
    }



    private void Start()
    {
        currentState = new RunPS(this, playerManager.playerAnimator);

        PlayerRigidbody = GetComponent<Rigidbody>();

        CanAccelerate = true;
    }

    private void Update()
    {
        FilterDir();
        FilterAcc();
        
        currentState = currentState.Process();

        Velocity = ( Vector3.forward * FSpeed + Vector3.right * sideSpeed ) * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        // Keep the gravity constant
        PlayerRigidbody.AddForce(Physics.gravity * gravityScale);
    }

    void LateUpdate()
    {
        if (!playerManager.gameplay.freeze)
        {
            transform.Translate(Velocity); // Makes the player run
        }
    }


    // ### Functions ###

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

        if (Mathf.Abs(realDir) <= 0.001f) realDir = 0f;

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

        if (Mathf.Abs(realAcc) <= 0.001f) realAcc = 0f;

        realAcc = Mathf.Clamp(realAcc, -1, 1);
    }


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


    public void Sprint() { if (!Sprinting) { Sprinting = true; Invoke(nameof(Rest), accelerationTime); } }
    private void Rest() { Sprinting = false; CanAccelerate = false; Invoke(nameof(Rested) , accelerationRestTime) ; }
    private void Rested() { CanAccelerate = true; }
}
