# kern.services.CumulocityClient.Api.InventoryAPIApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetInventoryApiResource**](InventoryAPIApi.md#getinventoryapiresource) | **GET** /inventory | Retrieve URIs to collections of managed objects |

<a id="getinventoryapiresource"></a>
# **GetInventoryApiResource**
> InventoryApiResource GetInventoryApiResource ()

Retrieve URIs to collections of managed objects

Retrieve URIs and URI templates to collections of managed objects.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetInventoryApiResourceExample
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

            var apiInstance = new InventoryAPIApi(config);

            try
            {
                // Retrieve URIs to collections of managed objects
                InventoryApiResource result = apiInstance.GetInventoryApiResource();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling InventoryAPIApi.GetInventoryApiResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetInventoryApiResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve URIs to collections of managed objects
    ApiResponse<InventoryApiResource> response = apiInstance.GetInventoryApiResourceWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling InventoryAPIApi.GetInventoryApiResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**InventoryApiResource**](InventoryApiResource.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.inventoryapi+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the URIs are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

