# kern.services.CumulocityClient.Api.TokensApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**PostNotificationTokenResource**](TokensApi.md#postnotificationtokenresource) | **POST** /notification2/token | Create a notification token |
| [**PostNotificationTokenUnsubscribeResource**](TokensApi.md#postnotificationtokenunsubscriberesource) | **POST** /notification2/unsubscribe | Unsubscribe a subscriber |

<a id="postnotificationtokenresource"></a>
# **PostNotificationTokenResource**
> NotificationToken PostNotificationTokenResource (NotificationTokenClaims notificationTokenClaims, string? accept = null, string? xCumulocityProcessingMode = null)

Create a notification token

Create a new JWT (JSON web token) access token which can be used to establish a successful WebSocket connection to read a sequence of notifications.  In general, each request to obtain an access token consists of:  *  The subscriber name which the client wishes to be identified with. *  The subscription name. This value must be associated with a subscription that's already been created and in essence, the obtained token will give the ability to read notifications for the subscription that is specified here. *  The token expiration duration.  <section><h5>Required roles</h5> ROLE_NOTIFICATION_2_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostNotificationTokenResourceExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://<TENANT_DOMAIN>";
            // Configure HTTP basic authorization: Basic
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";
            // Configure Bearer token for authorization: OAI-Secure
            config.AccessToken = "YOUR_BEARER_TOKEN";
            // Configure OAuth2 access token for authorization: SSO
            config.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new TokensApi(config);
            var notificationTokenClaims = new NotificationTokenClaims(); // NotificationTokenClaims | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Create a notification token
                NotificationToken result = apiInstance.PostNotificationTokenResource(notificationTokenClaims, accept, xCumulocityProcessingMode);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TokensApi.PostNotificationTokenResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostNotificationTokenResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create a notification token
    ApiResponse<NotificationToken> response = apiInstance.PostNotificationTokenResourceWithHttpInfo(notificationTokenClaims, accept, xCumulocityProcessingMode);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TokensApi.PostNotificationTokenResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **notificationTokenClaims** | [**NotificationTokenClaims**](NotificationTokenClaims.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

[**NotificationToken**](NotificationToken.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A notification token was created. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="postnotificationtokenunsubscriberesource"></a>
# **PostNotificationTokenUnsubscribeResource**
> NotificationSubscriptionResult PostNotificationTokenUnsubscribeResource (string token, string? accept = null, string? xCumulocityProcessingMode = null)

Unsubscribe a subscriber

Unsubscribe a notification subscriber using the notification token.  Once a subscription is made, notifications will be kept until they are consumed by all subscribers who have previously connected to the subscription. For non-volatile subscriptions, this can result in notifications remaining in storage if never consumed by the application. They will be deleted if a tenant is deleted. It can take up considerable space in permanent storage for high-frequency notification sources. Therefore, we recommend you to unsubscribe a subscriber that will never run again. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostNotificationTokenUnsubscribeResourceExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://<TENANT_DOMAIN>";
            // Configure HTTP basic authorization: Basic
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";
            // Configure Bearer token for authorization: OAI-Secure
            config.AccessToken = "YOUR_BEARER_TOKEN";
            // Configure OAuth2 access token for authorization: SSO
            config.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new TokensApi(config);
            var token = eyJhbGciOiJSUzI1NiJ9...eyJzdWIiOiJ0ZXN0U32Nya;  // string | Subscriptions associated with this token will be removed.
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Unsubscribe a subscriber
                NotificationSubscriptionResult result = apiInstance.PostNotificationTokenUnsubscribeResource(token, accept, xCumulocityProcessingMode);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TokensApi.PostNotificationTokenUnsubscribeResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostNotificationTokenUnsubscribeResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Unsubscribe a subscriber
    ApiResponse<NotificationSubscriptionResult> response = apiInstance.PostNotificationTokenUnsubscribeResourceWithHttpInfo(token, accept, xCumulocityProcessingMode);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TokensApi.PostNotificationTokenUnsubscribeResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **token** | **string** | Subscriptions associated with this token will be removed. |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

[**NotificationSubscriptionResult**](NotificationSubscriptionResult.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The notification subscription was deleted or is scheduled for deletion. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

