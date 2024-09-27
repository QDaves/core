using System.Collections.Generic;

using Xabbo.Messages;
using Xabbo.Messages.Flash;

namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Represents a friend list fragment.
/// <para/>
/// Received after requesting the user's list of friends.
/// <para/>
/// Supported clients: <see cref="ClientType.All"/>.
/// </summary>
public sealed class FriendListMsg : List<Friend>, IMessage<FriendListMsg>
{
    public FriendListMsg() { }
    public FriendListMsg(int capacity) : base(capacity) { }
    public FriendListMsg(IEnumerable<Friend> collection) : base(collection) { }

    public int FragmentIndex { get; set; }
    public int FragmentCount { get; set; }

    public static Identifier Identifier => In.FriendListFragment;

    public static FriendListMsg Parse(in PacketReader p)
    {
        int fragmentIndex = 0, fragmentCount = 1;

        if (p.Client is not ClientType.Shockwave)
        {
            fragmentCount = p.ReadInt();
            fragmentIndex = p.ReadInt();
        }

        return new(p.ParseArray<Friend>())
        {
            FragmentIndex = fragmentIndex,
            FragmentCount = fragmentCount,
        };
    }

    public void Compose(in PacketWriter p)
    {
        if (p.Client is not ClientType.Shockwave)
        {
            p.WriteInt(FragmentCount);
            p.WriteInt(FragmentIndex);
        }
        p.ComposeArray(this);
    }
}
