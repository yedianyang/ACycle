using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformNote : MonoBehaviour
{
    public float myBeat;

    [SerializeField]
    Vector3 startPos;

    Transform goal;

    [SerializeField, Range(0f, 0.1f)]
    float minScoreDist = 0.08f;

    void Start()
    {
        goal = Conductor.instance.goalTransform;
    }

    void Update()
    {
        // float pct = (Conductor.instance.beatsShownInAdvance - (myBeat - Conductor.instance.songPositionInBeats)) / Conductor.instance.beatsShownInAdvance;

        // if(Conductor.instance.jumpInput == 1.0f)
        // {
        //     float dist = 1f - pct; 
        //     if(dist <= minScoreDist) {
        //         float normalizedDist = (dist) / (1f - minScoreDist); 
        //         float reversedNormDist = 1f - normalizedDist;
        //         float score = Mathf.Lerp(100, 300, reversedNormDist);
        //         Conductor.instance.AddScore(score);
        //         Destroy(gameObject);
        //     }
        // }

        // if(pct < 1.0f) 
        // {
        //     transform.position = Vector3.Lerp(startPos, goal.position, pct);
        // }
        // else 
        // {
        //     Destroy(gameObject);
        // }
    }

    public void SetBeat(float beat)
    {
        myBeat = beat;
    }
}
