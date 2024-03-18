using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerNearDetector_ForTrees : MonoBehaviour
{
    [SerializeField] MMWiggle _MMWiggle;
    private bool isPlayerNear = false;

    private void Start() => TurnOffMMWiggle();

    private void OnTriggerStay(Collider other)
    {
        if (isPlayerNear)
            return;

        if (other.tag == "Player")
            TurnOnMMWiggle();
    }

    private void OnTriggerExit(Collider other) => TurnOffMMWiggle();

    private void TurnOffMMWiggle() => _MMWiggle.enabled = false;

    private void TurnOnMMWiggle() => _MMWiggle.enabled = true;

}
