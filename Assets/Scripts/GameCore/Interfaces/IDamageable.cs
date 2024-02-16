interface IDamageable
{
    public void GetDamage();

    public void Dead();
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class IDamageableDetectionCube : MonoBehaviour
//{
//    [SerializeField] private Transform detectionCubeCentre;

//    [Range(0, 10)] public float detectionRange;

//    public Color gizmoColor = Color.red;

//    private void Update()
//    {
//        Vector3 playerForward = transform.forward;

//        RaycastHit[] hits = Physics.BoxCastAll(transform.position, Vector3.one * 0.5f, playerForward, transform.rotation, detectionRange);

//        foreach (RaycastHit hit in hits)
//        {
//            if (hit.collider != null)
//            {
//                if (hit.collider.TryGetComponent<IDamageable>(out IDamageable _damageable))
//                {
//                    Debug.Log("Bot detected: " + hit.collider.gameObject.name);
//                }
//            }
//        }
//    }

//    private void OnDrawGizmosSelected()
//    {
//        Gizmos.color = gizmoColor;
//        Gizmos.DrawCube(detectionCubeCentre.position, Vector3.one * detectionRange);
//    }
//}
