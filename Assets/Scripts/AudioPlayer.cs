using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioClip explosionSound;
    public AudioClip pickSound;
    public AudioClip putSound;
    public AudioClip openSound;
    public AudioClip endSound;
    public AudioClip enemySound;
    public AudioClip errorSound;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void playSound(AudioClip thisAudioClip)
    {
        audioSource.PlayOneShot(thisAudioClip, 1.0f);
    }

    public void playSoundExplosion()
    {
        playSound(explosionSound);
    }

    public void playSoundPick()
    {
        playSound(pickSound);
    }

    public void playSoundPut()
    {
        playSound(putSound);
    }

    public void playSoundOpen()
    {
        playSound(openSound);
    }

    public void playSoundEnd()
    {
        playSound(endSound);
    }

    public void playSoundEnemy()
    {
        playSound(enemySound);
    }

    public void playSoundError()
    {
        playSound(errorSound);
    }


}
