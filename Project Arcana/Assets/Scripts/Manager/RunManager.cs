using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance { get; private set; }
    
    public List<CardData> startingDeck = new List<CardData>();
    
    public Deck currentDeck { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        StartNewRun();
    }

    public void StartNewRun()
    {
        currentDeck = new Deck();
        currentDeck.Init(startingDeck);
    }
}