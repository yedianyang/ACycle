using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclePlayer : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        // Move player by 90 degrees or PI / 2 every beat
        transform.parent.transform.eulerAngles = new Vector3(0f, 0f, -CycleConductor.instance.songPositionInBeats * 90f);
    }
}
