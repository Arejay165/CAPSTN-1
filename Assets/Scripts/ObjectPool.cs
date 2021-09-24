using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Start is called before the first frame update
    public static ObjectPool instances;
    public List<GameObject> pooledGameobjects = new List<GameObject>();
    public int amountToPool = 10;

    [SerializeField]
    private GameObject coinPrefab;

    public GameObject spawner;

    private Vector3 originSpawn;
    private void Awake()
    {
        if(instances == null)
        {
            instances = this;
        }
    }
    void Start()
    {
        for(int i =0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(coinPrefab);
            obj.SetActive(false);
            pooledGameobjects.Add(obj);
            RandomPosition();

        }
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0; i < pooledGameobjects.Count; i++)
        {
            if (!pooledGameobjects[i].activeInHierarchy)
            {
                return pooledGameobjects[i];
            }
        }

        return null;
    }

    public void RandomPosition()
    {
        float radius = 5f;
        originSpawn = spawner.gameObject.transform.position;
     

        for (int i = 0; i < pooledGameobjects.Count; i++)
        {
            originSpawn.x += Random.Range(-radius, radius);
            originSpawn.y += Random.Range(-radius, radius);
            pooledGameobjects[i].transform.position = new Vector3(originSpawn.x, originSpawn.y); 
        }
    }

    public void ResetPosition()
    {
        for (int i = 0; i < pooledGameobjects.Count; i++)
        {
           
            pooledGameobjects[i].transform.position = new Vector3(spawner.transform.position.x, spawner.transform.position.y);

        }
    }
}
