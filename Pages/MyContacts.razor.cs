using GameStore.Client.Data;
using GameStore.Client.GraphQl;
using GameStore.Client.GraphQl.Responses;
using GraphQL;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

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
    private string term = "";
    
    private bool FilterFunc1(Contact element) => FilterFunc(element, term);
    
   

    private bool FilterFunc(Contact contact, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (contact.phoneNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }

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