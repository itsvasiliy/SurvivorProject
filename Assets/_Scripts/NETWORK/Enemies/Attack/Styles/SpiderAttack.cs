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

    private void OnDisable() => StopAllCoroutines();

    private IEnumerator PlayerDetecting()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapBox(bitingOrigin.position, bitigZoneSize, Quaternion.identity);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<PlayerHealthHandlerForController>(out PlayerHealthHandlerForController playerHealth))
                {
                    if (playerHealth.IsAlive())
                    {
                        _animator.SetTrigger("IsAttacking");

                        // "For your mantal health" DO NOT READ THIS CODE
                        #region Confirming that player inside attack zone in a middle of attack animation

                        yield return new WaitForSeconds(attackAnimClip.length / 2f);

                        Collider[] colliders2 = Physics.OverlapBox(bitingOrigin.position, bitigZoneSize, Quaternion.identity);

                        foreach (Collider collider2 in colliders2)
                        {
                            if (collider2.TryGetComponent<PlayerHealthHandlerForController>(out PlayerHealthHandlerForController playerHealth2))
                            {
                                playerHealth.GetDamage(spiderDamage);
                            }
                        }

                        yield return new WaitForSeconds(attackAnimClip.length / 2f);

                        #endregion
                    }

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
