using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MechMovementScript))]
[RequireComponent(typeof(MechStatus))]

public class JumpJets : MonoBehaviour {
    [Tooltip("The Movement Script attached to this Gameobject. Must have one for this to work.")]
    public MechMovementScript movementScript;
    [Tooltip("The jump force amount used.")]
    public float JumpForce = 10f;
    [Tooltip("Cooldown of jump jets.")]
    public float JumpCooldown = 15f;
    public string playerId = "P1_";

    MechStatus status;

    void Awake()
    {
        playerId = GetComponent<PlayerIndex>().index;
        status = GetComponent<MechStatus>();
    }

    // Update is called once per frame
    void Update () {
        if (status.dead) return;
        if (playerId == "" || playerId == null)
        {
            playerId = GetComponent<PlayerIndex>().index;
            return;
        }
        var input = Input.GetButton(playerId + "Jump");
        if (input)
        {
            movementScript.ActivateJumpJets(JumpForce, JumpCooldown);
        }
    }
}
