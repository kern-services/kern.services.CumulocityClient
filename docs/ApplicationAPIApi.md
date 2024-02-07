# kern.services.CumulocityClient.Api.ApplicationAPIApi

All URIs are relative to *https://<TENANT_DOMAIN>*

Method | HTTP request | Description
------------- | ------------- | -------------
[**GetApplicationManagementApiResource**](ApplicationAPIApi.md#getapplicationmanagementapiresource) | **GET** /application | Retrieve URIs to collections of applications



## GetApplicationManagementApiResource

> ApplicationApiResource GetApplicationManagementApiResource ()

Retrieve URIs to collections of applications

Retrieve URIs and URI templates to collections of applications.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetApplicationManagementApiResourceExample
    {
        public static void Main()
        {
            Configuration.Default.BasePath = "https://<TENANT_DOMAIN>";
            // Configure HTTP basic authorization: Basic
            Configuration.Default.Username = "YOUR_USERNAME";
            Configuration.Default.Password = "YOUR_PASSWORD";
            // Configure HTTP bearer authorization: OAI-Secure
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";
            // Configure OAuth2 access token for authorization: SSO
            Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new ApplicationAPIApi(Configuration.Default);

            try
            {
                // Retrieve URIs to collections of applications
                ApplicationApiResource result = apiInstance.GetApplicationManagementApiResource();
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling ApplicationAPIApi.GetApplicationManagementApiResource: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

### Parameters

This endpoint does not need any parameter.

### Return type

[**ApplicationApiResource**](ApplicationApiResource.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/vnd.com.nsn.cumulocity.applicationapi+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and and the URIs are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)

