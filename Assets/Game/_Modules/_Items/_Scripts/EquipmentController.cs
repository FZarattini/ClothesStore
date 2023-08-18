using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    [Title("References")]
    [SerializeField] protected Transform _hatAnchor;
    [SerializeField] protected Transform _bodyAnchor;

    public Transform HatAnchor => _hatAnchor;
    public Transform BodyAnchor => _bodyAnchor;
}
