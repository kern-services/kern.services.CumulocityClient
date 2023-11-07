# kern.services.CumulocityClient.Api.EventsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**DeleteEventCollectionResource**](EventsApi.md#deleteeventcollectionresource) | **DELETE** /event/events | Remove event collections |
| [**DeleteEventResource**](EventsApi.md#deleteeventresource) | **DELETE** /event/events/{id} | Remove a specific event |
| [**GetEventCollectionResource**](EventsApi.md#geteventcollectionresource) | **GET** /event/events | Retrieve all events |
| [**GetEventResource**](EventsApi.md#geteventresource) | **GET** /event/events/{id} | Retrieve a specific event |
| [**PostEventCollectionResource**](EventsApi.md#posteventcollectionresource) | **POST** /event/events | Create an event |
| [**PutEventResource**](EventsApi.md#puteventresource) | **PUT** /event/events/{id} | Update a specific event |

<a name="deleteeventcollectionresource"></a>
# **DeleteEventCollectionResource**
> void DeleteEventCollectionResource (string? xCumulocityProcessingMode = null, DateTime? createdFrom = null, DateTime? createdTo = null, DateTime? dateFrom = null, DateTime? dateTo = null, string? fragmentType = null, string? source = null, string? type = null)

Remove event collections

Remove event collections specified by query parameters.  DELETE requests are not synchronous. The response could be returned before the delete request has been completed. This may happen especially when the deleted event has a lot of associated data. After sending the request, the platform starts deleting the associated data in an asynchronous way. Finally, the requested event is deleted after all associated data has been deleted.  > **⚠️ Important:** Note that it is possible to call this endpoint without providing any parameter - it will result in deleting all events and it is not recommended.  <section><h5>Required roles</h5> ROLE_EVENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteEventCollectionResourceExample
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

            var apiInstance = new EventsApi(config);
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)
            var createdFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the event's creation (set by the platform during creation). (optional) 
            var createdTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the event's creation (set by the platform during creation). (optional) 
            var dateFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the event occurrence (provided by the device). (optional) 
            var dateTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the event occurrence (provided by the device). (optional) 
            var fragmentType = c8y_IsDevice;  // string? | A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional) 
            var source = 251994;  // string? | The managed object ID to which the event is associated. (optional) 
            var type = c8y_OutgoingSmsLog;  // string? | The type of event to search for. (optional) 

            try
            {
                // Remove event collections
                apiInstance.DeleteEventCollectionResource(xCumulocityProcessingMode, createdFrom, createdTo, dateFrom, dateTo, fragmentType, source, type);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EventsApi.DeleteEventCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteEventCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove event collections
    apiInstance.DeleteEventCollectionResourceWithHttpInfo(xCumulocityProcessingMode, createdFrom, createdTo, dateFrom, dateTo, fragmentType, source, type);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EventsApi.DeleteEventCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |
| **createdFrom** | **DateTime?** | Start date or date and time of the event&#39;s creation (set by the platform during creation). | [optional]  |
| **createdTo** | **DateTime?** | End date or date and time of the event&#39;s creation (set by the platform during creation). | [optional]  |
| **dateFrom** | **DateTime?** | Start date or date and time of the event occurrence (provided by the device). | [optional]  |
| **dateTo** | **DateTime?** | End date or date and time of the event occurrence (provided by the device). | [optional]  |
| **fragmentType** | **string?** | A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. | [optional]  |
| **source** | **string?** | The managed object ID to which the event is associated. | [optional]  |
| **type** | **string?** | The type of event to search for. | [optional]  |

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
| **204** | A collection of events was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deleteeventresource"></a>
# **DeleteEventResource**
> void DeleteEventResource (string id, string? xCumulocityProcessingMode = null)

Remove a specific event

Remove a specific event by a given ID.  <section><h5>Required roles</h5> ROLE_EVENT_ADMIN <b>OR</b> owner of the source <b>OR</b> EVENT_ADMIN permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteEventResourceExample
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

            var apiInstance = new EventsApi(config);
            var id = 20200301;  // string | Unique identifier of the event.
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Remove a specific event
                apiInstance.DeleteEventResource(id, xCumulocityProcessingMode);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EventsApi.DeleteEventResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteEventResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove a specific event
    apiInstance.DeleteEventResourceWithHttpInfo(id, xCumulocityProcessingMode);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EventsApi.DeleteEventResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the event. |  |
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
| **204** | An event was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **404** | Event not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="geteventcollectionresource"></a>
# **GetEventCollectionResource**
> EventCollection GetEventCollectionResource (DateTime? createdFrom = null, DateTime? createdTo = null, int? currentPage = null, DateTime? dateFrom = null, DateTime? dateTo = null, string? fragmentType = null, string? fragmentValue = null, DateTime? lastUpdatedFrom = null, DateTime? lastUpdatedTo = null, int? pageSize = null, bool? revert = null, string? source = null, string? type = null, bool? withSourceAssets = null, bool? withSourceDevices = null, bool? withTotalElements = null, bool? withTotalPages = null)

Retrieve all events

Retrieve all events on your tenant.  In case of executing [range queries](https://en.wikipedia.org/wiki/Range_query_(database)) between an upper and lower boundary, for example, querying using `dateFrom`–`dateTo` or `createdFrom`–`createdTo`, the newest registered events are returned first. It is possible to change the order using the query parameter `revert=true`.  <section><h5>Required roles</h5> ROLE_EVENT_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetEventCollectionResourceExample
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

            var apiInstance = new EventsApi(config);
            var createdFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the event's creation (set by the platform during creation). (optional) 
            var createdTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the event's creation (set by the platform during creation). (optional) 
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var dateFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the event occurrence (provided by the device). (optional) 
            var dateTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the event occurrence (provided by the device). (optional) 
            var fragmentType = c8y_IsDevice;  // string? | A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional) 
            var fragmentValue = lorem;  // string? | Allows filtering events by the fragment's value, but only when provided together with `fragmentType`.  > **⚠️ Important:** Only fragments with a string value are supported.  (optional) 
            var lastUpdatedFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the last update made. (optional) 
            var lastUpdatedTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the last update made. (optional) 
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var revert = true;  // bool? | If you are using a range query (that is, at least one of the `dateFrom` or `dateTo` parameters is included in the request), then setting `revert=true` will sort the results by the oldest events first. By default, the results are sorted by the newest events first.  (optional)  (default to false)
            var source = 251994;  // string? | The managed object ID to which the event is associated. (optional) 
            var type = c8y_OutgoingSmsLog;  // string? | The type of event to search for. (optional) 
            var withSourceAssets = true;  // bool? | When set to `true` also events for related source assets will be included in the request. When this parameter is provided a `source` must be specified. (optional)  (default to false)
            var withSourceDevices = true;  // bool? | When set to `true` also events for related source devices will be included in the request. When this parameter is provided a `source` must be specified. (optional)  (default to false)
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve all events
                EventCollection result = apiInstance.GetEventCollectionResource(createdFrom, createdTo, currentPage, dateFrom, dateTo, fragmentType, fragmentValue, lastUpdatedFrom, lastUpdatedTo, pageSize, revert, source, type, withSourceAssets, withSourceDevices, withTotalElements, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EventsApi.GetEventCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetEventCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all events
    ApiResponse<EventCollection> response = apiInstance.GetEventCollectionResourceWithHttpInfo(createdFrom, createdTo, currentPage, dateFrom, dateTo, fragmentType, fragmentValue, lastUpdatedFrom, lastUpdatedTo, pageSize, revert, source, type, withSourceAssets, withSourceDevices, withTotalElements, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EventsApi.GetEventCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **createdFrom** | **DateTime?** | Start date or date and time of the event&#39;s creation (set by the platform during creation). | [optional]  |
| **createdTo** | **DateTime?** | End date or date and time of the event&#39;s creation (set by the platform during creation). | [optional]  |
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **dateFrom** | **DateTime?** | Start date or date and time of the event occurrence (provided by the device). | [optional]  |
| **dateTo** | **DateTime?** | End date or date and time of the event occurrence (provided by the device). | [optional]  |
| **fragmentType** | **string?** | A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. | [optional]  |
| **fragmentValue** | **string?** | Allows filtering events by the fragment&#39;s value, but only when provided together with &#x60;fragmentType&#x60;.  &gt; **⚠️ Important:** Only fragments with a string value are supported.  | [optional]  |
| **lastUpdatedFrom** | **DateTime?** | Start date or date and time of the last update made. | [optional]  |
| **lastUpdatedTo** | **DateTime?** | End date or date and time of the last update made. | [optional]  |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **revert** | **bool?** | If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the oldest events first. By default, the results are sorted by the newest events first.  | [optional] [default to false] |
| **source** | **string?** | The managed object ID to which the event is associated. | [optional]  |
| **type** | **string?** | The type of event to search for. | [optional]  |
| **withSourceAssets** | **bool?** | When set to &#x60;true&#x60; also events for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. | [optional] [default to false] |
| **withSourceDevices** | **bool?** | When set to &#x60;true&#x60; also events for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. | [optional] [default to false] |
| **withTotalElements** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**EventCollection**](EventCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.eventcollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and all events are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="geteventresource"></a>
# **GetEventResource**
> Event GetEventResource (string id)

Retrieve a specific event

Retrieve a specific event by a given ID.  <section><h5>Required roles</h5> ROLE_EVENT_READ <b>OR</b> owner of the source <b>OR</b> EVENT_READ permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetEventResourceExample
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

            var apiInstance = new EventsApi(config);
            var id = 20200301;  // string | Unique identifier of the event.

            try
            {
                // Retrieve a specific event
                Event result = apiInstance.GetEventResource(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EventsApi.GetEventResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetEventResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a specific event
    ApiResponse<Event> response = apiInstance.GetEventResourceWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EventsApi.GetEventResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the event. |  |

### Return type

[**Event**](Event.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.event+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the event is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Event not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="posteventcollectionresource"></a>
# **PostEventCollectionResource**
> Event PostEventCollectionResource (PostEventCollectionResourceRequest postEventCollectionResourceRequest, string? accept = null, string? xCumulocityProcessingMode = null)

Create an event

An event must be associated with a source (managed object) identified by an ID.<br> In general, each event consists of:  *  A type to identify the nature of the event. *  A time stamp to indicate when the event was last updated. *  A description of the event. *  The managed object which originated the event.  <section><h5>Required roles</h5> ROLE_EVENT_ADMIN <b>OR</b> owner of the source <b>OR</b> EVENT_ADMIN permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostEventCollectionResourceExample
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

            var apiInstance = new EventsApi(config);
            var postEventCollectionResourceRequest = new PostEventCollectionResourceRequest(); // PostEventCollectionResourceRequest | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Create an event
                Event result = apiInstance.PostEventCollectionResource(postEventCollectionResourceRequest, accept, xCumulocityProcessingMode);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EventsApi.PostEventCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostEventCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create an event
    ApiResponse<Event> response = apiInstance.PostEventCollectionResourceWithHttpInfo(postEventCollectionResourceRequest, accept, xCumulocityProcessingMode);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EventsApi.PostEventCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **postEventCollectionResourceRequest** | [**PostEventCollectionResourceRequest**](PostEventCollectionResourceRequest.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

[**Event**](Event.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.event+json
 - **Accept**: application/vnd.com.nsn.cumulocity.event+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | An event was created. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="puteventresource"></a>
# **PutEventResource**
> Event PutEventResource (string id, PutEventResourceRequest putEventResourceRequest, string? accept = null, string? xCumulocityProcessingMode = null)

Update a specific event

Update a specific event by a given ID. Only its text description and custom fragments can be updated.  <section><h5>Required roles</h5> ROLE_EVENT_ADMIN <b>OR</b> owner of the source <b>OR</b> EVENT_ADMIN permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutEventResourceExample
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

            var apiInstance = new EventsApi(config);
            var id = 20200301;  // string | Unique identifier of the event.
            var putEventResourceRequest = new PutEventResourceRequest(); // PutEventResourceRequest | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Update a specific event
                Event result = apiInstance.PutEventResource(id, putEventResourceRequest, accept, xCumulocityProcessingMode);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EventsApi.PutEventResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutEventResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update a specific event
    ApiResponse<Event> response = apiInstance.PutEventResourceWithHttpInfo(id, putEventResourceRequest, accept, xCumulocityProcessingMode);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EventsApi.PutEventResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the event. |  |
| **putEventResourceRequest** | [**PutEventResourceRequest**](PutEventResourceRequest.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

[**Event**](Event.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.event+json
 - **Accept**: application/vnd.com.nsn.cumulocity.event+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | An event was updated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Event not found. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

