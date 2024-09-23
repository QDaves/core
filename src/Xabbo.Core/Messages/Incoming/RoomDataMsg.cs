using Xabbo.Messages;
using Xabbo.Messages.Flash;

namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received after requesting room data for a specified room.
/// <para/>
/// Response for <see cref="Outgoing.GetRoomDataMsg"/>.
/// </summary>
public sealed record RoomDataMsg(RoomData Data) : IMessage<RoomDataMsg>
{
    static Identifier IMessage<RoomDataMsg>.Identifier => In.GetGuestRoomResult;
    static RoomDataMsg IParser<RoomDataMsg>.Parse(in PacketReader p) => new(p.Parse<RoomData>());
    void IComposer.Compose(in PacketWriter p) => p.Compose(Data);
}
