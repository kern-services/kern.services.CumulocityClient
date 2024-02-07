# kern.services.CumulocityClient.Api.ExternalIDsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**DeleteExternalIDResource**](ExternalIDsApi.md#deleteexternalidresource) | **DELETE** /identity/externalIds/{type}/{externalId} | Remove a specific external ID |
| [**GetExternalIDCollectionResource**](ExternalIDsApi.md#getexternalidcollectionresource) | **GET** /identity/globalIds/{id}/externalIds | Retrieve all external IDs of a specific managed object |
| [**GetExternalIDResource**](ExternalIDsApi.md#getexternalidresource) | **GET** /identity/externalIds/{type}/{externalId} | Retrieve a specific external ID |
| [**PostExternalIDCollectionResource**](ExternalIDsApi.md#postexternalidcollectionresource) | **POST** /identity/globalIds/{id}/externalIds | Create an external ID |

<a id="deleteexternalidresource"></a>
# **DeleteExternalIDResource**
> void DeleteExternalIDResource (string type, string externalId)

Remove a specific external ID

Remove a specific external ID of a particular type.  <section><h5>Required roles</h5> ROLE_IDENTITY_ADMIN <b>OR</b> owner of the resource <b>OR</b> MANAGED_OBJECT_ADMIN permission on the resource </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteExternalIDResourceExample
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

            var apiInstance = new ExternalIDsApi(config);
            var type = c8y_Serial;  // string | The identifier used in the external system that Cumulocity IoT interfaces with.
            var externalId = simulator_145074_1;  // string | The type of the external identifier.

            try
            {
                // Remove a specific external ID
                apiInstance.DeleteExternalIDResource(type, externalId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ExternalIDsApi.DeleteExternalIDResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteExternalIDResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove a specific external ID
    apiInstance.DeleteExternalIDResourceWithHttpInfo(type, externalId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ExternalIDsApi.DeleteExternalIDResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **type** | **string** | The identifier used in the external system that Cumulocity IoT interfaces with. |  |
| **externalId** | **string** | The type of the external identifier. |  |

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
| **204** | An external ID was deleted. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | External ID not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getexternalidcollectionresource"></a>
# **GetExternalIDCollectionResource**
> ExternalIds GetExternalIDCollectionResource (string id)

Retrieve all external IDs of a specific managed object

Retrieve all external IDs of a existing managed object (identified by ID).  <section><h5>Required roles</h5> ROLE_IDENTITY_READ <b>OR</b> owner of the resource <b>OR</b> MANAGED_OBJECT_READ permission on the resource </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetExternalIDCollectionResourceExample
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

            var apiInstance = new ExternalIDsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.

            try
            {
                // Retrieve all external IDs of a specific managed object
                ExternalIds result = apiInstance.GetExternalIDCollectionResource(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ExternalIDsApi.GetExternalIDCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetExternalIDCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all external IDs of a specific managed object
    ApiResponse<ExternalIds> response = apiInstance.GetExternalIDCollectionResourceWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ExternalIDsApi.GetExternalIDCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |

### Return type

[**ExternalIds**](ExternalIds.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.externalidcollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and all the external IDs are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getexternalidresource"></a>
# **GetExternalIDResource**
> ExternalId GetExternalIDResource (string type, string externalId)

Retrieve a specific external ID

Retrieve a specific external ID of a particular type.  <section><h5>Required roles</h5> ROLE_IDENTITY_READ <b>OR</b> owner of the resource <b>OR</b> MANAGED_OBJECT_READ permission on the resource </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetExternalIDResourceExample
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

            var apiInstance = new ExternalIDsApi(config);
            var type = c8y_Serial;  // string | The identifier used in the external system that Cumulocity IoT interfaces with.
            var externalId = simulator_145074_1;  // string | The type of the external identifier.

            try
            {
                // Retrieve a specific external ID
                ExternalId result = apiInstance.GetExternalIDResource(type, externalId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ExternalIDsApi.GetExternalIDResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetExternalIDResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a specific external ID
    ApiResponse<ExternalId> response = apiInstance.GetExternalIDResourceWithHttpInfo(type, externalId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ExternalIDsApi.GetExternalIDResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **type** | **string** | The identifier used in the external system that Cumulocity IoT interfaces with. |  |
| **externalId** | **string** | The type of the external identifier. |  |

### Return type

[**ExternalId**](ExternalId.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.externalid+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the external ID is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | External ID not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="postexternalidcollectionresource"></a>
# **PostExternalIDCollectionResource**
> ExternalId PostExternalIDCollectionResource (string id, ExternalId externalId, string? accept = null)

Create an external ID

Create an external ID for an existing managed object (identified by ID).  <section><h5>Required roles</h5> ROLE_IDENTITY_ADMIN <b>OR</b> owner of the resource <b>OR</b> MANAGED_OBJECT_ADMIN permission on the resource </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostExternalIDCollectionResourceExample
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

            var apiInstance = new ExternalIDsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var externalId = new ExternalId(); // ExternalId | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Create an external ID
                ExternalId result = apiInstance.PostExternalIDCollectionResource(id, externalId, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ExternalIDsApi.PostExternalIDCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostExternalIDCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create an external ID
    ApiResponse<ExternalId> response = apiInstance.PostExternalIDCollectionResourceWithHttpInfo(id, externalId, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ExternalIDsApi.PostExternalIDCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **externalId** | [**ExternalId**](ExternalId.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**ExternalId**](ExternalId.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.externalid+json
 - **Accept**: application/vnd.com.nsn.cumulocity.externalid+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | An external ID was created. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **409** | Duplicate – Identity already bound to a different Global ID. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

