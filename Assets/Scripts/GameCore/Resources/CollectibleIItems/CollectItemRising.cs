using TMPro;
using UnityEngine;

public class CollectItemRising : MonoBehaviour
{
    [SerializeField] float riseSpeed = 1f;
    [SerializeField] float riseHeight = 1f;
    [SerializeField] float lifeTime = 2f;
    [SerializeField] TextMeshPro textAmount;

    private int amount;


    private void Start() => Destroy(gameObject, lifeTime);
    public void SetAmount(int amount) => textAmount.text = $"+{amount}";
    void Update() => transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
}
