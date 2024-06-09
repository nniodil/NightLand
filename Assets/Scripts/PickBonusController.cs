using UnityEngine;

public class PickBonusController : MonoBehaviour
{
    public float speed;
    public GameManager gameManager;
    public ParticleSystem greenEffect;
    public AudioSource grabSound;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        grabSound = GameObject.Find("grabSFX").GetComponent<AudioSource>();
        greenEffect.Play();
        Destroy(gameObject, 30f);
    }

    void Update()
    {
        
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
        transform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("BonusPoints") && !gameManager.gameOver)
        {
            grabSound.Play();
            gameManager.points += 2000;
            Destroy(gameObject);
        }
        if (other.CompareTag("Player") && gameObject.CompareTag("MaxAmmo") && !gameManager.gameOver)
        {
            grabSound.Play();
            //weapon 1
            gameManager.gunControllers[0].ammoReserve = gameManager.gunControllers[0].maxAmmoReserve;
            gameManager.gunControllers[0].currentAmmoMagazine = gameManager.gunControllers[0].ammoMagazine;
            gameManager.gunControllers[0].isReloading = false;
            gameManager.gunControllers[0].GunReloadSound.Stop();



            //weapon 2
            if (gameManager.gunControllers[1] != null)
            {
                gameManager.gunControllers[1].ammoReserve = gameManager.gunControllers[1].maxAmmoReserve;
                gameManager.gunControllers[1].currentAmmoMagazine = gameManager.gunControllers[1].ammoMagazine;
                gameManager.gunControllers[1].isReloading = false;
                gameManager.gunControllers[1].GunReloadSound.Stop();

            }
            
            
            Destroy(gameObject);
        }
    }
}
