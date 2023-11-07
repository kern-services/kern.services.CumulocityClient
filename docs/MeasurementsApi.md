# kern.services.CumulocityClient.Api.MeasurementsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**DeleteMeasurementCollectionResource**](MeasurementsApi.md#deletemeasurementcollectionresource) | **DELETE** /measurement/measurements | Remove measurement collections |
| [**DeleteMeasurementResource**](MeasurementsApi.md#deletemeasurementresource) | **DELETE** /measurement/measurements/{id} | Remove a specific measurement |
| [**GetMeasurementCollectionResource**](MeasurementsApi.md#getmeasurementcollectionresource) | **GET** /measurement/measurements | Retrieve all measurements |
| [**GetMeasurementResource**](MeasurementsApi.md#getmeasurementresource) | **GET** /measurement/measurements/{id} | Retrieve a specific measurement |
| [**GetMeasurementSeriesResource**](MeasurementsApi.md#getmeasurementseriesresource) | **GET** /measurement/measurements/series | Retrieve a list of series and their values |
| [**PostMeasurementCollectionResource**](MeasurementsApi.md#postmeasurementcollectionresource) | **POST** /measurement/measurements | Create a measurement |

<a name="deletemeasurementcollectionresource"></a>
# **DeleteMeasurementCollectionResource**
> void DeleteMeasurementCollectionResource (string? xCumulocityProcessingMode = null, DateTime? dateFrom = null, DateTime? dateTo = null, string? fragmentType = null, string? source = null, string? type = null)

Remove measurement collections

Remove measurement collections specified by query parameters.  DELETE requests are not synchronous. The response could be returned before the delete request has been completed. This may happen especially when there are a lot of measurements to be deleted.  > **⚠️ Important:** Note that it is possible to call this endpoint without providing any parameter - it may result in deleting all measurements and it is not recommended.  <section><h5>Required roles</h5> ROLE_MEASUREMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteMeasurementCollectionResourceExample
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

            var apiInstance = new MeasurementsApi(config);
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)
            var dateFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the measurement. (optional) 
            var dateTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the measurement. (optional) 
            var fragmentType = c8y_IsDevice;  // string? | A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional) 
            var source = 251994;  // string? | The managed object ID to which the measurement is associated. (optional) 
            var type = c8y_Water;  // string? | The type of measurement to search for. (optional) 

            try
            {
                // Remove measurement collections
                apiInstance.DeleteMeasurementCollectionResource(xCumulocityProcessingMode, dateFrom, dateTo, fragmentType, source, type);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeasurementsApi.DeleteMeasurementCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteMeasurementCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove measurement collections
    apiInstance.DeleteMeasurementCollectionResourceWithHttpInfo(xCumulocityProcessingMode, dateFrom, dateTo, fragmentType, source, type);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeasurementsApi.DeleteMeasurementCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |
| **dateFrom** | **DateTime?** | Start date or date and time of the measurement. | [optional]  |
| **dateTo** | **DateTime?** | End date or date and time of the measurement. | [optional]  |
| **fragmentType** | **string?** | A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. | [optional]  |
| **source** | **string?** | The managed object ID to which the measurement is associated. | [optional]  |
| **type** | **string?** | The type of measurement to search for. | [optional]  |

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
| **204** | A collection of measurements was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletemeasurementresource"></a>
# **DeleteMeasurementResource**
> void DeleteMeasurementResource (string id, string? xCumulocityProcessingMode = null)

Remove a specific measurement

Remove a specific measurement by a given ID.  <section><h5>Required roles</h5> ROLE_MEASUREMENT_ADMIN <b>OR</b> owner of the source <b>OR</b> MEASUREMENT_ADMIN permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteMeasurementResourceExample
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

            var apiInstance = new MeasurementsApi(config);
            var id = 102700509;  // string | Unique identifier of the measurement.
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Remove a specific measurement
                apiInstance.DeleteMeasurementResource(id, xCumulocityProcessingMode);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeasurementsApi.DeleteMeasurementResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteMeasurementResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove a specific measurement
    apiInstance.DeleteMeasurementResourceWithHttpInfo(id, xCumulocityProcessingMode);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeasurementsApi.DeleteMeasurementResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the measurement. |  |
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
| **204** | A measurement was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **404** | Measurement not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getmeasurementcollectionresource"></a>
# **GetMeasurementCollectionResource**
> MeasurementCollection GetMeasurementCollectionResource (int? currentPage = null, DateTime? dateFrom = null, DateTime? dateTo = null, int? pageSize = null, bool? revert = null, string? source = null, string? type = null, string? valueFragmentSeries = null, string? valueFragmentType = null, bool? withTotalElements = null, bool? withTotalPages = null)

Retrieve all measurements

Retrieve all measurements on your tenant, or a specific subset based on queries.  In case of executing [range queries](https://en.wikipedia.org/wiki/Range_query_(database)) between an upper and lower boundary, for example, querying using `dateFrom`–`dateTo`, the oldest registered measurements are returned first. It is possible to change the order using the query parameter `revert=true`.  For large measurement collections, querying older records without filters can be slow as the server needs to scan from the beginning of the input results set before beginning to return the results. For cases when older measurements should be retrieved, it is recommended to narrow the scope by using range queries based on the time stamp reported by a device. The scope of query can also be reduced significantly when a source device is provided.  Review [Measurements Specifics](#tag/Measurements-specifics) for details about data streaming and response formats.  <section><h5>Required roles</h5> ROLE_MEASUREMENT_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetMeasurementCollectionResourceExample
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

            var apiInstance = new MeasurementsApi(config);
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var dateFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | Start date or date and time of the measurement. (optional) 
            var dateTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | End date or date and time of the measurement. (optional) 
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var revert = true;  // bool? | If you are using a range query (that is, at least one of the `dateFrom` or `dateTo` parameters is included in the request), then setting `revert=true` will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  (optional)  (default to false)
            var source = 251994;  // string? | The managed object ID to which the measurement is associated. (optional) 
            var type = c8y_Water;  // string? | The type of measurement to search for. (optional) 
            var valueFragmentSeries = Temperature;  // string? | The specific series to search for. (optional) 
            var valueFragmentType = c8y_Steam;  // string? | A characteristic which identifies the measurement. (optional) 
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve all measurements
                MeasurementCollection result = apiInstance.GetMeasurementCollectionResource(currentPage, dateFrom, dateTo, pageSize, revert, source, type, valueFragmentSeries, valueFragmentType, withTotalElements, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeasurementsApi.GetMeasurementCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetMeasurementCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all measurements
    ApiResponse<MeasurementCollection> response = apiInstance.GetMeasurementCollectionResourceWithHttpInfo(currentPage, dateFrom, dateTo, pageSize, revert, source, type, valueFragmentSeries, valueFragmentType, withTotalElements, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeasurementsApi.GetMeasurementCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **dateFrom** | **DateTime?** | Start date or date and time of the measurement. | [optional]  |
| **dateTo** | **DateTime?** | End date or date and time of the measurement. | [optional]  |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **revert** | **bool?** | If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  | [optional] [default to false] |
| **source** | **string?** | The managed object ID to which the measurement is associated. | [optional]  |
| **type** | **string?** | The type of measurement to search for. | [optional]  |
| **valueFragmentSeries** | **string?** | The specific series to search for. | [optional]  |
| **valueFragmentType** | **string?** | A characteristic which identifies the measurement. | [optional]  |
| **withTotalElements** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**MeasurementCollection**](MeasurementCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.measurementcollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and all measurements are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getmeasurementresource"></a>
# **GetMeasurementResource**
> Measurement GetMeasurementResource (string id)

Retrieve a specific measurement

Retrieve a specific measurement by a given ID.  <section><h5>Required roles</h5> ROLE_MEASUREMENT_READ <b>OR</b> owner of the source <b>OR</b> MEASUREMENT_READ permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetMeasurementResourceExample
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

            var apiInstance = new MeasurementsApi(config);
            var id = 102700509;  // string | Unique identifier of the measurement.

            try
            {
                // Retrieve a specific measurement
                Measurement result = apiInstance.GetMeasurementResource(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeasurementsApi.GetMeasurementResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetMeasurementResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a specific measurement
    ApiResponse<Measurement> response = apiInstance.GetMeasurementResourceWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeasurementsApi.GetMeasurementResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the measurement. |  |

### Return type

[**Measurement**](Measurement.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.measurement+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the measurement is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Measurement not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getmeasurementseriesresource"></a>
# **GetMeasurementSeriesResource**
> MeasurementSeries GetMeasurementSeriesResource (DateTime dateFrom, DateTime dateTo, string source, string? aggregationType = null, bool? revert = null, List<string>? series = null)

Retrieve a list of series and their values

Retrieve a list of series (all or only those matching the specified names) and their values within a given period of a specific managed object (source).<br> A series is any fragment in measurement that contains a `value` property.  It is possible to fetch aggregated results using the `aggregationType` parameter. If the aggregation is not specified, the result will contain no more than 5000 values.  > **⚠️ Important:** For the aggregation to be done correctly, a device shall always use the same time zone when it sends dates.  <section><h5>Required roles</h5> ROLE_MEASUREMENT_READ <b>OR</b> owner of the source <b>OR</b> MEASUREMENT_READ permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetMeasurementSeriesResourceExample
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

            var apiInstance = new MeasurementsApi(config);
            var dateFrom = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime | Start date or date and time of the measurement.
            var dateTo = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime | End date or date and time of the measurement.
            var source = 251994;  // string | The managed object ID to which the measurement is associated.
            var aggregationType = MINUTELY;  // string? | Fetch aggregated results as specified. (optional) 
            var revert = true;  // bool? | If you are using a range query (that is, at least one of the `dateFrom` or `dateTo` parameters is included in the request), then setting `revert=true` will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  (optional)  (default to false)
            var series = new List<string>?(); // List<string>? | The specific series to search for. >**&#9432; Info:** If you query for multiple series at once, comma-separate the values.  (optional) 

            try
            {
                // Retrieve a list of series and their values
                MeasurementSeries result = apiInstance.GetMeasurementSeriesResource(dateFrom, dateTo, source, aggregationType, revert, series);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeasurementsApi.GetMeasurementSeriesResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetMeasurementSeriesResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a list of series and their values
    ApiResponse<MeasurementSeries> response = apiInstance.GetMeasurementSeriesResourceWithHttpInfo(dateFrom, dateTo, source, aggregationType, revert, series);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeasurementsApi.GetMeasurementSeriesResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **dateFrom** | **DateTime** | Start date or date and time of the measurement. |  |
| **dateTo** | **DateTime** | End date or date and time of the measurement. |  |
| **source** | **string** | The managed object ID to which the measurement is associated. |  |
| **aggregationType** | **string?** | Fetch aggregated results as specified. | [optional]  |
| **revert** | **bool?** | If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  | [optional] [default to false] |
| **series** | [**List&lt;string&gt;?**](string.md) | The specific series to search for. &gt;**&amp;#9432; Info:** If you query for multiple series at once, comma-separate the values.  | [optional]  |

### Return type

[**MeasurementSeries**](MeasurementSeries.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the series are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postmeasurementcollectionresource"></a>
# **PostMeasurementCollectionResource**
> Measurement PostMeasurementCollectionResource (Dictionary<string, Object> requestBody, string? accept = null, string? xCumulocityProcessingMode = null)

Create a measurement

A measurement must be associated with a source (managed object) identified by ID, and must specify the type of measurement and the time when it was measured by the device (for example, a thermometer).  Each measurement fragment is an object (for example, `c8y_Steam`) containing the actual measurements as properties. The property name represents the name of the measurement (for example, `Temperature`) and it contains two properties:  *   `value` - The value of the individual measurement. The maximum precision for floating point numbers is 64-bit IEEE 754. For integers it's a 64-bit two's complement integer. The `value` is mandatory for a fragment. *   `unit` - The unit of the measurements.  Review the [System of units](#section/System-of-units) section for details about the conversions of units. Also review the [Naming conventions of fragments](https://cumulocity.com/guides/concepts/domain-model/#naming-conventions-of-fragments) in the Concepts guide.  The example below uses `c8y_Steam` in the request body to illustrate a fragment for recording temperature measurements.  > **⚠️ Important:** Property names used for fragment and series must not contain whitespaces nor the special characters `. , * [ ] ( ) @ $`. This is required to ensure a correct processing and visualization of measurement series on UI graphs.  ### Create multiple measurements  It is also possible to create multiple measurements at once by sending a `measurements` array containing all the measurements to be created. The content type must be `application/vnd.com.nsn.cumulocity.measurementcollection+json`.  > **&#9432; Info:** For more details about fragments with specific meanings, review the sections [Device management library](#section/Device-management-library) and [Sensor library](#section/Sensor-library).  <section><h5>Required roles</h5> ROLE_MEASUREMENT_ADMIN <b>OR</b> owner of the source <b>OR</b> MEASUREMENT_ADMIN permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostMeasurementCollectionResourceExample
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

            var apiInstance = new MeasurementsApi(config);
            var requestBody = new Dictionary<string, Object>(); // Dictionary<string, Object> | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Create a measurement
                Measurement result = apiInstance.PostMeasurementCollectionResource(requestBody, accept, xCumulocityProcessingMode);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeasurementsApi.PostMeasurementCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostMeasurementCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create a measurement
    ApiResponse<Measurement> response = apiInstance.PostMeasurementCollectionResourceWithHttpInfo(requestBody, accept, xCumulocityProcessingMode);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeasurementsApi.PostMeasurementCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **requestBody** | [**Dictionary&lt;string, Object&gt;**](Object.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

[**Measurement**](Measurement.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.measurement+json, application/vnd.com.nsn.cumulocity.measurementcollection+json
 - **Accept**: application/vnd.com.nsn.cumulocity.measurement+json, application/vnd.com.nsn.cumulocity.measurementcollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | A measurement was created. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

