using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    public static ParticlePool SharedInstance;
    public List<GameObject> pooledParticles;
    public GameObject particleObject;
    public int amountToPool;
    
    void Awake()
    {
        SharedInstance = this;
    }
    void Start()
    {
        pooledParticles = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(particleObject, transform);
            tmp.SetActive(false);
            pooledParticles.Add(tmp);
        }
    }

    public GameObject GetPooledParticle()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if(!pooledParticles[i].activeInHierarchy)
            {
                return pooledParticles[i];
            }
        }
        return null;
    }
}
