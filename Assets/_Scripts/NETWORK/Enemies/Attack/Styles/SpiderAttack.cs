using System.Collections;
using UnityEngine;

public class SpiderAttack : MonoBehaviour
{
    [Header("Spider parameters")]

    [SerializeField] private Animator _animator;

    [SerializeField] private AnimationClip attackAnimClip;

    [SerializeField] private Transform bitingOrigin;

    [SerializeField] private Vector3 bitigZoneSize;

    [SerializeField] private MonoBehaviour movementScript;

    [SerializeField] private int spiderDamage;

    private float checkForPlayerRate = 0.3f;

    private void Start()
    {
        StartCoroutine(PlayerDetecting());   
    }

    private IEnumerator PlayerDetecting()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapBox(bitingOrigin.position, bitigZoneSize, Quaternion.identity);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<PlayerHealthController>(out PlayerHealthController playerHealth))
                {
                    _animator.SetTrigger("IsAttacking");
                    playerHealth.GetDamage(spiderDamage);

                    yield return new WaitForSeconds(attackAnimClip.length);
                }
            }

            yield return new WaitForSeconds(checkForPlayerRate);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(bitingOrigin.position, bitigZoneSize);
    }
}
