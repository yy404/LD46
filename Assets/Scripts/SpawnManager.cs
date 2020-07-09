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
    public ParticleSystem enemyParticle;
    public ParticleSystem eneRangeParticle;

    // private float zEnemySpawn = 50.0f;
    // private float xSpawnRange = 200.0f;
    // private float zPowerupRange = 5.0f;
    private float ySpawn = 0.5f;

    private float powerupSpawnTime = 2.0f;
    private float enemySpawnTime = 0.5f;
    private float startDelay = 1.0f;

    private GameObject[,] groundInfo = new GameObject[5,20];
    private int enemySum = 0;

    public TextMeshProUGUI ratioText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI barDisplay;
    public TextMeshProUGUI explainText;

    private float timer;
    private bool isGameActive;
    private int minRatio;
    private int score;
    private float vitalityDuration = 5.0f;
    private float vitalityTimer = 0.0f;
    private string[] vitalityStrs = new string[6]{ "ZERO", "*", "**", "***", "****", "*****" };
    // private string[] barDisplayStrs = new string[8]{
    //                                                 "_ _ ",
    //                                                 "_ _ ",
    //                                                 "    _ _ ",
    //                                                 "    _ _ ",
    //                                                 "        _ _ ",
    //                                                 "        _ _ ",
    //                                                 "            _ _ ",
    //                                                 "            _ _ "};

    private AudioPlayer myAudioPlayer;
    private Renderer rend;
    private PlayerController playerCtrl;

    private int imgX = 20;
    private int imgZ = 5;
    private int imgHitCount = 0;

    private Queue capQueue;
    private int[,] groundNote = new int[5,20];

    private int ticVal = 0;
    private int decCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject AudioPlayerObject = GameObject.Find("Audio Player");
        myAudioPlayer = AudioPlayerObject.GetComponent("AudioPlayer") as AudioPlayer;

        GameObject theGroundObject = GameObject.Find("Ground");
        rend = theGroundObject.GetComponent<Renderer>();

        GameObject PlayerObject = GameObject.Find("Player");
        playerCtrl = PlayerObject.GetComponent("PlayerController") as PlayerController;

        capQueue = new Queue();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive)
        {
            if ((100 - enemySum <= minRatio) || (vitalityTimer > vitalityDuration)
            || (imgX+imgZ <= 1))
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
                if (timer > 10.0f)
                {
                    // vitalityTimer += Time.deltaTime;
                    rend.material.color = Color.Lerp(Color.red, Color.white, vitalityTimer/vitalityDuration);
                }
            }
            scoreText.text = "Score: " + score;
            timerText.text = "Time: " + Mathf.FloorToInt(timer) + " s";
            int vitalityVal = (int)vitalityDuration - Mathf.FloorToInt(vitalityTimer);
            ratioText.text = "Health: " + (100 - enemySum) + "%" + " > " + minRatio + "%"
            + "\n" + "Vitality: " + vitalityStrs[vitalityVal];
        }

    }

    void SpawnRandomEnemy(int thisNote)
    {
        // float randomX = Random.Range(0, xSpawnRange);
        // float randomZ = Random.Range(0, zEnemySpawn);
        int randomIndex = Random.Range(0, enemies.Length);

        // imgX += Random.Range(-1, 2);
        // imgZ += Random.Range(-1, 2);

        if (imgX == 0)
        {
            imgZ -= 1;
        }
        else
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    imgX += 0;
                    imgZ += 1;
                    break;
                case 1:
                    imgX += 0;
                    imgZ += 0;
                    break;
                case 2:
                    imgX += 0;
                    imgZ += -1;
                    break;
                case 3:
                    imgX += -1;
                    imgZ += 0;
                    break;
            }
        }

        imgX = Mathf.Clamp(imgX, 0, 19);
        imgZ = Mathf.Clamp(imgZ, 0, 4);

        // imgX = Random.Range(0, 20);
        // imgZ = Random.Range(0, 5);

        Vector3 particlePos = new Vector3(imgX*10+5, 5.0f, imgZ*10+5);
        Instantiate(enemyParticle, particlePos, enemyParticle.gameObject.transform.rotation);
        // enabled Play On Awake at prefab
        // the stop action is set to be Destroy at prefab

        if (imgX > 0)
        {
            int zTemp = imgZ;
            int xTemp = imgX-1;
            particlePos = new Vector3(xTemp*10+5, 0.0f, zTemp*10+5);
            Instantiate(eneRangeParticle, particlePos, eneRangeParticle.gameObject.transform.rotation);
            // enabled Play On Awake at prefab
            // the stop action is set to be Destroy at prefab
        }
        if (imgX < 19.0f)
        {
            int zTemp = imgZ;
            int xTemp = imgX+1;
            particlePos = new Vector3(xTemp*10+5, 0.0f, zTemp*10+5);
            Instantiate(eneRangeParticle, particlePos, eneRangeParticle.gameObject.transform.rotation);
            // enabled Play On Awake at prefab
            // the stop action is set to be Destroy at prefab
        }
        if (imgZ > 0)
        {
            int zTemp = imgZ-1;
            int xTemp = imgX;
            particlePos = new Vector3(xTemp*10+5, 0.0f, zTemp*10+5);
            Instantiate(eneRangeParticle, particlePos, eneRangeParticle.gameObject.transform.rotation);
            // enabled Play On Awake at prefab
            // the stop action is set to be Destroy at prefab
        }
        if (imgZ < 4.0f)
        {
            int zTemp = imgZ+1;
            int xTemp = imgX;
            particlePos = new Vector3(xTemp*10+5, 0.0f, zTemp*10+5);
            Instantiate(eneRangeParticle, particlePos, eneRangeParticle.gameObject.transform.rotation);
            // enabled Play On Awake at prefab
            // the stop action is set to be Destroy at prefab
        }

        if (checkGroundInfo(imgZ, imgX) == null)
        {
            Vector3 spawnPos = new Vector3(imgX*10+5, ySpawn, imgZ*10+5);

            GameObject thisObject = Instantiate(enemies[randomIndex], spawnPos, enemies[randomIndex].gameObject.transform.rotation);
            updateGroundInfo(imgZ, imgX, thisObject);
            updateGroundNote(imgZ, imgX, thisNote);
            updateEnemySum(1);

            Renderer rendTemp = thisObject.GetComponent<Renderer>();
            rendTemp.material.color = Color.Lerp(Color.black, Color.white, thisNote/8.0f);
            TextMesh textMeshComponent = thisObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>();
            textMeshComponent.text = thisNote.ToString();
        }
        else if (checkGroundInfo(imgZ, imgX).CompareTag("Enemy") == true)
        {
            GameObject thisObject = checkGroundInfo(imgZ, imgX);
            updateGroundNote(imgZ, imgX, thisNote);

            Renderer rendTemp = thisObject.GetComponent<Renderer>();
            rendTemp.material.color = Color.Lerp(Color.black, Color.white, thisNote/8.0f);
            TextMesh textMeshComponent = thisObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>();
            textMeshComponent.text = thisNote.ToString();
        }
        else if (checkGroundInfo(imgZ, imgX).CompareTag("Powerup") == true)
        {
            // now powerup killed in tictoc so should have no this case
        }


    }

    void SpawnPowerup()
    {
        // float randomX = Random.Range(-xSpawnRange, xSpawnRange);
        // float randomZ = Random.Range(-zPowerupRange, zPowerupRange);

        Vector3 spawnPos = new Vector3(0.0f, 5.0f, 0.0f);
        Instantiate(powerup, spawnPos, powerup.gameObject.transform.rotation);
        myAudioPlayer.playSoundPick();
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

    void initGroundData()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                groundInfo[i, j] = null;
                groundNote[i, j] = -1;
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

    public void updateGroundNote(int i, int j, int val)
    {
        groundNote[i, j] = val;
    }

    public int checkGroundNote(int i, int j)
    {
        return groundNote[i, j];
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int ratioInput)
    {
        initGroundData();

        InvokeRepeating("tictoc", startDelay, enemySpawnTime);
        // InvokeRepeating("SpawnPowerup", startDelay, powerupSpawnTime);
        Invoke("SpawnPowerup", startDelay);

        // for (int i = 30; i <= 180; i+=30)
        // {
        //   Invoke("decEnemySpawnTime", startDelay + (float)i );
        // }

        vitalityTimer = 0.0f;

        score = 0;
        scoreText.text = "Score: " + score;

        timer = 0.0f;
        timerText.text = "Time: " + timer + " s";

        minRatio = ratioInput;
        int vitalityVal = (int)vitalityDuration - Mathf.FloorToInt(vitalityTimer);
        ratioText.text = "Health: " + (100 - enemySum) + "%" + " > " + minRatio + "%"
        + "\n" + "Vitality: " + vitalityStrs[vitalityVal];

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
        if (enemySpawnTime > 0.9f) // 0.2f
        {
          enemySpawnTime *= 0.8f;
          CancelInvoke("tictoc");
          InvokeRepeating("tictoc", enemySpawnTime, enemySpawnTime);
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

    private void tictoc()
    {
        int thisBeatIndex = myAudioPlayer.getNextBeat();
        float thisFreq = myAudioPlayer.playSoundBass(thisBeatIndex);
        int thisNote = myAudioPlayer.lookupNote(thisFreq);

        // myAudioPlayer.playSoundTone(thisBeatIndex);
        // float thisTone = myAudioPlayer.playSoundTone(thisBeatIndex);
        float thisTone = 0;

        if (ticVal % 16 == 0)
        {
            playerCtrl.updateCapBuffer(1);
        }

        if (ticVal % 8 == 0)
        {
            imgHitCount = 0;
        }

        if (ticVal % (64 * Mathf.Pow(2,decCount) ) == 0)
        {
            if (ticVal > 0)
            {
                decEnemySpawnTime();
            }
            decCount += 1;

            // Debug.Log(ticVal);
            // Debug.Log(enemySpawnTime);
        }

        if (capQueue.Count != 0)
        {
            barDisplay.gameObject.SetActive(true);
            explainText.gameObject.SetActive(false);

            barDisplay.text = myAudioPlayer.getBarDisplay(thisBeatIndex);
            // if (ticVal % 8 == 0)
            // {
            //     barDisplay.text = myAudioPlayer.getBarDisplay(thisBeatIndex);
            // }
            // barDisplay.text += "> "; //barDisplayStrs[ticVal%8]

            barDisplay.text += "\nBuffer: " + playerCtrl.getCapBufferVal();
        }
        else
        {
            barDisplay.gameObject.SetActive(false);
            explainText.gameObject.SetActive(true);
            playerCtrl.ResetPlayer();
        }

        ticVal += 1;


        if (thisNote > 0)
        {
            SpawnRandomEnemy(thisNote);
        }

        if (capQueue.Count != 0)
        {
            int qLen = capQueue.Count;
            bool killAny = false;

            if (myAudioPlayer.playSoundBeat(thisBeatIndex) > 0)
            {
                for (int i = 0; i < qLen; i++)
                {
                    GameObject currentCap = (GameObject) capQueue.Dequeue();
                    CapController TheCapControllerInstance = currentCap.GetComponent("CapController") as CapController;

                    if (TheCapControllerInstance.checkLifeTime() < 8)
                    {
                        TheCapControllerInstance.incLifeTime(8); // will be 0 or 8; 1,2
                    }

                    Renderer rendTemp = currentCap.GetComponent<Renderer>();
                    if (TheCapControllerInstance.checkLifeTime() > 7) //2,0
                    {
                        rendTemp.material.color = Color.cyan;
                    }
                    else
                    {
                        rendTemp.material.color = Color.Lerp(Color.yellow, Color.cyan, TheCapControllerInstance.checkLifeTime()/8.0f);
                        // getting power sound?
                    }

                    capQueue.Enqueue(currentCap);
                }
            }
            // myAudioPlayer.playSoundTone(thisBeatIndex);

            bool skipRest = false;

            for (int i = 0; i < qLen; i++)
            {
                GameObject currentCap = (GameObject) capQueue.Dequeue();
                CapController TheCapControllerInstance;

                if (skipRest == true)
                {
                    capQueue.Enqueue(currentCap);
                    continue;
                }
                else
                {
                    TheCapControllerInstance = currentCap.GetComponent("CapController") as CapController;
                }

                if (TheCapControllerInstance.checkLifeTime() > 3 - 3)
                {
                    // Destroy(currentCap);
                    if (TheCapControllerInstance.checkHit(imgZ,imgX))
                    {
                        // crash cap sound
                        Destroy(currentCap);
                    }
                    else
                    if ( (TheCapControllerInstance.tryClean(thisNote) > 0) )
                        // || (thisNote == 0))
                    {
                        // TheCapControllerInstance.resetLifeTime();

                        if (thisNote > 0)
                        {
                            killAny = true;
                        }

                        // float thisTone = myAudioPlayer.playSoundTone(thisBeatIndex);
                        // barDisplay.text += myAudioPlayer.lookupNote(thisTone);

                        // if ( thisTone > 0)
                        if (true)
                        {
                            // to calculate hitsum from this note?
                            if (TheCapControllerInstance.checkLifeTime() > 7)
                            {
                                imgHitCount += 1; // 2
                            }
                            else
                            {
                                imgHitCount += 1;
                            }

                            if (imgHitCount > 3 || checkGroundCut(imgX)) //15
                            {
                              imgX = 20;
                              imgZ = 5;
                              imgHitCount = 0;
                            }

                            // hit sound as a function of hitCount

                            // Destroy(currentCap);

                            playerCtrl.updateCapBuffer(1); // move to above?

                        }
                        else
                        {
                            // playSoundHigh?
                        }

                        if ((thisNote > 0) || (thisTone > 0))
                        {
                            TheCapControllerInstance.incLifeTime(-8);
                        }

                        if (TheCapControllerInstance.checkLifeTime() < 0)
                        {
                            //?TheSpawnManagerInstance.updateGroundInfo(z, x, null);
                            //?auto to null?
                            Destroy(currentCap);
                        }
                        else
                        {
                            if (TheCapControllerInstance.checkLifeTime() == 0)
                            {
                                Renderer rendTemp = currentCap.GetComponent<Renderer>();
                                rendTemp.material.color = Color.yellow;
                            }

                            capQueue.Enqueue(currentCap);
                        }

                        skipRest = true; // don't check the rest of capQueue if anykill
                        // can't use break here to ensure the sequence of queue
                    }
                    else // did nothing for this cap
                    {
                        capQueue.Enqueue(currentCap);
                    }
                }
                else // capLifeTime <= 0
                {
                    capQueue.Enqueue(currentCap);
                }
            }

            barDisplay.text += "\nCombo: " +  imgHitCount.ToString() + "/4";

            if (killAny)
            {
                myAudioPlayer.playSoundHigh(thisBeatIndex);
                // Debug.Log("HERE");
            }
        }
        else
        {
            // TO DO
        }
    }

    public void addCapToQ(GameObject thisObject)
    {
        if (capQueue.Count >= 4)
        {
            GameObject currentCap = (GameObject) capQueue.Dequeue();
            Destroy(currentCap);
        }
        capQueue.Enqueue(thisObject);
    }

    public int getSizeCapQ()
    {
        return capQueue.Count;
    }

    public bool checkHitByEnemy(int zVal, int xVal)
    {
        if (Mathf.Abs(imgX - xVal) + Mathf.Abs(imgZ - zVal) <= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool checkGroundCut(int jCurr)
    {
        for (int j = jCurr; j < 20; j++)
        {
            int thisSum = 0;
            for (int i = 0; i < 5; i++)
            {
                thisSum += groundNote[i, j];
            }
            if (thisSum == -5)
            {
                return true;
            }
        }
        return false;
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
