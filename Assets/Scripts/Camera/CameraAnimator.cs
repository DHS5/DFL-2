using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Manages the animations of the camera
/// </summary>
public class CameraAnimator : MonoBehaviour
{
    
    Animation cameraAnimation;

    [Header("Animations")]
    [Tooltip("Animation Run")]
    [SerializeField] AnimationClip runAnim;
    [Tooltip("")]
    [SerializeField] AnimationClip sprintAnim;
    

    [Header("Post Processing")]
    [Tooltip("Post-Processing effect of the acceleration")]
    [SerializeField] private GameObject cameraAccPostProcess;
    private Volume accPPVolume;


    private bool isDefault = true;
    private bool isSprinting = false;


    /// <summary>
    /// Plays the default run animation
    /// </summary>
    public void DefaultAnim()
    {
        if (!isDefault)
        {
            isSprinting = false;
            isDefault = true;
            cameraAnimation.clip = runAnim;
            cameraAnimation.Play();
        }
    }

    

    /// <summary>
    /// Plays the sprint animation
    /// </summary>
    public void Sprint()
    {
        accPPVolume.weight = Mathf.Lerp(cameraAccPostProcess.GetComponent<Volume>().weight, 1, 0.005f);
        if (!isSprinting)
        {
            isDefault = false;
            isSprinting = true;
            cameraAnimation.clip = sprintAnim;
            cameraAnimation.Play();
        }
    }

    /// <summary>
    /// Ends the sprint animation
    /// </summary>
    public void EndSprint()
    {
        if (accPPVolume.weight > 0.01f)
            accPPVolume.weight = Mathf.Lerp(cameraAccPostProcess.GetComponent<Volume>().weight, 0, 0.01f);

        if (isSprinting)
        {
            isSprinting = false;
            DefaultAnim();
        }
    }

    

    /// <summary>
    /// Gets the animation and the post processing volume
    /// </summary>
    private void Start()
    {
        cameraAnimation = gameObject.GetComponent<Animation>();
        accPPVolume = cameraAccPostProcess.GetComponent<Volume>();
    }

}
