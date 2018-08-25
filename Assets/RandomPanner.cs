using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomPanner : MonoBehaviour {

    public Image fader;
    float minBounds;
    float maxBounds;
    Color c;
    float xzRadius = 50f;
    
    float speedMax = 4f;

    float deltaTime;

    bool fadedIn;
    bool fadedOut;

    bool fadingIn;
    bool fadingOut;

    Vector3 randomVelocity;
    // Use this for initialization
    void Start () {
        minBounds = transform.position.y;
        maxBounds = transform.position.y * 2;
        c = fader.color;
        transform.position = new Vector3(Random.Range(-maxBounds, maxBounds), Random.Range(minBounds, maxBounds), Random.Range(-maxBounds, maxBounds));
        randomVelocity = new Vector3(Random.Range(-speedMax, speedMax), 0, Random.Range(-speedMax, speedMax));
        transform.eulerAngles = new Vector3(Random.Range(45, 120), Random.Range(-25, 25), Random.Range(-5, 5));
        c.a = 255f;
        fader.color = c;
        StartCoroutine(StartNewFade(1f));
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += randomVelocity * Time.deltaTime;
        deltaTime = Time.deltaTime;
        if (fadingIn)
        {
            FadeIn();
        }
        if (fadingOut)
        {
            FadeOut();
        }
	}

    void FadeIn()
    {
        if (c.a > 250)
        {
            fadedIn = true;
            fadingIn = false;
            c.a = 255;
        }
        c.a += 255 * deltaTime;
        fader.color = c;
        
    }
    void FadeOut()
    {
        if (c.a <= 0)
        {
            fadedOut = true;
            fadingOut = false;
            c.a = 0;
        }
        c.a -= 255 * deltaTime;
        fader.color = c;

    }
    IEnumerator FadeAndNew()
    {
        fadingIn = true;
        print("1");
        while (fadedIn != true)
        {
            yield return null;
        }
        print("2");
        transform.position = new Vector3(Random.Range(-maxBounds, maxBounds), Random.Range(minBounds, maxBounds), Random.Range(-maxBounds, maxBounds));
        randomVelocity = new Vector3(Random.Range(-speedMax, speedMax), 0, Random.Range(-speedMax, speedMax));
        transform.eulerAngles = new Vector3(Random.Range(45, 120), Random.Range(-25, 25), Random.Range(-5, 5));
        fadingOut = true;
        while (fadedOut != true)
        {
            yield return null;
        }
        print("3");
        StartCoroutine(StartNewFade(Random.Range(3f, 6f)));

    }
    IEnumerator StartNewFade(float f)
    {
        fadedIn = false;
        fadedOut = false;
        fadingOut = false;
        fadingIn = false;
        yield return new WaitForSeconds(f);
        StartCoroutine(FadeAndNew());
    }
}
