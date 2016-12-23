using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

// Manages the regular functioning of a moon defense game
public class DefendorGame : MonoBehaviour {

    // Reference to the players main turret
    public GunTurret mainGun;

    // When did the level begin?
    private float timeStartedLevel;

    // A reference to all the pre-prepared level definitions that define enemy strength, frequency, etc.
    public LevelDefinition[] levelDefinitionsOrdered;

    // A list of the cities that the aliens can destroy. Can be any number of cities (for larger maps)
    public PlayerCity[] cities;

    // Static reference to self for convenience
    public static DefendorGame main;

    // A list of definitions which link gameobjects to text slugs that can be stored inside scriptable objects
    public EnemySpawnDefinition[] spawnDefinitions;

    // A holder for the current statistics of the level
    [SerializeField()]
    private LevelStatsSnapshot levelStats; 
    
    // Farthest left and right enemies can spawn
    public float minSpawnX = -50;
    public float maxSpawnX = 50;

    // At what Y value do enemies spawn?
    public float spawnDefaultY = 50;

    // Is the game over?
    public bool gameIsOver = false;

    // The level definition that defines the current levels behaviour
    private LevelDefinition currentLevelDefinition;

    // Reference to the game's state keeper for convenience
    private DefendorGameStateKeeper keeper;

    // Makes missiles randomly miss by certain X values, to make the game easier
    public float reduceMissileAccuracyBy = 0;

    // A list of object pools (which allow reuse of meteors and the like) where the key is the original object
    Dictionary<SpawnableObject, SpawnableObjectPool> pools = new Dictionary<SpawnableObject, SpawnableObjectPool>();

    // How many spawns should be kept in the pool?
    private const int numberOfEachSpawnToPool = 10;

    // Reference to the time last spawned enemy
    private float timeLastSpawnedEnemy;

    // Get all the original gameObjects that are the basis of spawns
    SpawnableObject[] GetAllSpawnOriginals()
    {
        return spawnDefinitions.Select(d => d.thingToSpawn).ToArray();
    }

    void Awake()
    {
        // Set convenience reference
        main = this;
    }    

    // Utility function for testing, accessed from the Editor script
    public void ForceGameWin()
    {
        HandleSurvivedLevel();
    }

    // Creates a bonus at the specified position
    public void SpawnBonusAt(Vector3 pos)
    {
        // TO DO... implement bonus calculation
        // Choose a bonus
        SpawnableObject bonus = pools[spawnDefinitions.FirstOrDefault(d => d.slug == "BONUS_SMALL").thingToSpawn].GetAvailableObject();
        // Set the bonus to active
        bonus.gameObject.SetActive(true);
        // Initialize its position and damageable object component
        bonus.GetComponent<DamagableObject>().Init();
        bonus.transform.position = pos;
        // Play a sound
        GetComponent<MoonDefenseGameSoundManager>().bonusAppearSound.Play();
    } 

    // Called by damagable objects when they are destroyed. 
    public void HandleDamagableObjectDestroyed(DamagableObject enemy, bool collectScore)
    {
        // Makes a call to the helper to handle the logic (See helper script)
        DamagableObjectDestructionHandlerHelper.HandleDestruction(enemy, collectScore, levelStats, currentLevelDefinition, this,
            GetComponent<MoonDefenseGameSoundManager>(),
            GetComponent<DefendorGameUIManager>());
    }

    
    void Start ()
    {
        // Get reference to the keeper script
        keeper = DefendorGameStateKeeper.keeper;
        
        // Set all the spawn originals to inactive
        foreach (SpawnableObject original in GetAllSpawnOriginals())
        {
            original.gameObject.SetActive(false);
        }

        // Choose a level definition based on the stage indicated in the State Keeper
        currentLevelDefinition = levelDefinitionsOrdered[keeper.lastSelectedStage];

        // Create a new snapshot of the level's stats
        levelStats = new LevelStatsSnapshot();
 
        // Create pools for each object that can possibly spawn
        foreach (SpawnableObject spawn in GetAllSpawnOriginals())
        {
            pools[spawn] = new SpawnableObjectPool(spawn, numberOfEachSpawnToPool);
        }

        // Note the time started the level
        timeStartedLevel = Time.time;
        //UpgradeApplyHelper.Apply(keeper, mainGun, this, cities);
        UpgradeApplyHelper.ApplyProfiles(keeper.upgradesAcquired.ToArray(), mainGun, this, cities);
    }

    // Function called when all the cities are destroyed (causing the player to lose, usually)
    void HandleAllCitiesDestroyed()
    {
        GetComponent<DefendorGameUIManager>().ShowLoseText();
        GetComponent<MoonDefenseGameSoundManager>().loseGameSound.Play();
        StartCoroutine(GoToFollowingScene(false));
    }

    // Wait 3 seconds and then go to the following scene
    private IEnumerator GoToFollowingScene(bool victory)
    {
        yield return new WaitForSeconds(3);
        // Choose the scene based on whether or not the game was won.
        SceneManager.LoadScene(victory ? "BeatLevel" : "YouDied");
    }

    // Function called when player survives the level, thereby winning
    private void HandleSurvivedLevel()
    {
        GetComponent<DefendorGameUIManager>().ShowWinText();

        levelStats.citiesRemain = cities.Count(c => !c.destroyed);
        levelStats.Apply(keeper);
        
        StartCoroutine(GoToFollowingScene(true));
    }

    // Return true if sufficient time has passed since the last enemy was spawned to spawn a new enemy
    private bool NextSpawnIsDue()
    {
        return Time.time - timeLastSpawnedEnemy > currentLevelDefinition.spawnEnemyEvery && 
            Time.time - timeStartedLevel > currentLevelDefinition.graceTimeBeforeFirstSpawn;
    }


    void Update () {      
        // Do nothing if its game over
        if (gameIsOver)
        {
            return;
        }

        // Aim the gun towards the mouse, and notify it if the user is requesting a shot this frame
        mainGun.AimTowards(InputHelper.GetMouseWorldPosition());
        mainGun.gunDischargeRequested = Input.GetMouseButton(0);

        // Create an array of cities not yet destroyed
        PlayerCity[] activeCities = cities.Where(c => !c.destroyed).ToArray();
        if (activeCities.Length == 0)
        {
            // If all the cities are destroyed, it's game over
            HandleAllCitiesDestroyed();
            gameIsOver = true;
        }

        if (Time.time - timeStartedLevel > currentLevelDefinition.levelDuration)
        {
            HandleSurvivedLevel();
            gameIsOver = true;
        }

        // Take note of the time until the level ends, and update the UI accordingly
        float timeUntillevelEnds = Mathf.Max(0, currentLevelDefinition.levelDuration - Time.timeSinceLevelLoad);
        GetComponent<DefendorGameUIManager>().UpdateTimeDisplay(timeUntillevelEnds);        

        // If it's time to make a new spawn...
        if (NextSpawnIsDue() && activeCities.Length > 0)
        {
            // Create a dictionary of odds for which thing will spawn
            Dictionary<SpawnableObject, int> odds = new Dictionary<SpawnableObject, int>();
            // For each possible thing that could spawn...
            
            foreach (EnemySpawnDefinition def in spawnDefinitions)
            {
                // See what the odds are based on the level definition
                var oddsDef = currentLevelDefinition.odds.FirstOrDefault(x => x.slug == def.slug);
                
                // If it is not null...
                if (oddsDef != null)
                {
                    // Populate the odds array with the odds in the definition
                    odds[def.thingToSpawn] = currentLevelDefinition.odds.FirstOrDefault(x => x.slug == def.slug).spawnOdds;
                }            
            }

            // See Calculation service for determining the winner
            SpawnableObject winner = Calculation.DetermineSpawn(odds, GetAllSpawnOriginals());
            if (!pools[winner].ObjectIsAvailable())
            {
                return;
            }

            SpawnableObject spawnedObject = pools[winner].GetAvailableObject();
                
            //Choose random xvalue for the new spawn
            float xVal = Random.Range(minSpawnX, maxSpawnX);
                
            // Place the spawn there
            spawnedObject.transform.position = new Vector2(xVal, spawnDefaultY);
                
            // Choose a target city
            PlayerCity targetCity = activeCities[Random.Range(0, activeCities.Length)];
                
            // Pick a target for the new spawn based on where the new city is, plus random missing
            float targetXOnGround = targetCity.transform.position.x + Random.Range(-currentLevelDefinition.randomlyMissTargetByUpTo + reduceMissileAccuracyBy, currentLevelDefinition.randomlyMissTargetByUpTo + reduceMissileAccuracyBy);
            float targetYOnGround = targetCity.transform.position.y;
            Vector2 targetLandingPos = new Vector2(targetXOnGround, targetYOnGround);

            // Set the new object's velocity
            spawnedObject.GetComponent<Rigidbody2D>().velocity = ((Vector3)targetLandingPos - spawnedObject.transform.position).normalized * spawnedObject.baseSpeed;

            // Take note of the time enemy was spawned
            timeLastSpawnedEnemy = Time.time; 
        }
	}
}