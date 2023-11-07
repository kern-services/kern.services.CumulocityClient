# kern.services.CumulocityClient.Api.UsersApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**DeleteUserReferenceResource**](UsersApi.md#deleteuserreferenceresource) | **DELETE** /user/{tenantId}/groups/{groupId}/users/{userId} | Remove a specific user from a specific user group of a specific tenant |
| [**DeleteUserResource**](UsersApi.md#deleteuserresource) | **DELETE** /user/{tenantId}/users/{userId} | Delete a specific user for a specific tenant |
| [**GetUserCollectionResource**](UsersApi.md#getusercollectionresource) | **GET** /user/{tenantId}/users | Retrieve all users for a specific tenant |
| [**GetUserReferenceCollectionResource**](UsersApi.md#getuserreferencecollectionresource) | **GET** /user/{tenantId}/groups/{groupId}/users | Retrieve the users of a specific user group of a specific tenant |
| [**GetUserResource**](UsersApi.md#getuserresource) | **GET** /user/{tenantId}/users/{userId} | Retrieve a specific user for a specific tenant |
| [**GetUsersByNameResource**](UsersApi.md#getusersbynameresource) | **GET** /user/{tenantId}/userByName/{username} | Retrieve a user by username in a specific tenant |
| [**GetUsersTfaResource**](UsersApi.md#getuserstfaresource) | **GET** /user/{tenantId}/users/{userId}/tfa | Retrieve the TFA settings of a specific user |
| [**PostLogoutUser**](UsersApi.md#postlogoutuser) | **POST** /user/logout | Terminate a user&#39;s session |
| [**PostUserCollectionResource**](UsersApi.md#postusercollectionresource) | **POST** /user/{tenantId}/users | Create a user for a specific tenant |
| [**PostUserReferenceCollectionResource**](UsersApi.md#postuserreferencecollectionresource) | **POST** /user/{tenantId}/groups/{groupId}/users | Add a user to a specific user group of a specific tenant |
| [**PutUserChangePasswordResource**](UsersApi.md#putuserchangepasswordresource) | **PUT** /user/{tenantId}/users/{userId}/password | Update a specific user&#39;s password of a specific tenant |
| [**PutUserResource**](UsersApi.md#putuserresource) | **PUT** /user/{tenantId}/users/{userId} | Update a specific user for a specific tenant |

<a name="deleteuserreferenceresource"></a>
# **DeleteUserReferenceResource**
> void DeleteUserReferenceResource (string tenantId, int groupId, string userId)

Remove a specific user from a specific user group of a specific tenant

Remove a specific user (by a given user ID) from a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is parent of the user <b>AND</b> is not the current user </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteUserReferenceResourceExample
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

            var apiInstance = new UsersApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var groupId = 2;  // int | Unique identifier of the user group.
            var userId = jdoe;  // string | Unique identifier of the a user.

            try
            {
                // Remove a specific user from a specific user group of a specific tenant
                apiInstance.DeleteUserReferenceResource(tenantId, groupId, userId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UsersApi.DeleteUserReferenceResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteUserReferenceResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove a specific user from a specific user group of a specific tenant
    apiInstance.DeleteUserReferenceResourceWithHttpInfo(tenantId, groupId, userId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UsersApi.DeleteUserReferenceResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **groupId** | **int** | Unique identifier of the user group. |  |
| **userId** | **string** | Unique identifier of the a user. |  |

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
| **204** | A user was removed from a group. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **404** | User not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deleteuserresource"></a>
# **DeleteUserResource**
> void DeleteUserResource (string tenantId, string userId)

Delete a specific user for a specific tenant

Delete a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is parent of the user <b>AND</b> not the current user </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteUserResourceExample
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

            var apiInstance = new UsersApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var userId = jdoe;  // string | Unique identifier of the a user.

            try
            {
                // Delete a specific user for a specific tenant
                apiInstance.DeleteUserResource(tenantId, userId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UsersApi.DeleteUserResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteUserResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete a specific user for a specific tenant
    apiInstance.DeleteUserResourceWithHttpInfo(tenantId, userId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UsersApi.DeleteUserResourceWithHttpInfo: " + e.Message);
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

void (empty response body)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **204** | A user was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **404** | User not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getusercollectionresource"></a>
# **GetUserCollectionResource**
> UserCollection GetUserCollectionResource (string tenantId, int? currentPage = null, List<string>? groups = null, bool? onlyDevices = null, string? owner = null, int? pageSize = null, string? username = null, bool? withSubusersCount = null, bool? withTotalElements = null, bool? withTotalPages = null)

Retrieve all users for a specific tenant

Retrieve all users for a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetUserCollectionResourceExample
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

            var apiInstance = new UsersApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var groups = new List<string>?(); // List<string>? | Numeric group identifiers. The response will contain only users which belong to at least one of the specified groups. >**&#9432; Info:** If you query for multiple user groups at once, comma-separate the values.  (optional) 
            var onlyDevices = true;  // bool? | If set to `true`, the response will only contain users created during bootstrap process (starting with “device_”). If the flag is absent or `false` the result will not contain “device_” users.  (optional)  (default to false)
            var owner = admin;  // string? | Exact username of the owner of the user (optional) 
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var username = jdoe;  // string? | Prefix or full username (optional) 
            var withSubusersCount = true;  // bool? | If set to `true`, then each of returned user will contain an additional field “subusersCount”. It is the number of direct subusers (users with corresponding “owner”).  (optional)  (default to false)
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve all users for a specific tenant
                UserCollection result = apiInstance.GetUserCollectionResource(tenantId, currentPage, groups, onlyDevices, owner, pageSize, username, withSubusersCount, withTotalElements, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UsersApi.GetUserCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetUserCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all users for a specific tenant
    ApiResponse<UserCollection> response = apiInstance.GetUserCollectionResourceWithHttpInfo(tenantId, currentPage, groups, onlyDevices, owner, pageSize, username, withSubusersCount, withTotalElements, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UsersApi.GetUserCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **groups** | [**List&lt;string&gt;?**](string.md) | Numeric group identifiers. The response will contain only users which belong to at least one of the specified groups. &gt;**&amp;#9432; Info:** If you query for multiple user groups at once, comma-separate the values.  | [optional]  |
| **onlyDevices** | **bool?** | If set to &#x60;true&#x60;, the response will only contain users created during bootstrap process (starting with “device_”). If the flag is absent or &#x60;false&#x60; the result will not contain “device_” users.  | [optional] [default to false] |
| **owner** | **string?** | Exact username of the owner of the user | [optional]  |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **username** | **string?** | Prefix or full username | [optional]  |
| **withSubusersCount** | **bool?** | If set to &#x60;true&#x60;, then each of returned user will contain an additional field “subusersCount”. It is the number of direct subusers (users with corresponding “owner”).  | [optional] [default to false] |
| **withTotalElements** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**UserCollection**](UserCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.usercollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and all users are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getuserreferencecollectionresource"></a>
# **GetUserReferenceCollectionResource**
> UserReferenceCollection GetUserReferenceCollectionResource (string tenantId, int groupId, int? currentPage = null, int? pageSize = null, bool? withTotalElements = null)

Retrieve the users of a specific user group of a specific tenant

Retrieve the users of a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> (ROLE_USER_MANAGEMENT_CREATE <b>AND</b> has access to the user group) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetUserReferenceCollectionResourceExample
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

            var apiInstance = new UsersApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var groupId = 2;  // int | Unique identifier of the user group.
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve the users of a specific user group of a specific tenant
                UserReferenceCollection result = apiInstance.GetUserReferenceCollectionResource(tenantId, groupId, currentPage, pageSize, withTotalElements);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UsersApi.GetUserReferenceCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetUserReferenceCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve the users of a specific user group of a specific tenant
    ApiResponse<UserReferenceCollection> response = apiInstance.GetUserReferenceCollectionResourceWithHttpInfo(tenantId, groupId, currentPage, pageSize, withTotalElements);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UsersApi.GetUserReferenceCollectionResourceWithHttpInfo: " + e.Message);
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
| **withTotalElements** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**UserReferenceCollection**](UserReferenceCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.userreferencecollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the users are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **404** | Group not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getuserresource"></a>
# **GetUserResource**
> User GetUserResource (string tenantId, string userId)

Retrieve a specific user for a specific tenant

Retrieve a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  Users in the response are sorted by username in ascending order. Only objects which the user is allowed to see are returned to the user. The user password is never returned in a GET response. Authentication mechanism is provided by another interface.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is parent of the user </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetUserResourceExample
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

            var apiInstance = new UsersApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var userId = jdoe;  // string | Unique identifier of the a user.

            try
            {
                // Retrieve a specific user for a specific tenant
                User result = apiInstance.GetUserResource(tenantId, userId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UsersApi.GetUserResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetUserResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a specific user for a specific tenant
    ApiResponse<User> response = apiInstance.GetUserResourceWithHttpInfo(tenantId, userId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UsersApi.GetUserResourceWithHttpInfo: " + e.Message);
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

[**User**](User.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.user+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the user is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **404** | User not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getusersbynameresource"></a>
# **GetUsersByNameResource**
> User GetUsersByNameResource (string tenantId, string username)

Retrieve a user by username in a specific tenant

Retrieve a user by username in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is parent of the user </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetUsersByNameResourceExample
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

            var apiInstance = new UsersApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var username = jdoe;  // string | The username of the a user.

            try
            {
                // Retrieve a user by username in a specific tenant
                User result = apiInstance.GetUsersByNameResource(tenantId, username);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UsersApi.GetUsersByNameResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetUsersByNameResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a user by username in a specific tenant
    ApiResponse<User> response = apiInstance.GetUsersByNameResourceWithHttpInfo(tenantId, username);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UsersApi.GetUsersByNameResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **username** | **string** | The username of the a user. |  |

### Return type

[**User**](User.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.user+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the user is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **404** | User not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getuserstfaresource"></a>
# **GetUsersTfaResource**
> UserTfaData GetUsersTfaResource (string tenantId, string userId)

Retrieve the TFA settings of a specific user

Retrieve the two-factor authentication settings for the specified user.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> (ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is parent of the user) <b>OR</b> is the current user </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetUsersTfaResourceExample
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

            var apiInstance = new UsersApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var userId = jdoe;  // string | Unique identifier of the a user.

            try
            {
                // Retrieve the TFA settings of a specific user
                UserTfaData result = apiInstance.GetUsersTfaResource(tenantId, userId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UsersApi.GetUsersTfaResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetUsersTfaResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve the TFA settings of a specific user
    ApiResponse<UserTfaData> response = apiInstance.GetUsersTfaResourceWithHttpInfo(tenantId, userId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UsersApi.GetUsersTfaResourceWithHttpInfo: " + e.Message);
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

[**UserTfaData**](UserTfaData.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the TFA settings are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **404** | User not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postlogoutuser"></a>
# **PostLogoutUser**
> void PostLogoutUser (string? cookie = null, string? X_XSRF_TOKEN = null)

Terminate a user's session

After logging out, a user has to enter valid credentials again to get access to the platform.  The request is responsible for removing cookies from the browser and invalidating internal platform access tokens. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostLogoutUserExample
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

            var apiInstance = new UsersApi(config);
            var cookie = authorization=<ACCESS_TOKEN>;  // string? | The authorization cookie storing the access token of the user. This parameter is specific to OAI-Secure authentication. (optional) 
            var X_XSRF_TOKEN = <X-XSRF-TOKEN>;  // string? | Prevents XRSF attack of the authenticated user. This parameter is specific to OAI-Secure authentication. (optional) 

            try
            {
                // Terminate a user's session
                apiInstance.PostLogoutUser(cookie, X_XSRF_TOKEN);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UsersApi.PostLogoutUser: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostLogoutUserWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Terminate a user's session
    apiInstance.PostLogoutUserWithHttpInfo(cookie, X_XSRF_TOKEN);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UsersApi.PostLogoutUserWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **cookie** | **string?** | The authorization cookie storing the access token of the user. This parameter is specific to OAI-Secure authentication. | [optional]  |
| **X_XSRF_TOKEN** | **string?** | Prevents XRSF attack of the authenticated user. This parameter is specific to OAI-Secure authentication. | [optional]  |

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
| **200** | The request has succeeded and the user is logged out. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postusercollectionresource"></a>
# **PostUserCollectionResource**
> User PostUserCollectionResource (string tenantId, PostUserCollectionResourceRequest postUserCollectionResourceRequest, string? accept = null)

Create a user for a specific tenant

Create a user for a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> has access to roles, groups, device permissions and applications </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostUserCollectionResourceExample
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

            var apiInstance = new UsersApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var postUserCollectionResourceRequest = new PostUserCollectionResourceRequest(); // PostUserCollectionResourceRequest | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Create a user for a specific tenant
                User result = apiInstance.PostUserCollectionResource(tenantId, postUserCollectionResourceRequest, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UsersApi.PostUserCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostUserCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create a user for a specific tenant
    ApiResponse<User> response = apiInstance.PostUserCollectionResourceWithHttpInfo(tenantId, postUserCollectionResourceRequest, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UsersApi.PostUserCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **postUserCollectionResourceRequest** | [**PostUserCollectionResourceRequest**](PostUserCollectionResourceRequest.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**User**](User.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.user+json
 - **Accept**: application/vnd.com.nsn.cumulocity.user+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | A user was created. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **409** | Duplicate – The userName or alias already exists. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postuserreferencecollectionresource"></a>
# **PostUserReferenceCollectionResource**
> UserReference PostUserReferenceCollectionResource (string tenantId, int groupId, SubscribedUser subscribedUser, string? accept = null)

Add a user to a specific user group of a specific tenant

Add a user to a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to assign root users in a user hierarchy <b>OR</b> users that are not in any hierarchy to any group<br/> ROLE_USER_MANAGEMENT_ADMIN to assign non-root users in a user hierarchy to groups accessible by the parent of assigned user<br/> ROLE_USER_MANAGEMENT_CREATE to assign descendants of the current user in a user hierarchy to groups accessible by current user <b>AND</b> accessible by the parent of assigned user </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostUserReferenceCollectionResourceExample
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

            var apiInstance = new UsersApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var groupId = 2;  // int | Unique identifier of the user group.
            var subscribedUser = new SubscribedUser(); // SubscribedUser | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Add a user to a specific user group of a specific tenant
                UserReference result = apiInstance.PostUserReferenceCollectionResource(tenantId, groupId, subscribedUser, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UsersApi.PostUserReferenceCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostUserReferenceCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Add a user to a specific user group of a specific tenant
    ApiResponse<UserReference> response = apiInstance.PostUserReferenceCollectionResourceWithHttpInfo(tenantId, groupId, subscribedUser, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UsersApi.PostUserReferenceCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **groupId** | **int** | Unique identifier of the user group. |  |
| **subscribedUser** | [**SubscribedUser**](SubscribedUser.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**UserReference**](UserReference.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.userreference+json
 - **Accept**: application/vnd.com.nsn.cumulocity.userreference+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | The user was added to the group. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **404** | Group not found. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="putuserchangepasswordresource"></a>
# **PutUserChangePasswordResource**
> void PutUserChangePasswordResource (string tenantId, string userId, PasswordChange passwordChange, string? accept = null)

Update a specific user's password of a specific tenant

Update a specific user's password (by a given user ID) of a specific tenant (by a given tenant ID).  Changing the user's password creates a corresponding audit record of type \"User\" and activity \"User updated\", and specifying that the password has been changed.  > **⚠️ Important:** If the tenant uses OAI-Secure authentication, the target user will be logged out.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to update root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to update non-root users in a user hierarchy <b>AND</b> whose parents have access to assigned roles, groups, device permissions and applications<br/> ROLE_USER_MANAGEMENT_CREATE to update descendants of the current user in a user hierarchy <b>AND</b> whose parents have access to assigned roles, groups, device permissions and applications </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutUserChangePasswordResourceExample
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

            var apiInstance = new UsersApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var userId = jdoe;  // string | Unique identifier of the a user.
            var passwordChange = new PasswordChange(); // PasswordChange | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Update a specific user's password of a specific tenant
                apiInstance.PutUserChangePasswordResource(tenantId, userId, passwordChange, accept);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UsersApi.PutUserChangePasswordResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutUserChangePasswordResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update a specific user's password of a specific tenant
    apiInstance.PutUserChangePasswordResourceWithHttpInfo(tenantId, userId, passwordChange, accept);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UsersApi.PutUserChangePasswordResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **userId** | **string** | Unique identifier of the a user. |  |
| **passwordChange** | [**PasswordChange**](PasswordChange.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

void (empty response body)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A user was updated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="putuserresource"></a>
# **PutUserResource**
> User PutUserResource (string tenantId, string userId, PutUserResourceRequest putUserResourceRequest, string? accept = null)

Update a specific user for a specific tenant

Update a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  Any change in user's roles, device permissions and groups creates corresponding audit records with type \"User\" and activity \"User updated\" with information which properties have been changed.  When the user is updated with changed permissions or groups, a corresponding audit record is created with type \"User\" and activity \"User updated\".  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to update root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to update non-root users in a user hierarchy <b>AND</b> whose parents have access to roles, groups, device permissions and applications being assigned<br/> ROLE_USER_MANAGEMENT_CREATE to update descendants of the current user in a user hierarchy <b>AND</b> whose parents have access to roles, groups, device permissions and applications being assigned </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutUserResourceExample
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

            var apiInstance = new UsersApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var userId = jdoe;  // string | Unique identifier of the a user.
            var putUserResourceRequest = new PutUserResourceRequest(); // PutUserResourceRequest | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Update a specific user for a specific tenant
                User result = apiInstance.PutUserResource(tenantId, userId, putUserResourceRequest, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling UsersApi.PutUserResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutUserResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update a specific user for a specific tenant
    ApiResponse<User> response = apiInstance.PutUserResourceWithHttpInfo(tenantId, userId, putUserResourceRequest, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling UsersApi.PutUserResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **userId** | **string** | Unique identifier of the a user. |  |
| **putUserResourceRequest** | [**PutUserResourceRequest**](PutUserResourceRequest.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**User**](User.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.user+json
 - **Accept**: application/vnd.com.nsn.cumulocity.user+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A user was updated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **404** | User not found. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

