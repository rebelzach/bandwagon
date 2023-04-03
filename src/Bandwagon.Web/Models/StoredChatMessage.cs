using Newtonsoft.Json;

#nullable disable

namespace Bandwagon.Web.Models;

public class StoredChatMessage : IChatMessage
{
    [JsonProperty("BadgeInfo")]
    public Badge[] BadgeInfo { get; set; }

    [JsonProperty("Bits")]
    public long Bits { get; set; }

    [JsonProperty("BitsInDollars")]
    public long BitsInDollars { get; set; }

    [JsonProperty("Channel")]
    public string Channel { get; set; }

    [JsonProperty("CheerBadge")]
    public object CheerBadge { get; set; }

    [JsonProperty("CustomRewardId")]
    public object CustomRewardId { get; set; }

    [JsonProperty("EmoteReplacedMessage")]
    public object EmoteReplacedMessage { get; set; }

    [JsonProperty("Id")]
    public Guid Id { get; set; }

    [JsonProperty("IsBroadcaster")]
    public bool IsBroadcaster { get; set; }

    [JsonProperty("IsFirstMessage")]
    public bool IsFirstMessage { get; set; }

    [JsonProperty("IsHighlighted")]
    public bool IsHighlighted { get; set; }

    [JsonProperty("IsMe")]
    public bool IsMe { get; set; }

    [JsonProperty("IsModerator")]
    public bool IsModerator { get; set; }

    [JsonProperty("IsSkippingSubMode")]
    public bool IsSkippingSubMode { get; set; }

    [JsonProperty("IsSubscriber")]
    public bool IsSubscriber { get; set; }

    [JsonProperty("IsVip")]
    public bool IsVip { get; set; }

    [JsonProperty("IsStaff")]
    public bool IsStaff { get; set; }

    [JsonProperty("IsPartner")]
    public bool IsPartner { get; set; }

    [JsonProperty("Message")]
    public string Message { get; set; }

    [JsonProperty("Noisy")]
    public long Noisy { get; set; }

    [JsonProperty("RoomId")]
    public string RoomId { get; set; }

    [JsonProperty("SubscribedMonthCount")]
    public long SubscribedMonthCount { get; set; }

    [JsonProperty("TmiSentTs")]
    public long TmiSentTs { get; set; }

    [JsonProperty("ChatReply")]
    public object ChatReply { get; set; }

    [JsonProperty("Badges")]
    public Badge[] Badges { get; set; }

    [JsonProperty("BotUsername")]
    public string BotUsername { get; set; }

    [JsonProperty("Color")]
    public string Color { get; set; }

    [JsonProperty("ColorHex")]
    public string ColorHex { get; set; }

    [JsonProperty("DisplayName")]
    public string DisplayName { get; set; }

    [JsonProperty("EmoteSet")]
    public EmoteSet EmoteSet { get; set; }

    [JsonProperty("IsTurbo")]
    public bool IsTurbo { get; set; }

    [JsonProperty("UserId")]
    public string UserId { get; set; }

    [JsonProperty("Username")]
    public string Username { get; set; }

    [JsonProperty("UserType")]
    public long UserType { get; set; }

    [JsonProperty("RawIrcMessage")]
    public string RawIrcMessage { get; set; }
}

public partial class Badge
{
    [JsonProperty("Key")]
    public string Key { get; set; }

    [JsonProperty("Value")]
    public string Value { get; set; }
}

public partial class EmoteSet
{
    [JsonProperty("Emotes")]
    public Emote[] Emotes { get; set; }

    [JsonProperty("RawEmoteSetString")]
    public string RawEmoteSetString { get; set; }
}

public partial class Emote
{
    [JsonProperty("Id")]
    public string Id { get; set; }

    [JsonProperty("Name")]
    public string Name { get; set; }

    [JsonProperty("StartIndex")]
    public long StartIndex { get; set; }

    [JsonProperty("EndIndex")]
    public long EndIndex { get; set; }

    [JsonProperty("ImageUrl")]
    public Uri ImageUrl { get; set; }
}
