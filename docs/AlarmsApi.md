# kern.services.CumulocityClient.Api.AlarmsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**DeleteAlarmCollectionResource**](AlarmsApi.md#deletealarmcollectionresource) | **DELETE** /alarm/alarms | Remove alarm collections |
| [**GetAlarmCollectionCountResource**](AlarmsApi.md#getalarmcollectioncountresource) | **GET** /alarm/alarms/count | Retrieve the total number of alarms |
| [**GetAlarmCollectionResource**](AlarmsApi.md#getalarmcollectionresource) | **GET** /alarm/alarms | Retrieve all alarms |
| [**GetAlarmResource**](AlarmsApi.md#getalarmresource) | **GET** /alarm/alarms/{id} | Retrieve a specific alarm |
| [**PostAlarmCollectionResource**](AlarmsApi.md#postalarmcollectionresource) | **POST** /alarm/alarms | Create an alarm |
| [**PutAlarmCollectionResource**](AlarmsApi.md#putalarmcollectionresource) | **PUT** /alarm/alarms | Update alarm collections |
| [**PutAlarmResource**](AlarmsApi.md#putalarmresource) | **PUT** /alarm/alarms/{id} | Update a specific alarm |

<a name="deletealarmcollectionresource"></a>
# **DeleteAlarmCollectionResource**
> void DeleteAlarmCollectionResource (string? xCumulocityProcessingMode = null, DateTime? createdFrom = null, DateTime? createdTo = null, DateTime? dateFrom = null, DateTime? dateTo = null, bool? resolved = null, List<string>? severity = null, string? source = null, List<string>? status = null, List<string>? type = null, bool? withSourceAssets = null, bool? withSourceDevices = null)

Remove alarm collections

Remove alarm collections specified by query parameters.  > **⚠️ Important:** Note that it is possible to call this endpoint without providing any parameter - it will result in deleting all alarms and it is not recommended. > Also note that DELETE requests are not synchronous. The response could be returned before the delete request has been completed.  <section><h5>Required roles</h5> ROLE_ALARM_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteAlarmCollectionResourceExample
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

            var apiInstance = new AlarmsApi(config);
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)
            var createdFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the alarm creation. (optional) 
            var createdTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the alarm creation. (optional) 
            var dateFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the alarm occurrence. (optional) 
            var dateTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the alarm occurrence. (optional) 
            var resolved = true;  // bool? | When set to `true` only alarms with status CLEARED will be fetched, whereas `false` will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional)  (default to false)
            var severity = new List<string>?(); // List<string>? | The severity of the alarm to search for. >**&#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional) 
            var source = 251994;  // string? | The managed object ID to which the alarm is associated. (optional) 
            var status = new List<string>?(); // List<string>? | The status of the alarm to search for. >**&#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional) 
            var type = new List<string>?(); // List<string>? | The types of alarm to search for. >**&#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional) 
            var withSourceAssets = true;  // bool? | When set to `true` also alarms for related source assets will be included in the request. When this parameter is provided a `source` must be specified. (optional)  (default to false)
            var withSourceDevices = true;  // bool? | When set to `true` also alarms for related source devices will be included in the request. When this parameter is provided a `source` must be specified. (optional)  (default to false)

            try
            {
                // Remove alarm collections
                apiInstance.DeleteAlarmCollectionResource(xCumulocityProcessingMode, createdFrom, createdTo, dateFrom, dateTo, resolved, severity, source, status, type, withSourceAssets, withSourceDevices);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AlarmsApi.DeleteAlarmCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteAlarmCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove alarm collections
    apiInstance.DeleteAlarmCollectionResourceWithHttpInfo(xCumulocityProcessingMode, createdFrom, createdTo, dateFrom, dateTo, resolved, severity, source, status, type, withSourceAssets, withSourceDevices);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AlarmsApi.DeleteAlarmCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |
| **createdFrom** | **DateTime?** | Start date or date and time of the alarm creation. | [optional]  |
| **createdTo** | **DateTime?** | End date or date and time of the alarm creation. | [optional]  |
| **dateFrom** | **DateTime?** | Start date or date and time of the alarm occurrence. | [optional]  |
| **dateTo** | **DateTime?** | End date or date and time of the alarm occurrence. | [optional]  |
| **resolved** | **bool?** | When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. | [optional] [default to false] |
| **severity** | [**List&lt;string&gt;?**](string.md) | The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  | [optional]  |
| **source** | **string?** | The managed object ID to which the alarm is associated. | [optional]  |
| **status** | [**List&lt;string&gt;?**](string.md) | The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  | [optional]  |
| **type** | [**List&lt;string&gt;?**](string.md) | The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  | [optional]  |
| **withSourceAssets** | **bool?** | When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. | [optional] [default to false] |
| **withSourceDevices** | **bool?** | When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. | [optional] [default to false] |

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
| **204** | A collection of alarms was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getalarmcollectioncountresource"></a>
# **GetAlarmCollectionCountResource**
> int GetAlarmCollectionCountResource (DateTime? dateFrom = null, DateTime? dateTo = null, bool? resolved = null, List<string>? severity = null, string? source = null, List<string>? status = null, List<string>? type = null, bool? withSourceAssets = null, bool? withSourceDevices = null)

Retrieve the total number of alarms

Count the total number of active alarms on your tenant.  <section><h5>Required roles</h5> The role ROLE_ALARM_READ is not required, but if a user has this role, all the alarms on the tenant are counted. Otherwise, inventory role permissions are used to count the alarms and the limit is 100. </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetAlarmCollectionCountResourceExample
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

            var apiInstance = new AlarmsApi(config);
            var dateFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the alarm occurrence. (optional) 
            var dateTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the alarm occurrence. (optional) 
            var resolved = true;  // bool? | When set to `true` only alarms with status CLEARED will be fetched, whereas `false` will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional)  (default to false)
            var severity = new List<string>?(); // List<string>? | The severity of the alarm to search for. >**&#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional) 
            var source = 251994;  // string? | The managed object ID to which the alarm is associated. (optional) 
            var status = new List<string>?(); // List<string>? | The status of the alarm to search for. >**&#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional) 
            var type = new List<string>?(); // List<string>? | The types of alarm to search for. >**&#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional) 
            var withSourceAssets = true;  // bool? | When set to `true` also alarms for related source assets will be included in the request. When this parameter is provided a `source` must be specified. (optional)  (default to false)
            var withSourceDevices = true;  // bool? | When set to `true` also alarms for related source devices will be included in the request. When this parameter is provided a `source` must be specified. (optional)  (default to false)

            try
            {
                // Retrieve the total number of alarms
                int result = apiInstance.GetAlarmCollectionCountResource(dateFrom, dateTo, resolved, severity, source, status, type, withSourceAssets, withSourceDevices);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AlarmsApi.GetAlarmCollectionCountResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAlarmCollectionCountResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve the total number of alarms
    ApiResponse<int> response = apiInstance.GetAlarmCollectionCountResourceWithHttpInfo(dateFrom, dateTo, resolved, severity, source, status, type, withSourceAssets, withSourceDevices);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AlarmsApi.GetAlarmCollectionCountResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **dateFrom** | **DateTime?** | Start date or date and time of the alarm occurrence. | [optional]  |
| **dateTo** | **DateTime?** | End date or date and time of the alarm occurrence. | [optional]  |
| **resolved** | **bool?** | When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. | [optional] [default to false] |
| **severity** | [**List&lt;string&gt;?**](string.md) | The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  | [optional]  |
| **source** | **string?** | The managed object ID to which the alarm is associated. | [optional]  |
| **status** | [**List&lt;string&gt;?**](string.md) | The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  | [optional]  |
| **type** | [**List&lt;string&gt;?**](string.md) | The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  | [optional]  |
| **withSourceAssets** | **bool?** | When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. | [optional] [default to false] |
| **withSourceDevices** | **bool?** | When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. | [optional] [default to false] |

### Return type

**int**

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the number of active alarms is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getalarmcollectionresource"></a>
# **GetAlarmCollectionResource**
> AlarmCollection GetAlarmCollectionResource (DateTime? createdFrom = null, DateTime? createdTo = null, int? currentPage = null, DateTime? dateFrom = null, DateTime? dateTo = null, DateTime? lastUpdatedFrom = null, DateTime? lastUpdatedTo = null, int? pageSize = null, bool? resolved = null, List<string>? severity = null, string? source = null, List<string>? status = null, List<string>? type = null, bool? withSourceAssets = null, bool? withSourceDevices = null, bool? withTotalElements = null, bool? withTotalPages = null)

Retrieve all alarms

Retrieve all alarms on your tenant, or a specific subset based on queries. The results are sorted by the newest alarms first.  #### Query parameters  The query parameter `withTotalPages` only works when the user has the ROLE_ALARM_READ role, otherwise it is ignored.  <section><h5>Required roles</h5> The role ROLE_ALARM_READ is not required, but if a user has this role, all the alarms on the tenant are returned. If a user has access to alarms through inventory roles, only those alarms are returned. </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetAlarmCollectionResourceExample
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

            var apiInstance = new AlarmsApi(config);
            var createdFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the alarm creation. (optional) 
            var createdTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the alarm creation. (optional) 
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var dateFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the alarm occurrence. (optional) 
            var dateTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the alarm occurrence. (optional) 
            var lastUpdatedFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the last update made. (optional) 
            var lastUpdatedTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the last update made. (optional) 
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var resolved = true;  // bool? | When set to `true` only alarms with status CLEARED will be fetched, whereas `false` will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional)  (default to false)
            var severity = new List<string>?(); // List<string>? | The severity of the alarm to search for. >**&#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional) 
            var source = 251994;  // string? | The managed object ID to which the alarm is associated. (optional) 
            var status = new List<string>?(); // List<string>? | The status of the alarm to search for. >**&#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional) 
            var type = new List<string>?(); // List<string>? | The types of alarm to search for. >**&#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional) 
            var withSourceAssets = true;  // bool? | When set to `true` also alarms for related source assets will be included in the request. When this parameter is provided a `source` must be specified. (optional)  (default to false)
            var withSourceDevices = true;  // bool? | When set to `true` also alarms for related source devices will be included in the request. When this parameter is provided a `source` must be specified. (optional)  (default to false)
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve all alarms
                AlarmCollection result = apiInstance.GetAlarmCollectionResource(createdFrom, createdTo, currentPage, dateFrom, dateTo, lastUpdatedFrom, lastUpdatedTo, pageSize, resolved, severity, source, status, type, withSourceAssets, withSourceDevices, withTotalElements, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AlarmsApi.GetAlarmCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAlarmCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all alarms
    ApiResponse<AlarmCollection> response = apiInstance.GetAlarmCollectionResourceWithHttpInfo(createdFrom, createdTo, currentPage, dateFrom, dateTo, lastUpdatedFrom, lastUpdatedTo, pageSize, resolved, severity, source, status, type, withSourceAssets, withSourceDevices, withTotalElements, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AlarmsApi.GetAlarmCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **createdFrom** | **DateTime?** | Start date or date and time of the alarm creation. | [optional]  |
| **createdTo** | **DateTime?** | End date or date and time of the alarm creation. | [optional]  |
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **dateFrom** | **DateTime?** | Start date or date and time of the alarm occurrence. | [optional]  |
| **dateTo** | **DateTime?** | End date or date and time of the alarm occurrence. | [optional]  |
| **lastUpdatedFrom** | **DateTime?** | Start date or date and time of the last update made. | [optional]  |
| **lastUpdatedTo** | **DateTime?** | End date or date and time of the last update made. | [optional]  |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **resolved** | **bool?** | When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. | [optional] [default to false] |
| **severity** | [**List&lt;string&gt;?**](string.md) | The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  | [optional]  |
| **source** | **string?** | The managed object ID to which the alarm is associated. | [optional]  |
| **status** | [**List&lt;string&gt;?**](string.md) | The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  | [optional]  |
| **type** | [**List&lt;string&gt;?**](string.md) | The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  | [optional]  |
| **withSourceAssets** | **bool?** | When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. | [optional] [default to false] |
| **withSourceDevices** | **bool?** | When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. | [optional] [default to false] |
| **withTotalElements** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**AlarmCollection**](AlarmCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.alarmcollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and all alarms are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getalarmresource"></a>
# **GetAlarmResource**
> Alarm GetAlarmResource (string id)

Retrieve a specific alarm

Retrieve a specific alarm by a given ID.  <section><h5>Required roles</h5> ROLE_ALARM_READ <b>OR</b> owner of the source <b>OR</b> ALARM_READ permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetAlarmResourceExample
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

            var apiInstance = new AlarmsApi(config);
            var id = 20200301;  // string | Unique identifier of the alarm.

            try
            {
                // Retrieve a specific alarm
                Alarm result = apiInstance.GetAlarmResource(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AlarmsApi.GetAlarmResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAlarmResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a specific alarm
    ApiResponse<Alarm> response = apiInstance.GetAlarmResourceWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AlarmsApi.GetAlarmResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the alarm. |  |

### Return type

[**Alarm**](Alarm.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.alarm+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the alarm is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **404** | Alarm not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postalarmcollectionresource"></a>
# **PostAlarmCollectionResource**
> Alarm PostAlarmCollectionResource (PostAlarmCollectionResourceRequest postAlarmCollectionResourceRequest, string? accept = null, string? xCumulocityProcessingMode = null)

Create an alarm

An alarm must be associated with a source (managed object) identified by ID.<br> In general, each alarm may consist of:  *   A status showing whether the alarm is ACTIVE, ACKNOWLEDGED or CLEARED. *   A time stamp to indicate when the alarm was last updated. *   The severity of the alarm: CRITICAL, MAJOR, MINOR or WARNING. *   A history of changes to the event in form of audit logs.  ### Alarm suppression  If the source device is in maintenance mode, the alarm is not created and not reported to the Cumulocity IoT event processing engine. When sending a POST request to create a new alarm and if the source device is in maintenance mode, the self link of the alarm will be:  ```json \"self\": \"https://<TENANT_DOMAIN>/alarm/alarms/null\" ```  ### Alarm de-duplication  If an ACTIVE or ACKNOWLEDGED alarm with the same source and type exists, no new alarm is created. Instead, the existing alarm is updated by incrementing the `count` property; the `time` property is also updated. Any other changes are ignored, and the alarm history is not updated. Alarms with status CLEARED are not de-duplicated. The first occurrence of the alarm is recorded in the `firstOccurrenceTime` property.  <section><h5>Required roles</h5> ROLE_ALARM_ADMIN <b>OR</b> owner of the source <b>OR</b> ALARM_ADMIN permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostAlarmCollectionResourceExample
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

            var apiInstance = new AlarmsApi(config);
            var postAlarmCollectionResourceRequest = new PostAlarmCollectionResourceRequest(); // PostAlarmCollectionResourceRequest | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Create an alarm
                Alarm result = apiInstance.PostAlarmCollectionResource(postAlarmCollectionResourceRequest, accept, xCumulocityProcessingMode);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AlarmsApi.PostAlarmCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostAlarmCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create an alarm
    ApiResponse<Alarm> response = apiInstance.PostAlarmCollectionResourceWithHttpInfo(postAlarmCollectionResourceRequest, accept, xCumulocityProcessingMode);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AlarmsApi.PostAlarmCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **postAlarmCollectionResourceRequest** | [**PostAlarmCollectionResourceRequest**](PostAlarmCollectionResourceRequest.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

[**Alarm**](Alarm.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.alarm+json
 - **Accept**: application/vnd.com.nsn.cumulocity.alarm+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | An alarm was created. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="putalarmcollectionresource"></a>
# **PutAlarmCollectionResource**
> void PutAlarmCollectionResource (PutAlarmCollectionResourceRequest putAlarmCollectionResourceRequest, string? accept = null, string? xCumulocityProcessingMode = null, DateTime? createdFrom = null, DateTime? createdTo = null, DateTime? dateFrom = null, DateTime? dateTo = null, bool? resolved = null, List<string>? severity = null, string? source = null, List<string>? status = null, bool? withSourceAssets = null, bool? withSourceDevices = null)

Update alarm collections

Update alarm collections specified by query parameters. At least one query parameter is required to avoid accidentally updating all existing alarms.<br> Currently, only the status of alarms can be modified.  > **&#9432; Info:** Since this operation can take considerable time, the request returns after maximum 0.5 seconds of processing, and the update operation continues as a background process in the platform.  <section><h5>Required roles</h5> ROLE_ALARM_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutAlarmCollectionResourceExample
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

            var apiInstance = new AlarmsApi(config);
            var putAlarmCollectionResourceRequest = new PutAlarmCollectionResourceRequest(); // PutAlarmCollectionResourceRequest | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)
            var createdFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the alarm creation. (optional) 
            var createdTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the alarm creation. (optional) 
            var dateFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the alarm occurrence. (optional) 
            var dateTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the alarm occurrence. (optional) 
            var resolved = true;  // bool? | When set to `true` only alarms with status CLEARED will be fetched, whereas `false` will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional)  (default to false)
            var severity = new List<string>?(); // List<string>? | The severity of the alarm to search for. >**&#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional) 
            var source = 251994;  // string? | The managed object ID to which the alarm is associated. (optional) 
            var status = new List<string>?(); // List<string>? | The status of the alarm to search for. >**&#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional) 
            var withSourceAssets = true;  // bool? | When set to `true` also alarms for related source assets will be included in the request. When this parameter is provided a `source` must be specified. (optional)  (default to false)
            var withSourceDevices = true;  // bool? | When set to `true` also alarms for related source devices will be included in the request. When this parameter is provided a `source` must be specified. (optional)  (default to false)

            try
            {
                // Update alarm collections
                apiInstance.PutAlarmCollectionResource(putAlarmCollectionResourceRequest, accept, xCumulocityProcessingMode, createdFrom, createdTo, dateFrom, dateTo, resolved, severity, source, status, withSourceAssets, withSourceDevices);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AlarmsApi.PutAlarmCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutAlarmCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update alarm collections
    apiInstance.PutAlarmCollectionResourceWithHttpInfo(putAlarmCollectionResourceRequest, accept, xCumulocityProcessingMode, createdFrom, createdTo, dateFrom, dateTo, resolved, severity, source, status, withSourceAssets, withSourceDevices);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AlarmsApi.PutAlarmCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **putAlarmCollectionResourceRequest** | [**PutAlarmCollectionResourceRequest**](PutAlarmCollectionResourceRequest.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |
| **createdFrom** | **DateTime?** | Start date or date and time of the alarm creation. | [optional]  |
| **createdTo** | **DateTime?** | End date or date and time of the alarm creation. | [optional]  |
| **dateFrom** | **DateTime?** | Start date or date and time of the alarm occurrence. | [optional]  |
| **dateTo** | **DateTime?** | End date or date and time of the alarm occurrence. | [optional]  |
| **resolved** | **bool?** | When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. | [optional] [default to false] |
| **severity** | [**List&lt;string&gt;?**](string.md) | The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  | [optional]  |
| **source** | **string?** | The managed object ID to which the alarm is associated. | [optional]  |
| **status** | [**List&lt;string&gt;?**](string.md) | The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  | [optional]  |
| **withSourceAssets** | **bool?** | When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. | [optional] [default to false] |
| **withSourceDevices** | **bool?** | When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. | [optional] [default to false] |

### Return type

void (empty response body)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.alarm+json
 - **Accept**: application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | An alarm collection was updated. |  -  |
| **202** | An alarm collection is being updated in background. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="putalarmresource"></a>
# **PutAlarmResource**
> Alarm PutAlarmResource (string id, PutAlarmResourceRequest putAlarmResourceRequest, string? accept = null, string? xCumulocityProcessingMode = null)

Update a specific alarm

Update a specific alarm by a given ID. Only text, status, severity and custom properties can be modified. A request will be rejected when non-modifiable properties are provided in the request body.  > **&#9432; Info:** Changes to alarms will generate a new audit record. The audit record will include the username and application that triggered the update, if applicable. If the update operation doesn’t change anything (that is, the request body contains data that is identical to the already present in the database), there will be no audit record added and no notifications will be sent.  <section><h5>Required roles</h5> ROLE_ALARM_ADMIN <b>OR</b> owner of the source <b>OR</b> ALARM_ADMIN permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutAlarmResourceExample
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

            var apiInstance = new AlarmsApi(config);
            var id = 20200301;  // string | Unique identifier of the alarm.
            var putAlarmResourceRequest = new PutAlarmResourceRequest(); // PutAlarmResourceRequest | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Update a specific alarm
                Alarm result = apiInstance.PutAlarmResource(id, putAlarmResourceRequest, accept, xCumulocityProcessingMode);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AlarmsApi.PutAlarmResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutAlarmResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update a specific alarm
    ApiResponse<Alarm> response = apiInstance.PutAlarmResourceWithHttpInfo(id, putAlarmResourceRequest, accept, xCumulocityProcessingMode);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AlarmsApi.PutAlarmResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the alarm. |  |
| **putAlarmResourceRequest** | [**PutAlarmResourceRequest**](PutAlarmResourceRequest.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

[**Alarm**](Alarm.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.alarm+json
 - **Accept**: application/vnd.com.nsn.cumulocity.alarm+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | An alarm was updated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **404** | Alarm not found. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

