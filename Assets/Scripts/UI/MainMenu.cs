using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OpenScanScene()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenProductListScene()
    {
        SceneManager.LoadScene(2);
    }

    public void OpenMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenScanDateScene()
    {
        SceneManager.LoadScene(3);
    }
}
