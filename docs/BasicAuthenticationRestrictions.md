
# kern.services.CumulocityClient.Model.BasicAuthenticationRestrictions

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**ForbiddenClients** | **List&lt;string&gt;** | List of types of clients which are not allowed to use basic authentication. Currently the only supported option is WEB_BROWSERS. | [optional] 
**ForbiddenUserAgents** | **List&lt;string&gt;** | List of user agents, passed in &#x60;User-Agent&#x60; HTTP header, which are blocked if basic authentication is used. | [optional] 
**TrustedUserAgents** | **List&lt;string&gt;** | List of user agents, passed in &#x60;User-Agent&#x60; HTTP header, which are allowed to use basic authentication. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to README]](../README.md)

