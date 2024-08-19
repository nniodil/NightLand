using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public BoxCollider attackBoxCollider;
    public ZombieController zombieController;
    public PlayerController playerController;
    public Animator animator;
    public GameManager gameManager;
    
    public float attackDamage = 5;

    public AudioClip[] attackSound;
    public AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        attackBoxCollider = GetComponent<BoxCollider>();
        zombieController = GetComponentInParent<ZombieController>();
        playerController = FindAnyObjectByType<PlayerController>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && zombieController.animator != null && !gameManager.gameOver)
        {
            int index = Random.Range(0, attackSound.Length);
            audioSource.clip = attackSound[index];
            audioSource.Play();
            
            animator.SetBool("isAttack",true);

            if(playerController.currentHealthPoint > 0)
            playerController.currentHealthPoint -= attackDamage * Time.deltaTime;
        }
    }
   
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && zombieController.animator != null)
        {
           animator.SetBool("isAttack", false);
        }
    }
}
