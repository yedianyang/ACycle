using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    private float myBeat;
    private Transform start;
    private Transform goal;
    public float pct = 0f;

    void Start()
    {
        start = Conductor.instance.startTransform;
        goal = Conductor.instance.goalTransform;
    }

    void Update()
    {
        pct = (Conductor.instance.beatsShownInAdvance - (myBeat - Conductor.instance.songPositionInBeats)) / Conductor.instance.beatsShownInAdvance;
        if(pct <= 1.0f) 
        {
            transform.position = Vector3.Lerp(start.position, goal.position, pct);
        }
        else if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    public void SetBeat(float beat)
    {
        myBeat = beat;
        gameObject.SetActive(true);
    }
}
