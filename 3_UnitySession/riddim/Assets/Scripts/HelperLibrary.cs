using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperLibrary
{
    public static int barDivision = 4;

    /// <summary>
    /// Get signed angle in degrees from one vector to another vector
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static float GetAngleBetweenVectors(Vector3 from, Vector3 to)
    {
        return Vector3.SignedAngle(from, to, Vector3.forward);
    }

    /// <summary>
    /// Get the nth bar from beat position, i.e. the nth row in beat map
    /// </summary>
    /// <param name="beatPosition"></param>
    /// <returns></returns>
    public static int GetBarIndex(float beatPosition)
    {
        return (int) Mathf.Floor(beatPosition / barDivision);
    }

    /// <summary>
    /// Get the beat position in a single bar, in the range [0, 4)
    /// </summary>
    /// <param name="beatPosition"></param>
    /// <returns></returns>
    public static float GetBeatPositionInBar(float beatPosition)
    {
        int barIndex = GetBarIndex(beatPosition);
        return Mathf.InverseLerp(barIndex * barDivision, (barIndex + 1) * barDivision, beatPosition);
    }

    /// <summary>
    /// Get the position in vector of point in circle
    /// </summary>
    /// <param name="beatPosition"></param>
    /// <returns></returns>
    public static Vector3 GetVectorFromBeatPosition(float beatPosition)
    {
        float beatPositionInBar = GetBeatPositionInBar(beatPosition);
        return new Vector3(
            CycleConductor.instance.radius * Mathf.Sin(beatPositionInBar * Mathf.PI * 2f),
            CycleConductor.instance.radius * Mathf.Cos(beatPositionInBar * Mathf.PI * 2f),
            0f
        );
    }

    public static float GetSongPositionInBeats(float songPositionInSeconds)
    {
        return songPositionInSeconds / CycleConductor.instance.secPerBeat;
    }

    public static float GetSongPositionInSeconds(float songPositionInBeats)
    {
        return songPositionInBeats * CycleConductor.instance.secPerBeat;
    }
}
