using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerTransform;
    public Transform enemyTransform;

    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetupBattle();
        
    }

    void SetupBattle()
    {
        Instantiate(playerPrefab, playerTransform);
        Instantiate(enemyPrefab, enemyTransform);
    }



}
