using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrabbable : MonoBehaviour
{

    Transform originalParent;
    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        originalParent = transform.parent;
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Grab(Transform newParent)
    {
        rigidbody.isKinematic = false;
        transform.parent = newParent;
    }

    public void ReleaseGrab()
    {
        rigidbody.isKinematic = true;
        transform.parent = originalParent;
    }
}
