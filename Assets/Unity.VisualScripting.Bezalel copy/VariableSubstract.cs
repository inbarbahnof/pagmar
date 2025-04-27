using Unity.VisualScripting;


[UnitCategory("Bezalel")]
[UnitShortTitle("Variable Substract")]
// [UnitSubtitle("Variable")]
[UnitTitle("Variable Substract")]
[TypeIcon(typeof(Subtract<>))]
// [UnitSurtitle("Add To")]
public sealed class VariableSubstract : VariableChanger
{
    public VariableSubstract() : base() { }

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
        return _preIncrementValue - GetAmount(flow);
    }
}
