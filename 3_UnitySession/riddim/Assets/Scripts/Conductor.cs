using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    float [] notes;
    int nextIndex;

    [SerializeField]
    GameObject notePrefab;

    [SerializeField]
    GameObject notesContainer;
    
    public Transform startTransform;
    public Transform goalTransform;

    public Transform effectTransform;
    public GameObject earlyEffect, greatEffect, perfectEffect, lateEffect, missEffect;

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
        notes = new float[(int)Mathf.Floor(clipLength)];
        for(int i = 0; i < Mathf.Floor(clipLength); i++)
        {
            notes[i] = (float)i;
        }
    }

    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
        songPositionInBeats = songPosition / secPerBeat;
        
        if(nextIndex < notes.Length && notes[nextIndex] < (songPositionInBeats + beatsShownInAdvance))
        {
            GameObject note = Instantiate(notePrefab, notesContainer.transform);
            note.GetComponent<NoteObject>().SetBeat(notes[nextIndex]);
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
        Debug.Log("Early");
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
