using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private RaycastHit hit;
    public Camera cam;
    public GameManager gameManager;
    private PlayerCamera playerCamera;
    private ZombieController zombieController;
    private PlayerController playerController;
    public ParticleSystem bulletImpact;
    public ParticleSystem bloodGush;


    [Header("Gun Settings")]

    public string weaponName;
    
    public bool isAutomaticGun;

    public float weaponDamage;
    
    public int ammoMagazine;
    public int currentAmmoMagazine;
    public int ammoReserve;
    public int maxAmmoReserve;


    public float aimTransition;
    public Vector3 normalLocalPosition;
    public Vector3 aimingLocalPosition;
    public Vector3 reloadingLocalPosition;


    public float fireRate;
    public bool canShoot;

    public bool isReloading;
    public float reloadTime;

    public LayerMask layermask;

    public GameObject crosshair;

    [Header("Mouse Settings")]

    public float weaponSwayAmount;
    public float recoilAmount;
    public Vector2 RangeRecoil;
    
    [Header("Gun Effects")]

    public GameObject muzzleFlash;
    public float muzzleFlashRate;

    public AudioSource GunFireSound;
    public AudioSource GunReloadSound;
    public AudioSource GunEmptySound;
    
    
    void Start()
    {
        isReloading = false;
        canShoot = true;
        ammoReserve = maxAmmoReserve;
        currentAmmoMagazine = ammoMagazine;
        gameManager = FindAnyObjectByType<GameManager>();
        playerCamera = FindAnyObjectByType<PlayerCamera>();
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        playerController = FindAnyObjectByType<PlayerController>();
        crosshair = GameObject.Find("Crosshair");
        GunEmptySound = GameObject.Find("Empty Mag").GetComponent<AudioSource>();
    }

    
    void Update()
    {
        GunShoot();
        GunReload();
        DetermineAim();
    }

    
    void GunShoot()
    {

        Ray ray = new Ray(cam.transform.position, cam.transform.forward * 100);

        Debug.DrawRay(cam.transform.position, cam.transform.forward * 100, Color.red);

        //Shot by shot / Automatic
        if (Physics.Raycast(ray, out hit, 100) && Input.GetKeyDown(KeyCode.Mouse0) && currentAmmoMagazine > 0 && canShoot == true && isReloading == false  && isAutomaticGun == false && layermask == 0 && !gameManager.gameOver ||
            Physics.Raycast(ray, out hit, 100) &&  Input.GetKey(KeyCode.Mouse0) && currentAmmoMagazine > 0 && canShoot == true && isReloading == false && isAutomaticGun == true && !gameManager.gameOver)
        {

            if (!hit.collider.CompareTag("Zombie") && !hit.collider.CompareTag("BonusPoints") && !hit.collider.CompareTag("MaxAmmo"))
            {
                Instantiate(bulletImpact, hit.point, Quaternion.Euler(0, 0, 180));
            }
            
            if (hit.collider.CompareTag("Zombie"))
            {
                ParticleSystem bloodEffect = Instantiate(bloodGush, hit.point, Quaternion.Euler(0, 0, 270));
            }


            DetermineRecoil();

            GunFireSound.Play();

            muzzleFlash.SetActive(true);
            StartCoroutine(MuzzleFlash());
            
            currentAmmoMagazine--;

            canShoot = false;
            StartCoroutine(FireRate());

            if (hit.collider.gameObject.layer == 0 && hit.collider.gameObject.CompareTag("Zombie"))
            {
                zombieController = hit.collider.gameObject.GetComponent<ZombieController>();
                zombieController.currentHealth -= weaponDamage;
                gameManager.points += 10;  
            }
        }
        
        if(Input.GetKeyDown(KeyCode.Mouse0) && currentAmmoMagazine <= 0 && ammoReserve <= 0)
        {
            GunEmptySound.Play();
        }
   }

    void GunReload()
    {
        
        if (currentAmmoMagazine <= 0 && ammoReserve > 0 && isReloading == false && !gameManager.gameOver || currentAmmoMagazine > 0 && ammoReserve > 0 && Input.GetKeyDown(KeyCode.R) && isReloading == false && !gameManager.gameOver)
        {
            if(currentAmmoMagazine < ammoMagazine) isReloading = true;
            
            if (isReloading == true) 
            {
                GunReloadSound.Play();
                muzzleFlash.SetActive(false);
                transform.localPosition = reloadingLocalPosition;
                StartCoroutine(Reloading());
            }  
         } 
    }

    IEnumerator MuzzleFlash()
    {
        yield return new WaitForSeconds(muzzleFlashRate);
        muzzleFlash.SetActive(false);
    }

    IEnumerator FireRate()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }


    IEnumerator Reloading()
    {
        crosshair.SetActive(true);
        playerController.walkSpeed = 7;
        yield return new WaitForSeconds(reloadTime);
        
        if (isReloading == true)
        {
            
            int ammoNeeded = ammoMagazine - currentAmmoMagazine;

            ammoReserve -= ammoNeeded;

            currentAmmoMagazine += ammoNeeded;
            
            isReloading = false;

            if(ammoReserve < 0) ammoReserve = 0;
        }
    }
    
    void DetermineAim()
     {
        if (isReloading == false && !gameManager.gameOver)
        {
            Vector3 target = normalLocalPosition;

            if (Input.GetKey(KeyCode.Mouse1))
            {
                playerController.walkSpeed = 4;
                target = aimingLocalPosition;
                crosshair.SetActive(false);

            }
            if (Input.GetKeyUp(KeyCode.Mouse1) || transform.localPosition == normalLocalPosition)
            {
                crosshair.SetActive(true);
                playerController.walkSpeed = 7;

            }
            Vector3 desiredPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * aimTransition);

            transform.localPosition = desiredPosition;

            transform.localPosition += (Vector3)playerCamera.mouseAxis * weaponSwayAmount / 1000;
        }
        
    }   

    void DetermineRecoil()
    {
        cam.transform.Rotate(Vector3.right * 1000);
        
        transform.localPosition -= Vector3.forward * recoilAmount;
        
        float xRange = Random.Range(-RangeRecoil.x, RangeRecoil.x);
        float yRange = Random.Range(-RangeRecoil.y, RangeRecoil.y);

        Vector2 recoil = new Vector2(xRange, yRange);

        playerCamera.mouseAxis += recoil;
    }
}
