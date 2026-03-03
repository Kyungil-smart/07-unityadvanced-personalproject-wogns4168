using System.Collections.Generic;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance { get; private set; }
    
    public List<CardData> startingDeck = new List<CardData>();
    public int baseMaxEnergy = 3; // 기본 에너지 (유물/이벤트로 증가 가능)
    
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