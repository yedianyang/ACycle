using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumrollNoteObject : MonoBehaviour
{
    public float beatPosition;
    public float endBeatPosition;
    public bool activated;
    
    public LineRenderer line;
    public int lineSteps;

    private Vector3 lastPointInArc;
    private bool keyPressed;

    void Update()
    {
        if(activated)
        {
            HandleKeyPress();
            if(GetAngleBetweenVectors(lastPointInArc, CyclePlayer.instance.transform.position) < 0)
            {
                activated = false;
                Destroy(gameObject);
            }
        }
    }

    public void SetBeatPosition(float _beatPosition, float _endPosition)
    {
        beatPosition = _beatPosition;
        endBeatPosition = _endPosition;
        int barDivision = 4; 
        int nthBar = (int) Mathf.Floor(beatPosition / barDivision);
        float beatPositionInBar = Mathf.InverseLerp(nthBar * barDivision, (nthBar + 1) * barDivision, beatPosition); // 0 1 2 3
        float angleDiffNormalized = (endBeatPosition - beatPosition) / (float)barDivision;

        transform.position = new Vector2(
            CycleConductor.instance.radius * Mathf.Sin(beatPositionInBar * Mathf.PI * 2f),
            CycleConductor.instance.radius * Mathf.Cos(beatPositionInBar * Mathf.PI * 2f));
            
        DrawArc(beatPositionInBar * Mathf.PI * 2, angleDiffNormalized * Mathf.PI * 2f);
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
        line.positionCount = lineSteps;

        for(int i = 0; i < lineSteps; i++)
        {
            float progress = (float)i / lineSteps;
            float currRadian = Mathf.Lerp(startAngle, startAngle + angleDiff, progress);
            Vector3 currPos = new Vector3(
                CycleConductor.instance.radius * Mathf.Sin(currRadian),
                CycleConductor.instance.radius * Mathf.Cos(currRadian),
                0f
            );
            line.SetPosition(i, currPos);

            if(i == lineSteps-1)
            {
                lastPointInArc = currPos;
            }
        }
    }

    float GetAngleBetweenVectors(Vector3 from, Vector3 to)
    {
        return Vector3.SignedAngle(from, to, Vector3.forward);
    }
}
