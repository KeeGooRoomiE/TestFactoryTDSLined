using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourComponent : MonoBehaviour
{
    private int health;
    public float speed;

    private GlobalContainer _gc;
    // Start is called before the first frame update
    void Start()
    {
        _gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GlobalContainer>();
        health = _gc.enemyHealth;
        speed = _gc.GetRandomEnemySpd();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * (speed * Time.deltaTime));
    }

    public void GetDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            Destroy(gameObject);
            _gc.enemiesToDefeat++;
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Entered enemy collider");
        
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
            if (GameObject.FindGameObjectWithTag("Player") == null) return;
            var p = GameObject.FindGameObjectWithTag("Player");
            Debug.Log("Enemy touches the wall");
            p.GetComponent<PlayerBehaviourComponent>().GetDamage(1);
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            GetDamage(_gc.playerBulletDamage);
        }
    }
}
