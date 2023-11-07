# kern.services.CumulocityClient.Api.RealTimeNotificationAPIApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**PostNotificationRealtimeResource**](RealTimeNotificationAPIApi.md#postnotificationrealtimeresource) | **POST** /notification/realtime | Responsive communication |

<a name="postnotificationrealtimeresource"></a>
# **PostNotificationRealtimeResource**
> RealtimeNotification PostNotificationRealtimeResource (RealtimeNotification realtimeNotification, string? accept = null, string? xCumulocityProcessingMode = null)

Responsive communication

The Real-time notification API enables responsive communication from Cumulocity IoT over restricted networks towards clients such as web browser and mobile devices. All clients subscribe to so-called channels to receive messages. These channels are filled by Cumulocity IoT with the output of [Operations](#tag/Operations). In addition, particular system channels are used for the initial handshake with clients, subscription to channels, removal from channels and connection. The [Bayeux protocol](https://docs.cometd.org/current/reference/#_concepts_bayeux_protocol) over HTTPS or WSS is used as communication mechanism.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostNotificationRealtimeResourceExample
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

            var apiInstance = new RealTimeNotificationAPIApi(config);
            var realtimeNotification = new RealtimeNotification(); // RealtimeNotification | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Responsive communication
                RealtimeNotification result = apiInstance.PostNotificationRealtimeResource(realtimeNotification, accept, xCumulocityProcessingMode);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling RealTimeNotificationAPIApi.PostNotificationRealtimeResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostNotificationRealtimeResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Responsive communication
    ApiResponse<RealtimeNotification> response = apiInstance.PostNotificationRealtimeResourceWithHttpInfo(realtimeNotification, accept, xCumulocityProcessingMode);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling RealTimeNotificationAPIApi.PostNotificationRealtimeResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **realtimeNotification** | [**RealtimeNotification**](RealtimeNotification.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

[**RealtimeNotification**](RealtimeNotification.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The operation was completed and the result is sent in the response. |  -  |
| **400** | Unprocessable Entity – invalid payload. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

