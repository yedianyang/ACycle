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

    public SpriteRenderer sprite, effect, shadow;
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
            Vector3 effectPos = new Vector3(transform.position.x - 1f, transform.position.y + 1f, 0f);
            if(angle <= -8f)
            {
                CycleConductor.instance.EarlyHit(effectPos);
            }
            else if (angle > -8f && angle <= -3f)
            {
                CycleConductor.instance.GreatHit(effectPos);
            }
            else if (angle > -3f && angle <= 3f)
            {
                CycleConductor.instance.PerfectHit(effectPos);
            }
            else if (angle > 3f)
            {
                CycleConductor.instance.LateHit(effectPos);
            }
            keyPressed = true;
            effect.enabled = true;
            sprite.enabled = false;
            shadow.enabled = false;
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
        CycleConductor.instance.MissNote(new Vector3(transform.position.x - 1f, transform.position.y + 1f, 0f), missNoteDamage);
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
