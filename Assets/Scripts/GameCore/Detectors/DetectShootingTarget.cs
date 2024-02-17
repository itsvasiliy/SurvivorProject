using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectShootingTarget : MonoBehaviour
{
    [SerializeField] private Transform centre;

    [Range(1, 100)] public float detectionRange;
}
