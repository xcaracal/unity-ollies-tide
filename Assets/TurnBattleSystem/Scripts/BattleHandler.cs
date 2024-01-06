/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandler : MonoBehaviour {

    private static BattleHandler instance;

    public static BattleHandler GetInstance() {
        return instance;
    }


    [SerializeField] private Transform pfCharacterBattle;
    public Texture2D playerSpritesheet;
    public Texture2D enemySpritesheet;

    private CharacterBattle playerCharacterBattle;
    private CharacterBattle enemyCharacterBattle;
    private CharacterBattle activeCharacterBattle;
    private State state;

    private enum State {
        WaitingForPlayer,
        Busy,
    }

    private void Awake() {
        instance = this;
    }

    private void Start() {
        playerCharacterBattle = SpawnCharacter(true);
        enemyCharacterBattle = SpawnCharacter(false);

        SetActiveCharacterBattle(playerCharacterBattle);
        state = State.WaitingForPlayer;
    }

    private void Update() {
        if (state == State.WaitingForPlayer) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                state = State.Busy;
                playerCharacterBattle.Attack(enemyCharacterBattle, () => {
                    ChooseNextActiveCharacter();
                });
            }
        }
    }

    private CharacterBattle SpawnCharacter(bool isPlayerTeam) {
        Vector3 position;
        if (isPlayerTeam) {
            position = new Vector3(-50, 0);
        } else {
            position = new Vector3(+50, 0);
        }
        Transform characterTransform = Instantiate(pfCharacterBattle, position, Quaternion.identity);
        CharacterBattle characterBattle = characterTransform.GetComponent<CharacterBattle>();
        characterBattle.Setup(isPlayerTeam);

        return characterBattle;
    }

    private void SetActiveCharacterBattle(CharacterBattle characterBattle) {
        if (activeCharacterBattle != null) {
            activeCharacterBattle.HideSelectionCircle();
        }

        activeCharacterBattle = characterBattle;
        activeCharacterBattle.ShowSelectionCircle();
    }

    private void ChooseNextActiveCharacter() {
        if (TestBattleOver()) {
            return;
        }

        if (activeCharacterBattle == playerCharacterBattle) {
            SetActiveCharacterBattle(enemyCharacterBattle);
            state = State.Busy;
            
            enemyCharacterBattle.Attack(playerCharacterBattle, () => {
                ChooseNextActiveCharacter();
            });
        } else {
            SetActiveCharacterBattle(playerCharacterBattle);
            state = State.WaitingForPlayer;
        }
    }

    private bool TestBattleOver() {
        if (playerCharacterBattle.IsDead()) {
            // Player dead, enemy wins
            //CodeMonkey.CMDebug.TextPopupMouse("Enemy Wins!");
            BattleOverWindow.Show_Static("Enemy Wins!");
            return true;
        }
        if (enemyCharacterBattle.IsDead()) {
            // Enemy dead, player wins
            //CodeMonkey.CMDebug.TextPopupMouse("Player Wins!");
            BattleOverWindow.Show_Static("Player Wins!");
            return true;
        }

        return false;
    }
}
