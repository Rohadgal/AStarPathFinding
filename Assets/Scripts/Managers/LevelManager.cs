using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelState {
    None,
    Playing,
    LevelFinished,
    GameOver
}
public class LevelManager : MonoBehaviour
{
    public static LevelManager s_instance;
    LevelState m_levelState;

    private void Awake() {
        if (FindObjectOfType<LevelManager>() != null &&
            FindObjectOfType<LevelManager>().gameObject != gameObject) {
            Destroy(gameObject);
        } else {
            s_instance = this;
        }

        m_levelState = LevelState.None;
    }

    private void Update() {
        if (m_levelState == LevelState.LevelFinished) {
            Debug.Log("LLegamos aca");
            GameManager.s_instance.changeGameSate(GameState.GameFinished);
        }
        if (m_levelState == LevelState.GameOver) {
            GameManager.s_instance.changeGameSate(GameState.GameOver);
        }
    }

    public void changeLevelState(LevelState state) {
        Debug.Log("level state changed");
        m_levelState = state;
    }

    public LevelState GetLevelState() { return m_levelState; }
}
