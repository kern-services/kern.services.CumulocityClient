# kern.services.CumulocityClient.Api.UsageStatisticsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetSummaryAllTenantsUsageStatistics**](UsageStatisticsApi.md#getsummaryalltenantsusagestatistics) | **GET** /tenant/statistics/allTenantsSummary | Retrieve a summary of all usage statistics |
| [**GetSummaryUsageStatistics**](UsageStatisticsApi.md#getsummaryusagestatistics) | **GET** /tenant/statistics/summary | Retrieve a usage statistics summary |
| [**GetTenantUsageStatisticsCollectionResource**](UsageStatisticsApi.md#gettenantusagestatisticscollectionresource) | **GET** /tenant/statistics | Retrieve statistics of the current tenant |
| [**GetTenantUsageStatisticsFileById**](UsageStatisticsApi.md#gettenantusagestatisticsfilebyid) | **GET** /tenant/statistics/files/{id} | Retrieve a usage statistics file |
| [**GetTenantUsageStatisticsFileCollectionResource**](UsageStatisticsApi.md#gettenantusagestatisticsfilecollectionresource) | **GET** /tenant/statistics/files | Retrieve usage statistics files metadata |
| [**GetTenantUsageStatisticsLatestFile**](UsageStatisticsApi.md#gettenantusagestatisticslatestfile) | **GET** /tenant/statistics/files/latest/{month} | Retrieve the latest usage statistics file |
| [**PostGenerateStatisticsFileRequest**](UsageStatisticsApi.md#postgeneratestatisticsfilerequest) | **POST** /tenant/statistics/files | Generate a statistics file report |

<a name="getsummaryalltenantsusagestatistics"></a>
# **GetSummaryAllTenantsUsageStatistics**
> List&lt;SummaryAllTenantsUsageStatistics&gt; GetSummaryAllTenantsUsageStatistics (DateTime? dateFrom = null, DateTime? dateTo = null)

Retrieve a summary of all usage statistics

Retrieve a summary of all tenants usage statistics.  <section><h5>Required roles</h5> ROLE_TENANT_MANAGEMENT_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetSummaryAllTenantsUsageStatisticsExample
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

            var apiInstance = new UsageStatisticsApi(config);
            var dateFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the statistics. (optional) 
            var dateTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the statistics. (optional) 

            try
            {
                // Retrieve a summary of all usage statistics
                List<SummaryAllTenantsUsageStatistics> result = apiInstance.GetSummaryAllTenantsUsageStatistics(dateFrom, dateTo);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UsageStatisticsApi.GetSummaryAllTenantsUsageStatistics: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetSummaryAllTenantsUsageStatisticsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a summary of all usage statistics
    ApiResponse<List<SummaryAllTenantsUsageStatistics>> response = apiInstance.GetSummaryAllTenantsUsageStatisticsWithHttpInfo(dateFrom, dateTo);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UsageStatisticsApi.GetSummaryAllTenantsUsageStatisticsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **dateFrom** | **DateTime?** | Start date or date and time of the statistics. | [optional]  |
| **dateTo** | **DateTime?** | End date or date and time of the statistics. | [optional]  |

### Return type

[**List&lt;SummaryAllTenantsUsageStatistics&gt;**](SummaryAllTenantsUsageStatistics.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the usage statistics summary is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsummaryusagestatistics"></a>
# **GetSummaryUsageStatistics**
> SummaryTenantUsageStatistics GetSummaryUsageStatistics (DateTime? dateFrom = null, DateTime? dateTo = null, string? tenant = null)

Retrieve a usage statistics summary

Retrieve a usage statistics summary of a tenant. <section><h5>Required roles</h5> ROLE_TENANT_STATISTICS_READ <b>OR</b> ROLE_INVENTORY_READ <br/> If the `tenant` request parameter is specified, then the current tenant must be the management tenant <b>OR</b> the parent of the requested `tenant`. </section>

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetSummaryUsageStatisticsExample
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

            var apiInstance = new UsageStatisticsApi(config);
            var dateFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the statistics. (optional) 
            var dateTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the statistics. (optional) 
            var tenant = t07007007;  // string? | Unique identifier of a Cumulocity IoT tenant. (optional) 

            try
            {
                // Retrieve a usage statistics summary
                SummaryTenantUsageStatistics result = apiInstance.GetSummaryUsageStatistics(dateFrom, dateTo, tenant);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UsageStatisticsApi.GetSummaryUsageStatistics: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetSummaryUsageStatisticsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a usage statistics summary
    ApiResponse<SummaryTenantUsageStatistics> response = apiInstance.GetSummaryUsageStatisticsWithHttpInfo(dateFrom, dateTo, tenant);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UsageStatisticsApi.GetSummaryUsageStatisticsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **dateFrom** | **DateTime?** | Start date or date and time of the statistics. | [optional]  |
| **dateTo** | **DateTime?** | End date or date and time of the statistics. | [optional]  |
| **tenant** | **string?** | Unique identifier of a Cumulocity IoT tenant. | [optional]  |

### Return type

[**SummaryTenantUsageStatistics**](SummaryTenantUsageStatistics.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.tenantusagestatisticssummary+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the usage statistics summary is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **404** | Tenant not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettenantusagestatisticscollectionresource"></a>
# **GetTenantUsageStatisticsCollectionResource**
> TenantUsageStatisticsCollection GetTenantUsageStatisticsCollectionResource (int? currentPage = null, DateTime? dateFrom = null, DateTime? dateTo = null, int? pageSize = null, bool? withTotalElements = null, bool? withTotalPages = null)

Retrieve statistics of the current tenant

Retrieve usage statistics of the current tenant.  <section><h5>Required roles</h5> ROLE_TENANT_STATISTICS_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetTenantUsageStatisticsCollectionResourceExample
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

            var apiInstance = new UsageStatisticsApi(config);
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var dateFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the statistics. (optional) 
            var dateTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the statistics. (optional) 
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve statistics of the current tenant
                TenantUsageStatisticsCollection result = apiInstance.GetTenantUsageStatisticsCollectionResource(currentPage, dateFrom, dateTo, pageSize, withTotalElements, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UsageStatisticsApi.GetTenantUsageStatisticsCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetTenantUsageStatisticsCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve statistics of the current tenant
    ApiResponse<TenantUsageStatisticsCollection> response = apiInstance.GetTenantUsageStatisticsCollectionResourceWithHttpInfo(currentPage, dateFrom, dateTo, pageSize, withTotalElements, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UsageStatisticsApi.GetTenantUsageStatisticsCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **dateFrom** | **DateTime?** | Start date or date and time of the statistics. | [optional]  |
| **dateTo** | **DateTime?** | End date or date and time of the statistics. | [optional]  |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **withTotalElements** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**TenantUsageStatisticsCollection**](TenantUsageStatisticsCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.tenantusagestatisticscollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the tenant statistics are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettenantusagestatisticsfilebyid"></a>
# **GetTenantUsageStatisticsFileById**
> System.IO.Stream GetTenantUsageStatisticsFileById (string id)

Retrieve a usage statistics file

Retrieve a specific usage statistics file (by a given ID).  <section><h5>Required roles</h5> ROLE_TENANT_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetTenantUsageStatisticsFileByIdExample
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

            var apiInstance = new UsageStatisticsApi(config);
            var id = 30303033;  // string | Unique identifier of the statistics file.

            try
            {
                // Retrieve a usage statistics file
                System.IO.Stream result = apiInstance.GetTenantUsageStatisticsFileById(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UsageStatisticsApi.GetTenantUsageStatisticsFileById: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetTenantUsageStatisticsFileByIdWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a usage statistics file
    ApiResponse<System.IO.Stream> response = apiInstance.GetTenantUsageStatisticsFileByIdWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UsageStatisticsApi.GetTenantUsageStatisticsFileByIdWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the statistics file. |  |

### Return type

**System.IO.Stream**

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/octet-stream, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the file is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Statistics file not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettenantusagestatisticsfilecollectionresource"></a>
# **GetTenantUsageStatisticsFileCollectionResource**
> TenantUsageStatisticsFileCollection GetTenantUsageStatisticsFileCollectionResource (int? currentPage = null, DateTime? dateFrom = null, DateTime? dateTo = null, int? pageSize = null, bool? withTotalPages = null)

Retrieve usage statistics files metadata

Retrieve usage statistics summary files report metadata.  <section><h5>Required roles</h5> ROLE_TENANT_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetTenantUsageStatisticsFileCollectionResourceExample
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

            var apiInstance = new UsageStatisticsApi(config);
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var dateFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the statistics file generation. (optional) 
            var dateTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the statistics file generation. (optional) 
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve usage statistics files metadata
                TenantUsageStatisticsFileCollection result = apiInstance.GetTenantUsageStatisticsFileCollectionResource(currentPage, dateFrom, dateTo, pageSize, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UsageStatisticsApi.GetTenantUsageStatisticsFileCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetTenantUsageStatisticsFileCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve usage statistics files metadata
    ApiResponse<TenantUsageStatisticsFileCollection> response = apiInstance.GetTenantUsageStatisticsFileCollectionResourceWithHttpInfo(currentPage, dateFrom, dateTo, pageSize, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UsageStatisticsApi.GetTenantUsageStatisticsFileCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **dateFrom** | **DateTime?** | Start date or date and time of the statistics file generation. | [optional]  |
| **dateTo** | **DateTime?** | End date or date and time of the statistics file generation. | [optional]  |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**TenantUsageStatisticsFileCollection**](TenantUsageStatisticsFileCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.tenantStatisticsfilecollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the tenant statistics are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettenantusagestatisticslatestfile"></a>
# **GetTenantUsageStatisticsLatestFile**
> System.IO.Stream GetTenantUsageStatisticsLatestFile (DateTime month)

Retrieve the latest usage statistics file

Retrieve the latest usage statistics file with REAL data for a given month.  There are two types of statistics files: * REAL - generated by the system on the first day of the month and includes statistics for the previous month. * TEST - generated by the user with a time range specified in the query parameters (`dateFrom`, `dateTo`).  <section><h5>Required roles</h5> ROLE_TENANT_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetTenantUsageStatisticsLatestFileExample
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

            var apiInstance = new UsageStatisticsApi(config);
            var month = Sun Mar 01 00:00:00 UTC 2020;  // DateTime | Date (format YYYY-MM-dd) specifying the month for which the statistics file will be downloaded (the day value is ignored).

            try
            {
                // Retrieve the latest usage statistics file
                System.IO.Stream result = apiInstance.GetTenantUsageStatisticsLatestFile(month);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UsageStatisticsApi.GetTenantUsageStatisticsLatestFile: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetTenantUsageStatisticsLatestFileWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve the latest usage statistics file
    ApiResponse<System.IO.Stream> response = apiInstance.GetTenantUsageStatisticsLatestFileWithHttpInfo(month);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UsageStatisticsApi.GetTenantUsageStatisticsLatestFileWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **month** | **DateTime** | Date (format YYYY-MM-dd) specifying the month for which the statistics file will be downloaded (the day value is ignored). |  |

### Return type

**System.IO.Stream**

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/octet-stream, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the file is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postgeneratestatisticsfilerequest"></a>
# **PostGenerateStatisticsFileRequest**
> StatisticsFile PostGenerateStatisticsFileRequest (RangeStatisticsFile rangeStatisticsFile, string? accept = null)

Generate a statistics file report

Generate a TEST statistics file report for a given time range.  There are two types of statistics files: * REAL - generated by the system on the first day of the month and including statistics from the previous month. * TEST - generated by the user with a time range specified in the query parameters (`dateFrom`, `dateTo`). <section><h5>Required roles</h5> ROLE_TENANT_MANAGEMENT_ADMIN <b>OR</b> ROLE_TENANT_MANAGEMENT_CREATE </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostGenerateStatisticsFileRequestExample
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

            var apiInstance = new UsageStatisticsApi(config);
            var rangeStatisticsFile = new RangeStatisticsFile(); // RangeStatisticsFile | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Generate a statistics file report
                StatisticsFile result = apiInstance.PostGenerateStatisticsFileRequest(rangeStatisticsFile, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UsageStatisticsApi.PostGenerateStatisticsFileRequest: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostGenerateStatisticsFileRequestWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Generate a statistics file report
    ApiResponse<StatisticsFile> response = apiInstance.PostGenerateStatisticsFileRequestWithHttpInfo(rangeStatisticsFile, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UsageStatisticsApi.PostGenerateStatisticsFileRequestWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **rangeStatisticsFile** | [**RangeStatisticsFile**](RangeStatisticsFile.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**StatisticsFile**](StatisticsFile.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.tenantstatisticsdate+json
 - **Accept**: application/vnd.com.nsn.cumulocity.tenantstatisticsfile+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | A statistics file was generated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

