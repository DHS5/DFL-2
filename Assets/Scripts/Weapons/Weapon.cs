using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct WeaponInfo
{
    public float range;
    public float angle;
    public int ammunition;
    public float reloadTime;
    public int maxVictim;
}

public abstract class Weapon : MonoBehaviour
{
    private Player player;

    private EnemiesManager enemiesManager;
    
    
    [Tooltip("Distance max at which the player can shoot a zombie")]
    [SerializeField] private float range;

    [Tooltip("Angle max at which the player can shoot a zombie (in degrees)")]
    [SerializeField] private float angle;

    [Tooltip("Number of time using the weapon")]
    [SerializeField] private int ammunition;

    [Tooltip("Time before shooting again")]
    [SerializeField] private float reloadTime;

    [Tooltip("Max number of zombies possible to kill in one shot")]
    [SerializeField] private int maxVictim;


    private AudioSource audioSource;

    [Tooltip("AudioClip of the weapon (played on use)")]
    [SerializeField] private AudioClip audioClip;


    public bool CanShoot { get; private set; }
    public float ReloadEndTime { get; private set; }


    // ### Properties ###

    public WeaponInfo WeaponInfo
    {
        get { return new WeaponInfo 
        { range = range, angle = angle, ammunition = ammunition, reloadTime = reloadTime, maxVictim = maxVictim }; }
        set
        {
            range = value.range;
            angle = value.angle;
            ammunition = value.ammunition;
            reloadTime = value.reloadTime;
            maxVictim = value.maxVictim;
        }
    }



    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(1) && CanShoot)
        {
            Shoot();
        }
    }


    // ### Functions ###

    public void Getter(in Player _player, in EnemiesManager _enemiesManager)
    {
        player = _player;
        enemiesManager = _enemiesManager;
    }
    public void Getter(in Player _player, in EnemiesManager _enemiesManager, WeaponInfo info)
    {
        Getter(_player, _enemiesManager);

        WeaponInfo = info;
    }


    /// <summary>
    /// Uses the weapon to kill zombies targetable
    /// </summary>
    protected virtual void Shoot()
    {
        // Initialization of the zombie's list & useful variables
        List<Enemy> zombieList = new(enemiesManager.enemies);
        Zombie z;

        float dist;
        float toZAngle;

        int victims = 0;
        int zNum = 0;


        CanShoot = false;
        audioSource.Play();

        while (victims < maxVictim && zNum < zombieList.Count)
        {
            z = (Zombie) zombieList[zNum];

            dist = z.rawDistance;
            toZAngle = Vector3.Angle(z.playerLookDirection, -z.toPlayerDirection);

            if (dist < range && toZAngle < angle)
            {
                ammunition--;
                victims++;
                z.Dead();
            }

            zNum++;
        }

        if (ammunition > 0)
        {
            Invoke(nameof(Reload), reloadTime);
            ReloadEndTime = Time.time + reloadTime;
        }

        else
            Destroy(gameObject);
    }


    /// <summary>
    /// Make the player able to shoot again
    /// </summary>
    public void Reload()
    {
        CanShoot = true;
    }
}
