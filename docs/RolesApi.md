# kern.services.CumulocityClient.Api.RolesApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**DeleteGroupRoleReferenceResource**](RolesApi.md#deletegrouprolereferenceresource) | **DELETE** /user/{tenantId}/groups/{groupId}/roles/{roleId} | Unassign a specific role for a specific user group in a specific tenant |
| [**DeleteUserRoleReferenceResource**](RolesApi.md#deleteuserrolereferenceresource) | **DELETE** /user/{tenantId}/users/{userId}/roles/{roleId} | Unassign a specific role from a specific user in a specific tenant |
| [**GetGroupsRoleReferenceCollectionResource**](RolesApi.md#getgroupsrolereferencecollectionresource) | **GET** /user/{tenantId}/groups/{groupId}/roles | Retrieve all roles assigned to a specific user group in a specific tenant |
| [**GetRoleCollectionResource**](RolesApi.md#getrolecollectionresource) | **GET** /user/roles | Retrieve all user roles |
| [**GetRoleCollectionResourceByName**](RolesApi.md#getrolecollectionresourcebyname) | **GET** /user/roles/{name} | Retrieve a user role by name |
| [**PostGroupsRoleReferenceCollectionResource**](RolesApi.md#postgroupsrolereferencecollectionresource) | **POST** /user/{tenantId}/groups/{groupId}/roles | Assign a role to a specific user group in a specific tenant |
| [**PostUsersRoleReferenceCollectionResource**](RolesApi.md#postusersrolereferencecollectionresource) | **POST** /user/{tenantId}/users/{userId}/roles | Assign a role to specific user in a specific tenant |

<a name="deletegrouprolereferenceresource"></a>
# **DeleteGroupRoleReferenceResource**
> void DeleteGroupRoleReferenceResource (string tenantId, int groupId, string roleId)

Unassign a specific role for a specific user group in a specific tenant

Unassign a specific role (given by a role ID) for a specific user group (by a given user group ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteGroupRoleReferenceResourceExample
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

            var apiInstance = new RolesApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var groupId = 2;  // int | Unique identifier of the user group.
            var roleId = ROLE_ALARM_ADMIN;  // string | Unique identifier of the user role.

            try
            {
                // Unassign a specific role for a specific user group in a specific tenant
                apiInstance.DeleteGroupRoleReferenceResource(tenantId, groupId, roleId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling RolesApi.DeleteGroupRoleReferenceResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteGroupRoleReferenceResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Unassign a specific role for a specific user group in a specific tenant
    apiInstance.DeleteGroupRoleReferenceResourceWithHttpInfo(tenantId, groupId, roleId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling RolesApi.DeleteGroupRoleReferenceResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **groupId** | **int** | Unique identifier of the user group. |  |
| **roleId** | **string** | Unique identifier of the user role. |  |

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
| **204** | A role was unassigned from a user group. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **404** | Role not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deleteuserrolereferenceresource"></a>
# **DeleteUserRoleReferenceResource**
> void DeleteUserRoleReferenceResource (string tenantId, string userId, string roleId)

Unassign a specific role from a specific user in a specific tenant

Unassign a specific role (by a given role ID) from a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is parent of the user <b>AND</b> has access to roles </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteUserRoleReferenceResourceExample
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

            var apiInstance = new RolesApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var userId = jdoe;  // string | Unique identifier of the a user.
            var roleId = ROLE_ALARM_ADMIN;  // string | Unique identifier of the user role.

            try
            {
                // Unassign a specific role from a specific user in a specific tenant
                apiInstance.DeleteUserRoleReferenceResource(tenantId, userId, roleId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling RolesApi.DeleteUserRoleReferenceResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteUserRoleReferenceResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Unassign a specific role from a specific user in a specific tenant
    apiInstance.DeleteUserRoleReferenceResourceWithHttpInfo(tenantId, userId, roleId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling RolesApi.DeleteUserRoleReferenceResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **userId** | **string** | Unique identifier of the a user. |  |
| **roleId** | **string** | Unique identifier of the user role. |  |

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
| **204** | A user role was unassigned from a user. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **404** | User not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getgroupsrolereferencecollectionresource"></a>
# **GetGroupsRoleReferenceCollectionResource**
> RoleReferenceCollection GetGroupsRoleReferenceCollectionResource (string tenantId, int groupId, int? currentPage = null, int? pageSize = null)

Retrieve all roles assigned to a specific user group in a specific tenant

Retrieve all roles assigned to a specific user group (by a given user group ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetGroupsRoleReferenceCollectionResourceExample
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

            var apiInstance = new RolesApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var groupId = 2;  // int | Unique identifier of the user group.
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)

            try
            {
                // Retrieve all roles assigned to a specific user group in a specific tenant
                RoleReferenceCollection result = apiInstance.GetGroupsRoleReferenceCollectionResource(tenantId, groupId, currentPage, pageSize);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling RolesApi.GetGroupsRoleReferenceCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetGroupsRoleReferenceCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all roles assigned to a specific user group in a specific tenant
    ApiResponse<RoleReferenceCollection> response = apiInstance.GetGroupsRoleReferenceCollectionResourceWithHttpInfo(tenantId, groupId, currentPage, pageSize);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling RolesApi.GetGroupsRoleReferenceCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **groupId** | **int** | Unique identifier of the user group. |  |
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |

### Return type

[**RoleReferenceCollection**](RoleReferenceCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.rolereferencecollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request succeeded and the roles are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **404** | Group not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getrolecollectionresource"></a>
# **GetRoleCollectionResource**
> UserRoleCollection GetRoleCollectionResource (int? currentPage = null, int? pageSize = null, bool? withTotalElements = null, bool? withTotalPages = null)

Retrieve all user roles

Retrieve all user roles.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> has access to the user role </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetRoleCollectionResourceExample
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

            var apiInstance = new RolesApi(config);
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve all user roles
                UserRoleCollection result = apiInstance.GetRoleCollectionResource(currentPage, pageSize, withTotalElements, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling RolesApi.GetRoleCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetRoleCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all user roles
    ApiResponse<UserRoleCollection> response = apiInstance.GetRoleCollectionResourceWithHttpInfo(currentPage, pageSize, withTotalElements, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling RolesApi.GetRoleCollectionResourceWithHttpInfo: " + e.Message);
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
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**UserRoleCollection**](UserRoleCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.rolecollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and all user roles are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getrolecollectionresourcebyname"></a>
# **GetRoleCollectionResourceByName**
> Role GetRoleCollectionResourceByName (string name)

Retrieve a user role by name

Retrieve a user role by name.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> current user has access to the role with this name </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetRoleCollectionResourceByNameExample
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

            var apiInstance = new RolesApi(config);
            var name = ROLE_ALARM_ADMIN;  // string | The name of the user role.

            try
            {
                // Retrieve a user role by name
                Role result = apiInstance.GetRoleCollectionResourceByName(name);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling RolesApi.GetRoleCollectionResourceByName: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetRoleCollectionResourceByNameWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a user role by name
    ApiResponse<Role> response = apiInstance.GetRoleCollectionResourceByNameWithHttpInfo(name);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling RolesApi.GetRoleCollectionResourceByNameWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **name** | **string** | The name of the user role. |  |

### Return type

[**Role**](Role.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.role+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the user role is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Role not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postgroupsrolereferencecollectionresource"></a>
# **PostGroupsRoleReferenceCollectionResource**
> RoleReference PostGroupsRoleReferenceCollectionResource (string tenantId, int groupId, SubscribedRole subscribedRole, string? accept = null)

Assign a role to a specific user group in a specific tenant

Assign a role to a specific user group (by a given user group ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostGroupsRoleReferenceCollectionResourceExample
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

            var apiInstance = new RolesApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var groupId = 2;  // int | Unique identifier of the user group.
            var subscribedRole = new SubscribedRole(); // SubscribedRole | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Assign a role to a specific user group in a specific tenant
                RoleReference result = apiInstance.PostGroupsRoleReferenceCollectionResource(tenantId, groupId, subscribedRole, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling RolesApi.PostGroupsRoleReferenceCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostGroupsRoleReferenceCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Assign a role to a specific user group in a specific tenant
    ApiResponse<RoleReference> response = apiInstance.PostGroupsRoleReferenceCollectionResourceWithHttpInfo(tenantId, groupId, subscribedRole, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling RolesApi.PostGroupsRoleReferenceCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **groupId** | **int** | Unique identifier of the user group. |  |
| **subscribedRole** | [**SubscribedRole**](SubscribedRole.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**RoleReference**](RoleReference.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.rolereference+json
 - **Accept**: application/vnd.com.nsn.cumulocity.rolereference+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | A user role was assigned to a user group. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **404** | Group not found. |  -  |
| **409** | Conflict – Role already assigned to the user group. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postusersrolereferencecollectionresource"></a>
# **PostUsersRoleReferenceCollectionResource**
> RoleReference PostUsersRoleReferenceCollectionResource (string tenantId, string userId, SubscribedRole subscribedRole, string? accept = null)

Assign a role to specific user in a specific tenant

Assign a role to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  When a role is assigned to a user, a corresponding audit record is created with type \"User\" and activity \"User updated\".  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to assign any role to root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to assign roles accessible by the parent of assigned user to non-root users in a user hierarchy<br/> ROLE_USER_MANAGEMENT_CREATE to assign roles accessible by the current user <b>AND</b> accessible by the parent of the assigned user to the descendants of the current user in a user hierarchy </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostUsersRoleReferenceCollectionResourceExample
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

            var apiInstance = new RolesApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var userId = jdoe;  // string | Unique identifier of the a user.
            var subscribedRole = new SubscribedRole(); // SubscribedRole | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Assign a role to specific user in a specific tenant
                RoleReference result = apiInstance.PostUsersRoleReferenceCollectionResource(tenantId, userId, subscribedRole, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling RolesApi.PostUsersRoleReferenceCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostUsersRoleReferenceCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Assign a role to specific user in a specific tenant
    ApiResponse<RoleReference> response = apiInstance.PostUsersRoleReferenceCollectionResourceWithHttpInfo(tenantId, userId, subscribedRole, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling RolesApi.PostUsersRoleReferenceCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **userId** | **string** | Unique identifier of the a user. |  |
| **subscribedRole** | [**SubscribedRole**](SubscribedRole.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**RoleReference**](RoleReference.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.rolereference+json
 - **Accept**: application/vnd.com.nsn.cumulocity.rolereference+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | A user role was assigned to a user. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **404** | User not found. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

