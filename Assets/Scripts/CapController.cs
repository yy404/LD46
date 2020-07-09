using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapController : MonoBehaviour
{

    public ParticleSystem explosionParticle;
    private SpawnManager TheSpawnManagerInstance;
    private AudioPlayer myAudioPlayer;
    private int lifeTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject TheSpawnManagerObject = GameObject.Find("Spawn Manager");
        TheSpawnManagerInstance = TheSpawnManagerObject.GetComponent("SpawnManager") as SpawnManager;

        GameObject AudioPlayerObject = GameObject.Find("Audio Player");
        myAudioPlayer = AudioPlayerObject.GetComponent("AudioPlayer") as AudioPlayer;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void incLifeTime(int delta)
    {
        lifeTime += delta;
    }

    public int checkLifeTime()
    {
        return lifeTime;
    }

    public void resetLifeTime()
    {
        lifeTime = 0;
    }

    public int tryClean(int thisNote)
    {
        int count = 0;

        float xPosTemp = Mathf.Floor(transform.position.x / 10);
        float xPos = Mathf.Clamp(xPosTemp, 0.0f, 19.0f);
        // float yPos = 0.0f;
        float zPosTemp = Mathf.Floor(transform.position.z / 10);
        float zPos = Mathf.Clamp(zPosTemp, 0.0f, 4.0f);

        if (xPos > 0)
        {
            if (tryKill( (int)zPos, (int)xPos-1, thisNote ) == true)
            {
                count += 1;
            }
        }
        if (xPos < 19.0f)
        {
            if (tryKill( (int)zPos, (int)xPos+1, thisNote ) == true)
            {
                count += 1;
            }
        }
        if (zPos > 0)
        {
            if (tryKill( (int)zPos-1, (int)xPos, thisNote ) == true)
            {
                count += 1;
            }
        }
        if (zPos < 4.0f)
        {
            if (tryKill( (int)zPos+1, (int)xPos, thisNote ) == true)
            {
                count += 1;
            }
        }
        return count;
    }


    private bool tryKill(int zPos, int xPos, int thisNote)
    {
      GameObject thisObject = TheSpawnManagerInstance.checkGroundInfo(zPos, xPos);
      if ((thisObject != null) && (thisObject.CompareTag("Enemy")))
      {
          if (TheSpawnManagerInstance.checkGroundNote(zPos, xPos) != thisNote)
          {
              return false;
          }

          ParticleSystem expParticle = Instantiate(explosionParticle, thisObject.transform.position, explosionParticle.gameObject.transform.rotation);
          // expParticle.Play(); // enabled Play On Awake at prefab
          // the stop action is set to be Destroy at prefab

          TheSpawnManagerInstance.updateGroundInfo(zPos, xPos, null);
          TheSpawnManagerInstance.updateGroundNote(zPos, xPos, -1);
          Destroy(thisObject);
          TheSpawnManagerInstance.updateEnemySum(-1);
          TheSpawnManagerInstance.addScore();
          return true;
      }
      return false;
    }

    public bool checkHit(int zVal, int xVal)
    {
      float xPosTemp = Mathf.Floor(transform.position.x / 10);
      float xPos = Mathf.Clamp(xPosTemp, 0.0f, 19.0f);
      float zPosTemp = Mathf.Floor(transform.position.z / 10);
      float zPos = Mathf.Clamp(zPosTemp, 0.0f, 4.0f);

      if (Mathf.Abs(xPos - xVal) + Mathf.Abs(zPos - zVal) <= 1)
      {
          return true;
      }
      else
      {
          return false;
      }
    }
}
