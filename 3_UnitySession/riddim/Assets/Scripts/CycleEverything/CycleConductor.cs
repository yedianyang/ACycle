using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CycleConductor : MonoBehaviour
{
    public static CycleConductor instance { get; private set; }

    public float radius = 4f;
    
    // Audio source things
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

    // Notes data structure
    float [] notes;
    int nextIndex;
    bool gameStarted = false;

    // Notes in scene
    public Transform notesContainer;
    public GameObject notePrefab;

    // Effects
    public Transform effectTransform;
    public GameObject earlyEffect, greatEffect, perfectEffect, lateEffect, missEffect;

    // Health and damage
    int currentHealth;
    public int maxHealth = 100;
    public HealthBar healthBar;
    public int missNoteDamage = 2;

    // Score
    int currentScore;
    public int scorePerEarlyOrLateHit = 100;
    public int scorePerGreatHit = 250;
    public int scorePerPerfectHit = 500;
    public TextMeshProUGUI scoreText;

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

        // Init score
        currentScore = 0;
        scoreText.text = currentScore.ToString();
    }

    void SpawnCycleNotes(float beatPosition)
    {
        GameObject cycleNote = Instantiate(notePrefab, notesContainer);
        cycleNote.GetComponent<CycleNoteObject>().SetBeatPosition(beatPosition);
    }

    public void EarlyHit() 
    { 
        Instantiate(earlyEffect, effectTransform);
        
        currentScore += scorePerEarlyOrLateHit;
        NoteHit(0);
    }
    
    public void GreatHit()
    { 
        Instantiate(greatEffect, effectTransform);

        currentScore += scorePerGreatHit;
        NoteHit(2);
    }

    public void PerfectHit()
    { 
        Instantiate(perfectEffect, effectTransform);
        
        currentScore += scorePerPerfectHit;
        NoteHit(5);
    }

    public void LateHit()
    { 
        Instantiate(lateEffect, effectTransform);

        currentScore += scorePerEarlyOrLateHit;
        NoteHit(0);
    }

    void NoteHit(int healPlayer)
    {
        scoreText.text = currentScore.ToString();
        if(healPlayer > 0 && currentHealth < 100)
        {
            currentHealth += healPlayer;
            healthBar.SetHealth(currentHealth);
        }
    }

    public void MissNote()
    { 
        Instantiate(missEffect, effectTransform);
        
        currentHealth -= missNoteDamage;
        healthBar.SetHealth(currentHealth);
    }
}