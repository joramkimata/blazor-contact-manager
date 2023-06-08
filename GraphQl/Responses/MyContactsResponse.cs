using GameStore.Client.Data;

namespace GameStore.Client.GraphQl.Responses;

public class MyContactsResponse
{
    public required List<Contact> GetMyContacts { get; set; }
}