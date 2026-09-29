using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class StateNodeSearchWindow :
    ScriptableObject,
    ISearchWindowProvider
{
    private StateGraphView _graphView;
    private EditorWindow _window;

    private Texture2D _indentationIcon;

    public void Init(EditorWindow window, StateGraphView graphView)
    {
        _window = window;
        _graphView = graphView;
        _indentationIcon = new Texture2D(1, 1);
        _indentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
        _indentationIcon.Apply();
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        return new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("State Graph"), 0),
            new SearchTreeEntry(new GUIContent("State", _indentationIcon))
            {
                level = 1,
                userData = "State"
            }
        };
    }

    public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
    {
        var worldMousePosition = _window.rootVisualElement.ChangeCoordinatesTo(_window.rootVisualElement.parent, 
            context.screenMousePosition - _window.position.position);

        var localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);

        if (entry.userData is string stateType && stateType == "State")
        {
            _graphView.CreateNode("NewState", localMousePosition);
            return true;
        }

        return false;
    }
}