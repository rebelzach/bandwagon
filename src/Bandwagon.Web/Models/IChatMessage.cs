namespace Bandwagon.Web.Models;

public interface IChatMessage
{
    Badge[] BadgeInfo { get; set; }
    Badge[] Badges { get; set; }
    string DisplayName { get; set; }
    object EmoteReplacedMessage { get; set; }
    EmoteSet EmoteSet { get; set; }
    string Message { get; set; }
    long TmiSentTs { get; set; }
    string Username { get; set; }
    long UserType { get; set; }
}