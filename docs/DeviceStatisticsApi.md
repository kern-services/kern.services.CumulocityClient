# kern.services.CumulocityClient.Api.DeviceStatisticsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetTenantDeviceStatisticsDailyCollection**](DeviceStatisticsApi.md#gettenantdevicestatisticsdailycollection) | **GET** /tenant/statistics/device/{tenantId}/daily/{date} | Retrieve daily device statistics |
| [**GetTenantDeviceStatisticsMonthlyCollection**](DeviceStatisticsApi.md#gettenantdevicestatisticsmonthlycollection) | **GET** /tenant/statistics/device/{tenantId}/monthly/{date} | Retrieve monthly device statistics |

<a name="gettenantdevicestatisticsdailycollection"></a>
# **GetTenantDeviceStatisticsDailyCollection**
> DeviceStatisticsCollection GetTenantDeviceStatisticsDailyCollection (string tenantId, DateTime date, int? currentPage = null, string? deviceId = null, int? pageSize = null, bool? withTotalPages = null)

Retrieve daily device statistics

Retrieve daily device statistics from a specific tenant (by a given ID).  <section><h5>Required roles</h5> ROLE_TENANT_STATISTICS_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetTenantDeviceStatisticsDailyCollectionExample
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

            var apiInstance = new DeviceStatisticsApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var date = Fri Jan 01 00:00:00 UTC 2021;  // DateTime | Date (format YYYY-MM-dd) of the queried day.
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var deviceId = 115;  // string? | The ID of the device to search for. (optional) 
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve daily device statistics
                DeviceStatisticsCollection result = apiInstance.GetTenantDeviceStatisticsDailyCollection(tenantId, date, currentPage, deviceId, pageSize, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DeviceStatisticsApi.GetTenantDeviceStatisticsDailyCollection: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetTenantDeviceStatisticsDailyCollectionWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve daily device statistics
    ApiResponse<DeviceStatisticsCollection> response = apiInstance.GetTenantDeviceStatisticsDailyCollectionWithHttpInfo(tenantId, date, currentPage, deviceId, pageSize, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DeviceStatisticsApi.GetTenantDeviceStatisticsDailyCollectionWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **date** | **DateTime** | Date (format YYYY-MM-dd) of the queried day. |  |
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **deviceId** | **string?** | The ID of the device to search for. | [optional]  |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**DeviceStatisticsCollection**](DeviceStatisticsCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the devices statistics are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettenantdevicestatisticsmonthlycollection"></a>
# **GetTenantDeviceStatisticsMonthlyCollection**
> DeviceStatisticsCollection GetTenantDeviceStatisticsMonthlyCollection (string tenantId, DateTime date, int? currentPage = null, string? deviceId = null, int? pageSize = null, bool? withTotalPages = null)

Retrieve monthly device statistics

Retrieve monthly device statistics from a specific tenant (by a given ID).  <section><h5>Required roles</h5> ROLE_TENANT_STATISTICS_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetTenantDeviceStatisticsMonthlyCollectionExample
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

            var apiInstance = new DeviceStatisticsApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var date = Fri Jan 01 00:00:00 UTC 2021;  // DateTime | Date (format YYYY-MM-dd) of the queried month (the day value is ignored).
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var deviceId = 115;  // string? | The ID of the device to search for. (optional) 
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve monthly device statistics
                DeviceStatisticsCollection result = apiInstance.GetTenantDeviceStatisticsMonthlyCollection(tenantId, date, currentPage, deviceId, pageSize, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DeviceStatisticsApi.GetTenantDeviceStatisticsMonthlyCollection: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetTenantDeviceStatisticsMonthlyCollectionWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve monthly device statistics
    ApiResponse<DeviceStatisticsCollection> response = apiInstance.GetTenantDeviceStatisticsMonthlyCollectionWithHttpInfo(tenantId, date, currentPage, deviceId, pageSize, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DeviceStatisticsApi.GetTenantDeviceStatisticsMonthlyCollectionWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **date** | **DateTime** | Date (format YYYY-MM-dd) of the queried month (the day value is ignored). |  |
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **deviceId** | **string?** | The ID of the device to search for. | [optional]  |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**DeviceStatisticsCollection**](DeviceStatisticsCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the devices statistics are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

