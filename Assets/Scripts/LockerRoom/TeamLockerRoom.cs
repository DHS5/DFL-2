using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TeamLockerRoom : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject[] teamObjects;
    [SerializeField] Animator[] teamAnimators;
    [SerializeField] SkinnedMeshRenderer[] teamRenderers;

    [Header("Content")]
    [SerializeField] private AttackerCardSO[] teamCards;


    private void Awake()
    {
        for (int i = 0; i < teamCards.Length; i++)
            if (teamCards[i] != null)
                ApplyAttackerInfo(i, teamCards[i]);
    }


    public void ApplyAttackerInfo(int index, Mesh _mesh, Avatar _avatar, Vector3 size)
    {
        teamRenderers[index].sharedMesh = _mesh;
        teamAnimators[index].avatar = _avatar;

        teamObjects[index].transform.localScale = new Vector3(size.x, size.y, size.z);
    }

    public void ApplyAttackerInfo(int index, AttackerCardSO attackerCard)
    {
        ApplyAttackerInfo(index, attackerCard.mesh, attackerCard.avatar, attackerCard.attribute.size);
    }

    public void ApplyTeamMaterial(Material mat)
    {
        foreach (SkinnedMeshRenderer r in teamRenderers)
        {
            r.material = mat;
        }
    }
}
