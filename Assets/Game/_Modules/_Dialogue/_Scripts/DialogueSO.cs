using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue/New Dialogue", order = 1)]
public class DialogueSO : ScriptableObject
{
    public List<string> dialogueText;
    public float writeSpeed;
}
