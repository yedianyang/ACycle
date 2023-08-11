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
    public AudioSource drumrollStartSound;
    public AudioSource drumrollLoopSound;
    public AudioSource drumrollEndSound;

    DrumrollNoteObject drumrollNoteObject = null;
    bool drumrollActivated = false;
    float drumrollEndPosition = -1f;
    bool drumrollLoopActivated = false;

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
        if(CycleConductor.instance.gameStarted)
        {
            // Move player by 90 degrees every beat
            transform.parent.transform.eulerAngles = new Vector3(0f, 0f, -CycleConductor.instance.songPositionInBeats * 90f);
            
            if(drumrollActivated && drumrollEndPosition > 0f)
            {
                if(CycleConductor.instance.songPositionInBeats < drumrollEndPosition)
                {
                    // Play sound
                    if(CycleConductor.instance.songPosition < (HelperLibrary.GetSongPositionInSeconds(drumrollEndPosition) - drumrollEndSound.clip.length * 0.5))
                    {
                        if(drumrollNoteObject.keyPressed)
                        {
                            drumrollLoopSound.UnPause();
                        }
                        else
                        {
                            drumrollLoopSound.Pause();
                        }
                    }
                    else if (drumrollLoopActivated)
                    {
                        DeactivateDrumrollLoop();
                    }
                }
                else
                {
                    DeactivateDrumroll();
                }
            }
            else
            {
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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Drumroll"))
        {
            drumrollActivated = true;
            drumrollNoteObject = other.gameObject.GetComponent<DrumrollNoteObject>();
            drumrollEndPosition = drumrollNoteObject.drumrollEndPosition;
            if(Input.GetAxisRaw("Horizontal") != 0)
            {
                drumrollStartSound.Play();
            }
            drumrollLoopSound.Play();
            drumrollLoopActivated = true;
            
        }
    }

    void DeactivateDrumrollLoop()
    {
        drumrollLoopSound.Stop();
        if(drumrollNoteObject.keyPressed)
        {
            drumrollEndSound.Play();
        }
        drumrollLoopActivated = false;
    }

    void DeactivateDrumroll()
    {
        drumrollEndPosition = -1f;
        drumrollActivated = false;
        drumrollNoteObject = null;
    }
}
