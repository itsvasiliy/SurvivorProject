using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerNearDetector_ForTrees : MonoBehaviour
{
    [SerializeField] MMWiggle[] _MMWiggle;
    private bool isPlayerNear = false;

    private void Start() => ChangeMMWigleState(isPlayerNear);


    private void OnTriggerStay(Collider other)
    {
        if (isPlayerNear)
            return;

        isPlayerNear = true;
        if (other.tag == "Player")
            ChangeMMWigleState(isPlayerNear);
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerNear = false;
        ChangeMMWigleState(isPlayerNear);
    }


    private void ChangeMMWigleState(bool value)
    {
        foreach (var m in _MMWiggle)
            m.enabled = value;
    }
}
