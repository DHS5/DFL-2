using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct WeaponInfo
{
    public int ammunitionLeft;
    public bool canShoot;
    public float reloadEndTime;
}

public class Weapon : MonoBehaviour
{
    private WeaponsManager weaponsManager;

    private Player player;

    private EnemiesManager enemiesManager;

    private CursorManager cursor;
    
    
    [Tooltip("Distance max at which the player can shoot a zombie")]
    [SerializeField] private float range; public float Range { get { return range; } }

    [Tooltip("Angle max at which the player can shoot a zombie (in degrees)")]
    [SerializeField] private float angle; public float Angle { get { return angle; } }

    [Tooltip("Number of time using the weapon")]
    [SerializeField] private int ammunition; public int Ammunition { get { return ammunition; } }

    [Tooltip("Time before shooting again")]
    [SerializeField] private float reloadTime; public float ReloadTime { get { return reloadTime; } }

    [Tooltip("Max number of zombies possible to kill in one shot")]
    [SerializeField] private int maxVictim; public int MaxVictim { get { return maxVictim; } }

    public bool fireArm;
    public bool bigWeapon;

    private AudioSource audioSource;

    [Tooltip("Sprite of the weapon's ammunition")]
    [SerializeField] private Sprite weaponSprite;
    
    [Tooltip("AudioClip of the weapon (played on use)")]
    [SerializeField] private AudioClip audioClip;


    private bool canShoot;
    private float reloadEndTime;


    // ### Properties ###

    public WeaponInfo WeaponInfo
    {
        get { return new WeaponInfo 
        { ammunitionLeft = ammunition, canShoot = canShoot, reloadEndTime = reloadEndTime }; }
        set
        {
            ammunition = value.ammunitionLeft;
            reloadEndTime = value.reloadEndTime;
            canShoot = value.canShoot;
        }
    }



    private void Awake()
    {
        //audioSource = GetComponent<AudioSource>();
        //audioSource.clip = audioClip;
    }

    private void Start()
    {
        canShoot = true;

        weaponsManager.ActuGameUI(weaponSprite, ammunition, canShoot, ammunition > 0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && canShoot && weaponsManager.GameOn && cursor.locked)
        {
            Shoot();
        }
    }


    // ### Functions ###

    public void Getter(in WeaponsManager _weaponsManager, in Player _player, in EnemiesManager _enemiesManager, in CursorManager _cursor)
    {
        weaponsManager = _weaponsManager;
        player = _player;
        enemiesManager = _enemiesManager;
        cursor = _cursor;
    }
    public void Getter(in WeaponsManager _weaponsManager, in Player _player, in EnemiesManager _enemiesManager, in CursorManager _cursor, WeaponInfo info)
    {
        Getter(_weaponsManager, _player, _enemiesManager, _cursor);

        WeaponInfo = info;
        if (!canShoot)
            Invoke(nameof(Reload), reloadEndTime - Time.time);
    }


    /// <summary>
    /// Uses the weapon to kill zombies targetable
    /// </summary>
    protected virtual void Shoot()
    {
        // Initialization of the zombie's list & useful variables
        List<Enemy> zombieList = new(enemiesManager.enemies);
        Zombie z;
        Zombie target;

        float dist;
        float toZAngle;

        float score;
        float minScore;

        int victims = 0;


        // Direct effects of shoot
        canShoot = false;
        ammunition--;
        player.controller.CurrentState.Shoot(fireArm);
        //audioSource.Play();

        // Enemy kill
        do
        {
            minScore = range;
            target = null;

            for (int zNum = 0; zNum < zombieList.Count; zNum++)
            {
                z = (Zombie)zombieList[zNum];

                if (z != null && !z.dead)
                {
                    dist = z.rawDistance;
                    toZAngle = Vector3.Angle(z.playerLookDirection, -z.toPlayerDirection);

                    if (dist < range && toZAngle < angle)
                    {
                        score = dist * (toZAngle / angle);
                        if (score < minScore)
                        {
                            target = z;
                            minScore = score;
                        }
                    }
                }
            }

            if (target != null)
            {
                victims++;
                weaponsManager.numberOfKill++;
                target.Dead();
            }
        } while (victims < maxVictim && target != null);

        weaponsManager.ActuGameUI(ammunition, canShoot, ammunition > 0);

        if (ammunition > 0)
        {
            Invoke(nameof(Reload), reloadTime);
            reloadEndTime = Time.time + reloadTime;
        }

        else
        {
            Invoke(nameof(DestroyWeapon), 0.4f);
        }
    }

    public void DestroyWeapon()
    {
        if (player != null)
            player.controller.CurrentState.SetWeapon(false, bigWeapon);
        Destroy(gameObject);
    }

    /// <summary>
    /// Make the player able to shoot again
    /// </summary>
    public void Reload()
    {
        canShoot = true;

        weaponsManager.ActuGameUI(ammunition, canShoot, ammunition > 0);
    }
}
