﻿using System.Collections;
using System.Collections.Generic;
using CustomUtilities;
using UnityEngine;
using UnityEngine.Events;

public class BreakableObject : MonoBehaviour, IDamageable
{
    public DamageType brokenByElements;
    public GameObject particlePrefab;
    public UnityEvent OnBreak;
    public UnityEvent OnFail;
    public bool recoilOnFail = true;
    DamageKnockback lastDamage;
    [Header("Drop Item")]
    public Item[] drops;
    public GameObject dropPrefab;
    public int dropPrefabAmount;
    public float forceMagnitude = 1f;
    public Vector3 angularVelocity;
    public void Recoil()
    {
        
    }

    public void StartCritVulnerability(float time)
    {
        
    }

    public void TakeDamage(DamageKnockback damage)
    {
        lastDamage = damage;
        if (damage.GetTypes().HasType(brokenByElements))
        {
            BreakObject();
            return;
        }
        if (recoilOnFail)
        {
            
            if (!damage.isRanged && damage.source.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.Recoil();   
            }
            damage.OnBlock.Invoke();
        }
    }

    public void BreakObject()
    {
        if (particlePrefab != null)
        {
            GameObject particle = Instantiate(particlePrefab);
            particle.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
            Destroy(particle, 5f);
        }
        DropItems();
        OnBreak.Invoke();
        Destroy(this.gameObject, 0.01f);
    }

    public void DropItems()
    {
        List<GameObject> droppedObjects = new List<GameObject>();

        foreach (Item item in drops)
        {
            LooseItem loose = LooseItem.CreateLooseItem(item);
            droppedObjects.Add(loose.gameObject);
        }
        for (int i = 0; i < dropPrefabAmount; i++)
        {
            droppedObjects.Add(Instantiate(dropPrefab));
        }

        foreach (GameObject droppedObject in droppedObjects)
        {
            droppedObject.transform.position = this.transform.position + Random.insideUnitSphere;
            if (droppedObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.velocity = Vector3.up * forceMagnitude;
                rigidbody.angularVelocity = angularVelocity;
                //rigidbody.AddExplosionForce(forceMagnitude, this.transform.position + Vector3.up * -0.5f, 1f);
            }
        }
    }
    public void SetHitParticlePosition(Vector3 position, Vector3 direction)
    {
        //throw new System.NotImplementedException();
    }

    public DamageKnockback GetLastTakenDamage()
    {
        return lastDamage;
    }
}