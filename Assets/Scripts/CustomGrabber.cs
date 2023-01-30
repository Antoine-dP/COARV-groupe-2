using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomGrabber : MonoBehaviour
{

    OVRInput.Controller m_controller;
    float grabAxis;
    float triggerAxis;
    bool isGrabbed;
    bool isTriggered;
    Collider grabbedCollider;

    Ray ray;
    RaycastHit hit;

    [SerializeField]
    GameObject RayCylinder;
    [SerializeField]
    Transform RayOrigin;
    [SerializeField]
    Transform RayDir;

    Vector3 fwd;

    public enum Hand
    {
        Left,
        Right
    };

    public Hand hand ;



    //The list of colliders currently inside the trigger
    //var TriggerList : List.<Collider> = new List.<Collider>();

    List<Collider> colliderList = new List<Collider>();

    //called when somet$$anonymous$$ng enters the trigger


    // Start is called before the first frame update
    void Start()
    {
        m_controller = hand == Hand.Left ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;

        grabAxis = 0;
        triggerAxis = 0;
        isGrabbed = false;
        isTriggered = false;

        fwd = Vector3.Normalize(RayDir.position - RayOrigin.position);
    }

    // Update is called once per frame
    void Update()
    {
        //print("Trigger " + hand.ToString() + ": " + OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, m_controller));
        //print("Grip " + hand.ToString() + ": " + OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, m_controller));

        grabAxis = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, m_controller);
        triggerAxis = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, m_controller);

        if (grabAxis > 0.5 && !isGrabbed)
        {
            Grab();
        }
        else if (grabAxis < 0.3 && isGrabbed)
        {
            ReleaseGrab();
        }

        if (triggerAxis > 0.5 && !isTriggered)
        {
            Trigger();
        }
        else if (triggerAxis < 0.3 && isTriggered)
        {
            RealeaseTrigger();
        }
    }

    void Grab()
    {
        if (colliderList.Count>0)
        {
            grabbedCollider = colliderList[0];
            grabbedCollider.GetComponent<CustomGrabbable>().Grab(transform);
            isGrabbed = true;
        }
        
    }
    void ReleaseGrab()
    {
        if(grabbedCollider != null)
        {
            grabbedCollider.GetComponent<CustomGrabbable>().ReleaseGrab();
            grabbedCollider = null;
            isGrabbed = false;
        }
        
    }
    void Trigger()
    {
        isTriggered = true;
        if (grabbedCollider == null)
        {
            fwd = Vector3.Normalize(RayDir.position - RayOrigin.position);
            if(Physics.Raycast(RayOrigin.position, fwd, out hit, 1.0f, (1 << 5)|(1<<6) ))
            {
                GameObject hitUI = hit.collider.gameObject;
                if (hitUI.GetComponent<Button>() != null)
                {
                    hitUI.GetComponent<Button>().onClick.Invoke();
                }
            }
            


        }
    }
    void RealeaseTrigger()
    {
        isTriggered = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        //if the object is not already in the list
        if (!colliderList.Contains(other) && other.gameObject.GetComponent<CustomGrabbable>() != null)
        {
            //add the object to the list
            colliderList.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if the object is in the list
        if (colliderList.Contains(other))
        {
            //remove it from the list
            colliderList.Remove(other);
        }
    }

}
