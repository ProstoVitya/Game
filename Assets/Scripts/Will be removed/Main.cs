using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<HealthBar>().GetHP() <= 0) {
            Lose();
        }
    }
    public void Lose()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
