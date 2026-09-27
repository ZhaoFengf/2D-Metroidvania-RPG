using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{
    public  readonly Vector2 defaultNodeSize = new Vector2(150, 400);

    public Blackboard Blackboard;
    public List<ExposeProperty> ExposedProperties = new List<ExposeProperty>();
    private NodeSearchWindow _searchWindow;

    public DialogueGraphView(EditorWindow editorWindow)
    {
        //styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));
        var sheet = Resources.Load<StyleSheet>("DialogueGraph");
        Debug.Log(sheet != null ? "Loaded: " + sheet.name : "Load FAILED");
        styleSheets.Add(sheet);
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

    private void AddSearchWindow(EditorWindow editorWindow)
    {
        _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        _searchWindow.Init(editorWindow, this);
        nodeCreationRequest = context =>
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        ports.ForEach((port) =>
        {
            if (startPort != port && startPort.node != port.node)
            {
                compatiblePorts.Add(port);
            }
        });

        return compatiblePorts;
    }


    private Port GeneratePort(DialogueNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }

    private DialogueNode GenerateEntryPointNode()
    {
        var node = new DialogueNode
        {
            title = "Start",
            GUID = System.Guid.NewGuid().ToString(),
            DialogueText = "ENTRY",
            EntryPoint = true
        };


        var generatedPort = GeneratePort(node, Direction.Output);
        generatedPort.portName = "Next";
        node.outputContainer.Add(generatedPort);

        node.capabilities &= ~Capabilities.Movable;
        node.capabilities &= ~Capabilities.Deletable;

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(100, 200, 150, 200));

        return node;
    }

    public void CreateNode(string nodeName, Vector2 position)
    {
        AddElement(CreateDialogueNode(nodeName, position));
    }

    public DialogueNode CreateDialogueNode(string nodeName, Vector2 position)
    {
        var dialogueNode = new DialogueNode
        {
            title = nodeName,
            GUID = System.Guid.NewGuid().ToString(),
            DialogueText = nodeName
        };

        var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        dialogueNode.inputContainer.Add(inputPort);

        dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));

        var button = new Button(() => { AddChoicePort(dialogueNode); });
        button.text = "New Choice";
        dialogueNode.titleButtonContainer.Add(button);

        var textField = new TextField(string.Empty);
        textField.RegisterValueChangedCallback(evt =>
        {
            dialogueNode.DialogueText = evt.newValue;
            dialogueNode.title = evt.newValue;
        });
        textField.SetValueWithoutNotify(dialogueNode.title);
        dialogueNode.mainContainer.Add(textField);

        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(position, defaultNodeSize));

        return dialogueNode;
    }


    public void AddChoicePort(DialogueNode dialogueNode, string overrideenPortName = "")
    {
        var generatedPort = GeneratePort(dialogueNode, Direction.Output);

        var oldLabel = generatedPort.contentContainer.Q<Label>("type");
        generatedPort.contentContainer.Remove(oldLabel);

        var outputPortCount = dialogueNode.outputContainer.Query("connector").ToList().Count;
        var choicePortName = overrideenPortName == "" ? $"Choice {outputPortCount}" : overrideenPortName;

        TextField textField = new TextField
        {
            name = string.Empty,
            value = choicePortName
        };
        textField.RegisterValueChangedCallback(evt =>
        {
            generatedPort.portName = evt.newValue;
        });

        textField.style.flexGrow = 0;
        textField.style.minWidth = 80;
        textField.style.maxWidth = 80;

        generatedPort.contentContainer.Add(new Label("  "));
        generatedPort.contentContainer.Add(textField);
        var deleteButton = new Button(() => RemovePort(dialogueNode, generatedPort))
        {
            text = "X"
        };
        generatedPort.contentContainer.Add(deleteButton);

        //generatedPort.portName = choicePortName;
        dialogueNode.outputContainer.Add(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();

    }

    private void RemovePort(DialogueNode dialogueNode, Port generatedPort)
    {
        var edgesToRemove = edges.ToList().Where(e => e.output == generatedPort || e.input == generatedPort).ToList();

        if (edgesToRemove.Any())
        {
            DeleteElements(edgesToRemove);
        }

        dialogueNode.outputContainer.Remove(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();

        //var targetEdge = edges.ToList().Where(x => x.output.portName == generatedPort.portName && x.output.node == generatedPort.node);

        //if (!targetEdge.Any()) return;

        //var edge = targetEdge.First();
        //edge.input.Disconnect(edge);
        //RemoveElement(targetEdge.First());

        //dialogueNode.outputContainer.Remove(generatedPort);
        //dialogueNode.RefreshPorts();
        //dialogueNode.RefreshExpandedState();
    }

    public void ClearBlackBoardAndExposedProperties()
    {
        ExposedProperties.Clear();
        Blackboard.Clear();
    }

    public void AddPropertyToBlackBoard(ExposeProperty exposeProperty)
    {
        var localPropertyName = exposeProperty.propertyName;
        var localPropertyValue = exposeProperty.propertyValue;
        while (ExposedProperties.Any(x => x.propertyName == localPropertyName))
        {
            localPropertyName = $"{localPropertyName}(1)";
        }

        var property = new ExposeProperty();
        property.propertyName = localPropertyName;
        property.propertyValue = localPropertyValue;
        ExposedProperties.Add(property);

        var container = new VisualElement();
        var blackboardField = new BlackboardField { text = property.propertyName, typeText = "string" };
        container.Add(blackboardField);

        var propertyValueTextField = new TextField("Value:")
        {
            value = localPropertyValue
        };
        propertyValueTextField.RegisterValueChangedCallback(evt =>
        {
            var changingProperty = ExposedProperties.FindIndex(x => x.propertyName == property.propertyName);
            ExposedProperties[changingProperty].propertyValue = evt.newValue;
        });
        var blackboardRow = new BlackboardRow(blackboardField, propertyValueTextField);
        container.Add(blackboardRow);

        Blackboard.Add(container);
    }
}
