using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Note 
{
    public float beatPosition;
    public int [] beatDirections;

    public Note(float _beatPosition, int [] _beatDirections)
    {
        beatPosition = _beatPosition;
        beatDirections = _beatDirections;
    }
}

public enum NoteDirection
{
    Up,
    Left,
    Right,
    Down
}

public class Conductor : MonoBehaviour
{
    public static Conductor instance { get; private set; }

    [SerializeField]
    float songBpm;
    
    [SerializeField]
    float firstBeatOffset;

    [SerializeField, Range(0f, 8f)]
    public float beatsShownInAdvance;
    
    float secPerBeat;
    float songPosition;
    public float songPositionInBeats;
    float dspSongTime;
    AudioSource musicSource;
    float clipLength;

    Note [] notes;
    int nextIndex;

    [SerializeField]
    GameObject notesContainer;

    public Transform effectTransform;
    public GameObject earlyEffect, greatEffect, perfectEffect, lateEffect, missEffect;

    public GameObject upArrow, leftArrow, rightArrow, downArrow;
    public Transform upGoalTransform, leftGoalTransform, rightGoalTransform, downGoalTransform;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else {
            instance = this;
        }
    }

    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        clipLength = musicSource.clip.length;
        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();
        nextIndex = 0;
        notes = new Note[(int)Mathf.Floor(clipLength)];

        // TODO replace this with ProcessBeatmap function
        for(int i = 0; i < Mathf.Floor(clipLength); i++)
        {
            int randLength = (int) Mathf.Ceil(Random.Range(0f, 2f));
            int [] directions = new int [randLength];
            for(int j = 0; j < randLength; j++) {
                directions[j] = (int) Mathf.Round(Random.Range(0f, 3f));
            }

            notes[i] = new Note((float)i, directions);
        }
    }

    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
        songPositionInBeats = songPosition / secPerBeat;
        
        if(nextIndex < notes.Length && notes[nextIndex].beatPosition < (songPositionInBeats + beatsShownInAdvance))
        {
            for(int i = 0; i < notes[nextIndex].beatDirections.Length; i++)
            {
                int direction = notes[nextIndex].beatDirections[i];
                KeyCode key = KeyCode.Space;
                GameObject notePrefab = upArrow;
                Transform goalTransform = upGoalTransform;
                string directionString = "";
                switch (direction)
                {
                    case 0:
                        key = KeyCode.UpArrow;
                        notePrefab = upArrow;
                        goalTransform = upGoalTransform;
                        directionString = "Up";
                        break;
                    
                    case 1:
                        key = KeyCode.LeftArrow;
                        notePrefab = leftArrow;
                        goalTransform = leftGoalTransform;
                        directionString = "Left";
                        break;

                    case 2:
                        key = KeyCode.RightArrow;
                        notePrefab = rightArrow;
                        goalTransform = rightGoalTransform;
                        directionString = "Right";
                        break;

                    case 3:
                        key = KeyCode.DownArrow;
                        notePrefab = downArrow;
                        goalTransform = downGoalTransform;
                        directionString = "Down";
                        break;
                }
                GameObject note = Instantiate(notePrefab, notesContainer.transform);
                note.GetComponent<NoteObject>().SetBeat(notes[nextIndex].beatPosition);
                note.GetComponent<NoteObject>().SetDirection(key, directionString);
                note.GetComponent<NoteObject>().SetStart(notePrefab.transform);
                note.GetComponent<NoteObject>().SetGoal(goalTransform);
            }
            
            nextIndex++;
        }

        // DEBUG
        if(Input.GetMouseButton(0))
        {
            Debug.Log(songPositionInBeats);
        }
    }

    public void EarlyHit() 
    { 
        Debug.Log("Early ");
        Instantiate(earlyEffect, effectTransform);
    }
    
    public void GreatHit()
    { 
        Debug.Log("Great");
        Instantiate(greatEffect, effectTransform);
    }

    public void PerfectHit()
    { 
        Debug.Log("Perfect");
        Instantiate(perfectEffect, effectTransform);
    }

    public void LateHit()
    { 
        Debug.Log("Late");
        Instantiate(lateEffect, effectTransform);
    }

    public void MissNote()
    { 
        Debug.Log("Miss");
        Instantiate(missEffect, effectTransform);
    }
}
