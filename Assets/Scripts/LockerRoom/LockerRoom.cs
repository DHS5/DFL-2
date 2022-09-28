using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerPauses { SIMPLE, BICEPSFLEX, PECFLEX, FER, GTL, SHOWEM, HOLD, HOLDNBFLEX, DOUBLEFLEX, KING }


public class LockerRoom : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator playerAnimator;
    [SerializeField] SkinnedMeshRenderer playerRenderer;
    [SerializeField] GameObject footballGameObject;

    [Header("Content")]
    [SerializeField] private PlayerCardSO playerCard;


    private string trigger = "";


    private void Awake()
    {
        if (playerCard != null)
            ApplyPlayerInfo(playerCard);
    }


    public void ApplyPlayerInfo(Mesh _mesh, Avatar _avatar, Material[] materials, PlayerPauses pause, bool ball)
    {
        if (trigger != "") playerAnimator.ResetTrigger(trigger);

        playerRenderer.sharedMesh = _mesh;
        playerRenderer.materials = materials;
        playerAnimator.avatar = _avatar;
        trigger = pause.ToString();
        playerAnimator.SetTrigger(trigger);
        footballGameObject.SetActive(ball);
    }

    public void ApplyPlayerInfo(PlayerCardSO playerCard)
    {
        ApplyPlayerInfo(playerCard.playerInfo.mesh, playerCard.playerInfo.avatar, playerCard.playerInfo.materials, playerCard.playerPause, playerCard.footballActive);
    }
}
