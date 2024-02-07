# kern.services.CumulocityClient.Api.OperationsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteOperationCollectionResource**](OperationsApi.md#deleteoperationcollectionresource) | **DELETE** /devicecontrol/operations | Delete a list of operations
[**GetOperationCollectionResource**](OperationsApi.md#getoperationcollectionresource) | **GET** /devicecontrol/operations | Retrieve a list of operations
[**GetOperationResource**](OperationsApi.md#getoperationresource) | **GET** /devicecontrol/operations/{id} | Retrieve a specific operation
[**PostOperationCollectionResource**](OperationsApi.md#postoperationcollectionresource) | **POST** /devicecontrol/operations | Create an operation
[**PutOperationResource**](OperationsApi.md#putoperationresource) | **PUT** /devicecontrol/operations/{id} | Update a specific operation status



## DeleteOperationCollectionResource

> void DeleteOperationCollectionResource (string xCumulocityProcessingMode = null, string agentId = null, DateTime? dateFrom = null, DateTime? dateTo = null, string deviceId = null, string status = null)

Delete a list of operations

Delete a list of operations.  The DELETE method allows for deletion of operation collections.  <section><h5>Required roles</h5> ROLE_DEVICE_CONTROL_ADMIN </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteOperationCollectionResourceExample
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

            var apiInstance = new OperationsApi(Configuration.Default);
            var xCumulocityProcessingMode = PERSISTENT;  // string | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)
            var agentId = simulator_145074_1;  // string | An agent ID that may be part of the operation. (optional) 
            var dateFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the operation. (optional) 
            var dateTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the operation. (optional) 
            var deviceId = 1234;  // string | The ID of the device the operation is performed for. (optional) 
            var status = FAILED;  // string | Status of the operation. (optional) 

            try
            {
                // Delete a list of operations
                apiInstance.DeleteOperationCollectionResource(xCumulocityProcessingMode, agentId, dateFrom, dateTo, deviceId, status);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling OperationsApi.DeleteOperationCollectionResource: " + e.Message );
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
 **xCumulocityProcessingMode** | **string**| Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT]
 **agentId** | **string**| An agent ID that may be part of the operation. | [optional] 
 **dateFrom** | **DateTime?**| Start date or date and time of the operation. | [optional] 
 **dateTo** | **DateTime?**| End date or date and time of the operation. | [optional] 
 **deviceId** | **string**| The ID of the device the operation is performed for. | [optional] 
 **status** | **string**| Status of the operation. | [optional] 

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
| **204** | A list of operations was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)


## GetOperationCollectionResource

> OperationCollection GetOperationCollectionResource (string agentId = null, string bulkOperationId = null, int? currentPage = null, DateTime? dateFrom = null, DateTime? dateTo = null, string deviceId = null, string fragmentType = null, int? pageSize = null, bool? revert = null, string status = null, bool? withTotalElements = null, bool? withTotalPages = null)

Retrieve a list of operations

Retrieve a list of operations.  Notes about operation collections:  * The embedded operation object contains `deviceExternalIDs` only when queried with an `agentId` parameter. * The embedded operation object is filled with `deviceName`, but only when requesting resource: Get a collection of operations. * Operations are returned in the order of their ascending IDs.  <section><h5>Required roles</h5> ROLE_DEVICE_CONTROL_READ </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetOperationCollectionResourceExample
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

            var apiInstance = new OperationsApi(Configuration.Default);
            var agentId = simulator_145074_1;  // string | An agent ID that may be part of the operation. If this parameter is set, the operation response objects contain the `deviceExternalIDs` object. (optional) 
            var bulkOperationId = 1237;  // string | The bulk operation ID that this operation belongs to. (optional) 
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var dateFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the operation. (optional) 
            var dateTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the operation. (optional) 
            var deviceId = 1234;  // string | The ID of the device the operation is performed for. (optional) 
            var fragmentType = com_cumulocity_model_WebCamDevice;  // string | The type of fragment that must be part of the operation. (optional) 
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var revert = true;  // bool? | If you are using a range query (that is, at least one of the `dateFrom` or `dateTo` parameters is included in the request), then setting `revert=true` will sort the results by the newest operations first. By default, the results are sorted by the oldest operations first.  (optional)  (default to false)
            var status = FAILED;  // string | Status of the operation. (optional) 
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve a list of operations
                OperationCollection result = apiInstance.GetOperationCollectionResource(agentId, bulkOperationId, currentPage, dateFrom, dateTo, deviceId, fragmentType, pageSize, revert, status, withTotalElements, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling OperationsApi.GetOperationCollectionResource: " + e.Message );
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
 **agentId** | **string**| An agent ID that may be part of the operation. If this parameter is set, the operation response objects contain the &#x60;deviceExternalIDs&#x60; object. | [optional] 
 **bulkOperationId** | **string**| The bulk operation ID that this operation belongs to. | [optional] 
 **currentPage** | **int?**| The current page of the paginated results. | [optional] [default to 1]
 **dateFrom** | **DateTime?**| Start date or date and time of the operation. | [optional] 
 **dateTo** | **DateTime?**| End date or date and time of the operation. | [optional] 
 **deviceId** | **string**| The ID of the device the operation is performed for. | [optional] 
 **fragmentType** | **string**| The type of fragment that must be part of the operation. | [optional] 
 **pageSize** | **int?**| Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5]
 **revert** | **bool?**| If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest operations first. By default, the results are sorted by the oldest operations first.  | [optional] [default to false]
 **status** | **string**| Status of the operation. | [optional] 
 **withTotalElements** | **bool?**| When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false]
 **withTotalPages** | **bool?**| When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false]

### Return type

[**OperationCollection**](OperationCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/vnd.com.nsn.cumulocity.operationcollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the list of operations is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)


## GetOperationResource

> Operation GetOperationResource (string id)

Retrieve a specific operation

Retrieve a specific operation (by a given ID).  <section><h5>Required roles</h5> ROLE_DEVICE_CONTROL_READ <b>OR</b> owner of the resource <b>OR</b> ADMIN permission on the device </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetOperationResourceExample
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

            var apiInstance = new OperationsApi(Configuration.Default);
            var id = 123;  // string | Unique identifier of the operation.

            try
            {
                // Retrieve a specific operation
                Operation result = apiInstance.GetOperationResource(id);
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling OperationsApi.GetOperationResource: " + e.Message );
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
 **id** | **string**| Unique identifier of the operation. | 

### Return type

[**Operation**](Operation.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/vnd.com.nsn.cumulocity.operation+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the operation is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Operation not found. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)


## PostOperationCollectionResource

> Operation PostOperationCollectionResource (PostOperationCollectionResourceRequest postOperationCollectionResourceRequest, string accept = null, string xCumulocityProcessingMode = null)

Create an operation

Create an operation.  It is possible to add custom fragments to operations, for example `com_cumulocity_model_WebCamDevice` is a custom object of the webcam operation.  <section><h5>Required roles</h5> ROLE_DEVICE_CONTROL_ADMIN <b>OR</b> owner of the device <b>OR</b> ADMIN permissions on the device </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostOperationCollectionResourceExample
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

            var apiInstance = new OperationsApi(Configuration.Default);
            var postOperationCollectionResourceRequest = new PostOperationCollectionResourceRequest(); // PostOperationCollectionResourceRequest | 
            var accept = application/json;  // string | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Create an operation
                Operation result = apiInstance.PostOperationCollectionResource(postOperationCollectionResourceRequest, accept, xCumulocityProcessingMode);
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling OperationsApi.PostOperationCollectionResource: " + e.Message );
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
 **postOperationCollectionResourceRequest** | [**PostOperationCollectionResourceRequest**](PostOperationCollectionResourceRequest.md)|  | 
 **accept** | **string**| Advertises which content types, expressed as MIME types, the client is able to understand. | [optional] 
 **xCumulocityProcessingMode** | **string**| Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT]

### Return type

[**Operation**](Operation.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

- **Content-Type**: application/vnd.com.nsn.cumulocity.operation+json
- **Accept**: application/vnd.com.nsn.cumulocity.operation+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | An operation was created. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)


## PutOperationResource

> Operation PutOperationResource (string id, PutOperationResourceRequest putOperationResourceRequest, string accept = null, string xCumulocityProcessingMode = null)

Update a specific operation status

Update a specific operation (by a given ID). You can only update its status.  <section><h5>Required roles</h5> ROLE_DEVICE_CONTROL_ADMIN <b>OR</b> owner of the resource <b>OR</b> ADMIN permission on the device </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutOperationResourceExample
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

            var apiInstance = new OperationsApi(Configuration.Default);
            var id = 123;  // string | Unique identifier of the operation.
            var putOperationResourceRequest = new PutOperationResourceRequest(); // PutOperationResourceRequest | 
            var accept = application/json;  // string | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Update a specific operation status
                Operation result = apiInstance.PutOperationResource(id, putOperationResourceRequest, accept, xCumulocityProcessingMode);
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling OperationsApi.PutOperationResource: " + e.Message );
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
 **id** | **string**| Unique identifier of the operation. | 
 **putOperationResourceRequest** | [**PutOperationResourceRequest**](PutOperationResourceRequest.md)|  | 
 **accept** | **string**| Advertises which content types, expressed as MIME types, the client is able to understand. | [optional] 
 **xCumulocityProcessingMode** | **string**| Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT]

### Return type

[**Operation**](Operation.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

- **Content-Type**: application/vnd.com.nsn.cumulocity.operation+json
- **Accept**: application/vnd.com.nsn.cumulocity.operation+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | An operation was updated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Operation not found. |  -  |
| **422** | Validation error. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)

