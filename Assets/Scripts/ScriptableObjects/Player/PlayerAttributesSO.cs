using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerAttribute", menuName = "ScriptableObjects/Player/PlayerAttribute", order = 1)]
public class PlayerAttributesSO : ScriptableObject
{
    [Header("Physic parameters")]
    public Vector3 size;


    [Header("Control parameters")]
    [SerializeField] private float dirSensitivity; public float DirSensitivity { get { return dirSensitivity; } }
    [SerializeField] private float dirGravity; public float DirGravity { get { return dirGravity; } }
    [Space]
    [SerializeField] private float accSensitivity; public float AccSensitivity { get { return accSensitivity; } }
    [SerializeField] private float accGravity; public float AccGravity { get { return accGravity; } }
    [Space]
    public float snap;


    [Header("Normal speed variables of the player")]
    [Tooltip("Forward speed of the player when running forward")]
    [SerializeField] private float normalSpeed; public float NormalSpeed { get { return normalSpeed; } }

    [Tooltip("Side speed multiplier of the player")]
    [SerializeField] private float normalSideSpeed; public float NormalSideSpeed { get { return normalSideSpeed; } }



    [Header("Acceleration parameters")]
    [Tooltip("Acceleration multiplier of the player")]
    [SerializeField] private float accelerationM; public float AccelerationM { get { return accelerationM; } }

    [Tooltip("Side speed during an acceleration of the player")]
    [SerializeField] private float accSideSpeed; public float AccSideSpeed { get { return accSideSpeed; } }
    [Space]
    [Tooltip("Time during which the player is able to accelerate")]
    public float accelerationTime;

    [Tooltip("Time during which the player need to rest to accelerate again")]
    public float accelerationRestTime;



    [Header("Slowrun parameters")]
    [Tooltip("Slowrun multiplier of the player")]
    [SerializeField] private float slowM; public float SlowM { get { return slowM; } }

    [Tooltip("Side speed during a slowrun of the player")]
    [SerializeField] private float slowSideSpeed; public float SlowSideSpeed { get { return slowSideSpeed; } }



    [Header("Jump parameters")]
    [Tooltip("Height the player is reaching when jumping")]
    [SerializeField] private float jumpHeight; public float JumpHeight { get { return jumpHeight; } }
    
    [Tooltip("Bonus height the player is reaching when fliping")]
    [SerializeField] private float flipHeight; public Vector3 FlipHeight { get { return new Vector3(0, flipHeight, 0); } }

    [Tooltip("Hang time when the player's jumping")]
    public float HangTime { get { return 0.5f + 0.09f * (jumpHeight - 2); } }


    [Header("Skill moves")]
    [SerializeField] private bool canJuke; public bool CanJuke { get { return canJuke; } }
    [SerializeField] private bool canSpin; public bool CanSpin { get { return canSpin; } }
    [SerializeField] private bool canFeint; public bool CanFeint { get { return canFeint; } }
    [SerializeField] private bool canSlide; public bool CanSlide { get { return canSlide; } }
    [SerializeField] private bool canFlip; public bool CanFlip { get { return canFlip; } }
    [SerializeField] private bool canTruck; public bool CanTruck { get { return canTruck; } }


    [Header("Skill moves speed")]
    [Tooltip("Juke side speed of the player")]
    [SerializeField] private float jukeSideSpeed; public float JukeSideSpeed { get { return jukeSideSpeed; } }

    [Tooltip("Juke speed of the player")]
    [SerializeField] private float jukeSpeed; public float JukeSpeed { get { return jukeSpeed; } }

    [Tooltip("Spin side speed of the player")]
    [SerializeField] private float spinSideSpeed; public float SpinSideSpeed { get { return spinSideSpeed; } }

    [Tooltip("Spin speed of the player")]
    [SerializeField] private float spinSpeed; public float SpinSpeed { get { return spinSpeed; } }

    [Tooltip("Feint side speed of the player")]
    [SerializeField] private float feintSideSpeed; public float FeintSideSpeed { get { return feintSideSpeed; } }

    [Tooltip("Feint speed of the player")]
    [SerializeField] private float feintSpeed; public float FeintSpeed { get { return feintSpeed; } }

    [Tooltip("Slide speed of the player")]
    [SerializeField] private float slideSpeed; public float SlideSpeed { get { return slideSpeed; } }
    
    [Tooltip("Flip speed of the player")]
    [SerializeField] private float flipSpeed; public float FlipSpeed { get { return flipSpeed; } }
}
