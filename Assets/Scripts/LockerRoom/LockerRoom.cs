using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerPauses 
{
    SIMPLE, BICEPSFLEX, TANK, FER, GTL, BICEP, HOLD, HOLDNBFLEX, BIGFLEX, KING, YOUCDIS, ARCHER, GRIDDY, TIME, STUCK, QUADS, OPENARM, NUMBER, GUN,
    FIST, FINGERS, EATAW, ARMCROSS, LITTLE
}


public class LockerRoom : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject playerObject;
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


    public void ApplyPlayerInfo(Mesh _mesh, Avatar _avatar, Material[] materials, PlayerPauses pause, bool ball, Vector3 size)
    {
        if (trigger != "") playerAnimator.ResetTrigger(trigger);

        playerRenderer.sharedMesh = _mesh;
        playerRenderer.materials = materials;
        playerAnimator.avatar = _avatar;
        trigger = pause.ToString();
        playerAnimator.SetTrigger(trigger);
        footballGameObject.SetActive(ball);

        playerObject.transform.localScale = new Vector3(size.x, size.y, size.z);
    }

    public void ApplyPlayerInfo(PlayerCardSO playerCard)
    {
        ApplyPlayerInfo(playerCard.playerInfo.mesh, playerCard.playerInfo.avatar, playerCard.playerInfo.materials, playerCard.playerPause, playerCard.footballActive, playerCard.playerInfo.attributes.size);
    }
}
