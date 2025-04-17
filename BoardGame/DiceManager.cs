
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static DiceManager instance;
    public int DiceDeger;
    public Rigidbody rb;
    public bool hasLanded;
    public bool GroundTrigger;
    public bool thrown;
    public bool thrown2;
    public bool thrown3;
    Vector3 initposition;
    public int diceValue;
    public DiceSide[] diceSides;
    public Quaternion initrotatiton;
    public float initialForce;
    public float forceDecayRate;
    private float currentForce;
    public float Force;
    public GameManager gameManager;

    public float Yonx, Yony, Yonz;

    public DicesManager DicesMainManager;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (gameManager.isServer)
            {
                GroundTrigger = true;
                //rb.useGravity = true;
                if(!hasLanded)
                {
                    rb.velocity = Vector3.zero;
                }             
            }                            
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initposition = transform.position;
        rb.useGravity = false;
        rb.isKinematic = false;
        initrotatiton = rb.rotation;
        currentForce = initialForce;
    }

    private void Update()
    {
        if (gameManager.isServer)
        {
            if (!thrown2)
            {
                transform.position = initposition;
                Quaternion randomRotation = Random.rotation;
                rb.rotation = randomRotation;
                transform.rotation = randomRotation;
                thrown3 = false;
                hasLanded = false;
                currentForce = initialForce;
                diceValue = 0;
                for (int i = 0; i < diceSides.Length; i++)
                {
                    diceSides[i].onGround = false;
                }
                GroundTrigger = false;
            }
            else
            {
                thrown3 = true;
            }
        }
    }
 
    public void SleepingModeOn(DiceSide side)
    {
        AlignDiceWithGround(side);
        hasLanded = true;
        Debug.Log(hasLanded + " sdhjgrthhejt");
        //rb.useGravity = false;
        GroundTrigger = true;
        //rb.isKinematic = true;
        //SideValueCheck();
    }
    public IEnumerator RollDice(float x, float y, float z)
    {
        if (gameManager.isServer)
        {
            if (!thrown && !hasLanded)
            {
                thrown2 = true;
                //Yonz = Random.Range(-3f, -8f);     
                while (!thrown3)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                rb.isKinematic = false;
                rb.useGravity = true;
                thrown = true;
                Yonx = Random.Range(1f, 4f);
                Yony = Random.Range(-7f, -10f);
                Yonz = -1 * Yonx;
                rb.AddTorque(x, y, z);
                //rb.AddForce(transform.forward * currentForce, ForceMode.Impulse);
            }
        }      
    }

    private void FixedUpdate()
    {
        if(gameManager.isServer)
        {
            if (thrown && !hasLanded)
            {
                if (!GroundTrigger)
                {
                    //currentForce -= forceDecayRate * Time.deltaTime;
                    //rb.AddForce(transform.forward * currentForce);
                    Vector3 vector3 = new Vector3(Yonx, Yony, Yonz);
                    rb.AddForce(vector3 * Force);
                }
            }
        }    
    }
    public void ResetDice()
    {
        thrown2 = false;         
        thrown = false;
        thrown3 = false;
        hasLanded = false;
        currentForce = initialForce;
        diceValue = 0;
        for (int i = 0; i < diceSides.Length; i++)
        {
            diceSides[i].onGround = false;
        }
        GroundTrigger = false;
    }  
    public void AlignDiceWithGround(DiceSide side)
    {
        int Deger = side.sideValue;
        diceValue = side.sideValue;
        DicesMainManager.DiceValueUpdate(diceValue, DiceDeger);
        //DicesMainManager.DicesStatusUpdate(this.gameObject, Deger);
        Debug.Log(Deger + "zar");
            if (Deger == 1)
            {
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);
                transform.rotation = targetRotation;
            Debug.Log("Calisti" + Deger);
            }
            else if (Deger == 2)
            {
                Quaternion targetRotation = Quaternion.Euler(-90f, 0f, 0f);
                transform.rotation = targetRotation;
            Debug.Log("Calisti" + Deger);
        }
            else if (Deger == 3)
            {
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, -90f);
                transform.rotation = targetRotation;
            Debug.Log("Calisti" + Deger);
        }
            else if (Deger == 4)
            {
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, 90f);
                transform.rotation = targetRotation;
            Debug.Log("Calisti" + Deger);
        }
            else if (Deger == 5)
            {
                Quaternion targetRotation = Quaternion.Euler(90f, 0f, 0f);
                transform.rotation = targetRotation;
            Debug.Log("Calisti" + Deger);
        }
            else if (Deger == 6)
            {
                Quaternion targetRotation = Quaternion.Euler(180f, 0f, 0f);
                transform.rotation = targetRotation;
            Debug.Log("Calisti" + Deger);
        }
        
        if (gameManager.isServer)
        {
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
        }
    }
}
