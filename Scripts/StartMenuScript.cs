using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void QuitButton()
    {
        Application.Quit();
    }
    public void StartButton()
    {
        SceneManager.LoadScene("Die Untoten");
    }
}
