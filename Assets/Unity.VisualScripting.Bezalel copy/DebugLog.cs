using Unity.VisualScripting;
using UnityEngine;


[UnitCategory("Bezalel")]
[UnitSurtitle("Debug")]
[UnitTitle("Log")]
[UnitShortTitle("Log")]
[TypeIcon(typeof(Debug))]
public sealed class DebugLog : Unit
{
    
    [DoNotSerialize]
    [PortLabelHidden]
    public ControlInput enter { get; private set; }

    [DoNotSerialize]
    [PortLabelHidden]
    public ControlOutput exit { get; private set; }
    
    [DoNotSerialize] private ValueInput _message; 
    
    protected override void Definition()
    {
        _message = ValueInput<string>("Message", string.Empty);
        enter = ControlInput(nameof(enter), Enter);
        exit = ControlOutput(nameof(exit));
    }
    
    private ControlOutput Enter(Flow flow)
    {
        Debug.Log(flow.GetValue<string>(_message));
        return exit;
    }
}
