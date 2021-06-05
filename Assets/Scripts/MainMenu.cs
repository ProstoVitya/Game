using UnityEngine;

//скрипт кнопок главного меню
//кнопки "история" и "настройки" работают без кода
public class MainMenu : MonoBehaviour
{

    //при появлении разблокирует курсор
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    //метод запуска игры
    //загружает сцену с индексом 1
    public void PlayGame()
    {
        gameObject.AddComponent<LevelLoader>().LoadNextLevel(1);
    }

    //метод выхода из игры
    //при нажатии на кнопку "выход" закрывает приложение
    public void ExitGame()
    {
        Application.Quit();
    }
}
