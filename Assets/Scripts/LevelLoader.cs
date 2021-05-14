using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//скрипт прогрузки новой сцены
public class LevelLoader : MonoBehaviour
{
    public Animator transition; //анимация перехода между сценами

    //метод загружающий сцену с номером index
    //номер сцены передается параметром
    //запускает корутин LoadLevel
    public void LoadNextLevel(int index) {
        StartCoroutine(LoadLevel(index));
    }

    //метод останавливает выполнение кода, пока не пройдет одна секунда
    //1 секунда - время анимации перехода между сценами
    //после этого прогружает сцену с номером levelIndex
    IEnumerator LoadLevel(int levelIndex) {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelIndex);
    }

    //метод выхода из игры
    //при нажатии на кнопку "выход" закрывает приложение
    public void ExitGame()
    {
        Application.Quit();
    }
}
