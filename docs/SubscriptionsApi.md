# kern.services.CumulocityClient.Api.SubscriptionsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**DeleteNotificationSubscriptionBySourceResource**](SubscriptionsApi.md#deletenotificationsubscriptionbysourceresource) | **DELETE** /notification2/subscriptions | Remove subscriptions by source |
| [**DeleteNotificationSubscriptionResource**](SubscriptionsApi.md#deletenotificationsubscriptionresource) | **DELETE** /notification2/subscriptions/{id} | Remove a specific subscription |
| [**GetNotificationSubscriptionCollectionResource**](SubscriptionsApi.md#getnotificationsubscriptioncollectionresource) | **GET** /notification2/subscriptions | Retrieve all subscriptions |
| [**GetNotificationSubscriptionResource**](SubscriptionsApi.md#getnotificationsubscriptionresource) | **GET** /notification2/subscriptions/{id} | Retrieve a specific subscription |
| [**PostNotificationSubscriptionResource**](SubscriptionsApi.md#postnotificationsubscriptionresource) | **POST** /notification2/subscriptions | Create a subscription |

<a name="deletenotificationsubscriptionbysourceresource"></a>
# **DeleteNotificationSubscriptionBySourceResource**
> void DeleteNotificationSubscriptionBySourceResource (string? xCumulocityProcessingMode = null, string? context = null, string? source = null)

Remove subscriptions by source

Remove subscriptions by source and context.  >**&#9432; Info:** The request will result in an error if there are no query parameters. The `source` parameter is optional only if the `context` parameter equals `tenant`.  <section><h5>Required roles</h5> ROLE_NOTIFICATION_2_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteNotificationSubscriptionBySourceResourceExample
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

            var apiInstance = new SubscriptionsApi(config);
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)
            var context = tenant;  // string? | The context to which the subscription is associated. > **&#9432; Info:** If the value is `mo`, then `source` must also be provided in the query.  (optional)  (default to mo)
            var source = 251982;  // string? | The managed object ID to which the subscription is associated. (optional) 

            try
            {
                // Remove subscriptions by source
                apiInstance.DeleteNotificationSubscriptionBySourceResource(xCumulocityProcessingMode, context, source);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SubscriptionsApi.DeleteNotificationSubscriptionBySourceResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteNotificationSubscriptionBySourceResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove subscriptions by source
    apiInstance.DeleteNotificationSubscriptionBySourceResourceWithHttpInfo(xCumulocityProcessingMode, context, source);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SubscriptionsApi.DeleteNotificationSubscriptionBySourceResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |
| **context** | **string?** | The context to which the subscription is associated. &gt; **&amp;#9432; Info:** If the value is &#x60;mo&#x60;, then &#x60;source&#x60; must also be provided in the query.  | [optional] [default to mo] |
| **source** | **string?** | The managed object ID to which the subscription is associated. | [optional]  |

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
| **204** | A collection of subscriptions was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **422** | Unprocessable Entity – error in query parameters |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletenotificationsubscriptionresource"></a>
# **DeleteNotificationSubscriptionResource**
> void DeleteNotificationSubscriptionResource (string id, string? xCumulocityProcessingMode = null)

Remove a specific subscription

Remove a specific subscription by a given ID.  <section><h5>Required roles</h5> ROLE_NOTIFICATION_2_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteNotificationSubscriptionResourceExample
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

            var apiInstance = new SubscriptionsApi(config);
            var id = 102700509;  // string | Unique identifier of the notification subscription.
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Remove a specific subscription
                apiInstance.DeleteNotificationSubscriptionResource(id, xCumulocityProcessingMode);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SubscriptionsApi.DeleteNotificationSubscriptionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteNotificationSubscriptionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove a specific subscription
    apiInstance.DeleteNotificationSubscriptionResourceWithHttpInfo(id, xCumulocityProcessingMode);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SubscriptionsApi.DeleteNotificationSubscriptionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the notification subscription. |  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

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
| **204** | A subscription was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **404** | Subscription not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getnotificationsubscriptioncollectionresource"></a>
# **GetNotificationSubscriptionCollectionResource**
> NotificationSubscriptionCollection GetNotificationSubscriptionCollectionResource (string? context = null, int? currentPage = null, int? pageSize = null, string? source = null, bool? withTotalPages = null)

Retrieve all subscriptions

Retrieve all subscriptions on your tenant, or a specific subset based on queries.  <section><h5>Required roles</h5> ROLE_NOTIFICATION_2_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetNotificationSubscriptionCollectionResourceExample
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

            var apiInstance = new SubscriptionsApi(config);
            var context = mo;  // string? | The context to which the subscription is associated. (optional) 
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var source = 251982;  // string? | The managed object ID to which the subscription is associated. (optional) 
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve all subscriptions
                NotificationSubscriptionCollection result = apiInstance.GetNotificationSubscriptionCollectionResource(context, currentPage, pageSize, source, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SubscriptionsApi.GetNotificationSubscriptionCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetNotificationSubscriptionCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all subscriptions
    ApiResponse<NotificationSubscriptionCollection> response = apiInstance.GetNotificationSubscriptionCollectionResourceWithHttpInfo(context, currentPage, pageSize, source, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SubscriptionsApi.GetNotificationSubscriptionCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **context** | **string?** | The context to which the subscription is associated. | [optional]  |
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **source** | **string?** | The managed object ID to which the subscription is associated. | [optional]  |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**NotificationSubscriptionCollection**](NotificationSubscriptionCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.subscriptioncollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and all subscriptions are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getnotificationsubscriptionresource"></a>
# **GetNotificationSubscriptionResource**
> NotificationSubscription GetNotificationSubscriptionResource (string id)

Retrieve a specific subscription

Retrieve a specific subscription by a given ID.  <section><h5>Required roles</h5> ROLE_NOTIFICATION_2_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetNotificationSubscriptionResourceExample
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

            var apiInstance = new SubscriptionsApi(config);
            var id = 102700509;  // string | Unique identifier of the notification subscription.

            try
            {
                // Retrieve a specific subscription
                NotificationSubscription result = apiInstance.GetNotificationSubscriptionResource(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SubscriptionsApi.GetNotificationSubscriptionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetNotificationSubscriptionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a specific subscription
    ApiResponse<NotificationSubscription> response = apiInstance.GetNotificationSubscriptionResourceWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SubscriptionsApi.GetNotificationSubscriptionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the notification subscription. |  |

### Return type

[**NotificationSubscription**](NotificationSubscription.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.subscription+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the subscription is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **404** | Subscription not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postnotificationsubscriptionresource"></a>
# **PostNotificationSubscriptionResource**
> NotificationSubscription PostNotificationSubscriptionResource (NotificationSubscription notificationSubscription, string? accept = null, string? xCumulocityProcessingMode = null)

Create a subscription

Create a new subscription, for example, a subscription that forwards measurements and events of a specific type for a given device.  In general, each subscription may consist of:  *  The managed object to which the subscription is associated. *  The context under which the subscription is to be processed. *  The name of the subscription. *  The applicable filter criteria. *  The option to only include specific custom fragments in the forwarded data.  <section><h5>Required roles</h5> ROLE_NOTIFICATION_2_ADMIN </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostNotificationSubscriptionResourceExample
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

            var apiInstance = new SubscriptionsApi(config);
            var notificationSubscription = new NotificationSubscription(); // NotificationSubscription | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Create a subscription
                NotificationSubscription result = apiInstance.PostNotificationSubscriptionResource(notificationSubscription, accept, xCumulocityProcessingMode);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SubscriptionsApi.PostNotificationSubscriptionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostNotificationSubscriptionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create a subscription
    ApiResponse<NotificationSubscription> response = apiInstance.PostNotificationSubscriptionResourceWithHttpInfo(notificationSubscription, accept, xCumulocityProcessingMode);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SubscriptionsApi.PostNotificationSubscriptionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **notificationSubscription** | [**NotificationSubscription**](NotificationSubscription.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

[**NotificationSubscription**](NotificationSubscription.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.subscription+json
 - **Accept**: application/vnd.com.nsn.cumulocity.subscription+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | A notification subscription was created. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not enough permissions/roles to perform this operation. |  -  |
| **404** | Managed object not found. |  -  |
| **409** | Duplicated subscription. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

