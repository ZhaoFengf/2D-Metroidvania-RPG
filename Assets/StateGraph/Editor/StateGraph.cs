using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class StateGraph : EditorWindow
{
    private StateGraphView _graphView;

    [MenuItem("Graph/State Graph")]
    public static void OpenStateGraphWindow()
    {
        StateGraph window = GetWindow<StateGraph>();
        window.titleContent = new GUIContent("State Graph");
    }
    private void OnEnable()
    {
        ConstructGraphView();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }

    private void ConstructGraphView()
    {
        _graphView = new StateGraphView(this)
        {
            name = "State Graph"
        };

        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

}
