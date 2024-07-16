using UnityEngine;

public class PlayerBehaviourComponent : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 moveInput;
    private Vector2 moveVelocity;
    private GlobalContainer _gc;
    private LineRenderer _lr;
    public GameObject bulletPrefab;
    private Transform bulletSpawnPoint;
    public LayerMask enemyLayer;

    private float shootTimer;
    private int lineSegs;

    void Start()
    {
        #region Inits

        _gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GlobalContainer>();
        _rb = GetComponent<Rigidbody2D>();
        _lr = GetComponent<LineRenderer>();

        #endregion
        
        lineSegs = 64;

        _gc.playerHealth = _gc.playerHealthMax;
    }

    void Update()
    {
        #region Player movement

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        moveInput = new Vector2(moveX, moveY).normalized;
        moveVelocity = moveInput * _gc.playerMoveSpeed;

        #endregion

        #region Player aiming

        DrawShootRange();

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0)
        {
            ShootNearestEnemy();
            shootTimer = _gc.playerShootSpeed;
        }

        #endregion
    }

    void FixedUpdate()
    {
        Vector2 newPosition = _rb.position + moveVelocity * Time.fixedDeltaTime;

        Vector2 clampedPosition = ClampToCamera(newPosition);
        _rb.MovePosition(clampedPosition);
    }

    Vector2 ClampToCamera(Vector2 targetPosition)
    {
        Vector2 minScreenBounds = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 maxScreenBounds = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        float clampedX = Mathf.Clamp(targetPosition.x, minScreenBounds.x, maxScreenBounds.x);
        float clampedY = Mathf.Clamp(targetPosition.y, minScreenBounds.y, maxScreenBounds.y);

        return new Vector2(clampedX, clampedY);
    }

    void ShootNearestEnemy()
    {
        Collider2D[] enemiesInRange =
            Physics2D.OverlapCircleAll(gameObject.transform.position, _gc.playerShootRange / 2, enemyLayer);
        bulletSpawnPoint = gameObject.transform;

        if (enemiesInRange.Length == 0)
            return;

        Collider2D nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider2D enemy in enemiesInRange)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<Bullet>().SetTarget(nearestEnemy.transform);
        }
    }

    void DrawShootRange()
    {
        _lr.positionCount = lineSegs + 1;
        _lr.useWorldSpace = false;

        float angle = 20f;

        for (int i = 0; i < lineSegs + 1; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * _gc.playerShootRange;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * _gc.playerShootRange;

            _lr.SetPosition(i, new Vector3(x, y, 0));
            angle += (360f / lineSegs);
        }
    }

    public void GetDamage(int dmg)
    {
        _gc.playerHealth -= dmg;
        if (_gc.playerHealth <= 0)
        {
            Destroy(gameObject);
            _gc.ShowLoseScreen();
        }
    }
}