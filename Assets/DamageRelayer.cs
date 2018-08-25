using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRelayer : MonoBehaviour {
    [Tooltip("List of armor scripts. If more than one, consider it like a torso (left and right sides, but needs bounds values)")]
    public List<ComponentHealth> armorScripts;
    //0 is CT
    //1 is left
    //2 is right

    public Transform centerPos;
    public Transform leftPos;
    public Transform rightPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ReduceHealth(float damage, Vector3 pos)
    {
        if (armorScripts.Count == 1)
        {
            print("Reducing health");
            armorScripts[0].ReduceHealth(damage, false);
        } else
        {
            var trans = ClosestPoint(pos);
            if (trans == centerPos)
            {
                //do stuff
                armorScripts[0].ReduceHealth(damage, false);
            }
            if (trans == leftPos)
            {
                //do stuff
                armorScripts[1].ReduceHealth(damage, false);
            }
            if (trans == rightPos)
            {
                //do stuff
                armorScripts[2].ReduceHealth(damage, false);
            }
        }
    }

    Transform ClosestPoint(Vector3 pos)
    {
        Transform closestPoint = centerPos;
        var float1 = Vector3.Distance(pos, centerPos.position);
        var float2 = Vector3.Distance(pos, leftPos.position);
        var float3 = Vector3.Distance(pos, rightPos.position);
        var finalFloat = Mathf.Min(float1, Mathf.Min(float2, float3));
        if (finalFloat == float1)
        {
            closestPoint = centerPos;
        }
        if (finalFloat == float2)
        {
            closestPoint = leftPos;
        }
        if (finalFloat == float3)
        {
            closestPoint = rightPos;
        }
        return closestPoint;
    }
}
