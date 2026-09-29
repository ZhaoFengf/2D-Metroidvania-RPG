using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StateGraph", menuName = "Graph/State Graph")]
public class StateGraphAsset : ScriptableObject
{
    public List<StateNodeData> Nodes = new();
    public List<TransitionData> Transitions = new();
}