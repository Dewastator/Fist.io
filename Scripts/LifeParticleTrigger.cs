using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeParticleTrigger : MonoBehaviour
{
    public ParticleSystem ps;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    int collectedLife=0;
    ParticleSystemForceField detectedForceField;
    Player player;
    AIController ai;
    List<ParticleSystemForceField> forceFields = new List<ParticleSystemForceField>();
    // Use this for initialization
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        Invoke("EnableExternalForces", 1f);
    }
    void EnableExternalForces()
    {
        foreach (var item in forceFields)
        {
            ps.externalForces.AddInfluence(item);
        }
    }

    void OnParticleTrigger()
    {
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter, out var enterData);

        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            var go = enterData.GetCollider(i, 0).gameObject;
            if (go.layer == 10)
            {
                go.GetComponent<AIController>().experience++;
            }
            else if (go.layer == 7)
            {
                go.GetComponent<Player>().experience++;
            }
            enter[i] = p;
        }
        
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            player = other.GetComponent<Player>();
            ps.trigger.AddCollider(other.transform);
            detectedForceField = other.GetComponentInChildren<ParticleSystemForceField>();
            if (!forceFields.Contains(detectedForceField))
                forceFields.Add(detectedForceField);
            else
                ps.externalForces.AddInfluence(detectedForceField);

        }
        if (other.gameObject.layer == 10)
        {
            ai = other.GetComponent<AIController>();
            ps.trigger.AddCollider(other.transform);
            detectedForceField = other.GetComponentInChildren<ParticleSystemForceField>();
            if (!forceFields.Contains(detectedForceField))
                forceFields.Add(detectedForceField);
            else
                ps.externalForces.AddInfluence(detectedForceField);
        }
    }
}
