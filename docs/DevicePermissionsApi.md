# kern.services.CumulocityClient.Api.DevicePermissionsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetDevicePermissionsResource**](DevicePermissionsApi.md#getdevicepermissionsresource) | **GET** /user/devicePermissions/{id} | Returns all device permissions assignments |
| [**PutDevicePermissionsResource**](DevicePermissionsApi.md#putdevicepermissionsresource) | **PUT** /user/devicePermissions/{id} | Updates the device permissions assignments |

<a name="getdevicepermissionsresource"></a>
# **GetDevicePermissionsResource**
> DevicePermissionOwners GetDevicePermissionsResource (string id)

Returns all device permissions assignments

Returns all device permissions assignments if the current user has READ permission.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetDevicePermissionsResourceExample
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

            var apiInstance = new DevicePermissionsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.

            try
            {
                // Returns all device permissions assignments
                DevicePermissionOwners result = apiInstance.GetDevicePermissionsResource(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DevicePermissionsApi.GetDevicePermissionsResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetDevicePermissionsResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Returns all device permissions assignments
    ApiResponse<DevicePermissionOwners> response = apiInstance.GetDevicePermissionsResourceWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DevicePermissionsApi.GetDevicePermissionsResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |

### Return type

[**DevicePermissionOwners**](DevicePermissionOwners.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the device permissions assignments are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="putdevicepermissionsresource"></a>
# **PutDevicePermissionsResource**
> void PutDevicePermissionsResource (string id, UpdatedDevicePermissions updatedDevicePermissions, string? accept = null)

Updates the device permissions assignments

Updates the device permissions assignments if the current user has ADMIN permission or CREATE permission and also has all device permissions.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN <b>OR</b> ROLE_USER_MANAGEMENT_CREATE </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutDevicePermissionsResourceExample
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

            var apiInstance = new DevicePermissionsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var updatedDevicePermissions = new UpdatedDevicePermissions(); // UpdatedDevicePermissions | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Updates the device permissions assignments
                apiInstance.PutDevicePermissionsResource(id, updatedDevicePermissions, accept);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DevicePermissionsApi.PutDevicePermissionsResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutDevicePermissionsResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Updates the device permissions assignments
    apiInstance.PutDevicePermissionsResourceWithHttpInfo(id, updatedDevicePermissions, accept);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DevicePermissionsApi.PutDevicePermissionsResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **updatedDevicePermissions** | [**UpdatedDevicePermissions**](UpdatedDevicePermissions.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

void (empty response body)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The device permissions were successfully updated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

