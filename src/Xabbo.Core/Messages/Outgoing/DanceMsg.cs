using System;

using Xabbo.Messages;

namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when starting, stopping or changing dances in a room.
/// <para/>
/// Supported clients: <see cref="ClientType.All"/>.
/// </summary>
/// <param name="Dance">The dance to change to.</param>
public sealed record DanceMsg(Dances Dance = Dances.Dance) : IMessage<DanceMsg>
{
    static Identifier IMessage<DanceMsg>.Identifier => default;
    static bool IMessage<DanceMsg>.UseTargetedIdentifiers => true;

    static Identifier[] IMessage<DanceMsg>.Identifiers { get; } = [
        Xabbo.Messages.Shockwave.Out.STOP,
        Xabbo.Messages.Shockwave.Out.DANCE,
        Xabbo.Messages.Flash.Out.Dance,
    ];

    Identifier IMessage.GetIdentifier(ClientType client) => client switch
    {
        ClientType.Shockwave => Dance switch
        {
            Dances.None => Xabbo.Messages.Shockwave.Out.STOP,
            _ => Xabbo.Messages.Shockwave.Out.DANCE,
        },
        _ => Xabbo.Messages.Flash.Out.Dance,
    };

    static bool IMessage<DanceMsg>.Match(in PacketReader p)
    {
        if (p.Context is null)
            throw new Exception($"Context is null when attempting to match {nameof(DanceMsg)}.");

        if (p.Context.Messages.Is(p.Header, Xabbo.Messages.Shockwave.Out.STOP))
            return p.ReadContent().Equals("Dance");
        else
            return true;
    }

    static DanceMsg IParser<DanceMsg>.Parse(in PacketReader p)
    {
        Dances dance;

        if (p.Client is ClientType.Shockwave)
        {
            if (p.Context is null)
                throw new Exception($"Context is null when parsing {nameof(DanceMsg)}.");

            dance = p.Context.Messages.Is(p.Header, Xabbo.Messages.Shockwave.Out.STOP)
                ? Dances.None : Dances.Dance;
        }
        else
        {
            dance = (Dances)p.ReadInt();
        }

        return new DanceMsg { Dance = dance };
    }

    void IComposer.Compose(in PacketWriter p)
    {
        if (p.Client is ClientType.Shockwave)
        {
            switch (Dance)
            {
                case Dances.None:
                    // Message identifier should be 'STOP'
                    p.WriteContent("Dance");
                    break;
                case Dances.Dance:
                    break;
                default:
                    throw new Exception($"{Dance} is not supported on Shockwave.");
            }
        }
        else
        {
            p.WriteInt((int)Dance);
        }
    }
}
