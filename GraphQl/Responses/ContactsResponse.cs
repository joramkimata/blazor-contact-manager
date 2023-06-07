

using GameStore.Client.Data;

namespace GameStore.Client.GraphQl.Responses;

public class ContactsResponse
{
    public required List<Contact> GetPublicContacts { get; set; }
}