using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Beat
{
    public float beatPosition;
    public float beatEndPosition;
    public int beatType;

    public Beat(float pos, int type, float endPos = -1f)
    {
        beatPosition = pos;
        beatEndPosition = endPos;
        beatType = type;
    }
}

public class CycleConductor : MonoBehaviour
{
    public static CycleConductor instance { get; private set; }

    
    // Audio source things
    [SerializeField]
    float songBpm;
    [SerializeField]
    float firstBeatOffset;
    [SerializeField, Range(0f, 8f)]
    public float beatsShownInAdvance;
    public float secPerBeat;
    public float songPosition;
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
    public GameObject drumrollPrefab;
    public float radius = 4f;

    // Effects
    public Transform effectTransform;
    public GameObject earlyEffect, greatEffect, perfectEffect, lateEffect, missEffect;

    // Health and damage
    int currentHealth;
    public int maxHealth = 100;
    public HealthBar healthBar;

    // Score
    int currentScore;
    public int scorePerDrumrollHit = 10;
    public int scorePerEarlyOrLateHit = 100;
    public int scorePerGreatHit = 250;
    public int scorePerPerfectHit = 500;
    public TextMeshProUGUI scoreText;

    // Debug
    public TextMeshProUGUI barNum;

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

        // Initializing notes data structure
        nextIndex = 0;
        ParseFile();
        
        StartNewGame();
    }

    void Update()
    {
        if(gameStarted)
        {
            songPosition = (float)(AudioSettings.dspTime - dspSongTime + firstBeatOffset);
            songPositionInBeats = songPosition / secPerBeat;

            barNum.text = "Bar num: " + HelperLibrary.GetBarIndex(songPositionInBeats);
            
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
            case 3:
                cycleBeat = Instantiate(donPrefab, notesContainer);
                break;
            
            case 2:
            case 4:
                cycleBeat = Instantiate(kaPrefab, notesContainer);
                break;
            
            case 5:
            case 6:
            case 7:
                cycleBeat = Instantiate(drumrollPrefab, notesContainer);
                break;
        }
        if(cycleBeat != null)
        {
            if(beat.beatEndPosition == -1f)
            {
                cycleBeat.GetComponent<SingleNoteObject>().SetBeatPosition(beat.beatPosition);
            }
            else
            {
                cycleBeat.GetComponent<DrumrollNoteObject>().SetBeatPosition(beat.beatPosition, beat.beatEndPosition);
            }
        }
    }

    public void EarlyHit(Vector3 pos) 
    { 
        Instantiate(earlyEffect, pos, Quaternion.identity);
        
        currentScore += scorePerEarlyOrLateHit;
        Debug.Log("EARLY");
        NoteHit(0);
    }
    
    public void GreatHit(Vector3 pos)
    { 
        Instantiate(greatEffect, pos, Quaternion.identity);

        currentScore += scorePerGreatHit;
        Debug.Log("GREAT");
        NoteHit(2);
    }

    public void PerfectHit(Vector3 pos)
    { 
        Instantiate(perfectEffect, pos, Quaternion.identity);
        
        currentScore += scorePerPerfectHit;
        Debug.Log("Perfect");
        NoteHit(5);
    }

    public void LateHit(Vector3 pos)
    { 
        Instantiate(lateEffect, pos, Quaternion.identity);

        currentScore += scorePerEarlyOrLateHit;
        Debug.Log("Late");
        NoteHit(0);
    }

    public void DrumrollHit()
    {
        currentScore += scorePerDrumrollHit;
        NoteHit(0);
        Debug.Log("drumroll hit");
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

    public void MissNote(Vector3 pos, int missNoteDamage)
    { 
        Instantiate(missEffect, pos, Quaternion.identity);
        
        currentHealth -= missNoteDamage;
        Debug.Log("MISS");
        healthBar.SetHealth(currentHealth);
    }

    void ParseFile()
    {
        char[] splitLine = new char[] {','};
        string[] lines = file.text.Split(splitLine, System.StringSplitOptions.RemoveEmptyEntries);

        beats = new List<Beat>();
        Beat tempDrumroll = null;

        for(int i = 0; i < lines.Length; i++)
        {
            string bar = lines[i].Trim();
            float tempoBase = (float) bar.Length / 4f;

            for(int j = 0; j < bar.Length; j++)
            {
                int note = bar[j] - '0';
                float pos = (float)j / tempoBase + (float) i * 4f;

                if(note >= 1 && note <= 4)
                {
                    Beat beat = new Beat(pos, note);
                    beats.Add(beat);
                }
                else if (note >= 5 && note <= 7)
                {
                    tempDrumroll = new Beat(pos, note);
                }
                else if (note == 8)
                {
                    if(tempDrumroll != null)
                    {
                        tempDrumroll.beatEndPosition = pos;
                        beats.Add(tempDrumroll);
                        tempDrumroll = null;
                    }
                }
            }
        }
    }
}