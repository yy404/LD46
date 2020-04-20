using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    // private Rigidbody objectRb;

    public ParticleSystem explosionParticle;

    // Start is called before the first frame update
    void Start()
    {
        // objectRb = GetComponent<Rigidbody>();
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
            GameObject TheSpawnManagerObject = GameObject.Find("Spawn Manager");
            SpawnManager TheSpawnManagerInstance = TheSpawnManagerObject.GetComponent("SpawnManager") as SpawnManager;

            TheSpawnManagerInstance.updateGroundInfo((int)zPos, (int)xPos, null);

            ParticleSystem expParticle = Instantiate(explosionParticle, transform.position, explosionParticle.gameObject.transform.rotation);
            // expParticle.Play(); // enabled Play On Awake at prefab
            // the stop action is set to be Destroy at prefab

            GameObject AudioPlayerObject = GameObject.Find("Audio Player");
            AudioPlayer myAudioPlayer = AudioPlayerObject.GetComponent("AudioPlayer") as AudioPlayer;
            myAudioPlayer.playSoundExplosion();

            Destroy(other.gameObject);
            Destroy(gameObject);
            TheSpawnManagerInstance.updateEnemySum(-1);
            TheSpawnManagerInstance.addScore();

            if (xPos > 0)
            {
                GameObject thisObject = TheSpawnManagerInstance.checkGroundInfo((int)zPos, (int)xPos - 1 );
                if (thisObject != null)
                {
                    if (thisObject.CompareTag("Enemy"))
                    {
                        expParticle = Instantiate(explosionParticle, thisObject.transform.position, explosionParticle.gameObject.transform.rotation);
                        // expParticle.Play(); // enabled Play On Awake at prefab
                        // the stop action is set to be Destroy at prefab

                        TheSpawnManagerInstance.updateGroundInfo((int)zPos, (int)xPos - 1, null);
                        Destroy(thisObject);
                        TheSpawnManagerInstance.updateEnemySum(-1);
                        TheSpawnManagerInstance.addScore();
                    }
                }
            }
            if (xPos < 19.0f)
            {
              GameObject thisObject = TheSpawnManagerInstance.checkGroundInfo((int)zPos, (int)xPos + 1 );
              if (thisObject != null)
              {
                  if (thisObject.CompareTag("Enemy"))
                  {
                      expParticle = Instantiate(explosionParticle, thisObject.transform.position, explosionParticle.gameObject.transform.rotation);
                      // expParticle.Play(); // enabled Play On Awake at prefab
                      // the stop action is set to be Destroy at prefab

                      TheSpawnManagerInstance.updateGroundInfo((int)zPos, (int)xPos + 1, null);
                      Destroy(thisObject);
                      TheSpawnManagerInstance.updateEnemySum(-1);
                      TheSpawnManagerInstance.addScore();
                  }
              }
            }
            if (zPos > 0)
            {
              GameObject thisObject = TheSpawnManagerInstance.checkGroundInfo((int)zPos - 1, (int)xPos );
              if (thisObject != null)
              {
                  if (thisObject.CompareTag("Enemy"))
                  {
                      expParticle = Instantiate(explosionParticle, thisObject.transform.position, explosionParticle.gameObject.transform.rotation);
                      // expParticle.Play(); // enabled Play On Awake at prefab
                      // the stop action is set to be Destroy at prefab

                      TheSpawnManagerInstance.updateGroundInfo((int)zPos - 1, (int)xPos, null);
                      Destroy(thisObject);
                      TheSpawnManagerInstance.updateEnemySum(-1);
                      TheSpawnManagerInstance.addScore();
                  }
              }
            }
            if (zPos < 4.0f)
            {
              GameObject thisObject = TheSpawnManagerInstance.checkGroundInfo((int)zPos + 1, (int)xPos );
              if (thisObject != null)
              {
                  if (thisObject.CompareTag("Enemy"))
                  {
                      expParticle = Instantiate(explosionParticle, thisObject.transform.position, explosionParticle.gameObject.transform.rotation);
                      // expParticle.Play(); // enabled Play On Awake at prefab
                      // the stop action is set to be Destroy at prefab

                      TheSpawnManagerInstance.updateGroundInfo((int)zPos + 1, (int)xPos, null);
                      Destroy(thisObject);
                      TheSpawnManagerInstance.updateEnemySum(-1);
                      TheSpawnManagerInstance.addScore();
                  }
              }
            }

        }
    }

}
