using Unity.VisualScripting;
using UnityEngine;


[UnitCategory("Bezalel/Physics")]
[UnitSurtitle("Rigidbody 2D")]
[UnitTitle("Translate")]
[UnitShortTitle("Translate")]
[TypeIcon(typeof(Rigidbody2D))]
public sealed class Physics2DTranslate : Unit
{

    [DoNotSerialize]
    [PortLabelHidden]
    public ControlInput enter { get; private set; }

    [DoNotSerialize]
    [PortLabelHidden]
    [NullMeansSelf]
    public ValueInput ValueRigidBody2D; 

    [DoNotSerialize]
    [PortLabelHidden]
    public ValueInput ValueInX;

    [DoNotSerialize]
    [PortLabelHidden]
    public ValueInput ValueInY;
    
    [DoNotSerialize]
    [PortLabelHidden]
    public ValueOutput ValueOut;

    [DoNotSerialize]
    [PortLabelHidden]
    public ControlOutput exit { get; private set; }


    protected override void Definition()
    {
    
        ValueRigidBody2D = ValueInput<Rigidbody2D>(nameof(ValueRigidBody2D),null).NullMeansSelf();
        ValueInX = ValueInput<float>("X", 0);
        ValueInY = ValueInput<float>("Y", 0);
        ValueOut = ValueOutput<Vector2>(nameof(ValueOut));
    
        enter = ControlInput(nameof(enter), Enter);
        exit = ControlOutput(nameof(exit));
    }

    private ControlOutput Enter(Flow flow)
    {
        var body = flow.GetValue<Rigidbody2D>(ValueRigidBody2D);
        var currentPosition = body.position;
        var x = flow.GetValue<float>(ValueInX);
        var y = flow.GetValue<float>(ValueInY);
        var newPosition = currentPosition + new Vector2(x, y);
    
        // note MovePosition updates on fixed and not immediately
        body.MovePosition(newPosition);
    
        flow.SetValue(ValueOut, newPosition);
        return exit;
    }

}