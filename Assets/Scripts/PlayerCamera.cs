using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float mouseSensitivity;
    public GameObject player;
    public float cameraPosY = 0;
    public Vector2 mouseAxis = new Vector2();
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


    }

    // Update is called once per frame
    void LateUpdate()
    {
        PlayerMouseLook();
        
    
    }

    void PlayerMouseLook()
    {
        if (!gameManager.gameOver)
        {
            mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            player.transform.Rotate(Vector3.up * mouseAxis.x * mouseSensitivity);

            cameraPosY -= mouseAxis.y * mouseSensitivity;

            cameraPosY = Mathf.Clamp(cameraPosY, -90, 90);

            transform.localEulerAngles = Vector3.right * cameraPosY * mouseSensitivity;
        }
        

    }

    
}
