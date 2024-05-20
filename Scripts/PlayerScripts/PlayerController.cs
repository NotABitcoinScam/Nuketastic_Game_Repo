using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public GameObject MainCamera = null;
    public GameObject FuelSlider;

    public float Thrust = 0;
    public float AngularThrust = 0;

    public Rigidbody2D rigidBody;

    public float turnInp;
    public float thrustInp;

    public GameObject fireAnim = null;

    public GameObject particleobj = null;
    public GameObject blackparticleobj = null;
    public GameObject particleobjparent = null;

    int counter = 0;

    private bool outOfFuel = false;

// Start is called before the first frame update
void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        float zoomVal = Mathf.Clamp((8 + 5 * (Mathf.Log(rigidBody.velocity.magnitude, 10))),10,100);
        
        MainCamera.transform.position = new Vector3(rigidBody.transform.position.x, rigidBody.transform.position.y, -zoomVal);
        
    }

    void FixedUpdate()
    {
        


        

        counter++;
        turnInp = Input.GetAxis("Horizontal");
        thrustInp = Input.GetAxis("Vertical");

        if (thrustInp > 0 && !outOfFuel)
        {

            if (Random.Range(1,3) == 1)
            {
                GameObject newParticle = Instantiate(particleobj);
                newParticle.transform.position = fireAnim.transform.position;
                float scale = Random.Range(0.75f,1.25f);
                newParticle.transform.localScale = new Vector3(scale,scale,1);
                particleobj.transform.SetParent(particleobjparent.transform);

                Destroy(newParticle, 5.0f);
            }

            rigidBody.AddForce((transform.up * thrustInp * Thrust * Time.deltaTime));

            if (counter % 5 == 0)
            {
                if (counter % 2 == 0)
                {
                    fireAnim.SetActive(true);
                }
                else
                {
                    fireAnim.SetActive(true);
                }
            }
        } else
        {
            fireAnim.SetActive(false);
        }

        Slider s = FuelSlider.transform.GetComponent<Slider>();
        if (s.value <= 0)
        {
            outOfFuel = true;
        }
        else
        {
            outOfFuel = false;
        }

        if (outOfFuel)
        {
            if (Random.Range(1, 3) == 1)
            {
                GameObject newParticle = Instantiate(blackparticleobj);
                newParticle.transform.position = fireAnim.transform.position;
                float scale = Random.Range(0.75f, 1.25f);
                newParticle.transform.localScale = new Vector3(scale, scale, 1);
                particleobj.transform.SetParent(particleobjparent.transform);

                Destroy(newParticle, 2.5f);
            }
        }

        //Debug.Log(thrustInp);
        rigidBody.AddTorque((-turnInp * AngularThrust * Time.deltaTime));

    }
}
