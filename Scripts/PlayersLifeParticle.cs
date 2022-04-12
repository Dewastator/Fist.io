using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersLifeParticle : MonoBehaviour
{
    public ParticleSystem ps;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> outside = new List<ParticleSystem.Particle>();
    int collectedLife = 0;
    ParticleSystemForceField detectedForceField;
    AIController ai;
    bool destSetted;
    ParticleSystem.Particle currentP;
    // Use this for initialization
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        Invoke("EnableExternalForces", 1f);
    }
    void EnableExternalForces()
    {
        ps.externalForces.AddInfluence(detectedForceField);
    }

    void OnParticleTrigger()
    {

        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        int numOutside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Outside, outside);


        for (int i = 0; i < numEnter; i++)
        {
            ai.experience++;
            ParticleSystem.Particle p = enter[i];
            enter[i] = p;
        }
        

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Outside, outside);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            ai = other.GetComponent<AIController>();
            ps.trigger.AddCollider(other.transform);
            detectedForceField = other.GetComponentInChildren<ParticleSystemForceField>();
        }
    }
}
