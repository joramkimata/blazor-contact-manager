using System.Net.Http.Headers;
using Blazored.LocalStorage;
using GameStore.Client.Data;
using GameStore.Client.GraphQl.Inputs;
using GameStore.Client.GraphQl.Responses;
using GameStore.Client.Utils;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace GameStore.Client.GraphQl;

public class GraphqlService
{
    private readonly GraphQLHttpClient _graphqlClient =
        new GraphQLHttpClient("http://localhost:8030/graphql", new NewtonsoftJsonSerializer());

    private readonly ISyncLocalStorageService _storageService;
    
    public GraphqlService(ISyncLocalStorageService storageService)
    {
        _storageService = storageService;
    }

    private String GetToken()
    {
        var httpClient = new HttpClient();
        var token = _storageService.GetItem<string>(LocalStorageConstants.TokenKey);
        return token;
    }


    private readonly GraphQLRequest _getPublicContacts = new GraphQLRequest
    {
        Query = @"
            query getPublicContacts{
                getPublicContacts {
                    uuid
                    phoneNumber
                    isPublic
                }
            }
        ",
        OperationName = "getPublicContacts"
    };

    private readonly GraphQLRequest _getMyContacts = new GraphQLRequest
    {
        Query = @"
            query getMyContacts {
              getMyContacts {
                uuid
                isPublic
                phoneNumber
              }
            }
        ",
        OperationName = "getMyContacts"
    };

  


    public async Task<GraphQL.GraphQLResponse<PublicContactsResponse>> GetPublicContacts()
    {
        
        

        var fetchQuery = await _graphqlClient.SendQueryAsync<PublicContactsResponse>(_getPublicContacts);

        return fetchQuery;
    }
    
    public async Task<GraphQL.GraphQLResponse<MyContactsResponse>> GetMyContacts()
    {
        _graphqlClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
        var fetchQuery = await _graphqlClient.SendQueryAsync<MyContactsResponse>(_getMyContacts);

        return fetchQuery;
    }

    public async Task<GraphQL.GraphQLResponse<Contact>> CreateContact(ContactInput contactInput)
    {
        var createContact = new GraphQLRequest
        {
            Query = @"
            mutation createContact($contactInput: ContactInput!) {
              createContact(contactInput: $contactInput) {
                uuid
                phoneNumber
                isPublic
              }
            }        
            ",
            Variables = new
            {
                contactInput
            },
            OperationName = "createContact"
        };
        
        _graphqlClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
        var fetchQuery = await _graphqlClient.SendQueryAsync<Contact>(createContact);

        return fetchQuery;
    }
}