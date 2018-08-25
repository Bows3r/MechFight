using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatValueGrabber : MonoBehaviour {
    Text text;
    [Tooltip("If true, we read current heat. If false, we read max heat.")]
    public bool currentHeat;
    public MechStatus heatScript;

	// Use this for initialization
    void Awake()
    {
        text = GetComponent<Text>();
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (text == null) return;
        if (heatScript == null) return;
        text.text = (currentHeat) ? Mathf.Round(heatScript.currentHeat).ToString() + "" : Mathf.Round(heatScript.heatCap * heatScript.heatSinkOrangeCap).ToString() + "";
		
	}
}
