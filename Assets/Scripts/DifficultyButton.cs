﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    private Button button;
    private SpawnManager gameManager;

    public int ratioThreshold;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        gameManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        button.onClick.AddListener(SetDifficulty);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetDifficulty()
    {
        // Debug.Log(button.gameObject.name + " was clicked");
        gameManager.StartGame(ratioThreshold);
    }
}
