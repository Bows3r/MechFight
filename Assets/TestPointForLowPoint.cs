using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPointForLowPoint : MonoBehaviour
{
    public GameObject indicator;
    CharacterController controller;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = controller.center;
        pos.y -= controller.height / 1.95f;
        indicator.transform.localPosition = pos;
    }
}
