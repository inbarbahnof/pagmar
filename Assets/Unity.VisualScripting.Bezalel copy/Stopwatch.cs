using System;
using UnityEngine;
using Unity.VisualScripting;


/// <summary>
/// Runs a timer and outputs elapsed and remaining measurements.
/// </summary>
[UnitCategory("Bezalel/Time")]
[UnitShortTitle("Stopwatch")]
[UnitTitle("Stopwatch")]
[TypeIcon(typeof(Timer))]
// [UnitOrder(7)]
public sealed class Stopwatch : Unit, IGraphElementWithData, IGraphEventListener
{
    public sealed class Data : IGraphElementData
    {
        public float elapsed;

        // public float duration;

        public bool active;

        public bool paused;

        public bool unscaled;

        public Delegate update;

        public bool isListening;
    }

    /// <summary>
    /// The moment at which to start the stopwatch.
    /// If the timer is already started, this will reset it.
    /// If the timer is paused, this will resume it.
    /// </summary>
    [DoNotSerialize]
    public ControlInput start { get; private set; }

    /// <summary>
    /// Trigger to pause the stopwatch.
    /// </summary>
    [DoNotSerialize]
    public ControlInput pause { get; private set; }

    /// <summary>
    /// Trigger to resume the stopwatch.
    /// </summary>
    [DoNotSerialize]
    public ControlInput resume { get; private set; }

    /// <summary>
    /// Trigger to toggle the stopwatch.
    /// If it is idle/stopped, it will start.
    /// If it is active, it will pause.
    /// If it is paused, it will resume.
    /// </summary>
    [DoNotSerialize]
    public ControlInput toggle { get; private set; }
    
    /// <summary>
    /// Trigger to stop the timer.
    /// If it is idle/stopped, nothing will change.
    /// If it is active, it will stop.
    /// If it is started/paused, it will stop and reset elapsed time
    /// </summary>
    [DoNotSerialize]
    public ControlInput stop { get; private set; }

    [DoNotSerialize]
    [PortLabel("Unscaled")]
    public ValueInput unscaledTime { get; private set; }

    [DoNotSerialize]
    public ControlOutput started { get; private set; }

    [DoNotSerialize]
    public ControlOutput tick { get; private set; }

    [DoNotSerialize]
    public ControlOutput completed { get; private set; }

    [DoNotSerialize]
    [PortLabel("Elapsed")]
    public ValueOutput elapsedSeconds { get; private set; }


    protected override void Definition()
    {
        isControlRoot = true;

        start = ControlInput(nameof(start), Start);
        stop = ControlInput(nameof(stop), Stop);
        pause = ControlInput(nameof(pause), Pause);
        resume = ControlInput(nameof(resume), Resume);
        toggle = ControlInput(nameof(toggle), Toggle);

        unscaledTime = ValueInput(nameof(unscaledTime), false);

        started = ControlOutput(nameof(started));
        tick = ControlOutput(nameof(tick));
        completed = ControlOutput(nameof(completed));

        elapsedSeconds = ValueOutput<float>(nameof(elapsedSeconds));
    }

    public IGraphElementData CreateData()
    {
        return new Data();
    }

    public void StartListening(GraphStack stack)
    {
        var data = stack.GetElementData<Data>(this);

        if (data.isListening)
        {
            return;
        }

        var reference = stack.ToReference();
        var hook = new EventHook(EventHooks.Update, stack.machine);
        Action<EmptyEventArgs> update = args => TriggerUpdate(reference);
        EventBus.Register(hook, update);
        data.update = update;
        data.isListening = true;
    }

    public void StopListening(GraphStack stack)
    {
        var data = stack.GetElementData<Data>(this);

        if (!data.isListening)
        {
            return;
        }

        var hook = new EventHook(EventHooks.Update, stack.machine);
        EventBus.Unregister(hook, data.update);
        data.update = null;
        data.isListening = false;
    }

    public bool IsListening(GraphPointer pointer)
    {
        return pointer.GetElementData<Data>(this).isListening;
    }

    private void TriggerUpdate(GraphReference reference)
    {
        using (var flow = Flow.New(reference))
        {
            Update(flow);
        }
    }

    private ControlOutput Start(Flow flow)
    {
        var data = flow.stack.GetElementData<Data>(this);

        data.elapsed = 0;
        data.active = true;
        data.paused = false;
        data.unscaled = flow.GetValue<bool>(unscaledTime);

        AssignMetrics(flow, data);

        return started;
    }    
    
    private ControlOutput Stop(Flow flow)
    {
        var data = flow.stack.GetElementData<Data>(this);

        if (!data.active) return null;
        
        data.active = false;
        data.paused = false;
        
        AssignMetrics(flow, data);

        return completed;
    }

    private ControlOutput Pause(Flow flow)
    {
        var data = flow.stack.GetElementData<Data>(this);

        data.paused = true;

        return null;
    }

    private ControlOutput Resume(Flow flow)
    {
        var data = flow.stack.GetElementData<Data>(this);

        data.paused = false;

        return null;
    }

    private ControlOutput Toggle(Flow flow)
    {
        var data = flow.stack.GetElementData<Data>(this);

        if (!data.active)
        {
            return Start(flow);
        }
        else
        {
            data.paused = !data.paused;

            return null;
        }
    }

    private void AssignMetrics(Flow flow, Data data)
    {
        flow.SetValue(elapsedSeconds, data.elapsed);
    }

    public void Update(Flow flow)
    {
        var data = flow.stack.GetElementData<Data>(this);

        if (!data.active || data.paused)
        {
            return;
        }

        data.elapsed += data.unscaled ? Time.unscaledDeltaTime : Time.deltaTime;

        AssignMetrics(flow, data);

        var stack = flow.PreserveStack();

        flow.Invoke(tick);
        flow.DisposePreservedStack(stack);
    }
}

