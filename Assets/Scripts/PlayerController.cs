using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 50.0f;
    // private float force = 20.0f;
    private float zBound = 50;
    private float xBound = 200;
    private Rigidbody playerRb;
    private AudioPlayer myAudioPlayer;
    public ParticleSystem teleportParticle;
    private SpawnManager TheSpawnManagerInstance;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();

        GameObject AudioPlayerObject = GameObject.Find("Audio Player");
        myAudioPlayer = AudioPlayerObject.GetComponent("AudioPlayer") as AudioPlayer;

        GameObject TheSpawnManagerObject = GameObject.Find("Spawn Manager");
        TheSpawnManagerInstance = TheSpawnManagerObject.GetComponent("SpawnManager") as SpawnManager;

    }

    // Update is called once per frame
    void Update()
    {
        if (TheSpawnManagerInstance.checkGameActive())
        {
            MovePlayer();
            // MovePlayerByForce();
            ConstrainPlayerPosition();
            Release();
            playerRb.velocity = Vector3.zero;
        }
    }

    void MovePlayerByForce()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if(playerRb.velocity.magnitude <= speed)
        {
            // playerRb.AddForce(Vector3.forward * force * verticalInput);
            // playerRb.AddForce(Vector3.right * force * horizontalInput);
            playerRb.velocity = new Vector3 (horizontalInput * speed, playerRb.velocity.y, verticalInput * speed);
            playerRb.velocity =  Vector3.ClampMagnitude(playerRb.velocity, speed);
        }
        else
        {
            // print("Reach max speed");
        }
    }

    void MovePlayer()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3 (horizontalInput, 0, verticalInput);
        transform.Translate(movement * speed * Time.deltaTime);
    }

    void ConstrainPlayerPosition()
    {
        bool isConstrained = false;

        if (transform.position.z < 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            isConstrained = true;
        }
        if (transform.position.z > zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBound);
            isConstrained = true;
        }
        if (transform.position.x < 0)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
            isConstrained = true;
        }
        if (transform.position.x > xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
            isConstrained = true;
        }

        if (isConstrained == true)
        {
            playerRb.velocity = new Vector3 (0, 0, 0);
        }
    }

    // public int GetHoldNum()
    // {
    //   return holdNum;
    // }
    //
    // public void MinusHoldNum()
    // {
    //   holdNum -= 1;
    // }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            // Debug.Log("Player has collided with enemy.");
            // Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            // Debug.Log("Player has collided with powerup.");

            // if (Input.GetKeyDown("space"))
            // {
            //     Destroy(other.gameObject);
            //     holdNum += 1;
            // }

            if (other.gameObject.transform.position.y == 0.0f)
            {
                //pass
            }
            else if (transform.childCount > 0)
            {
                //pass
            }
            else
            {
                // print("Pick powerup.");

                myAudioPlayer.playSoundPick();

                Vector3 thisPos = new Vector3(transform.position.x, 10.0f, transform.position.z);

                other.transform.position = thisPos;
                other.gameObject.transform.parent = transform;
                TheSpawnManagerInstance.SetSpawnPowerup();

            }

        }
    }

    void Release()
    {
        if (Input.GetKeyDown("space"))
        {
            // print("space key was pressed");

            if (transform.childCount > 0)
            {
                GameObject powerup = transform.GetChild(0).gameObject;
                if (powerup != null)
                {
                    float xPosTemp = Mathf.Floor(transform.position.x / 10);
                    float xPos = Mathf.Clamp(xPosTemp, 0.0f, 19.0f);
                    float yPos = 0.0f;
                    float zPosTemp = Mathf.Floor(transform.position.z / 10);
                    float zPos = Mathf.Clamp(zPosTemp, 0.0f, 4.0f);

                    if ( null == TheSpawnManagerInstance.checkGroundInfo((int)zPos, (int)xPos) )
                    {
                        Vector3 thisPos = new Vector3(xPos*10+5, yPos, zPos*10+5);
                        powerup.transform.position = thisPos;
                        powerup.transform.SetParent(null);
                        TheSpawnManagerInstance.updateGroundInfo((int)zPos, (int)xPos, powerup);

                        myAudioPlayer.playSoundPut();

                        ParticleSystem teleParticle1 = Instantiate(teleportParticle, transform.position, teleportParticle.gameObject.transform.rotation);
                        transform.position = new Vector3(0, transform.position.y, 0);
                        ParticleSystem teleParticle2 = Instantiate(teleportParticle, transform.position, teleportParticle.gameObject.transform.rotation);

                        if (TheSpawnManagerInstance.checkGameActive())
                        {
                            TheSpawnManagerInstance.resetVitalityTimer();
                        }
                    }
                    else
                    {
                        myAudioPlayer.playSoundError();
                    }
                }
            }
            else
            {
                myAudioPlayer.playSoundError();
            }
        }
    }

}
