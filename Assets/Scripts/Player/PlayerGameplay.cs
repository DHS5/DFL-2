using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the gameplay of the player
/// </summary>
public class PlayerGameplay : MonoBehaviour
{
    [Tooltip("Player script")]
    private Player player;



    [Tooltip("Player's number of life")]
    [HideInInspector] public int lifeNumber = 1;

    [Tooltip("Whether the player is on the field")]
    [HideInInspector] public bool onField = false;

    [Tooltip("Whether the player is in a zone where he's chasable")]
    [HideInInspector] public bool isChasable = false;

    [Tooltip("Whether the player is invincible (bonus)")]
    [HideInInspector] public bool isInvincible = false;

    [Tooltip("Whether the player is visible")]
    [HideInInspector] public bool isVisible = true;

    [Tooltip("Whether the player is freezed")]
    [HideInInspector] public bool freeze = true;




    private void Awake()
    {
        player = GetComponent<Player>();
    }



    /// <summary>
    /// Called when the player collide with a trigger
    /// </summary>
    /// <param name="other">Collider of the trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        // When the player enter the tunnel --> Generates a new field
        if (other.gameObject.CompareTag("NextWave"))
        {
            // Deactivates the trigger (prevent from triggering several times)
            other.gameObject.SetActive(false);

            transform.position = new Vector3(0, 0, transform.position.z + 40);

            player.gameManager.NextWave();
        }
        // When the player accounter a field limit
        if (other.gameObject.CompareTag("FieldLimit"))
        {
            // Deactivates the trigger (prevent from triggering several times)
            other.gameObject.SetActive(false);

            onField = !onField;
        }
    }
    /// <summary>
    /// Called when the player collide with a colliding object
    /// </summary>
    /// <param name="collision">Collider of the colliding object</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (!player.gameManager.GameOver)
        {
            // When the player collides with an enemy --> game over
            if (collision.gameObject.CompareTag("Enemy") && !isInvincible)
            {
                Hurt(collision.gameObject);
                Debug.Log("Hurt by enemy");
            }
            // When the player collides with an obstacle --> game over
            if (collision.gameObject.CompareTag("Obstacle") && !isInvincible)
            {
                AudioSource a = collision.gameObject.GetComponent<AudioSource>();
                if (a != null)
                {
                    a.Play();
                    a.volume = 1f;
                }
                
                Hurt(collision.gameObject);
                Debug.Log("Hurt by obstacle");
            }
            if (collision.gameObject.CompareTag("OutOfBounds"))
            {
                Dead(collision.gameObject);
                Debug.Log("OutOfBounds");
            }
        }
    }

    private void Hurt(GameObject g)
    {
        //playerManager.audioSource.Play();

        lifeNumber--;
        if (lifeNumber > 0)
        {
            //gameManager.gameUIManager.ModifyLife(false, lifeNumber - 1);
            isInvincible = true;
            Invoke(nameof(NotInvincible), 1f);
        }
        // If the player has no life left, GAME OVER
        else Dead(g);
    }

    private void NotInvincible() { isInvincible = false; }

    /// <summary>
    /// Game Over for the player
    /// </summary>
    /// <param name="g"></param>
    private void Dead(GameObject g)
    {
        //Vector3 dir = g.transform.position - gameManager.player.transform.position;
        //Vector3 playerRot = gameManager.player.transform.rotation.eulerAngles;
        //gameManager.player.transform.rotation = Quaternion.Euler(playerRot.x, Quaternion.LookRotation(dir, gameManager.player.transform.up).eulerAngles.y, playerRot.z);
        //playerManager.gameManager.GameOver = true;
    }

}
