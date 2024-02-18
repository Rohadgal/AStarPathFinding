using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    None,
    Menu,
    //ChangeLevel,
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
    bool isCoroutineActivated, isEasyMode = true;
    #endregion


    [SerializeField] 
    GameObject canvas, mainMenuUI, winUI, gameOverUI, creditsUI;

    private void Awake() {
        if (canvas != null) {
            winUI.SetActive(false);
            gameOverUI.SetActive(false);
            creditsUI.SetActive(false);
            canvas.SetActive(true);
            mainMenuUI.SetActive(true);
            //DontDestroyOnLoad(gameObject);
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


    public void changeGameSate(GameState t_newState) {
        if (m_gameState == t_newState) {
            return;
        }
        m_gameState = t_newState;
        switch (m_gameState) {
            case GameState.None:
                break;
            case GameState.Menu:
                //loadMainMenu();
                break;
            // case GameState.StartGame:
            //startGame();
            //break;
            case GameState.Playing:
                setCanvasOff();
                LevelManager.s_instance.changeLevelState(LevelState.Playing);
                break;
            case GameState.GameOver:
                SoundManager.s_instance.PlaySFXCaught();
                gameOver();
                break;
            case GameState.GameFinished:
                SoundManager.s_instance.PlaySFXVictory();
                Debug.Log("manager finisehd game");
                gameFinished();
                break;
            default:
                throw new UnityException("Invalid Game State");
        }
    }

    //public GameState getGameState() {
    //    return m_gameState;
    //}

    public void setCanvasOff() {
        Debug.Log("Canvas off");
        canvas.SetActive(false);
    }

    public void openCredits() {
        //float waitTimeForCredits = 4f;
        //SoundManager.s_instance.PlaySFXCredits();
        winUI.SetActive(false );
        gameOverUI.SetActive(false );
        mainMenuUI.SetActive(false);
        creditsUI.SetActive(true);
    }

    public void mainMenu() {
        gameOverUI.SetActive(false);
        winUI.SetActive(false);
        creditsUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void startGame() {
        //canvas.SetActive(false);
        changeGameSate(GameState.Playing);
    }

    public void easy() {
        isEasyMode = true;
        Debug.Log("mode: " + isEasyMode);
    }

    public void hard() {
        isEasyMode = false;
        Debug.Log("mode: " + isEasyMode);
    }

    public bool getIsEasyMode() { return isEasyMode; }

    void gameOver() {
        if (canvas != null) {
            // StartCoroutine(slowDownGameOverCanvas());
            mainMenuUI.SetActive(false);
            winUI.SetActive(false);
            canvas.SetActive(true);
            gameOverUI.SetActive(true);
        }
        LevelManager.s_instance.changeLevelState(LevelState.None);
    }

    void gameFinished() {
        Debug.Log("thisss");
        if (canvas != null) {
            mainMenuUI.SetActive(false);
            gameOverUI.SetActive(false);
            canvas.SetActive(true);
            winUI.SetActive(true);
        }
        LevelManager.s_instance.changeLevelState(LevelState.None);
    }

    //IEnumerator slowDownGameOverCanvas() {
    //    yield return new WaitForSeconds(4f);
    //    canvas.SetActive(true);
    //    gameOverUI.SetActive(true);
    //}

    public void exitGame() {
        Application.Quit();
    }

    //public void retryLevel() {
    //    SceneManager.LoadScene(gameScene);
    //}
}
