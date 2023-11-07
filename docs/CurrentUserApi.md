# kern.services.CumulocityClient.Api.CurrentUserApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetCurrentUserResource**](CurrentUserApi.md#getcurrentuserresource) | **GET** /user/currentUser | Retrieve the current user |
| [**GetCurrentUserTfaTotpResourceActivity**](CurrentUserApi.md#getcurrentusertfatotpresourceactivity) | **GET** /user/currentUser/totpSecret/activity | Returns the activation state of the two-factor authentication feature. |
| [**PostCurrentUserTfaTotpResource**](CurrentUserApi.md#postcurrentusertfatotpresource) | **POST** /user/currentUser/totpSecret | Generate secret to set up TFA |
| [**PostCurrentUserTfaTotpResourceActivity**](CurrentUserApi.md#postcurrentusertfatotpresourceactivity) | **POST** /user/currentUser/totpSecret/activity | Activates or deactivates the two-factor authentication feature |
| [**PostCurrentUserTfaTotpResourceVerify**](CurrentUserApi.md#postcurrentusertfatotpresourceverify) | **POST** /user/currentUser/totpSecret/verify | Verify TFA code |
| [**PutCurrentUserPasswordResource**](CurrentUserApi.md#putcurrentuserpasswordresource) | **PUT** /user/currentUser/password | Update the current user&#39;s password |
| [**PutCurrentUserResource**](CurrentUserApi.md#putcurrentuserresource) | **PUT** /user/currentUser | Update the current user |

<a name="getcurrentuserresource"></a>
# **GetCurrentUserResource**
> CurrentUser GetCurrentUserResource ()

Retrieve the current user

Retrieve the user reference of the current user.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_OWN_READ <b>OR</b> ROLE_SYSTEM </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetCurrentUserResourceExample
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

            var apiInstance = new CurrentUserApi(config);

            try
            {
                // Retrieve the current user
                CurrentUser result = apiInstance.GetCurrentUserResource();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CurrentUserApi.GetCurrentUserResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetCurrentUserResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve the current user
    ApiResponse<CurrentUser> response = apiInstance.GetCurrentUserResourceWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CurrentUserApi.GetCurrentUserResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**CurrentUser**](CurrentUser.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.currentuser+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the current user is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcurrentusertfatotpresourceactivity"></a>
# **GetCurrentUserTfaTotpResourceActivity**
> CurrentUserTotpSecretActivity GetCurrentUserTfaTotpResourceActivity ()

Returns the activation state of the two-factor authentication feature.

Returns the activation state of the two-factor authentication feature for the current user.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_OWN_READ <b>OR</b> ROLE_SYSTEM </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetCurrentUserTfaTotpResourceActivityExample
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

            var apiInstance = new CurrentUserApi(config);

            try
            {
                // Returns the activation state of the two-factor authentication feature.
                CurrentUserTotpSecretActivity result = apiInstance.GetCurrentUserTfaTotpResourceActivity();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CurrentUserApi.GetCurrentUserTfaTotpResourceActivity: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetCurrentUserTfaTotpResourceActivityWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Returns the activation state of the two-factor authentication feature.
    ApiResponse<CurrentUserTotpSecretActivity> response = apiInstance.GetCurrentUserTfaTotpResourceActivityWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CurrentUserApi.GetCurrentUserTfaTotpResourceActivityWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**CurrentUserTotpSecretActivity**](CurrentUserTotpSecretActivity.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Returns the activation state. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | User not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postcurrentusertfatotpresource"></a>
# **PostCurrentUserTfaTotpResource**
> CurrentUserTotpSecret PostCurrentUserTfaTotpResource (string? accept = null)

Generate secret to set up TFA

Generate a secret code to create a QR code to set up the two-factor authentication functionality using a TFA app/service.  For more information about the feature, see [User Guide > Administration > Two-factor authentication](https://cumulocity.com/guides/users-guide/administration/#tfa) in the *Cumulocity IoT documentation*.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_OWN_READ <b>OR</b> ROLE_SYSTEM </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostCurrentUserTfaTotpResourceExample
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

            var apiInstance = new CurrentUserApi(config);
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Generate secret to set up TFA
                CurrentUserTotpSecret result = apiInstance.PostCurrentUserTfaTotpResource(accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CurrentUserApi.PostCurrentUserTfaTotpResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostCurrentUserTfaTotpResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Generate secret to set up TFA
    ApiResponse<CurrentUserTotpSecret> response = apiInstance.PostCurrentUserTfaTotpResourceWithHttpInfo(accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CurrentUserApi.PostCurrentUserTfaTotpResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**CurrentUserTotpSecret**](CurrentUserTotpSecret.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the secret is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postcurrentusertfatotpresourceactivity"></a>
# **PostCurrentUserTfaTotpResourceActivity**
> void PostCurrentUserTfaTotpResourceActivity (CurrentUserTotpSecretActivity currentUserTotpSecretActivity)

Activates or deactivates the two-factor authentication feature

Activates or deactivates the two-factor authentication feature for the current user.  For more information about the feature, see [User Guide > Administration > Two-factor authentication](https://cumulocity.com/guides/users-guide/administration/#tfa) in the *Cumulocity IoT documentation*.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_OWN_READ <b>OR</b> ROLE_SYSTEM </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostCurrentUserTfaTotpResourceActivityExample
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

            var apiInstance = new CurrentUserApi(config);
            var currentUserTotpSecretActivity = new CurrentUserTotpSecretActivity(); // CurrentUserTotpSecretActivity | 

            try
            {
                // Activates or deactivates the two-factor authentication feature
                apiInstance.PostCurrentUserTfaTotpResourceActivity(currentUserTotpSecretActivity);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CurrentUserApi.PostCurrentUserTfaTotpResourceActivity: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostCurrentUserTfaTotpResourceActivityWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Activates or deactivates the two-factor authentication feature
    apiInstance.PostCurrentUserTfaTotpResourceActivityWithHttpInfo(currentUserTotpSecretActivity);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CurrentUserApi.PostCurrentUserTfaTotpResourceActivityWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **currentUserTotpSecretActivity** | [**CurrentUserTotpSecretActivity**](CurrentUserTotpSecretActivity.md) |  |  |

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
| **204** | The two-factor authentication was activated or deactivated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Cannot deactivate TOTP setup. |  -  |
| **404** | User not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postcurrentusertfatotpresourceverify"></a>
# **PostCurrentUserTfaTotpResourceVerify**
> void PostCurrentUserTfaTotpResourceVerify (CurrentUserTotpCode currentUserTotpCode)

Verify TFA code

Verifies the authentication code that the current user received from a TFA app/service and uploaded to the platform to gain access or enable the two-factor authentication feature.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_OWN_READ <b>OR</b> ROLE_SYSTEM </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostCurrentUserTfaTotpResourceVerifyExample
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

            var apiInstance = new CurrentUserApi(config);
            var currentUserTotpCode = new CurrentUserTotpCode(); // CurrentUserTotpCode | 

            try
            {
                // Verify TFA code
                apiInstance.PostCurrentUserTfaTotpResourceVerify(currentUserTotpCode);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CurrentUserApi.PostCurrentUserTfaTotpResourceVerify: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostCurrentUserTfaTotpResourceVerifyWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Verify TFA code
    apiInstance.PostCurrentUserTfaTotpResourceVerifyWithHttpInfo(currentUserTotpCode);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CurrentUserApi.PostCurrentUserTfaTotpResourceVerifyWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **currentUserTotpCode** | [**CurrentUserTotpCode**](CurrentUserTotpCode.md) |  |  |

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
| **204** | The sent code was correct and the access can be granted. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Invalid verification code. |  -  |
| **404** | Cannot validate TFA TOTP code - user&#39;s TFA TOTP secret does not exist. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="putcurrentuserpasswordresource"></a>
# **PutCurrentUserPasswordResource**
> void PutCurrentUserPasswordResource (PasswordChange passwordChange, string? accept = null)

Update the current user's password

Update the current user's  password.  > **⚠️ Important:** If the tenant uses OAI-Secure authentication, the current user will not be logged out. Instead, a new cookie will be set with a new token, and the previous token will expire within a minute.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_OWN_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutCurrentUserPasswordResourceExample
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

            var apiInstance = new CurrentUserApi(config);
            var passwordChange = new PasswordChange(); // PasswordChange | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Update the current user's password
                apiInstance.PutCurrentUserPasswordResource(passwordChange, accept);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CurrentUserApi.PutCurrentUserPasswordResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutCurrentUserPasswordResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update the current user's password
    apiInstance.PutCurrentUserPasswordResourceWithHttpInfo(passwordChange, accept);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CurrentUserApi.PutCurrentUserPasswordResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
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
| **200** | The current user password was updated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="putcurrentuserresource"></a>
# **PutCurrentUserResource**
> CurrentUser PutCurrentUserResource (CurrentUser currentUser, string? accept = null)

Update the current user

Update the current user.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_OWN_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutCurrentUserResourceExample
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

            var apiInstance = new CurrentUserApi(config);
            var currentUser = new CurrentUser(); // CurrentUser | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Update the current user
                CurrentUser result = apiInstance.PutCurrentUserResource(currentUser, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CurrentUserApi.PutCurrentUserResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutCurrentUserResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update the current user
    ApiResponse<CurrentUser> response = apiInstance.PutCurrentUserResourceWithHttpInfo(currentUser, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CurrentUserApi.PutCurrentUserResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **currentUser** | [**CurrentUser**](CurrentUser.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**CurrentUser**](CurrentUser.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.currentuser+json
 - **Accept**: application/vnd.com.nsn.cumulocity.currentuser+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The current user was updated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

