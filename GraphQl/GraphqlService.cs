using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Client.Data;
using GameStore.Client.GraphQl.Responses;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace GameStore.Client.GraphQl;

public class GraphqlService
{
    private readonly GraphQL.Client.Http.GraphQLHttpClient _graphqlClient =
        new GraphQLHttpClient("http://localhost:8030/graphql", new NewtonsoftJsonSerializer());

    private readonly GraphQLRequest _getPublicContacts = new GraphQLRequest
    {
        Query = @"
            query getPublicContacts{
                getPublicContacts {
                    uuid
                    phoneNumber
                    isPublic
                    deletedAt
                    deletedAt
                    createdAt
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