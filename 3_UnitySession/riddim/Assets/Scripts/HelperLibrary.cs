using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperLibrary
{
    public static int barDivision = 4;

    public static float GetAngleBetweenVectors(Vector3 from, Vector3 to)
    {
        return Vector3.SignedAngle(from, to, Vector3.forward);
    }

    public static int GetBarIndex(float beatPosition)
    {
        return (int) Mathf.Floor(beatPosition / barDivision);
    }

    public static float GetBeatPositionInBar(float beatPosition)
    {
        int barIndex = GetBarIndex(beatPosition);
        return Mathf.InverseLerp(barIndex * barDivision, (barIndex + 1) * barDivision, beatPosition);
    }

    public static Vector3 GetVectorFromBeatPosition(float beatPosition)
    {
        float beatPositionInBar = GetBeatPositionInBar(beatPosition);
        return new Vector3(
            CycleConductor.instance.radius * Mathf.Sin(beatPositionInBar * Mathf.PI * 2f),
            CycleConductor.instance.radius * Mathf.Cos(beatPositionInBar * Mathf.PI * 2f),
            0f
        );
    }
}
