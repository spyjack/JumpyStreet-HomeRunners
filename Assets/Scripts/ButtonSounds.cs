using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    public AudioSource myFX;
    public AudioClip hoverFX;
    public AudioClip clickFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHoverFX()
    {
        myFX.PlayOneShot(hoverFX);
    }

    public void OnClickFX()
    {
        myFX.PlayOneShot(clickFX);
    }
}
