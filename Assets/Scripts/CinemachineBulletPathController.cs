using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineBulletPathController : CinemachinePathController
{
    [SerializeField] LayerMask mask;

    public override bool CheckIfPathISClear(Transform target, float distance, Quaternion orientation)
    {
        if (Physics.BoxCast(target.TransformPoint(boxCollider.center),
            boxCollider.size / 2f, target.forward, out RaycastHit hit,
            orientation, distance, ~mask))
        {
            // Abaikan terrain jika kena
            if (hit.collider.GetComponent<Terrain>() != null ||
                hit.collider.gameObject.name == "Static" ||
                hit.collider.gameObject.name == "Pipes_set_v1_H_set_v1")
            {
                return true; // Abaikan Terrain, Static, dan Pipes_set_v1_H_set_v1
            }
            Debug.LogError(hit.collider.gameObject.name);
            return false; // Ada penghalang lain
        }

        return true; // Tidak ada penghalang
    }
}
