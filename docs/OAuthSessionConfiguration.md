# kern.services.CumulocityClient.Model.OAuthSessionConfiguration
The session configuration properties are only available for OAuth internal. See [Changing settings > OAuth internal](https://cumulocity.com/guides/users-guide/administration/#oauth-internal) for more details.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**AbsoluteTimeoutMillis** | **int** | Maximum session duration (in milliseconds) during which a user does not have to login again. | [optional] 
**MaximumNumberOfParallelSessions** | **int** | Maximum number of parallel sessions for one user. | [optional] 
**RenewalTimeoutMillis** | **int** | Amount of time before a token expires (in milliseconds) during which the token may be renewed. | [optional] 
**UserAgentValidationRequired** | **bool** | Switch to turn additional user agent verification on or off during the session. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

