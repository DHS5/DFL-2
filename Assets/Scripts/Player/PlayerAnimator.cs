using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animation playerAnimation;

    [Header("Animations")]
    [Tooltip("")]
    [SerializeField] AnimationClip jukeRightAnim;
    [Tooltip("")]
    [SerializeField] AnimationClip jukeLeftAnim;
    [Tooltip("")]
    [SerializeField] AnimationClip spinRightAnim;
    [Tooltip("")]
    [SerializeField] AnimationClip spinLeftAnim;

    [HideInInspector] public bool isJuking = false;
    [HideInInspector] public float jukeSpeed = 0;
    [HideInInspector] public bool isSpining = false;
    private bool isDefault = true;


    /// <summary>
    /// Puts the animator to 
    /// </summary>
    public void DefaultAnim()
    {
        if (!isDefault && !isSpining)
        {
            isJuking = false;
            isSpining = false;
            isDefault = true;
            jukeSpeed = 0f;
            playerAnimation.Stop();
        }
    }


    /// <summary>
    /// Plays the juke animation on the side given by dir
    /// </summary>
    /// <param name="dir">Direction of the juke (-1 : left / 1 : right)</param>
    public void Juke(float dir)
    {
        if (!isJuking && jukeSpeed * dir <= 0 && !isSpining)
        {
            jukeSpeed = dir;
            isJuking = true;
            isDefault = false;

            playerAnimation.clip = (dir > 0 ? jukeRightAnim : jukeLeftAnim);
            playerAnimation.Play();

            float animTime = playerAnimation.clip.length;
            Invoke(nameof(DefaultAnim), animTime);
        }
    }


    public void Spin(float dir)
    {
        if (!isSpining)
        {
            isSpining = true;
            isDefault = false;

            playerAnimation.clip = (dir > 0 ? spinRightAnim : spinLeftAnim);
            playerAnimation.Play();

            float animTime = playerAnimation.clip.length;
            Invoke(nameof(EndSpin), animTime);
        }
    }
    private void EndSpin()
    {
        isSpining = false;
        DefaultAnim();
    }


    /// <summary>
    /// Gets the animation
    /// </summary>
    private void Start()
    {
        playerAnimation = gameObject.GetComponent<Animation>();
    }
}
