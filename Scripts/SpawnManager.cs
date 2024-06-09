using System.Collections;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    public GameManager gameManager;
    public ZombieController zombieController;

    [Header("Spawners")]

    public GameObject[] spawners = new GameObject[2];


    [Header("Settings")]

    public AudioSource newRound;
    public AudioSource firstRound;
    public GameObject zombie;
    public bool canSpawn;
    public bool isSpawning;
    public bool isNewWave;
    public int spawnedZombies;
    public float healthIncreaseAmount;
    public bool spawnerFound = true;


    void Start()
    {
        zombieController = FindAnyObjectByType<ZombieController>();
        gameManager = FindAnyObjectByType<GameManager>();
        gameManager.currentZombiesLeft = gameManager.zombiesAmount;
    }


    void Update()
    {
        Spawn();
        ZombiesLeft();
    }


    void Spawn()
    {
        if (canSpawn == true && isSpawning == false && !gameManager.gameOver)
        {
            StartCoroutine(ZombieSpawner());
        }
    }

    void ZombiesLeft()
    {
        if (gameManager.currentZombiesLeft <= 0 && isNewWave == false && !gameManager.gameOver)
        {
            StartCoroutine(NewRound());
        }

    }

    IEnumerator ZombieSpawner()
    {
        if (gameManager.currentZombiesLeft > 0 && spawnedZombies < gameManager.zombiesAmount)
        {
            isSpawning = true;

            yield return new WaitForSeconds(2);
            spawnedZombies++;

            ZombiesPrefab();

            isSpawning = false;

        }


    }
    IEnumerator NewRound()
    {

        isNewWave = true;
        canSpawn = false;
        newRound.Play();



        yield return new WaitForSeconds(8);
        gameManager.rounds++;
        gameManager.currentZombiesLeft += gameManager.zombiesAmount + 2;
        gameManager.zombiesAmount = gameManager.currentZombiesLeft;

        yield return new WaitForSeconds(8);
        healthIncreaseAmount += 1;
        isNewWave = false;
        spawnedZombies = 0;
        canSpawn = true;

    }

    void ZombiesPrefab()
    {
        int index = 0;

        while (!spawnerFound)
        {
            index = Random.Range(0, spawners.Length);

            if (spawners[index].activeSelf)
            {
                spawnerFound = true;
            }
        }


        if (spawnerFound)
        {
            GameObject newZombie = Instantiate(zombie, spawners[index].transform.position, transform.rotation);
            ZombieController newZombieController = newZombie.GetComponent<ZombieController>();
            spawnerFound = false;

            if (canSpawn == true)
            {
                newZombieController.maxHealth += healthIncreaseAmount;
                newZombieController.currentHealth = newZombieController.maxHealth;
            }
        }

    }


}


