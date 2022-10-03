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

    [Tooltip("Whether the player is invincible (bonus)")]
    [HideInInspector] public bool isInvincible = false;

    [Tooltip("Whether the player is visible")]
    [HideInInspector] public bool isVisible = true;

    [Tooltip("Whether the player is frozen")]
    [HideInInspector] public bool freeze = true;

    [HideInInspector] public bool isTrucking = false;
    [HideInInspector] public bool isHighKneeing = false;



    private float tunnelWidth = 5f;
    private float recupTime = 1.5f;
    private float impactMin = 15;
    private float impactMax = 30;


    // ### Properties ###

    private bool Catchable
    {
        get { return !isInvincible && !isTrucking && !isHighKneeing; }
    }


    // ### Built-in ###


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
        // When the player go through the tunnel --> Generates a new field and destroys the former one
        if (other.gameObject.CompareTag("NextWave"))
        {
            // Deactivates the trigger (prevent from triggering several times)
            other.gameObject.SetActive(false);

            // Calls the next wave
            player.gameManager.NextWave();

            // Goes to the next field
            Vector3 spawnPos = player.fieldManager.stadium.SpawnPosition.transform.position;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, spawnPos.x - tunnelWidth, spawnPos.x + tunnelWidth), spawnPos.y, spawnPos.z);

            player.controller.CurrentState.TD(false);
        }

        // When the player accounter a field limit
        if (other.gameObject.CompareTag("FieldLimit"))
        {
            // Deactivates the trigger (prevent from triggering several times)
            other.gameObject.SetActive(false);

            // Changes the onField state of the player
            onField = !onField;

            if (onField)
            {
                player.gameManager.EnterField();
                player.controller.CurrentState.SetRandomCelebration();
            }
            else player.controller.CurrentState.TD(true);
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
            if (collision.gameObject.CompareTag("Enemy") && Catchable)
            {
                Debug.Log("Hurt by enemy ");
                Hurt(collision);
            }
            if (collision.gameObject.CompareTag("Enemy") && (isTrucking || isHighKneeing))
            {
                collision.gameObject.GetComponentInParent<Enemy>().Trucked(collision.impulse);
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
                
                Hurt(collision);
                Debug.Log("Hurt by obstacle");
            }

            if (collision.gameObject.CompareTag("StadiumLimit"))
            {
                Dead(collision);
                Debug.Log("OutOfBounds");
            }

            if (collision.gameObject.CompareTag("OutOfBounds"))
            {
                Lose();
                Debug.Log("OutOfBounds");
            }

            if (collision.gameObject.CompareTag("Buzzer"))
            {
                Win();
                Debug.Log("Win");
            }
        }
    }

    private void Hurt(Collision collision)
    {
        player.audioSource.Play();

        lifeNumber--;
        if (lifeNumber > 0)
        {
            player.playerManager.UIModifyLife(false, lifeNumber - 1);
            isInvincible = true;
            Invoke(nameof(NotInvincible), recupTime);
        }
        // If the player has no life left, GAME OVER
        else Dead(collision);
    }

    private void NotInvincible() { isInvincible = false; }

    /// <summary>
    /// Game Over for the player
    /// </summary>
    /// <param name="g"></param>
    private void Dead(Collision collision)
    {
        // Player animator dead
        player.controller.CurrentState.Dead();
        Vector3 impactDir = (collision.GetContact(0).point - transform.position).normalized;
        float impactPower = Mathf.Clamp(collision.impulse.magnitude / Time.fixedDeltaTime, impactMin, impactMax);
        Vector3 impact = impactPower * new Vector3(impactDir.x, 0, impactDir.z).normalized;

        player.activeBody.transform.localRotation = Quaternion.Euler(0, Quaternion.LookRotation(impact).eulerAngles.y, 0);
        player.controller.PlayerRigidbody.AddForce(-impact, ForceMode.Impulse);

        player.playerManager.DeadPlayer();
    }


    private void Win()
    {
        player.controller.PlayerRigidbody.AddForce(new Vector3(0, 0, -5), ForceMode.Impulse);
        player.activeBody.transform.rotation = Quaternion.Euler(0, 180, 0);

        player.controller.CurrentState.Win();

        player.playerManager.WinPlayer();
    }
    
    public void Lose()
    {
        player.controller.CurrentState.Lose();

        player.playerManager.DeadPlayer();
    }

    private void EndCelebration()
    {
        player.controller.CurrentState.TD(false);
    }
}
