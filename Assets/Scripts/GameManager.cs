using CombatSystem;
using ContextSteering;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct WaveData
    {
        public int enemiesKilled;
        public float spawnDelay;
        public int enemyHP;

        public static bool operator ==(WaveData x, WaveData y) => x.enemiesKilled == y.enemiesKilled;
        public static bool operator !=(WaveData x, WaveData y) => x.enemiesKilled != y.enemiesKilled;
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
    public GameObject spawnAndDeathParticle;

    [Range(0, 1)]
    public float weaponDropChance;
    public WeaponData[] weaponTemplates;
    public WeaponItem weaponDropPrefab;
    //TODO impliment pause
    //public bool paused;
    //public GameObject pauseScreen;

    public TextMeshProUGUI infoTxt;

    public int currentWave;
    public int killedEnemies;

    private void Start()
    {
        currentWave = 0;
        currentEnemies = 0;
        //paused = false;
        pauseKey.action.performed += ctx => TogglePause();
        secondsTillNextSpawn = 0;
        killedEnemies = 0;

        infoTxt.text = "Wave:\t0\nKills:\t0";
        player.GetComponent<WeaponController>().LoadWeapons(GenerateWeaponData());
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
        secondsTillNextSpawn = waves[currentWave].spawnDelay;

        if (currentEnemies >= maxEnemies)
            return;

        var spawnPos = GetSpawnPos();

        //prevents spawning too close to the player or in a wall
        while (Vector3.Distance(spawnPos, player.transform.position) < avoidPlayerRadius ||
            Physics2D.OverlapCircle(spawnPos, 1))
            spawnPos = GetSpawnPos();

        currentEnemies++;

        var ai = Instantiate(enemy, spawnPos, Quaternion.identity);
        ai.SetMaxHP(waves[currentWave].enemyHP);
        ai.GetComponent<EnemyAI>().player = player.transform;
        ai.onDieE += OnEnemyKill;

        Instantiate(spawnAndDeathParticle, spawnPos, Quaternion.identity);
    }

    private Vector3 GetSpawnPos()
    {
        //rnd vector2
        var rnd = Random.insideUnitCircle;

        return transform.position + Vector2.Scale(rnd, spawnArea / 2).ToV3();
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
        killedEnemies++;
        var posToSpawn = entity.position;

        Instantiate(spawnAndDeathParticle, posToSpawn, Quaternion.identity);

        if (weaponDropChance > Random.value)
            DropWeapon(posToSpawn);

        int newWave = 0;
        for (int i = 1; i < waves.Length; i++)
        {
            if (waves[i].enemiesKilled > killedEnemies)
                break;
            newWave = i;
        }

        if (currentWave !=  newWave)
            currentWave = newWave;

        infoTxt.text = $"Wave:\t{currentWave}\nKills:\t{killedEnemies}";
    }
    private WeaponInstance GenerateWeaponData()
    {
        var template = weaponTemplates[Random.Range(0, weaponTemplates.Length - 1)];
        return WeaponGenerator.GenerateWeapon(template);
    }
    private void DropWeapon(Vector2 posToSpawn)
    {
        var wepInstance = GenerateWeaponData();

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
