using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "PlayerAttribute", menuName = "ScriptableObjects/Player/PlayerAttribute", order = 1)]
public class PlayerAttributesSO : ScriptableObject
{
    [Header("Physic parameters")]
    public Vector3 size;


    [Header("Control parameters")]
    [SerializeField][Range(4, 20)] private float dirSensitivity; public float DirSensitivity { get { return dirSensitivity; } }
    [SerializeField][Range(1, 20)] private float dirGravity; public float DirGravity { get { return dirGravity; } }
    [Space]
    [SerializeField][Range(2, 20)] private float accSensitivity; public float AccSensitivity { get { return accSensitivity; } }
    [SerializeField][Range(1, 20)] private float accGravity; public float AccGravity { get { return accGravity; } }
    [Space]
    [Range(0.001f, 0.01f)] public float snap;


    [Header("Normal speed variables of the player")]
    [Tooltip("Forward speed of the player when running forward")]
    [SerializeField][Range(10, 18)] private float normalSpeed; public float NormalSpeed { get { return normalSpeed; } }

    [Tooltip("Side speed multiplier of the player")]
    [SerializeField][Range(5, 10)] private float normalSideSpeed; public float NormalSideSpeed { get { return normalSideSpeed; } }



    [Header("Acceleration parameters")]
    [Tooltip("Acceleration multiplier of the player")]
    [SerializeField][Range(1.1f, 1.75f)] private float accelerationM; public float AccelerationM { get { return accelerationM; } }

    [Tooltip("Side speed during an acceleration of the player")]
    [SerializeField][Range(2, 8)] private float accSideSpeed; public float AccSideSpeed { get { return accSideSpeed; } }
    [Space]
    [Tooltip("Time during which the player is able to accelerate")]
    [Range(1, 6)] public float accelerationTime;

    [Tooltip("Time during which the player need to rest to accelerate again")]
    [Range(3, 12)] public float accelerationRestTime;



    [Header("Slowrun parameters")]
    [Tooltip("Slowrun multiplier of the player")]
    [SerializeField][Range(0.1f, 0.9f)] private float slowM; public float SlowM { get { return slowM; } }

    [Tooltip("Side speed during a slowrun of the player")]
    [SerializeField][Range(7, 14)] private float slowSideSpeed; public float SlowSideSpeed { get { return slowSideSpeed; } }



    [Header("Jump parameters")]
    [Tooltip("Height the player is reaching when jumping")]
    [SerializeField] private float jumpHeight; public float JumpHeight { get { return jumpHeight; } }
    
    [Tooltip("Bonus height the player is reaching when fliping")]
    [SerializeField] private float flipHeight; public Vector3 FlipHeight { get { return new Vector3(0, flipHeight, 0); } }

    [Tooltip("Hang time when the player's jumping")]
    public float HangTime { get { return 0.5f + 0.09f * (jumpHeight - 2); } }

    [Space]
    [Tooltip("Jump cost")]
    [SerializeField] private float jumpCost; public float JumpCost { get { return jumpCost; } }
    
    [Tooltip("Flip cost")]
    [SerializeField] private float flipCost; public float FlipCost { get { return flipCost; } }

    [Tooltip("Total jump and flip stamina")]
    [SerializeField] private float jumpStamina; public float JumpStamina { get { return jumpStamina; } }

    [Space]
    [Tooltip("Total recharge time of jump and flip")]
    [SerializeField] private float jumpRechargeTime; public float JumpRechargeTime { get { return jumpRechargeTime; } }



    [Header("Skill moves")]
    [SerializeField] private bool canJuke; public bool CanJuke { get { return canJuke; } }
    [SerializeField] private bool canSpin; public bool CanSpin { get { return canSpin; } }
    [SerializeField] private bool canJukeSpin; public bool CanJukeSpin { get { return canJukeSpin; } }
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


    [Space]
    public Material teamMaterial;
}
