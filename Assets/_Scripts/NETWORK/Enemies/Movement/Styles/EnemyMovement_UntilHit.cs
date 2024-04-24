using UnityEngine;

public class EnemyMovement_UntilHit : EnemyMovement
{
    private int direction = 1;

    void Update()
    {
        transform.Translate(Vector3.right * direction * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            direction *= -1;
          
            float randomRotationY = Random.Range(-270f, 270f); 
            transform.Rotate(new Vector3(0f, randomRotationY, 0f));
        }
    }
}
