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
    private float enemySpawnTime = 2.0f;
    private float startDelay = 1.0f;

    private GameObject[,] groundInfo = new GameObject[5,20];
    private int enemySum = 0;

    public TextMeshProUGUI ratioText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    private float timer;
    private bool isGameActive;
    private int minRatio;
    private int score;
    private float vitalityDuration = 10.0f;
    private float vitalityTimer = 0.0f;

    private AudioPlayer myAudioPlayer;
    private Renderer rend;


    // Start is called before the first frame update
    void Start()
    {
        GameObject AudioPlayerObject = GameObject.Find("Audio Player");
        myAudioPlayer = AudioPlayerObject.GetComponent("AudioPlayer") as AudioPlayer;

        GameObject theGroundObject = GameObject.Find("Ground");
        rend = theGroundObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive)
        {
            if ((100 - enemySum <= minRatio) || (vitalityTimer > vitalityDuration))
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
                vitalityTimer += Time.deltaTime;
                rend.material.color = Color.Lerp(Color.red, Color.white, vitalityTimer/vitalityDuration);
            }
            scoreText.text = "Score: " + score;
            timerText.text = "Time: " + Mathf.FloorToInt(timer) + " s";
            ratioText.text = "Health: " + (100 - enemySum) + "%" + " > " + minRatio + "%"
            + "\n" + "Vitality: " + (10 - Mathf.FloorToInt(vitalityTimer));
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

            // Debug.Log("Try random again");
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

    public void refreshSpawnPowerup()
    {
        if (IsInvoking("SpawnPowerup"))
        {
            CancelInvoke("SpawnPowerup");
            SpawnPowerup();
        }
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

        Invoke("decEnemySpawnTime", startDelay + 30.0f);
        Invoke("decEnemySpawnTime", startDelay + 60.0f);
        Invoke("decEnemySpawnTime", startDelay + 90.0f);
        Invoke("decEnemySpawnTime", startDelay + 120.0f);
        Invoke("decEnemySpawnTime", startDelay + 150.0f);

        vitalityTimer = 0.0f;

        score = 0;
        scoreText.text = "Score: " + score;

        timer = 0.0f;
        timerText.text = "Time: " + timer + " s";

        minRatio = ratioInput;
        ratioText.text = "Health: " + (100 - enemySum) + "%" + " > " + minRatio + "%"
        + "\n" + "Vitality: " + (10 - Mathf.FloorToInt(vitalityTimer));

        isGameActive = true;
        titleScreen.gameObject.SetActive(false);

        myAudioPlayer.playSoundOpen();

        restartButton.gameObject.SetActive(true);
    }

    public void addScore()
    {
        score += 1;
    }

    private void decEnemySpawnTime()
    {
        if (enemySpawnTime > 0.5f)
        {
          enemySpawnTime *= 0.8f;
          CancelInvoke("SpawnRandomEnemy");
          InvokeRepeating("SpawnRandomEnemy", enemySpawnTime, enemySpawnTime);
        }
    }

    public void resetVitalityTimer()
    {
        vitalityTimer = 0.0f;
    }

    public bool checkGameActive()
    {
        return isGameActive;
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
