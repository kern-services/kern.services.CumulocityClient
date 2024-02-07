/*
 * Cumulocity IoT
 *
 * # REST implementation  This section describes the aspects common to all REST-based interfaces of Cumulocity IoT. The interfaces are based on the [Hypertext Transfer Protocol 1.1](https://tools.ietf.org/html/rfc2616) using [HTTPS](http://en.wikipedia.org/wiki/HTTP_Secure).  ## HTTP usage  ### Application management  Cumulocity IoT uses a so-called \"application key\" to distinguish requests coming from devices and traffic from applications. If you write an application, pass the following header as part of all requests:  ```markup X-Cumulocity-Application-Key: <APPLICATION_KEY> ```  For example, if you registered your application in the Cumulocity IoT Administration application with the key \"myapp\", your requests should contain the header:  ```markup X-Cumulocity-Application-Key: myapp ```  This makes your application subscribable and billable. If you implement a device, do not pass the key.  > **&#9432; Info:** Make sure that you pass the key in **all** requests coming from an application. If you leave out the key, > the request will be considered as a device request, and the corresponding device will be marked as \"available\".  ### Limited HTTP clients  If you use an HTTP client that can only perform GET and POST methods in HTTP, you can emulate the other methods through an additional \"X-HTTP-METHOD\" header. Simply issue a POST request and add the header, specifying the actual REST method to be executed. For example, to emulate the \"PUT\" (modify) method, you can use:  ```http POST ... X-HTTP-METHOD: PUT ```  ### Processing mode  Every update request (PUT, POST, DELETE) executes with a so-called *processing mode*. The processing modes are as follows:  |Processing mode|Description| |- --|- --| |PERSISTENT (default)|All updates will be send both to the Cumulocity IoT database and to real-time processing.| |TRANSIENT|Updates will be sent only to real-time processing. As part of real-time processing, the user can decide case by case through scripts whether updates should be stored to the database or not.| |QUIESCENT|The QUIESCENT processing mode behaves like the PERSISTENT processing mode with the exception that no real-time notifications will be sent. Currently, the QUIESCENT processing mode is applicable for measurements, events and managed objects.| |CEP| With the CEP processing mode, requests will only be processed by CEP or Apama. Currently, the CEP processing mode is applicable for measurements and events only.|  To explicitly control the processing mode of an update request, you can use the \"X-Cumulocity-Processing-Mode\" header with a value of either \"PERSISTENT\", \"TRANSIENT\", \"QUIESCENT\" or \"CEP\":  ```markup X-Cumulocity-Processing-Mode: PERSISTENT ```  > **&#9432; Info:** Events are always delivered to CEP/Apama for all processing modes. This is independent from real-time notifications.  ### Authorization  All requests issued to Cumulocity IoT are subject to authorization. To determine the required permissions, see the \"Required role\" entries for the individual requests. To learn more about the different permissions and the concept of ownership in Cumulocity IoT, see [Security aspects > Managing roles and assigning permissions](https://cumulocity.com/guides/concepts/security/#managing-roles-and-assigning-permissions)\".  ### Media types  Each type of data is associated with an own media type. The general format of media types is:  ```markup application/vnd.com.nsn.cumulocity.<TYPE>+json;ver=<VERSION>;charset=UTF-8 ```  Each media type contains a parameter `ver` indicating the version of the type. At the time of writing, the latest version is \"0.9\". As an example, the media type for an error message in the current version is:  ```markup application/vnd.com.nsn.cumulocity.error+json;ver=0.9;charset=UTF-8 ```  Media types are used in HTTP \"Content-Type\" and \"Accept\" headers. If you specify an \"Accept\" header in a POST or PUT request, the response will contain the newly created or updated object. If you do not specify the header, the response body will be empty.  If a media type without the `ver` parameter is given, the oldest available version will be returned by the server. If the \"Accept\" header contains the same media type in multiple versions, the server will return a representation in the latest supported version.  Note that media type values should be treated as case insensitive.  ### Date format  Data exchanged with Cumulocity IoT in HTTP requests and responses is encoded in [JSON format](http://www.ietf.org/rfc/rfc4627.txt) and [UTF-8](http://en.wikipedia.org/wiki/UTF-8) character encoding. Timestamps and dates are accepted and emitted by Cumulocity IoT in [ISO 8601](http://www.w3.org/TR/NOTE-datetime) format:  ```markup Date: YYYY-MM-DD Time: hh:mm:ss±hh:mm Timestamp: YYYY-MM-DDThh:mm:ss±hh:mm ```  To avoid ambiguity, all times and timestamps must include timezone information. Please take into account that the plus character \"+\" must be encoded as \"%2B\".  ### Response Codes  Cumulocity IoT uses conventional HTTP response codes to indicate the success or failure of an API request. Codes in the `2xx` range indicate success. Codes in the `4xx` range indicate a user error. The response provides information on why the request failed (for example, a required parameter was omitted). Codes in the `5xx` range indicate an error with Cumulocity IoT's servers ([these are very rare](https://www.softwareag.cloud/site/sla/cumulocity-iot.html#availability)).  #### HTTP status code summary  |Code|Message|Description| |:- --:|:- --|:- --| |200|OK|Everything worked as expected.| |201|Created|A managed object was created.| |204|No content|An object was removed.| |400|Bad Request|The request was unacceptable, often due to missing a required parameter.| |401|Unauthorized|Authentication has failed, or credentials were required but not provided.| |403|Forbidden|The authenticated user doesn't have permissions to perform the request.| |404|Not Found|The requested resource doesn't exist.| |405|Method not allowed|The employed HTTP method cannot be used on this resource (for example, using PUT on a read-only resource).| |409|Conflict| The data is correct but it breaks some constraints (for example, application version limit is exceeded). | |422|Invalid data| Invalid data was sent on the request and/or a query could not be understood.                             | |422|Unprocessable Entity| The requested resource cannot be updated or mandatory fields are missing on the executed operation.      | |500<br>503|Server Errors| Something went wrong on Cumulocity IoT's end.                                                            |  ## REST usage  ### Interpretation of HTTP verbs  The semantics described in the [HTTP specification](http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html#sec9) are used:  * POST creates a new resource. In the response \"Location\" header, the URI of the newly created resource is returned. * GET retrieves a resource. * PUT updates an existing resource with the contents of the request. * DELETE removes a resource. The response will be \"204 No Content\".  If a PUT request only contains parts of a resource (also known as fragments), only those parts are updated. To remove such a part, use a PUT request with a null value for it:  ```json {   \"resourcePartName\": null } ```  > **&#9432; Info:** A PUT request cannot update sub-resources that are identified by a separate URI.  ### URI space and URI templates  Clients should not make assumptions on the layout of URIs used in requests, but construct URIs from previously returned URIs or URI templates. The [root interface](#tag/Platform-API) provides the entry point for clients.  URI templates contain placeholders in curly braces (for example, `{type}`), which must be filled by the client to produce a URI. As an example, see the following excerpt from the event API response:  ```json {   \"events\": {       \"self\": \"https://<TENANT_DOMAIN>/event\"   },   \"eventsForSourceAndType\": \"https://<TENANT_DOMAIN>/event/events?type={type}&source={source}\" } ```  The client must fill the `{type}` and `{source}` placeholders with the desired type and source devices of the events to be returned. The meaning of these placeholders is documented in the respective interface descriptions.  ### Interface structure  In general, Cumulocity IoT REST resources are modeled according to the following pattern:  * The starting point are API resources, which will provide access to the actual data through URIs and URI templates to collection resources. For example, the above event API resource provides the `events` URI and the `eventsForSourceAndType` URI to access collections of events. * Collection resources aggregate member resources and allow creating new member resources in the collection. For example, through the `events` collection resource, new events can be created. * Finally, individual resources can be edited.  #### Query result paging  Collection resources support paging of data to avoid passing huge data volumes in one block from client to server. GET requests to collections accept two query parameters:  * `currentPage` defines the slice of data to be returned, starting with 1. By default, the first page is returned. * `pageSize` indicates how many entries of the collection should be returned. By default, 5 entries are returned. The upper limit for one page is currently 2,000 documents. Any larger requested page size is trimmed to the upper limit. * `withTotalElements` will yield the total number of elements in the statistics object. This is only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). * `withTotalPages` will yield the total number of pages in the statistics object. This is only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)).  For convenience, collection resources provide `next` and `prev` links to retrieve the next and previous pages of the results. The following is an example response for managed object collections (the contents of the array `managedObjects` have been omitted):  ```json {   \"self\" : \"https://<TENANT_DOMAIN>/inventory/managedObjects?pageSize=5&currentPage=2\",   \"managedObjects\" : [...],   \"statistics\" : {     \"totalPages\" : 7,     \"pageSize\" : 5,     \"currentPage\" : 2,     \"totalElements\" : 34   },   \"prev\" : \"https://<TENANT_DOMAIN>/inventory/managedObjects?pageSize=5&currentPage=1\",   \"next\" : \"https://<TENANT_DOMAIN>/inventory/managedObjects?pageSize=5&currentPage=3\" } ```  The `totalPages` and `totalElements` properties can be expensive to compute, hence they are not returned by default for [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). To include any of them in the result, add the query parameters `withTotalPages=true` and/or `withTotalElements=true`.  > **&#9432; Info:** If inventory roles are applied to a user, a query by the user may return less than `pageSize` results even if there are more results in total.  #### Query result paging for users with restricted access  If a user does not have a global role for reading data from the API resource but rather has [inventory roles](https://cumulocity.com/guides/users-guide/administration/#inventory) for reading only particular documents, there are some differences in query result paging:  * In some circumstances the response may contain less than `pageSize` and `totalElements` elements though there is more data in the database accessible for the user. * In some circumstances `next` and `prev` links may appear in the response though there is no more data in the database accessible for the user. * The property `currentPage` of the response does not contain the page number but the offset of the next element not yet processed by the querying mechanism. * The query parameters `withTotalPages=true` and `withTotalElements=true` have no effect, and the value of the `totalPages` and `totalElements` properties is always null.  The above behavior results from the fact that the querying mechanism is iterating maximally over 10 * max(pageSize, 100) documents per request, and it stops even though the full page of data accessible for the user could not be collected. When the next page is requested the querying mechanism starts the iteration where it completed the previous time.  #### Query result by time interval  Use the following query parameters to obtain data for a specified time interval:  * `dateFrom` - Start date or date and time. * `dateTo` - End date or date and time.  Example formats:  ```markup dateTo=2019-04-20 dateTo=2019-04-20T08:30:00.000Z ```  Parameters are optional. Values provided with those parameters are inclusive.  > **⚠️ Important:** If your servers are not running in UTC (Coordinated Universal Time), any date passed without timezone will be handled as UTC, regardless of the server local timezone. This might lead to a difference regarding the date/time range included in the results.  ### Root interface  To discover the URIs to the various interfaces of Cumulocity IoT, it provides a \"root\" interface. This root interface aggregates all the underlying API resources. See the [Platform API](#tag/Platform-API) endpoint. For more information on the different API resources, consult the respective API sections.  ## Generic media types  ### Error  The error type provides further information on the reason of a failed request.  Content-Type: application/vnd.com.nsn.cumulocity.error+json  |Name|Type|Description| |- --|- --|- --| |error|string|Error type formatted as `<RESOURCE_TYPE>/<ERROR_NAME>`. For example, an object not found in the inventory is reported as `inventory/notFound`.| |info|string|URL to an error description on the Internet.| |message|string|Short text description of the error|  ### Paging statistics  Paging statistics for collection of resources.  Content-Type: application/vnd.com.nsn.cumulocity.pagingstatistics+json  |Name|Type|Description| |- --|- --|- --| |currentPage|integer|The current returned page within the full result set, starting at \"1\".| |pageSize|integer|Maximum number of records contained in this query.| |totalElements|integer|The total number of results (elements).| |totalPages|integer|The total number of paginated results (pages).|  > **&#9432; Info:** The `totalPages` and `totalElements` properties are not returned by default in the response. To include any of them, add the query parameters `withTotalPages=true` and/or `withTotalElements=true`. Be aware of [differences in query result paging for users with restricted access](#query-result-paging-for-users-with-restricted-access).  > **&#9432; Info:** To improve performance, the `totalPages` and `totalElements` statistics are cached for 10 seconds.  # Device management library  The device management library has moved. Visit the [device management library](https://cumulocity.com/guides/reference/device-management-library/#overview) in the *Reference guide*.  # Sensor library  The sensor library has moved. Visit the [sensor library](https://cumulocity.com/guides/reference/sensor-library/#overview) in the *Reference guide*.  # Login options  When you sign up for an account on the [Cumulocity IoT platform](https://cumulocity.com/), for example, by using a free trial, you will be provided with a dedicated URL address for your tenant. All requests to the platform must be authenticated employing your tenant ID, Cumulocity IoT user (c8yuser for short) and password. Cumulocity IoT offers the following forms of authentication:  * Basic authentication (Basic) * OAI-Secure authentication (OAI-Secure) * SSO with authentication code grant (SSO) * JSON Web Token authentication (JWT, deprecated)  You can check your login options with a GET call to the endpoint <kbd><a href=\"#tag/Login-options\">/tenant/loginOptions</a></kbd>. 
 *
 * The version of the OpenAPI document: Release 10.15.0
 * 
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using RestSharp;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace kern.services.CumulocityClient.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IManagedObjectsApi : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Remove a specific managed object
        /// </summary>
        /// <remarks>
        /// Remove a specific managed object (for example, device) by a given ID.  > **&#9432; Info:** Inventory DELETE requests are not synchronous. The response could be returned before the delete request has been completed. This may happen especially when the deleted managed object has a lot of associated data. After sending the request, the platform starts deleting the associated data in an asynchronous way. Finally, the requested managed object is deleted after all associated data has been deleted.  > **&#9432; Info:** By default, the delete operation is always propagated to the subgroups, but only if the deleted object is a group.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cascade">When set to &#x60;true&#x60; and the managed object is a device or group, all the hierarchy will be deleted. (optional, default to false)</param>
        /// <param name="forceCascade">When set to &#x60;true&#x60; all the hierarchy will be deleted without checking the type of managed object. It takes precedence over the parameter &#x60;cascade&#x60;. (optional, default to false)</param>
        /// <param name="withDeviceUser">When set to &#x60;true&#x60; and the managed object is a device, it deletes the associated device user (credentials). (optional, default to false)</param>
        /// <returns></returns>
        void DeleteManagedObjectResource (string id, string xCumulocityProcessingMode = default(string), bool? cascade = default(bool?), bool? forceCascade = default(bool?), bool? withDeviceUser = default(bool?));

        /// <summary>
        /// Remove a specific managed object
        /// </summary>
        /// <remarks>
        /// Remove a specific managed object (for example, device) by a given ID.  > **&#9432; Info:** Inventory DELETE requests are not synchronous. The response could be returned before the delete request has been completed. This may happen especially when the deleted managed object has a lot of associated data. After sending the request, the platform starts deleting the associated data in an asynchronous way. Finally, the requested managed object is deleted after all associated data has been deleted.  > **&#9432; Info:** By default, the delete operation is always propagated to the subgroups, but only if the deleted object is a group.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cascade">When set to &#x60;true&#x60; and the managed object is a device or group, all the hierarchy will be deleted. (optional, default to false)</param>
        /// <param name="forceCascade">When set to &#x60;true&#x60; all the hierarchy will be deleted without checking the type of managed object. It takes precedence over the parameter &#x60;cascade&#x60;. (optional, default to false)</param>
        /// <param name="withDeviceUser">When set to &#x60;true&#x60; and the managed object is a device, it deletes the associated device user (credentials). (optional, default to false)</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DeleteManagedObjectResourceWithHttpInfo (string id, string xCumulocityProcessingMode = default(string), bool? cascade = default(bool?), bool? forceCascade = default(bool?), bool? withDeviceUser = default(bool?));
        /// <summary>
        /// Retrieve the latest availability date of a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve the date when a specific managed object (by a given ID) sent the last message to Cumulocity IoT.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <returns>DateTime</returns>
        DateTime GetLastAvailabilityManagedObjectResource (string id);

        /// <summary>
        /// Retrieve the latest availability date of a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve the date when a specific managed object (by a given ID) sent the last message to Cumulocity IoT.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <returns>ApiResponse of DateTime</returns>
        ApiResponse<DateTime> GetLastAvailabilityManagedObjectResourceWithHttpInfo (string id);
        /// <summary>
        /// Retrieve all managed objects
        /// </summary>
        /// <remarks>
        /// Retrieve all managed objects (for example, devices, assets, etc.) registered in your tenant, or a subset based on queries. 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="childAdditionId">Search for a specific child addition and list all the groups to which it belongs. (optional)</param>
        /// <param name="childAssetId">Search for a specific child asset and list all the groups to which it belongs. (optional)</param>
        /// <param name="childDeviceId">Search for a specific child device and list all the groups to which it belongs. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="fragmentType">A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional)</param>
        /// <param name="ids">The managed object IDs to search for. &gt;**&amp;#9432; Info:** If you query for multiple IDs at once, comma-separate the values.  (optional)</param>
        /// <param name="onlyRoots">When set to &#x60;true&#x60; it returns managed objects which don&#39;t have any parent. If the current user doesn&#39;t have access to the parent, this is also root for the user. (optional, default to false)</param>
        /// <param name="owner">Username of the owner of the managed objects. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="q">Similar to the parameter &#x60;query&#x60;, but it assumes that this is a device query request and it adds automatically the search criteria &#x60;fragmentType&#x3D;c8y_IsDevice&#x60;. (optional)</param>
        /// <param name="query">Use query language to perform operations and/or filter the results. Details about the properties and supported operations can be found in [Query language](#tag/Query-language). (optional)</param>
        /// <param name="skipChildrenNames">When set to &#x60;true&#x60;, the returned references of child devices won&#39;t contain their names. (optional, default to false)</param>
        /// <param name="text">Search for managed objects where any property value is equal to the given one. Only string values are supported. (optional)</param>
        /// <param name="type">The type of managed object to search for. (optional)</param>
        /// <param name="withChildren">Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. (optional, default to true)</param>
        /// <param name="withChildrenCount">When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). (optional, default to false)</param>
        /// <param name="withGroups">When set to &#x60;true&#x60; it returns additional information about the groups to which the searched managed object belongs. This results in setting the &#x60;assetParents&#x60; property with additional information about the groups. (optional, default to false)</param>
        /// <param name="withParents">When set to &#x60;true&#x60;, the returned references of child parents will return the device&#39;s parents (if any). Otherwise, it will be an empty array. (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <returns>ManagedObjectCollection</returns>
        ManagedObjectCollection GetManagedObjectCollectionResource (string childAdditionId = default(string), string childAssetId = default(string), string childDeviceId = default(string), int? currentPage = default(int?), string fragmentType = default(string), List<string> ids = default(List<string>), bool? onlyRoots = default(bool?), string owner = default(string), int? pageSize = default(int?), string q = default(string), string query = default(string), bool? skipChildrenNames = default(bool?), string text = default(string), string type = default(string), bool? withChildren = default(bool?), bool? withChildrenCount = default(bool?), bool? withGroups = default(bool?), bool? withParents = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?));

        /// <summary>
        /// Retrieve all managed objects
        /// </summary>
        /// <remarks>
        /// Retrieve all managed objects (for example, devices, assets, etc.) registered in your tenant, or a subset based on queries. 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="childAdditionId">Search for a specific child addition and list all the groups to which it belongs. (optional)</param>
        /// <param name="childAssetId">Search for a specific child asset and list all the groups to which it belongs. (optional)</param>
        /// <param name="childDeviceId">Search for a specific child device and list all the groups to which it belongs. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="fragmentType">A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional)</param>
        /// <param name="ids">The managed object IDs to search for. &gt;**&amp;#9432; Info:** If you query for multiple IDs at once, comma-separate the values.  (optional)</param>
        /// <param name="onlyRoots">When set to &#x60;true&#x60; it returns managed objects which don&#39;t have any parent. If the current user doesn&#39;t have access to the parent, this is also root for the user. (optional, default to false)</param>
        /// <param name="owner">Username of the owner of the managed objects. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="q">Similar to the parameter &#x60;query&#x60;, but it assumes that this is a device query request and it adds automatically the search criteria &#x60;fragmentType&#x3D;c8y_IsDevice&#x60;. (optional)</param>
        /// <param name="query">Use query language to perform operations and/or filter the results. Details about the properties and supported operations can be found in [Query language](#tag/Query-language). (optional)</param>
        /// <param name="skipChildrenNames">When set to &#x60;true&#x60;, the returned references of child devices won&#39;t contain their names. (optional, default to false)</param>
        /// <param name="text">Search for managed objects where any property value is equal to the given one. Only string values are supported. (optional)</param>
        /// <param name="type">The type of managed object to search for. (optional)</param>
        /// <param name="withChildren">Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. (optional, default to true)</param>
        /// <param name="withChildrenCount">When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). (optional, default to false)</param>
        /// <param name="withGroups">When set to &#x60;true&#x60; it returns additional information about the groups to which the searched managed object belongs. This results in setting the &#x60;assetParents&#x60; property with additional information about the groups. (optional, default to false)</param>
        /// <param name="withParents">When set to &#x60;true&#x60;, the returned references of child parents will return the device&#39;s parents (if any). Otherwise, it will be an empty array. (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <returns>ApiResponse of ManagedObjectCollection</returns>
        ApiResponse<ManagedObjectCollection> GetManagedObjectCollectionResourceWithHttpInfo (string childAdditionId = default(string), string childAssetId = default(string), string childDeviceId = default(string), int? currentPage = default(int?), string fragmentType = default(string), List<string> ids = default(List<string>), bool? onlyRoots = default(bool?), string owner = default(string), int? pageSize = default(int?), string q = default(string), string query = default(string), bool? skipChildrenNames = default(bool?), string text = default(string), string type = default(string), bool? withChildren = default(bool?), bool? withChildrenCount = default(bool?), bool? withGroups = default(bool?), bool? withParents = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?));
        /// <summary>
        /// Retrieve a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve a specific managed object (for example, device, group, template) by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="skipChildrenNames">When set to &#x60;true&#x60;, the returned references of child devices won&#39;t contain their names. (optional, default to false)</param>
        /// <param name="withChildren">Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. (optional, default to true)</param>
        /// <param name="withChildrenCount">When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). (optional, default to false)</param>
        /// <param name="withParents">When set to &#x60;true&#x60;, the returned references of child parents will return the device&#39;s parents (if any). Otherwise, it will be an empty array. (optional, default to false)</param>
        /// <returns>ManagedObject</returns>
        ManagedObject GetManagedObjectResource (string id, bool? skipChildrenNames = default(bool?), bool? withChildren = default(bool?), bool? withChildrenCount = default(bool?), bool? withParents = default(bool?));

        /// <summary>
        /// Retrieve a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve a specific managed object (for example, device, group, template) by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="skipChildrenNames">When set to &#x60;true&#x60;, the returned references of child devices won&#39;t contain their names. (optional, default to false)</param>
        /// <param name="withChildren">Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. (optional, default to true)</param>
        /// <param name="withChildrenCount">When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). (optional, default to false)</param>
        /// <param name="withParents">When set to &#x60;true&#x60;, the returned references of child parents will return the device&#39;s parents (if any). Otherwise, it will be an empty array. (optional, default to false)</param>
        /// <returns>ApiResponse of ManagedObject</returns>
        ApiResponse<ManagedObject> GetManagedObjectResourceWithHttpInfo (string id, bool? skipChildrenNames = default(bool?), bool? withChildren = default(bool?), bool? withChildrenCount = default(bool?), bool? withParents = default(bool?));
        /// <summary>
        /// Retrieve the username and state of a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve the device owner's username and state (enabled or disabled) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <returns>ManagedObjectUser</returns>
        ManagedObjectUser GetManagedObjectUserResource (string id);

        /// <summary>
        /// Retrieve the username and state of a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve the device owner's username and state (enabled or disabled) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <returns>ApiResponse of ManagedObjectUser</returns>
        ApiResponse<ManagedObjectUser> GetManagedObjectUserResourceWithHttpInfo (string id);
        /// <summary>
        /// Retrieve all supported measurement fragments of a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve all measurement types of a specific managed object by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <returns>SupportedMeasurements</returns>
        SupportedMeasurements GetSupportedMeasurementsManagedObjectResource (string id);

        /// <summary>
        /// Retrieve all supported measurement fragments of a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve all measurement types of a specific managed object by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <returns>ApiResponse of SupportedMeasurements</returns>
        ApiResponse<SupportedMeasurements> GetSupportedMeasurementsManagedObjectResourceWithHttpInfo (string id);
        /// <summary>
        /// Retrieve all supported measurement fragments and series of a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve all supported measurement fragments and series of a specific managed object by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <returns>SupportedSeries</returns>
        SupportedSeries GetSupportedSeriesManagedObjectResource (string id);

        /// <summary>
        /// Retrieve all supported measurement fragments and series of a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve all supported measurement fragments and series of a specific managed object by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <returns>ApiResponse of SupportedSeries</returns>
        ApiResponse<SupportedSeries> GetSupportedSeriesManagedObjectResourceWithHttpInfo (string id);
        /// <summary>
        /// Create a managed object
        /// </summary>
        /// <remarks>
        /// Create a managed object, for example, a device with temperature measurements support or a binary switch.<br> In general, each managed object may consist of:  *  A unique identifier that references the object. *  The name of the object. *  The most specific type of the managed object. *  A time stamp showing the last update. *  Fragments with specific meanings, for example, `c8y_IsDevice`, `c8y_SupportedOperations`. *  Any additional custom fragments.  Imagine, for example, that you want to describe electric meters from different vendors. Depending on the make of the meter, one may have a relay and one may be capable to measure a single phase or three phases (for example, a three-phase electricity sensor). A fragment `c8y_ThreePhaseElectricitySensor` would identify such an electric meter. Devices' characteristics are identified by storing fragments for each of them.  > **&#9432; Info:** For more details about fragments with specific meanings, review the sections [Device management library](#section/Device-management-library) and [Sensor library](#section/Sensor-library).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> ROLE_INVENTORY_CREATE </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="requestBody"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ManagedObject</returns>
        ManagedObject PostManagedObjectCollectionResource (Dictionary<string, Object> requestBody, string accept = default(string), string xCumulocityProcessingMode = default(string));

        /// <summary>
        /// Create a managed object
        /// </summary>
        /// <remarks>
        /// Create a managed object, for example, a device with temperature measurements support or a binary switch.<br> In general, each managed object may consist of:  *  A unique identifier that references the object. *  The name of the object. *  The most specific type of the managed object. *  A time stamp showing the last update. *  Fragments with specific meanings, for example, `c8y_IsDevice`, `c8y_SupportedOperations`. *  Any additional custom fragments.  Imagine, for example, that you want to describe electric meters from different vendors. Depending on the make of the meter, one may have a relay and one may be capable to measure a single phase or three phases (for example, a three-phase electricity sensor). A fragment `c8y_ThreePhaseElectricitySensor` would identify such an electric meter. Devices' characteristics are identified by storing fragments for each of them.  > **&#9432; Info:** For more details about fragments with specific meanings, review the sections [Device management library](#section/Device-management-library) and [Sensor library](#section/Sensor-library).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> ROLE_INVENTORY_CREATE </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="requestBody"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ApiResponse of ManagedObject</returns>
        ApiResponse<ManagedObject> PostManagedObjectCollectionResourceWithHttpInfo (Dictionary<string, Object> requestBody, string accept = default(string), string xCumulocityProcessingMode = default(string));
        /// <summary>
        /// Update a specific managed object
        /// </summary>
        /// <remarks>
        /// Update a specific managed object (for example, device) by a given ID.  For example, if you want to specify that your managed object is a device, you must add the fragment `c8y_IsDevice`.   The endpoint can also be used as a device availability heartbeat. If you only specifiy the `id`, it updates the date when the last message was received and no other property. The response then only contains the `id` instead of the full managed object.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="requestBody"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ManagedObject</returns>
        ManagedObject PutManagedObjectResource (string id, Dictionary<string, Object> requestBody, string accept = default(string), string xCumulocityProcessingMode = default(string));

        /// <summary>
        /// Update a specific managed object
        /// </summary>
        /// <remarks>
        /// Update a specific managed object (for example, device) by a given ID.  For example, if you want to specify that your managed object is a device, you must add the fragment `c8y_IsDevice`.   The endpoint can also be used as a device availability heartbeat. If you only specifiy the `id`, it updates the date when the last message was received and no other property. The response then only contains the `id` instead of the full managed object.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="requestBody"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ApiResponse of ManagedObject</returns>
        ApiResponse<ManagedObject> PutManagedObjectResourceWithHttpInfo (string id, Dictionary<string, Object> requestBody, string accept = default(string), string xCumulocityProcessingMode = default(string));
        /// <summary>
        /// Update the user's details of a specific managed object
        /// </summary>
        /// <remarks>
        /// Update the device owner's state (enabled or disabled) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="managedObjectUser"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ManagedObjectUser</returns>
        ManagedObjectUser PutManagedObjectUserResource (string id, ManagedObjectUser managedObjectUser, string accept = default(string), string xCumulocityProcessingMode = default(string));

        /// <summary>
        /// Update the user's details of a specific managed object
        /// </summary>
        /// <remarks>
        /// Update the device owner's state (enabled or disabled) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="managedObjectUser"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ApiResponse of ManagedObjectUser</returns>
        ApiResponse<ManagedObjectUser> PutManagedObjectUserResourceWithHttpInfo (string id, ManagedObjectUser managedObjectUser, string accept = default(string), string xCumulocityProcessingMode = default(string));
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Remove a specific managed object
        /// </summary>
        /// <remarks>
        /// Remove a specific managed object (for example, device) by a given ID.  > **&#9432; Info:** Inventory DELETE requests are not synchronous. The response could be returned before the delete request has been completed. This may happen especially when the deleted managed object has a lot of associated data. After sending the request, the platform starts deleting the associated data in an asynchronous way. Finally, the requested managed object is deleted after all associated data has been deleted.  > **&#9432; Info:** By default, the delete operation is always propagated to the subgroups, but only if the deleted object is a group.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cascade">When set to &#x60;true&#x60; and the managed object is a device or group, all the hierarchy will be deleted. (optional, default to false)</param>
        /// <param name="forceCascade">When set to &#x60;true&#x60; all the hierarchy will be deleted without checking the type of managed object. It takes precedence over the parameter &#x60;cascade&#x60;. (optional, default to false)</param>
        /// <param name="withDeviceUser">When set to &#x60;true&#x60; and the managed object is a device, it deletes the associated device user (credentials). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DeleteManagedObjectResourceAsync (string id, string xCumulocityProcessingMode = default(string), bool? cascade = default(bool?), bool? forceCascade = default(bool?), bool? withDeviceUser = default(bool?), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Remove a specific managed object
        /// </summary>
        /// <remarks>
        /// Remove a specific managed object (for example, device) by a given ID.  > **&#9432; Info:** Inventory DELETE requests are not synchronous. The response could be returned before the delete request has been completed. This may happen especially when the deleted managed object has a lot of associated data. After sending the request, the platform starts deleting the associated data in an asynchronous way. Finally, the requested managed object is deleted after all associated data has been deleted.  > **&#9432; Info:** By default, the delete operation is always propagated to the subgroups, but only if the deleted object is a group.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cascade">When set to &#x60;true&#x60; and the managed object is a device or group, all the hierarchy will be deleted. (optional, default to false)</param>
        /// <param name="forceCascade">When set to &#x60;true&#x60; all the hierarchy will be deleted without checking the type of managed object. It takes precedence over the parameter &#x60;cascade&#x60;. (optional, default to false)</param>
        /// <param name="withDeviceUser">When set to &#x60;true&#x60; and the managed object is a device, it deletes the associated device user (credentials). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DeleteManagedObjectResourceWithHttpInfoAsync (string id, string xCumulocityProcessingMode = default(string), bool? cascade = default(bool?), bool? forceCascade = default(bool?), bool? withDeviceUser = default(bool?), CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Retrieve the latest availability date of a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve the date when a specific managed object (by a given ID) sent the last message to Cumulocity IoT.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of DateTime</returns>
        System.Threading.Tasks.Task<DateTime> GetLastAvailabilityManagedObjectResourceAsync (string id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieve the latest availability date of a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve the date when a specific managed object (by a given ID) sent the last message to Cumulocity IoT.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (DateTime)</returns>
        System.Threading.Tasks.Task<ApiResponse<DateTime>> GetLastAvailabilityManagedObjectResourceWithHttpInfoAsync (string id, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Retrieve all managed objects
        /// </summary>
        /// <remarks>
        /// Retrieve all managed objects (for example, devices, assets, etc.) registered in your tenant, or a subset based on queries. 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="childAdditionId">Search for a specific child addition and list all the groups to which it belongs. (optional)</param>
        /// <param name="childAssetId">Search for a specific child asset and list all the groups to which it belongs. (optional)</param>
        /// <param name="childDeviceId">Search for a specific child device and list all the groups to which it belongs. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="fragmentType">A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional)</param>
        /// <param name="ids">The managed object IDs to search for. &gt;**&amp;#9432; Info:** If you query for multiple IDs at once, comma-separate the values.  (optional)</param>
        /// <param name="onlyRoots">When set to &#x60;true&#x60; it returns managed objects which don&#39;t have any parent. If the current user doesn&#39;t have access to the parent, this is also root for the user. (optional, default to false)</param>
        /// <param name="owner">Username of the owner of the managed objects. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="q">Similar to the parameter &#x60;query&#x60;, but it assumes that this is a device query request and it adds automatically the search criteria &#x60;fragmentType&#x3D;c8y_IsDevice&#x60;. (optional)</param>
        /// <param name="query">Use query language to perform operations and/or filter the results. Details about the properties and supported operations can be found in [Query language](#tag/Query-language). (optional)</param>
        /// <param name="skipChildrenNames">When set to &#x60;true&#x60;, the returned references of child devices won&#39;t contain their names. (optional, default to false)</param>
        /// <param name="text">Search for managed objects where any property value is equal to the given one. Only string values are supported. (optional)</param>
        /// <param name="type">The type of managed object to search for. (optional)</param>
        /// <param name="withChildren">Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. (optional, default to true)</param>
        /// <param name="withChildrenCount">When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). (optional, default to false)</param>
        /// <param name="withGroups">When set to &#x60;true&#x60; it returns additional information about the groups to which the searched managed object belongs. This results in setting the &#x60;assetParents&#x60; property with additional information about the groups. (optional, default to false)</param>
        /// <param name="withParents">When set to &#x60;true&#x60;, the returned references of child parents will return the device&#39;s parents (if any). Otherwise, it will be an empty array. (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ManagedObjectCollection</returns>
        System.Threading.Tasks.Task<ManagedObjectCollection> GetManagedObjectCollectionResourceAsync (string childAdditionId = default(string), string childAssetId = default(string), string childDeviceId = default(string), int? currentPage = default(int?), string fragmentType = default(string), List<string> ids = default(List<string>), bool? onlyRoots = default(bool?), string owner = default(string), int? pageSize = default(int?), string q = default(string), string query = default(string), bool? skipChildrenNames = default(bool?), string text = default(string), string type = default(string), bool? withChildren = default(bool?), bool? withChildrenCount = default(bool?), bool? withGroups = default(bool?), bool? withParents = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieve all managed objects
        /// </summary>
        /// <remarks>
        /// Retrieve all managed objects (for example, devices, assets, etc.) registered in your tenant, or a subset based on queries. 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="childAdditionId">Search for a specific child addition and list all the groups to which it belongs. (optional)</param>
        /// <param name="childAssetId">Search for a specific child asset and list all the groups to which it belongs. (optional)</param>
        /// <param name="childDeviceId">Search for a specific child device and list all the groups to which it belongs. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="fragmentType">A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional)</param>
        /// <param name="ids">The managed object IDs to search for. &gt;**&amp;#9432; Info:** If you query for multiple IDs at once, comma-separate the values.  (optional)</param>
        /// <param name="onlyRoots">When set to &#x60;true&#x60; it returns managed objects which don&#39;t have any parent. If the current user doesn&#39;t have access to the parent, this is also root for the user. (optional, default to false)</param>
        /// <param name="owner">Username of the owner of the managed objects. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="q">Similar to the parameter &#x60;query&#x60;, but it assumes that this is a device query request and it adds automatically the search criteria &#x60;fragmentType&#x3D;c8y_IsDevice&#x60;. (optional)</param>
        /// <param name="query">Use query language to perform operations and/or filter the results. Details about the properties and supported operations can be found in [Query language](#tag/Query-language). (optional)</param>
        /// <param name="skipChildrenNames">When set to &#x60;true&#x60;, the returned references of child devices won&#39;t contain their names. (optional, default to false)</param>
        /// <param name="text">Search for managed objects where any property value is equal to the given one. Only string values are supported. (optional)</param>
        /// <param name="type">The type of managed object to search for. (optional)</param>
        /// <param name="withChildren">Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. (optional, default to true)</param>
        /// <param name="withChildrenCount">When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). (optional, default to false)</param>
        /// <param name="withGroups">When set to &#x60;true&#x60; it returns additional information about the groups to which the searched managed object belongs. This results in setting the &#x60;assetParents&#x60; property with additional information about the groups. (optional, default to false)</param>
        /// <param name="withParents">When set to &#x60;true&#x60;, the returned references of child parents will return the device&#39;s parents (if any). Otherwise, it will be an empty array. (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (ManagedObjectCollection)</returns>
        System.Threading.Tasks.Task<ApiResponse<ManagedObjectCollection>> GetManagedObjectCollectionResourceWithHttpInfoAsync (string childAdditionId = default(string), string childAssetId = default(string), string childDeviceId = default(string), int? currentPage = default(int?), string fragmentType = default(string), List<string> ids = default(List<string>), bool? onlyRoots = default(bool?), string owner = default(string), int? pageSize = default(int?), string q = default(string), string query = default(string), bool? skipChildrenNames = default(bool?), string text = default(string), string type = default(string), bool? withChildren = default(bool?), bool? withChildrenCount = default(bool?), bool? withGroups = default(bool?), bool? withParents = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Retrieve a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve a specific managed object (for example, device, group, template) by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="skipChildrenNames">When set to &#x60;true&#x60;, the returned references of child devices won&#39;t contain their names. (optional, default to false)</param>
        /// <param name="withChildren">Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. (optional, default to true)</param>
        /// <param name="withChildrenCount">When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). (optional, default to false)</param>
        /// <param name="withParents">When set to &#x60;true&#x60;, the returned references of child parents will return the device&#39;s parents (if any). Otherwise, it will be an empty array. (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ManagedObject</returns>
        System.Threading.Tasks.Task<ManagedObject> GetManagedObjectResourceAsync (string id, bool? skipChildrenNames = default(bool?), bool? withChildren = default(bool?), bool? withChildrenCount = default(bool?), bool? withParents = default(bool?), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieve a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve a specific managed object (for example, device, group, template) by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="skipChildrenNames">When set to &#x60;true&#x60;, the returned references of child devices won&#39;t contain their names. (optional, default to false)</param>
        /// <param name="withChildren">Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. (optional, default to true)</param>
        /// <param name="withChildrenCount">When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). (optional, default to false)</param>
        /// <param name="withParents">When set to &#x60;true&#x60;, the returned references of child parents will return the device&#39;s parents (if any). Otherwise, it will be an empty array. (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (ManagedObject)</returns>
        System.Threading.Tasks.Task<ApiResponse<ManagedObject>> GetManagedObjectResourceWithHttpInfoAsync (string id, bool? skipChildrenNames = default(bool?), bool? withChildren = default(bool?), bool? withChildrenCount = default(bool?), bool? withParents = default(bool?), CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Retrieve the username and state of a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve the device owner's username and state (enabled or disabled) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ManagedObjectUser</returns>
        System.Threading.Tasks.Task<ManagedObjectUser> GetManagedObjectUserResourceAsync (string id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieve the username and state of a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve the device owner's username and state (enabled or disabled) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (ManagedObjectUser)</returns>
        System.Threading.Tasks.Task<ApiResponse<ManagedObjectUser>> GetManagedObjectUserResourceWithHttpInfoAsync (string id, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Retrieve all supported measurement fragments of a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve all measurement types of a specific managed object by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of SupportedMeasurements</returns>
        System.Threading.Tasks.Task<SupportedMeasurements> GetSupportedMeasurementsManagedObjectResourceAsync (string id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieve all supported measurement fragments of a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve all measurement types of a specific managed object by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (SupportedMeasurements)</returns>
        System.Threading.Tasks.Task<ApiResponse<SupportedMeasurements>> GetSupportedMeasurementsManagedObjectResourceWithHttpInfoAsync (string id, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Retrieve all supported measurement fragments and series of a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve all supported measurement fragments and series of a specific managed object by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of SupportedSeries</returns>
        System.Threading.Tasks.Task<SupportedSeries> GetSupportedSeriesManagedObjectResourceAsync (string id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieve all supported measurement fragments and series of a specific managed object
        /// </summary>
        /// <remarks>
        /// Retrieve all supported measurement fragments and series of a specific managed object by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (SupportedSeries)</returns>
        System.Threading.Tasks.Task<ApiResponse<SupportedSeries>> GetSupportedSeriesManagedObjectResourceWithHttpInfoAsync (string id, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Create a managed object
        /// </summary>
        /// <remarks>
        /// Create a managed object, for example, a device with temperature measurements support or a binary switch.<br> In general, each managed object may consist of:  *  A unique identifier that references the object. *  The name of the object. *  The most specific type of the managed object. *  A time stamp showing the last update. *  Fragments with specific meanings, for example, `c8y_IsDevice`, `c8y_SupportedOperations`. *  Any additional custom fragments.  Imagine, for example, that you want to describe electric meters from different vendors. Depending on the make of the meter, one may have a relay and one may be capable to measure a single phase or three phases (for example, a three-phase electricity sensor). A fragment `c8y_ThreePhaseElectricitySensor` would identify such an electric meter. Devices' characteristics are identified by storing fragments for each of them.  > **&#9432; Info:** For more details about fragments with specific meanings, review the sections [Device management library](#section/Device-management-library) and [Sensor library](#section/Sensor-library).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> ROLE_INVENTORY_CREATE </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="requestBody"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ManagedObject</returns>
        System.Threading.Tasks.Task<ManagedObject> PostManagedObjectCollectionResourceAsync (Dictionary<string, Object> requestBody, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Create a managed object
        /// </summary>
        /// <remarks>
        /// Create a managed object, for example, a device with temperature measurements support or a binary switch.<br> In general, each managed object may consist of:  *  A unique identifier that references the object. *  The name of the object. *  The most specific type of the managed object. *  A time stamp showing the last update. *  Fragments with specific meanings, for example, `c8y_IsDevice`, `c8y_SupportedOperations`. *  Any additional custom fragments.  Imagine, for example, that you want to describe electric meters from different vendors. Depending on the make of the meter, one may have a relay and one may be capable to measure a single phase or three phases (for example, a three-phase electricity sensor). A fragment `c8y_ThreePhaseElectricitySensor` would identify such an electric meter. Devices' characteristics are identified by storing fragments for each of them.  > **&#9432; Info:** For more details about fragments with specific meanings, review the sections [Device management library](#section/Device-management-library) and [Sensor library](#section/Sensor-library).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> ROLE_INVENTORY_CREATE </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="requestBody"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (ManagedObject)</returns>
        System.Threading.Tasks.Task<ApiResponse<ManagedObject>> PostManagedObjectCollectionResourceWithHttpInfoAsync (Dictionary<string, Object> requestBody, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Update a specific managed object
        /// </summary>
        /// <remarks>
        /// Update a specific managed object (for example, device) by a given ID.  For example, if you want to specify that your managed object is a device, you must add the fragment `c8y_IsDevice`.   The endpoint can also be used as a device availability heartbeat. If you only specifiy the `id`, it updates the date when the last message was received and no other property. The response then only contains the `id` instead of the full managed object.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="requestBody"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ManagedObject</returns>
        System.Threading.Tasks.Task<ManagedObject> PutManagedObjectResourceAsync (string id, Dictionary<string, Object> requestBody, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update a specific managed object
        /// </summary>
        /// <remarks>
        /// Update a specific managed object (for example, device) by a given ID.  For example, if you want to specify that your managed object is a device, you must add the fragment `c8y_IsDevice`.   The endpoint can also be used as a device availability heartbeat. If you only specifiy the `id`, it updates the date when the last message was received and no other property. The response then only contains the `id` instead of the full managed object.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="requestBody"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (ManagedObject)</returns>
        System.Threading.Tasks.Task<ApiResponse<ManagedObject>> PutManagedObjectResourceWithHttpInfoAsync (string id, Dictionary<string, Object> requestBody, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Update the user's details of a specific managed object
        /// </summary>
        /// <remarks>
        /// Update the device owner's state (enabled or disabled) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="managedObjectUser"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ManagedObjectUser</returns>
        System.Threading.Tasks.Task<ManagedObjectUser> PutManagedObjectUserResourceAsync (string id, ManagedObjectUser managedObjectUser, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update the user&#39;s details of a specific managed object
        /// </summary>
        /// <remarks>
        /// Update the device owner's state (enabled or disabled) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="managedObjectUser"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (ManagedObjectUser)</returns>
        System.Threading.Tasks.Task<ApiResponse<ManagedObjectUser>> PutManagedObjectUserResourceWithHttpInfoAsync (string id, ManagedObjectUser managedObjectUser, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken));
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class ManagedObjectsApi : IManagedObjectsApi
    {
        private kern.services.CumulocityClient.Client.ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedObjectsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public ManagedObjectsApi(String basePath)
        {
            this.Configuration = new kern.services.CumulocityClient.Client.Configuration { BasePath = basePath };

            ExceptionFactory = kern.services.CumulocityClient.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedObjectsApi"/> class
        /// </summary>
        /// <returns></returns>
        public ManagedObjectsApi()
        {
            this.Configuration = kern.services.CumulocityClient.Client.Configuration.Default;

            ExceptionFactory = kern.services.CumulocityClient.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedObjectsApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public ManagedObjectsApi(kern.services.CumulocityClient.Client.Configuration configuration = null)
        {
            if (configuration == null) // use the default one in Configuration
                this.Configuration = kern.services.CumulocityClient.Client.Configuration.Default;
            else
                this.Configuration = configuration;

            ExceptionFactory = kern.services.CumulocityClient.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public String GetBasePath()
        {
            return this.Configuration.ApiClient.RestClient.BaseUrl.ToString();
        }

        /// <summary>
        /// Sets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        [Obsolete("SetBasePath is deprecated, please do 'Configuration.ApiClient = new ApiClient(\"http://new-path\")' instead.")]
        public void SetBasePath(String basePath)
        {
            // do nothing
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public kern.services.CumulocityClient.Client.Configuration Configuration {get; set;}

        /// <summary>
        /// Provides a factory method hook for the creation of exceptions.
        /// </summary>
        public kern.services.CumulocityClient.Client.ExceptionFactory ExceptionFactory
        {
            get
            {
                if (_exceptionFactory != null && _exceptionFactory.GetInvocationList().Length > 1)
                {
                    throw new InvalidOperationException("Multicast delegate for ExceptionFactory is unsupported.");
                }
                return _exceptionFactory;
            }
            set { _exceptionFactory = value; }
        }

        /// <summary>
        /// Gets the default header.
        /// </summary>
        /// <returns>Dictionary of HTTP header</returns>
        [Obsolete("DefaultHeader is deprecated, please use Configuration.DefaultHeader instead.")]
        public IDictionary<String, String> DefaultHeader()
        {
            return new ReadOnlyDictionary<string, string>(this.Configuration.DefaultHeader);
        }

        /// <summary>
        /// Add default header.
        /// </summary>
        /// <param name="key">Header field name.</param>
        /// <param name="value">Header field value.</param>
        /// <returns></returns>
        [Obsolete("AddDefaultHeader is deprecated, please use Configuration.AddDefaultHeader instead.")]
        public void AddDefaultHeader(string key, string value)
        {
            this.Configuration.AddDefaultHeader(key, value);
        }

        /// <summary>
        /// Remove a specific managed object Remove a specific managed object (for example, device) by a given ID.  > **&#9432; Info:** Inventory DELETE requests are not synchronous. The response could be returned before the delete request has been completed. This may happen especially when the deleted managed object has a lot of associated data. After sending the request, the platform starts deleting the associated data in an asynchronous way. Finally, the requested managed object is deleted after all associated data has been deleted.  > **&#9432; Info:** By default, the delete operation is always propagated to the subgroups, but only if the deleted object is a group.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cascade">When set to &#x60;true&#x60; and the managed object is a device or group, all the hierarchy will be deleted. (optional, default to false)</param>
        /// <param name="forceCascade">When set to &#x60;true&#x60; all the hierarchy will be deleted without checking the type of managed object. It takes precedence over the parameter &#x60;cascade&#x60;. (optional, default to false)</param>
        /// <param name="withDeviceUser">When set to &#x60;true&#x60; and the managed object is a device, it deletes the associated device user (credentials). (optional, default to false)</param>
        /// <returns></returns>
        public void DeleteManagedObjectResource (string id, string xCumulocityProcessingMode = default(string), bool? cascade = default(bool?), bool? forceCascade = default(bool?), bool? withDeviceUser = default(bool?))
        {
             DeleteManagedObjectResourceWithHttpInfo(id, xCumulocityProcessingMode, cascade, forceCascade, withDeviceUser);
        }

        /// <summary>
        /// Remove a specific managed object Remove a specific managed object (for example, device) by a given ID.  > **&#9432; Info:** Inventory DELETE requests are not synchronous. The response could be returned before the delete request has been completed. This may happen especially when the deleted managed object has a lot of associated data. After sending the request, the platform starts deleting the associated data in an asynchronous way. Finally, the requested managed object is deleted after all associated data has been deleted.  > **&#9432; Info:** By default, the delete operation is always propagated to the subgroups, but only if the deleted object is a group.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cascade">When set to &#x60;true&#x60; and the managed object is a device or group, all the hierarchy will be deleted. (optional, default to false)</param>
        /// <param name="forceCascade">When set to &#x60;true&#x60; all the hierarchy will be deleted without checking the type of managed object. It takes precedence over the parameter &#x60;cascade&#x60;. (optional, default to false)</param>
        /// <param name="withDeviceUser">When set to &#x60;true&#x60; and the managed object is a device, it deletes the associated device user (credentials). (optional, default to false)</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> DeleteManagedObjectResourceWithHttpInfo (string id, string xCumulocityProcessingMode = default(string), bool? cascade = default(bool?), bool? forceCascade = default(bool?), bool? withDeviceUser = default(bool?))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ManagedObjectsApi->DeleteManagedObjectResource");

            var localVarPath = "/inventory/managedObjects/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (cascade != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "cascade", cascade)); // query parameter
            if (forceCascade != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "forceCascade", forceCascade)); // query parameter
            if (withDeviceUser != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withDeviceUser", withDeviceUser)); // query parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) this.Configuration.ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("DeleteManagedObjectResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                null);
        }

        /// <summary>
        /// Remove a specific managed object Remove a specific managed object (for example, device) by a given ID.  > **&#9432; Info:** Inventory DELETE requests are not synchronous. The response could be returned before the delete request has been completed. This may happen especially when the deleted managed object has a lot of associated data. After sending the request, the platform starts deleting the associated data in an asynchronous way. Finally, the requested managed object is deleted after all associated data has been deleted.  > **&#9432; Info:** By default, the delete operation is always propagated to the subgroups, but only if the deleted object is a group.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cascade">When set to &#x60;true&#x60; and the managed object is a device or group, all the hierarchy will be deleted. (optional, default to false)</param>
        /// <param name="forceCascade">When set to &#x60;true&#x60; all the hierarchy will be deleted without checking the type of managed object. It takes precedence over the parameter &#x60;cascade&#x60;. (optional, default to false)</param>
        /// <param name="withDeviceUser">When set to &#x60;true&#x60; and the managed object is a device, it deletes the associated device user (credentials). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DeleteManagedObjectResourceAsync (string id, string xCumulocityProcessingMode = default(string), bool? cascade = default(bool?), bool? forceCascade = default(bool?), bool? withDeviceUser = default(bool?), CancellationToken cancellationToken = default(CancellationToken))
        {
             await DeleteManagedObjectResourceWithHttpInfoAsync(id, xCumulocityProcessingMode, cascade, forceCascade, withDeviceUser, cancellationToken);

        }

        /// <summary>
        /// Remove a specific managed object Remove a specific managed object (for example, device) by a given ID.  > **&#9432; Info:** Inventory DELETE requests are not synchronous. The response could be returned before the delete request has been completed. This may happen especially when the deleted managed object has a lot of associated data. After sending the request, the platform starts deleting the associated data in an asynchronous way. Finally, the requested managed object is deleted after all associated data has been deleted.  > **&#9432; Info:** By default, the delete operation is always propagated to the subgroups, but only if the deleted object is a group.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cascade">When set to &#x60;true&#x60; and the managed object is a device or group, all the hierarchy will be deleted. (optional, default to false)</param>
        /// <param name="forceCascade">When set to &#x60;true&#x60; all the hierarchy will be deleted without checking the type of managed object. It takes precedence over the parameter &#x60;cascade&#x60;. (optional, default to false)</param>
        /// <param name="withDeviceUser">When set to &#x60;true&#x60; and the managed object is a device, it deletes the associated device user (credentials). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> DeleteManagedObjectResourceWithHttpInfoAsync (string id, string xCumulocityProcessingMode = default(string), bool? cascade = default(bool?), bool? forceCascade = default(bool?), bool? withDeviceUser = default(bool?), CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ManagedObjectsApi->DeleteManagedObjectResource");

            var localVarPath = "/inventory/managedObjects/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (cascade != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "cascade", cascade)); // query parameter
            if (forceCascade != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "forceCascade", forceCascade)); // query parameter
            if (withDeviceUser != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withDeviceUser", withDeviceUser)); // query parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await this.Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType, cancellationToken);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("DeleteManagedObjectResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                null);
        }

        /// <summary>
        /// Retrieve the latest availability date of a specific managed object Retrieve the date when a specific managed object (by a given ID) sent the last message to Cumulocity IoT.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <returns>DateTime</returns>
        public DateTime GetLastAvailabilityManagedObjectResource (string id)
        {
             ApiResponse<DateTime> localVarResponse = GetLastAvailabilityManagedObjectResourceWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve the latest availability date of a specific managed object Retrieve the date when a specific managed object (by a given ID) sent the last message to Cumulocity IoT.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <returns>ApiResponse of DateTime</returns>
        public ApiResponse<DateTime> GetLastAvailabilityManagedObjectResourceWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ManagedObjectsApi->GetLastAvailabilityManagedObjectResource");

            var localVarPath = "/inventory/managedObjects/{id}/availability";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "text/plain, application/json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) this.Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GetLastAvailabilityManagedObjectResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<DateTime>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (DateTime) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(DateTime)));
        }

        /// <summary>
        /// Retrieve the latest availability date of a specific managed object Retrieve the date when a specific managed object (by a given ID) sent the last message to Cumulocity IoT.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of DateTime</returns>
        public async System.Threading.Tasks.Task<DateTime> GetLastAvailabilityManagedObjectResourceAsync (string id, CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<DateTime> localVarResponse = await GetLastAvailabilityManagedObjectResourceWithHttpInfoAsync(id, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Retrieve the latest availability date of a specific managed object Retrieve the date when a specific managed object (by a given ID) sent the last message to Cumulocity IoT.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (DateTime)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<DateTime>> GetLastAvailabilityManagedObjectResourceWithHttpInfoAsync (string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ManagedObjectsApi->GetLastAvailabilityManagedObjectResource");

            var localVarPath = "/inventory/managedObjects/{id}/availability";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "text/plain, application/json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await this.Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType, cancellationToken);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GetLastAvailabilityManagedObjectResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<DateTime>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (DateTime) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(DateTime)));
        }

        /// <summary>
        /// Retrieve all managed objects Retrieve all managed objects (for example, devices, assets, etc.) registered in your tenant, or a subset based on queries. 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="childAdditionId">Search for a specific child addition and list all the groups to which it belongs. (optional)</param>
        /// <param name="childAssetId">Search for a specific child asset and list all the groups to which it belongs. (optional)</param>
        /// <param name="childDeviceId">Search for a specific child device and list all the groups to which it belongs. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="fragmentType">A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional)</param>
        /// <param name="ids">The managed object IDs to search for. &gt;**&amp;#9432; Info:** If you query for multiple IDs at once, comma-separate the values.  (optional)</param>
        /// <param name="onlyRoots">When set to &#x60;true&#x60; it returns managed objects which don&#39;t have any parent. If the current user doesn&#39;t have access to the parent, this is also root for the user. (optional, default to false)</param>
        /// <param name="owner">Username of the owner of the managed objects. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="q">Similar to the parameter &#x60;query&#x60;, but it assumes that this is a device query request and it adds automatically the search criteria &#x60;fragmentType&#x3D;c8y_IsDevice&#x60;. (optional)</param>
        /// <param name="query">Use query language to perform operations and/or filter the results. Details about the properties and supported operations can be found in [Query language](#tag/Query-language). (optional)</param>
        /// <param name="skipChildrenNames">When set to &#x60;true&#x60;, the returned references of child devices won&#39;t contain their names. (optional, default to false)</param>
        /// <param name="text">Search for managed objects where any property value is equal to the given one. Only string values are supported. (optional)</param>
        /// <param name="type">The type of managed object to search for. (optional)</param>
        /// <param name="withChildren">Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. (optional, default to true)</param>
        /// <param name="withChildrenCount">When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). (optional, default to false)</param>
        /// <param name="withGroups">When set to &#x60;true&#x60; it returns additional information about the groups to which the searched managed object belongs. This results in setting the &#x60;assetParents&#x60; property with additional information about the groups. (optional, default to false)</param>
        /// <param name="withParents">When set to &#x60;true&#x60;, the returned references of child parents will return the device&#39;s parents (if any). Otherwise, it will be an empty array. (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <returns>ManagedObjectCollection</returns>
        public ManagedObjectCollection GetManagedObjectCollectionResource (string childAdditionId = default(string), string childAssetId = default(string), string childDeviceId = default(string), int? currentPage = default(int?), string fragmentType = default(string), List<string> ids = default(List<string>), bool? onlyRoots = default(bool?), string owner = default(string), int? pageSize = default(int?), string q = default(string), string query = default(string), bool? skipChildrenNames = default(bool?), string text = default(string), string type = default(string), bool? withChildren = default(bool?), bool? withChildrenCount = default(bool?), bool? withGroups = default(bool?), bool? withParents = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?))
        {
             ApiResponse<ManagedObjectCollection> localVarResponse = GetManagedObjectCollectionResourceWithHttpInfo(childAdditionId, childAssetId, childDeviceId, currentPage, fragmentType, ids, onlyRoots, owner, pageSize, q, query, skipChildrenNames, text, type, withChildren, withChildrenCount, withGroups, withParents, withTotalElements, withTotalPages);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve all managed objects Retrieve all managed objects (for example, devices, assets, etc.) registered in your tenant, or a subset based on queries. 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="childAdditionId">Search for a specific child addition and list all the groups to which it belongs. (optional)</param>
        /// <param name="childAssetId">Search for a specific child asset and list all the groups to which it belongs. (optional)</param>
        /// <param name="childDeviceId">Search for a specific child device and list all the groups to which it belongs. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="fragmentType">A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional)</param>
        /// <param name="ids">The managed object IDs to search for. &gt;**&amp;#9432; Info:** If you query for multiple IDs at once, comma-separate the values.  (optional)</param>
        /// <param name="onlyRoots">When set to &#x60;true&#x60; it returns managed objects which don&#39;t have any parent. If the current user doesn&#39;t have access to the parent, this is also root for the user. (optional, default to false)</param>
        /// <param name="owner">Username of the owner of the managed objects. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="q">Similar to the parameter &#x60;query&#x60;, but it assumes that this is a device query request and it adds automatically the search criteria &#x60;fragmentType&#x3D;c8y_IsDevice&#x60;. (optional)</param>
        /// <param name="query">Use query language to perform operations and/or filter the results. Details about the properties and supported operations can be found in [Query language](#tag/Query-language). (optional)</param>
        /// <param name="skipChildrenNames">When set to &#x60;true&#x60;, the returned references of child devices won&#39;t contain their names. (optional, default to false)</param>
        /// <param name="text">Search for managed objects where any property value is equal to the given one. Only string values are supported. (optional)</param>
        /// <param name="type">The type of managed object to search for. (optional)</param>
        /// <param name="withChildren">Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. (optional, default to true)</param>
        /// <param name="withChildrenCount">When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). (optional, default to false)</param>
        /// <param name="withGroups">When set to &#x60;true&#x60; it returns additional information about the groups to which the searched managed object belongs. This results in setting the &#x60;assetParents&#x60; property with additional information about the groups. (optional, default to false)</param>
        /// <param name="withParents">When set to &#x60;true&#x60;, the returned references of child parents will return the device&#39;s parents (if any). Otherwise, it will be an empty array. (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <returns>ApiResponse of ManagedObjectCollection</returns>
        public ApiResponse<ManagedObjectCollection> GetManagedObjectCollectionResourceWithHttpInfo (string childAdditionId = default(string), string childAssetId = default(string), string childDeviceId = default(string), int? currentPage = default(int?), string fragmentType = default(string), List<string> ids = default(List<string>), bool? onlyRoots = default(bool?), string owner = default(string), int? pageSize = default(int?), string q = default(string), string query = default(string), bool? skipChildrenNames = default(bool?), string text = default(string), string type = default(string), bool? withChildren = default(bool?), bool? withChildrenCount = default(bool?), bool? withGroups = default(bool?), bool? withParents = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?))
        {

            var localVarPath = "/inventory/managedObjects";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.managedobjectcollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (childAdditionId != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "childAdditionId", childAdditionId)); // query parameter
            if (childAssetId != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "childAssetId", childAssetId)); // query parameter
            if (childDeviceId != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "childDeviceId", childDeviceId)); // query parameter
            if (currentPage != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "currentPage", currentPage)); // query parameter
            if (fragmentType != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "fragmentType", fragmentType)); // query parameter
            if (ids != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("csv", "ids", ids)); // query parameter
            if (onlyRoots != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "onlyRoots", onlyRoots)); // query parameter
            if (owner != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "owner", owner)); // query parameter
            if (pageSize != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "pageSize", pageSize)); // query parameter
            if (q != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "q", q)); // query parameter
            if (query != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "query", query)); // query parameter
            if (skipChildrenNames != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "skipChildrenNames", skipChildrenNames)); // query parameter
            if (text != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "text", text)); // query parameter
            if (type != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "type", type)); // query parameter
            if (withChildren != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withChildren", withChildren)); // query parameter
            if (withChildrenCount != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withChildrenCount", withChildrenCount)); // query parameter
            if (withGroups != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withGroups", withGroups)); // query parameter
            if (withParents != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withParents", withParents)); // query parameter
            if (withTotalElements != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withTotalElements", withTotalElements)); // query parameter
            if (withTotalPages != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withTotalPages", withTotalPages)); // query parameter

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) this.Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GetManagedObjectCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<ManagedObjectCollection>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (ManagedObjectCollection) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(ManagedObjectCollection)));
        }

        /// <summary>
        /// Retrieve all managed objects Retrieve all managed objects (for example, devices, assets, etc.) registered in your tenant, or a subset based on queries. 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="childAdditionId">Search for a specific child addition and list all the groups to which it belongs. (optional)</param>
        /// <param name="childAssetId">Search for a specific child asset and list all the groups to which it belongs. (optional)</param>
        /// <param name="childDeviceId">Search for a specific child device and list all the groups to which it belongs. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="fragmentType">A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional)</param>
        /// <param name="ids">The managed object IDs to search for. &gt;**&amp;#9432; Info:** If you query for multiple IDs at once, comma-separate the values.  (optional)</param>
        /// <param name="onlyRoots">When set to &#x60;true&#x60; it returns managed objects which don&#39;t have any parent. If the current user doesn&#39;t have access to the parent, this is also root for the user. (optional, default to false)</param>
        /// <param name="owner">Username of the owner of the managed objects. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="q">Similar to the parameter &#x60;query&#x60;, but it assumes that this is a device query request and it adds automatically the search criteria &#x60;fragmentType&#x3D;c8y_IsDevice&#x60;. (optional)</param>
        /// <param name="query">Use query language to perform operations and/or filter the results. Details about the properties and supported operations can be found in [Query language](#tag/Query-language). (optional)</param>
        /// <param name="skipChildrenNames">When set to &#x60;true&#x60;, the returned references of child devices won&#39;t contain their names. (optional, default to false)</param>
        /// <param name="text">Search for managed objects where any property value is equal to the given one. Only string values are supported. (optional)</param>
        /// <param name="type">The type of managed object to search for. (optional)</param>
        /// <param name="withChildren">Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. (optional, default to true)</param>
        /// <param name="withChildrenCount">When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). (optional, default to false)</param>
        /// <param name="withGroups">When set to &#x60;true&#x60; it returns additional information about the groups to which the searched managed object belongs. This results in setting the &#x60;assetParents&#x60; property with additional information about the groups. (optional, default to false)</param>
        /// <param name="withParents">When set to &#x60;true&#x60;, the returned references of child parents will return the device&#39;s parents (if any). Otherwise, it will be an empty array. (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ManagedObjectCollection</returns>
        public async System.Threading.Tasks.Task<ManagedObjectCollection> GetManagedObjectCollectionResourceAsync (string childAdditionId = default(string), string childAssetId = default(string), string childDeviceId = default(string), int? currentPage = default(int?), string fragmentType = default(string), List<string> ids = default(List<string>), bool? onlyRoots = default(bool?), string owner = default(string), int? pageSize = default(int?), string q = default(string), string query = default(string), bool? skipChildrenNames = default(bool?), string text = default(string), string type = default(string), bool? withChildren = default(bool?), bool? withChildrenCount = default(bool?), bool? withGroups = default(bool?), bool? withParents = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<ManagedObjectCollection> localVarResponse = await GetManagedObjectCollectionResourceWithHttpInfoAsync(childAdditionId, childAssetId, childDeviceId, currentPage, fragmentType, ids, onlyRoots, owner, pageSize, q, query, skipChildrenNames, text, type, withChildren, withChildrenCount, withGroups, withParents, withTotalElements, withTotalPages, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Retrieve all managed objects Retrieve all managed objects (for example, devices, assets, etc.) registered in your tenant, or a subset based on queries. 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="childAdditionId">Search for a specific child addition and list all the groups to which it belongs. (optional)</param>
        /// <param name="childAssetId">Search for a specific child asset and list all the groups to which it belongs. (optional)</param>
        /// <param name="childDeviceId">Search for a specific child device and list all the groups to which it belongs. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="fragmentType">A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional)</param>
        /// <param name="ids">The managed object IDs to search for. &gt;**&amp;#9432; Info:** If you query for multiple IDs at once, comma-separate the values.  (optional)</param>
        /// <param name="onlyRoots">When set to &#x60;true&#x60; it returns managed objects which don&#39;t have any parent. If the current user doesn&#39;t have access to the parent, this is also root for the user. (optional, default to false)</param>
        /// <param name="owner">Username of the owner of the managed objects. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="q">Similar to the parameter &#x60;query&#x60;, but it assumes that this is a device query request and it adds automatically the search criteria &#x60;fragmentType&#x3D;c8y_IsDevice&#x60;. (optional)</param>
        /// <param name="query">Use query language to perform operations and/or filter the results. Details about the properties and supported operations can be found in [Query language](#tag/Query-language). (optional)</param>
        /// <param name="skipChildrenNames">When set to &#x60;true&#x60;, the returned references of child devices won&#39;t contain their names. (optional, default to false)</param>
        /// <param name="text">Search for managed objects where any property value is equal to the given one. Only string values are supported. (optional)</param>
        /// <param name="type">The type of managed object to search for. (optional)</param>
        /// <param name="withChildren">Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. (optional, default to true)</param>
        /// <param name="withChildrenCount">When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). (optional, default to false)</param>
        /// <param name="withGroups">When set to &#x60;true&#x60; it returns additional information about the groups to which the searched managed object belongs. This results in setting the &#x60;assetParents&#x60; property with additional information about the groups. (optional, default to false)</param>
        /// <param name="withParents">When set to &#x60;true&#x60;, the returned references of child parents will return the device&#39;s parents (if any). Otherwise, it will be an empty array. (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (ManagedObjectCollection)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<ManagedObjectCollection>> GetManagedObjectCollectionResourceWithHttpInfoAsync (string childAdditionId = default(string), string childAssetId = default(string), string childDeviceId = default(string), int? currentPage = default(int?), string fragmentType = default(string), List<string> ids = default(List<string>), bool? onlyRoots = default(bool?), string owner = default(string), int? pageSize = default(int?), string q = default(string), string query = default(string), bool? skipChildrenNames = default(bool?), string text = default(string), string type = default(string), bool? withChildren = default(bool?), bool? withChildrenCount = default(bool?), bool? withGroups = default(bool?), bool? withParents = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), CancellationToken cancellationToken = default(CancellationToken))
        {

            var localVarPath = "/inventory/managedObjects";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.managedobjectcollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (childAdditionId != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "childAdditionId", childAdditionId)); // query parameter
            if (childAssetId != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "childAssetId", childAssetId)); // query parameter
            if (childDeviceId != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "childDeviceId", childDeviceId)); // query parameter
            if (currentPage != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "currentPage", currentPage)); // query parameter
            if (fragmentType != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "fragmentType", fragmentType)); // query parameter
            if (ids != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("csv", "ids", ids)); // query parameter
            if (onlyRoots != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "onlyRoots", onlyRoots)); // query parameter
            if (owner != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "owner", owner)); // query parameter
            if (pageSize != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "pageSize", pageSize)); // query parameter
            if (q != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "q", q)); // query parameter
            if (query != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "query", query)); // query parameter
            if (skipChildrenNames != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "skipChildrenNames", skipChildrenNames)); // query parameter
            if (text != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "text", text)); // query parameter
            if (type != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "type", type)); // query parameter
            if (withChildren != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withChildren", withChildren)); // query parameter
            if (withChildrenCount != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withChildrenCount", withChildrenCount)); // query parameter
            if (withGroups != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withGroups", withGroups)); // query parameter
            if (withParents != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withParents", withParents)); // query parameter
            if (withTotalElements != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withTotalElements", withTotalElements)); // query parameter
            if (withTotalPages != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withTotalPages", withTotalPages)); // query parameter

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await this.Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType, cancellationToken);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GetManagedObjectCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<ManagedObjectCollection>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (ManagedObjectCollection) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(ManagedObjectCollection)));
        }

        /// <summary>
        /// Retrieve a specific managed object Retrieve a specific managed object (for example, device, group, template) by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="skipChildrenNames">When set to &#x60;true&#x60;, the returned references of child devices won&#39;t contain their names. (optional, default to false)</param>
        /// <param name="withChildren">Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. (optional, default to true)</param>
        /// <param name="withChildrenCount">When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). (optional, default to false)</param>
        /// <param name="withParents">When set to &#x60;true&#x60;, the returned references of child parents will return the device&#39;s parents (if any). Otherwise, it will be an empty array. (optional, default to false)</param>
        /// <returns>ManagedObject</returns>
        public ManagedObject GetManagedObjectResource (string id, bool? skipChildrenNames = default(bool?), bool? withChildren = default(bool?), bool? withChildrenCount = default(bool?), bool? withParents = default(bool?))
        {
             ApiResponse<ManagedObject> localVarResponse = GetManagedObjectResourceWithHttpInfo(id, skipChildrenNames, withChildren, withChildrenCount, withParents);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve a specific managed object Retrieve a specific managed object (for example, device, group, template) by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="skipChildrenNames">When set to &#x60;true&#x60;, the returned references of child devices won&#39;t contain their names. (optional, default to false)</param>
        /// <param name="withChildren">Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. (optional, default to true)</param>
        /// <param name="withChildrenCount">When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). (optional, default to false)</param>
        /// <param name="withParents">When set to &#x60;true&#x60;, the returned references of child parents will return the device&#39;s parents (if any). Otherwise, it will be an empty array. (optional, default to false)</param>
        /// <returns>ApiResponse of ManagedObject</returns>
        public ApiResponse<ManagedObject> GetManagedObjectResourceWithHttpInfo (string id, bool? skipChildrenNames = default(bool?), bool? withChildren = default(bool?), bool? withChildrenCount = default(bool?), bool? withParents = default(bool?))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ManagedObjectsApi->GetManagedObjectResource");

            var localVarPath = "/inventory/managedObjects/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.managedobject+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (skipChildrenNames != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "skipChildrenNames", skipChildrenNames)); // query parameter
            if (withChildren != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withChildren", withChildren)); // query parameter
            if (withChildrenCount != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withChildrenCount", withChildrenCount)); // query parameter
            if (withParents != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withParents", withParents)); // query parameter

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) this.Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GetManagedObjectResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<ManagedObject>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (ManagedObject) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(ManagedObject)));
        }

        /// <summary>
        /// Retrieve a specific managed object Retrieve a specific managed object (for example, device, group, template) by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="skipChildrenNames">When set to &#x60;true&#x60;, the returned references of child devices won&#39;t contain their names. (optional, default to false)</param>
        /// <param name="withChildren">Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. (optional, default to true)</param>
        /// <param name="withChildrenCount">When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). (optional, default to false)</param>
        /// <param name="withParents">When set to &#x60;true&#x60;, the returned references of child parents will return the device&#39;s parents (if any). Otherwise, it will be an empty array. (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ManagedObject</returns>
        public async System.Threading.Tasks.Task<ManagedObject> GetManagedObjectResourceAsync (string id, bool? skipChildrenNames = default(bool?), bool? withChildren = default(bool?), bool? withChildrenCount = default(bool?), bool? withParents = default(bool?), CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<ManagedObject> localVarResponse = await GetManagedObjectResourceWithHttpInfoAsync(id, skipChildrenNames, withChildren, withChildrenCount, withParents, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Retrieve a specific managed object Retrieve a specific managed object (for example, device, group, template) by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="skipChildrenNames">When set to &#x60;true&#x60;, the returned references of child devices won&#39;t contain their names. (optional, default to false)</param>
        /// <param name="withChildren">Determines if children with ID and name should be returned when fetching the managed object. Set it to &#x60;false&#x60; to improve query performance. (optional, default to true)</param>
        /// <param name="withChildrenCount">When set to &#x60;true&#x60;, the returned result will contain the total number of children in the respective objects (&#x60;childAdditions&#x60;, &#x60;childAssets&#x60; and &#x60;childDevices&#x60;). (optional, default to false)</param>
        /// <param name="withParents">When set to &#x60;true&#x60;, the returned references of child parents will return the device&#39;s parents (if any). Otherwise, it will be an empty array. (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (ManagedObject)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<ManagedObject>> GetManagedObjectResourceWithHttpInfoAsync (string id, bool? skipChildrenNames = default(bool?), bool? withChildren = default(bool?), bool? withChildrenCount = default(bool?), bool? withParents = default(bool?), CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ManagedObjectsApi->GetManagedObjectResource");

            var localVarPath = "/inventory/managedObjects/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.managedobject+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (skipChildrenNames != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "skipChildrenNames", skipChildrenNames)); // query parameter
            if (withChildren != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withChildren", withChildren)); // query parameter
            if (withChildrenCount != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withChildrenCount", withChildrenCount)); // query parameter
            if (withParents != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withParents", withParents)); // query parameter

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await this.Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType, cancellationToken);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GetManagedObjectResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<ManagedObject>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (ManagedObject) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(ManagedObject)));
        }

        /// <summary>
        /// Retrieve the username and state of a specific managed object Retrieve the device owner's username and state (enabled or disabled) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <returns>ManagedObjectUser</returns>
        public ManagedObjectUser GetManagedObjectUserResource (string id)
        {
             ApiResponse<ManagedObjectUser> localVarResponse = GetManagedObjectUserResourceWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve the username and state of a specific managed object Retrieve the device owner's username and state (enabled or disabled) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <returns>ApiResponse of ManagedObjectUser</returns>
        public ApiResponse<ManagedObjectUser> GetManagedObjectUserResourceWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ManagedObjectsApi->GetManagedObjectUserResource");

            var localVarPath = "/inventory/managedObjects/{id}/user";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.managedobjectuser+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) this.Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GetManagedObjectUserResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<ManagedObjectUser>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (ManagedObjectUser) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(ManagedObjectUser)));
        }

        /// <summary>
        /// Retrieve the username and state of a specific managed object Retrieve the device owner's username and state (enabled or disabled) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ManagedObjectUser</returns>
        public async System.Threading.Tasks.Task<ManagedObjectUser> GetManagedObjectUserResourceAsync (string id, CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<ManagedObjectUser> localVarResponse = await GetManagedObjectUserResourceWithHttpInfoAsync(id, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Retrieve the username and state of a specific managed object Retrieve the device owner's username and state (enabled or disabled) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (ManagedObjectUser)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<ManagedObjectUser>> GetManagedObjectUserResourceWithHttpInfoAsync (string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ManagedObjectsApi->GetManagedObjectUserResource");

            var localVarPath = "/inventory/managedObjects/{id}/user";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.managedobjectuser+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await this.Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType, cancellationToken);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GetManagedObjectUserResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<ManagedObjectUser>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (ManagedObjectUser) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(ManagedObjectUser)));
        }

        /// <summary>
        /// Retrieve all supported measurement fragments of a specific managed object Retrieve all measurement types of a specific managed object by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <returns>SupportedMeasurements</returns>
        public SupportedMeasurements GetSupportedMeasurementsManagedObjectResource (string id)
        {
             ApiResponse<SupportedMeasurements> localVarResponse = GetSupportedMeasurementsManagedObjectResourceWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve all supported measurement fragments of a specific managed object Retrieve all measurement types of a specific managed object by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <returns>ApiResponse of SupportedMeasurements</returns>
        public ApiResponse<SupportedMeasurements> GetSupportedMeasurementsManagedObjectResourceWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ManagedObjectsApi->GetSupportedMeasurementsManagedObjectResource");

            var localVarPath = "/inventory/managedObjects/{id}/supportedMeasurements";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) this.Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GetSupportedMeasurementsManagedObjectResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<SupportedMeasurements>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (SupportedMeasurements) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(SupportedMeasurements)));
        }

        /// <summary>
        /// Retrieve all supported measurement fragments of a specific managed object Retrieve all measurement types of a specific managed object by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of SupportedMeasurements</returns>
        public async System.Threading.Tasks.Task<SupportedMeasurements> GetSupportedMeasurementsManagedObjectResourceAsync (string id, CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<SupportedMeasurements> localVarResponse = await GetSupportedMeasurementsManagedObjectResourceWithHttpInfoAsync(id, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Retrieve all supported measurement fragments of a specific managed object Retrieve all measurement types of a specific managed object by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (SupportedMeasurements)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<SupportedMeasurements>> GetSupportedMeasurementsManagedObjectResourceWithHttpInfoAsync (string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ManagedObjectsApi->GetSupportedMeasurementsManagedObjectResource");

            var localVarPath = "/inventory/managedObjects/{id}/supportedMeasurements";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await this.Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType, cancellationToken);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GetSupportedMeasurementsManagedObjectResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<SupportedMeasurements>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (SupportedMeasurements) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(SupportedMeasurements)));
        }

        /// <summary>
        /// Retrieve all supported measurement fragments and series of a specific managed object Retrieve all supported measurement fragments and series of a specific managed object by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <returns>SupportedSeries</returns>
        public SupportedSeries GetSupportedSeriesManagedObjectResource (string id)
        {
             ApiResponse<SupportedSeries> localVarResponse = GetSupportedSeriesManagedObjectResourceWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve all supported measurement fragments and series of a specific managed object Retrieve all supported measurement fragments and series of a specific managed object by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <returns>ApiResponse of SupportedSeries</returns>
        public ApiResponse<SupportedSeries> GetSupportedSeriesManagedObjectResourceWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ManagedObjectsApi->GetSupportedSeriesManagedObjectResource");

            var localVarPath = "/inventory/managedObjects/{id}/supportedSeries";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) this.Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GetSupportedSeriesManagedObjectResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<SupportedSeries>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (SupportedSeries) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(SupportedSeries)));
        }

        /// <summary>
        /// Retrieve all supported measurement fragments and series of a specific managed object Retrieve all supported measurement fragments and series of a specific managed object by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of SupportedSeries</returns>
        public async System.Threading.Tasks.Task<SupportedSeries> GetSupportedSeriesManagedObjectResourceAsync (string id, CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<SupportedSeries> localVarResponse = await GetSupportedSeriesManagedObjectResourceWithHttpInfoAsync(id, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Retrieve all supported measurement fragments and series of a specific managed object Retrieve all supported measurement fragments and series of a specific managed object by a given ID.  <section><h5>Required roles</h5> ROLE_INVENTORY_READ <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_READ permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (SupportedSeries)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<SupportedSeries>> GetSupportedSeriesManagedObjectResourceWithHttpInfoAsync (string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ManagedObjectsApi->GetSupportedSeriesManagedObjectResource");

            var localVarPath = "/inventory/managedObjects/{id}/supportedSeries";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await this.Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType, cancellationToken);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GetSupportedSeriesManagedObjectResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<SupportedSeries>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (SupportedSeries) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(SupportedSeries)));
        }

        /// <summary>
        /// Create a managed object Create a managed object, for example, a device with temperature measurements support or a binary switch.<br> In general, each managed object may consist of:  *  A unique identifier that references the object. *  The name of the object. *  The most specific type of the managed object. *  A time stamp showing the last update. *  Fragments with specific meanings, for example, `c8y_IsDevice`, `c8y_SupportedOperations`. *  Any additional custom fragments.  Imagine, for example, that you want to describe electric meters from different vendors. Depending on the make of the meter, one may have a relay and one may be capable to measure a single phase or three phases (for example, a three-phase electricity sensor). A fragment `c8y_ThreePhaseElectricitySensor` would identify such an electric meter. Devices' characteristics are identified by storing fragments for each of them.  > **&#9432; Info:** For more details about fragments with specific meanings, review the sections [Device management library](#section/Device-management-library) and [Sensor library](#section/Sensor-library).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> ROLE_INVENTORY_CREATE </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="requestBody"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ManagedObject</returns>
        public ManagedObject PostManagedObjectCollectionResource (Dictionary<string, Object> requestBody, string accept = default(string), string xCumulocityProcessingMode = default(string))
        {
             ApiResponse<ManagedObject> localVarResponse = PostManagedObjectCollectionResourceWithHttpInfo(requestBody, accept, xCumulocityProcessingMode);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create a managed object Create a managed object, for example, a device with temperature measurements support or a binary switch.<br> In general, each managed object may consist of:  *  A unique identifier that references the object. *  The name of the object. *  The most specific type of the managed object. *  A time stamp showing the last update. *  Fragments with specific meanings, for example, `c8y_IsDevice`, `c8y_SupportedOperations`. *  Any additional custom fragments.  Imagine, for example, that you want to describe electric meters from different vendors. Depending on the make of the meter, one may have a relay and one may be capable to measure a single phase or three phases (for example, a three-phase electricity sensor). A fragment `c8y_ThreePhaseElectricitySensor` would identify such an electric meter. Devices' characteristics are identified by storing fragments for each of them.  > **&#9432; Info:** For more details about fragments with specific meanings, review the sections [Device management library](#section/Device-management-library) and [Sensor library](#section/Sensor-library).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> ROLE_INVENTORY_CREATE </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="requestBody"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ApiResponse of ManagedObject</returns>
        public ApiResponse<ManagedObject> PostManagedObjectCollectionResourceWithHttpInfo (Dictionary<string, Object> requestBody, string accept = default(string), string xCumulocityProcessingMode = default(string))
        {
            // verify the required parameter 'requestBody' is set
            if (requestBody == null)
                throw new ApiException(400, "Missing required parameter 'requestBody' when calling ManagedObjectsApi->PostManagedObjectCollectionResource");

            var localVarPath = "/inventory/managedObjects";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.managedobject+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.managedobject+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter
            if (requestBody != null && requestBody.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(requestBody); // http body (model) parameter
            }
            else
            {
                localVarPostBody = requestBody; // byte array
            }

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) this.Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("PostManagedObjectCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<ManagedObject>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (ManagedObject) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(ManagedObject)));
        }

        /// <summary>
        /// Create a managed object Create a managed object, for example, a device with temperature measurements support or a binary switch.<br> In general, each managed object may consist of:  *  A unique identifier that references the object. *  The name of the object. *  The most specific type of the managed object. *  A time stamp showing the last update. *  Fragments with specific meanings, for example, `c8y_IsDevice`, `c8y_SupportedOperations`. *  Any additional custom fragments.  Imagine, for example, that you want to describe electric meters from different vendors. Depending on the make of the meter, one may have a relay and one may be capable to measure a single phase or three phases (for example, a three-phase electricity sensor). A fragment `c8y_ThreePhaseElectricitySensor` would identify such an electric meter. Devices' characteristics are identified by storing fragments for each of them.  > **&#9432; Info:** For more details about fragments with specific meanings, review the sections [Device management library](#section/Device-management-library) and [Sensor library](#section/Sensor-library).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> ROLE_INVENTORY_CREATE </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="requestBody"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ManagedObject</returns>
        public async System.Threading.Tasks.Task<ManagedObject> PostManagedObjectCollectionResourceAsync (Dictionary<string, Object> requestBody, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<ManagedObject> localVarResponse = await PostManagedObjectCollectionResourceWithHttpInfoAsync(requestBody, accept, xCumulocityProcessingMode, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create a managed object Create a managed object, for example, a device with temperature measurements support or a binary switch.<br> In general, each managed object may consist of:  *  A unique identifier that references the object. *  The name of the object. *  The most specific type of the managed object. *  A time stamp showing the last update. *  Fragments with specific meanings, for example, `c8y_IsDevice`, `c8y_SupportedOperations`. *  Any additional custom fragments.  Imagine, for example, that you want to describe electric meters from different vendors. Depending on the make of the meter, one may have a relay and one may be capable to measure a single phase or three phases (for example, a three-phase electricity sensor). A fragment `c8y_ThreePhaseElectricitySensor` would identify such an electric meter. Devices' characteristics are identified by storing fragments for each of them.  > **&#9432; Info:** For more details about fragments with specific meanings, review the sections [Device management library](#section/Device-management-library) and [Sensor library](#section/Sensor-library).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> ROLE_INVENTORY_CREATE </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="requestBody"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (ManagedObject)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<ManagedObject>> PostManagedObjectCollectionResourceWithHttpInfoAsync (Dictionary<string, Object> requestBody, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'requestBody' is set
            if (requestBody == null)
                throw new ApiException(400, "Missing required parameter 'requestBody' when calling ManagedObjectsApi->PostManagedObjectCollectionResource");

            var localVarPath = "/inventory/managedObjects";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.managedobject+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.managedobject+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter
            if (requestBody != null && requestBody.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(requestBody); // http body (model) parameter
            }
            else
            {
                localVarPostBody = requestBody; // byte array
            }

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await this.Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType, cancellationToken);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("PostManagedObjectCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<ManagedObject>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (ManagedObject) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(ManagedObject)));
        }

        /// <summary>
        /// Update a specific managed object Update a specific managed object (for example, device) by a given ID.  For example, if you want to specify that your managed object is a device, you must add the fragment `c8y_IsDevice`.   The endpoint can also be used as a device availability heartbeat. If you only specifiy the `id`, it updates the date when the last message was received and no other property. The response then only contains the `id` instead of the full managed object.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="requestBody"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ManagedObject</returns>
        public ManagedObject PutManagedObjectResource (string id, Dictionary<string, Object> requestBody, string accept = default(string), string xCumulocityProcessingMode = default(string))
        {
             ApiResponse<ManagedObject> localVarResponse = PutManagedObjectResourceWithHttpInfo(id, requestBody, accept, xCumulocityProcessingMode);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Update a specific managed object Update a specific managed object (for example, device) by a given ID.  For example, if you want to specify that your managed object is a device, you must add the fragment `c8y_IsDevice`.   The endpoint can also be used as a device availability heartbeat. If you only specifiy the `id`, it updates the date when the last message was received and no other property. The response then only contains the `id` instead of the full managed object.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="requestBody"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ApiResponse of ManagedObject</returns>
        public ApiResponse<ManagedObject> PutManagedObjectResourceWithHttpInfo (string id, Dictionary<string, Object> requestBody, string accept = default(string), string xCumulocityProcessingMode = default(string))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ManagedObjectsApi->PutManagedObjectResource");
            // verify the required parameter 'requestBody' is set
            if (requestBody == null)
                throw new ApiException(400, "Missing required parameter 'requestBody' when calling ManagedObjectsApi->PutManagedObjectResource");

            var localVarPath = "/inventory/managedObjects/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.managedobject+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.managedobject+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter
            if (requestBody != null && requestBody.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(requestBody); // http body (model) parameter
            }
            else
            {
                localVarPostBody = requestBody; // byte array
            }

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) this.Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("PutManagedObjectResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<ManagedObject>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (ManagedObject) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(ManagedObject)));
        }

        /// <summary>
        /// Update a specific managed object Update a specific managed object (for example, device) by a given ID.  For example, if you want to specify that your managed object is a device, you must add the fragment `c8y_IsDevice`.   The endpoint can also be used as a device availability heartbeat. If you only specifiy the `id`, it updates the date when the last message was received and no other property. The response then only contains the `id` instead of the full managed object.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="requestBody"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ManagedObject</returns>
        public async System.Threading.Tasks.Task<ManagedObject> PutManagedObjectResourceAsync (string id, Dictionary<string, Object> requestBody, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<ManagedObject> localVarResponse = await PutManagedObjectResourceWithHttpInfoAsync(id, requestBody, accept, xCumulocityProcessingMode, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Update a specific managed object Update a specific managed object (for example, device) by a given ID.  For example, if you want to specify that your managed object is a device, you must add the fragment `c8y_IsDevice`.   The endpoint can also be used as a device availability heartbeat. If you only specifiy the `id`, it updates the date when the last message was received and no other property. The response then only contains the `id` instead of the full managed object.  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="requestBody"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (ManagedObject)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<ManagedObject>> PutManagedObjectResourceWithHttpInfoAsync (string id, Dictionary<string, Object> requestBody, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ManagedObjectsApi->PutManagedObjectResource");
            // verify the required parameter 'requestBody' is set
            if (requestBody == null)
                throw new ApiException(400, "Missing required parameter 'requestBody' when calling ManagedObjectsApi->PutManagedObjectResource");

            var localVarPath = "/inventory/managedObjects/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.managedobject+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.managedobject+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter
            if (requestBody != null && requestBody.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(requestBody); // http body (model) parameter
            }
            else
            {
                localVarPostBody = requestBody; // byte array
            }

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await this.Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType, cancellationToken);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("PutManagedObjectResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<ManagedObject>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (ManagedObject) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(ManagedObject)));
        }

        /// <summary>
        /// Update the user's details of a specific managed object Update the device owner's state (enabled or disabled) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="managedObjectUser"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ManagedObjectUser</returns>
        public ManagedObjectUser PutManagedObjectUserResource (string id, ManagedObjectUser managedObjectUser, string accept = default(string), string xCumulocityProcessingMode = default(string))
        {
             ApiResponse<ManagedObjectUser> localVarResponse = PutManagedObjectUserResourceWithHttpInfo(id, managedObjectUser, accept, xCumulocityProcessingMode);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Update the user's details of a specific managed object Update the device owner's state (enabled or disabled) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="managedObjectUser"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ApiResponse of ManagedObjectUser</returns>
        public ApiResponse<ManagedObjectUser> PutManagedObjectUserResourceWithHttpInfo (string id, ManagedObjectUser managedObjectUser, string accept = default(string), string xCumulocityProcessingMode = default(string))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ManagedObjectsApi->PutManagedObjectUserResource");
            // verify the required parameter 'managedObjectUser' is set
            if (managedObjectUser == null)
                throw new ApiException(400, "Missing required parameter 'managedObjectUser' when calling ManagedObjectsApi->PutManagedObjectUserResource");

            var localVarPath = "/inventory/managedObjects/{id}/user";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.managedobjectuser+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.managedobjectuser+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter
            if (managedObjectUser != null && managedObjectUser.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(managedObjectUser); // http body (model) parameter
            }
            else
            {
                localVarPostBody = managedObjectUser; // byte array
            }

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) this.Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("PutManagedObjectUserResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<ManagedObjectUser>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (ManagedObjectUser) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(ManagedObjectUser)));
        }

        /// <summary>
        /// Update the user's details of a specific managed object Update the device owner's state (enabled or disabled) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="managedObjectUser"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ManagedObjectUser</returns>
        public async System.Threading.Tasks.Task<ManagedObjectUser> PutManagedObjectUserResourceAsync (string id, ManagedObjectUser managedObjectUser, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<ManagedObjectUser> localVarResponse = await PutManagedObjectUserResourceWithHttpInfoAsync(id, managedObjectUser, accept, xCumulocityProcessingMode, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Update the user's details of a specific managed object Update the device owner's state (enabled or disabled) of a specific managed object (by a given ID).  <section><h5>Required roles</h5> ROLE_INVENTORY_ADMIN <b>OR</b> owner of the source <b>OR</b> MANAGE_OBJECT_ADMIN permission on the source </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the managed object.</param>
        /// <param name="managedObjectUser"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (ManagedObjectUser)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<ManagedObjectUser>> PutManagedObjectUserResourceWithHttpInfoAsync (string id, ManagedObjectUser managedObjectUser, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ManagedObjectsApi->PutManagedObjectUserResource");
            // verify the required parameter 'managedObjectUser' is set
            if (managedObjectUser == null)
                throw new ApiException(400, "Missing required parameter 'managedObjectUser' when calling ManagedObjectsApi->PutManagedObjectUserResource");

            var localVarPath = "/inventory/managedObjects/{id}/user";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.managedobjectuser+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.managedobjectuser+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter
            if (managedObjectUser != null && managedObjectUser.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(managedObjectUser); // http body (model) parameter
            }
            else
            {
                localVarPostBody = managedObjectUser; // byte array
            }

            // authentication (Basic) required
            // http basic authentication required
            if (!String.IsNullOrEmpty(this.Configuration.Username) || !String.IsNullOrEmpty(this.Configuration.Password))
            {
                localVarHeaderParams["Authorization"] = "Basic " + ApiClient.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password);
            }
            // authentication (OAI-Secure) required
            // http bearer authentication required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }
            // authentication (SSO) required
            // oauth required
            if (!String.IsNullOrEmpty(this.Configuration.AccessToken))
            {
                localVarHeaderParams["Authorization"] = "Bearer " + this.Configuration.AccessToken;
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await this.Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType, cancellationToken);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("PutManagedObjectUserResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<ManagedObjectUser>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (ManagedObjectUser) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(ManagedObjectUser)));
        }

    }
}
