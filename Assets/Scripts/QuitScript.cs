using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Destroy(gameObject, 20f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * 250);
    }
    public void QuitButton()
    {
        Application.Quit();

    }
    public void StartButton()
    {
        SceneManager.LoadScene("Die Untoten");

    }
}
