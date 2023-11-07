# kern.services.CumulocityClient.Api.InventoryRolesApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**DeleteInventoryAssignmentResourceById**](InventoryRolesApi.md#deleteinventoryassignmentresourcebyid) | **DELETE** /user/{tenantId}/users/{userId}/roles/inventory/{id} | Remove a specific inventory role assigned to a user |
| [**DeleteInventoryRoleResourceId**](InventoryRolesApi.md#deleteinventoryroleresourceid) | **DELETE** /user/inventoryroles/{id} | Remove a specific inventory role |
| [**GetInventoryAssignmentResource**](InventoryRolesApi.md#getinventoryassignmentresource) | **GET** /user/{tenantId}/users/{userId}/roles/inventory | Retrieve all inventory roles assigned to a user |
| [**GetInventoryAssignmentResourceById**](InventoryRolesApi.md#getinventoryassignmentresourcebyid) | **GET** /user/{tenantId}/users/{userId}/roles/inventory/{id} | Retrieve a specific inventory role assigned to a user |
| [**GetInventoryRoleResource**](InventoryRolesApi.md#getinventoryroleresource) | **GET** /user/inventoryroles | Retrieve all inventory roles |
| [**GetInventoryRoleResourceId**](InventoryRolesApi.md#getinventoryroleresourceid) | **GET** /user/inventoryroles/{id} | Retrieve a specific inventory role |
| [**PostInventoryAssignmentResource**](InventoryRolesApi.md#postinventoryassignmentresource) | **POST** /user/{tenantId}/users/{userId}/roles/inventory | Assign an inventory role to a user |
| [**PostInventoryRoleResource**](InventoryRolesApi.md#postinventoryroleresource) | **POST** /user/inventoryroles | Create an inventory role |
| [**PutInventoryAssignmentResourceById**](InventoryRolesApi.md#putinventoryassignmentresourcebyid) | **PUT** /user/{tenantId}/users/{userId}/roles/inventory/{id} | Update a specific inventory role assigned to a user |
| [**PutInventoryRoleResourceId**](InventoryRolesApi.md#putinventoryroleresourceid) | **PUT** /user/inventoryroles/{id} | Update a specific inventory role |

<a name="deleteinventoryassignmentresourcebyid"></a>
# **DeleteInventoryAssignmentResourceById**
> void DeleteInventoryAssignmentResourceById (string tenantId, string userId, int id)

Remove a specific inventory role assigned to a user

Remove a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN <b>AND</b> (is not in user hierarchy <b>OR</b> is root in the user hierarchy) <b>OR</b> ROLE_USER_MANAGEMENT_ADMIN <b>AND</b> is in user hiararchy <b>AND</b> has parent access to inventory assignments <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is parent of the user <b>AND</b> is not the current user <b>AND</b> has current user access to inventory assignments <b>AND</b> has parent access to inventory assignments </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteInventoryAssignmentResourceByIdExample
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

            var apiInstance = new InventoryRolesApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var userId = jdoe;  // string | Unique identifier of the a user.
            var id = 1;  // int | Unique identifier of the inventory assignment.

            try
            {
                // Remove a specific inventory role assigned to a user
                apiInstance.DeleteInventoryAssignmentResourceById(tenantId, userId, id);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling InventoryRolesApi.DeleteInventoryAssignmentResourceById: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteInventoryAssignmentResourceByIdWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove a specific inventory role assigned to a user
    apiInstance.DeleteInventoryAssignmentResourceByIdWithHttpInfo(tenantId, userId, id);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling InventoryRolesApi.DeleteInventoryAssignmentResourceByIdWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **userId** | **string** | Unique identifier of the a user. |  |
| **id** | **int** | Unique identifier of the inventory assignment. |  |

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
| **204** | An inventory assignment was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **404** | Role not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deleteinventoryroleresourceid"></a>
# **DeleteInventoryRoleResourceId**
> void DeleteInventoryRoleResourceId (int id)

Remove a specific inventory role

Remove a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteInventoryRoleResourceIdExample
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

            var apiInstance = new InventoryRolesApi(config);
            var id = 4;  // int | Unique identifier of the inventory role.

            try
            {
                // Remove a specific inventory role
                apiInstance.DeleteInventoryRoleResourceId(id);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling InventoryRolesApi.DeleteInventoryRoleResourceId: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteInventoryRoleResourceIdWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove a specific inventory role
    apiInstance.DeleteInventoryRoleResourceIdWithHttpInfo(id);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling InventoryRolesApi.DeleteInventoryRoleResourceIdWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **int** | Unique identifier of the inventory role. |  |

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
| **204** | An inventory role was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **404** | Role not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getinventoryassignmentresource"></a>
# **GetInventoryAssignmentResource**
> InventoryAssignmentCollection GetInventoryAssignmentResource (string tenantId, string userId)

Retrieve all inventory roles assigned to a user

Retrieve all inventory roles assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is the parent of the user </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetInventoryAssignmentResourceExample
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

            var apiInstance = new InventoryRolesApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var userId = jdoe;  // string | Unique identifier of the a user.

            try
            {
                // Retrieve all inventory roles assigned to a user
                InventoryAssignmentCollection result = apiInstance.GetInventoryAssignmentResource(tenantId, userId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling InventoryRolesApi.GetInventoryAssignmentResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetInventoryAssignmentResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all inventory roles assigned to a user
    ApiResponse<InventoryAssignmentCollection> response = apiInstance.GetInventoryAssignmentResourceWithHttpInfo(tenantId, userId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling InventoryRolesApi.GetInventoryAssignmentResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **userId** | **string** | Unique identifier of the a user. |  |

### Return type

[**InventoryAssignmentCollection**](InventoryAssignmentCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.inventoryassignmentcollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the inventory roles are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **404** | User not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getinventoryassignmentresourcebyid"></a>
# **GetInventoryAssignmentResourceById**
> InventoryAssignment GetInventoryAssignmentResourceById (string tenantId, string userId, int id)

Retrieve a specific inventory role assigned to a user

Retrieve a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is the parent of the user </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetInventoryAssignmentResourceByIdExample
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

            var apiInstance = new InventoryRolesApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var userId = jdoe;  // string | Unique identifier of the a user.
            var id = 1;  // int | Unique identifier of the inventory assignment.

            try
            {
                // Retrieve a specific inventory role assigned to a user
                InventoryAssignment result = apiInstance.GetInventoryAssignmentResourceById(tenantId, userId, id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling InventoryRolesApi.GetInventoryAssignmentResourceById: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetInventoryAssignmentResourceByIdWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a specific inventory role assigned to a user
    ApiResponse<InventoryAssignment> response = apiInstance.GetInventoryAssignmentResourceByIdWithHttpInfo(tenantId, userId, id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling InventoryRolesApi.GetInventoryAssignmentResourceByIdWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **userId** | **string** | Unique identifier of the a user. |  |
| **id** | **int** | Unique identifier of the inventory assignment. |  |

### Return type

[**InventoryAssignment**](InventoryAssignment.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.inventoryassignment+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the inventory role is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **404** | Role not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getinventoryroleresource"></a>
# **GetInventoryRoleResource**
> InventoryRoleCollection GetInventoryRoleResource (int? currentPage = null, int? pageSize = null, bool? withTotalElements = null)

Retrieve all inventory roles

Retrieve all inventory roles.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetInventoryRoleResourceExample
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

            var apiInstance = new InventoryRolesApi(config);
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve all inventory roles
                InventoryRoleCollection result = apiInstance.GetInventoryRoleResource(currentPage, pageSize, withTotalElements);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling InventoryRolesApi.GetInventoryRoleResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetInventoryRoleResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all inventory roles
    ApiResponse<InventoryRoleCollection> response = apiInstance.GetInventoryRoleResourceWithHttpInfo(currentPage, pageSize, withTotalElements);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling InventoryRolesApi.GetInventoryRoleResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **withTotalElements** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**InventoryRoleCollection**](InventoryRoleCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.inventoryrolecollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request succeeded and all inventory roles are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getinventoryroleresourceid"></a>
# **GetInventoryRoleResourceId**
> InventoryRole GetInventoryRoleResourceId (int id)

Retrieve a specific inventory role

Retrieve a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> has access to the inventory role </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetInventoryRoleResourceIdExample
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

            var apiInstance = new InventoryRolesApi(config);
            var id = 4;  // int | Unique identifier of the inventory role.

            try
            {
                // Retrieve a specific inventory role
                InventoryRole result = apiInstance.GetInventoryRoleResourceId(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling InventoryRolesApi.GetInventoryRoleResourceId: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetInventoryRoleResourceIdWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a specific inventory role
    ApiResponse<InventoryRole> response = apiInstance.GetInventoryRoleResourceIdWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling InventoryRolesApi.GetInventoryRoleResourceIdWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **int** | Unique identifier of the inventory role. |  |

### Return type

[**InventoryRole**](InventoryRole.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.inventoryrole+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request succeeded and the inventory role is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Role not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postinventoryassignmentresource"></a>
# **PostInventoryAssignmentResource**
> InventoryAssignment PostInventoryAssignmentResource (string tenantId, string userId, PostInventoryAssignmentResourceRequest postInventoryAssignmentResourceRequest, string? accept = null)

Assign an inventory role to a user

Assign an existing inventory role to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to assign any inventory role to root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to assign inventory roles accessible by the parent of the assigned user to non-root users in a user hierarchy<br/> ROLE_USER_MANAGEMENT_CREATE to assign inventory roles accessible by the current user <b>AND</b> accessible by the parent of the assigned user to the descendants of the current user in a user hierarchy </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostInventoryAssignmentResourceExample
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

            var apiInstance = new InventoryRolesApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var userId = jdoe;  // string | Unique identifier of the a user.
            var postInventoryAssignmentResourceRequest = new PostInventoryAssignmentResourceRequest(); // PostInventoryAssignmentResourceRequest | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Assign an inventory role to a user
                InventoryAssignment result = apiInstance.PostInventoryAssignmentResource(tenantId, userId, postInventoryAssignmentResourceRequest, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling InventoryRolesApi.PostInventoryAssignmentResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostInventoryAssignmentResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Assign an inventory role to a user
    ApiResponse<InventoryAssignment> response = apiInstance.PostInventoryAssignmentResourceWithHttpInfo(tenantId, userId, postInventoryAssignmentResourceRequest, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling InventoryRolesApi.PostInventoryAssignmentResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **userId** | **string** | Unique identifier of the a user. |  |
| **postInventoryAssignmentResourceRequest** | [**PostInventoryAssignmentResourceRequest**](PostInventoryAssignmentResourceRequest.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**InventoryAssignment**](InventoryAssignment.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.inventoryassignment+json
 - **Accept**: application/vnd.com.nsn.cumulocity.inventoryassignment+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | An inventory role was assigned to a user. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **404** | User not found. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postinventoryroleresource"></a>
# **PostInventoryRoleResource**
> InventoryRole PostInventoryRoleResource (PostInventoryRoleResourceRequest postInventoryRoleResourceRequest, string? accept = null)

Create an inventory role

Create an inventory role.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostInventoryRoleResourceExample
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

            var apiInstance = new InventoryRolesApi(config);
            var postInventoryRoleResourceRequest = new PostInventoryRoleResourceRequest(); // PostInventoryRoleResourceRequest | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Create an inventory role
                InventoryRole result = apiInstance.PostInventoryRoleResource(postInventoryRoleResourceRequest, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling InventoryRolesApi.PostInventoryRoleResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostInventoryRoleResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create an inventory role
    ApiResponse<InventoryRole> response = apiInstance.PostInventoryRoleResourceWithHttpInfo(postInventoryRoleResourceRequest, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling InventoryRolesApi.PostInventoryRoleResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **postInventoryRoleResourceRequest** | [**PostInventoryRoleResourceRequest**](PostInventoryRoleResourceRequest.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**InventoryRole**](InventoryRole.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.inventoryrole+json
 - **Accept**: application/vnd.com.nsn.cumulocity.inventoryrole+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | An inventory role was created. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **409** | Duplicate – The inventory role already exists. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="putinventoryassignmentresourcebyid"></a>
# **PutInventoryAssignmentResourceById**
> InventoryAssignment PutInventoryAssignmentResourceById (string tenantId, string userId, int id, InventoryAssignmentReference inventoryAssignmentReference, string? accept = null)

Update a specific inventory role assigned to a user

Update a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to update the assignment of any inventory roles to root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to update the assignment of inventory roles accessible by the assigned user parent, to non-root users in a user hierarchy<br/> ROLE_USER_MANAGEMENT_CREATE to update the assignment of inventory roles accessible by the current user <b>AND</b> the parent of the assigned user to the descendants of the current user in the user hierarchy </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutInventoryAssignmentResourceByIdExample
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

            var apiInstance = new InventoryRolesApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var userId = jdoe;  // string | Unique identifier of the a user.
            var id = 1;  // int | Unique identifier of the inventory assignment.
            var inventoryAssignmentReference = new InventoryAssignmentReference(); // InventoryAssignmentReference | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Update a specific inventory role assigned to a user
                InventoryAssignment result = apiInstance.PutInventoryAssignmentResourceById(tenantId, userId, id, inventoryAssignmentReference, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling InventoryRolesApi.PutInventoryAssignmentResourceById: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutInventoryAssignmentResourceByIdWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update a specific inventory role assigned to a user
    ApiResponse<InventoryAssignment> response = apiInstance.PutInventoryAssignmentResourceByIdWithHttpInfo(tenantId, userId, id, inventoryAssignmentReference, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling InventoryRolesApi.PutInventoryAssignmentResourceByIdWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **userId** | **string** | Unique identifier of the a user. |  |
| **id** | **int** | Unique identifier of the inventory assignment. |  |
| **inventoryAssignmentReference** | [**InventoryAssignmentReference**](InventoryAssignmentReference.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**InventoryAssignment**](InventoryAssignment.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.inventoryassignment+json
 - **Accept**: application/vnd.com.nsn.cumulocity.inventoryassignment+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | An inventory assignment was updated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **404** | Role not found. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="putinventoryroleresourceid"></a>
# **PutInventoryRoleResourceId**
> InventoryRole PutInventoryRoleResourceId (int id, InventoryRole inventoryRole, string? accept = null)

Update a specific inventory role

Update a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutInventoryRoleResourceIdExample
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

            var apiInstance = new InventoryRolesApi(config);
            var id = 4;  // int | Unique identifier of the inventory role.
            var inventoryRole = new InventoryRole(); // InventoryRole | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Update a specific inventory role
                InventoryRole result = apiInstance.PutInventoryRoleResourceId(id, inventoryRole, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling InventoryRolesApi.PutInventoryRoleResourceId: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutInventoryRoleResourceIdWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update a specific inventory role
    ApiResponse<InventoryRole> response = apiInstance.PutInventoryRoleResourceIdWithHttpInfo(id, inventoryRole, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling InventoryRolesApi.PutInventoryRoleResourceIdWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **int** | Unique identifier of the inventory role. |  |
| **inventoryRole** | [**InventoryRole**](InventoryRole.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**InventoryRole**](InventoryRole.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.inventoryrole+json
 - **Accept**: application/vnd.com.nsn.cumulocity.inventoryrole+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | An inventory role was updated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Role not found. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

