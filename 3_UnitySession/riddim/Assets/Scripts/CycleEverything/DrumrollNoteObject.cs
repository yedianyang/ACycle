using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumrollNoteObject : MonoBehaviour
{
    public float beatPosition;
    public float endBeatPosition;
    public bool activated;
    
    public LineRenderer line;
    public SpriteRenderer sprite;
    public int lineSteps;

    private Vector3 lastPointInArc;
    private bool keyPressed;

    void Update()
    {
        if(activated)
        {
            // Hide drumroll starting icon
            sprite.enabled = false;

            // Handle drumroll key presses
            HandleKeyPress();
            
            // Destroy if it reaches the end of drumroll
            if(Mathf.Abs(HelperLibrary.GetAngleBetweenVectors(CyclePlayer.instance.transform.position, lastPointInArc)) < 3f)
            {
                activated = false;
                Destroy(gameObject);
            }
        }

        // Draw arc!
        float startBeatPosition = activated ? CycleConductor.instance.songPositionInBeats : beatPosition;
        float startBeatPositionInBar = HelperLibrary.GetBeatPositionInBar(startBeatPosition);
        float end = Mathf.Min(CycleConductor.instance.songPositionInBeats + CycleConductor.instance.beatsShownInAdvance, endBeatPosition);
        float angleDiffNormalized = (end - startBeatPosition) / (float)HelperLibrary.barDivision; // 0 - 1 
        DrawArc(startBeatPositionInBar * Mathf.PI * 2, angleDiffNormalized * Mathf.PI * 2f);
    }

    public void SetBeatPosition(float _beatPosition, float _endPosition)
    {
        beatPosition = _beatPosition;
        endBeatPosition = _endPosition;
        transform.position = HelperLibrary.GetVectorFromBeatPosition(beatPosition);
    }

    void HandleKeyPress()
    {
        if(Input.GetAxisRaw("Horizontal") != 0 && !keyPressed)
        {
            CycleConductor.instance.GreatHit();
            keyPressed = true;
        }
        
        if(Input.GetAxisRaw("Horizontal") == 0)
        {
            keyPressed = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            activated = true;
        }
    }

    void DrawArc(float startAngle, float angleDiff)
    {
        line.positionCount = lineSteps + 1;
        float angleStep = angleDiff / (float)lineSteps;
        float angle = startAngle;
        for(int i = 0; i <= lineSteps; i++)
        {
            Vector3 currPos = new Vector3(
                CycleConductor.instance.radius * Mathf.Sin(angle),
                CycleConductor.instance.radius * Mathf.Cos(angle),
                0f
            );
            line.SetPosition(i, currPos);
            if(i == lineSteps)
            {
                lastPointInArc = currPos;
            }
            angle += angleStep;
        }
    }
}
