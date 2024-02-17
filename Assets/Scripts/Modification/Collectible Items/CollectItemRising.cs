using TMPro;
using UnityEngine;

public class CollectItemRising : MonoBehaviour
{
    [SerializeField] float riseSpeed = 1f;
    [SerializeField] float riseHeight = 1f;
    [SerializeField] float lifeTime = 2f;
    [SerializeField] int amount;
    [SerializeField] TextMeshPro textAmount;


    private void Start()
    {
        textAmount.text = $"+{amount}";
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
    }
}
