using System.Collections.Generic;
using Xabbo.Messages;
using Xabbo.Messages.Flash;

namespace Xabbo.Core.Messages.Incoming.Modern;

/// <summary>
/// Received after searching the marketplace for offers.
/// <para/>
/// Response for <see cref="Outgoing.Modern.SearchMarketplaceOffersMsg"/>.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>.
/// </summary>
/// <param name="Offers">The list of marketplace offers.</param>
public sealed record MarketplaceOffersMsg(List<MarketplaceOffer> Offers) : IMessage<MarketplaceOffersMsg>
{
    static ClientType IMessage<MarketplaceOffersMsg>.SupportedClients => ClientType.Modern;
    static Identifier IMessage<MarketplaceOffersMsg>.Identifier => In.MarketPlaceOffers;
    static MarketplaceOffersMsg IParser<MarketplaceOffersMsg>.Parse(in PacketReader p) => new([.. p.ParseArray<MarketplaceOffer>()]);
    void IComposer.Compose(in PacketWriter p) => p.ComposeArray(Offers);
}
