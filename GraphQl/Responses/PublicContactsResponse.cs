

using GameStore.Client.Data;

namespace GameStore.Client.GraphQl.Responses;

public class PublicContactsResponse
{
    public required List<Contact> GetPublicContacts { get; set; }
}