using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    private float myBeat;
    private Transform start;
    private Transform goal;
    private float pct = 0f;
    private bool activated = false;
    public KeyCode keyToPress;

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
        pct = (Conductor.instance.beatsShownInAdvance - (myBeat - Conductor.instance.songPositionInBeats)) / Conductor.instance.beatsShownInAdvance;
        if(pct <= 1.0f) 
        {
            transform.position = Vector3.Lerp(start.position, goal.position, pct);
        }
        else if (gameObject != null)
        {
            Conductor.instance.MissHit();
            Destroy(gameObject);
        }
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

            float distance = transform.position.y - goal.position.y;
            if(distance >= 0.5f) 
            {
                Conductor.instance.EarlyHit();
            } 
            else if (distance < 0.5f && distance >= 0.1f)
            {
                Conductor.instance.GreatHit();
            }
            else if (distance < 0.1f && distance >= 0f)
            {
                Conductor.instance.PerfectHit();
            }
            else if (distance < 0f && distance >= -0.2f)
            {
                Conductor.instance.LateHit();
            }
            Destroy(gameObject);
        }
    }

    public void SetBeat(float beat)
    {
        myBeat = beat;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Activator"))
        {
            activated = true;
        }
    }
}
