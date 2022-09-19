using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerPauses { SIMPLE, BICEPSFLEX, PECFLEX, FER, GTL, SHOWEM, HOLD, HOLDNBFLEX, DOUBLEFLEX, KING }


public class LockerRoom : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator playerAnimator;
    [SerializeField] SkinnedMeshRenderer playerRenderer;

    [Header("Content")]
    [SerializeField] private PlayerCardSO playerCard;


    private string trigger = "";


    private void Awake()
    {
        if (playerCard != null)
            ApplyPlayerInfo(playerCard);
    }


    public void ApplyPlayerInfo(Mesh _mesh, Avatar _avatar, Material _numberMat, PlayerPauses pause)
    {
        if (trigger != "") playerAnimator.ResetTrigger(trigger);

        playerRenderer.sharedMesh = _mesh;
        playerRenderer.materials = new Material[] { playerRenderer.materials[0], _numberMat };
        playerAnimator.avatar = _avatar;
        trigger = pause.ToString();
        playerAnimator.SetTrigger(trigger);
    }

    public void ApplyPlayerInfo(PlayerCardSO playerCard)
    {
        ApplyPlayerInfo(playerCard.playerMesh, playerCard.playerAvatar, playerCard.numberMaterial, playerCard.playerPause);
    }
}
