namespace NI2S.Node
{
    public enum NodeState
    {
        // Initial state.
        None = 0,

        // In starting.
        Starting = 1,

        // Started.
        Started = 2,

        // In stopping
        Stopping = 3,

        // Stopped.
        Stopped = 4,

        // Failed to start.
        Failed = 5
    }
}
