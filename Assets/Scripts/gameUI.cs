using UnityEngine;
using UnityEngine.SceneManagement;

//перечисление состояний меню паузы
public enum menuState { pauseOpt, restMenu, toMenu, exitMenu, notAtPause }

//скрипт включает в себя управление меню паузы и экраном смерти
public class gameUI : MonoBehaviour
{
    public GameObject           pauseMenuUI;                             //меню паузы
    public GameObject           DeathScreenUI;                           //экран смерти
    public GameObject           bossHP;                                  //полоска хп для босса
    public AudioSource          source;                                  //источник для звука смерти
    public AudioClip            deathSound;                              //звук смерти
    private bool                wasDestroyed     = false;                //провека на уничтожение игрока
    public menuState            state            = menuState.notAtPause; //вкладка меню, на которой находится пользователь
    [SerializeField] GameObject pauseOptions;
    [SerializeField] GameObject restartMenu;
    [SerializeField] GameObject exitToMenu;
    [SerializeField] GameObject exitMenu;

    //запускается в начале работы скрипта
    //ставит скорость игры в нормальное состояние
    private void Start()
    {
        Time.timeScale = 1f;
    }

    //вызывается на каждый фрейм
    //при нажатии Esc проверяет состояние меню паузы
    //ставит или снимает игру с паузы, возвращает меню на предыдущую страницу
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (state)
            {
                case menuState.pauseOpt:
                    state = menuState.notAtPause;
                    Resume();
                    break;
                case menuState.restMenu:
                    state = menuState.pauseOpt;
                    pauseOptions.SetActive(true);
                    restartMenu.SetActive(false);
                    break;
                case menuState.toMenu:
                    state = menuState.pauseOpt;
                    pauseOptions.SetActive(true);
                    exitToMenu.SetActive(false);
                    break;
                case menuState.exitMenu:
                    state = menuState.pauseOpt;
                    pauseOptions.SetActive(true);
                    exitMenu.SetActive(false);
                    break;
                case menuState.notAtPause:
                    state = menuState.pauseOpt;
                    Pause();
                    break;
            }
        }

        //если у объекта игрока 0хп вызывает экран смерти, отключает управление
        //останавливавет музыку игры, проигрывает звук смерти
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<HealthBar>().GetHP() <= 0 && !wasDestroyed) {
            wasDestroyed = true;
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
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //метод ставит игру на паузу, останавливает игровое время, активирует меню паузы
    //активирует курсор
    void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
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

    //метод установки состояния меню
    //вызывается при нажатии кнопок в меню
    public void setState(string state) {
        if (state == "restMenu")
            this.state = menuState.restMenu;
        else if (state == "toMenu")
            this.state = menuState.toMenu;
        else if (state == "exitMenu")
            this.state = menuState.exitMenu;
        else if (state == "pauseOpt")
            this.state = menuState.pauseOpt;
        else if (state == "notAtPause")
            this.state = menuState.notAtPause;
    }
}
