using UnityEngine;


public class DoorController : MonoBehaviour
{
    
    public GameManager gameManager;
    public int doorPrice;
    public AudioSource doorAudioSource;
    public GameObject[] spawnerToActivate;
    public GameObject lightning;


    void Start()
    {
        doorAudioSource = GameObject.Find("doorOpenSound").GetComponent<AudioSource>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("Doors"))
        {
            gameManager.doorPriceText.text = "Press F to clear debris [Cost: " + doorPrice.ToString() + "]";
            gameManager.doorGameObject.SetActive(true);
        }
        
        if (Input.GetKey(KeyCode.F) && gameManager.doorGameObject.activeSelf && gameManager.points >= doorPrice && !gameManager.gameOver)
        {
            gameManager.points -= doorPrice;
            doorAudioSource.Play();

            spawnerToActivate[0].SetActive(true);
            spawnerToActivate[1].SetActive(true);
            spawnerToActivate[2].SetActive(true);

            gameObject.SetActive(false);
            gameManager.doorGameObject.SetActive(false);
            lightning.SetActive(true);
            Destroy(lightning, 1);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("Doors"))
        {
            gameManager.doorGameObject.gameObject.SetActive(false);

            
        }
    }

  
        



}
