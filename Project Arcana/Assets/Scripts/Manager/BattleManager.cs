using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    private TurnSystem _turnSystem;
    [SerializeField] private List<MonsterBase> _monsters;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        _monsters = new List<MonsterBase>(
            FindObjectsByType<MonsterBase>(FindObjectsSortMode.None)
        );
        
        _turnSystem = new TurnSystem();
        _turnSystem.Init();
    }

    private void Update()
    {
        _turnSystem.Update();
        /*if (Input.GetKeyDown(KeyCode.R)) // 몬스터 데미지 입히는 테스트 코드 
        {
            foreach (var monster in _monsters)
            {
                monster.TakeDamage(10);
            }   
        }*/
    }
    
    public void OnClickEndPlayerTurn()
    {
        _turnSystem.ChangeTurn(_turnSystem.monsterState);
    }
    
    public void MonsterTurnStart()
    {
        StartCoroutine(MonsterTurnRoutine());
    }
    
    private IEnumerator MonsterTurnRoutine()
    {
        foreach (var monster in _monsters)
        {
            if (monster == null) continue;
            if (monster.isDead) continue; // Health에 있다 가정

            yield return StartCoroutine(monster.Act());
        }
        
        _turnSystem.ChangeTurn(_turnSystem.playerState);
    }
}
