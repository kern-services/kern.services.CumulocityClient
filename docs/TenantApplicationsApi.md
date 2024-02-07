# kern.services.CumulocityClient.Api.TenantApplicationsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteTenantApplicationReferenceResource**](TenantApplicationsApi.md#deletetenantapplicationreferenceresource) | **DELETE** /tenant/tenants/{tenantId}/applications/{applicationId} | Unsubscribe from an application
[**GetTenantApplicationReferenceCollectionResource**](TenantApplicationsApi.md#gettenantapplicationreferencecollectionresource) | **GET** /tenant/tenants/{tenantId}/applications | Retrieve subscribed applications
[**PostTenantApplicationReferenceCollectionResource**](TenantApplicationsApi.md#posttenantapplicationreferencecollectionresource) | **POST** /tenant/tenants/{tenantId}/applications | Subscribe to an application



## DeleteTenantApplicationReferenceResource

> void DeleteTenantApplicationReferenceResource (string tenantId, string applicationId)

Unsubscribe from an application

Unsubscribe a tenant (by a given tenant ID) from an application (by a given application ID).  <section><h5>Required roles</h5> (ROLE_APPLICATION_MANAGEMENT_ADMIN <b>AND</b> is the application owner <b>AND</b> is the current tenant) <b>OR</b><br> ((ROLE_TENANT_MANAGEMENT_ADMIN <b>OR</b> ROLE_TENANT_MANAGEMENT_UPDATE) <b>AND</b> (the current tenant is its parent <b>OR</b> is the management tenant)) </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteTenantApplicationReferenceResourceExample
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

            var apiInstance = new TenantApplicationsApi(Configuration.Default);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var applicationId = 20200301;  // string | Unique identifier of the application.

            try
            {
                // Unsubscribe from an application
                apiInstance.DeleteTenantApplicationReferenceResource(tenantId, applicationId);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling TenantApplicationsApi.DeleteTenantApplicationReferenceResource: " + e.Message );
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
 **tenantId** | **string**| Unique identifier of a Cumulocity IoT tenant. | 
 **applicationId** | **string**| Unique identifier of the application. | 

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
| **204** | A tenant was unsubscribed from an application. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Tenant not found. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)


## GetTenantApplicationReferenceCollectionResource

> ApplicationReferenceCollection GetTenantApplicationReferenceCollectionResource (string tenantId, int? currentPage = null, int? pageSize = null, bool? withTotalElements = null, bool? withTotalPages = null)

Retrieve subscribed applications

Retrieve the tenant subscribed applications by a given tenant ID.  <section><h5>Required roles</h5> (ROLE_TENANT_MANAGEMENT_READ <b>OR</b> ROLE_TENANT_ADMIN) <b>AND</b> (the current tenant is its parent <b>OR</b> is the management tenant) </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetTenantApplicationReferenceCollectionResourceExample
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

            var apiInstance = new TenantApplicationsApi(Configuration.Default);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve subscribed applications
                ApplicationReferenceCollection result = apiInstance.GetTenantApplicationReferenceCollectionResource(tenantId, currentPage, pageSize, withTotalElements, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling TenantApplicationsApi.GetTenantApplicationReferenceCollectionResource: " + e.Message );
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
 **tenantId** | **string**| Unique identifier of a Cumulocity IoT tenant. | 
 **currentPage** | **int?**| The current page of the paginated results. | [optional] [default to 1]
 **pageSize** | **int?**| Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5]
 **withTotalElements** | **bool?**| When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false]
 **withTotalPages** | **bool?**| When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false]

### Return type

[**ApplicationReferenceCollection**](ApplicationReferenceCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/vnd.com.nsn.cumulocity.applicationreferencecollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the tenant applications are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **404** | Tenant not found. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)


## PostTenantApplicationReferenceCollectionResource

> ApplicationReference PostTenantApplicationReferenceCollectionResource (string tenantId, SubscribedApplicationReference subscribedApplicationReference, string accept = null)

Subscribe to an application

Subscribe a tenant (by a given ID) to an application.  <section><h5>Required roles</h5> 1. the current tenant is application owner and has the role ROLE_APPLICATION_MANAGEMENT_ADMIN <b>OR</b><br> 2. for applications that are not microservices, the current tenant is the management tenant or the parent of the application owner tenant, and the user has one of the follwoing roles: ROLE_TENANT_MANAGEMENT_ADMIN, ROLE_TENANT_MANAGEMENT_UPDATE <b>OR</b><br> 3. for microservices, the current tenant is the management tenant or the parent of the application owner tenant, and the user has the role ROLE_TENANT_MANAGEMENT_ADMIN OR ROLE_TENANT_MANAGEMENT_UPDATE and one of following conditions is met:<br> * the microservice has no manifest<br> * the microservice version is supported<br> * the current tenant is subscribed to 'feature-privileged-microservice-hosting' </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostTenantApplicationReferenceCollectionResourceExample
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

            var apiInstance = new TenantApplicationsApi(Configuration.Default);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var subscribedApplicationReference = new SubscribedApplicationReference(); // SubscribedApplicationReference | 
            var accept = application/json;  // string | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Subscribe to an application
                ApplicationReference result = apiInstance.PostTenantApplicationReferenceCollectionResource(tenantId, subscribedApplicationReference, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling TenantApplicationsApi.PostTenantApplicationReferenceCollectionResource: " + e.Message );
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
 **tenantId** | **string**| Unique identifier of a Cumulocity IoT tenant. | 
 **subscribedApplicationReference** | [**SubscribedApplicationReference**](SubscribedApplicationReference.md)|  | 
 **accept** | **string**| Advertises which content types, expressed as MIME types, the client is able to understand. | [optional] 

### Return type

[**ApplicationReference**](ApplicationReference.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

- **Content-Type**: application/vnd.com.nsn.cumulocity.applicationreference+json
- **Accept**: application/vnd.com.nsn.cumulocity.applicationreference+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | A tenant was subscribed to an application. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Application not found. |  -  |
| **409** | The application is already assigned to the tenant. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)

