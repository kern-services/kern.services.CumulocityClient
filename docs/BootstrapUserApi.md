# kern.services.CumulocityClient.Api.BootstrapUserApi

All URIs are relative to *https://<TENANT_DOMAIN>*

Method | HTTP request | Description
------------- | ------------- | -------------
[**GetApplicationUserRepresentation**](BootstrapUserApi.md#getapplicationuserrepresentation) | **GET** /application/applications/{id}/bootstrapUser | Retrieve the bootstrap user for a specific application



## GetApplicationUserRepresentation

> BootstrapUser GetApplicationUserRepresentation (string id)

Retrieve the bootstrap user for a specific application

Retrieve the bootstrap user for a specific application (by a given ID).  This only works for microservice applications.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetApplicationUserRepresentationExample
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

            var apiInstance = new BootstrapUserApi(Configuration.Default);
            var id = 20200301;  // string | Unique identifier of the application.

            try
            {
                // Retrieve the bootstrap user for a specific application
                BootstrapUser result = apiInstance.GetApplicationUserRepresentation(id);
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling BootstrapUserApi.GetApplicationUserRepresentation: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

### Parameters


Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **string**| Unique identifier of the application. | 

### Return type

[**BootstrapUser**](BootstrapUser.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/vnd.com.nsn.cumulocity.user+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the bootstrap user of the application is sent in the response. |  -  |
| **400** | Bad request. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)

