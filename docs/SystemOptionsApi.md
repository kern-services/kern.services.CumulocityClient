# kern.services.CumulocityClient.Api.SystemOptionsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetSystemOptionCollectionResource**](SystemOptionsApi.md#getsystemoptioncollectionresource) | **GET** /tenant/system/options | Retrieve all system options |
| [**GetSystemOptionResource**](SystemOptionsApi.md#getsystemoptionresource) | **GET** /tenant/system/options/{category}/{key} | Retrieve a specific system option |

<a name="getsystemoptioncollectionresource"></a>
# **GetSystemOptionCollectionResource**
> SystemOptionCollection GetSystemOptionCollectionResource ()

Retrieve all system options

Retrieve all the system options available on the tenant.  > **⚠️ Important:** Note that it is possible to call this endpoint without the ROLE_OPTION_MANAGEMENT_ADMIN role, but options that are considered secured (see the list of options above) will be obfuscated with a constant value `\"<<Encrypted>>\"`. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetSystemOptionCollectionResourceExample
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

            var apiInstance = new SystemOptionsApi(config);

            try
            {
                // Retrieve all system options
                SystemOptionCollection result = apiInstance.GetSystemOptionCollectionResource();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SystemOptionsApi.GetSystemOptionCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetSystemOptionCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all system options
    ApiResponse<SystemOptionCollection> response = apiInstance.GetSystemOptionCollectionResourceWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SystemOptionsApi.GetSystemOptionCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**SystemOptionCollection**](SystemOptionCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.optioncollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the system options are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsystemoptionresource"></a>
# **GetSystemOptionResource**
> SystemOption GetSystemOptionResource (string category, string key)

Retrieve a specific system option

Retrieve a specific system option (by a given category and key) on your tenant.  > **⚠️ Important:** Note that it is possible to call this endpoint without the ROLE_OPTION_MANAGEMENT_ADMIN role, but only the options that are considered not secured (see the list of options above) will be returned. Otherwise, if the option is considered secured and the user does not have the required role, an HTTP response 403 will be returned. <section><h5>Required roles</h5> ROLE_OPTION_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetSystemOptionResourceExample
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

            var apiInstance = new SystemOptionsApi(config);
            var category = password;  // string | The category of the system options.
            var key = enforce.strength;  // string | The key of a system option.

            try
            {
                // Retrieve a specific system option
                SystemOption result = apiInstance.GetSystemOptionResource(category, key);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SystemOptionsApi.GetSystemOptionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetSystemOptionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a specific system option
    ApiResponse<SystemOption> response = apiInstance.GetSystemOptionResourceWithHttpInfo(category, key);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SystemOptionsApi.GetSystemOptionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **category** | **string** | The category of the system options. |  |
| **key** | **string** | The key of a system option. |  |

### Return type

[**SystemOption**](SystemOption.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.option+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the system option is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

