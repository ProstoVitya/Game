using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public void LoadNextLevel(int index) {
        StartCoroutine(LoadLevel(index));
    }

    IEnumerator LoadLevel(int levelIndex) {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
