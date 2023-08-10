using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialTeleprompter : MonoBehaviour
{
    public TextMeshProUGUI text;

    void Start()
    {
        
    }

    void Update()
    {
        int currentBarIndex = HelperLibrary.GetBarIndex(CycleConductor.instance.songPositionInBeats);
        switch(currentBarIndex)
        {
            case 0:
                text.text = "Let's practice hitting some beats!";
                break;
            
            case 1:
                text.text = "Hit green single notes with left arrow key";
                break;
            
            case 4:
                text.text = "Hit blue single notes with right arrow key";
                break;
            
            case 7:
                text.text = "Hold left / right keys for long yellow notes";
                break;

            case 10:
                text.text = "That's it! Keep it up!";
                break;

            case 17:
                text.text = "Speed it up!!";
                break;

            case 38:
                text.text = "";
                break;

            case 54:
            case 55:
                text.text = "Great work! Press ESC to return to menu and play!";
                break;

            default: 
                text.text = "";
                break;
        }
    }
}
