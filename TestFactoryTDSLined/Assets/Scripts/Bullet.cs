using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private Transform target;
    private Vector2 direction;
    private GlobalContainer _gc;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    void Start()
    {
        _gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GlobalContainer>();
        speed = _gc.bulletSpeed;
        direction = (target.position - transform.position);
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.Translate(direction * (speed * Time.deltaTime));
        
        // if (Vector2.Distance(transform.position, target.position) < 0.1f)
        // {
        //     Destroy(gameObject);
        //     
        // }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Entered bullet collider");
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            // target.GetComponent<EnemyBehaviourComponent>().GetDamage(_gc.playerBulletDamage);
        }
    }
}