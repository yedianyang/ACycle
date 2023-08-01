using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Beat
{
    public float beatPosition;
    public int beatType;

    public Beat(float pos, int type)
    {
        beatPosition = pos;
        beatType = type;
    }
}

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
    public TextAsset file;
    List<Beat> beats;
    int nextIndex;
    bool gameStarted = false;

    // Notes in scene
    public Transform notesContainer;
    public GameObject donPrefab;
    public GameObject kaPrefab;

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
            
            if(nextIndex < beats.Count && beats[nextIndex].beatPosition < (songPositionInBeats + beatsShownInAdvance))
            {
                SpawnCycleBeat(beats[nextIndex]);
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
        ParseFile();

        // Initializes player health
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        gameStarted = true;

        // Init score
        currentScore = 0;
        scoreText.text = currentScore.ToString();
    }

    void SpawnCycleBeat(Beat beat)
    {
        GameObject cycleBeat = null;
        switch(beat.beatType)
        {
            case 1:
                cycleBeat = Instantiate(donPrefab, notesContainer);
                break;
            
            case 2:
                cycleBeat = Instantiate(kaPrefab, notesContainer);
                break;
            
            case 3:
                cycleBeat = Instantiate(donPrefab, notesContainer);
                break;
            
            case 4:
                cycleBeat = Instantiate(kaPrefab, notesContainer);
                break;
        }
        if(cycleBeat != null)
        {
            cycleBeat.GetComponent<CycleNoteObject>().SetBeatPosition(beat.beatPosition);
        }
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

    void ParseFile()
    {
        char[] splitLine = new char[] {','};
        string[] lines = file.text.Split(splitLine, System.StringSplitOptions.RemoveEmptyEntries);

        beats = new List<Beat>();

        for(int i = 0; i < lines.Length; i++)
        {
            string bar = lines[i];
            for(int j = 0; j < bar.Length; j++)
            {
                char note = bar[j];
                if(note != '0')
                {
                    float pos = (float)j / 4f + (float) i * 4f;
                    Beat beat = new Beat(pos, note - '0');
                    beats.Add(beat);
                }
            }
        }
    }
}