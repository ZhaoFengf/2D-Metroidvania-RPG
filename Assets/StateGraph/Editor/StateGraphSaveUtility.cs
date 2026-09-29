using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class StateGraphSaveUtility
{
    private StateGraphView _targetGraphView;

    private List<StateNodeView> Nodes => _targetGraphView.nodes.ToList().OfType<StateNodeView>().ToList();

    private List<Edge> Edges => _targetGraphView.edges.ToList();

    private StateGraphSaveUtility(StateGraphView graphView)
    {
        _targetGraphView = graphView;
    }

    public static StateGraphSaveUtility GetInstance(StateGraphView graphView)
    {
        return new StateGraphSaveUtility(graphView);
    }

    #region Save

    public void SaveGraph(string fileName)
    {
        var graphAsset = ScriptableObject.CreateInstance<StateGraphAsset>();

        SaveNodes(graphAsset);
        SaveTransitions(graphAsset);

        string path = $"Assets/{fileName}.asset";

        AssetDatabase.CreateAsset(graphAsset, path);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = graphAsset;
    }

    private void SaveNodes(StateGraphAsset graphAsset)
    {
        foreach (var node in Nodes)
        {
            if (node.StateId == "Entry")
                continue;

            var nodeData = new StateNodeData
            {
                Guid = node.Guid,
                StateId = node.StateId,
                Position = node.GetPosition().position
            };

            graphAsset.Nodes.Add(nodeData);
        }
    }

    private void SaveTransitions(StateGraphAsset graphAsset)
    {
        foreach (var edge in Edges)
        {
            var outputNode = edge.output.node as StateNodeView;
            var inputNode = edge.input.node as StateNodeView;
            if (outputNode == null || inputNode == null) continue;

            var transitionData = new TransitionData
            {
                Guid = System.Guid.NewGuid().ToString(),
                FromNodeGuid = outputNode.Guid,
                ToNodeGuid = inputNode.Guid
            };

            graphAsset.Transitions.Add(transitionData);
        }
    }
    #endregion

    #region Load
    public void LoadGraph(StateGraphAsset graphAsset)
    {
        ClearGraph();
        CreateNodes(graphAsset);
        CreateTransitions(graphAsset);
    }

    private void CreateNodes(StateGraphAsset graphAsset)
    {
        foreach (var nodeData in graphAsset.Nodes)
        {
            var node = _targetGraphView.CreateStateNode(nodeData.StateId, nodeData.Position);
            node.Guid = nodeData.Guid;
            _targetGraphView.AddElement(node);
        }
    }

    private void CreateTransitions(
        StateGraphAsset graphAsset)
    {
        foreach (var transition in graphAsset.Transitions)
        {
            var outputNode = Nodes.FirstOrDefault(node => node.Guid == transition.FromNodeGuid);

            var inputNode = Nodes.FirstOrDefault(node => node.Guid == transition.ToNodeGuid);

            if (outputNode == null || inputNode == null)
            {
                continue;
            }

            var outputPort = outputNode.outputContainer.Q<Port>();

            var inputPort = inputNode.inputContainer.Q<Port>();

            if (outputPort == null || inputPort == null)
            {
                continue;
            }

            var edge = new Edge
            {
                output = outputPort,
                input = inputPort
            };
            outputPort.Connect(edge);
            inputPort.Connect(edge);
            _targetGraphView.Add(edge);
        }
    }

    #endregion

    #region Clear

    private void ClearGraph()
    {
        var elementsToRemove = _targetGraphView.graphElements.ToList();

        _targetGraphView.DeleteElements(elementsToRemove);

        // Entry 重新创建
        _targetGraphView.AddElement(CreateEntryNode());
    }

    private StateNodeView CreateEntryNode()
    {
        var node = new StateNodeView("Entry");
        var outputPort = node.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));

        outputPort.portName = "Start";
        node.outputContainer.Add(outputPort);
        node.capabilities &= ~Capabilities.Deletable;
        node.capabilities &= ~Capabilities.Movable;
        node.capabilities &= ~Capabilities.Selectable;

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(100, 200, 150, 100));

        return node;
    }

    #endregion
}