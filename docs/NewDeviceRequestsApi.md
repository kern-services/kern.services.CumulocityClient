# kern.services.CumulocityClient.Api.NewDeviceRequestsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**DeleteNewDeviceRequestResource**](NewDeviceRequestsApi.md#deletenewdevicerequestresource) | **DELETE** /devicecontrol/newDeviceRequests/{requestId} | Delete a specific new device request |
| [**GetNewDeviceRequestCollectionResource**](NewDeviceRequestsApi.md#getnewdevicerequestcollectionresource) | **GET** /devicecontrol/newDeviceRequests | Retrieve a list of new device requests |
| [**GetNewDeviceRequestResource**](NewDeviceRequestsApi.md#getnewdevicerequestresource) | **GET** /devicecontrol/newDeviceRequests/{requestId} | Retrieve a specific new device request |
| [**PostNewDeviceRequestCollectionResource**](NewDeviceRequestsApi.md#postnewdevicerequestcollectionresource) | **POST** /devicecontrol/newDeviceRequests | Create a new device request |
| [**PutNewDeviceRequestResource**](NewDeviceRequestsApi.md#putnewdevicerequestresource) | **PUT** /devicecontrol/newDeviceRequests/{requestId} | Update a specific new device request status |

<a id="deletenewdevicerequestresource"></a>
# **DeleteNewDeviceRequestResource**
> void DeleteNewDeviceRequestResource (string requestId)

Delete a specific new device request

Delete a specific new device request (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteNewDeviceRequestResourceExample
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

            var apiInstance = new NewDeviceRequestsApi(config);
            var requestId = 54545454;  // string | Unique identifier of the new device request.

            try
            {
                // Delete a specific new device request
                apiInstance.DeleteNewDeviceRequestResource(requestId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling NewDeviceRequestsApi.DeleteNewDeviceRequestResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteNewDeviceRequestResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete a specific new device request
    apiInstance.DeleteNewDeviceRequestResourceWithHttpInfo(requestId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling NewDeviceRequestsApi.DeleteNewDeviceRequestResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **requestId** | **string** | Unique identifier of the new device request. |  |

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
| **204** | A new device request was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **404** | New device request not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getnewdevicerequestcollectionresource"></a>
# **GetNewDeviceRequestCollectionResource**
> NewDeviceRequestCollection GetNewDeviceRequestCollectionResource (int? currentPage = null, int? pageSize = null, bool? withTotalElements = null, bool? withTotalPages = null)

Retrieve a list of new device requests

Retrieve a list of new device requests.  <section><h5>Required roles</h5> ROLE_DEVICE_CONTROL_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetNewDeviceRequestCollectionResourceExample
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

            var apiInstance = new NewDeviceRequestsApi(config);
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve a list of new device requests
                NewDeviceRequestCollection result = apiInstance.GetNewDeviceRequestCollectionResource(currentPage, pageSize, withTotalElements, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling NewDeviceRequestsApi.GetNewDeviceRequestCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetNewDeviceRequestCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a list of new device requests
    ApiResponse<NewDeviceRequestCollection> response = apiInstance.GetNewDeviceRequestCollectionResourceWithHttpInfo(currentPage, pageSize, withTotalElements, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling NewDeviceRequestsApi.GetNewDeviceRequestCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **withTotalElements** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**NewDeviceRequestCollection**](NewDeviceRequestCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.newdevicerequestcollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the list of new device requests sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getnewdevicerequestresource"></a>
# **GetNewDeviceRequestResource**
> NewDeviceRequest GetNewDeviceRequestResource (string requestId)

Retrieve a specific new device request

Retrieve a specific new device request (by a given ID).  <section><h5>Required roles</h5> ROLE_DEVICE_CONTROL_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetNewDeviceRequestResourceExample
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

            var apiInstance = new NewDeviceRequestsApi(config);
            var requestId = 54545454;  // string | Unique identifier of the new device request.

            try
            {
                // Retrieve a specific new device request
                NewDeviceRequest result = apiInstance.GetNewDeviceRequestResource(requestId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling NewDeviceRequestsApi.GetNewDeviceRequestResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetNewDeviceRequestResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a specific new device request
    ApiResponse<NewDeviceRequest> response = apiInstance.GetNewDeviceRequestResourceWithHttpInfo(requestId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling NewDeviceRequestsApi.GetNewDeviceRequestResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **requestId** | **string** | Unique identifier of the new device request. |  |

### Return type

[**NewDeviceRequest**](NewDeviceRequest.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.newdevicerequest+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the new device request is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | New device request not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="postnewdevicerequestcollectionresource"></a>
# **PostNewDeviceRequestCollectionResource**
> NewDeviceRequest PostNewDeviceRequestCollectionResource (PostNewDeviceRequestCollectionResourceRequest postNewDeviceRequestCollectionResourceRequest, string? accept = null, string? xCumulocityProcessingMode = null)

Create a new device request

Create a new device request.  <section><h5>Required roles</h5> ROLE_DEVICE_CONTROL_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostNewDeviceRequestCollectionResourceExample
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

            var apiInstance = new NewDeviceRequestsApi(config);
            var postNewDeviceRequestCollectionResourceRequest = new PostNewDeviceRequestCollectionResourceRequest(); // PostNewDeviceRequestCollectionResourceRequest | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Create a new device request
                NewDeviceRequest result = apiInstance.PostNewDeviceRequestCollectionResource(postNewDeviceRequestCollectionResourceRequest, accept, xCumulocityProcessingMode);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling NewDeviceRequestsApi.PostNewDeviceRequestCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostNewDeviceRequestCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create a new device request
    ApiResponse<NewDeviceRequest> response = apiInstance.PostNewDeviceRequestCollectionResourceWithHttpInfo(postNewDeviceRequestCollectionResourceRequest, accept, xCumulocityProcessingMode);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling NewDeviceRequestsApi.PostNewDeviceRequestCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **postNewDeviceRequestCollectionResourceRequest** | [**PostNewDeviceRequestCollectionResourceRequest**](PostNewDeviceRequestCollectionResourceRequest.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

[**NewDeviceRequest**](NewDeviceRequest.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.newdevicerequest+json
 - **Accept**: application/vnd.com.nsn.cumulocity.newdevicerequest+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | A new device request was created. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="putnewdevicerequestresource"></a>
# **PutNewDeviceRequestResource**
> NewDeviceRequest PutNewDeviceRequestResource (string requestId, PutNewDeviceRequestResourceRequest putNewDeviceRequestResourceRequest, string? accept = null)

Update a specific new device request status

Update a specific new device request (by a given ID). You can only update its status.  <section><h5>Required roles</h5> ROLE_DEVICE_CONTROL_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutNewDeviceRequestResourceExample
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

            var apiInstance = new NewDeviceRequestsApi(config);
            var requestId = 54545454;  // string | Unique identifier of the new device request.
            var putNewDeviceRequestResourceRequest = new PutNewDeviceRequestResourceRequest(); // PutNewDeviceRequestResourceRequest | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Update a specific new device request status
                NewDeviceRequest result = apiInstance.PutNewDeviceRequestResource(requestId, putNewDeviceRequestResourceRequest, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling NewDeviceRequestsApi.PutNewDeviceRequestResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutNewDeviceRequestResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update a specific new device request status
    ApiResponse<NewDeviceRequest> response = apiInstance.PutNewDeviceRequestResourceWithHttpInfo(requestId, putNewDeviceRequestResourceRequest, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling NewDeviceRequestsApi.PutNewDeviceRequestResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **requestId** | **string** | Unique identifier of the new device request. |  |
| **putNewDeviceRequestResourceRequest** | [**PutNewDeviceRequestResourceRequest**](PutNewDeviceRequestResourceRequest.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**NewDeviceRequest**](NewDeviceRequest.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.newdevicerequest+json
 - **Accept**: application/vnd.com.nsn.cumulocity.newdevicerequest+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A new device request was updated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | New device request not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

