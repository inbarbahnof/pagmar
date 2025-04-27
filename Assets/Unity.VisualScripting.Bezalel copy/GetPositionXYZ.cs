using Unity.VisualScripting;
using UnityEngine;


[UnitCategory("Bezalel")]
[UnitSurtitle("Transform")]
[UnitTitle("Get Position")]
[UnitShortTitle("Get Position")]
[TypeIcon(typeof(Transform))]
public sealed class GetPositionXYZ : Unit
{

    [DoNotSerialize]
    [PortLabelHidden]
    [NullMeansSelf]
    public ValueInput Transform; 
    
    [DoNotSerialize]
    // [PortLabelHidden]
    public ValueOutput X;

    [DoNotSerialize]
    // [PortLabelHidden]
    public ValueOutput Y;

    [DoNotSerialize]
    // [PortLabelHidden]
    public ValueOutput Z;

    protected override void Definition()
    {
        Transform = ValueInput<Transform>(nameof(Transform),null).NullMeansSelf();
        
        X = ValueOutput("X", ReturnX);
        Y = ValueOutput("Y", ReturnY);
        Z = ValueOutput("Z", ReturnZ);
    }

    private float ReturnX(Flow flow)
    {
        var transform = flow.GetValue<Transform>(Transform);
        return  transform.position.x;
    }

    private float ReturnY(Flow flow)
    {
        var transform = flow.GetValue<Transform>(Transform);
        return  transform.position.y;
    }

    private float ReturnZ(Flow flow)
    {
        var transform = flow.GetValue<Transform>(Transform);
        return  transform.position.z;
    }
}