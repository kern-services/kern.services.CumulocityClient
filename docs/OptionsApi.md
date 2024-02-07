# kern.services.CumulocityClient.Api.OptionsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**DeleteOptionResource**](OptionsApi.md#deleteoptionresource) | **DELETE** /tenant/options/{category}/{key} | Remove a specific option |
| [**GetCategoryOptionResource**](OptionsApi.md#getcategoryoptionresource) | **GET** /tenant/options/{category} | Retrieve all options by category |
| [**GetOptionCollectionResource**](OptionsApi.md#getoptioncollectionresource) | **GET** /tenant/options | Retrieve all options |
| [**GetOptionResource**](OptionsApi.md#getoptionresource) | **GET** /tenant/options/{category}/{key} | Retrieve a specific option |
| [**PostOptionCollectionResource**](OptionsApi.md#postoptioncollectionresource) | **POST** /tenant/options | Create an option |
| [**PutCategoryOptionResource**](OptionsApi.md#putcategoryoptionresource) | **PUT** /tenant/options/{category} | Update options by category |
| [**PutOptionResource**](OptionsApi.md#putoptionresource) | **PUT** /tenant/options/{category}/{key} | Update a specific option |

<a id="deleteoptionresource"></a>
# **DeleteOptionResource**
> void DeleteOptionResource (string category, string key)

Remove a specific option

Remove a specific option (by a given category and key) on your tenant.  <section><h5>Required roles</h5> ROLE_OPTION_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteOptionResourceExample
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

            var apiInstance = new OptionsApi(config);
            var category = alarm.type.mapping;  // string | The category of the options.
            var key = temp_too_high;  // string | The key of an option.

            try
            {
                // Remove a specific option
                apiInstance.DeleteOptionResource(category, key);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OptionsApi.DeleteOptionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteOptionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove a specific option
    apiInstance.DeleteOptionResourceWithHttpInfo(category, key);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling OptionsApi.DeleteOptionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **category** | **string** | The category of the options. |  |
| **key** | **string** | The key of an option. |  |

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
| **204** | An option was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Option not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getcategoryoptionresource"></a>
# **GetCategoryOptionResource**
> Dictionary&lt;string, Object&gt; GetCategoryOptionResource (string category)

Retrieve all options by category

Retrieve all the options (by a specified category) on your tenant.  <section><h5>Required roles</h5> ROLE_OPTION_MANAGEMENT_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetCategoryOptionResourceExample
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

            var apiInstance = new OptionsApi(config);
            var category = alarm.type.mapping;  // string | The category of the options.

            try
            {
                // Retrieve all options by category
                Dictionary<string, Object> result = apiInstance.GetCategoryOptionResource(category);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OptionsApi.GetCategoryOptionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetCategoryOptionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all options by category
    ApiResponse<Dictionary<string, Object>> response = apiInstance.GetCategoryOptionResourceWithHttpInfo(category);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling OptionsApi.GetCategoryOptionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **category** | **string** | The category of the options. |  |

### Return type

**Dictionary<string, Object>**

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.option+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the options are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getoptioncollectionresource"></a>
# **GetOptionCollectionResource**
> OptionCollection GetOptionCollectionResource (int? currentPage = null, int? pageSize = null, bool? withTotalPages = null)

Retrieve all options

Retrieve all the options available on the tenant.  <section><h5>Required roles</h5> ROLE_OPTION_MANAGEMENT_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetOptionCollectionResourceExample
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

            var apiInstance = new OptionsApi(config);
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve all options
                OptionCollection result = apiInstance.GetOptionCollectionResource(currentPage, pageSize, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OptionsApi.GetOptionCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetOptionCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all options
    ApiResponse<OptionCollection> response = apiInstance.GetOptionCollectionResourceWithHttpInfo(currentPage, pageSize, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling OptionsApi.GetOptionCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**OptionCollection**](OptionCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.optioncollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the options are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getoptionresource"></a>
# **GetOptionResource**
> Option GetOptionResource (string category, string key)

Retrieve a specific option

Retrieve a specific option (by a given category and key) on your tenant.  <section><h5>Required roles</h5> ROLE_OPTION_MANAGEMENT_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetOptionResourceExample
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

            var apiInstance = new OptionsApi(config);
            var category = alarm.type.mapping;  // string | The category of the options.
            var key = temp_too_high;  // string | The key of an option.

            try
            {
                // Retrieve a specific option
                Option result = apiInstance.GetOptionResource(category, key);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OptionsApi.GetOptionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetOptionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a specific option
    ApiResponse<Option> response = apiInstance.GetOptionResourceWithHttpInfo(category, key);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling OptionsApi.GetOptionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **category** | **string** | The category of the options. |  |
| **key** | **string** | The key of an option. |  |

### Return type

[**Option**](Option.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.option+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the option is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Option not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="postoptioncollectionresource"></a>
# **PostOptionCollectionResource**
> Option PostOptionCollectionResource (PostOptionCollectionResourceRequest postOptionCollectionResourceRequest, string? accept = null)

Create an option

Create an option on your tenant.  Options are category-key-value tuples which store tenant configurations. Some categories of options allow the creation of new ones, while others are limited to predefined set of keys.  Any option of any tenant can be defined as \"non-editable\" by the \"management\" tenant; once done, any PUT or DELETE requests made on that option by the tenant owner will result in a 403 error (Unauthorized).  ### Default option categories  **access.control**  | Key | Default value | Predefined | Description | |- -|- -|- -|- -| | allow.origin | * | Yes | Comma separated list of domains allowed for execution of CORS. Wildcards are allowed (for example, `*.cumuclocity.com`) |  **alarm.type.mapping**  | Key  | Predefined | Description | |- -|- -|- -| | &lt;ALARM_TYPE> | No | Overrides the severity and alarm text for the alarm with type &lt;ALARM_TYPE>. The severity and text are specified as `<ALARM_SEVERITY>\\|<ALARM_TEXT>`. If either part is empty, the value will not be overridden. If the severity is NONE, the alarm will be suppressed. Example: `\"CRITICAL\\|temperature too high\"`|  ### Encrypted credentials  Adding a \"credentials.\" prefix to the `key` will make the `value` of the option encrypted. When the option is  sent to a microservice, the \"credentials.\" prefix is removed and the `value` is decrypted. For example:  ```json {   \"category\": \"secrets\",   \"key\": \"credentials.mykey\",   \"value\": \"myvalue\" } ```  In that particular example, the request will contain an additional header `\"Mykey\": \"myvalue\"`.  <section><h5>Required roles</h5> ROLE_OPTION_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostOptionCollectionResourceExample
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

            var apiInstance = new OptionsApi(config);
            var postOptionCollectionResourceRequest = new PostOptionCollectionResourceRequest(); // PostOptionCollectionResourceRequest | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Create an option
                Option result = apiInstance.PostOptionCollectionResource(postOptionCollectionResourceRequest, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OptionsApi.PostOptionCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostOptionCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create an option
    ApiResponse<Option> response = apiInstance.PostOptionCollectionResourceWithHttpInfo(postOptionCollectionResourceRequest, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling OptionsApi.PostOptionCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **postOptionCollectionResourceRequest** | [**PostOptionCollectionResourceRequest**](PostOptionCollectionResourceRequest.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**Option**](Option.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.option+json
 - **Accept**: application/vnd.com.nsn.cumulocity.option+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | An option was created. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="putcategoryoptionresource"></a>
# **PutCategoryOptionResource**
> Dictionary&lt;string, Object&gt; PutCategoryOptionResource (string category, Dictionary<string, Object> requestBody, string? accept = null)

Update options by category

Update one or more options (by a specified category) on your tenant.  <section><h5>Required roles</h5> ROLE_OPTION_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutCategoryOptionResourceExample
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

            var apiInstance = new OptionsApi(config);
            var category = alarm.type.mapping;  // string | The category of the options.
            var requestBody = new Dictionary<string, Object>(); // Dictionary<string, Object> | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Update options by category
                Dictionary<string, Object> result = apiInstance.PutCategoryOptionResource(category, requestBody, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OptionsApi.PutCategoryOptionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutCategoryOptionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update options by category
    ApiResponse<Dictionary<string, Object>> response = apiInstance.PutCategoryOptionResourceWithHttpInfo(category, requestBody, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling OptionsApi.PutCategoryOptionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **category** | **string** | The category of the options. |  |
| **requestBody** | [**Dictionary&lt;string, Object&gt;**](Object.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

**Dictionary<string, Object>**

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/vnd.com.nsn.cumulocity.option+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A collection of options was updated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="putoptionresource"></a>
# **PutOptionResource**
> Option PutOptionResource (string category, string key, CategoryKeyOption categoryKeyOption, string? accept = null)

Update a specific option

Update the value of a specific option (by a given category and key) on your tenant.  <section><h5>Required roles</h5> ROLE_OPTION_MANAGEMENT_ADMIN <b>AND</b> the option is editable </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutOptionResourceExample
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

            var apiInstance = new OptionsApi(config);
            var category = alarm.type.mapping;  // string | The category of the options.
            var key = temp_too_high;  // string | The key of an option.
            var categoryKeyOption = new CategoryKeyOption(); // CategoryKeyOption | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Update a specific option
                Option result = apiInstance.PutOptionResource(category, key, categoryKeyOption, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OptionsApi.PutOptionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutOptionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update a specific option
    ApiResponse<Option> response = apiInstance.PutOptionResourceWithHttpInfo(category, key, categoryKeyOption, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling OptionsApi.PutOptionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **category** | **string** | The category of the options. |  |
| **key** | **string** | The key of an option. |  |
| **categoryKeyOption** | [**CategoryKeyOption**](CategoryKeyOption.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**Option**](Option.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/vnd.com.nsn.cumulocity.option+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | An option was updated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Option not found. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

