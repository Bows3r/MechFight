using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(ComponentHealth))]
public class PeasantBehaviour : MonoBehaviour {
    CharacterController controller;
    public float walkSpeed = 0.7f;
    public float runSpeed = 1.4f;
    public float currentSpeed = 0.7f;
    public float startRunDistance = 20f;
    Transform offset;

    ComponentHealth health;

    Vector3 moveDirection;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        offset = transform.GetChild(0);
        health = GetComponent<ComponentHealth>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;


        ToggleRun();
        CheckKill();
        if (Physics.Raycast(offset.position, offset.forward, out hit, currentSpeed * Time.deltaTime * 1.1f))
        {
            transform.Rotate(0, Random.Range(0, 360), 0);
        }
        if (!Physics.Raycast(offset.position, -offset.up, out hit, 1f))
        {
            transform.Rotate(0, Random.Range(0, 360), 0);
        }
        moveDirection = transform.forward * currentSpeed;
        moveDirection.y -= MapSettings.Instance.gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

    }

    void ToggleRun()
    {
        currentSpeed = walkSpeed;
        for (int i = 0; i < MapSettings.Instance.playerMechs.Count; i++)
        {
            if (Vector3.Distance(MapSettings.Instance.playerMechs[i].transform.position, transform.position) < startRunDistance)
            {
                currentSpeed = runSpeed;
                break;
            }
        }
    }

    void CheckKill()
    {
        for (int i = 0; i < MapSettings.Instance.playerMechs.Count; i++)
        {
            if (Vector3.Distance(MapSettings.Instance.playerMechs[i].transform.position, transform.position) < 2.5f)
            {
                health.ReduceHealth(5000f, false);
            }
        }
    }
    
    
}
