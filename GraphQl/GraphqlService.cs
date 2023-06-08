using System.Net.Http.Headers;
using GameStore.Client.GraphQl.Responses;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace GameStore.Client.GraphQl;

public class GraphqlService
{
    private readonly GraphQLHttpClient _graphqlClient;

    public GraphqlService()
    {
        var httpClient = new HttpClient();


        var token = ""; //localStorage.GetItem<string>("token");

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        _graphqlClient = new GraphQLHttpClient(new GraphQLHttpClientOptions()
        {
            EndPoint = new Uri("http://localhost:8030/graphql")
        }, new NewtonsoftJsonSerializer(), httpClient);
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

    public async Task<GraphQL.GraphQLResponse<ContactsResponse>> GetPublicContacts()
    {
        var fetchQuery = await _graphqlClient.SendQueryAsync<ContactsResponse>(_getPublicContacts);

        return fetchQuery;
    }
}