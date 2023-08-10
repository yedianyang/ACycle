using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumrollParticlePool : MonoBehaviour
{
    public static DrumrollParticlePool SharedInstance;
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
            tmp.transform.position = new Vector3(0f, CycleConductor.instance.radius, 0f);
            tmp.SetActive(false);
            pooledParticles.Add(tmp);
        }
    }
    void Update()
    {
        // Move player by 90 degrees every beat
        transform.eulerAngles = new Vector3(0f, 0f, -CycleConductor.instance.songPositionInBeats * 90f);
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
