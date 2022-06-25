using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;

    private Weapon fpWeapon;
    private Weapon tpWeapon;

    private Weapon currentWeapon;

    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    // ### Functions ###

    private void InstantiateWeapon()
    {
        //Weapon weapon;

        //weapon.Getter(main.PlayerManager.player, main.EnemiesManager);
    }
    private void ViewChange()
    {
        if (currentWeapon != null)
        {
            Weapon newWeapon = (main.PlayerManager.ViewType == ViewType.FPS) ? fpWeapon : tpWeapon;
            newWeapon.Getter(main.PlayerManager.player, main.EnemiesManager, currentWeapon.WeaponInfo);

            if (!currentWeapon.CanShoot)
                newWeapon.Invoke(nameof(newWeapon.Reload), currentWeapon.ReloadEndTime);
        }
    }


    public void GameOver()
    {
        Destroy(currentWeapon.gameObject);
    }
}
