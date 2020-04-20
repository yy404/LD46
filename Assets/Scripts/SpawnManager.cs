using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{

    public GameObject[] enemies;
    public GameObject powerup;
    public GameObject player;
    public GameObject titleScreen;

    // private float zEnemySpawn = 50.0f;
    // private float xSpawnRange = 200.0f;
    // private float zPowerupRange = 5.0f;
    private float ySpawn = 0.5f;

    private float powerupSpawnTime = 2.0f;
    private float enemySpawnTime = 1.0f;
    private float startDelay = 1.0f;

    private GameObject[,] groundInfo = new GameObject[5,20];
    private int enemySum = 0;

    public TextMeshProUGUI ratioText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public TextMeshProUGUI timerText;

    private float timer;
    private bool isGameActive;
    private int minRatio;

    private AudioPlayer myAudioPlayer;


    // Start is called before the first frame update
    void Start()
    {
        GameObject AudioPlayerObject = GameObject.Find("Audio Player");
        myAudioPlayer = AudioPlayerObject.GetComponent("AudioPlayer") as AudioPlayer;

    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive)
        {
            if (100 - enemySum <= minRatio)
            {
                gameOverText.gameObject.SetActive(true);
                // Time.timeScale = 0;
                CancelInvoke();
                isGameActive = false;

                myAudioPlayer.playSoundEnd();

            }
            else
            {
                timer += Time.deltaTime;
            }
            timerText.text = "Timer: " + Mathf.Round(timer * 1.0f) * 1.0f + " s";
            ratioText.text = "Healthy Bar: " + (100 - enemySum) + "%" + " > " + minRatio + "%";
        }

    }

    void SpawnRandomEnemy()
    {
        // float randomX = Random.Range(0, xSpawnRange);
        // float randomZ = Random.Range(0, zEnemySpawn);
        int randomIndex = Random.Range(0, enemies.Length);

        int randomX = Random.Range(0, 20);
        int randomZ = Random.Range(0, 5);

        while ( true )
        {
            if (randomX + randomZ > 0)
            {
                if (checkGroundInfo(randomZ, randomX) == null)
                {
                    break;
                }
                else if (checkGroundInfo(randomZ, randomX).CompareTag("Enemy") == false)
                {
                    break;
                }
            }
            // print("Try random again");
            randomX = Random.Range(0, 20);
            randomZ = Random.Range(0, 5);
        }

        Vector3 spawnPos = new Vector3(randomX*10+5, ySpawn, randomZ*10+5);

        GameObject thisObject = Instantiate(enemies[randomIndex], spawnPos, enemies[randomIndex].gameObject.transform.rotation);
        updateGroundInfo(randomZ, randomX, thisObject);
        updateEnemySum(1);

        myAudioPlayer.playSoundEnemy();
    }

    void SpawnPowerup()
    {
        // float randomX = Random.Range(-xSpawnRange, xSpawnRange);
        // float randomZ = Random.Range(-zPowerupRange, zPowerupRange);

        Vector3 spawnPos = new Vector3(0.0f, 5.0f, 0.0f);
        Instantiate(powerup, spawnPos, powerup.gameObject.transform.rotation);
    }

    public void ExpPowerup( float x, float y, float z)
    {
        Vector3 spawnPos = new Vector3(x, y, z);
        Instantiate(powerup, spawnPos, powerup.gameObject.transform.rotation);
    }

    public void SetSpawnPowerup()
    {
        Invoke("SpawnPowerup", powerupSpawnTime);
    }

    void initGroundInfo()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                groundInfo[i, j] = null;
            }
        }
    }

    public void updateEnemySum(int inc)
    {
        enemySum += inc;
    }

    public void updateGroundInfo(int i, int j, GameObject thisObject)
    {
        // update groundEnemySum += occupyCode;
        groundInfo[i, j] = thisObject;
    }

    public GameObject checkGroundInfo(int i, int j)
    {
        return groundInfo[i, j];
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int ratioInput)
    {
        InvokeRepeating("SpawnRandomEnemy", startDelay, enemySpawnTime);
        // InvokeRepeating("SpawnPowerup", startDelay, powerupSpawnTime);
        Invoke("SpawnPowerup", startDelay);

        timer = 0.0f;
        timerText.text = "Timer: " + timer + " s";

        minRatio = ratioInput;
        ratioText.text = "Healthy Bar: " + (100 - enemySum) + "%" + " > " + minRatio + "%";

        isGameActive = true;
        titleScreen.gameObject.SetActive(false);

        myAudioPlayer.playSoundOpen();

        restartButton.gameObject.SetActive(true);
    }

    // void Release()
    // {
    //     if (Input.GetKeyDown("space"))
    //     {
    //         // print("space key was pressed");
    //
    //         PlayerController playerCtrl = player.GetComponent("PlayerController") as PlayerController;
    //
    //         if (playerCtrl.GetHoldNum() < 1)
    //         {
    //             print("To load first");
    //         }
    //         else
    //         {
    //             float xPosTemp = Mathf.Floor(player.gameObject.transform.position.x / 10);
    //             float xPos = Mathf.Clamp(xPosTemp, 0.0f, 19.0f);
    //             float yPos = player.gameObject.transform.position.y;
    //             float zPosTemp = Mathf.Floor(player.gameObject.transform.position.z / 10);
    //             float zPos = Mathf.Clamp(zPosTemp, 0.0f, 4.0f);
    //             Vector3 spawnPos = new Vector3(xPos*10+5, yPos, zPos*10+5);
    //             Instantiate(powerup, spawnPos, powerup.gameObject.transform.rotation);
    //             playerCtrl.MinusHoldNum();
    //         }
    //     }
    // }
}
