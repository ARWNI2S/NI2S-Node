using Orleans;

namespace ARWNI2S.Engine.Orleans
{
    public interface IQuorumValueGrain<TValue> : IGrainWithIntegerKey
    {
        Task Vote(TValue value);
    }

    public interface IQuorumValueGrain<TValue, TQuorum> : IQuorumValueGrain<TValue>
        where TQuorum : IQuorumValueGrain<TValue>
    {
        new IList<TQuorum> QuorumMembers { get; }
    }
}
