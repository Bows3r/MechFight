using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTestScene : MonoBehaviour {

    bool unloading = false;

    bool firstboot;

    float timer = 0f;
    float originaltimer = 3f;
	// Use this for initialization
	void Start () {
        Cursor.visible = false;
        unloading = false;
        firstboot = true;
        if (SceneManager.GetSceneByName("TestScene").isLoaded) return;
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Space) && !unloading && firstboot)
        {
            SceneManager.UnloadSceneAsync(1);
            timer = originaltimer;
            unloading = true;
        }
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0 && unloading)
            {
                SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
                unloading = false;
                firstboot = true;
            }
            return;
        }
    }
}
