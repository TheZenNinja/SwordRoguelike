using CombatSystem;
using ContextSteering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct WaveData
    {
        public float gameTime;
        public float spawnDelay;
    }
    public InputActionReference pauseKey;

    public Player player;
    public Entity enemy;
    public int currentEnemies = 0;
    public int maxEnemies = 50;
    public WaveData[] waves;
    public float secondsTillNextSpawn = 0;
    public Vector2 spawnArea;
    public float avoidPlayerRadius = 6;
    public GameObject spawnParticle;

    [Range(0, 1)]
    public float weaponDropChance;
    public WeaponData[] weaponTemplates;
    public WeaponItem weaponDropPrefab;
    //TODO impliment pause
    //public bool paused;
    //public GameObject pauseScreen;

    private void Start()
    {
        currentEnemies = 0;
        //paused = false;
        pauseKey.action.performed += ctx => TogglePause();
        secondsTillNextSpawn = 0;
    }

    private void FixedUpdate()
    {
        if (secondsTillNextSpawn <= 0)
            SpawnEnemy();
        if (secondsTillNextSpawn > 0)
            secondsTillNextSpawn -= Time.fixedDeltaTime;
    }

    private void SpawnEnemy()
    {
        WaveData currentWave = waves[0];

        foreach (var w in waves)
        {
            if (w.gameTime > Time.time)
                break;
            currentWave = w;
        }

        secondsTillNextSpawn = currentWave.spawnDelay;

        if (currentEnemies >= maxEnemies)
            return;

        var spawnPos = GetSpawnPos();

        while (Vector3.Distance(spawnPos, player.transform.position) < avoidPlayerRadius)
            spawnPos = GetSpawnPos();

        currentEnemies++;

        var ai = Instantiate(enemy, spawnPos, Quaternion.identity);
        ai.GetComponent<EnemyAI>().player = player.transform;
        ai.onDieE += OnEnemyKill;
    }

    private Vector3 GetSpawnPos()
    {
        //rnd vector2
        var rnd = Random.insideUnitCircle;
        
        return transform.position + Vector2.Scale(rnd, spawnArea/2).ToV3();
    }

    private void TogglePause()
    {
        SceneManager.LoadScene("MainMenu");

        //paused = !paused;
        //Time.timeScale = paused ? 1f : 0f;
    }
    public void OnPlayerDeath()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnEnemyKill(Entity entity)
    {
        currentEnemies--;
        var posToSpawn = entity.position;

        if (weaponDropChance > UnityEngine.Random.value)
            DropWeapon(posToSpawn);

    }

    private void DropWeapon(Vector2 posToSpawn)
    {
        var template = weaponTemplates[UnityEngine.Random.Range(0, weaponTemplates.Length - 1)];
        var wepInstance = WeaponGenerator.GenerateWeapon(template);

        var drop = Instantiate(weaponDropPrefab, posToSpawn, Quaternion.identity);
        drop.weapon = wepInstance;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnArea.ToV3());
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(player.transform.position, avoidPlayerRadius);

    }
}
