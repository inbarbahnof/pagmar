using Unity.VisualScripting;


[UnitCategory("Bezalel")]
[UnitShortTitle("Variable Multiply")]
// [UnitSubtitle("Variable")]
[UnitTitle("Variable Multiply")]
[TypeIcon(typeof(Multiply<>))]
// [UnitSurtitle("Add To")]
public sealed class VariableMultiply : VariableChanger
{
    public VariableMultiply() : base() { }

    [DoNotSerialize]
    [PortLabel("amount")]
    public ValueInput amount { get; private set; }


    /// <summary>
    /// The value assigned to the variable after incrementing.
    /// </summary>
    [DoNotSerialize]
    [PortLabelHidden]
    public ValueOutput postIncrement { get; private set; }


    protected override void Definition()
    {
        base.Definition();

        amount = ValueInput<float>(nameof(amount), 1);
        postIncrement = ValueOutput<float>(nameof(postIncrement), (x) => _postIncrementValue);
        
        Requirement(amount, assign);
        Requirement(amount, postIncrement);
    }

    protected override float GetAmount(Flow flow)
    {
        return flow.GetValue<float>(amount);
    }

    protected override float Calculate(Flow flow)
    {
        return _preIncrementValue * GetAmount(flow);
    }
}
