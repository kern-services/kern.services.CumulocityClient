# kern.services.CumulocityClient.Api.GroupsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteGroupByIdResource**](GroupsApi.md#deletegroupbyidresource) | **DELETE** /user/{tenantId}/groups/{groupId} | Delete a specific user group for a specific tenant
[**GetGroupByIdResource**](GroupsApi.md#getgroupbyidresource) | **GET** /user/{tenantId}/groups/{groupId} | Retrieve a specific user group for a specific tenant
[**GetGroupByNameResource**](GroupsApi.md#getgroupbynameresource) | **GET** /user/{tenantId}/groupByName/{groupName} | Retrieve a user group by group name for a specific tenant
[**GetGroupCollectionResource**](GroupsApi.md#getgroupcollectionresource) | **GET** /user/{tenantId}/groups | Retrieve all user groups of a specific tenant
[**GetGroupReferenceCollectionResource**](GroupsApi.md#getgroupreferencecollectionresource) | **GET** /user/{tenantId}/users/{userId}/groups | Get all user groups for specific user in a specific tenant
[**PostGroupCollectionResource**](GroupsApi.md#postgroupcollectionresource) | **POST** /user/{tenantId}/groups | Create a user group for a specific tenant
[**PutGroupByIdResource**](GroupsApi.md#putgroupbyidresource) | **PUT** /user/{tenantId}/groups/{groupId} | Update a specific user group for a specific tenant



## DeleteGroupByIdResource

> void DeleteGroupByIdResource (string tenantId, int groupId)

Delete a specific user group for a specific tenant

Delete a specific user group (by a given user group ID) for a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteGroupByIdResourceExample
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

            var apiInstance = new GroupsApi(Configuration.Default);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var groupId = 2;  // int | Unique identifier of the user group.

            try
            {
                // Delete a specific user group for a specific tenant
                apiInstance.DeleteGroupByIdResource(tenantId, groupId);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling GroupsApi.DeleteGroupByIdResource: " + e.Message );
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
 **groupId** | **int**| Unique identifier of the user group. | 

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
| **204** | A user group was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **404** | Group not found. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)


## GetGroupByIdResource

> Group GetGroupByIdResource (string tenantId, int groupId)

Retrieve a specific user group for a specific tenant

Retrieve a specific user group (by a given user group ID) for a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is parent of the user <b>AND</b> is not the current user </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetGroupByIdResourceExample
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

            var apiInstance = new GroupsApi(Configuration.Default);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var groupId = 2;  // int | Unique identifier of the user group.

            try
            {
                // Retrieve a specific user group for a specific tenant
                Group result = apiInstance.GetGroupByIdResource(tenantId, groupId);
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling GroupsApi.GetGroupByIdResource: " + e.Message );
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
 **groupId** | **int**| Unique identifier of the user group. | 

### Return type

[**Group**](Group.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/vnd.com.nsn.cumulocity.group+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request succeeded and the user group is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **404** | Group not found. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)


## GetGroupByNameResource

> Group GetGroupByNameResource (string tenantId, string groupName)

Retrieve a user group by group name for a specific tenant

Retrieve a user group by group name for a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> has access to groups </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetGroupByNameResourceExample
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

            var apiInstance = new GroupsApi(Configuration.Default);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var groupName = admins;  // string | The name of the user group.

            try
            {
                // Retrieve a user group by group name for a specific tenant
                Group result = apiInstance.GetGroupByNameResource(tenantId, groupName);
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling GroupsApi.GetGroupByNameResource: " + e.Message );
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
 **groupName** | **string**| The name of the user group. | 

### Return type

[**Group**](Group.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/vnd.com.nsn.cumulocity.group+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request succeeded and the user group is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **404** | Group not found. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)


## GetGroupCollectionResource

> UserGroupCollection GetGroupCollectionResource (string tenantId, int? currentPage = null, int? pageSize = null, bool? withTotalElements = null, bool? withTotalPages = null)

Retrieve all user groups of a specific tenant

Retrieve all user groups of a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetGroupCollectionResourceExample
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

            var apiInstance = new GroupsApi(Configuration.Default);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve all user groups of a specific tenant
                UserGroupCollection result = apiInstance.GetGroupCollectionResource(tenantId, currentPage, pageSize, withTotalElements, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling GroupsApi.GetGroupCollectionResource: " + e.Message );
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

[**UserGroupCollection**](UserGroupCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/vnd.com.nsn.cumulocity.groupcollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and all user groups are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)


## GetGroupReferenceCollectionResource

> GroupReferenceCollection GetGroupReferenceCollectionResource (string tenantId, string userId, int? currentPage = null, int? pageSize = null, bool? withTotalElements = null, bool? withTotalPages = null)

Get all user groups for specific user in a specific tenant

Get all user groups for a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is parent of the user </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetGroupReferenceCollectionResourceExample
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

            var apiInstance = new GroupsApi(Configuration.Default);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var userId = jdoe;  // string | Unique identifier of the a user.
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Get all user groups for specific user in a specific tenant
                GroupReferenceCollection result = apiInstance.GetGroupReferenceCollectionResource(tenantId, userId, currentPage, pageSize, withTotalElements, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling GroupsApi.GetGroupReferenceCollectionResource: " + e.Message );
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
 **userId** | **string**| Unique identifier of the a user. | 
 **currentPage** | **int?**| The current page of the paginated results. | [optional] [default to 1]
 **pageSize** | **int?**| Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5]
 **withTotalElements** | **bool?**| When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false]
 **withTotalPages** | **bool?**| When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false]

### Return type

[**GroupReferenceCollection**](GroupReferenceCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: application/vnd.com.nsn.cumulocity.groupreferencecollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request succeeded and all groups for the user are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **404** | User not found. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)


## PostGroupCollectionResource

> Group PostGroupCollectionResource (string tenantId, PostGroupCollectionResourceRequest postGroupCollectionResourceRequest, string accept = null)

Create a user group for a specific tenant

Create a user group for a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostGroupCollectionResourceExample
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

            var apiInstance = new GroupsApi(Configuration.Default);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var postGroupCollectionResourceRequest = new PostGroupCollectionResourceRequest(); // PostGroupCollectionResourceRequest | 
            var accept = application/json;  // string | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Create a user group for a specific tenant
                Group result = apiInstance.PostGroupCollectionResource(tenantId, postGroupCollectionResourceRequest, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling GroupsApi.PostGroupCollectionResource: " + e.Message );
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
 **postGroupCollectionResourceRequest** | [**PostGroupCollectionResourceRequest**](PostGroupCollectionResourceRequest.md)|  | 
 **accept** | **string**| Advertises which content types, expressed as MIME types, the client is able to understand. | [optional] 

### Return type

[**Group**](Group.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

- **Content-Type**: application/vnd.com.nsn.cumulocity.group+json
- **Accept**: application/vnd.com.nsn.cumulocity.group+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | A user group was created. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **409** | Duplicate – Group name already exists. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)


## PutGroupByIdResource

> Group PutGroupByIdResource (string tenantId, int groupId, PostGroupCollectionResourceRequest postGroupCollectionResourceRequest, string accept = null)

Update a specific user group for a specific tenant

Update a specific user group (by a given user group ID) for a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutGroupByIdResourceExample
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

            var apiInstance = new GroupsApi(Configuration.Default);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var groupId = 2;  // int | Unique identifier of the user group.
            var postGroupCollectionResourceRequest = new PostGroupCollectionResourceRequest(); // PostGroupCollectionResourceRequest | 
            var accept = application/json;  // string | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Update a specific user group for a specific tenant
                Group result = apiInstance.PutGroupByIdResource(tenantId, groupId, postGroupCollectionResourceRequest, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling GroupsApi.PutGroupByIdResource: " + e.Message );
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
 **groupId** | **int**| Unique identifier of the user group. | 
 **postGroupCollectionResourceRequest** | [**PostGroupCollectionResourceRequest**](PostGroupCollectionResourceRequest.md)|  | 
 **accept** | **string**| Advertises which content types, expressed as MIME types, the client is able to understand. | [optional] 

### Return type

[**Group**](Group.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

- **Content-Type**: application/vnd.com.nsn.cumulocity.group+json
- **Accept**: application/vnd.com.nsn.cumulocity.group+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A user group was updated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **404** | Group not found. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)

