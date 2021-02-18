using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    /* Slime needs to be spawn directly on NavMesh or it gets errored and never moves
     * will fix later, need to get list of all Spawned grass blocks and spawn Slime on top
     * of those
     **/
    //public GameObject SlimeEnemy; 
    public GameObject BatEnemy;
    public GameObject RabbitEnemy;

    public int xPosMaxLimit;
    public int zPosMaxLimit;
    public int xPosMinLimit = 0;
    public int zPosMinLimit = 0;
    private List<GameObject> _spawnedEnemies;
    private List<GameObject> _enemyTypes;
    
    [SerializeField] private float _spawnLimit = 1.0f;
    [Range(0.0f , 1.0f)]
    [Tooltip("Rate of Increase for Spawn Limit")]
    [SerializeField] float spawnLimitIncreaseRate = 0.10f; 
    [Range(0.0f , 1.0f)]
    [Tooltip("How much the current day adds to the Spawn Limit")]
    [SerializeField] float dayWeight = 0.50f; 
    
    private void Start()
    {
        _spawnedEnemies = new List<GameObject>();
        _enemyTypes = new List<GameObject>();
        _enemyTypes.Add(BatEnemy);
        _enemyTypes.Add(RabbitEnemy);
        //_enemyTypes.Add(SlimeEnemy);
        DayCycleManager.instance.EnteringNight.AddListener(SpawnEnemy);
        DayCycleManager.instance.EnteringDay.AddListener(RemoveEnemies);
    }

    /// <summary>
    /// This Method is listening to an Event in the DayCycleManager
    /// When triggered it will check the current day and increase the spawn limit
    /// by a rate of increase plus the current multiplied by a weight.
    /// </summary>
    void SpawnEnemy()
    {
        int currentDay = DayCycleManager.instance.Day;

        _spawnLimit += (_spawnLimit * spawnLimitIncreaseRate) + (currentDay * dayWeight); 
        
        StartCoroutine(StartSpawner(_spawnLimit));
    }
    /// <summary>
    /// This Function is listening to an Event in the DayCycleManager
    /// When this is called it removes all the spawnedEnemies from the scene
    /// </summary>
    void RemoveEnemies()
    {
        if (_spawnedEnemies.Count <= 0) return; 
        
        foreach (var enemy in _spawnedEnemies)
        {
            Destroy(enemy);
        }
        _spawnedEnemies.TrimExcess();
    }
    /// <summary>
    /// This Coroutine will keep spawning enemies until it hits the spawnLimit
    /// All Enemies are added to a List to be referenced later
    /// </summary>
    /// <param name="spawnLimit"></param>
    /// <returns></returns>
    IEnumerator StartSpawner(float spawnLimit)
    {
        //TO-DO Add new enemy types as the days increase current picking Random
        for (int i = 0; i < (int)spawnLimit; i++)
        {
            GameObject enemy = _enemyTypes[UnityEngine.Random.Range(0, _enemyTypes.Count)];
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(xPosMinLimit,xPosMaxLimit), 10, UnityEngine.Random.Range(zPosMinLimit, zPosMaxLimit));
            var spawnerTransform = transform;
            GameObject spawnedEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity, spawnerTransform);
            _spawnedEnemies.Add((spawnedEnemy));
            yield return new WaitForSeconds(1.0f);
        }
    }
}
