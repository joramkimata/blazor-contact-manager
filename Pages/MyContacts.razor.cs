using GameStore.Client.Data;
using GameStore.Client.GraphQl;
using GameStore.Client.GraphQl.Responses;
using GraphQL;
using Microsoft.AspNetCore.Components;

namespace GameStore.Client.Pages;

public partial class MyContacts
{
    [Inject]
    private GraphqlService graphqlService { get; set; }

    private String ErrorMsg = String.Empty;

    private GraphQLResponse<MyContactsResponse>? contactsData;
    
    private bool _hidePosition;
    private bool _loading;
    private IEnumerable<Contact> Contacts = new List<Contact>();

    protected override async Task OnInitializedAsync()
    {
        _loading = true;
        contactsData = await graphqlService.GetMyContacts();
        _loading = false;
        if (contactsData.Errors != null)
        {
            ErrorMsg = "Error occurred";
        }
        else
        {
            ErrorMsg = String.Empty;
            Contacts = contactsData.Data.GetMyContacts;
        }

    }
}