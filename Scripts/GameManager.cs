using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerController playerController;
    public DoorController doorController;

    [Header("Game Over")]
    public GameObject gameOverText;
    public bool gameOver = true;
    public AudioSource gameOverSound;
    public bool gameOverSong = false;

    [Header("UI Text / GameObjects")]

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI zombiesLeftText;
    public TextMeshProUGUI roundsText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI doorPriceText;
    public TextMeshProUGUI wallBuyPrice;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI roundSurvived;
    
    public GameObject noAmmo;
    public GameObject endGameText;
    public GameObject doorGameObject;
    public GameObject healingText;
    public GameObject wallBuyGameObject;
    public GameObject startingImage;

    public int rounds;
    public int zombiesAmount;
    public int currentZombiesLeft;
    public float points;

    [Header("Inventory System")]

    public GameObject[] weapons;
    public GunController[] gunControllers;
    public GameObject currentWeapon;
    public GunController currentGunController;
    public bool[] weaponsActive;




    void Start()
    {
        startingImage.SetActive(true);
        gameOver = false;
        currentWeapon = weapons[0];
        playerController = FindAnyObjectByType<PlayerController>();
        gunControllers[1] = null;
    }

    void Update()
    {
        Inventory();
        UIText();
        PlayerHealthText();
        GameOver();
    }

    void GameOver()
    {
        if (gameOver)
        {
            if(gameOverSong == false)
            {
                gameOverSound.Play();
                Debug.Log("song");
                gameOverSong = true;
            }
            
            gameOverText.SetActive(true);
        }
        else if (!gameOver)
        {
            gameOverText.SetActive(false);
        }
    }

    void UIText()
    {
        ammoText.text = currentGunController.currentAmmoMagazine.ToString() + "/" + currentGunController.ammoReserve.ToString();
        zombiesLeftText.text = "Zombies Left : " + currentZombiesLeft.ToString();
        roundsText.text = rounds.ToString();
        pointsText.text = points.ToString() + "$";
        weaponName.text = currentWeapon.name;
        roundSurvived.text = "You Survived " + rounds.ToString() + " Rounds";

        if (currentGunController.currentAmmoMagazine <= 0 && currentGunController.ammoReserve <= 0)
        {
            noAmmo.SetActive(true);
        }
        else
        {
            noAmmo.SetActive(false);
        }
    }

    void PlayerHealthText()
    {
        healthText.text = "+ " + playerController.currentHealthPoint.ToString("F0");
    }

    IEnumerator SwitchDelay1()
    {
        yield return new WaitForSeconds(0.25f);
        weapons[0].SetActive(true);

        //disable current weapon
        weapons[1].SetActive(false);
        gunControllers[1].isReloading = false;
        gunControllers[1].canShoot = true;
        gunControllers[1].muzzleFlash.SetActive(false);

        //enabled new weapon
        currentWeapon = weapons[0];
        gunControllers[1].canShoot = true;
        weapons[0].transform.localPosition = gunControllers[0].reloadingLocalPosition;
        gunControllers[1].isReloading = false;
        gunControllers[1].canShoot = true;

    }
    IEnumerator SwitchDelay2()
    {
        
        yield return new WaitForSeconds(0.25f);
        weapons[1].SetActive(true);

        //disable current weapon
        weapons[0].SetActive(false);
        gunControllers[0].isReloading = false;
        gunControllers[0].canShoot = true;
        gunControllers[0].muzzleFlash.SetActive(false);
        
        //enabled new weapon
        currentWeapon = weapons[1];
        gunControllers[1].canShoot = true;
        weapons[1].transform.localPosition = gunControllers[1].reloadingLocalPosition;
        gunControllers[1].isReloading = false;
        gunControllers[1].canShoot = true;
    }

    void Inventory()
    {
        if (weaponsActive[0] == true)
        {
            currentWeapon.name = gunControllers[0].weaponName;
        }
        if (weaponsActive[1] == true)
        {
            currentWeapon.name = gunControllers[1].weaponName;
        }

        if (weapons[0] != null)
            gunControllers[0] = weapons[0].GetComponent<GunController>();

        if (weapons[1] != null)
            gunControllers[1] = weapons[1].GetComponent<GunController>();

        currentGunController = currentWeapon.GetComponent<GunController>();

        if (weapons[0].activeSelf)
        {
            weaponsActive[0] = true;
            weaponsActive[1] = false;
        }
        else if (weapons[1].activeSelf)
        {
            weaponsActive[1] = true;
            weaponsActive[0] = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && weapons[0] != null && gunControllers[0] != null && !gameOver)
        {
            if (weaponsActive[0] == false && gunControllers[0].isReloading == false)
            {
                StartCoroutine(SwitchDelay1());
            }

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && weapons[1] != null && gunControllers[1] != null && !gameOver)
        {
            if (weaponsActive[1] == false && gunControllers[1].isReloading == false)
            {
                StartCoroutine(SwitchDelay2());
            }
        }
    }

    public void QuitButton()
    {
        Application.Quit();
    }
    public void RestartButton()
    {
        SceneManager.LoadScene("Die Untoten");
    }
}
