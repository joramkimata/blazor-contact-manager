

namespace GameStore.Client.Data;


public class Contact
{
    public required string uuid { get; set; }
    public required string phoneNumber { get; set; }
    public required bool isPublic { get; set; }
}