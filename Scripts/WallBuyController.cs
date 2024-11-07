using UnityEngine;

public class WallBuyController : MonoBehaviour
{
    public GameObject weapon;

    public string weaponName;
    public GameManager gameManager;
    public AudioSource wallbuySound;
    private Camera cam;

    public bool canBuy;

    public float weaponPrice;
    public float weaponAmmoPrice;

    void Start()
    {
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        gameManager = FindAnyObjectByType<GameManager>();
        wallbuySound = GameObject.Find("WallBuySound").GetComponent<AudioSource>();
    }

    void Update()
    {
        BuyWeapon();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("WallBuy"))
        {
            gameManager.wallBuyPrice.text = "Press F to buy " + weaponName + " [Cost: " + weaponPrice.ToString() + "] - Ammos [Cost: " + weaponAmmoPrice.ToString() + "]";
            gameManager.wallBuyGameObject.SetActive(true);
            canBuy = true;
         }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("WallBuy"))
        {
            gameManager.wallBuyGameObject.SetActive(false);
            canBuy = false;
        }
    }

    void BuyWeapon()
    {
        if (Input.GetKeyDown(KeyCode.F) && gameManager.wallBuyGameObject.activeSelf && canBuy == true && 
            gameManager.currentWeapon.name == weaponName && gameManager.currentGunController.ammoReserve != gameManager.currentGunController.maxAmmoReserve && gameManager.points >= weaponAmmoPrice && !gameManager.gameOver)
        {
            gameManager.points -= weaponAmmoPrice;
            gameManager.currentGunController.ammoReserve = gameManager.currentGunController.maxAmmoReserve;
            wallbuySound.Play();
        }



        if (Input.GetKeyDown(KeyCode.F) && gameManager.wallBuyGameObject.activeSelf && canBuy == true && gameManager.points >= weaponPrice && !gameManager.gameOver)
        {
            if ((gameManager.weapons[0] == null || gameManager.weapons[0].name != weaponName) && (gameManager.weapons[1] == null || gameManager.weapons[1].name != weaponName) && gameManager.currentWeapon.name != weaponName)
            {
                GameObject weaponBought = Instantiate(weapon, cam.transform.position, weapon.transform.rotation);
                weaponBought.transform.SetParent(cam.transform, false);

                wallbuySound.Play();

                //the first weapon bought is automatically switch to slot 2 then showed
                if (gameManager.weapons[0] != null && gameManager.weapons[1] == null)
                {
                    if (gameManager.weaponsActive[0] == true)
                    {
                        gameManager.points -= weaponPrice;
                        gameManager.weapons[1] = weaponBought;
                        gameManager.gunControllers[1] = weaponBought.GetComponent<GunController>();

                        gameManager.currentWeapon = gameManager.weapons[1];


                        gameManager.weaponsActive[1] = true;
                        gameManager.weaponsActive[0] = false;

                        gameManager.weapons[1].SetActive(true);
                        gameManager.weapons[0].SetActive(false);

                    }
                }
                
                //if weapon 0 is active then replace with new one
                if (gameManager.weapons[0] != null && gameManager.weapons[1] != null && gameManager.gunControllers[1].weaponName != weaponName)
                {
                    if (gameManager.weaponsActive[0] == true)
                    {
                        Destroy(gameManager.weapons[0]);

                        gameManager.points -= weaponPrice;
                        gameManager.weapons[0] = weaponBought;
                        gameManager.gunControllers[0] = weaponBought.GetComponent<GunController>();

                        gameManager.currentWeapon = gameManager.weapons[0];

                        gameManager.weapons[0].SetActive(true);
                    }

                }
               
                //if weapon 1 is active then replace with new one
                if (gameManager.weapons[0] != null && gameManager.weapons[1] != null && gameManager.gunControllers[1].weaponName != weaponName)
                {
                    if (gameManager.weaponsActive[1] == true)
                    {
                        Destroy(gameManager.weapons[1]);

                        gameManager.points -= weaponPrice;
                        gameManager.weapons[1] = weaponBought;
                        gameManager.gunControllers[1] = weaponBought.GetComponent<GunController>();

                        gameManager.currentWeapon = gameManager.weapons[1];

                        gameManager.weapons[1].SetActive(true);
                    }
                }
            }
        }
    }
}
