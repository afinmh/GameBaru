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
            if (hit.collider.GetComponent<Terrain>() != null)
            {
                return true; // Path dianggap clear jika hanya terrain yang kena
            }

            Debug.LogError(hit.collider.gameObject.name);
            return false; // Ada penghalang lain
        }

        return true; // Tidak ada penghalang
    }
}
