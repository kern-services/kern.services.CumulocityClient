# kern.services.CumulocityClient.Api.ApplicationBinariesApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**DeleteBinaryApplicationContentResourceById**](ApplicationBinariesApi.md#deletebinaryapplicationcontentresourcebyid) | **DELETE** /application/applications/{id}/binaries/{binaryId} | Delete a specific application attachment |
| [**GetBinaryApplicationContentResource**](ApplicationBinariesApi.md#getbinaryapplicationcontentresource) | **GET** /application/applications/{id}/binaries | Retrieve all application attachments |
| [**GetBinaryApplicationContentResourceById**](ApplicationBinariesApi.md#getbinaryapplicationcontentresourcebyid) | **GET** /application/applications/{id}/binaries/{binaryId} | Retrieve a specific application attachment |
| [**PostBinaryApplicationContentResource**](ApplicationBinariesApi.md#postbinaryapplicationcontentresource) | **POST** /application/applications/{id}/binaries | Upload an application attachment |

<a name="deletebinaryapplicationcontentresourcebyid"></a>
# **DeleteBinaryApplicationContentResourceById**
> void DeleteBinaryApplicationContentResourceById (string id, string binaryId)

Delete a specific application attachment

Delete  a specific application attachment (by a given application ID and a given binary ID). This method is not supported by microservice applications.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN <b>AND</b> tenant is the owner of the application </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteBinaryApplicationContentResourceByIdExample
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

            var apiInstance = new ApplicationBinariesApi(config);
            var id = 20200301;  // string | Unique identifier of the application.
            var binaryId = 30303033;  // string | Unique identifier of the binary.

            try
            {
                // Delete a specific application attachment
                apiInstance.DeleteBinaryApplicationContentResourceById(id, binaryId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationBinariesApi.DeleteBinaryApplicationContentResourceById: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteBinaryApplicationContentResourceByIdWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete a specific application attachment
    apiInstance.DeleteBinaryApplicationContentResourceByIdWithHttpInfo(id, binaryId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationBinariesApi.DeleteBinaryApplicationContentResourceByIdWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the application. |  |
| **binaryId** | **string** | Unique identifier of the binary. |  |

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
| **204** | An application binary was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getbinaryapplicationcontentresource"></a>
# **GetBinaryApplicationContentResource**
> ApplicationBinaries GetBinaryApplicationContentResource (string id)

Retrieve all application attachments

Retrieve all application attachments. This method is not supported by microservice applications.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetBinaryApplicationContentResourceExample
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

            var apiInstance = new ApplicationBinariesApi(config);
            var id = 20200301;  // string | Unique identifier of the application.

            try
            {
                // Retrieve all application attachments
                ApplicationBinaries result = apiInstance.GetBinaryApplicationContentResource(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationBinariesApi.GetBinaryApplicationContentResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetBinaryApplicationContentResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all application attachments
    ApiResponse<ApplicationBinaries> response = apiInstance.GetBinaryApplicationContentResourceWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationBinariesApi.GetBinaryApplicationContentResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the application. |  |

### Return type

[**ApplicationBinaries**](ApplicationBinaries.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.applicationbinaries+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the application attachments are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Application not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getbinaryapplicationcontentresourcebyid"></a>
# **GetBinaryApplicationContentResourceById**
> System.IO.Stream GetBinaryApplicationContentResourceById (string id, string binaryId)

Retrieve a specific application attachment

Retrieve a specific application attachment (by a given application ID and a given binary ID). This method is not supported by microservice applications.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetBinaryApplicationContentResourceByIdExample
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

            var apiInstance = new ApplicationBinariesApi(config);
            var id = 20200301;  // string | Unique identifier of the application.
            var binaryId = 30303033;  // string | Unique identifier of the binary.

            try
            {
                // Retrieve a specific application attachment
                System.IO.Stream result = apiInstance.GetBinaryApplicationContentResourceById(id, binaryId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationBinariesApi.GetBinaryApplicationContentResourceById: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetBinaryApplicationContentResourceByIdWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a specific application attachment
    ApiResponse<System.IO.Stream> response = apiInstance.GetBinaryApplicationContentResourceByIdWithHttpInfo(id, binaryId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationBinariesApi.GetBinaryApplicationContentResourceByIdWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the application. |  |
| **binaryId** | **string** | Unique identifier of the binary. |  |

### Return type

**System.IO.Stream**

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/zip, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the application attachment is sent as a ZIP file in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postbinaryapplicationcontentresource"></a>
# **PostBinaryApplicationContentResource**
> Application PostBinaryApplicationContentResource (string id, System.IO.Stream file, string? accept = null)

Upload an application attachment

Upload an application attachment (by a given application ID).  For the applications of type “microservice” and “web application” to be available for Cumulocity IoT platform users, an attachment ZIP file must be uploaded.  For a microservice application, the ZIP file must consist of:  * cumulocity.json - file describing the deployment * image.tar - executable Docker image  For a web application, the ZIP file must include an index.html file in the root directory.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN <b>AND</b> tenant is the owner of the application </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostBinaryApplicationContentResourceExample
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

            var apiInstance = new ApplicationBinariesApi(config);
            var id = 20200301;  // string | Unique identifier of the application.
            var file = new System.IO.MemoryStream(System.IO.File.ReadAllBytes("/path/to/file.txt"));  // System.IO.Stream | The ZIP file to be uploaded.
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Upload an application attachment
                Application result = apiInstance.PostBinaryApplicationContentResource(id, file, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationBinariesApi.PostBinaryApplicationContentResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostBinaryApplicationContentResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Upload an application attachment
    ApiResponse<Application> response = apiInstance.PostBinaryApplicationContentResourceWithHttpInfo(id, file, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationBinariesApi.PostBinaryApplicationContentResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the application. |  |
| **file** | **System.IO.Stream****System.IO.Stream** | The ZIP file to be uploaded. |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**Application**](Application.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: multipart/form-data
 - **Accept**: application/vnd.com.nsn.cumulocity.application+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | The application attachments have been uploaded. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

