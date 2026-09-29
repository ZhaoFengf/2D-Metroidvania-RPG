using UnityEditor.Experimental.GraphView;

public class StateNodeView : Node
{
    public string Guid;
    public string StateId;

    public StateNodeView() : this("New State") { }
    public StateNodeView(string stateId)
    {
        Guid = System.Guid.NewGuid().ToString();
        StateId = stateId;

        title = StateId;
    }

    public void SetStateId(string stateId)
    {
        StateId = stateId;
        title = StateId;
    }
}