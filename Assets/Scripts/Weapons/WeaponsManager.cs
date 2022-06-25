using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;


    private Weapon currentWeapon;

    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    // ### Functions ###

    private void InstantiateWeapon()
    {

    }


    public void GameOver()
    {
        Destroy(currentWeapon.gameObject);
    }
}
