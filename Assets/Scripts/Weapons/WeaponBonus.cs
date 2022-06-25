using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBonus : MonoBehaviour
{
    private WeaponsManager weaponsManager;


    [SerializeField] private GameObject prefab;



    // ### Functions ###

    public void Getter(in WeaponsManager _weaponsManager)
    {
        weaponsManager = _weaponsManager;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);

            weaponsManager.InstantiateWeapon(prefab);
        }
    }
}
