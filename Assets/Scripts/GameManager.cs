using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    None,
    LoadMenu,
    ChangeLevel,
    Playing,
    GameOver,
    GameFinished
}

public class GameManager : MonoBehaviour
{
    #region Public
    public static GameManager s_instance;
    #endregion

    #region Private
    private GameState m_gameState;
    int mainMenuScene = 0, levelIndex;
    bool isCoroutineActivated;
    #endregion


    [SerializeField] GameObject canvas, winUI, gameOverUI, creditsUI;

    private void Awake() {
        if (canvas != null && SceneManager.GetActiveScene().buildIndex != mainMenuScene) {
            canvas.SetActive(false);
            winUI.SetActive(false);
            gameOverUI.SetActive(false);
            creditsUI.SetActive(false);
            DontDestroyOnLoad(gameObject);
        }
        if (FindObjectOfType<GameManager>() != null &&
            FindObjectOfType<GameManager>().gameObject != gameObject) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        s_instance = this;
        m_gameState = GameState.None;

    }

    private void Update() {
        if (m_gameState == GameState.GameOver) {
            gameOver();
        }
        if (m_gameState == GameState.GameFinished && !isCoroutineActivated) {
            isCoroutineActivated = true;
            gameFinished();
        }
    }

    public void changeScene() {
        levelIndex = SceneManager.GetActiveScene().buildIndex;
        //Debug.Log("Manager ChangeScene");

        if (SceneManager.GetActiveScene().name == "LevelThree") {
            m_gameState = GameState.GameFinished;
            Debug.Log("Corutina!!!!");
            StartCoroutine(openCredits());
            return;
        }

        //Debug.Log("Hasta aca lleg?");
        if (levelIndex < SceneManager.sceneCountInBuildSettings - 1) {
            //Debug.Log("eeeeee");
            levelIndex++;
            SceneManager.LoadScene(levelIndex);
        }
        //else {
        //    m_gameState = GameState.GameFinished;
        //}
    }

    public void changeGameSate(GameState t_newState) {
        if (m_gameState == t_newState) {
            return;
        }
        m_gameState = t_newState;
        switch (m_gameState) {
            case GameState.None:
                break;
            case GameState.LoadMenu:
                loadMainMenu();
                break;
            // case GameState.StartGame:
            //startGame();
            //break;
            case GameState.ChangeLevel:
                break;
            case GameState.Playing:
                break;
            case GameState.GameOver:
                //gameOver();
                break;
            case GameState.GameFinished:
                break;
            default:
                throw new UnityException("Invalid Game State");
        }
    }

    public GameState getGameState() {
        return m_gameState;
    }

    public void startGame() {
        changeGameSate(GameState.Playing);
    }

    IEnumerator openCredits() {
        //float waitTimeForCredits = 4f;
        yield return new WaitForSeconds(4f);
        winUI.SetActive(false);
        creditsUI.SetActive(true);
    }

    public void loadMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    void gameOver() {
        if (canvas != null) {
            StartCoroutine(slowDownGameOverCanvas());
        }
    }

    void gameFinished() {
        if (canvas != null) {
            canvas.SetActive(true);
            winUI.SetActive(true);
        }

        //if (SceneManager.GetActiveScene().name == "LevelThree1" && !isCoroutineActivated) {
        //    isCoroutineActivated=true;
        //    //canvas.SetActive(true);
        //    //StartCoroutine(openCredits());
        //    Debug.LogError("YAAAA");
        //}
    }

    IEnumerator slowDownGameOverCanvas() {
        yield return new WaitForSeconds(4f);
        canvas.SetActive(true);
        gameOverUI.SetActive(true);
    }

    public void exitGame() {
        Application.Quit();
    }

    public void retryLevel() {
        levelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(levelIndex);
    }
}
