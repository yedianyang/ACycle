using UnityEngine;

public abstract class BaseConductor
{
    public float songbpm;
    public float firstBeatOffset;
    public float beatsShownInAdvance;
    float secPerBeat;
    float songPosition;
    public float songPositionInBeats;
    float dspSongTime;
    AudioSource musicSource;
    float clipLength;
}
