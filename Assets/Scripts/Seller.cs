using UnityEngine;

public class Seller : MonoBehaviour
{
    private GameManager gameManager;
    public int value = 1;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hamburger"))
        {
            gameManager.hamCoins += value;
            Destroy(other.gameObject);
        }
    }
}
