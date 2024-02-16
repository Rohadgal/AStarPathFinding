using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelState {
    None,
    Continue,
    LevelFinished,
    GameOver
}
public class LevelManager : MonoBehaviour
{
    public static LevelManager s_instance;
    LevelState m_levelState;
    int platformSpawnSection = 0;

    private float time = 2.2f;
    private void Awake() {
        if (FindObjectOfType<LevelManager>() != null &&
            FindObjectOfType<LevelManager>().gameObject != gameObject) {
            Destroy(gameObject);
        } else {
            s_instance = this;
        }
    }

    private void Update() {
        if (m_levelState == LevelState.LevelFinished) {
            GameManager.s_instance.changeScene();
            Debug.Log("LLegamos aca");
        }
        if (m_levelState == LevelState.GameOver) {
            GameManager.s_instance.changeGameSate(GameState.GameOver);
        }
    }

    public void changeLevelState(LevelState state) {
        m_levelState = state;
    }

    public float getTime() {
        return time;
    }

    //public void checkIncrease(int t_score) {
    //    if (t_score > 100) {
    //        return;
    //    }
    //    if (t_score % 20 == 0) {
    //        IncreaseTime();
    //    }
    //}

    //void IncreaseTime() {
    //    if (time >= 1.2f) {
    //        time -= 0.2f;
    //    }
    //}
    public void setSpawnerSection(int t_section) {
        platformSpawnSection = t_section;
    }

    public int getSpawnSection() {
        return platformSpawnSection;
    }
}
