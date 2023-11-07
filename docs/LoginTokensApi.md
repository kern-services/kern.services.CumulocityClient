# kern.services.CumulocityClient.Api.LoginTokensApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**PostLoginFormBody**](LoginTokensApi.md#postloginformbody) | **POST** /tenant/oauth/token | Obtain an access token |
| [**PostLoginFormCookie**](LoginTokensApi.md#postloginformcookie) | **POST** /tenant/oauth | Obtain access tokens in cookies |

<a name="postloginformbody"></a>
# **PostLoginFormBody**
> AccessToken PostLoginFormBody (string? accept = null, string? code = null, string? grantType = null, string? password = null, string? tfaCode = null, string? username = null)

Obtain an access token

Obtain an OAI-Secure access token.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostLoginFormBodyExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://<TENANT_DOMAIN>";
            // Configure HTTP basic authorization: Basic
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new LoginTokensApi(config);
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var code = "code_example";  // string? | Used in case of SSO login. A code received from the external authentication server is exchanged to an internal access token. (optional) 
            var grantType = "PASSWORD";  // string? | Dependent on the authentication type. PASSWORD is used for OAI-Secure. (optional) 
            var password = "password_example";  // string? | Used in cases of basic or OAI-Secure authentication. (optional) 
            var tfaCode = "tfaCode_example";  // string? | Current TFA code, sent by the user, if a TFA code is required to log in. (optional) 
            var username = "username_example";  // string? | Used in cases of basic or OAI-Secure authentication. (optional) 

            try
            {
                // Obtain an access token
                AccessToken result = apiInstance.PostLoginFormBody(accept, code, grantType, password, tfaCode, username);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling LoginTokensApi.PostLoginFormBody: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostLoginFormBodyWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Obtain an access token
    ApiResponse<AccessToken> response = apiInstance.PostLoginFormBodyWithHttpInfo(accept, code, grantType, password, tfaCode, username);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling LoginTokensApi.PostLoginFormBodyWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **code** | **string?** | Used in case of SSO login. A code received from the external authentication server is exchanged to an internal access token. | [optional]  |
| **grantType** | **string?** | Dependent on the authentication type. PASSWORD is used for OAI-Secure. | [optional]  |
| **password** | **string?** | Used in cases of basic or OAI-Secure authentication. | [optional]  |
| **tfaCode** | **string?** | Current TFA code, sent by the user, if a TFA code is required to log in. | [optional]  |
| **username** | **string?** | Used in cases of basic or OAI-Secure authentication. | [optional]  |

### Return type

[**AccessToken**](AccessToken.md)

### Authorization

[Basic](../README.md#Basic)

### HTTP request headers

 - **Content-Type**: application/x-www-form-urlencoded
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The OAI-Secure access token is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postloginformcookie"></a>
# **PostLoginFormCookie**
> void PostLoginFormCookie (string? accept = null, string? code = null, string? grantType = null, string? password = null, string? tfaCode = null, string? username = null)

Obtain access tokens in cookies

Obtain an OAI-Secure and XSRF tokens in cookies. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostLoginFormCookieExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://<TENANT_DOMAIN>";
            // Configure HTTP basic authorization: Basic
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";

            var apiInstance = new LoginTokensApi(config);
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var code = "code_example";  // string? | Used in case of SSO login. A code received from the external authentication server is exchanged to an internal access token. (optional) 
            var grantType = "PASSWORD";  // string? | Dependent on the authentication type. PASSWORD is used for OAI-Secure. (optional) 
            var password = "password_example";  // string? | Used in cases of basic or OAI-Secure authentication. (optional) 
            var tfaCode = "tfaCode_example";  // string? | Current TFA code, sent by the user, if a TFA code is required to log in. (optional) 
            var username = "username_example";  // string? | Used in cases of basic or OAI-Secure authentication. (optional) 

            try
            {
                // Obtain access tokens in cookies
                apiInstance.PostLoginFormCookie(accept, code, grantType, password, tfaCode, username);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling LoginTokensApi.PostLoginFormCookie: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostLoginFormCookieWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Obtain access tokens in cookies
    apiInstance.PostLoginFormCookieWithHttpInfo(accept, code, grantType, password, tfaCode, username);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling LoginTokensApi.PostLoginFormCookieWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **code** | **string?** | Used in case of SSO login. A code received from the external authentication server is exchanged to an internal access token. | [optional]  |
| **grantType** | **string?** | Dependent on the authentication type. PASSWORD is used for OAI-Secure. | [optional]  |
| **password** | **string?** | Used in cases of basic or OAI-Secure authentication. | [optional]  |
| **tfaCode** | **string?** | Current TFA code, sent by the user, if a TFA code is required to log in. | [optional]  |
| **username** | **string?** | Used in cases of basic or OAI-Secure authentication. | [optional]  |

### Return type

void (empty response body)

### Authorization

[Basic](../README.md#Basic)

### HTTP request headers

 - **Content-Type**: application/x-www-form-urlencoded
 - **Accept**: application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The tokens are return in cookies. |  * Set-Cookie -  <br>  *  Set-Cookie -  <br>  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

