using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleNoteObject : MonoBehaviour
{
    public float beatPosition;
    public bool activated;
    public KeyCode keyInput;
    private bool keyPressed = false;
    public int missNoteDamage = 2;
    public ParticleSystem.MinMaxGradient gradient;

    void Update()
    {
        if(activated)
        {
            float angle = HelperLibrary.GetAngleBetweenVectors(CyclePlayer.instance.transform.position, transform.position);
            
            HandleKeyPress(angle);
        }
    }

    public void SetBeatPosition(float _beatPosition) 
    {
        beatPosition = _beatPosition;

        transform.position = HelperLibrary.GetVectorFromBeatPosition(beatPosition);
    }

    void HandleKeyPress(float angle)
    {
        if(Input.GetKeyDown(keyInput))
        {
            if(angle <= -8f)
            {
                CycleConductor.instance.EarlyHit(transform.position, gradient);
            }
            else if (angle > -8f && angle <= -3f)
            {
                CycleConductor.instance.GreatHit(transform.position, gradient);
            }
            else if (angle > -3f && angle <= 3f)
            {
                CycleConductor.instance.PerfectHit(transform.position, gradient);
            }
            else if (angle > 3f)
            {
                CycleConductor.instance.LateHit(transform.position, gradient);
            }
            keyPressed = true;
            Destroy(gameObject);
        }
        else
        {
            keyPressed = false;
        }
    }

    void MissNote()
    {
        activated = false;
        CycleConductor.instance.MissNote(transform.position, missNoteDamage);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player"))
        {
            activated = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player") && activated && !keyPressed)
        {
            MissNote();
        }
    }
}
