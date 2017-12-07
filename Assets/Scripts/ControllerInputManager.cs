using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputManager : MonoBehaviour
{
    /*public SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device device;
    */
    private OVRInput.Controller controller;
    public bool leftHand;

    //Teleporter
    private LineRenderer laser;
    public GameObject teleportAimerObject;
    public Vector3 teleportLocation;
    public GameObject player;
    private Vector3 startingLocation;
    public LayerMask laserMask;
    public float yNudgeAmount = 1f; //specific to teleportAimerObject height
    public int teleportLength = 1;
    public float throwForce = 1.5f;
	public GameObject gamePlatform;


    // Use this for initialization
    void Start()
    {
        if (leftHand)
            controller = OVRInput.Controller.LTouch;
        else
            controller = OVRInput.Controller.RTouch;
        laser = GetComponentInChildren<LineRenderer>();
        startingLocation = player.transform.position;
        Debug.Log("Starting Location: " + startingLocation);

    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, controller))
        {
            //Debug.Log("Index trigger of controller pressed");
            laser.gameObject.SetActive(true);
            teleportAimerObject.SetActive(true);
            // Keep the camera height consistent
            Vector3 forwardNoY = new Vector3(transform.forward.x, 0, transform.forward.z);

            laser.SetPosition(0, gameObject.transform.position + new Vector3(0, 0, -.2f));
            RaycastHit hit;
            if (Physics.Raycast(transform.position, forwardNoY, out hit, teleportLength, laserMask))
            {
                teleportLocation = hit.point;
				teleportLocation = new Vector3 (teleportLocation.x, startingLocation.y, teleportLocation.z);
                laser.SetPosition(1, teleportLocation);
                //aimer position
                teleportAimerObject.transform.position = new Vector3(teleportLocation.x, startingLocation.y-1, teleportLocation.z);
            }
            else
            {
                //Debug.Log("Transform forward: " + forwardNoY);
                teleportLocation = forwardNoY * teleportLength + transform.position;
                RaycastHit groundRay;
                if (Physics.Raycast(teleportLocation, -Vector3.up, out groundRay, 17, laserMask))
                {
                    Debug.Log("Ground Ray");
                    teleportLocation = groundRay.point;
					teleportLocation = new Vector3 (teleportLocation.x, startingLocation.y, teleportLocation.z);
                }
                laser.SetPosition(1, forwardNoY * teleportLength + transform.position);
                //Debug.Log("Laser Position: " + laser.GetPosition(1));
                //aimer position
                teleportAimerObject.transform.position = teleportLocation + new Vector3(0, 0, 0);

            }
        }
        teleportLocation = new Vector3(teleportLocation.x, startingLocation.y, teleportLocation.z);

        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, controller))
        {
            Debug.Log("Index trigger of controller up");
            laser.gameObject.SetActive(false);
            teleportAimerObject.SetActive(false);
            Debug.Log("Camera Location" + player.transform.position);
            Debug.Log("Teleport Location" + teleportLocation);
            player.transform.position = teleportLocation;
        }
        //Debug.Log("Teleport Location" + teleportLocation);

        if (OVRInput.GetDown(OVRInput.Button.One, controller))
        {
            Debug.Log("X Button Pressed!");
            player.transform.position = startingLocation;
        }


    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Throwable"))
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger,
                controller) < 0.1f)
            {
                ThrowObject(col);
            }
            else if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger,
                controller) > 0.1f)
            {
                GrabObject(col);
            }
        }
    }
    void GrabObject(Collider coli)
    {
        coli.transform.SetParent(gameObject.transform);
        coli.GetComponent<Rigidbody>().isKinematic = true;
        //device.TriggerHapticPulse(2000);
        Debug.Log("You are touching down the trigger on an object");
    }
    void ThrowObject(Collider coli)
    {
        coli.transform.SetParent(null);
        Rigidbody rigidBody = coli.GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;
        rigidBody.velocity =
            OVRInput.GetLocalControllerVelocity(controller) * throwForce;
        rigidBody.angularVelocity =
            OVRInput.GetLocalControllerAngularVelocity(controller);
        Debug.Log("You have released the trigger");
    }
}
