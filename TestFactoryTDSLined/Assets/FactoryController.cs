using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class FactoryController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    public bool thisFactoryWillCreate = false;

    private GlobalContainer _gc;

    private Transform _t;
    // Start is called before the first frame update
    void Start()
    {
        _gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GlobalContainer>();
        _t = gameObject.transform;
        
        StartCoroutine(SpawnEnemies());

    }

    // Update is called once per frame
    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(_gc.spawnTimeout);
            _gc.GetRandomSpawnTimeout();

            if (_gc.canMakeFactory)
            {
                if (thisFactoryWillCreate)
                {
                    // _gc.GetRandomEnemySpd();
                    GameObject newEnemy = Instantiate(enemyPrefab, _t.position, _t.rotation);
                    newEnemy.GetComponent<EnemyBehaviourComponent>().speed = _gc.GetRandomEnemySpd();
                    thisFactoryWillCreate = false;
                }
            }
        }
    }

}
