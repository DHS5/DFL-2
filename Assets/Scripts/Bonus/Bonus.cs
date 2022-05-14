using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [Tooltip("Bonus Manager of the game")]
    [HideInInspector] public BonusManager bonusManager;

    [Tooltip("Game object of the player")]
    [HideInInspector] public GameObject player;

    [Tooltip("PlayerController of the player\n" +
        "bonusSpeed / bonusJump")]
    protected PlayerController playerC;
    [Tooltip("PlayerGameplay of the player\n" +
        "isChasable / isInvincible")]
    protected PlayerGameplay playerG;


    [SerializeField] private float bonusTime;
    [SerializeField] private Color bonusColor;

    protected bool bar = true;


    protected virtual void Start()
    {
        playerC = player.GetComponent<PlayerController>();
        playerG = player.GetComponent<PlayerGameplay>();
    }


    /// <summary>
    /// Called when the player collides with the bonus
    /// </summary>
    /// <param name="other">The collider in contact</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TriggerBonus();
            bonusManager.BonusAnim(bar, bonusTime, bonusColor);
        }
    }


    protected virtual void TriggerBonus()
    {
        gameObject.SetActive(false);
        Invoke(nameof(EndBonus), bonusTime);
    }

    /// <summary>
    /// Puts the bonus attributes back to their initial state
    /// </summary>
    protected virtual void EndBonus() { }
}
