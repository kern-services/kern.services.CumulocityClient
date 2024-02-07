# kern.services.CumulocityClient.Api.ChildOperationsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**DeleteManagedObjectChildAdditionResource**](ChildOperationsApi.md#deletemanagedobjectchildadditionresource) | **DELETE** /inventory/managedObjects/{id}/childAdditions/{childId} | Remove a specific child addition from its parent |
| [**DeleteManagedObjectChildAdditionResourceMultiple**](ChildOperationsApi.md#deletemanagedobjectchildadditionresourcemultiple) | **DELETE** /inventory/managedObjects/{id}/childAdditions | Remove specific child additions from its parent |
| [**DeleteManagedObjectChildAssetResource**](ChildOperationsApi.md#deletemanagedobjectchildassetresource) | **DELETE** /inventory/managedObjects/{id}/childAssets/{childId} | Remove a specific child asset from its parent |
| [**DeleteManagedObjectChildAssetResourceMultiple**](ChildOperationsApi.md#deletemanagedobjectchildassetresourcemultiple) | **DELETE** /inventory/managedObjects/{id}/childAssets | Remove specific child assets from its parent |
| [**DeleteManagedObjectChildDeviceResource**](ChildOperationsApi.md#deletemanagedobjectchilddeviceresource) | **DELETE** /inventory/managedObjects/{id}/childDevices/{childId} | Remove a specific child device from its parent |
| [**DeleteManagedObjectChildDeviceResourceMultiple**](ChildOperationsApi.md#deletemanagedobjectchilddeviceresourcemultiple) | **DELETE** /inventory/managedObjects/{id}/childDevices | Remove specific child devices from its parent |
| [**GetManagedObjectChildAdditionResource**](ChildOperationsApi.md#getmanagedobjectchildadditionresource) | **GET** /inventory/managedObjects/{id}/childAdditions/{childId} | Retrieve a specific child addition of a specific managed object |
| [**GetManagedObjectChildAdditionsResource**](ChildOperationsApi.md#getmanagedobjectchildadditionsresource) | **GET** /inventory/managedObjects/{id}/childAdditions | Retrieve all child additions of a specific managed object |
| [**GetManagedObjectChildAssetResource**](ChildOperationsApi.md#getmanagedobjectchildassetresource) | **GET** /inventory/managedObjects/{id}/childAssets/{childId} | Retrieve a specific child asset of a specific managed object |
| [**GetManagedObjectChildAssetsResource**](ChildOperationsApi.md#getmanagedobjectchildassetsresource) | **GET** /inventory/managedObjects/{id}/childAssets | Retrieve all child assets of a specific managed object |
| [**GetManagedObjectChildDeviceResource**](ChildOperationsApi.md#getmanagedobjectchilddeviceresource) | **GET** /inventory/managedObjects/{id}/childDevices/{childId} | Retrieve a specific child device of a specific managed object |
| [**GetManagedObjectChildDevicesResource**](ChildOperationsApi.md#getmanagedobjectchilddevicesresource) | **GET** /inventory/managedObjects/{id}/childDevices | Retrieve all child devices of a specific managed object |
| [**PostManagedObjectChildAdditionsResource**](ChildOperationsApi.md#postmanagedobjectchildadditionsresource) | **POST** /inventory/managedObjects/{id}/childAdditions | Assign a managed object as child addition |
| [**PostManagedObjectChildAssetsResource**](ChildOperationsApi.md#postmanagedobjectchildassetsresource) | **POST** /inventory/managedObjects/{id}/childAssets | Assign a managed object as child asset |
| [**PostManagedObjectChildDevicesResource**](ChildOperationsApi.md#postmanagedobjectchilddevicesresource) | **POST** /inventory/managedObjects/{id}/childDevices | Assign a managed object as child device |

<a id="deletemanagedobjectchildadditionresource"></a>
# **DeleteManagedObjectChildAdditionResource**
> void DeleteManagedObjectChildAdditionResource (string id, string childId, string? xCumulocityProcessingMode = null)

Remove a specific child addition from its parent

Remove a specific child addition (by a given child ID) from its parent (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source (parent) <b>OR</b> owner of the child <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source (parent) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteManagedObjectChildAdditionResourceExample
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

            var apiInstance = new ChildOperationsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var childId = 72022;  // string | Unique identifier of the child object.
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Remove a specific child addition from its parent
                apiInstance.DeleteManagedObjectChildAdditionResource(id, childId, xCumulocityProcessingMode);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChildOperationsApi.DeleteManagedObjectChildAdditionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteManagedObjectChildAdditionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove a specific child addition from its parent
    apiInstance.DeleteManagedObjectChildAdditionResourceWithHttpInfo(id, childId, xCumulocityProcessingMode);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChildOperationsApi.DeleteManagedObjectChildAdditionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **childId** | **string** | Unique identifier of the child object. |  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

void (empty response body)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **204** | A child addition was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |
| **422** | Invalid data was sent. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="deletemanagedobjectchildadditionresourcemultiple"></a>
# **DeleteManagedObjectChildAdditionResourceMultiple**
> void DeleteManagedObjectChildAdditionResourceMultiple (string id, ChildOperationsAddMultiple childOperationsAddMultiple, string? xCumulocityProcessingMode = null)

Remove specific child additions from its parent

Remove specific child additions (by given child IDs) from its parent (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source (parent) <b>OR</b> owner of the child <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source (parent) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteManagedObjectChildAdditionResourceMultipleExample
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

            var apiInstance = new ChildOperationsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var childOperationsAddMultiple = new ChildOperationsAddMultiple(); // ChildOperationsAddMultiple | 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Remove specific child additions from its parent
                apiInstance.DeleteManagedObjectChildAdditionResourceMultiple(id, childOperationsAddMultiple, xCumulocityProcessingMode);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChildOperationsApi.DeleteManagedObjectChildAdditionResourceMultiple: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteManagedObjectChildAdditionResourceMultipleWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove specific child additions from its parent
    apiInstance.DeleteManagedObjectChildAdditionResourceMultipleWithHttpInfo(id, childOperationsAddMultiple, xCumulocityProcessingMode);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChildOperationsApi.DeleteManagedObjectChildAdditionResourceMultipleWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **childOperationsAddMultiple** | [**ChildOperationsAddMultiple**](ChildOperationsAddMultiple.md) |  |  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

void (empty response body)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.managedobjectreferencecollection+json
 - **Accept**: application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **204** | Child additions were removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="deletemanagedobjectchildassetresource"></a>
# **DeleteManagedObjectChildAssetResource**
> void DeleteManagedObjectChildAssetResource (string id, string childId, string? xCumulocityProcessingMode = null)

Remove a specific child asset from its parent

Remove a specific child asset (by a given child ID) from its parent (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source (parent) <b>OR</b> owner of the child <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source (parent) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteManagedObjectChildAssetResourceExample
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

            var apiInstance = new ChildOperationsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var childId = 72022;  // string | Unique identifier of the child object.
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Remove a specific child asset from its parent
                apiInstance.DeleteManagedObjectChildAssetResource(id, childId, xCumulocityProcessingMode);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChildOperationsApi.DeleteManagedObjectChildAssetResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteManagedObjectChildAssetResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove a specific child asset from its parent
    apiInstance.DeleteManagedObjectChildAssetResourceWithHttpInfo(id, childId, xCumulocityProcessingMode);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChildOperationsApi.DeleteManagedObjectChildAssetResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **childId** | **string** | Unique identifier of the child object. |  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

void (empty response body)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **204** | A child asset was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |
| **422** | Invalid data was sent. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="deletemanagedobjectchildassetresourcemultiple"></a>
# **DeleteManagedObjectChildAssetResourceMultiple**
> void DeleteManagedObjectChildAssetResourceMultiple (string id, ChildOperationsAddMultiple childOperationsAddMultiple, string? xCumulocityProcessingMode = null)

Remove specific child assets from its parent

Remove specific child assets (by given child IDs) from its parent (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source (parent) <b>OR</b> owner of the child <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source (parent) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteManagedObjectChildAssetResourceMultipleExample
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

            var apiInstance = new ChildOperationsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var childOperationsAddMultiple = new ChildOperationsAddMultiple(); // ChildOperationsAddMultiple | 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Remove specific child assets from its parent
                apiInstance.DeleteManagedObjectChildAssetResourceMultiple(id, childOperationsAddMultiple, xCumulocityProcessingMode);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChildOperationsApi.DeleteManagedObjectChildAssetResourceMultiple: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteManagedObjectChildAssetResourceMultipleWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove specific child assets from its parent
    apiInstance.DeleteManagedObjectChildAssetResourceMultipleWithHttpInfo(id, childOperationsAddMultiple, xCumulocityProcessingMode);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChildOperationsApi.DeleteManagedObjectChildAssetResourceMultipleWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **childOperationsAddMultiple** | [**ChildOperationsAddMultiple**](ChildOperationsAddMultiple.md) |  |  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

void (empty response body)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.managedobjectreferencecollection+json
 - **Accept**: application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **204** | Child assets were removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="deletemanagedobjectchilddeviceresource"></a>
# **DeleteManagedObjectChildDeviceResource**
> void DeleteManagedObjectChildDeviceResource (string id, string childId, string? xCumulocityProcessingMode = null)

Remove a specific child device from its parent

Remove a specific child device (by a given child ID) from its parent (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source (parent) <b>OR</b> owner of the child <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source (parent) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteManagedObjectChildDeviceResourceExample
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

            var apiInstance = new ChildOperationsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var childId = 72022;  // string | Unique identifier of the child object.
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Remove a specific child device from its parent
                apiInstance.DeleteManagedObjectChildDeviceResource(id, childId, xCumulocityProcessingMode);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChildOperationsApi.DeleteManagedObjectChildDeviceResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteManagedObjectChildDeviceResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove a specific child device from its parent
    apiInstance.DeleteManagedObjectChildDeviceResourceWithHttpInfo(id, childId, xCumulocityProcessingMode);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChildOperationsApi.DeleteManagedObjectChildDeviceResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **childId** | **string** | Unique identifier of the child object. |  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

void (empty response body)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **204** | A child device was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |
| **422** | Invalid data was sent. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="deletemanagedobjectchilddeviceresourcemultiple"></a>
# **DeleteManagedObjectChildDeviceResourceMultiple**
> void DeleteManagedObjectChildDeviceResourceMultiple (string id, ChildOperationsAddMultiple childOperationsAddMultiple, string? xCumulocityProcessingMode = null)

Remove specific child devices from its parent

Remove specific child devices (by given child IDs) from its parent (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source (parent) <b>OR</b> owner of the child <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source (parent) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteManagedObjectChildDeviceResourceMultipleExample
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

            var apiInstance = new ChildOperationsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var childOperationsAddMultiple = new ChildOperationsAddMultiple(); // ChildOperationsAddMultiple | 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Remove specific child devices from its parent
                apiInstance.DeleteManagedObjectChildDeviceResourceMultiple(id, childOperationsAddMultiple, xCumulocityProcessingMode);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChildOperationsApi.DeleteManagedObjectChildDeviceResourceMultiple: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteManagedObjectChildDeviceResourceMultipleWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove specific child devices from its parent
    apiInstance.DeleteManagedObjectChildDeviceResourceMultipleWithHttpInfo(id, childOperationsAddMultiple, xCumulocityProcessingMode);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChildOperationsApi.DeleteManagedObjectChildDeviceResourceMultipleWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **childOperationsAddMultiple** | [**ChildOperationsAddMultiple**](ChildOperationsAddMultiple.md) |  |  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

void (empty response body)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.managedobjectreferencecollection+json
 - **Accept**: application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **204** | Child devices were removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getmanagedobjectchildadditionresource"></a>
# **GetManagedObjectChildAdditionResource**
> GetManagedObjectChildAdditionResource200Response GetManagedObjectChildAdditionResource (string id, string childId)

Retrieve a specific child addition of a specific managed object

Retrieve a specific child addition (by a given child ID) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> MANAGE_OBJECT_READ permission on the source (parent) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetManagedObjectChildAdditionResourceExample
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

            var apiInstance = new ChildOperationsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var childId = 72022;  // string | Unique identifier of the child object.

            try
            {
                // Retrieve a specific child addition of a specific managed object
                GetManagedObjectChildAdditionResource200Response result = apiInstance.GetManagedObjectChildAdditionResource(id, childId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChildOperationsApi.GetManagedObjectChildAdditionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetManagedObjectChildAdditionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a specific child addition of a specific managed object
    ApiResponse<GetManagedObjectChildAdditionResource200Response> response = apiInstance.GetManagedObjectChildAdditionResourceWithHttpInfo(id, childId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChildOperationsApi.GetManagedObjectChildAdditionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **childId** | **string** | Unique identifier of the child object. |  |

### Return type

[**GetManagedObjectChildAdditionResource200Response**](GetManagedObjectChildAdditionResource200Response.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.managedobjectreference+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the child addition is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |
| **422** | Invalid data was sent. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getmanagedobjectchildadditionsresource"></a>
# **GetManagedObjectChildAdditionsResource**
> ManagedObjectReferenceCollection GetManagedObjectChildAdditionsResource (string id, int? currentPage = null, int? pageSize = null, string? query = null, bool? withChildren = null, bool? withChildrenCount = null, bool? withTotalElements = null, bool? withTotalPages = null)

Retrieve all child additions of a specific managed object

Retrieve all child additions of a specific managed object by a given ID, or a subset based on queries.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetManagedObjectChildAdditionsResourceExample
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

            var apiInstance = new ChildOperationsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var query = $filter=(owner+eq+'manga');  // string? | Use query language to perform operations and/or filter the results. Details about the properties and supported operations can be found in [Query language](#tag/Query-language). (optional) 
            var withChildren = false;  // bool? | Determines if children with ID and name should be returned when fetching the managed object. Set it to `false` to improve query performance. (optional)  (default to true)
            var withChildrenCount = true;  // bool? | When set to `true`, the returned result will contain the total number of children in the respective objects (`childAdditions`, `childAssets` and `childDevices`). (optional)  (default to false)
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve all child additions of a specific managed object
                ManagedObjectReferenceCollection result = apiInstance.GetManagedObjectChildAdditionsResource(id, currentPage, pageSize, query, withChildren, withChildrenCount, withTotalElements, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChildOperationsApi.GetManagedObjectChildAdditionsResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetManagedObjectChildAdditionsResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all child additions of a specific managed object
    ApiResponse<ManagedObjectReferenceCollection> response = apiInstance.GetManagedObjectChildAdditionsResourceWithHttpInfo(id, currentPage, pageSize, query, withChildren, withChildrenCount, withTotalElements, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChildOperationsApi.GetManagedObjectChildAdditionsResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **query** | **string?** | Use query language to perform operations and/or filter the results. Details about the properties and supported operations can be found in [Query language](#tag/Query-language). | [optional]  |
| **withChildren** | **bool?** | Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. | [optional] [default to true] |
| **withChildrenCount** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). | [optional] [default to false] |
| **withTotalElements** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**ManagedObjectReferenceCollection**](ManagedObjectReferenceCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.managedobjectreferencecollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and all child additions are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |
| **422** | Invalid data was sent. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getmanagedobjectchildassetresource"></a>
# **GetManagedObjectChildAssetResource**
> GetManagedObjectChildAssetResource200Response GetManagedObjectChildAssetResource (string id, string childId)

Retrieve a specific child asset of a specific managed object

Retrieve a specific child asset (by a given child ID) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> MANAGE_OBJECT_READ permission on the source (parent) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetManagedObjectChildAssetResourceExample
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

            var apiInstance = new ChildOperationsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var childId = 72022;  // string | Unique identifier of the child object.

            try
            {
                // Retrieve a specific child asset of a specific managed object
                GetManagedObjectChildAssetResource200Response result = apiInstance.GetManagedObjectChildAssetResource(id, childId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChildOperationsApi.GetManagedObjectChildAssetResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetManagedObjectChildAssetResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a specific child asset of a specific managed object
    ApiResponse<GetManagedObjectChildAssetResource200Response> response = apiInstance.GetManagedObjectChildAssetResourceWithHttpInfo(id, childId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChildOperationsApi.GetManagedObjectChildAssetResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **childId** | **string** | Unique identifier of the child object. |  |

### Return type

[**GetManagedObjectChildAssetResource200Response**](GetManagedObjectChildAssetResource200Response.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.managedobjectreference+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the child asset is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |
| **422** | Invalid data was sent. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getmanagedobjectchildassetsresource"></a>
# **GetManagedObjectChildAssetsResource**
> GetManagedObjectChildAssetsResource200Response GetManagedObjectChildAssetsResource (string id, int? currentPage = null, int? pageSize = null, string? query = null, bool? withChildren = null, bool? withChildrenCount = null, bool? withTotalElements = null, bool? withTotalPages = null)

Retrieve all child assets of a specific managed object

Retrieve all child assets of a specific managed object by a given ID, or a subset based on queries.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetManagedObjectChildAssetsResourceExample
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

            var apiInstance = new ChildOperationsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var query = $filter=(owner+eq+'manga');  // string? | Use query language to perform operations and/or filter the results. Details about the properties and supported operations can be found in [Query language](#tag/Query-language). (optional) 
            var withChildren = false;  // bool? | Determines if children with ID and name should be returned when fetching the managed object. Set it to `false` to improve query performance. (optional)  (default to true)
            var withChildrenCount = true;  // bool? | When set to `true`, the returned result will contain the total number of children in the respective objects (`childAdditions`, `childAssets` and `childDevices`). (optional)  (default to false)
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve all child assets of a specific managed object
                GetManagedObjectChildAssetsResource200Response result = apiInstance.GetManagedObjectChildAssetsResource(id, currentPage, pageSize, query, withChildren, withChildrenCount, withTotalElements, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChildOperationsApi.GetManagedObjectChildAssetsResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetManagedObjectChildAssetsResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all child assets of a specific managed object
    ApiResponse<GetManagedObjectChildAssetsResource200Response> response = apiInstance.GetManagedObjectChildAssetsResourceWithHttpInfo(id, currentPage, pageSize, query, withChildren, withChildrenCount, withTotalElements, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChildOperationsApi.GetManagedObjectChildAssetsResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **query** | **string?** | Use query language to perform operations and/or filter the results. Details about the properties and supported operations can be found in [Query language](#tag/Query-language). | [optional]  |
| **withChildren** | **bool?** | Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. | [optional] [default to true] |
| **withChildrenCount** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). | [optional] [default to false] |
| **withTotalElements** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**GetManagedObjectChildAssetsResource200Response**](GetManagedObjectChildAssetsResource200Response.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.managedobjectreferencecollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and all child assets are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |
| **422** | Invalid data was sent. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getmanagedobjectchilddeviceresource"></a>
# **GetManagedObjectChildDeviceResource**
> GetManagedObjectChildDeviceResource200Response GetManagedObjectChildDeviceResource (string id, string childId)

Retrieve a specific child device of a specific managed object

Retrieve a specific child device (by a given child ID) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> MANAGE_OBJECT_READ permission on the source (parent) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetManagedObjectChildDeviceResourceExample
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

            var apiInstance = new ChildOperationsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var childId = 72022;  // string | Unique identifier of the child object.

            try
            {
                // Retrieve a specific child device of a specific managed object
                GetManagedObjectChildDeviceResource200Response result = apiInstance.GetManagedObjectChildDeviceResource(id, childId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChildOperationsApi.GetManagedObjectChildDeviceResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetManagedObjectChildDeviceResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a specific child device of a specific managed object
    ApiResponse<GetManagedObjectChildDeviceResource200Response> response = apiInstance.GetManagedObjectChildDeviceResourceWithHttpInfo(id, childId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChildOperationsApi.GetManagedObjectChildDeviceResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **childId** | **string** | Unique identifier of the child object. |  |

### Return type

[**GetManagedObjectChildDeviceResource200Response**](GetManagedObjectChildDeviceResource200Response.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.managedobjectreference+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the child device is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |
| **422** | Invalid data was sent. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getmanagedobjectchilddevicesresource"></a>
# **GetManagedObjectChildDevicesResource**
> GetManagedObjectChildDevicesResource200Response GetManagedObjectChildDevicesResource (string id, int? currentPage = null, int? pageSize = null, string? query = null, bool? withChildren = null, bool? withChildrenCount = null, bool? withTotalElements = null, bool? withTotalPages = null)

Retrieve all child devices of a specific managed object

Retrieve all child devices of a specific managed object by a given ID, or a subset based on queries.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetManagedObjectChildDevicesResourceExample
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

            var apiInstance = new ChildOperationsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var query = $filter=(owner+eq+'manga');  // string? | Use query language to perform operations and/or filter the results. Details about the properties and supported operations can be found in [Query language](#tag/Query-language). (optional) 
            var withChildren = false;  // bool? | Determines if children with ID and name should be returned when fetching the managed object. Set it to `false` to improve query performance. (optional)  (default to true)
            var withChildrenCount = true;  // bool? | When set to `true`, the returned result will contain the total number of children in the respective objects (`childAdditions`, `childAssets` and `childDevices`). (optional)  (default to false)
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve all child devices of a specific managed object
                GetManagedObjectChildDevicesResource200Response result = apiInstance.GetManagedObjectChildDevicesResource(id, currentPage, pageSize, query, withChildren, withChildrenCount, withTotalElements, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChildOperationsApi.GetManagedObjectChildDevicesResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetManagedObjectChildDevicesResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all child devices of a specific managed object
    ApiResponse<GetManagedObjectChildDevicesResource200Response> response = apiInstance.GetManagedObjectChildDevicesResourceWithHttpInfo(id, currentPage, pageSize, query, withChildren, withChildrenCount, withTotalElements, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChildOperationsApi.GetManagedObjectChildDevicesResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **query** | **string?** | Use query language to perform operations and/or filter the results. Details about the properties and supported operations can be found in [Query language](#tag/Query-language). | [optional]  |
| **withChildren** | **bool?** | Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. | [optional] [default to true] |
| **withChildrenCount** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). | [optional] [default to false] |
| **withTotalElements** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**GetManagedObjectChildDevicesResource200Response**](GetManagedObjectChildDevicesResource200Response.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.managedobjectreferencecollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and all child devices are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |
| **422** | Invalid data was sent. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="postmanagedobjectchildadditionsresource"></a>
# **PostManagedObjectChildAdditionsResource**
> void PostManagedObjectChildAdditionsResource (string id, ChildOperationsAddOne childOperationsAddOne, string? accept = null, string? xCumulocityProcessingMode = null)

Assign a managed object as child addition

The possible ways to assign child objects are:  *  Assign an existing managed object (by a given child ID) as child addition of another managed object (by a given ID). *  Assign multiple existing managed objects (by given child IDs) as child additions of another managed object (by a given ID). *  Create a managed object in the inventory and assign it as a child addition to another managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> ((owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source) <b>AND</b> (owner of the child <b>OR</b> MANAGE_OBJECT_ADMIN permission on the child)) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostManagedObjectChildAdditionsResourceExample
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

            var apiInstance = new ChildOperationsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var childOperationsAddOne = new ChildOperationsAddOne(); // ChildOperationsAddOne | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Assign a managed object as child addition
                apiInstance.PostManagedObjectChildAdditionsResource(id, childOperationsAddOne, accept, xCumulocityProcessingMode);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChildOperationsApi.PostManagedObjectChildAdditionsResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostManagedObjectChildAdditionsResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Assign a managed object as child addition
    apiInstance.PostManagedObjectChildAdditionsResourceWithHttpInfo(id, childOperationsAddOne, accept, xCumulocityProcessingMode);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChildOperationsApi.PostManagedObjectChildAdditionsResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **childOperationsAddOne** | [**ChildOperationsAddOne**](ChildOperationsAddOne.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

void (empty response body)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.managedobjectreference+json, application/vnd.com.nsn.cumulocity.managedobjectreferencecollection+json, application/vnd.com.nsn.cumulocity.managedobject+json
 - **Accept**: application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | A managed object was assigned as child addition. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="postmanagedobjectchildassetsresource"></a>
# **PostManagedObjectChildAssetsResource**
> void PostManagedObjectChildAssetsResource (string id, ChildOperationsAddOne childOperationsAddOne, string? accept = null, string? xCumulocityProcessingMode = null)

Assign a managed object as child asset

The possible ways to assign child objects are:  *  Assign an existing managed object (by a given child ID) as child asset of another managed object (by a given ID). *  Assign multiple existing managed objects (by given child IDs) as child assets of another managed object (by a given ID). *  Create a managed object in the inventory and assign it as a child asset to another managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> ((owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source) <b>AND</b> (owner of the child <b>OR</b> MANAGE_OBJECT_ADMIN permission on the child)) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostManagedObjectChildAssetsResourceExample
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

            var apiInstance = new ChildOperationsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var childOperationsAddOne = new ChildOperationsAddOne(); // ChildOperationsAddOne | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Assign a managed object as child asset
                apiInstance.PostManagedObjectChildAssetsResource(id, childOperationsAddOne, accept, xCumulocityProcessingMode);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChildOperationsApi.PostManagedObjectChildAssetsResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostManagedObjectChildAssetsResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Assign a managed object as child asset
    apiInstance.PostManagedObjectChildAssetsResourceWithHttpInfo(id, childOperationsAddOne, accept, xCumulocityProcessingMode);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChildOperationsApi.PostManagedObjectChildAssetsResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **childOperationsAddOne** | [**ChildOperationsAddOne**](ChildOperationsAddOne.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

void (empty response body)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.managedobjectreference+json, application/vnd.com.nsn.cumulocity.managedobjectreferencecollection+json, application/vnd.com.nsn.cumulocity.managedobject+json
 - **Accept**: application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | A managed object was assigned as child asset. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="postmanagedobjectchilddevicesresource"></a>
# **PostManagedObjectChildDevicesResource**
> void PostManagedObjectChildDevicesResource (string id, ChildOperationsAddOne childOperationsAddOne, string? accept = null, string? xCumulocityProcessingMode = null)

Assign a managed object as child device

The possible ways to assign child objects are:  *  Assign an existing managed object (by a given child ID) as child device of another managed object (by a given ID). *  Assign multiple existing managed objects (by given child IDs) as child devices of another managed object (by a given ID). *  Create a managed object in the inventory and assign it as a child device to another managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> ((owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source) <b>AND</b> (owner of the child <b>OR</b> MANAGE_OBJECT_ADMIN permission on the child)) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostManagedObjectChildDevicesResourceExample
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

            var apiInstance = new ChildOperationsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var childOperationsAddOne = new ChildOperationsAddOne(); // ChildOperationsAddOne | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Assign a managed object as child device
                apiInstance.PostManagedObjectChildDevicesResource(id, childOperationsAddOne, accept, xCumulocityProcessingMode);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ChildOperationsApi.PostManagedObjectChildDevicesResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostManagedObjectChildDevicesResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Assign a managed object as child device
    apiInstance.PostManagedObjectChildDevicesResourceWithHttpInfo(id, childOperationsAddOne, accept, xCumulocityProcessingMode);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ChildOperationsApi.PostManagedObjectChildDevicesResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **childOperationsAddOne** | [**ChildOperationsAddOne**](ChildOperationsAddOne.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

void (empty response body)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.managedobjectreference+json, application/vnd.com.nsn.cumulocity.managedobjectreferencecollection+json, application/vnd.com.nsn.cumulocity.managedobject+json
 - **Accept**: application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | A managed object was assigned as child device. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

