# kern.services.CumulocityClient.Api.ApplicationVersionsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**DeleteApplicationVersionResource**](ApplicationVersionsApi.md#deleteapplicationversionresource) | **DELETE** /application/applications/{id}/versions | Delete a specific version of an application |
| [**GetApplicationVersionCollectionResource**](ApplicationVersionsApi.md#getapplicationversioncollectionresource) | **GET** /application/applications/{id}/versions | Retrieve all versions of an application |
| [**GetApplicationVersionResource**](ApplicationVersionsApi.md#getapplicationversionresource) | **GET** /application/applications/{id}/versions?version&#x3D;1.0 | Retrieve a specific version of an application |
| [**PostApplicationVersionResource**](ApplicationVersionsApi.md#postapplicationversionresource) | **POST** /application/applications/{id}/versions | Create an application version |
| [**PutApplicationVersionResource**](ApplicationVersionsApi.md#putapplicationversionresource) | **PUT** /application/applications/{id}/versions/{version} | Replace an application version&#39;s tags |

<a id="deleteapplicationversionresource"></a>
# **DeleteApplicationVersionResource**
> void DeleteApplicationVersionResource (string id, string? varVersion = null, string? tag = null)

Delete a specific version of an application

Delete a specific version of an application in your tenant, by a given tag or version.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteApplicationVersionResourceExample
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

            var apiInstance = new ApplicationVersionsApi(config);
            var id = 20200301;  // string | Unique identifier of the application.
            var varVersion = 1;  // string? | The version field of the application version. (optional) 
            var tag = tag1;  // string? | The tag of the application version. (optional) 

            try
            {
                // Delete a specific version of an application
                apiInstance.DeleteApplicationVersionResource(id, varVersion, tag);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationVersionsApi.DeleteApplicationVersionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteApplicationVersionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete a specific version of an application
    apiInstance.DeleteApplicationVersionResourceWithHttpInfo(id, varVersion, tag);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationVersionsApi.DeleteApplicationVersionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the application. |  |
| **varVersion** | **string?** | The version field of the application version. | [optional]  |
| **tag** | **string?** | The tag of the application version. | [optional]  |

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
| **204** | A version was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Application version not found. |  -  |
| **409** | Version with tag latest cannot be removed. |  -  |
| **422** | both parameters (version and tag) are present. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getapplicationversioncollectionresource"></a>
# **GetApplicationVersionCollectionResource**
> ApplicationVersionCollection GetApplicationVersionCollectionResource (string id, string? accept = null)

Retrieve all versions of an application

Retrieve all versions of an application in your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetApplicationVersionCollectionResourceExample
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

            var apiInstance = new ApplicationVersionsApi(config);
            var id = 20200301;  // string | Unique identifier of the application.
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Retrieve all versions of an application
                ApplicationVersionCollection result = apiInstance.GetApplicationVersionCollectionResource(id, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationVersionsApi.GetApplicationVersionCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetApplicationVersionCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all versions of an application
    ApiResponse<ApplicationVersionCollection> response = apiInstance.GetApplicationVersionCollectionResourceWithHttpInfo(id, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationVersionsApi.GetApplicationVersionCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the application. |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**ApplicationVersionCollection**](ApplicationVersionCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.applicationVersionCollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the list of application versions is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Application version not found. |  -  |
| **422** | This application doesn&#39;t support versioning. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getapplicationversionresource"></a>
# **GetApplicationVersionResource**
> ApplicationVersion GetApplicationVersionResource (string id, string accept, string? varVersion = null, string? tag = null)

Retrieve a specific version of an application

Retrieve the selected version of an application in your tenant. To select the version, use only the version or only the tag query parameter. <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section>

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetApplicationVersionResourceExample
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

            var apiInstance = new ApplicationVersionsApi(config);
            var id = 20200301;  // string | Unique identifier of the application.
            var accept = application/vnd.com.nsn.cumulocity.applicationVersion+json;  // string | The header is required to access this endpoint.
            var varVersion = 1;  // string? | The version field of the application version. (optional) 
            var tag = tag1;  // string? | The tag of the application version. (optional) 

            try
            {
                // Retrieve a specific version of an application
                ApplicationVersion result = apiInstance.GetApplicationVersionResource(id, accept, varVersion, tag);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationVersionsApi.GetApplicationVersionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetApplicationVersionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a specific version of an application
    ApiResponse<ApplicationVersion> response = apiInstance.GetApplicationVersionResourceWithHttpInfo(id, accept, varVersion, tag);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationVersionsApi.GetApplicationVersionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the application. |  |
| **accept** | **string** | The header is required to access this endpoint. |  |
| **varVersion** | **string?** | The version field of the application version. | [optional]  |
| **tag** | **string?** | The tag of the application version. | [optional]  |

### Return type

[**ApplicationVersion**](ApplicationVersion.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.applicationVersion+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the application version is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Application not found. |  -  |
| **422** | both parameters (version and tag) are present. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="postapplicationversionresource"></a>
# **PostApplicationVersionResource**
> ApplicationVersion PostApplicationVersionResource (string id, System.IO.Stream applicationBinary, string applicationVersion, string? accept = null)

Create an application version

Create an application version in your tenant.  Uploaded version and tags can only contain upper and lower case letters, integers and `.`,` + `,` -`. Other characters are prohibited.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostApplicationVersionResourceExample
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

            var apiInstance = new ApplicationVersionsApi(config);
            var id = 20200301;  // string | Unique identifier of the application.
            var applicationBinary = new System.IO.MemoryStream(System.IO.File.ReadAllBytes("/path/to/file.txt"));  // System.IO.Stream | The ZIP file to be uploaded.
            var applicationVersion = "applicationVersion_example";  // string | The JSON file with version information.
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Create an application version
                ApplicationVersion result = apiInstance.PostApplicationVersionResource(id, applicationBinary, applicationVersion, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationVersionsApi.PostApplicationVersionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostApplicationVersionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create an application version
    ApiResponse<ApplicationVersion> response = apiInstance.PostApplicationVersionResourceWithHttpInfo(id, applicationBinary, applicationVersion, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationVersionsApi.PostApplicationVersionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the application. |  |
| **applicationBinary** | **System.IO.Stream****System.IO.Stream** | The ZIP file to be uploaded. |  |
| **applicationVersion** | **string** | The JSON file with version information. |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**ApplicationVersion**](ApplicationVersion.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: multipart/form-data
 - **Accept**: application/vnd.com.nsn.cumulocity.applicationVersion+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | An application version was created. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Application version not found. |  -  |
| **409** | Duplicate version/tag or versions limit exceeded. |  -  |
| **422** | tag or version contains unacceptable characters. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="putapplicationversionresource"></a>
# **PutApplicationVersionResource**
> ApplicationVersion PutApplicationVersionResource (string id, string varVersion, ApplicationVersionTag applicationVersionTag, string? accept = null)

Replace an application version's tags

Replaces the tags of a given application version in your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutApplicationVersionResourceExample
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

            var apiInstance = new ApplicationVersionsApi(config);
            var id = 20200301;  // string | Unique identifier of the application.
            var varVersion = 1.0;  // string | Version of the application.
            var applicationVersionTag = new ApplicationVersionTag(); // ApplicationVersionTag | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Replace an application version's tags
                ApplicationVersion result = apiInstance.PutApplicationVersionResource(id, varVersion, applicationVersionTag, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationVersionsApi.PutApplicationVersionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutApplicationVersionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Replace an application version's tags
    ApiResponse<ApplicationVersion> response = apiInstance.PutApplicationVersionResourceWithHttpInfo(id, varVersion, applicationVersionTag, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationVersionsApi.PutApplicationVersionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the application. |  |
| **varVersion** | **string** | Version of the application. |  |
| **applicationVersionTag** | [**ApplicationVersionTag**](ApplicationVersionTag.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**ApplicationVersion**](ApplicationVersion.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/vnd.com.nsn.cumulocity.applicationVersion+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | An application version was updated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Application version not found. |  -  |
| **409** | Duplicate version/tag or versions limit exceeded. |  -  |
| **422** | tag contains unacceptable characters. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

