using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PonySound : MonoBehaviour
{
    // Assign your audio clip in the Inspector

    // Assign your AudioSource component in the Inspector
    public AudioSource _PonySound;
    // Start is called before the first frame update
    void Start()
    {
        // Start the sound playback every 10 seconds
        InvokeRepeating("PlaySound", 0f, 10f);
    }
    private void PlaySound()
    {
        // Play the sound clip
        _PonySound.Play();
    }
}
