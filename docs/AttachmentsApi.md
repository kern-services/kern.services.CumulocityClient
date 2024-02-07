# kern.services.CumulocityClient.Api.AttachmentsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteEventBinaryResource**](AttachmentsApi.md#deleteeventbinaryresource) | **DELETE** /event/events/{id}/binaries | Remove the attached file from a specific event
[**GetEventBinaryResource**](AttachmentsApi.md#geteventbinaryresource) | **GET** /event/events/{id}/binaries | Retrieve the attached file of a specific event
[**PostEventBinaryResource**](AttachmentsApi.md#posteventbinaryresource) | **POST** /event/events/{id}/binaries | Attach a file to a specific event
[**PutEventBinaryResource**](AttachmentsApi.md#puteventbinaryresource) | **PUT** /event/events/{id}/binaries | Replace the attached file of a specific event



## DeleteEventBinaryResource

> void DeleteEventBinaryResource (string id)

Remove the attached file from a specific event

Remove the attached file (binary) from a specific event by a given ID.  <section><h5>Required roles</h5> ROLE_EVENT_ADMIN <b>OR</b> owner of the source <b>OR</b> EVENT_ADMIN permission on the source </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteEventBinaryResourceExample
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

            var apiInstance = new AttachmentsApi(Configuration.Default);
            var id = 20200301;  // string | Unique identifier of the event.

            try
            {
                // Remove the attached file from a specific event
                apiInstance.DeleteEventBinaryResource(id);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling AttachmentsApi.DeleteEventBinaryResource: " + e.Message );
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
 **id** | **string**| Unique identifier of the event. | 

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
| **204** | A file was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Event not found. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)


## GetEventBinaryResource

> System.IO.Stream GetEventBinaryResource (string id)

Retrieve the attached file of a specific event

Retrieve the attached file (binary) of a specific event by a given ID.  <section><h5>Required roles</h5> ROLE_EVENT_READ <b>OR</b> EVENT_READ permission on the source </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetEventBinaryResourceExample
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

            var apiInstance = new AttachmentsApi(Configuration.Default);
            var id = 20200301;  // string | Unique identifier of the event.

            try
            {
                // Retrieve the attached file of a specific event
                System.IO.Stream result = apiInstance.GetEventBinaryResource(id);
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling AttachmentsApi.GetEventBinaryResource: " + e.Message );
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
 **id** | **string**| Unique identifier of the event. | 

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
| **404** | Attachment not found. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)


## PostEventBinaryResource

> EventBinary PostEventBinaryResource (string id, System.IO.Stream body)

Attach a file to a specific event

Upload a file (binary) as an attachment of a specific event by a given ID. The size of the attachment is configurable, and the default size is 50 MiB. The default chunk size is 5MiB.  > **&#9432; Info:** If there is a binary already attached to the event, the POST request results in a 409 error.  When the file has been uploaded, the corresponding event contains the fragment `c8y_IsBinary` similar to:  ```json \"c8y_IsBinary\": {     \"name\": \"hello.txt\",     \"length\": 365,     \"type\": \"text/plain\" } ```  There are two request body schemas you can use for your POST requests. `text/plain` is preselected (see below). If you set it to `multipart/form-data` each value is sent as a block of data (body part), with a user agent-defined delimiter (`boundary`) separating each part. The keys are given in the `Content-Disposition` header of each part.  ```http POST /event/events/{id}/binaries Host: https://<TENANT_DOMAIN> Authorization: <AUTHORIZATION> Accept: application/json Content-Type: multipart/form-data;boundary=\"boundary\"  - -boundary Content-Disposition: form-data; name=\"object\"  { \"name\": \"hello.txt\", \"type\": \"text/plain\" } - -boundary Content-Disposition: form-data; name=\"file\"; filename=\"hello.txt\" Content-Type: text/plain  <FILE_CONTENTS> - -boundary- - ```  <section><h5>Required roles</h5> ROLE_EVENT_ADMIN <b>OR</b> owner of the source <b>OR</b> EVENT_ADMIN permission on the source </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostEventBinaryResourceExample
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

            var apiInstance = new AttachmentsApi(Configuration.Default);
            var id = 20200301;  // string | Unique identifier of the event.
            var body = new System.IO.MemoryStream(System.IO.File.ReadAllBytes("/path/to/file.txt"));  // System.IO.Stream | 

            try
            {
                // Attach a file to a specific event
                EventBinary result = apiInstance.PostEventBinaryResource(id, body);
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling AttachmentsApi.PostEventBinaryResource: " + e.Message );
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
 **id** | **string**| Unique identifier of the event. | 
 **body** | **System.IO.Stream**|  | 

### Return type

[**EventBinary**](EventBinary.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

- **Content-Type**: text/plain, multipart/form-data
- **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | A file was uploaded. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Event not found. |  -  |
| **409** | An attachment exists already. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)


## PutEventBinaryResource

> EventBinary PutEventBinaryResource (string id, System.IO.Stream body = null)

Replace the attached file of a specific event

Upload and replace the attached file (binary) of a specific event by a given ID.<br> The size of the attachment is configurable, and the default size is 50 MiB. The default chunk size is 5MiB.  <section><h5>Required roles</h5> ROLE_EVENT_ADMIN <b>OR</b> owner of the source <b>OR</b> EVENT_ADMIN permission on the source </section> 

### Example

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutEventBinaryResourceExample
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

            var apiInstance = new AttachmentsApi(Configuration.Default);
            var id = 20200301;  // string | Unique identifier of the event.
            var body = new System.IO.MemoryStream(System.IO.File.ReadAllBytes("/path/to/file.txt"));  // System.IO.Stream |  (optional) 

            try
            {
                // Replace the attached file of a specific event
                EventBinary result = apiInstance.PutEventBinaryResource(id, body);
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling AttachmentsApi.PutEventBinaryResource: " + e.Message );
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
 **id** | **string**| Unique identifier of the event. | 
 **body** | **System.IO.Stream**|  | [optional] 

### Return type

[**EventBinary**](EventBinary.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

- **Content-Type**: text/plain
- **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | A file was uploaded. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Event not found. |  -  |

[[Back to top]](#)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to Model list]](../README.md#documentation-for-models)
[[Back to README]](../README.md)

