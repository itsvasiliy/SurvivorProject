using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TentPlayerRespawner : MonoBehaviour
{
    [SerializeField] Structure structureScrript;
    private static List<Vector3> tentPositionsList = new List<Vector3>();

    private void Start()
    {
        if (structureScrript.isViewing)
            return;

        tentPositionsList.Add(transform.position);
    }

    public static Vector3 GetLastTentPosition() => tentPositionsList.LastOrDefault();


    private void OnDestroy()
    {
        if (structureScrript.isViewing)
            return;

        var currentTent = tentPositionsList.Where(x => x == transform.position).First();
        tentPositionsList.Remove(currentTent);
    }

}
