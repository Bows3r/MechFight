using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MechStatus))]
public class MechTorsoTwistScript : MonoBehaviour {

    public GameObject torsoObjectX;
    public GameObject torsoObjectY;

    public string playerId;

    public float twistSpeed = 45f;
    public float xLimit = 80f;
    public float yLimit = 20f;

    float rotationX;
    float rotationY;

    public Vector3 yVector = new Vector3(1, 0, 0);
    public Vector3 xVector = new Vector3(0, 0, 1);


    [HideInInspector]
    public float verticalInput;
    [HideInInspector]
    public float horizontalInput;

    public AudioSource twistSound;

    MechStatus status;

    [HideInInspector]
    public bool aiControl = false;

    // Use this for initialization
    void Awake () {
        playerId = GetComponent<PlayerIndex>().index;
        status = GetComponent<MechStatus>();
    }

    void Start()
    {

    }
	
	// Update is called once per frame
	void Update () {
        if (status.dead) return;
        if (playerId == "" || playerId == null)
        {
            return;
        }

        if (!aiControl)
        {
            verticalInput = Input.GetAxis(playerId + "TwistY");
            horizontalInput = Input.GetAxis(playerId + "TwistX");
        }
        LockedRotation(Time.deltaTime);
        if (twistSound == null)
        {
            return;
        }
        if (verticalInput != 0f || horizontalInput != 0f)
        {
            twistSound.Play();
        }
        else twistSound.Stop();
	}

    void LockedRotation(float dTime)
    {

        CalcRotations(dTime);
        torsoObjectY.transform.localEulerAngles = yVector * rotationY;
        torsoObjectX.transform.localEulerAngles = xVector * rotationX;
    }

    void CalcRotations(float dTime)
    {
        rotationX += horizontalInput * twistSpeed * dTime;
        rotationX = Mathf.Clamp(rotationX, -xLimit, xLimit);

        rotationY += verticalInput * twistSpeed * dTime;
        rotationY = Mathf.Clamp(rotationY, -yLimit, yLimit);
    }
}
