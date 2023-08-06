using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclePlayer : MonoBehaviour
{
    public static CyclePlayer instance {private set; get;}
    public KeyCode donHitKey;
    public KeyCode kaHitKey;
    public AudioSource donSound;
    public AudioSource kaSound;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else {
            instance = this;
        }
    }

    void Start()
    {
        transform.position = new Vector3(0f, CycleConductor.instance.radius, 0f);
    }

    void Update()
    {
        // Move player by 90 degrees every beat
        transform.parent.transform.eulerAngles = new Vector3(0f, 0f, -CycleConductor.instance.songPositionInBeats * 90f);

        if(Input.GetKeyDown(donHitKey))
        {
            donSound.Play();
        }

        if(Input.GetKeyDown(kaHitKey))
        {
            kaSound.Play();
        }
    }
}
