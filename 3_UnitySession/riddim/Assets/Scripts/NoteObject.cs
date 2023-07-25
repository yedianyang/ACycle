using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public float myBeat;
    private Transform start;
    private Transform goal;
    public float distToHit;
    public float normalizedUnclamped = 0f;
    private bool activated = false;
    public KeyCode keyToPress;
    private bool keyPressed = false;

    void Start()
    {
        start = Conductor.instance.startTransform;
        goal = Conductor.instance.goalTransform;
    }

    void Update()
    {
        MoveNote();

        if(activated)
        {
            HandleKeyPress();
        }
    }

    void MoveNote()
    {
        distToHit = (Conductor.instance.beatsShownInAdvance - (myBeat - Conductor.instance.songPositionInBeats));
        normalizedUnclamped = InvLerp(0f, Conductor.instance.beatsShownInAdvance, distToHit);

        transform.position = Vector3.LerpUnclamped(start.position, goal.position, normalizedUnclamped);
    }

    void HandleKeyPress()
    {
        if(Input.GetKeyDown(keyToPress))
        {
            // For notes traveling from top to bottom, descending Y value
            // dist >= 0.5f                ====== Early
            // dist < 0.5f && dist >= 0.1f ====== Great
            // dist < 0.1f && dist >= 0f   ====== Perfect
            // dist < 0f && dist >= -0.2f  ====== Late
            keyPressed = true;
            float distance = transform.position.y - goal.position.y;
            if(distance >= 0.3f) 
            {
                Conductor.instance.EarlyHit();
            } 
            else if (distance < 0.3f && distance >= 0.1f)
            {
                Conductor.instance.GreatHit();
            }
            else if (distance < 0.1f && distance >= -0.1f)
            {
                Conductor.instance.PerfectHit();
            }
            else if (distance < -0.1f && distance >= -0.5f)
            {
                Conductor.instance.LateHit();
            }
            Destroy(gameObject);
        }
        else 
        {
            keyPressed = false;
        }
    }

    public void SetBeat(float beat)
    {
        myBeat = beat;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Activator"))
        {
            activated = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Activator") && !keyPressed)
        {
            activated = false;
            Conductor.instance.MissNote();
            Destroy(gameObject);
        }
    }

    float InvLerp(float a, float b, float v)
    {
        return (v - a) / (b - a);
    }
}
