using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Da_NUKE_One : MonoBehaviour
{

    // PLAYER DATA OBJECTS
    public GameObject playerCharacter;
    public GameObject explosionParent;
    public Rigidbody2D rigidBody;
    public GameObject Cam;
    public float explodeImpulseThreshold;
    public Vector3 respawnPos;

    // ANGER BAR
    public float Anger;
    public float AngerSpeed;
    public GameObject rageSlider;

    // FUEL BAR
    public float Fuel;
    public float FuelSpeed;
    public GameObject fuelSlider;

    // FOR CALCULATING DELTA-V
    private float xVelLastFrame;
    private float xVelThisFrame;
    private float yVelLastFrame;
    private float yVelThisFrame;
    private float amountMovedX;
    private float amountMovedY;
    private Vector2 delta;
    private float magnitude;

    // EXPLOSION EFFECT PARTICLE EMITTERS
    private ParticleSystem smokeEmitter;
    private ParticleSystem fireEmitter1;
    private ParticleSystem fireEmitter2;
    private ParticleSystem fireEmitter3;

    // ARE BARS EMPTY?
    private bool hasExploded;
    private bool outOfFuel;

    //SLOMO BOOL
    private bool sloMo;

    
    // ROTATION DEFS
    private float rotation;
    private float rotAfterOutOfFuel;
    private GameObject rocketParts;
    private GameObject face;
    private GameObject finsParent;
    private GameObject fin1;
    private GameObject fin2;
    private GameObject fin3;
    private GameObject fin4;
    private GameObject rocketBody;

    // ROTATION OFFSETS

    private Vector3 faceOffset;
    private Vector3 fin1Offset;
    private Vector3 fin2Offset;
    private Vector3 fin3Offset;



    // Start is called before the first frame update
    void Start()
    {
        xVelLastFrame = rigidBody.position.x;
        yVelLastFrame = rigidBody.position.y;

        explosionParent.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
        explosionParent.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Stop();
        hasExploded = false;
        outOfFuel = false;
        sloMo = false;

        rotation = 0f;
        rocketParts = playerCharacter.transform.GetChild(0).gameObject;
        face = rocketParts.transform.GetChild(0).gameObject;
        finsParent = rocketParts.transform.GetChild(1).gameObject;
        fin1 = finsParent.transform.GetChild(0).gameObject;
        fin2 = finsParent.transform.GetChild(1).gameObject;
        fin3 = finsParent.transform.GetChild(2).gameObject;
        fin4 = finsParent.transform.GetChild(3).gameObject;
        rocketBody = rocketParts.transform.GetChild(2).gameObject;

        faceOffset = face.transform.localPosition - rocketBody.transform.localPosition;
        fin3Offset = fin3.transform.localPosition - rocketBody.transform.localPosition;

    }

    IEnumerator CameraShake(float duration, float magnitude)
    {

        float zoomVal = Mathf.Clamp((8 + 5 * (Mathf.Log(rigidBody.velocity.magnitude, 10))), 10, 100);
        Vector3 originPos = new Vector3(rigidBody.transform.position.x, rigidBody.transform.position.y, -zoomVal);


        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {

            float tmpMagnitude = magnitude * (duration - elapsedTime);

            float xOffset = Random.Range(-0.5f, 0.5f) * tmpMagnitude;
            float yOffset = Random.Range(-0.5f, 0.5f) * tmpMagnitude;

            Cam.transform.position = new Vector3(originPos.x + xOffset, originPos.y + yOffset, -zoomVal);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }

    IEnumerator Explode()
    {
        if (hasExploded == false)
        {
            rotAfterOutOfFuel = Random.Range(-0.025f, 0.025f);
            sloMo = false;
            Time.timeScale = 1f;
            explosionParent.transform.position = rigidBody.transform.position;

            explosionParent.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
            explosionParent.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();

            explosionParent.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Stop();
            explosionParent.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();

            explosionParent.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>().Stop();
            explosionParent.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>().Play();

            explosionParent.transform.GetChild(3).gameObject.GetComponent<ParticleSystem>().Stop();
            explosionParent.transform.GetChild(3).gameObject.GetComponent<ParticleSystem>().Play();


            playerCharacter.SetActive(false);
            hasExploded = true;

            StartCoroutine(CameraShake(2, 0.5f));
            yield return new WaitForSeconds(3);

            Anger = 0;
            hasExploded = false;
            Fuel = 100;
            outOfFuel = false;
            updateFuel();
            playerCharacter.SetActive(true);

            rigidBody.transform.position = respawnPos;
            rigidBody.velocity = new Vector3(0, 0, 0);
            rigidBody.transform.rotation = new Quaternion(0, 0, 0, 1);

        }
    }
    // Update is called once per frame
    void Update()
    {

        xVelThisFrame = rigidBody.velocity.x;
        amountMovedX = xVelThisFrame - xVelLastFrame;
        xVelLastFrame = xVelThisFrame;


        yVelThisFrame = rigidBody.velocity.y;
        amountMovedY = yVelThisFrame - yVelLastFrame;
        yVelLastFrame = yVelThisFrame;

        delta = new Vector2(amountMovedX, amountMovedY);
        magnitude = delta.magnitude;

        if (magnitude >= explodeImpulseThreshold)
        {
            StartCoroutine(Explode());
        }
        if (Input.GetKeyDown("r"))
        {
            StartCoroutine(Explode());
        }
        if (Input.GetKey("q"))
        {
            if (sloMo)
            {
                sloMo = false;
                Time.timeScale = 1f;
            } else
            {
                sloMo = true;
                Time.timeScale = 0.33f;
            }
        }

        updateAnger();
        updateFuel();
        updateRotation();

    }

    void updateFuel()
    {
        if (!outOfFuel && !hasExploded)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                Fuel -= (FuelSpeed) * Time.deltaTime * Input.GetAxis("Vertical");
            }
            Slider s = fuelSlider.transform.GetComponent<Slider>();
            s.value = Fuel;
            if (Fuel <= 0)
            {
                outOfFuel = true;
            }
        }
    }


    void updateAnger()
    {
        if (Anger >= 100f)
        {
            StartCoroutine(Explode());
        }
        if (!hasExploded)
        {
            Anger += (AngerSpeed - (rigidBody.velocity.magnitude / 10)) * Time.deltaTime;
            Slider s = rageSlider.transform.GetComponent<Slider>();
            s.value = Anger;
        }
    }

    void updateRotation()
    {

        //ADD ROTATION
        if (outOfFuel)
        {
            rotation += rotAfterOutOfFuel;
        } else
        {
            rotation = -rigidBody.angularVelocity * 0.01f;
        }


        float multiplier = 0.4f;
        float multiplier2 = 0.5f;
        //FACE ROTATION
        face.transform.localPosition = rocketBody.transform.localPosition;
        face.transform.localPosition += new Vector3(Mathf.Sin(rotation) * multiplier, 0, -Mathf.Cos(rotation) * multiplier) + faceOffset;
        face.transform.localScale = new Vector3(Mathf.Cos(rotation) * 0.6f, 0.6f, 0);

        //FIN ROTATION

        //LEFT
        fin1.transform.localPosition = rocketBody.transform.localPosition;
        fin1.transform.localPosition += new Vector3(Mathf.Sin(rotation - Mathf.PI / 2) * multiplier2, 0, -Mathf.Cos(rotation - Mathf.PI / 2) * multiplier2) + fin3Offset;

        //RIGHT
        fin2.transform.localPosition = rocketBody.transform.localPosition;
        fin2.transform.localPosition += new Vector3(Mathf.Sin(rotation + Mathf.PI / 2) * multiplier2, 0, -Mathf.Cos(rotation + Mathf.PI / 2) * multiplier2) + fin3Offset;

        //FRONT
        fin3.transform.localPosition = rocketBody.transform.localPosition;
        fin3.transform.localPosition += new Vector3(Mathf.Sin(rotation) * multiplier2, 0, -Mathf.Cos(rotation) * multiplier2) + fin3Offset;
        
        //BACK
        fin4.transform.localPosition = rocketBody.transform.localPosition;
        fin4.transform.localPosition += new Vector3(Mathf.Sin(rotation + Mathf.PI) * multiplier2, 0, -Mathf.Cos(rotation + Mathf.PI) * multiplier2) + fin3Offset;
    }

}
