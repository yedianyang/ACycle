using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleNoteObject : MonoBehaviour
{
    public float beatPosition;
    public bool activated;

    void Start()
    {
        
    }

    void Update()
    {
        
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

    private void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log("Entering " + other.gameObject.name);
        activated = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player") && activated)
        {
            Destroy(gameObject);
        }
    }
}
