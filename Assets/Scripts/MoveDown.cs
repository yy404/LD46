using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    public ParticleSystem explosionParticle;
    private SpawnManager TheSpawnManagerInstance;
    private AudioPlayer myAudioPlayer;

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
        //
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            // Debug.Log("Enemy has collided with powerup.");

            float xPosTemp = Mathf.Floor(transform.position.x / 10);
            float xPos = Mathf.Clamp(xPosTemp, 0.0f, 19.0f);
            // float yPos = 0.0f;
            float zPosTemp = Mathf.Floor(transform.position.z / 10);
            float zPos = Mathf.Clamp(zPosTemp, 0.0f, 4.0f);

            myAudioPlayer.playSoundExplosion();
            Destroy(other.gameObject);

            // updateGroundInfo?
            TheSpawnManagerInstance.refreshSpawnPowerup();
            TheSpawnManagerInstance.resetVitalityTimer();

            tryKill( (int)zPos, (int)xPos );

            if (xPos > 0)
            {
                tryKill( (int)zPos, (int)xPos-1 );
            }
            if (xPos < 19.0f)
            {
                tryKill( (int)zPos, (int)xPos+1 );
            }
            if (zPos > 0)
            {
                tryKill( (int)zPos-1, (int)xPos );
            }
            if (zPos < 4.0f)
            {
                tryKill( (int)zPos+1, (int)xPos );
            }

        }
    }

    private bool tryKill(int zPos, int xPos)
    {
      GameObject thisObject = TheSpawnManagerInstance.checkGroundInfo(zPos, xPos);
      if ((thisObject != null) && (thisObject.CompareTag("Enemy")))
      {
          ParticleSystem expParticle = Instantiate(explosionParticle, thisObject.transform.position, explosionParticle.gameObject.transform.rotation);
          // expParticle.Play(); // enabled Play On Awake at prefab
          // the stop action is set to be Destroy at prefab

          TheSpawnManagerInstance.updateGroundInfo(zPos, xPos, null);
          Destroy(thisObject);
          TheSpawnManagerInstance.updateEnemySum(-1);
          TheSpawnManagerInstance.addScore();
          return true;
      }
      return false;
    }

}
