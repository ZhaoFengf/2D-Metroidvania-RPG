using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class StateGraphView : GraphView
{
    public readonly Vector2 DefaultNodeSize = new Vector2(200, 150);

    private StateNodeSearchWindow _searchWindow;

    public StateGraphView(EditorWindow editorWindow)
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        AddElement(GenerateEntryPointNode());

        AddSearchWindow(editorWindow);
    }

    #region Search Window
    private void AddSearchWindow(EditorWindow editorWindow)
    {
        _searchWindow = ScriptableObject.CreateInstance<StateNodeSearchWindow>();

        _searchWindow.Init(editorWindow, this);

        nodeCreationRequest = context =>
        {
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        };
    }
    #endregion

    #region Port
    private Port GeneratePort(StateNodeView node, Direction direction, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(bool));
    }

    #endregion

    #region Entry
    private StateNodeView GenerateEntryPointNode()
    {
        var node = new StateNodeView("Entry");

        var outputPort = GeneratePort(node, Direction.Output, Port.Capacity.Single
        );

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

    #region State Node
    public StateNodeView CreateStateNode(string stateId, Vector2 position)
    {
        var stateNode = new StateNodeView(stateId);

        // Input
        var inputPort = GeneratePort(stateNode, Direction.Input, Port.Capacity.Multi);

        inputPort.portName = "Input ";

        stateNode.inputContainer.Add(inputPort);

        // Output
        var outputPort = GeneratePort(stateNode, Direction.Output, Port.Capacity.Multi);

        outputPort.portName = "Output ";

        stateNode.outputContainer.Add(outputPort);

        // StateId ±à¼­
        var stateIdField = new TextField("State Id")
        {
            value = stateId
        };

        stateIdField.RegisterValueChangedCallback(evt =>
        {
            var newStateId = evt.newValue.Trim();

            if (string.IsNullOrEmpty(newStateId))
            {
                stateIdField.SetValueWithoutNotify(stateNode.StateId);
                return;
            }

            stateNode.SetStateId(newStateId);
        });

        stateNode.mainContainer.Add(stateIdField);

        stateNode.RefreshExpandedState();
        stateNode.RefreshPorts();

        stateNode.SetPosition(new Rect(position, DefaultNodeSize));

        return stateNode;
    }

    public void CreateNode(string stateId, Vector2 position)
    {
        var node = CreateStateNode(stateId, position);
        AddElement(node);
    }
    #endregion

    #region Edge
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        ports.ForEach(port =>
        {
            if (startPort == port) return;
            if (startPort.node == port.node) return;
            if (startPort.direction == port.direction) return;
            compatiblePorts.Add(port);
        });
        return compatiblePorts;
    }
    #endregion

    #region Delete
    public override EventPropagation DeleteSelection()
    {
        var selectedElements = selection.ToList();

        var edges = selectedElements.OfType<Edge>().ToList();

        foreach (var edge in edges)
        {
            RemoveElement(edge);
        }
        return base.DeleteSelection();
    }
    #endregion
}