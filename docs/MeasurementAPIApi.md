# kern.services.CumulocityClient.Api.MeasurementAPIApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetMeasurementApiResource**](MeasurementAPIApi.md#getmeasurementapiresource) | **GET** /measurement | Retrieve URIs to collections of measurements |

<a id="getmeasurementapiresource"></a>
# **GetMeasurementApiResource**
> MeasurementApiResource GetMeasurementApiResource ()

Retrieve URIs to collections of measurements

Retrieve URIs and URI templates to collections of measurements.  > **&#9432; Info:** The response sample on the right side contains a subset of all URIs returned by the endpoint method. For all available query parameters see [Retrieve all measurements](#operation/getMeasurementCollectionResource). 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetMeasurementApiResourceExample
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

            var apiInstance = new MeasurementAPIApi(config);

            try
            {
                // Retrieve URIs to collections of measurements
                MeasurementApiResource result = apiInstance.GetMeasurementApiResource();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeasurementAPIApi.GetMeasurementApiResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetMeasurementApiResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve URIs to collections of measurements
    ApiResponse<MeasurementApiResource> response = apiInstance.GetMeasurementApiResourceWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeasurementAPIApi.GetMeasurementApiResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**MeasurementApiResource**](MeasurementApiResource.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.measurementapi+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the URIs are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

