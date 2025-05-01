using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    private Rigidbody[] _rigidbodies;
    private Rigidbody[] rigidbodies
    {
        get
        {
            if (_rigidbodies == null)
                _rigidbodies = GetComponentsInChildren<Rigidbody>();
            return _rigidbodies;
        }       
    }

    private bool isRagdollEnabled = false;
    public bool IsRagdollEnabled => isRagdollEnabled;
    
    private void Awake()
    {
        DisableRagdoll();
    }
    private void DisableRagdoll()
    {
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.None;

        }
        isRagdollEnabled = false;
    }
    public void EnableRagdoll()
    {
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        isRagdollEnabled = true;
    }

    private void Update()
    {
        DebugRagdoll();
    }

    private void DebugRagdoll()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            EnableRagdoll();
    }
}
