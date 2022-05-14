using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    [Tooltip("Singleton Instance of the GameManager")]
    [SerializeField] private GameManager gameManager;


    [SerializeField] private GameObject[] bonusPrefabs;

    private Bonus activeBonus;


    private GameObject fieldZone;

    /// <summary>
    /// Gets the field zone
    /// </summary>
    private void GetZone()
    {
        fieldZone = gameManager.currentField.fieldZone;
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
        Vector3 zonePos = fieldZone.transform.position;
        float xScale = fieldZone.transform.localScale.x / 2;
        float zScale = fieldZone.transform.localScale.z / 2.5f;

        // Gets a random position in the first part of the field zone
        Vector3 randomPos = new Vector3(Random.Range(-xScale, xScale), 1.5f, Random.Range(-zScale, 0)) + zonePos;

        // Instantiate the bonus
        bonus = Instantiate(bonusPrefabs[Random.Range(0, bonusPrefabs.Length)], randomPos, Quaternion.identity).GetComponent<Bonus>();
        bonus.bonusManager = this;
        bonus.player = gameManager.player;
        activeBonus = bonus;
    }

    /// <summary>
    /// Destroys the active bonus
    /// </summary>
    public void DestroyBonus()
    {
        Destroy(activeBonus);
    }



    public void BonusAnim(bool bar, float time, Color color)
    {
        if (bar) gameManager.gameUIManager.BonusBarAnim(time, color);
    }

    public void AddLife(int lifeNumber)
    {
        gameManager.gameUIManager.ModifyLife(true, lifeNumber);
    }
}
