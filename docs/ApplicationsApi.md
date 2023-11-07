# kern.services.CumulocityClient.Api.ApplicationsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**DeleteApplicationResource**](ApplicationsApi.md#deleteapplicationresource) | **DELETE** /application/applications/{id} | Delete an application |
| [**GetAbstractApplicationCollectionResource**](ApplicationsApi.md#getabstractapplicationcollectionresource) | **GET** /application/applications | Retrieve all applications |
| [**GetApplicationResource**](ApplicationsApi.md#getapplicationresource) | **GET** /application/applications/{id} | Retrieve a specific application |
| [**GetApplicationsByNameCollectionResource**](ApplicationsApi.md#getapplicationsbynamecollectionresource) | **GET** /application/applicationsByName/{name} | Retrieve applications by name |
| [**GetApplicationsByOwnerCollectionResource**](ApplicationsApi.md#getapplicationsbyownercollectionresource) | **GET** /application/applicationsByOwner/{tenantId} | Retrieve applications by owner |
| [**GetApplicationsByTenantCollectionResource**](ApplicationsApi.md#getapplicationsbytenantcollectionresource) | **GET** /application/applicationsByTenant/{tenantId} | Retrieve applications by tenant |
| [**GetApplicationsByUserCollectionResource**](ApplicationsApi.md#getapplicationsbyusercollectionresource) | **GET** /application/applicationsByUser/{username} | Retrieve applications by user |
| [**PostApplicationCollectionResource**](ApplicationsApi.md#postapplicationcollectionresource) | **POST** /application/applications | Create an application |
| [**PostApplicationResource**](ApplicationsApi.md#postapplicationresource) | **POST** /application/applications/{id}/clone | Copy an application |
| [**PutApplicationResource**](ApplicationsApi.md#putapplicationresource) | **PUT** /application/applications/{id} | Update a specific application |

<a name="deleteapplicationresource"></a>
# **DeleteApplicationResource**
> void DeleteApplicationResource (string id, bool? force = null, string? xCumulocityProcessingMode = null)

Delete an application

Delete an application (by a given ID). This method is not supported by microservice applications.  > **&#9432; Info:** With regards to a hosted application, there is a caching mechanism in place that keeps the information about the placement of application files (html, javascript, css, fonts, etc.). Removing a hosted application, in normal circumstances, will cause the subsequent requests for application files to fail with an HTTP 404 error because the application is removed synchronously, its files are immediately removed on the node serving the request and at the same time the information is propagated to other nodes – but in rare cases there might be a delay with this propagation. In such situations, the files of the removed application can be served from those nodes up until the aforementioned cache expires. For the same reason, the cache can also cause HTTP 404 errors when the application is updated as it will keep the path to the files of the old version of the application. The cache is filled on demand, so there should not be issues if application files were not accessed prior to the delete request. The expiration delay of the cache can differ, but should not take more than one minute.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN <b>AND</b> tenant is the owner of the application </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteApplicationResourceExample
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

            var apiInstance = new ApplicationsApi(config);
            var id = 20200301;  // string | Unique identifier of the application.
            var force = true;  // bool? | Force deletion by unsubscribing all tenants from the application first and then deleting the application itself. (optional)  (default to false)
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Delete an application
                apiInstance.DeleteApplicationResource(id, force, xCumulocityProcessingMode);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationsApi.DeleteApplicationResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteApplicationResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete an application
    apiInstance.DeleteApplicationResourceWithHttpInfo(id, force, xCumulocityProcessingMode);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationsApi.DeleteApplicationResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the application. |  |
| **force** | **bool?** | Force deletion by unsubscribing all tenants from the application first and then deleting the application itself. | [optional] [default to false] |
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
| **204** | An application was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **404** | Application not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getabstractapplicationcollectionresource"></a>
# **GetAbstractApplicationCollectionResource**
> ApplicationCollection GetAbstractApplicationCollectionResource (int? currentPage = null, string? name = null, string? owner = null, int? pageSize = null, string? providedFor = null, string? subscriber = null, string? tenant = null, string? type = null, string? user = null, bool? withTotalElements = null, bool? withTotalPages = null, bool? hasVersions = null)

Retrieve all applications

Retrieve all applications on your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetAbstractApplicationCollectionResourceExample
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

            var apiInstance = new ApplicationsApi(config);
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var name = devicemanagement;  // string? | The name of the application. (optional) 
            var owner = management;  // string? | The ID of the tenant that owns the applications. (optional) 
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var providedFor = t07007007;  // string? | The ID of a tenant that is subscribed to the applications but doesn't own them. (optional) 
            var subscriber = management;  // string? | The ID of a tenant that is subscribed to the applications. (optional) 
            var tenant = management;  // string? | The ID of a tenant that either owns the application or is subscribed to the applications. (optional) 
            var type = HOSTED;  // string? | The type of the application. It is possible to use multiple values separated by a comma. For example, `EXTERNAL,HOSTED` will return only applications with type `EXTERNAL` or `HOSTED`. (optional) 
            var user = jdoe;  // string? | The ID of a user that has access to the applications. (optional) 
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var hasVersions = true;  // bool? | When set to `true`, the returned result contains applications with an `applicationVersions` field that is not empty. When set to `false`, the result will contain applications with an empty `applicationVersions` field. (optional) 

            try
            {
                // Retrieve all applications
                ApplicationCollection result = apiInstance.GetAbstractApplicationCollectionResource(currentPage, name, owner, pageSize, providedFor, subscriber, tenant, type, user, withTotalElements, withTotalPages, hasVersions);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationsApi.GetAbstractApplicationCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAbstractApplicationCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all applications
    ApiResponse<ApplicationCollection> response = apiInstance.GetAbstractApplicationCollectionResourceWithHttpInfo(currentPage, name, owner, pageSize, providedFor, subscriber, tenant, type, user, withTotalElements, withTotalPages, hasVersions);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationsApi.GetAbstractApplicationCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **name** | **string?** | The name of the application. | [optional]  |
| **owner** | **string?** | The ID of the tenant that owns the applications. | [optional]  |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **providedFor** | **string?** | The ID of a tenant that is subscribed to the applications but doesn&#39;t own them. | [optional]  |
| **subscriber** | **string?** | The ID of a tenant that is subscribed to the applications. | [optional]  |
| **tenant** | **string?** | The ID of a tenant that either owns the application or is subscribed to the applications. | [optional]  |
| **type** | **string?** | The type of the application. It is possible to use multiple values separated by a comma. For example, &#x60;EXTERNAL,HOSTED&#x60; will return only applications with type &#x60;EXTERNAL&#x60; or &#x60;HOSTED&#x60;. | [optional]  |
| **user** | **string?** | The ID of a user that has access to the applications. | [optional]  |
| **withTotalElements** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |
| **hasVersions** | **bool?** | When set to &#x60;true&#x60;, the returned result contains applications with an &#x60;applicationVersions&#x60; field that is not empty. When set to &#x60;false&#x60;, the result will contain applications with an empty &#x60;applicationVersions&#x60; field. | [optional]  |

### Return type

[**ApplicationCollection**](ApplicationCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.applicationcollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the list of applications is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getapplicationresource"></a>
# **GetApplicationResource**
> Application GetApplicationResource (string id)

Retrieve a specific application

Retrieve a specific application (by a given ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ <b>OR</b> current user has explicit access to the application </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetApplicationResourceExample
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

            var apiInstance = new ApplicationsApi(config);
            var id = 20200301;  // string | Unique identifier of the application.

            try
            {
                // Retrieve a specific application
                Application result = apiInstance.GetApplicationResource(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationsApi.GetApplicationResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetApplicationResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a specific application
    ApiResponse<Application> response = apiInstance.GetApplicationResourceWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationsApi.GetApplicationResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the application. |  |

### Return type

[**Application**](Application.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.application+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the application is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Application not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getapplicationsbynamecollectionresource"></a>
# **GetApplicationsByNameCollectionResource**
> GetApplicationsByNameCollectionResource200Response GetApplicationsByNameCollectionResource (string name)

Retrieve applications by name

Retrieve applications by name.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetApplicationsByNameCollectionResourceExample
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

            var apiInstance = new ApplicationsApi(config);
            var name = my-application;  // string | The name of the application.

            try
            {
                // Retrieve applications by name
                GetApplicationsByNameCollectionResource200Response result = apiInstance.GetApplicationsByNameCollectionResource(name);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationsApi.GetApplicationsByNameCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetApplicationsByNameCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve applications by name
    ApiResponse<GetApplicationsByNameCollectionResource200Response> response = apiInstance.GetApplicationsByNameCollectionResourceWithHttpInfo(name);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationsApi.GetApplicationsByNameCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | The name of the application. |  |

### Return type

[**GetApplicationsByNameCollectionResource200Response**](GetApplicationsByNameCollectionResource200Response.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.applicationcollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the applications are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getapplicationsbyownercollectionresource"></a>
# **GetApplicationsByOwnerCollectionResource**
> GetApplicationsByOwnerCollectionResource200Response GetApplicationsByOwnerCollectionResource (string tenantId, int? currentPage = null, int? pageSize = null, bool? withTotalElements = null, bool? withTotalPages = null)

Retrieve applications by owner

Retrieve all applications owned by a particular tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetApplicationsByOwnerCollectionResourceExample
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

            var apiInstance = new ApplicationsApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve applications by owner
                GetApplicationsByOwnerCollectionResource200Response result = apiInstance.GetApplicationsByOwnerCollectionResource(tenantId, currentPage, pageSize, withTotalElements, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationsApi.GetApplicationsByOwnerCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetApplicationsByOwnerCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve applications by owner
    ApiResponse<GetApplicationsByOwnerCollectionResource200Response> response = apiInstance.GetApplicationsByOwnerCollectionResourceWithHttpInfo(tenantId, currentPage, pageSize, withTotalElements, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationsApi.GetApplicationsByOwnerCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **withTotalElements** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**GetApplicationsByOwnerCollectionResource200Response**](GetApplicationsByOwnerCollectionResource200Response.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.applicationcollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the applications are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getapplicationsbytenantcollectionresource"></a>
# **GetApplicationsByTenantCollectionResource**
> GetApplicationsByTenantCollectionResource200Response GetApplicationsByTenantCollectionResource (string tenantId)

Retrieve applications by tenant

Retrieve applications subscribed or owned by a particular tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetApplicationsByTenantCollectionResourceExample
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

            var apiInstance = new ApplicationsApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.

            try
            {
                // Retrieve applications by tenant
                GetApplicationsByTenantCollectionResource200Response result = apiInstance.GetApplicationsByTenantCollectionResource(tenantId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationsApi.GetApplicationsByTenantCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetApplicationsByTenantCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve applications by tenant
    ApiResponse<GetApplicationsByTenantCollectionResource200Response> response = apiInstance.GetApplicationsByTenantCollectionResourceWithHttpInfo(tenantId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationsApi.GetApplicationsByTenantCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |

### Return type

[**GetApplicationsByTenantCollectionResource200Response**](GetApplicationsByTenantCollectionResource200Response.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.applicationcollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the applications are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getapplicationsbyusercollectionresource"></a>
# **GetApplicationsByUserCollectionResource**
> GetApplicationsByUserCollectionResource200Response GetApplicationsByUserCollectionResource (string username, int? currentPage = null, int? pageSize = null, bool? withTotalElements = null, bool? withTotalPages = null)

Retrieve applications by user

Retrieve all applications for a particular user (by a given username).  <section><h5>Required roles</h5> (ROLE_USER_MANAGEMENT_OWN_READ <b>AND</b> is the current user) <b>OR</b> (ROLE_USER_MANAGEMENT_READ <b>AND</b> ROLE_APPLICATION_MANAGEMENT_READ) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetApplicationsByUserCollectionResourceExample
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

            var apiInstance = new ApplicationsApi(config);
            var username = jdoe;  // string | The username of the a user.
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve applications by user
                GetApplicationsByUserCollectionResource200Response result = apiInstance.GetApplicationsByUserCollectionResource(username, currentPage, pageSize, withTotalElements, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationsApi.GetApplicationsByUserCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetApplicationsByUserCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve applications by user
    ApiResponse<GetApplicationsByUserCollectionResource200Response> response = apiInstance.GetApplicationsByUserCollectionResourceWithHttpInfo(username, currentPage, pageSize, withTotalElements, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationsApi.GetApplicationsByUserCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **username** | **string** | The username of the a user. |  |
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **withTotalElements** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**GetApplicationsByUserCollectionResource200Response**](GetApplicationsByUserCollectionResource200Response.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.applicationcollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the applications are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postapplicationcollectionresource"></a>
# **PostApplicationCollectionResource**
> Application PostApplicationCollectionResource (PostApplicationCollectionResourceRequest postApplicationCollectionResourceRequest, string? accept = null, string? xCumulocityProcessingMode = null)

Create an application

Create an application on your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostApplicationCollectionResourceExample
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

            var apiInstance = new ApplicationsApi(config);
            var postApplicationCollectionResourceRequest = new PostApplicationCollectionResourceRequest(); // PostApplicationCollectionResourceRequest | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Create an application
                Application result = apiInstance.PostApplicationCollectionResource(postApplicationCollectionResourceRequest, accept, xCumulocityProcessingMode);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationsApi.PostApplicationCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostApplicationCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create an application
    ApiResponse<Application> response = apiInstance.PostApplicationCollectionResourceWithHttpInfo(postApplicationCollectionResourceRequest, accept, xCumulocityProcessingMode);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationsApi.PostApplicationCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **postApplicationCollectionResourceRequest** | [**PostApplicationCollectionResourceRequest**](PostApplicationCollectionResourceRequest.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

[**Application**](Application.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.application+json
 - **Accept**: application/vnd.com.nsn.cumulocity.application+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | An application was created. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **409** | Duplicate key/name. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postapplicationresource"></a>
# **PostApplicationResource**
> Application PostApplicationResource (string id, string? accept = null, string? xCumulocityProcessingMode = null)

Copy an application

Copy an application (by a given ID).  This method is not supported by microservice applications.  A request to the \"clone\" resource creates a new application based on an already existing one.  The properties are copied to the newly created application and the prefix \"clone\" is added to the properties `name`, `key` and `contextPath` in order to be unique.  If the target application is hosted and has an active version, the new application will have the active version with the same content. <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostApplicationResourceExample
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

            var apiInstance = new ApplicationsApi(config);
            var id = 20200301;  // string | Unique identifier of the application.
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Copy an application
                Application result = apiInstance.PostApplicationResource(id, accept, xCumulocityProcessingMode);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationsApi.PostApplicationResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostApplicationResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Copy an application
    ApiResponse<Application> response = apiInstance.PostApplicationResourceWithHttpInfo(id, accept, xCumulocityProcessingMode);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationsApi.PostApplicationResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the application. |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

[**Application**](Application.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.application+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | An application was copied. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **422** | Unprocessable Entity – method not supported |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="putapplicationresource"></a>
# **PutApplicationResource**
> Application PutApplicationResource (string id, PutApplicationResourceRequest putApplicationResourceRequest, string? accept = null, string? xCumulocityProcessingMode = null)

Update a specific application

Update a specific application (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutApplicationResourceExample
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

            var apiInstance = new ApplicationsApi(config);
            var id = 20200301;  // string | Unique identifier of the application.
            var putApplicationResourceRequest = new PutApplicationResourceRequest(); // PutApplicationResourceRequest | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Update a specific application
                Application result = apiInstance.PutApplicationResource(id, putApplicationResourceRequest, accept, xCumulocityProcessingMode);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationsApi.PutApplicationResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutApplicationResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update a specific application
    ApiResponse<Application> response = apiInstance.PutApplicationResourceWithHttpInfo(id, putApplicationResourceRequest, accept, xCumulocityProcessingMode);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationsApi.PutApplicationResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the application. |  |
| **putApplicationResourceRequest** | [**PutApplicationResourceRequest**](PutApplicationResourceRequest.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

[**Application**](Application.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.application+json
 - **Accept**: application/vnd.com.nsn.cumulocity.application+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | An application was updated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Application not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

