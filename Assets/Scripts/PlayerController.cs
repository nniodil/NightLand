using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController playerChara;
    public GameManager gameManager;

    [Header("Player Settings")]

    public float maxHealthPoint;
    public float currentHealthPoint;
    public bool isHealing = true;
    public bool canHeal = false;
    public bool coroutineLock = false;

    public float walkSpeed;
    public float jumpForce;

    private Vector3 velocity;
    private float gravity = -25;

    public bool isGrounded;
    public Transform groundCheck;
    private float groundDistance = 0.8f;
    public LayerMask groundMask;

    void Start()
    {
        playerChara = GetComponent<CharacterController>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        SprintPlayer();
        JumpPlayer();
        MovePlayer();
        HealthPointPlayer();
    }

    void MovePlayer()
    {
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y);

        if (!gameManager.gameOver)
        {
            playerChara.Move(transform.TransformDirection(moveDirection * Time.deltaTime * walkSpeed));

            playerChara.Move(velocity * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }
        


        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

    }

    void SprintPlayer()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.Mouse1) && !gameManager.gameOver)
        {
            walkSpeed = 10;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.W))
        {
            walkSpeed = 7;
        }

    }

    void JumpPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !gameManager.gameOver)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2 * gravity);
        }

    }

    void HealthPointPlayer()
    {
        if (currentHealthPoint == maxHealthPoint)
        {
            gameManager.healingText.SetActive(false);
            isHealing = false;
            canHeal = false;
        }
        else if (currentHealthPoint < maxHealthPoint && !isHealing)
        {
            canHeal = true;
        }

        
        if (currentHealthPoint > 0 && !isHealing && canHeal && !coroutineLock)
        {
            coroutineLock = true;
            gameManager.healingText.SetActive(true);
            isHealing = true;
            canHeal = false;
            StartCoroutine(HealthRegenPlayer());

        }

        if (currentHealthPoint > maxHealthPoint)
        {
            currentHealthPoint = maxHealthPoint;
        }
        
        if (currentHealthPoint <= 0)
        {
            currentHealthPoint =  0;
            gameManager.gameOver = true;
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            gameManager.currentWeapon.transform.Translate(Vector3.up * 10);
            
            gameManager.healingText.SetActive(false);
        }

    }

    IEnumerator HealthRegenPlayer()
    {
        yield return new WaitForSeconds(30);

        if(currentHealthPoint > 0) 
        {
            float healthPointNeeded = maxHealthPoint - currentHealthPoint;
            currentHealthPoint += healthPointNeeded;
        }
        

        if (currentHealthPoint > maxHealthPoint)
        {
            currentHealthPoint = maxHealthPoint;
        }

        isHealing = false;
        canHeal = false;
        coroutineLock = false;
        gameManager.healingText.SetActive(false);

    }

    
    
    
}


