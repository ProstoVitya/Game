using UnityEngine;
using UnityEngine.SceneManagement;

//скрипт включает в себя управление меню паузы и экраном смерти
public class gameUI : MonoBehaviour
{
    public bool        gameIsPaused     = false; //проверка остановки игры
    public GameObject  pauseMenuUI;              //меню паузы
    public GameObject  DeathScreenUI;            //экран смерти
    public GameObject  bossHP;                   //полоска хп для босса
    public AudioSource source;                   //источник для звука смерти
    public AudioClip   deathSound;               //звук смерти
    public bool temp = false;

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
        //останавливавет музыку игры, проигрывает звук смерти
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<HealthBar>().GetHP() <= 0 && !temp) {
            temp = true;
            DeathScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            source.Stop();
            source.PlayOneShot(deathSound);
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            //Destroy(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>());
        }

    }

    //метод снимает игру с паузы, отключает меню паузы
    //убирает курсор
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        Cursor.visible = true;
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
        Cursor.visible = false;
        SceneManager.LoadScene(1);
    }
}
