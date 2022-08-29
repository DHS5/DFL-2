using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpBar : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] private Image background;
    [SerializeField] private Image fullImage;


    [HideInInspector] public float currentValue;
    [HideInInspector] public float fullValue;

    [HideInInspector] public float rechargeTime;


    private bool full;


    // ### Properties ###

    private float ChargeBySec { get { return fullValue / rechargeTime; } }
    private float CurrentPourcent { get { return currentValue / fullValue; } }


    // ### Base functions ###

    private void Start()
    {
        Player player = FindObjectOfType<Player>();

        fullValue = player.controller.playerAtt.JumpStamina;
        currentValue = fullValue;

        rechargeTime = player.controller.playerAtt.JumpRechargeTime;

        full = true;
    }

    private void Update()
    {
        if (!full) Recharge();
    }


    // ### Functions ###

    public void Jump(float cost)
    {
        full = false;
        currentValue -= cost;
    }

    private void Recharge()
    {
        currentValue += Time.deltaTime * ChargeBySec;

        if (currentValue >= fullValue)
        {
            full = true;
            currentValue = fullValue;
        }

        ActuBar();
    }

    private void ActuBar()
    {
        Animation fillImageAnim = fullImage.GetComponent<Animation>();
        fillImageAnim.Stop();

        float size = background.GetComponent<RectTransform>().rect.height;

        float time = rechargeTime * CurrentPourcent;

        float tangent = - size * (1 - CurrentPourcent) / time;

        Keyframe[] keys = new Keyframe[2];
        keys[0] = new Keyframe(0.0f, - (1 - CurrentPourcent) * size / 2);
        keys[1] = new Keyframe(time, 0.0f);
        keys[0].outTangent = tangent / 2;
        keys[1].inTangent = tangent / 2;

        AnimationCurve curve = new(keys);
        fillImageAnim.clip.SetCurve("", typeof(RectTransform), "m_AnchoredPosition.y", curve);


        keys[0].value = - (1 - CurrentPourcent) * size;
        keys[0].outTangent = tangent;
        keys[1].inTangent = tangent;

        curve = new AnimationCurve(keys);
        fillImageAnim.clip.SetCurve("", typeof(RectTransform), "m_SizeDelta.y", curve);


        fillImageAnim.Play();
    }
}
