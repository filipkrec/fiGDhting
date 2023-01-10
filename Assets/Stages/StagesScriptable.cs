using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stages", menuName = "ScriptableObjects/Stages", order = 2)]
public class StagesScriptable : ScriptableObject
{
    public List<Sprite> stages;
}
