using UnityEngine;
using UnityEngine.SceneManagement;

public class gameUI : MonoBehaviour
{
    public  bool gameIsPaused = false;
    public GameObject  pauseMenuUI;
    public GameObject  DeathScreenUI;

    private void Start()
    {
        Time.timeScale = 1f;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<HealthBar>().GetHP() <= 0) {
            DeathScreenUI.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>());
        }

    }

   public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void LoadMenu() {
        gameIsPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit() {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
}
