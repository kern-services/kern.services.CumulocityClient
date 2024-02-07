# kern.services.CumulocityClient.Api.AlarmAPIApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetAlarmsApiResource**](AlarmAPIApi.md#getalarmsapiresource) | **GET** /alarm | Retrieve URIs to collections of alarms |

<a id="getalarmsapiresource"></a>
# **GetAlarmsApiResource**
> AlarmsApiResource GetAlarmsApiResource ()

Retrieve URIs to collections of alarms

Retrieve URIs and URI templates to collections of alarms.  <section><h5>Required roles</h5> ROLE_ALARM_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetAlarmsApiResourceExample
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

            var apiInstance = new AlarmAPIApi(config);

            try
            {
                // Retrieve URIs to collections of alarms
                AlarmsApiResource result = apiInstance.GetAlarmsApiResource();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AlarmAPIApi.GetAlarmsApiResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAlarmsApiResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve URIs to collections of alarms
    ApiResponse<AlarmsApiResource> response = apiInstance.GetAlarmsApiResourceWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AlarmAPIApi.GetAlarmsApiResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**AlarmsApiResource**](AlarmsApiResource.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.alarmapi+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the URIs are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

