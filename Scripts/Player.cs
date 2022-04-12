using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject laser, lifeParticles;
    [SerializeField]
    SphereCollider hitTrigger;
    [SerializeField]
    Transform firePoint;
    [SerializeField]
    float maxHealth;
    [DebugUtilities.DebugPanelProperty("Player/Health")]
    public int MyHealth { get => (int)health; set => health = value; }
    float health = 3;
    public int attackDamage = 1;
    [SerializeField]
    Material hitMaterial;
    Material originalMaterial;
    [SerializeField]
    Image healthBar;
    [SerializeField]
    Transform hands;
    public bool isPunching;
    public float handsAngle;
    [SerializeField]
    AnimationCurve curve;
    public float t;
    float time;
    int levelBorder = 15;
    public int experience;
    bool isHitted;
    public float increaseTimer = 0;
    bool collidedWithAi;
    public int level = 1;
    Vector3 originalScale;
    int levelCounter = 1;
    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        health = maxHealth;
        originalMaterial = GetComponentInChildren<MeshRenderer>().material;
        handsAngle = -130f;
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Expirience>().exp = experience;
        Leaderboard.Instance.UpdateExp(transform);

        if (isPunching)
        {
            time += Time.deltaTime * 10f;
            t = curve.Evaluate(time);
            hands.localEulerAngles = Vector3.right * t * 10f;
            if(t >= 0.8f)
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
        
        if(experience >= levelBorder)
        {
            LevelUp();
            levelBorder += levelBorder;
        }
    }

    private void LevelUp()
    {
        StartCoroutine("LevelUpAnimation");
        level++;
    }
    private IEnumerator LevelUpAnimation()
    {
        var wantedSize = transform.localScale * 1.2f;
        float elapsedTime = 0;
        float waitTime = 1f;
        maxHealth += 3;
        health = maxHealth;
        healthBar.fillAmount = 1;
        GetComponent<PlayerMovement>().speed -= 1f;

        if (levelCounter + 2 == level)
        {
            attackDamage++;
            levelCounter += 2;
        }
        while(elapsedTime < waitTime)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, wantedSize, (elapsedTime/waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void Fire()
    {
        isPunching = true;
    }

    [DebugUtilities.DebugPanelAction("Player/TakeDamage")]
    public void TakeDamage(int amount)
    {
        health -= amount;
        increaseTimer += 2.5f;
        StartCoroutine("HitAnimation");
        StartCoroutine(HealthBarDecrease(0.05f));

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(lifeParticles, transform.position, lifeParticles.transform.rotation);
        gameObject.SetActive(false);
        Invoke("Reborn", 2f);
        healthBar.fillAmount = 1;
        experience = 0;
        levelBorder = 15;
        gameObject.GetComponent<PlayerMovement>().speed = 15;
        maxHealth = 3f;
        health = maxHealth;
        transform.localScale = originalScale;
        level = 1;
        attackDamage = 1;
        levelCounter = 1;
        StopAllCoroutines();
    }
    private void Reborn()
    {
        transform.position = Positions.Instance.respawnPoints[Random.Range(0, Positions.Instance.respawnPoints.Count - 1)].position;
        gameObject.SetActive(true);
        GetComponentInChildren<MeshRenderer>().material = originalMaterial;

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
        isHitted = true;
        if (healthBar.fillAmount > targetHealth)
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
        while (t < 2.5f)
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
        var currHealth = health;
        float start = current;
        float t = 0;

        while (t < 1f)
        {
            if (isHitted)
                yield break;
            if (t > health / maxHealth)
            {
                health++;
            }


            t += Time.deltaTime * duration;
            healthBar.fillAmount = Mathf.Lerp(start, next, t);

            yield return null;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 10)
        {
            collidedWithAi = true;
        }
    }
}
