using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleNoteObject : MonoBehaviour
{
    public float beatPosition;
    public bool activated;
    public KeyCode keyInput;
    private bool keyPressed = false;

    void Update()
    {
        if(activated)
        {
            float angle = Vector3.SignedAngle(
                CyclePlayer.instance.transform.position, 
                transform.position,
                Vector3.forward
            );
            
            HandleKeyPress(angle);
        }
    }

    public void SetBeatPosition(float _beatPosition) 
    {
        beatPosition = _beatPosition;

        int barDivision = 4; // replace 4 with the conductor's beat division
        int nthBar = (int) Mathf.Floor(beatPosition / barDivision);
        float beatPositionInBar = Mathf.InverseLerp(nthBar * barDivision, (nthBar + 1) * barDivision, beatPosition);
        
        transform.position = new Vector2(
            CycleConductor.instance.radius * Mathf.Sin(beatPositionInBar * Mathf.PI * 2f),
            CycleConductor.instance.radius * Mathf.Cos(beatPositionInBar * Mathf.PI * 2f));
    }

    void HandleKeyPress(float angle)
    {
        if(Input.GetKeyDown(keyInput))
        {
            if(angle <= -8f)
            {
                CycleConductor.instance.EarlyHit();
            }
            else if (angle > -8f && angle <= -3f)
            {
                CycleConductor.instance.GreatHit();
            }
            else if (angle > -3f && angle <= 3f)
            {
                CycleConductor.instance.PerfectHit();
            }
            else if (angle > 3f)
            {
                CycleConductor.instance.LateHit();
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
        CycleConductor.instance.MissNote();
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
