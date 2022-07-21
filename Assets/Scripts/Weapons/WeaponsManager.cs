using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;


    [SerializeField] private GameObject[] weaponBonusPrefabs;


    private Weapon fpWeapon;
    private Weapon tpWeapon;

    private Weapon currentWeapon;


    // ### Properties ###
    public bool GameOn
    {
        get { return main.GameManager.GameOn; }
    }



    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    // ### Functions ###


    public void GenerateWeaponBonus()
    {
        main.PlayerManager.player.fPPlayer.football.SetActive(false);
        main.PlayerManager.player.tPPlayer.football.SetActive(false);

        // Gets the zones position and scale info
        GameObject weaponZone = main.FieldManager.field.bonusZone;
        
        Vector3 zonePos = weaponZone.transform.position;
        float xScale = weaponZone.transform.localScale.x / 2;
        float zScale = weaponZone.transform.localScale.z / 2;

        // Gets a random position in the first part of the field zone
        Vector3 randomPos = new Vector3(Random.Range(-xScale, xScale), 1.5f, Random.Range(-zScale, zScale)) + zonePos;

        WeaponBonus bonus = Instantiate(weaponBonusPrefabs[Random.Range(0, weaponBonusPrefabs.Length)], randomPos, Quaternion.identity).GetComponent<WeaponBonus>();
        bonus.Getter(this);
    }


    public void InstantiateWeapon(GameObject prefab)
    {
        if (currentWeapon != null)
        {
            currentWeapon.DestroyWeapon();
            ((main.PlayerManager.ViewType != ViewType.FPS) ? fpWeapon : tpWeapon).DestroyWeapon();
        }

        fpWeapon = Instantiate(prefab, main.PlayerManager.player.fPPlayer.rightHand.transform).GetComponent<Weapon>();
        tpWeapon = Instantiate(prefab, main.PlayerManager.player.tPPlayer.rightHand.transform).GetComponent<Weapon>();

        currentWeapon = (main.PlayerManager.ViewType == ViewType.FPS) ? fpWeapon : tpWeapon;

        currentWeapon.Getter(this, main.PlayerManager.player, main.EnemiesManager, main.CursorManager);

        main.PlayerManager.player.controller.CurrentState.SetWeapon(true, currentWeapon.bigWeapon);
    }

    public void ViewChange()
    {
        if (currentWeapon != null)
        {
            Weapon newWeapon = (main.PlayerManager.ViewType == ViewType.FPS) ? fpWeapon : tpWeapon;
            newWeapon.Getter(this, main.PlayerManager.player, main.EnemiesManager, main.CursorManager, currentWeapon.WeaponInfo);
            currentWeapon = newWeapon;
            main.PlayerManager.player.controller.CurrentState.SetWeapon(true, currentWeapon.bigWeapon);
        }
    }


    public void GameOver()
    {
        if (currentWeapon != null)
            Destroy(currentWeapon.gameObject);
    }


    public void ActuGameUI()
    {
        main.GameUIManager.DisplayWeapon(currentWeapon.WeaponInfo.ammunitionLeft, currentWeapon.fireArm, currentWeapon.WeaponInfo.canShoot, currentWeapon.WeaponInfo.ammunitionLeft > 0);
    }
}
