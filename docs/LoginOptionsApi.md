# kern.services.CumulocityClient.Api.LoginOptionsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetLoginOptionCollectionResource**](LoginOptionsApi.md#getloginoptioncollectionresource) | **GET** /tenant/loginOptions | Retrieve the login options |
| [**PostLoginOptionCollectionResource**](LoginOptionsApi.md#postloginoptioncollectionresource) | **POST** /tenant/loginOptions | Create a login option |
| [**PutAccessLoginOptionResource**](LoginOptionsApi.md#putaccessloginoptionresource) | **PUT** /tenant/loginOptions/{type_or_id}/restrict | Update a tenant&#39;s access to the login option |

<a id="getloginoptioncollectionresource"></a>
# **GetLoginOptionCollectionResource**
> LoginOptionCollection GetLoginOptionCollectionResource (bool? management = null, string? tenantId = null)

Retrieve the login options

Retrieve the login options available in the tenant.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetLoginOptionCollectionResourceExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://<TENANT_DOMAIN>";
            var apiInstance = new LoginOptionsApi(config);
            var management = true;  // bool? | If this is set to `true`, the management tenant login options will be returned.  > **&#9432; Info:** The `tenantId` parameter must not be present in the request when using the `management` parameter, otherwise it will cause an error.  (optional)  (default to false)
            var tenantId = t07007007;  // string? | Unique identifier of a Cumulocity IoT tenant. (optional) 

            try
            {
                // Retrieve the login options
                LoginOptionCollection result = apiInstance.GetLoginOptionCollectionResource(management, tenantId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling LoginOptionsApi.GetLoginOptionCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetLoginOptionCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve the login options
    ApiResponse<LoginOptionCollection> response = apiInstance.GetLoginOptionCollectionResourceWithHttpInfo(management, tenantId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling LoginOptionsApi.GetLoginOptionCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **management** | **bool?** | If this is set to &#x60;true&#x60;, the management tenant login options will be returned.  &gt; **&amp;#9432; Info:** The &#x60;tenantId&#x60; parameter must not be present in the request when using the &#x60;management&#x60; parameter, otherwise it will cause an error.  | [optional] [default to false] |
| **tenantId** | **string?** | Unique identifier of a Cumulocity IoT tenant. | [optional]  |

### Return type

[**LoginOptionCollection**](LoginOptionCollection.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.loginoptioncollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the login options are sent in the response. |  -  |
| **400** | Bad request – invalid parameters. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="postloginoptioncollectionresource"></a>
# **PostLoginOptionCollectionResource**
> AuthConfig PostLoginOptionCollectionResource (AuthConfig authConfig, string? accept = null)

Create a login option

Create an authentication configuration on your tenant.  <section><h5>Required roles</h5> ROLE_TENANT_ADMIN <b>OR</b> ROLE_TENANT_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostLoginOptionCollectionResourceExample
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

            var apiInstance = new LoginOptionsApi(config);
            var authConfig = new AuthConfig(); // AuthConfig | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Create a login option
                AuthConfig result = apiInstance.PostLoginOptionCollectionResource(authConfig, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling LoginOptionsApi.PostLoginOptionCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostLoginOptionCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create a login option
    ApiResponse<AuthConfig> response = apiInstance.PostLoginOptionCollectionResourceWithHttpInfo(authConfig, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling LoginOptionsApi.PostLoginOptionCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **authConfig** | [**AuthConfig**](AuthConfig.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**AuthConfig**](AuthConfig.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.authconfig+json
 - **Accept**: application/vnd.com.nsn.cumulocity.authconfig+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A login option was created. |  -  |
| **400** | Duplicated – The login option already exists. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="putaccessloginoptionresource"></a>
# **PutAccessLoginOptionResource**
> AuthConfig PutAccessLoginOptionResource (string typeOrId, string targetTenant, AuthConfigAccess authConfigAccess, string? accept = null)

Update a tenant's access to the login option

Update the tenant's access to the authentication configuration.  <section><h5>Required roles</h5> ROLE_TENANT_MANAGEMENT_ADMIN <b>AND</b> is the management tenant </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutAccessLoginOptionResourceExample
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

            var apiInstance = new LoginOptionsApi(config);
            var typeOrId = OAUTH2;  // string | The type or ID of the login option. The type's value is case insensitive and can be `OAUTH2`, `OAUTH2_INTERNAL` or `BASIC`.
            var targetTenant = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var authConfigAccess = new AuthConfigAccess(); // AuthConfigAccess | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Update a tenant's access to the login option
                AuthConfig result = apiInstance.PutAccessLoginOptionResource(typeOrId, targetTenant, authConfigAccess, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling LoginOptionsApi.PutAccessLoginOptionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutAccessLoginOptionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update a tenant's access to the login option
    ApiResponse<AuthConfig> response = apiInstance.PutAccessLoginOptionResourceWithHttpInfo(typeOrId, targetTenant, authConfigAccess, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling LoginOptionsApi.PutAccessLoginOptionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **typeOrId** | **string** | The type or ID of the login option. The type&#39;s value is case insensitive and can be &#x60;OAUTH2&#x60;, &#x60;OAUTH2_INTERNAL&#x60; or &#x60;BASIC&#x60;. |  |
| **targetTenant** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **authConfigAccess** | [**AuthConfigAccess**](AuthConfigAccess.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**AuthConfig**](AuthConfig.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/vnd.com.nsn.cumulocity.authconfig+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The login option was updated. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **404** | Tenant not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

