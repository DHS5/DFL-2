using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    private bool canShoot;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(1) && canShoot)
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

    protected virtual void Shoot()
    {
        // Initialization of the zombie's list & useful variables
        List<Enemy> zombieList = new(enemiesManager.enemies);
        Zombie z;

        float dist;
        float toZAngle;

        int victims = 0;
        int zNum = 0;


        canShoot = false;
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
            Invoke(nameof(Reload), reloadTime);

        else
            Destroy(gameObject);
    }

    protected void Reload()
    {
        canShoot = true;
    }
}
