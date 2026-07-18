using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Effect")]
public class ItemEffect : ScriptableObject
{
    [TextArea]
    public string efftecDescription;

    public virtual void ExecuteEffect(Transform _enemyPosition)
    {
        Debug.Log("Executing item effect");
        // Implement the effect logic here
    }
}
