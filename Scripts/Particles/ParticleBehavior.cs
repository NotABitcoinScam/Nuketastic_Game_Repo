using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehavior : MonoBehaviour
{

    public GameObject particleParent = null;
    public GameObject selfObject = null;

    private float rotIncrement;
    private Vector3 randomVelocity;
    private float sizeMultiplier = 0.999f;

    // Start is called before the first frame update
    void Start()
    {
        randomVelocity = new Vector3 (Random.Range(-1f,1f), Random.Range(-1f, 1f), 0);
        rotIncrement = Random.Range(-20f,20f);
    }

    // Update is called once per frame
    void Update()
    {
        selfObject.transform.position += randomVelocity * Time.deltaTime;
        selfObject.transform.rotation = new Quaternion(selfObject.transform.rotation.x, selfObject.transform.rotation.y, selfObject.transform.rotation.z + rotIncrement * Time.deltaTime, 1);
        selfObject.transform.localScale = new Vector3(selfObject.transform.localScale.x * sizeMultiplier, selfObject.transform.localScale.y * sizeMultiplier, selfObject.transform.localScale.z * sizeMultiplier);
    }
}
