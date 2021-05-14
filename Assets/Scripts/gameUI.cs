using UnityEngine;
using UnityEngine.SceneManagement;

//скрипт включает в себя управление меню паузы и экраном смерти
public class gameUI : MonoBehaviour
{
    public bool        gameIsPaused     = false; //проверка остановки игры
    public GameObject  pauseMenuUI;              //меню паузы
    public GameObject  DeathScreenUI;            //экран смерти

    //запускается в начале работы скрипта
    //ставит скорость игры в нормальное состояние
    private void Start()
    {
        Time.timeScale = 1f;
    }

    //вызывается на каждый фрейм
    //ставит игру на паузу/снимает с нее
    //вызывает экран смерти
    private void Update()
    {
        //при нажатии Escape ставит/снимает с паузы
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }

        //если у объекта игрока 0хп вызывает экран смерти, отключает управление
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<HealthBar>().GetHP() <= 0) {
            DeathScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Destroy(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>());
        }

    }

    //метод снимает игру с паузы, отключает меню паузы
    //убирает курсор
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    //метод ставит игру на паузу, останавливает игровое время, активирует меню паузы
    //активирует курсор
    void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    //метод для кнопки вхыода в меню
    //загружает сцену главного меню
    //активирует курсор
    public void LoadMenu()
    {
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
    }

    //кнопка выхода из игры, закрывает приложение
    public void Exit()
    {
        Application.Quit();
    }

    //кнопка перезапуска на экране смерти, перезагружает игровую сцену
    public void Restart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(1);
    }
}
