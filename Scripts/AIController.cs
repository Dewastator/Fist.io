using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIController : MonoBehaviour
{
    [SerializeField]
    float health = 3f,maxHealth = 3f;

    [SerializeField]
    Material hitMaterial;
    Material originalMaterial;
    [SerializeField]
    Image healthBar;
    [SerializeField]
    GameObject laser, lifeParticle;
    [SerializeField]
    Transform firePoint;
    public NavMeshAgent agent;
    [SerializeField]
    float turnSpeed = 8f;
    float angle;
    public Transform enemy;
    float timeSinceFired = 0, rate = 0.5f;
    float time, t;
    public int attackDamage = 1;
    [SerializeField]
    AnimationCurve curve;
    [SerializeField]
    Transform hands;
    [SerializeField]
    SphereCollider hitTrigger;
    public bool isPunching;
    public int experience;
    public Vector3 dest;
    public bool isInParticles;
    public float wanderRadius = 20f;
    public float wanderTimer = 1f;
    float timer;
    Transform particleTarget;
    bool wander, enemyFound;
    public bool positionSet;
    public Transform pos;
    int levelBorder = 15;
    Vector3 originalScale;
    public SphereCollider rangeTrigger;
    public GameObject waypointPrefab;
    Queue<Vector3> waypoints = new Queue<Vector3>();
    bool isColiding, isHitted;
    public bool isFleeing = false;
    int enemyHealth;
    public int level = 1, enemyLevel;
    public LayerMask collisionLayers;
    int levelCounter = 1;
    int fleeNumber;
    // Start is called before the first frame update
    void Start()
    {
        originalMaterial = GetComponentInChildren<MeshRenderer>().material;
        originalScale = transform.localScale;
        agent = GetComponent<NavMeshAgent>();
        rate = Random.Range(0.5f, 0.6f);
        dest = transform.position;
        fleeNumber = Random.Range(1, 2);
    }

    // Update is called once per frame
    void Update()
    {
        isColiding = false;
        GetComponent<Expirience>().exp = experience;
        Leaderboard.Instance.UpdateExp(transform);
        if (enemy != null && enemy.gameObject.activeSelf)
        {
            if (agent.enabled && !isFleeing)
                agent.SetDestination(enemy.position);
            var myPos = new Vector2(transform.position.x, transform.position.z);
            var ePos = new Vector2(enemy.position.x, enemy.position.z);
            if (enemy.gameObject.layer == 7)
            {
                enemyHealth = enemy.GetComponent<Player>().MyHealth;
                enemyLevel = enemy.GetComponent<Player>().level;
            }
            else
            {
                enemyHealth = (int)enemy.GetComponent<AIController>().health;
                enemyLevel = enemy.GetComponent<AIController>().level;

            }
            //if (Vector2.Distance(myPos, ePos) > rangeTrigger.radius + 20f || !enemy.gameObject.activeSelf)
            //{
            //    SetPosition();
            //    return;
            //}
            FleeWhenLowHealth();
            FleeFromBiggerEnemy();
            if (Vector2.Distance(myPos, ePos) < 2f + enemy.localScale.x / 2)
            {
                if (!isFleeing)
                    agent.enabled = false;
                if (Time.time > rate + timeSinceFired)
                {
                    isPunching = true;
                    rate = Random.Range(0.5f, 1f);
                    timeSinceFired = Time.time;

                }

                var dir = (enemy.position - transform.position).normalized;
                var targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed);
                transform.eulerAngles = Vector3.up * angle;
            }
            else
            {
                agent.enabled = true;
            }
        }
        //else if (particleTarget != null)
        //{
        //    PickupLifeParticle();
        //}
        else
        {
            agent.enabled = true;
            SetPosition();
        }
       
        if (isPunching)
        {
            Fire();
        }

        if (experience >= levelBorder)
        {
            LevelUp();
            levelBorder += levelBorder;
        }
    }

    private void PickupLifeParticle()
    {
        agent.enabled = true;
        if (waypoints.Count > 0)
        {
            var next = waypoints.Peek();
            Debug.Log(next);
            agent.SetDestination(next);
            if (transform.position.x == next.x)
                waypoints.Dequeue();

        }
    }

    private void FleeFromBiggerEnemy()
    {
        if (level + 2 < enemyLevel && enemyHealth > 1)
        {
            SetPosition();
        }
       
    }

    private void FleeWhenLowHealth()
    {
        if (health < fleeNumber && enemyHealth > health)
        {
            isFleeing = true;
            agent.enabled = true;
            var dirToEnemy = transform.position - enemy.position;
            var newPos = transform.position + dirToEnemy;
            agent.SetDestination(newPos);
        }
        else if (health == maxHealth)
        {
            isFleeing = false;
        }
    }

    private void LevelUp()
    {
        StartCoroutine("LevelUpAnimation");
        level++;
    }
 

    private void SetPosition()
    {
        if (pos != null && !pos.gameObject.activeSelf)
            positionSet = false;    
        if (!positionSet)
        {
            agent.enabled = true;
            pos = Positions.Instance.GetPosition(transform);
            positionSet = true;
            enemy = pos;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
    public void Fire()
    {
        time += Time.deltaTime * 10f;
        t = curve.Evaluate(time);
        hands.localEulerAngles = Vector3.right * t * 10f;
        if (t >= 0.8f)
        {
            hitTrigger.enabled = true;
        }
        if (t >= 1)
        {
            isPunching = false;
            hitTrigger.enabled = false;
            hands.localEulerAngles -= Vector3.right * 130f;
            time = 0;
            t = 0;
        }
    }

    private void Die()
    {
        Instantiate(lifeParticle, transform.position, lifeParticle.transform.rotation);
        gameObject.SetActive(false);
        Invoke("Reborn", 2f);
        healthBar.fillAmount = 1;
        experience = 0;
        levelBorder = 15;
        agent.speed = 15;
        maxHealth = 3f;
        health = maxHealth;
        transform.localScale = originalScale;
        level = 1;
        attackDamage = 1;
        fleeNumber = Random.Range(1, 2);
        StopAllCoroutines();
    }

    private void Reborn()
    {
        transform.position = Positions.Instance.respawnPoints[Random.Range(0, Positions.Instance.respawnPoints.Count - 1)].position;
        gameObject.SetActive(true);
        GetComponentInChildren<MeshRenderer>().material = originalMaterial;

    }
    private IEnumerator LevelUpAnimation()
    {
        var wantedSize = transform.localScale * 1.2f;
        float elapsedTime = 0;
        float waitTime = 1f;
        maxHealth += 3;
        health = maxHealth;
        healthBar.fillAmount = 1;
        agent.speed -= 1f;
        fleeNumber = Random.Range(1, 3);
        if (levelCounter + 3 == level)
        {
            attackDamage++;
            levelCounter += 3;
        }
        while (elapsedTime < waitTime)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, wantedSize, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    [DebugUtilities.DebugPanelAction("AI/TakeDamage")]
    public void TakeDamage(int amount)
    {
        health -= amount;
        isHitted = true;
        StartCoroutine("HitAnimation");
        StartCoroutine(HealthBarDecrease(0.05f));

        if (health <= 0)
        {
            Die();
        }
    }

    IEnumerator HitAnimation()
    {
        GetComponentInChildren<MeshRenderer>().material = hitMaterial;
        yield return new WaitForSeconds(0.13f);
        GetComponentInChildren<MeshRenderer>().material = originalMaterial;

    }
    IEnumerator HealthBarDecrease(float amount)
    {
        yield return new WaitForSeconds(0.01f);
        var targetHealth = health / maxHealth;
        healthBar.fillAmount -= amount;
        if(healthBar.fillAmount > targetHealth)
        {
            StartCoroutine(HealthBarDecrease(0.05f));
        }
        else
        {
            isHitted = false;
            StartCoroutine(InitIncrease());

        }
    }
    IEnumerator InitIncrease()
    {
        float t = 0;
        while(t < 2.5f)
        {
            if (isHitted)
                yield break;
            t += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(HealthBarIncrease());
    }
    IEnumerator HealthBarIncrease()
    {
        float current = healthBar.fillAmount;
        float next = 1f;
        float duration = 2f;
        float start = current;
        float t = 0;

        while (t < 1f)
        {
            if (isHitted)
                yield break;
            if(t > health / maxHealth)
            {
                health++;
            }

            t += Time.deltaTime * duration;
            healthBar.fillAmount = Mathf.Lerp(start, next, t);
            yield return null;
        }
        health = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7 || other.gameObject.layer == 10)
        {
            enemy = other.transform;
            positionSet = false;
        }
        if (other.gameObject.layer == 14)
        {
            if (isColiding)
                return;
            isColiding = true;
            GetPointsInCircle();
            particleTarget = other.transform;
        }
    }
        
    private void GetPointsInCircle()
    {
        for (int i = 0; i < 4; i++)
        {
            float angle = i * Mathf.PI * 2 / 4;
            float x = Mathf.Cos(angle) * 5;
            float z = Mathf.Sin(angle) * 5;
            var pos = new Vector3(x, transform.position.y, z);
            waypoints.Enqueue(pos);
        }
    }

   
}
