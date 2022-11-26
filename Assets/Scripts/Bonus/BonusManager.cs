using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;



    [SerializeField] private GameObject[] bonusPrefabs;

    private Bonus activeBonus;


    private GameObject bonusZone;


    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    // ### Functions ###


    /// <summary>
    /// Gets the field zone
    /// </summary>
    private void GetZone()
    {
        bonusZone = main.FieldManager.field.bonusZone;
    }


    /// <summary>
    /// Generates a bonus on the first part of the field
    /// </summary>
    public void GenerateBonus()
    {
        Bonus bonus;

        // Gets the field zone
        GetZone();

        // Gets the zones position and scale info
        Vector3 zonePos = bonusZone.transform.position;
        float xScale = bonusZone.transform.localScale.x / 2;
        float zScale = bonusZone.transform.localScale.z / 2;

        // Gets a random position in the first part of the field zone
        Vector3 randomPos = new Vector3(Random.Range(-xScale, xScale), 1.5f, Random.Range(-zScale, zScale)) + zonePos;

        // Instantiate the bonus
        bonus = Instantiate(bonusPrefabs[Random.Range(0, bonusPrefabs.Length)], randomPos, Quaternion.identity).GetComponent<Bonus>();
        bonus.bonusManager = this;
        bonus.player = main.PlayerManager.player;
        activeBonus = bonus;
    }

    /// <summary>
    /// Destroys the active bonus
    /// </summary>
    public void DestroyBonus()
    {
        Destroy(activeBonus.gameObject);
    }



    public void BonusAnim(bool anim, float time, Color color, Sprite sprite)
    {
        if (anim)
        {
            main.GameUIManager.BonusBarAnim(time, color, sprite);
            main.PlayerManager.player.effects.BonusVolume(color, time);
        }
    }

    public void AddLife(int lifeNumber)
    {
        main.GameUIManager.ModifyLife(true, lifeNumber);
    }
}
