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

    public float radius = 4f;
    
    float secPerBeat;
    float songPosition;
    public float songPositionInBeats;
    float dspSongTime;
    AudioSource musicSource;
    float clipLength;
    bool gameStarted = false;

    float [] notes;
    int nextIndex;

    // Notes in scene
    public Transform notesContainer;
    public GameObject notePrefab;

    // Effects
    public Transform effectTransform;
    public GameObject earlyEffect, greatEffect, perfectEffect, lateEffect, missEffect;

    // Health and damage
    public int maxHealth = 100;
    private int currentHealth;
    public HealthBar healthBar;
    public int missNoteDamage = 2;

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
    }

    void Update()
    {
        if(gameStarted)
        {
            songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
            songPositionInBeats = songPosition / secPerBeat;
            
            if(nextIndex < notes.Length && notes[nextIndex] < (songPositionInBeats + beatsShownInAdvance))
            {
                SpawnCycleNotes(notes[nextIndex]);
                nextIndex++;
            }
        }
    }

    public void StartNewGame()
    {
        // Audio source things
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();

        // Initializing notes data structure
        nextIndex = 0;
        notes = new float[(int)Mathf.Floor(clipLength)];
        for(int i = 0; i < Mathf.Floor(clipLength); i++)
        {
            notes[i] = (float) i;
        }

        // Initializes player health
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        gameStarted = true;
    }

    void SpawnCycleNotes(float beatPosition)
    {
        GameObject cycleNote = Instantiate(notePrefab, notesContainer);
        cycleNote.GetComponent<CycleNoteObject>().SetBeatPosition(beatPosition);
    }

    public void EarlyHit() 
    { 
        Instantiate(earlyEffect, effectTransform);
    }
    
    public void GreatHit()
    { 
        Instantiate(greatEffect, effectTransform);
    }

    public void PerfectHit()
    { 
        Instantiate(perfectEffect, effectTransform);
    }

    public void LateHit()
    { 
        Instantiate(lateEffect, effectTransform);
    }

    public void MissNote()
    { 
        Instantiate(missEffect, effectTransform);
        
        currentHealth -= missNoteDamage;
        healthBar.SetHealth(currentHealth);
    }
}