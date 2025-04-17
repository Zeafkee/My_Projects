
using UnityEngine;

public class DiceSide : MonoBehaviour
{
    public int sideValue;
    public bool onGround;

    public DiceManager diceManager;
    public GameManager gameManager;

    private void Start()
    {
        gameManager = diceManager.gameManager;
    }
    private void OnTriggerEnter(Collider other)
    {    
        if (other.tag=="Ground")
        {
            if (gameManager.isServer)
            {
                diceManager.SleepingModeOn(this);
            }                      
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ground")
        {
            onGround = true;
            Debug.Log("stay true");
        }
    }
    /*public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
            onGround = false;

    }*/
}
