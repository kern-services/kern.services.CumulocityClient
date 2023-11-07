# kern.services.CumulocityClient.Api.ManagedObjectsApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**DeleteManagedObjectResource**](ManagedObjectsApi.md#deletemanagedobjectresource) | **DELETE** /inventory/managedObjects/{id} | Remove a specific managed object |
| [**GetLastAvailabilityManagedObjectResource**](ManagedObjectsApi.md#getlastavailabilitymanagedobjectresource) | **GET** /inventory/managedObjects/{id}/availability | Retrieve the latest availability date of a specific managed object |
| [**GetManagedObjectCollectionResource**](ManagedObjectsApi.md#getmanagedobjectcollectionresource) | **GET** /inventory/managedObjects | Retrieve all managed objects |
| [**GetManagedObjectResource**](ManagedObjectsApi.md#getmanagedobjectresource) | **GET** /inventory/managedObjects/{id} | Retrieve a specific managed object |
| [**GetManagedObjectUserResource**](ManagedObjectsApi.md#getmanagedobjectuserresource) | **GET** /inventory/managedObjects/{id}/user | Retrieve the username and state of a specific managed object |
| [**GetSupportedMeasurementsManagedObjectResource**](ManagedObjectsApi.md#getsupportedmeasurementsmanagedobjectresource) | **GET** /inventory/managedObjects/{id}/supportedMeasurements | Retrieve all supported measurement fragments of a specific managed object |
| [**GetSupportedSeriesManagedObjectResource**](ManagedObjectsApi.md#getsupportedseriesmanagedobjectresource) | **GET** /inventory/managedObjects/{id}/supportedSeries | Retrieve all supported measurement fragments and series of a specific managed object |
| [**PostManagedObjectCollectionResource**](ManagedObjectsApi.md#postmanagedobjectcollectionresource) | **POST** /inventory/managedObjects | Create a managed object |
| [**PutManagedObjectResource**](ManagedObjectsApi.md#putmanagedobjectresource) | **PUT** /inventory/managedObjects/{id} | Update a specific managed object |
| [**PutManagedObjectUserResource**](ManagedObjectsApi.md#putmanagedobjectuserresource) | **PUT** /inventory/managedObjects/{id}/user | Update the user&#39;s details of a specific managed object |

<a name="deletemanagedobjectresource"></a>
# **DeleteManagedObjectResource**
> void DeleteManagedObjectResource (string id, string? xCumulocityProcessingMode = null, bool? cascade = null, bool? forceCascade = null, bool? withDeviceUser = null)

Remove a specific managed object

Remove a specific managed object (for example, device) by a given ID.  > **&#9432; Info:** Inventory DELETE requests are not synchronous. The response could be returned before the delete request has been completed. This may happen especially when the deleted managed object has a lot of associated data. After sending the request, the platform starts deleting the associated data in an asynchronous way. Finally, the requested managed object is deleted after all associated data has been deleted.  > **&#9432; Info:** By default, the delete operation is always propagated to the subgroups, but only if the deleted object is a group.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteManagedObjectResourceExample
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

            var apiInstance = new ManagedObjectsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)
            var cascade = true;  // bool? | When set to `true` and the managed object is a device or group, all the hierarchy will be deleted. (optional)  (default to false)
            var forceCascade = true;  // bool? | When set to `true` all the hierarchy will be deleted without checking the type of managed object. It takes precedence over the parameter `cascade`. (optional)  (default to false)
            var withDeviceUser = true;  // bool? | When set to `true` and the managed object is a device, it deletes the associated device user (credentials). (optional)  (default to false)

            try
            {
                // Remove a specific managed object
                apiInstance.DeleteManagedObjectResource(id, xCumulocityProcessingMode, cascade, forceCascade, withDeviceUser);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ManagedObjectsApi.DeleteManagedObjectResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteManagedObjectResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove a specific managed object
    apiInstance.DeleteManagedObjectResourceWithHttpInfo(id, xCumulocityProcessingMode, cascade, forceCascade, withDeviceUser);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ManagedObjectsApi.DeleteManagedObjectResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |
| **cascade** | **bool?** | When set to &#x60;true&#x60; and the managed object is a device or group, all the hierarchy will be deleted. | [optional] [default to false] |
| **forceCascade** | **bool?** | When set to &#x60;true&#x60; all the hierarchy will be deleted without checking the type of managed object. It takes precedence over the parameter &#x60;cascade&#x60;. | [optional] [default to false] |
| **withDeviceUser** | **bool?** | When set to &#x60;true&#x60; and the managed object is a device, it deletes the associated device user (credentials). | [optional] [default to false] |

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
| **204** | A managed object was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |
| **409** | Conflict – The managed object is associated to other objects, for example child devices. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getlastavailabilitymanagedobjectresource"></a>
# **GetLastAvailabilityManagedObjectResource**
> DateTime GetLastAvailabilityManagedObjectResource (string id)

Retrieve the latest availability date of a specific managed object

Retrieve the date when a specific managed object (by a given ID) sent the last message to Cumulocity IoT.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetLastAvailabilityManagedObjectResourceExample
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

            var apiInstance = new ManagedObjectsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.

            try
            {
                // Retrieve the latest availability date of a specific managed object
                DateTime result = apiInstance.GetLastAvailabilityManagedObjectResource(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ManagedObjectsApi.GetLastAvailabilityManagedObjectResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetLastAvailabilityManagedObjectResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve the latest availability date of a specific managed object
    ApiResponse<DateTime> response = apiInstance.GetLastAvailabilityManagedObjectResourceWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ManagedObjectsApi.GetLastAvailabilityManagedObjectResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |

### Return type

**DateTime**

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the date is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getmanagedobjectcollectionresource"></a>
# **GetManagedObjectCollectionResource**
> ManagedObjectCollection GetManagedObjectCollectionResource (string? childAdditionId = null, string? childAssetId = null, string? childDeviceId = null, int? currentPage = null, string? fragmentType = null, List<string>? ids = null, bool? onlyRoots = null, string? owner = null, int? pageSize = null, string? q = null, string? query = null, bool? skipChildrenNames = null, string? text = null, string? type = null, bool? withChildren = null, bool? withChildrenCount = null, bool? withGroups = null, bool? withParents = null, bool? withTotalElements = null, bool? withTotalPages = null)

Retrieve all managed objects

Retrieve all managed objects (for example, devices, assets, etc.) registered in your tenant, or a subset based on queries. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetManagedObjectCollectionResourceExample
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

            var apiInstance = new ManagedObjectsApi(config);
            var childAdditionId = 3003;  // string? | Search for a specific child addition and list all the groups to which it belongs. (optional) 
            var childAssetId = 200;  // string? | Search for a specific child asset and list all the groups to which it belongs. (optional) 
            var childDeviceId = 2001;  // string? | Search for a specific child device and list all the groups to which it belongs. (optional) 
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var fragmentType = c8y_IsDevice;  // string? | A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional) 
            var ids = new List<string>?(); // List<string>? | The managed object IDs to search for. >**&#9432; Info:** If you query for multiple IDs at once, comma-separate the values.  (optional) 
            var onlyRoots = true;  // bool? | When set to `true` it returns managed objects which don't have any parent. If the current user doesn't have access to the parent, this is also root for the user. (optional)  (default to false)
            var owner = manga;  // string? | Username of the owner of the managed objects. (optional) 
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var q = $filter=(owner+eq+'manga');  // string? | Similar to the parameter `query`, but it assumes that this is a device query request and it adds automatically the search criteria `fragmentType=c8y_IsDevice`. (optional) 
            var query = $filter=(owner+eq+'manga');  // string? | Use query language to perform operations and/or filter the results. Details about the properties and supported operations can be found in [Query language](#tag/Query-language). (optional) 
            var skipChildrenNames = true;  // bool? | When set to `true`, the returned references of child devices won't contain their names. (optional)  (default to false)
            var text = my_value;  // string? | Search for managed objects where any property value is equal to the given one. Only string values are supported. (optional) 
            var type = c8y_DeviceGroup;  // string? | The type of managed object to search for. (optional) 
            var withChildren = false;  // bool? | Determines if children with ID and name should be returned when fetching the managed object. Set it to `false` to improve query performance. (optional)  (default to true)
            var withChildrenCount = true;  // bool? | When set to `true`, the returned result will contain the total number of children in the respective objects (`childAdditions`, `childAssets` and `childDevices`). (optional)  (default to false)
            var withGroups = true;  // bool? | When set to `true` it returns additional information about the groups to which the searched managed object belongs. This results in setting the `assetParents` property with additional information about the groups. (optional)  (default to false)
            var withParents = true;  // bool? | When set to `true`, the returned references of child parents will return the device's parents (if any). Otherwise, it will be an empty array. (optional)  (default to false)
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve all managed objects
                ManagedObjectCollection result = apiInstance.GetManagedObjectCollectionResource(childAdditionId, childAssetId, childDeviceId, currentPage, fragmentType, ids, onlyRoots, owner, pageSize, q, query, skipChildrenNames, text, type, withChildren, withChildrenCount, withGroups, withParents, withTotalElements, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ManagedObjectsApi.GetManagedObjectCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetManagedObjectCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all managed objects
    ApiResponse<ManagedObjectCollection> response = apiInstance.GetManagedObjectCollectionResourceWithHttpInfo(childAdditionId, childAssetId, childDeviceId, currentPage, fragmentType, ids, onlyRoots, owner, pageSize, q, query, skipChildrenNames, text, type, withChildren, withChildrenCount, withGroups, withParents, withTotalElements, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ManagedObjectsApi.GetManagedObjectCollectionResourceWithHttpInfo: " + e.Message);
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
| **fragmentType** | **string?** | A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. | [optional]  |
| **ids** | [**List&lt;string&gt;?**](string.md) | The managed object IDs to search for. &gt;**&amp;#9432; Info:** If you query for multiple IDs at once, comma-separate the values.  | [optional]  |
| **onlyRoots** | **bool?** | When set to &#x60;true&#x60; it returns managed objects which don&#39;t have any parent. If the current user doesn&#39;t have access to the parent, this is also root for the user. | [optional] [default to false] |
| **owner** | **string?** | Username of the owner of the managed objects. | [optional]  |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **q** | **string?** | Similar to the parameter &#x60;query&#x60;, but it assumes that this is a device query request and it adds automatically the search criteria &#x60;fragmentType&#x3D;c8y_IsDevice&#x60;. | [optional]  |
| **query** | **string?** | Use query language to perform operations and/or filter the results. Details about the properties and supported operations can be found in [Query language](#tag/Query-language). | [optional]  |
| **skipChildrenNames** | **bool?** | When set to &#x60;true&#x60;, the returned references of child devices won&#39;t contain their names. | [optional] [default to false] |
| **text** | **string?** | Search for managed objects where any property value is equal to the given one. Only string values are supported. | [optional]  |
| **type** | **string?** | The type of managed object to search for. | [optional]  |
| **withChildren** | **bool?** | Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. | [optional] [default to true] |
| **withChildrenCount** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). | [optional] [default to false] |
| **withGroups** | **bool?** | When set to &#x60;true&#x60; it returns additional information about the groups to which the searched managed object belongs. This results in setting the &#x60;assetParents&#x60; property with additional information about the groups. | [optional] [default to false] |
| **withParents** | **bool?** | When set to &#x60;true&#x60;, the returned references of child parents will return the device&#39;s parents (if any). Otherwise, it will be an empty array. | [optional] [default to false] |
| **withTotalElements** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**ManagedObjectCollection**](ManagedObjectCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.managedobjectcollection+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the collection of objects is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **422** | Invalid data was sent. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getmanagedobjectresource"></a>
# **GetManagedObjectResource**
> ManagedObject GetManagedObjectResource (string id, bool? skipChildrenNames = null, bool? withChildren = null, bool? withChildrenCount = null, bool? withParents = null)

Retrieve a specific managed object

Retrieve a specific managed object (for example, device, group, template) by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetManagedObjectResourceExample
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

            var apiInstance = new ManagedObjectsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var skipChildrenNames = true;  // bool? | When set to `true`, the returned references of child devices won't contain their names. (optional)  (default to false)
            var withChildren = false;  // bool? | Determines if children with ID and name should be returned when fetching the managed object. Set it to `false` to improve query performance. (optional)  (default to true)
            var withChildrenCount = true;  // bool? | When set to `true`, the returned result will contain the total number of children in the respective objects (`childAdditions`, `childAssets` and `childDevices`). (optional)  (default to false)
            var withParents = true;  // bool? | When set to `true`, the returned references of child parents will return the device's parents (if any). Otherwise, it will be an empty array. (optional)  (default to false)

            try
            {
                // Retrieve a specific managed object
                ManagedObject result = apiInstance.GetManagedObjectResource(id, skipChildrenNames, withChildren, withChildrenCount, withParents);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ManagedObjectsApi.GetManagedObjectResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetManagedObjectResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a specific managed object
    ApiResponse<ManagedObject> response = apiInstance.GetManagedObjectResourceWithHttpInfo(id, skipChildrenNames, withChildren, withChildrenCount, withParents);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ManagedObjectsApi.GetManagedObjectResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **skipChildrenNames** | **bool?** | When set to &#x60;true&#x60;, the returned references of child devices won&#39;t contain their names. | [optional] [default to false] |
| **withChildren** | **bool?** | Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. | [optional] [default to true] |
| **withChildrenCount** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). | [optional] [default to false] |
| **withParents** | **bool?** | When set to &#x60;true&#x60;, the returned references of child parents will return the device&#39;s parents (if any). Otherwise, it will be an empty array. | [optional] [default to false] |

### Return type

[**ManagedObject**](ManagedObject.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.managedobject+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the object is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getmanagedobjectuserresource"></a>
# **GetManagedObjectUserResource**
> ManagedObjectUser GetManagedObjectUserResource (string id)

Retrieve the username and state of a specific managed object

Retrieve the device owner's username and state (enabled or disabled) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetManagedObjectUserResourceExample
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

            var apiInstance = new ManagedObjectsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.

            try
            {
                // Retrieve the username and state of a specific managed object
                ManagedObjectUser result = apiInstance.GetManagedObjectUserResource(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ManagedObjectsApi.GetManagedObjectUserResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetManagedObjectUserResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve the username and state of a specific managed object
    ApiResponse<ManagedObjectUser> response = apiInstance.GetManagedObjectUserResourceWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ManagedObjectsApi.GetManagedObjectUserResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |

### Return type

[**ManagedObjectUser**](ManagedObjectUser.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.managedobjectuser+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the username and state are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsupportedmeasurementsmanagedobjectresource"></a>
# **GetSupportedMeasurementsManagedObjectResource**
> SupportedMeasurements GetSupportedMeasurementsManagedObjectResource (string id)

Retrieve all supported measurement fragments of a specific managed object

Retrieve all measurement types of a specific managed object by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetSupportedMeasurementsManagedObjectResourceExample
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

            var apiInstance = new ManagedObjectsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.

            try
            {
                // Retrieve all supported measurement fragments of a specific managed object
                SupportedMeasurements result = apiInstance.GetSupportedMeasurementsManagedObjectResource(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ManagedObjectsApi.GetSupportedMeasurementsManagedObjectResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetSupportedMeasurementsManagedObjectResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all supported measurement fragments of a specific managed object
    ApiResponse<SupportedMeasurements> response = apiInstance.GetSupportedMeasurementsManagedObjectResourceWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ManagedObjectsApi.GetSupportedMeasurementsManagedObjectResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |

### Return type

[**SupportedMeasurements**](SupportedMeasurements.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and all measurement types are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsupportedseriesmanagedobjectresource"></a>
# **GetSupportedSeriesManagedObjectResource**
> SupportedSeries GetSupportedSeriesManagedObjectResource (string id)

Retrieve all supported measurement fragments and series of a specific managed object

Retrieve all supported measurement fragments and series of a specific managed object by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetSupportedSeriesManagedObjectResourceExample
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

            var apiInstance = new ManagedObjectsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.

            try
            {
                // Retrieve all supported measurement fragments and series of a specific managed object
                SupportedSeries result = apiInstance.GetSupportedSeriesManagedObjectResource(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ManagedObjectsApi.GetSupportedSeriesManagedObjectResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetSupportedSeriesManagedObjectResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all supported measurement fragments and series of a specific managed object
    ApiResponse<SupportedSeries> response = apiInstance.GetSupportedSeriesManagedObjectResourceWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ManagedObjectsApi.GetSupportedSeriesManagedObjectResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |

### Return type

[**SupportedSeries**](SupportedSeries.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and all supported measurement series are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postmanagedobjectcollectionresource"></a>
# **PostManagedObjectCollectionResource**
> ManagedObject PostManagedObjectCollectionResource (Dictionary<string, Object> requestBody, string? accept = null, string? xCumulocityProcessingMode = null)

Create a managed object

Create a managed object, for example, a device with temperature measurements support or a binary switch.<br> In general, each managed object may consist of:  *  A unique identifier that references the object. *  The name of the object. *  The most specific type of the managed object. *  A time stamp showing the last update. *  Fragments with specific meanings, for example, `c8y_IsDevice`, `c8y_SupportedOperations`. *  Any additional custom fragments.  Imagine, for example, that you want to describe electric meters from different vendors. Depending on the make of the meter, one may have a relay and one may be capable to measure a single phase or three phases (for example, a three-phase electricity sensor). A fragment `c8y_ThreePhaseElectricitySensor` would identify such an electric meter. Devices' characteristics are identified by storing fragments for each of them.  > **&#9432; Info:** For more details about fragments with specific meanings, review the sections [Device management library](#section/Device-management-library) and [Sensor library](#section/Sensor-library).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> ROLE_INVENTORY_CREATE </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostManagedObjectCollectionResourceExample
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

            var apiInstance = new ManagedObjectsApi(config);
            var requestBody = new Dictionary<string, Object>(); // Dictionary<string, Object> | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Create a managed object
                ManagedObject result = apiInstance.PostManagedObjectCollectionResource(requestBody, accept, xCumulocityProcessingMode);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ManagedObjectsApi.PostManagedObjectCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostManagedObjectCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create a managed object
    ApiResponse<ManagedObject> response = apiInstance.PostManagedObjectCollectionResourceWithHttpInfo(requestBody, accept, xCumulocityProcessingMode);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ManagedObjectsApi.PostManagedObjectCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **requestBody** | [**Dictionary&lt;string, Object&gt;**](Object.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

[**ManagedObject**](ManagedObject.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.managedobject+json
 - **Accept**: application/vnd.com.nsn.cumulocity.managedobject+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | A managed object was created. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="putmanagedobjectresource"></a>
# **PutManagedObjectResource**
> ManagedObject PutManagedObjectResource (string id, Dictionary<string, Object> requestBody, string? accept = null, string? xCumulocityProcessingMode = null)

Update a specific managed object

Update a specific managed object (for example, device) by a given ID.  For example, if you want to specify that your managed object is a device, you must add the fragment `c8y_IsDevice`.   The endpoint can also be used as a device availability heartbeat. If you only specifiy the `id`, it updates the date when the last message was received and no other property. The response then only contains the `id` instead of the full managed object.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutManagedObjectResourceExample
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

            var apiInstance = new ManagedObjectsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var requestBody = new Dictionary<string, Object>(); // Dictionary<string, Object> | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Update a specific managed object
                ManagedObject result = apiInstance.PutManagedObjectResource(id, requestBody, accept, xCumulocityProcessingMode);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ManagedObjectsApi.PutManagedObjectResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutManagedObjectResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update a specific managed object
    ApiResponse<ManagedObject> response = apiInstance.PutManagedObjectResourceWithHttpInfo(id, requestBody, accept, xCumulocityProcessingMode);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ManagedObjectsApi.PutManagedObjectResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **requestBody** | [**Dictionary&lt;string, Object&gt;**](Object.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

[**ManagedObject**](ManagedObject.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.managedobject+json
 - **Accept**: application/vnd.com.nsn.cumulocity.managedobject+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A managed object was updated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="putmanagedobjectuserresource"></a>
# **PutManagedObjectUserResource**
> ManagedObjectUser PutManagedObjectUserResource (string id, ManagedObjectUser managedObjectUser, string? accept = null, string? xCumulocityProcessingMode = null)

Update the user's details of a specific managed object

Update the device owner's state (enabled or disabled) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutManagedObjectUserResourceExample
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

            var apiInstance = new ManagedObjectsApi(config);
            var id = 251982;  // string | Unique identifier of the managed object.
            var managedObjectUser = new ManagedObjectUser(); // ManagedObjectUser | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 
            var xCumulocityProcessingMode = PERSISTENT;  // string? | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional)  (default to PERSISTENT)

            try
            {
                // Update the user's details of a specific managed object
                ManagedObjectUser result = apiInstance.PutManagedObjectUserResource(id, managedObjectUser, accept, xCumulocityProcessingMode);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ManagedObjectsApi.PutManagedObjectUserResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutManagedObjectUserResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update the user's details of a specific managed object
    ApiResponse<ManagedObjectUser> response = apiInstance.PutManagedObjectUserResourceWithHttpInfo(id, managedObjectUser, accept, xCumulocityProcessingMode);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ManagedObjectsApi.PutManagedObjectUserResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | Unique identifier of the managed object. |  |
| **managedObjectUser** | [**ManagedObjectUser**](ManagedObjectUser.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |
| **xCumulocityProcessingMode** | **string?** | Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. | [optional] [default to PERSISTENT] |

### Return type

[**ManagedObjectUser**](ManagedObjectUser.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/vnd.com.nsn.cumulocity.managedobjectuser+json
 - **Accept**: application/vnd.com.nsn.cumulocity.managedobjectuser+json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The user&#39;s details of a specific managed object were updated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Managed object not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

