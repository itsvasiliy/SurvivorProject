using UnityEngine;

public class ObjectRandomToggle : MonoBehaviour
{
    public GameObject objectToToggle; // Объект, который мы хотим включить или выключить

    private float toggleDelay; 
    private float nextToggleTime; 

    private bool isObjectActive = false; 

    private void Start()
    {
       
        nextToggleTime = Time.time + Random.Range(1f, 5f); 
    }

    private void Update()
    {
      
        if (Time.time >= nextToggleTime)
        {
           
            isObjectActive = !isObjectActive;
        
            objectToToggle.SetActive(isObjectActive);
            
            toggleDelay = Random.Range(10f, 120f); 
            nextToggleTime = Time.time + toggleDelay;
        }
    }
}
