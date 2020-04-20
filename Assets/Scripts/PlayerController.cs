using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 20.0f;
    // private float force = 20.0f;
    private float zBound = 50;
    private float xBound = 200;
    private Rigidbody playerRb;
    private AudioPlayer myAudioPlayer;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();

        GameObject AudioPlayerObject = GameObject.Find("Audio Player");
        myAudioPlayer = AudioPlayerObject.GetComponent("AudioPlayer") as AudioPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        MovePlayerByForce();
        ConstrainPlayerPosition();
        Release();
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
        }
        else
        {
            // print("Reach max speed");
        }
    }

    void MovePlayer()
    {
        float xDist = 0.0f;
        float zDist = 0.0f;
        if (Input.GetKey(KeyCode.A) && transform.position.x > 0)
        {
            xDist -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) && transform.position.z > 0)
        {
            zDist -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) && transform.position.x < xBound)
        {
            xDist += speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W) && transform.position.z < zBound)
        {
            zDist += speed * Time.deltaTime;
        }
        transform.Translate(xDist, 0, zDist);
    }

    void ConstrainPlayerPosition()
    {
        if (transform.position.z < 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        if (transform.position.z > zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBound);
        }
        if (transform.position.x < 0)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        if (transform.position.x > xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
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
                GameObject TheSpawnManagerObject = GameObject.Find("Spawn Manager");
                SpawnManager TheSpawnManagerInstance = TheSpawnManagerObject.GetComponent("SpawnManager") as SpawnManager;
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

                    GameObject TheSpawnManagerObject = GameObject.Find("Spawn Manager");
                    SpawnManager TheSpawnManagerInstance = TheSpawnManagerObject.GetComponent("SpawnManager") as SpawnManager;
                    if ( null == TheSpawnManagerInstance.checkGroundInfo((int)zPos, (int)xPos) )
                    {
                        Vector3 thisPos = new Vector3(xPos*10+5, yPos, zPos*10+5);
                        powerup.transform.position = thisPos;
                        powerup.transform.SetParent(null);
                        TheSpawnManagerInstance.updateGroundInfo((int)zPos, (int)xPos, powerup);

                        myAudioPlayer.playSoundPut();
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
