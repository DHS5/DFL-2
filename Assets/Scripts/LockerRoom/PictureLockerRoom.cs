using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PictureLockerRoom : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject[] playerObject;
    [SerializeField] Animator[] playerAnimator;
    [SerializeField] SkinnedMeshRenderer[] playerRenderer;
    [SerializeField] GameObject[] footballGameObject;

    [Header("Content")]
    [SerializeField] private PlayerCardSO[] playerCards;


    private string trigger = "";


    private void Awake()
    {
        for (int i = 0; i < playerCards.Length; i++)
            ApplyPlayerInfo(playerCards[i], i);
    }


    public void ApplyPlayerInfo(int i, Mesh _mesh, Avatar _avatar, Material[] materials, PlayerPauses pause, bool ball, Vector3 size)
    {
        if (trigger != "") playerAnimator[i].ResetTrigger(trigger);

        playerRenderer[i].sharedMesh = _mesh;
        playerRenderer[i].materials = materials;
        playerAnimator[i].avatar = _avatar;
        trigger = pause.ToString();
        playerAnimator[i].SetTrigger(trigger);
        footballGameObject[i].SetActive(ball);

        playerObject[i].transform.localScale = new Vector3(size.x, size.y, size.z);
    }

    public void ApplyPlayerInfo(PlayerCardSO playerCard, int i)
    {
        ApplyPlayerInfo(i, playerCard.playerInfo.mesh, playerCard.playerInfo.avatar, playerCard.playerInfo.materials, playerCard.playerPause, playerCard.footballActive, playerCard.playerInfo.attributes.size);
    }
}
