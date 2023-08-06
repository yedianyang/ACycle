using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UIButton : MonoBehaviour
{
    public AudioSource clickSound;
    
    public void ButtonClicked()
    {
        clickSound.Play();
    }
    
}
