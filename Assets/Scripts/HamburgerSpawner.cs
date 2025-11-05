using UnityEngine;
using System.Collections;
public class HamburgerSpawner : MonoBehaviour
{
    public GameObject hamburger;
    public float speed = 1;
    public int qualityUpgradeLevel;
    public int speedUpgradeLevel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        StartCoroutine(SpawnHamburgers());
    }

     

    private IEnumerator SpawnHamburgers()
    {
        yield return new WaitForSeconds(speed);

        while (true)
        {
            Vector3 selfPos = transform.position;
            Quaternion selfRot = transform.rotation;
            GameObject newHam = Instantiate(hamburger, selfPos, selfRot);
            newHam.GetComponent<Rigidbody>().AddForce(-transform.forward * 50 * speed);
            yield return new WaitForSeconds(1 / speed);
        }
    }
}
