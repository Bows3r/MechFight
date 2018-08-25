using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(MechStatus))]
public class MechMovementScript : MonoBehaviour
{
    public string playerId = "P1_";

    public float lastFrameSpeed;
    public float maxSpeed = 5f;
    public float reverseSpeedMultiplier = 0.6f;

    public float acceleration = 3f;

    private float currentTurnSpeed;
    public float turnAccel = 60f;
    public float turnSpeed = 60f;

    public float maxAnimSpeed = 1f;
    public float animSpeedMultiplier = 1.1f;

    private CharacterController controller;
    public Animator anim;
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;

    bool grounded = true;
    float originalGroundTimer;
    float groundTimer = 0.3f;
    bool isGrounded;
    float rcRange;

    bool walking;
    [HideInInspector]
    public float verticalInput;
    [HideInInspector]
    public float horizontalInput;
    // Use this for initialization
    [SerializeField]
    string currentAnim;
    public string queuedAnim { get; private set; }

    //JUMPJETS INFO
    bool jumpQued = false;
    bool canJump = true;
    float jumpForce = 0f;
    float jumpCooldown = 0f;
    float jumpJetHeat = 20f;
    //JUMPJETS INFO END HERE

    MechStatus status;

    //AI?
    [HideInInspector]
    public bool aiControl = false;

    public LayerMask mask;
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerId = GetComponent<PlayerIndex>().index;
        originalGroundTimer = groundTimer;
        rcRange = controller.height * 0.55f;
        status = GetComponent<MechStatus>();
    }

    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        if (status.dead) {
            anim.Play("Idle");
            return;
        }
        if (playerId == "" || playerId == null)
        {
            playerId = GetComponent<PlayerIndex>().index;
            return;
        }
        if (!aiControl)
        {
            verticalInput = Input.GetAxis(playerId + "Vertical");
            horizontalInput = Input.GetAxis(playerId + "Horizontal");
        }
        var desiredSpeed = SpeedCalcs(Time.deltaTime);
        if (grounded)
        {
            lastFrameSpeed = desiredSpeed;
        }
        else lastFrameSpeed = 0f;

        if (controller.isGrounded)
        {
            if (!grounded)
            {
                grounded = true;
                MasterAudioSummoner.instance.PlayAudio(MasterAudioSummoner.instance.mechAudioFiles, "Mech_Footstep", 0.1f, 0.1f, transform.position);
            }
            moveDirection = transform.forward * desiredSpeed;
        }
        float turn = TurnCalcs(Time.deltaTime);
        transform.Rotate(0, turn * Time.deltaTime, 0);
        currentTurnSpeed = turn;
        HandleAnimation(Time.deltaTime);
        /*
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, controller.radius, Vector3.down, out hitInfo,
                           controller.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        moveDirection += Vector3.ProjectOnPlane(moveDirection, hitInfo.normal).normalized;
        */
        if (jumpQued)
        {
            JumpJets(jumpForce);

        }
        moveDirection.y -= MapSettings.Instance.gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        /*
        moveDirection.y -= MapSettings.Instance.gravity * Time.deltaTime;
        controller.Move(new Vector3(0, moveDirection.y, 0) * Time.deltaTime);
        */
    }

    float TurnCalcs(float dTime)
    {
        float f = currentTurnSpeed;
        f = Mathf.MoveTowards(f, turnSpeed * horizontalInput, turnAccel * dTime);
        return f;
    }

    float SpeedCalcs(float dTime)
    {
        var reverseMod = verticalInput < 0 ? reverseSpeedMultiplier : 1f;
        var targetSpeed = verticalInput * maxSpeed * reverseMod;
        var desiredSpeed = Mathf.MoveTowards(lastFrameSpeed, targetSpeed, acceleration * dTime);
        return desiredSpeed;
    }

    float AnimSpeedCalcs()
    {
        //-3 / 5 = -0.6
        //5 / 5 = 1
        var animSpeed = lastFrameSpeed / maxSpeed;
        animSpeed += queuedAnim == "Walk_Cycle_Reverse" ? -Mathf.Abs(currentTurnSpeed) / turnSpeed / 5f : Mathf.Abs(currentTurnSpeed) / turnSpeed / 5f;
        animSpeed = Mathf.Clamp(animSpeed, -maxAnimSpeed * reverseSpeedMultiplier, maxAnimSpeed);
        animSpeed *= animSpeedMultiplier;
        return animSpeed;
    }

    void HandleAnimation(float dTime)
    {
        float animSpeed = AnimSpeedCalcs();
        if ((animSpeed == 0f && currentAnim != "Idle") || !grounded)
        {
            walking = false;
            //anim.CrossFade("Idle", 0.6f);
            queuedAnim = "Idle";
        }
        else if (animSpeed != 0f && animSpeed > 0f && currentAnim != "Walk_Cycle" && grounded)
        {
            walking = true;
            //anim.CrossFade("Walk_Cycle", 0.1f);
            queuedAnim = "Walk_Cycle";
        }
        else if (animSpeed != 0f && animSpeed < 0f && currentAnim != "Walk_Cycle_Reverse" && grounded)
        {
            walking = true;
            //anim.CrossFade("Walk_Cycle_Reverse", 0.1f);
            queuedAnim = "Walk_Cycle_Reverse";
        }
        animSpeed = AnimSpeedCalcs();
        if (!controller.isGrounded)
        {
            HandleWhenNotOnGround(dTime);
        }
        if (grounded)
        {
            //var f = !walking ? 1f : Mathf.Abs(animSpeed);
            anim.speed = !walking ? 1f : Mathf.Abs(animSpeed);
        }
        //print("grounded is " + grounded);
        if (currentAnim != queuedAnim)
        {
            print(queuedAnim);
            if (!grounded)
            {
                anim.CrossFade("Idle", 0.1f);
            }
             else anim.CrossFade(queuedAnim, queuedAnim == "Idle" ? 0.6f : 0.1f);
            currentAnim = queuedAnim;
        }
    }

    
    
    void HandleWhenNotOnGround(float dTime)
    {
        grounded = CheckIfGrounded();
        walking = grounded;
        if (!grounded)
        {
            queuedAnim = "Idle";
        }
    }

    bool CheckIfGrounded()
    {
        bool b = false;
        //if (!grounded) return false;
        Vector3 pos = transform.position + controller.center;
        //pos.y -= controller.height / 2.2f;
        
        RaycastHit hit;
        if (Physics.SphereCast(pos,
            controller.radius * (1f + Mathf.Epsilon),
            Vector3.down, out hit, rcRange, ~mask))
        {
            b = true;
        }
        return b;
    }

    void JumpJets(float force)
    {
        canJump = false;
        jumpQued = false;
        status.currentHeat += jumpJetHeat;
        moveDirection.y = force;
        StartCoroutine(JumpJetCooldown(jumpCooldown));
        if (MasterAudioSummoner.instance != null)
        {
            MasterAudioSummoner.instance.PlayAudio(MasterAudioSummoner.instance.mechAudioFiles, "JumpJetLight", 0.3f, 0.1f, transform.position);
        }
    }

    public void ActivateJumpJets(float force, float cooldown)
    {
        if (canJump == false) return;
        jumpCooldown = cooldown;
        jumpForce = force;
        jumpQued = true;
    }
    IEnumerator JumpJetCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canJump = true;
    }
}
