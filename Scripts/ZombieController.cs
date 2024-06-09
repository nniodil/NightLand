using UnityEngine;
using UnityEngine.AI;


public class ZombieController : MonoBehaviour
{
    private GunController gunController;
    private PlayerController playerController;
    private GameManager gameManager;
    public Animator animator;
    public BoxCollider boxCollider;
    public ZombieAttack zombieAttack;

    [Header("Zombies Settings")]

    public bool isAlive;
    public float currentHealth;
    public float maxHealth;
    public NavMeshAgent zombie;
    public Transform playerTransform;
    public GameObject[] bonus;

    [Header("Effects")]

    public AudioClip[] runningSound;
    public AudioClip[] deathSound;
    public AudioSource audioSource;

    void Start()
    {
        isAlive = true;
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
        gameManager = FindAnyObjectByType<GameManager>();
        zombie = GetComponent<NavMeshAgent>();
        playerTransform = FindAnyObjectByType<PlayerController>().GetComponent<Transform>();
        playerController = FindAnyObjectByType<PlayerController>();
        playerController.isHealing = false;

        int index = Random.Range(0, runningSound.Length);
        audioSource.clip = runningSound[index];
        audioSource.Play();

    }


    void Update()
    {
        Health();
        FollowingPlayer();
    }


    void Health()
    {
        if (currentHealth <= 0 && isAlive == true)
        {
            isAlive = false;
            audioSource.Stop();
            
            int index2 = Random.Range(0, 300);
            if (index2 < 10)
            {
                int index = Random.Range(0, bonus.Length);
                Instantiate(bonus[index],transform.position,transform.rotation);
            }
            
            

            if(currentHealth <= 0 && isAlive == false)
            {
                audioSource.loop = false;
                int index = Random.Range(0, deathSound.Length);
                audioSource.clip = deathSound[index];
                audioSource.Play();
            }

            
            
            zombie.speed = 0;
            
            zombieAttack.attackBoxCollider.enabled = false;
            boxCollider.enabled = false;
            animator.SetBool("isDead", true);
            animator.SetBool("isAttack", false);
            gameManager.points += 100;
            gameManager.currentZombiesLeft--;
            Destroy(gameObject, 30);
        }
    
    
    }

    void FollowingPlayer()
    {
        if (isAlive == true)
        {
            zombie.SetDestination(playerTransform.position);
        }
    }
}
