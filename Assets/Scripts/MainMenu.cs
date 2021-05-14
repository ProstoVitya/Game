using UnityEngine;

//скрипт кнопок главного меню
//кнопки "история" и "настройки" работают без кода
public class MainMenu : MonoBehaviour
{
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
