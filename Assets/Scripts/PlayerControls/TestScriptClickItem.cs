using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptClickItem : MonoBehaviour
{
    [SerializeField]
    Transform   targetPosition;
    [SerializeField]
    Vector3     distanceToTarget;
    [SerializeField]
    bool        isPlaced;
    // Start is called before the first frame update
    void Start()
    {
        isPlaced = false;
        if (targetPosition)
        {
            distanceToTarget = targetPosition.position - this.transform.position;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
