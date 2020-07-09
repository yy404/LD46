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

    private Queue qt;
    private Dictionary<int, float> notes;
    private Dictionary<float, int> dict;
    private int sampleFreq = 44100;
    private float[] song;
    private float[] tone;
    private float[] drum;

    private int songLen = 64; // 32
    private int currIndex = 0;

    private float[] c = new float[6]{ 16.35f, 32.70f, 65.41f, 130.81f, 216.63f, 523.25f};
    private float[] d = new float[6]{ 18.35f, 36.71f, 73.42f, 146.83f, 293.66f, 587.33f};
    private float[] ds = new float[6]{ 19.45f, 38.89f, 77.78f, 155.56f, 311.13f, 622.25f};
    private float[] e = new float[6]{ 20.60f, 41.20f, 82.41f, 164.81f, 329.63f, 659.25f};
    private float[] f = new float[6]{ 21.83f, 43.65f, 87.31f, 174.61f, 349.23f, 698.46f};
    private float[] g = new float[6]{ 24.50f, 49.00f, 98.00f, 196.00f, 392.00f, 783.99f};
    private float[] a = new float[6]{ 27.50f, 55.00f, 110.00f, 220.00f, 440.00f, 880.00f};
    private float[] b = new float[6]{ 30.87f, 61.74f, 123.47f, 246.94f, 493.88f, 987.77f};

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        qt = new Queue();


        song = new float[]{
                            d[2],0,d[3],0,d[2],0,d[3],0,
                            e[2],0,e[3],0,e[2],0,e[3],0,
                            a[2],0,a[3],0,a[2],0,a[3],0,
                            a[2],0,a[3],0,a[2],0,a[3],0,
                            //
                            // d[2],0,d[3],0,d[2],0,d[3],0,
                            // e[2],0,e[3],0,e[2],0,e[3],0,
                            // a[2],0,a[3],0,a[2],0,a[3],0,
                            // a[2],0,a[3],0,a[2],0,a[3],0,

                            d[2],0,d[3],0,d[2],0,d[3],0,
                            e[2],0,e[3],0,e[2],0,e[3],0,
                            a[2],0,e[3],0,a[3],0,e[3],0,
                            a[2],a[3],b[2],b[3],c[2],c[3],d[2],ds[3],
                          };

        tone = new float[]{
                            e[3],a[2],c[3],d[3],e[3],0,0,0,
                            e[2],a[2],c[3],g[3],e[3],0,0,0,
                            a[2],e[3],g[3],a[3],e[3],0,0,0,
                            0,0,0,0,0,0,0,0,

                            e[3],a[2],c[3],d[3],e[3],0,0,0,
                            e[2],a[2],c[3],d[3],ds[3],d[3],c[3],a[2],
                            0,0,0,0,0,0,0,0,
                            0,0,0,0,0,0,0,0,
                          };

        drum = new float[]{
                            0,1,0,1,0,1,0,1, 0,1,0,1,0,1,0,1,
                            0,1,0,1,0,1,0,1, 0,1,0,1,0,1,0,1,

                            0,1,0,1,0,1,0,1, 0,1,0,1,0,1,0,1,
                            0,1,0,1,0,1,0,1, 0,1,0,1,0,1,0,1,
                          };

        // // ver
        // song = new float[]{ d[2],0,d[3],0,d[2],0,d[3],0, f[2],0,f[3],0,f[2],0,f[3],0,
        //                     c[2],0,c[3],0,c[2],0,c[3],0, g[2],0,g[3],0,g[2],0,g[3],0};
        //
        // tone = new float[]{ f[3],f[3],e[3],d[3],0,0,0,0, e[3],e[3],d[3],c[3],0,0,0,0,
        //                     f[3],f[3],g[3],a[3],0,0,0,0, e[3],e[3],f[3],g[3],0,0,0,0};
        //
        // drum = new float[]{ 0,1,0,1,0,1,0,1, 0,1,0,1,0,1,0,1,
        //                     0,1,0,1,0,1,0,1, 0,1,0,1,0,1,0,1,};
        // // drum = new float[]{ 1,0,0,1,0,1,1,0, 1,0,0,1,0,1,1,0,
        // //                     1,0,0,1,0,1,1,0, 1,0,0,1,0,1,1,0,};

        // ver
        // song = new float[]{ c[5],d[5],c[5],d[5],c[5],c[5],c[5],g[4],
        //                     c[5],d[5],d[5],d[5],c[5],0,0,0};

        // ver
        // song = new int[]{ 1,1,2,2,3,2,1,1, 2,2,3,3,1,1,1,0};
        // song = new int[]{ 2,0,3,0,2,0,3,0, 2,0,1,0,1,0,6,0};
        // song = new int[]{ 2,3,2,3,2,1,1,6, 2,3,2,1,1,0,0,0};








        queueReload(song);

        notes = new Dictionary<int, float>();
        notes[0]= 0.0f;
        notes[6]= 440.0f;
        notes[1]= 523.25f;
        notes[2]= 587.33f;
        notes[3]= 659.25f;
        notes[4]= 698.46f;
        notes[5]= 783.99f;

        dict = new Dictionary<float, int>();
        dict[0] = 0;
        dict[1] = 1;
        foreach(float item in c)
        {
            dict[item] = 1;
        }
        foreach(float item in d)
        {
            dict[item] = 2;
        }
        foreach(float item in ds)
        {
            dict[item] = 2; //should be 2.5
        }
        foreach(float item in e)
        {
            dict[item] = 3;
        }
        foreach(float item in f)
        {
            dict[item] = 4;
        }
        foreach(float item in g)
        {
            dict[item] = 5;
        }
        foreach(float item in a)
        {
            dict[item] = 6;
        }
        foreach(float item in b)
        {
            dict[item] = 7;
        }
    }

    // Update is called once per frame
    void Update()
    {
      if(qt.Count == 0){
         queueReload(song);
      }
      // To Debug
      // NullReferenceException: Object reference not set to an instance of an object
      // AudioPlayer.Update () (at Assets/Scripts/AudioPlayer.cs:103)

    }

    void playSound(AudioClip thisAudioClip, float vol = 1.0f)
    {
        audioSource.PlayOneShot(thisAudioClip, vol);
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

    public float playSoundBass()
    {
        float thisFreq;
        if (song is int[]) // (song.GetType() == typeof(int[]))
        {
            thisFreq = notes[(int) qt.Dequeue()];
        }
        else // float
        {
            thisFreq = (float) qt.Dequeue();
        }

        playMySound(thisFreq, 0.1f, 3);
        return thisFreq;
    }

    public float playSoundBass(int thisIndex)
    {
        float thisFreq = song[thisIndex];
        playMySound(thisFreq, 0.1f, 3);
        return thisFreq;
    }

    public float playSoundHigh(int thisIndex)
    {
        float thisFreq = song[thisIndex];
        playMySound(thisFreq, 0.1f, 2);
        return thisFreq;
    }

    public float playSoundTone(int thisIndex)
    {
        float thisFreq = tone[thisIndex];
        playMySound(thisFreq, 0.1f, 1);
        return thisFreq;
    }

    public float playSoundBeat(int thisIndex)
    {
        float thisFreq = drum[thisIndex];
        if (thisFreq > 0)
        {
            playMySound(0, 0.1f, 4, 0.1f);
        }
        else
        {
            //pass;
        }

        return thisFreq;

        // if ( currIndex % 2 == 1) //qt.Count
        // {
        //     playMySound(0);
        // }
        // else
        // {
        //     playMySound(freq, 0.1f, 4);
        // }
    }

    private void playMySound(float frequency, float duration = 0.1f, int type = 1, float vol = 1.0f)
    {
        int samplesLength = Mathf.CeilToInt(sampleFreq * duration);
        float[] samples = new float[samplesLength];
        for(int i = 0; i < samplesLength; i++)
        {
          switch (type)
          {
              case 0:
                  // sin
                  samples[i] = Mathf.Sin(Mathf.PI*2*i*frequency/sampleFreq);
                  break;
              case 1:
                  // rectangle
                  samples[i] = (Mathf.Repeat(i*frequency/sampleFreq,1) > 0.5f)?1f:-1f;
                  // samples[i] = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * i * frequency / sampleFreq));
                  break;
              case 2:
                  // sawtooth
                  samples[i] = Mathf.Repeat(i*frequency/sampleFreq,1)*2f - 1f;
                  break;
              case 3:
                  // triangle
                  samples[i] = Mathf.PingPong(i*2f*frequency/sampleFreq,1)*2f - 1f;
                  break;
              case 4:
                  // noise
                  // not using freq for now
                  samples[i] = Random.Range(0, 1.0f) * 2.0f - 1.0f;
                  break;
          }
        }
        AudioClip ac = AudioClip.Create("Test", samplesLength, 1, sampleFreq, false);
        ac.SetData(samples, 0);
        playSound(ac, vol);
    }

    private void queueReload(float[] aSong)
    {
        foreach(float item in aSong)
        {
            qt.Enqueue(item);
        }
    }
    private void queueReload(int[] aSong)
    {
        foreach(int item in aSong)
        {
            qt.Enqueue(item);
        }
    }

    public int lookupNote(float freq)
    {
        return dict[freq];
    }

    public int getNextBeat()
    {
        int ans;
        ans = currIndex;

        currIndex += 1;
        if (currIndex >= songLen)
        {
            currIndex = 0;
        }

        return ans;
    }

    public string getBarDisplay(int thisIndex)
    {
        int thisIndexDiv = thisIndex - (thisIndex%8);

        string ans = "";

        // ans += "Bass: ";
        for(int i = thisIndexDiv; i < thisIndexDiv + 8; i++)
        {
            ans += lookupNote(song[i]);
            ans += " ";
        }
        ans += "\n";

        // ans += "Rhythem: ";
        for(int i = thisIndexDiv; i < thisIndexDiv + 8; i++)
        {
            if (i == thisIndex)
            {
                ans += lookupNote(drum[i]);
            }
            else
            {
                ans += "_";
            }
            ans += " ";

            // ans += lookupNote(drum[i]);
            // ans += " ";
        }
        ans += "\n";

        // ans += "Lead: ";
        for(int i = thisIndexDiv; i < thisIndexDiv + 8; i++)
        {
            // if (i < thisIndex)
            // {
            //     ans += "_";
            //     ans += " ";
            // }
            ans += lookupNote(tone[i]);
            ans += " ";
        }
        // ans += "\n";

        return ans;

    }

}
