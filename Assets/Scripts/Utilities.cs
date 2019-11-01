using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utilities : MonoBehaviour
{
    public bool paused;

    public void PauseGame()
    {
        paused = !paused;

        if (paused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitToWindows()
    {
        Application.Quit();
    }

    public void AnimateGrow(GameObject button)
    {
        button.GetComponent<Animator>().Play("Mouse Over Grow");
    }

    public void AnimateShrink(GameObject button)
    {
        button.GetComponent<Animator>().Play("Mouse Over Shrink");
    }
}
