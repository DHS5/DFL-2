using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerEffects : MonoBehaviour
{
    [Tooltip("Player script")]
    private Player player;


    [Header("Post Processing game object")]
    [Tooltip("Volume for the bonus post processing")]
    [SerializeField] private Volume accVolume;

    [Tooltip("Volume for the bonus post processing")]
    [SerializeField] private Volume bonusVolume;

    [Header("VFX game objects")]
    [Tooltip("Rain particle system")]
    [SerializeField] private ParticleSystem rainParticleSystem;
    [Tooltip("Rain splash particle system")]
    [SerializeField] private ParticleSystem rainSplashParticleSystem;


    readonly private float minVignette = 0f, maxVignette = 1f;
    readonly private float minDistorsion = 0f, maxDistorsion = 0.3f;
    readonly private float minScale = 1f, maxScale = 1.1f;
    readonly private float minBlur = 0f, maxBlur = 1f;


    private Coroutine sprintCR;
    private Coroutine restCR;


    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        InitSprintVolume();
    }


    // ### Functions ###

    // # Post Processing #
    private void InitSprintVolume()
    {
        accVolume.profile.TryGet(out Vignette vignette);
        accVolume.profile.TryGet(out LensDistortion distorsion);
        accVolume.profile.TryGet(out MotionBlur blur);

        vignette.intensity.value = minVignette;
        distorsion.intensity.value = minDistorsion;
        distorsion.scale.value = minScale;
        blur.intensity.value = minBlur;
    }

    public void SprintVolume(bool state, float time)
    {
        accVolume.gameObject.SetActive(true);

        accVolume.profile.TryGet(out Vignette vignette);
        accVolume.profile.TryGet(out LensDistortion distorsion);
        accVolume.profile.TryGet(out MotionBlur blur);

        if (state)
        {
            if (restCR != null)
                StopCoroutine(restCR);
            sprintCR = StartCoroutine(SprintVolumeCR(vignette, distorsion, blur, time));
        }
        else
        {
            if (sprintCR != null)
                StopCoroutine(sprintCR);
            restCR = StartCoroutine(RestVolumeCR(vignette, distorsion, blur, time));
        }
    }

    private IEnumerator SprintVolumeCR(Vignette vignette, LensDistortion distorsion, MotionBlur blur, float sprintTime)
    {
        float currentVignette = vignette.intensity.value;
        float currentDistorsion = distorsion.intensity.value;
        float currentScale = distorsion.scale.value;
        float currentBlur = blur.intensity.value;


        for (int i = 0; i < 100; i++)
        {
            vignette.intensity.value += (maxVignette - currentVignette) / 100;
            distorsion.intensity.value += (maxDistorsion - currentDistorsion) / 100;
            distorsion.scale.value += (maxScale - currentScale) / 100;
            blur.intensity.value += (maxBlur - currentBlur) / 100;
            yield return new WaitForSeconds(sprintTime / 200);
        }
    }

    private IEnumerator RestVolumeCR(Vignette vignette, LensDistortion distorsion, MotionBlur blur, float restTime)
    {
        float currentVignette = vignette.intensity.value;
        float currentDistorsion = distorsion.intensity.value;
        float currentScale = distorsion.scale.value;
        float currentBlur = blur.intensity.value;


        for (int i = 0; i < 100; i++)
        {
            vignette.intensity.value -= (currentVignette - minVignette) / 100;
            distorsion.intensity.value -= (currentDistorsion - minDistorsion) / 100;
            distorsion.scale.value -= (currentScale - minScale) / 100;
            blur.intensity.value -= (currentBlur - minBlur) / 100;
            yield return new WaitForSeconds(restTime / 400);
        }

        accVolume.gameObject.SetActive(false);
    }



    public void BonusVolume(Color color, float time)
    {
        bonusVolume.gameObject.SetActive(true);

        bonusVolume.profile.TryGet(out Vignette vignette);
        vignette.color.value = color;
        StartCoroutine(BonusVolumeCR(time));
    }

    private IEnumerator BonusVolumeCR(float time)
    {
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(time / 100);
            bonusVolume.weight -= 0.004f;
        }

        bonusVolume.gameObject.SetActive(false);
        bonusVolume.weight = 1;
    }

    // # VFX #

    public void Rain(bool state, float particleAddition)
    {
        var emission = rainParticleSystem.emission;
        float emi = emission.rateOverTime.constant;
        emission.rateOverTime = emi + particleAddition;

        rainParticleSystem.gameObject.SetActive(state);
        rainSplashParticleSystem.gameObject.SetActive(state);
    }
}
