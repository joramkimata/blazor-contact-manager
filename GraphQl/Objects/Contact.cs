

namespace GameStore.Client.Data;


public class Contact
{
    public required string uuid { get; set; }
    public required string createdAt { get; set; }
    public required string updatedAt { get; set; }
    public required string deletedAt { get; set; }
    public required string phoneNumber { get; set; }
    public required bool isPublic { get; set; }
}