

using GameStore.Client.GraphQl;
using GameStore.Client.GraphQl.Responses;
using GraphQL;
using Microsoft.AspNetCore.Components;

namespace GameStore.Client.Components
{
    public partial class Contacts
    {
        [Inject]
        private GraphqlService graphqlService { get; set; }

        private GraphQLResponse<ContactsResponse>? contactsData;

        protected override async Task OnInitializedAsync()
        {
            contactsData = await graphqlService.GetPublicContacts();
        }
    }
}