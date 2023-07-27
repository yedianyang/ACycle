using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CycleConductor : MonoBehaviour
{
    public static CycleConductor instance { get; private set; }

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
    GameObject notesContainer;
    
    public GameObject notePrefab;

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
            notes[i] = (float) i;
        }
    }

    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
        songPositionInBeats = songPosition / secPerBeat;
        
        if(nextIndex < notes.Length && notes[nextIndex] < (songPositionInBeats + beatsShownInAdvance))
        {
            SpawnCycleNotes();
            nextIndex++;
        }
    }

    void SpawnCycleNotes()
    {
        
    }
}