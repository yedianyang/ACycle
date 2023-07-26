using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public float beatPosition {get; private set; }
    public Transform start {get; private set; }
    public Transform goal {get; private set; }
    public float distToHit;
    public float normalizedUnclamped = 0f;
    private bool activated = false;

    public KeyCode keyToPress {get; private set; }
    private bool keyPressed = false;

    private string colliderTag;

    void Start()
    {
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
        distToHit = (Conductor.instance.beatsShownInAdvance - (beatPosition - Conductor.instance.songPositionInBeats));
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
        beatPosition = beat;
        gameObject.SetActive(true);
    }

    public void SetDirection(KeyCode _keyCode, string _direction)
    {
        keyToPress = _keyCode;
        colliderTag = "Activator_" + _direction;
    }

    public void SetStart(Transform transform)
    {
        start = transform;
    }

    public void SetGoal(Transform transform)
    {
        goal = transform;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag(colliderTag))
        {
            activated = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag(colliderTag) && !keyPressed)
        {
            // Not sure why it's called twice sometimes
            Debug.Log(other.gameObject.name + " " + beatPosition + " " + gameObject.name); 
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
