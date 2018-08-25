using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(MechGunSystem))]
[RequireComponent(typeof(MechMovementScript))]
[RequireComponent(typeof(MechTorsoTwistScript))]
[RequireComponent(typeof(MechStatus))]
[RequireComponent(typeof(CharacterController))]

public class MechAIScript : MonoBehaviour {

    MechGunSystem weapons;
    MechMovementScript movement;
    MechTorsoTwistScript torso;
    MechStatus status;
    JumpJets jumpJets;
    CharacterController controller;

    float biasMovement = 5f;
    float biasTurn = 5f;

    public float legsHz;
    public float legsVrt;
    public float torsoHz;
    public float torsoVrt;
    public float gun1;
    public float gun2;
    public float gun3;
    public float gun4;
    public float gun5;
    public float jets;

    float raycastHeight;

    public Transform target;

    float deltaTime;
    float targetFloat = 1f;

    //ingore layers
    //8
    //9
    //10
    public LayerMask mask;

    Vector3 destinationPos;
    float leashRange = 1f;
    float timerForNewDestination = 3f;
    NavMeshPath path;
    bool canFetchPatch = true;

    float delta = 1f;

    // Use this for initialization
    void Awake()
    {
        weapons = GetComponent<MechGunSystem>();
        movement = GetComponent<MechMovementScript>();
        torso = GetComponent<MechTorsoTwistScript>();
        status = GetComponent<MechStatus>();
        jumpJets = GetComponent<JumpJets>();
        controller = GetComponent<CharacterController>();
        raycastHeight = controller.height / 2f;
        path = new NavMeshPath();
    }

    void Start() {
    }

    void Forward()
    {
        targetFloat = 1f;
    }

    void Reverse()
    {
        targetFloat = -1f;
    }

    // Update is called once per frame
    void Update() {
        if (target == null)
        {
            ZeroTorsoValues();
            ZeroMovementValues();
            return;
        }
        
        deltaTime = Time.deltaTime;

        CalculatePath();
        CalculateMovementValues();
        //before applying values, calculate here
        //do shits
        CalcTurn();
        MaxValues();
        ApplyValues();

    }

    void CalcTurn()
    {
        var heading = destinationPos - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        float f = AngleDir(transform.forward, direction, transform.up);
        legsHz = f;
    }
    public float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);
        if (dir < 0f) dir -= 0.02f;
        if (dir > 0f) dir += 0.02f;
        return dir;
        /*
        if (dir > 0.0f)
        {
            return 1.0f;
        }
        else if (dir < 0.0f)
        {
            return -1.0f;
        }
        else
        {
            return 0.0f;
        }
        */
    }
    IEnumerator GetNavPathCooldown(float f)
    {
        canFetchPatch = false;
        yield return new WaitForSeconds(f);
        canFetchPatch = true;
    }

    void CalculateMovementValues()
    {
        if (path.corners.Length == 0)
        {
            ZeroMovementValues();
            return;
        }
        if (path.corners[1] == null)
        {
            CalculatePath();
            return;
        }
        destinationPos = path.corners[1];
        //transform.position = path.corners[1];

    }
    void CalculatePath()
    {
        if (!canFetchPatch)
        {
            return;
        }
        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        StartCoroutine(GetNavPathCooldown(timerForNewDestination));
        return;
    }

    void MaxValues()
    {
        legsHz = Mathf.Clamp(legsHz, -1, 1);
        legsVrt = Mathf.Clamp(legsVrt, -1, 1);
        torsoHz = Mathf.Clamp(torsoHz, -1, 1);
        torsoVrt = Mathf.Clamp(torsoVrt, -1, 1);
        gun1 = Mathf.Clamp(gun1, -1, 1);
        gun2 = Mathf.Clamp(gun2, -1, 1);
        gun3 = Mathf.Clamp(gun3, -1, 1);
        gun4 = Mathf.Clamp(gun4, -1, 1);
        gun5 = Mathf.Clamp(gun5, -1, 1);
        jets = Mathf.Clamp(jets, -1, 1);
    }

    void ApplyValues()
    {
        if (movement != null)
        {
            movement.aiControl = true;
            movement.horizontalInput = legsHz;
            movement.verticalInput = legsVrt;
        }
        if (torso != null)
        {
            torso.aiControl = true;
            torso.horizontalInput = torsoHz;
            torso.verticalInput = torsoVrt;
        }
        if (weapons != null)
        {
            //TODO
        }
    }

    void ZeroMovementValues()
    {
        legsHz = Mathf.Clamp(legsHz, 0, 0);
        legsVrt = Mathf.Clamp(legsVrt, 0, 0);
    }

    void ZeroTorsoValues()
    {
        torsoHz = Mathf.Clamp(torsoHz, 0, 0);
        torsoVrt = Mathf.Clamp(torsoVrt, 0, 0);
    }


}
