# kern.services.CumulocityClient.Api.BinariesApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**DeleteBinariesResource**](BinariesApi.md#deletebinariesresource) | **DELETE** /inventory/binaries/{id} | Remove a stored file |
| [**GetBinariesCollectionResource**](BinariesApi.md#getbinariescollectionresource) | **GET** /inventory/binaries | Retrieve the stored files |
| [**GetBinariesResource**](BinariesApi.md#getbinariesresource) | **GET** /inventory/binaries/{id} | Retrieve a stored file |
| [**PostBinariesCollectionResource**](BinariesApi.md#postbinariescollectionresource) | **POST** /inventory/binaries | Upload a file |
| [**PutBinariesResource**](BinariesApi.md#putbinariesresource) | **PUT** /inventory/binaries/{id} | Replace a file |

<a id="deletebinariesresource"></a>
# **DeleteBinariesResource**
> void DeleteBinariesResource (string id)

Remove a stored file

Remove a managed object and its stored file by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the resource <b>OR</b> MANAGE_OBJECT_ADMIN permission on the resource </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteBinariesResourceExample
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

            var apiInstance = new BinariesApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.

            try
            {
                // Remove a stored file
                apiInstance.DeleteBinariesResource(id);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BinariesApi.DeleteBinariesResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteBinariesResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove a stored file
    apiInstance.DeleteBinariesResourceWithHttpInfo(id);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BinariesApi.DeleteBinariesResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |

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
| **204** | A managed object and its stored file was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getbinariescollectionresource"></a>
# **GetBinariesCollectionResource**
> BinaryCollection GetBinariesCollectionResource (string? childAdditionId = null, string? childAssetId = null, string? childDeviceId = null, int? currentPage = null, List<string>? ids = null, string? owner = null, int? pageSize = null, string? text = null, string? type = null, bool? withTotalPages = null)

Retrieve the stored files

Retrieve the stored files as a collections of managed objects. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetBinariesCollectionResourceExample
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

            var apiInstance = new BinariesApi(config);
            var childAdditionId = 3003;  // string? | Search for a specific child addition and list all the groups to which it belongs. (optional) 
            var childAssetId = 200;  // string? | Search for a specific child asset and list all the groups to which it belongs. (optional) 
            var childDeviceId = 2001;  // string? | Search for a specific child device and list all the groups to which it belongs. (optional) 
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var ids = new List<string>?(); // List<string>? | The managed object IDs to search for. >**&#9432; Info:** If you query for multiple IDs at once, comma-separate the values.  (optional) 
            var owner = manga;  // string? | Username of the owner of the managed objects. (optional) 
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var text = my_value;  // string? | Search for managed objects where any property value is equal to the given one. Only string values are supported. (optional) 
            var type = c8y_DeviceGroup;  // string? | The type of managed object to search for. (optional) 
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve the stored files
                BinaryCollection result = apiInstance.GetBinariesCollectionResource(childAdditionId, childAssetId, childDeviceId, currentPage, ids, owner, pageSize, text, type, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BinariesApi.GetBinariesCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetBinariesCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve the stored files
    ApiResponse<BinaryCollection> response = apiInstance.GetBinariesCollectionResourceWithHttpInfo(childAdditionId, childAssetId, childDeviceId, currentPage, ids, owner, pageSize, text, type, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BinariesApi.GetBinariesCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **childAdditionId** | **string?** | Search for a specific child addition and list all the groups to which it belongs. | [optional]  |
| **childAssetId** | **string?** | Search for a specific child asset and list all the groups to which it belongs. | [optional]  |
| **childDeviceId** | **string?** | Search for a specific child device and list all the groups to which it belongs. | [optional]  |
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **ids** | [**List&lt;string&gt;?**](string.md) | The managed object IDs to search for. &gt;**&amp;#9432; Info:** If you query for multiple IDs at once, comma-separate the values.  | [optional]  |
| **owner** | **string?** | Username of the owner of the managed objects. | [optional]  |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **text** | **string?** | Search for managed objects where any property value is equal to the given one. Only string values are supported. | [optional]  |
| **type** | **string?** | The type of managed object to search for. | [optional]  |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**BinaryCollection**](BinaryCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.managedobjectcollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the managed objects are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getbinariesresource"></a>
# **GetBinariesResource**
> System.IO.Stream GetBinariesResource (string id)

Retrieve a stored file

Retrieve a stored file (managed object) by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the resource <b>OR</b> MANAGE_OBJECT_READ permission on the resource </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetBinariesResourceExample
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

            var apiInstance = new BinariesApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.

            try
            {
                // Retrieve a stored file
                System.IO.Stream result = apiInstance.GetBinariesResource(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BinariesApi.GetBinariesResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetBinariesResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a stored file
    ApiResponse<System.IO.Stream> response = apiInstance.GetBinariesResourceWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BinariesApi.GetBinariesResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |

### Return type

**System.IO.Stream**

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/octet-stream, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the file is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="postbinariescollectionresource"></a>
# **PostBinariesCollectionResource**
> byte[] PostBinariesCollectionResource (BinaryInfo varObject, System.IO.Stream file)

Upload a file

Uploading a file (binary) requires providing the following properties:  * `object` – In JSON format, it contains information about the file. * `file` – Contains the file to be uploaded.  After the file has been uploaded, the corresponding managed object will contain the fragment `c8y_IsBinary`.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> ROLE_INVENTORY_CREATE </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostBinariesCollectionResourceExample
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

            var apiInstance = new BinariesApi(config);
            var varObject = new BinaryInfo(); // BinaryInfo | 
            var file = new System.IO.MemoryStream(System.IO.File.ReadAllBytes("/path/to/file.txt"));  // System.IO.Stream | Path of the file to be uploaded.

            try
            {
                // Upload a file
                byte[] result = apiInstance.PostBinariesCollectionResource(varObject, file);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BinariesApi.PostBinariesCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostBinariesCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Upload a file
    ApiResponse<byte[]> response = apiInstance.PostBinariesCollectionResourceWithHttpInfo(varObject, file);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BinariesApi.PostBinariesCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **varObject** | [**BinaryInfo**](BinaryInfo.md) |  |  |
| **file** | **System.IO.Stream****System.IO.Stream** | Path of the file to be uploaded. |  |

### Return type

**byte[]**

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: multipart/form-data
 - **Accept**: application/vnd.com.nsn.cumulocity.managedobject+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | A file was uploaded. |  -  |
| **400** | Unprocessable Entity – invalid payload. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="putbinariesresource"></a>
# **PutBinariesResource**
> byte[] PutBinariesResource (string id, System.IO.Stream body)

Replace a file

Upload and replace the attached file (binary) of a specific managed object by a given ID.<br>  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the resource <b>OR</b> MANAGE_OBJECT_ADMIN permission on the resource </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutBinariesResourceExample
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

            var apiInstance = new BinariesApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var body = new System.IO.MemoryStream(System.IO.File.ReadAllBytes("/path/to/file.txt"));  // System.IO.Stream | 

            try
            {
                // Replace a file
                byte[] result = apiInstance.PutBinariesResource(id, body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BinariesApi.PutBinariesResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutBinariesResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Replace a file
    ApiResponse<byte[]> response = apiInstance.PutBinariesResourceWithHttpInfo(id, body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BinariesApi.PutBinariesResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **body** | **System.IO.Stream****System.IO.Stream** |  |  |

### Return type

**byte[]**

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: text/plain
 - **Accept**: application/vnd.com.nsn.cumulocity.managedobject+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | A file was uploaded. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

