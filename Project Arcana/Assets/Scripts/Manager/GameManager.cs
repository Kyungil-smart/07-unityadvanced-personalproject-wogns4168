using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject cardPrefab;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PoolManager.Instance.CreatePool(cardPrefab, 20);
        if (SceneManager.GetActiveScene().buildIndex == 0) SceneManager.LoadScene(1);
    }
}
