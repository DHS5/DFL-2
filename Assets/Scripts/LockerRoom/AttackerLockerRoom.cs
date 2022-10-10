using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AttackerLockerRoom : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject attackerObject;
    [SerializeField] Animator attackerAnimator;
    [SerializeField] SkinnedMeshRenderer attackerRenderer;

    [Header("Content")]
    [SerializeField] private AttackerCardSO attackerCard;


    private void Awake()
    {
        if (attackerCard != null)
            ApplyAttackerInfo(attackerCard);
    }


    public void ApplyAttackerInfo(Mesh _mesh, Avatar _avatar, Vector3 size)
    {
        attackerRenderer.sharedMesh = _mesh;
        attackerAnimator.avatar = _avatar;

        attackerObject.transform.localScale = new Vector3(size.x, size.y, size.z);
    }

    public void ApplyAttackerInfo(AttackerCardSO attackerCard)
    {
        ApplyAttackerInfo(attackerCard.mesh, attackerCard.avatar, attackerCard.attribute.size);
    }

    public void ApplyTeamMaterial(Material mat)
    {
        attackerRenderer.material = mat;
    }
}
