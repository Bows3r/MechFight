using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MechStatus))]

public class MechGunSystem : MonoBehaviour {
    public string playerId = "";

    GameObject owner;
    
    public List<MechWeapon> GroupOneWeapons;
    public List<MechWeapon> GroupTwoWeapons;
    public List<MechWeapon> GroupThreeWeapons;
    public List<MechWeapon> GroupFourWeapons;
    public List<MechWeapon> GroupFiveWeapons;

    MechStatus status;
    void Awake()
    {
        playerId = GetComponent<PlayerIndex>().index;
        if (owner == null)
        {
            owner = this.gameObject;
        }
        status = GetComponent<MechStatus>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (status.dead) return;
        if (playerId == "" || playerId == null)
        {
            playerId = GetComponent<PlayerIndex>().index;
            return;
        }
        if (Input.GetButton(playerId + "Fire1"))
        {
            ShootWeaponGroup(GroupOneWeapons);
        }
        if (Input.GetButton(playerId + "Fire2"))
        {
            ShootWeaponGroup(GroupTwoWeapons);
        }
        if (Input.GetButton(playerId + "Fire3"))
        {
            ShootWeaponGroup(GroupThreeWeapons);
        }
        if (Input.GetButton(playerId + "Fire4"))
        {
            ShootWeaponGroup(GroupFourWeapons);
        }
        if (Input.GetButton(playerId + "Fire5"))
        {
            ShootWeaponGroup(GroupFiveWeapons);
        }

    }

    void ShootWeaponGroup(List<MechWeapon> group)
    {
        for (int i = 0; i < group.Count; i++)
        {
            if (group[i] != null)
            group[i].StartShoot(owner);
        }
    }
}
