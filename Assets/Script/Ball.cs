using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public Vector3 Position => rb.position;
    public bool IsMoving => rb.velocity != Vector3.zero;
    public bool IsTeleporting => isTeleporting;

    Vector3 lastPosition;

    bool isTeleporting;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        lastPosition = this.transform.position;
    }

    internal void Addforce(Vector3 force)
    {
        rb.isKinematic = false;
        lastPosition = this.transform.position;
        rb.AddForce(force, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        if (rb.velocity != Vector3.zero &&
            rb.velocity.magnitude < 0.5f)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            rb.rotation = Quaternion.Euler(Vector3.zero);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Out"))
        {
            StopAllCoroutines();
            StartCoroutine(DelayedTeleport());
        }
    }

    IEnumerator DelayedTeleport()
    {
        isTeleporting = true;
        yield return new WaitForSeconds(3);
        rb.isKinematic = false;
        this.transform.position = lastPosition;
        isTeleporting = false;
    }
}
