/*
 * Cumulocity IoT
 *
 * # REST implementation  This section describes the aspects common to all REST-based interfaces of Cumulocity IoT. The interfaces are based on the [Hypertext Transfer Protocol 1.1](https://tools.ietf.org/html/rfc2616) using [HTTPS](http://en.wikipedia.org/wiki/HTTP_Secure).  ## HTTP usage  ### Application management  Cumulocity IoT uses a so-called \"application key\" to distinguish requests coming from devices and traffic from applications. If you write an application, pass the following header as part of all requests:  ```markup X-Cumulocity-Application-Key: <APPLICATION_KEY> ```  For example, if you registered your application in the Cumulocity IoT Administration application with the key \"myapp\", your requests should contain the header:  ```markup X-Cumulocity-Application-Key: myapp ```  This makes your application subscribable and billable. If you implement a device, do not pass the key.  > **&#9432; Info:** Make sure that you pass the key in **all** requests coming from an application. If you leave out the key, > the request will be considered as a device request, and the corresponding device will be marked as \"available\".  ### Limited HTTP clients  If you use an HTTP client that can only perform GET and POST methods in HTTP, you can emulate the other methods through an additional \"X-HTTP-METHOD\" header. Simply issue a POST request and add the header, specifying the actual REST method to be executed. For example, to emulate the \"PUT\" (modify) method, you can use:  ```http POST ... X-HTTP-METHOD: PUT ```  ### Processing mode  Every update request (PUT, POST, DELETE) executes with a so-called *processing mode*. The processing modes are as follows:  |Processing mode|Description| |- --|- --| |PERSISTENT (default)|All updates will be send both to the Cumulocity IoT database and to real-time processing.| |TRANSIENT|Updates will be sent only to real-time processing. As part of real-time processing, the user can decide case by case through scripts whether updates should be stored to the database or not.| |QUIESCENT|The QUIESCENT processing mode behaves like the PERSISTENT processing mode with the exception that no real-time notifications will be sent. Currently, the QUIESCENT processing mode is applicable for measurements, events and managed objects.| |CEP| With the CEP processing mode, requests will only be processed by CEP or Apama. Currently, the CEP processing mode is applicable for measurements and events only.|  To explicitly control the processing mode of an update request, you can use the \"X-Cumulocity-Processing-Mode\" header with a value of either \"PERSISTENT\", \"TRANSIENT\", \"QUIESCENT\" or \"CEP\":  ```markup X-Cumulocity-Processing-Mode: PERSISTENT ```  > **&#9432; Info:** Events are always delivered to CEP/Apama for all processing modes. This is independent from real-time notifications.  ### Authorization  All requests issued to Cumulocity IoT are subject to authorization. To determine the required permissions, see the \"Required role\" entries for the individual requests. To learn more about the different permissions and the concept of ownership in Cumulocity IoT, see [Security aspects > Managing roles and assigning permissions](https://cumulocity.com/guides/concepts/security/#managing-roles-and-assigning-permissions)\".  ### Media types  Each type of data is associated with an own media type. The general format of media types is:  ```markup application/vnd.com.nsn.cumulocity.<TYPE>+json;ver=<VERSION>;charset=UTF-8 ```  Each media type contains a parameter `ver` indicating the version of the type. At the time of writing, the latest version is \"0.9\". As an example, the media type for an error message in the current version is:  ```markup application/vnd.com.nsn.cumulocity.error+json;ver=0.9;charset=UTF-8 ```  Media types are used in HTTP \"Content-Type\" and \"Accept\" headers. If you specify an \"Accept\" header in a POST or PUT request, the response will contain the newly created or updated object. If you do not specify the header, the response body will be empty.  If a media type without the `ver` parameter is given, the oldest available version will be returned by the server. If the \"Accept\" header contains the same media type in multiple versions, the server will return a representation in the latest supported version.  Note that media type values should be treated as case insensitive.  ### Date format  Data exchanged with Cumulocity IoT in HTTP requests and responses is encoded in [JSON format](http://www.ietf.org/rfc/rfc4627.txt) and [UTF-8](http://en.wikipedia.org/wiki/UTF-8) character encoding. Timestamps and dates are accepted and emitted by Cumulocity IoT in [ISO 8601](http://www.w3.org/TR/NOTE-datetime) format:  ```markup Date: YYYY-MM-DD Time: hh:mm:ss±hh:mm Timestamp: YYYY-MM-DDThh:mm:ss±hh:mm ```  To avoid ambiguity, all times and timestamps must include timezone information. Please take into account that the plus character \"+\" must be encoded as \"%2B\".  ### Response Codes  Cumulocity IoT uses conventional HTTP response codes to indicate the success or failure of an API request. Codes in the `2xx` range indicate success. Codes in the `4xx` range indicate a user error. The response provides information on why the request failed (for example, a required parameter was omitted). Codes in the `5xx` range indicate an error with Cumulocity IoT's servers ([these are very rare](https://www.softwareag.cloud/site/sla/cumulocity-iot.html#availability)).  #### HTTP status code summary  |Code|Message|Description| |:- --:|:- --|:- --| |200|OK|Everything worked as expected.| |201|Created|A managed object was created.| |204|No content|An object was removed.| |400|Bad Request|The request was unacceptable, often due to missing a required parameter.| |401|Unauthorized|Authentication has failed, or credentials were required but not provided.| |403|Forbidden|The authenticated user doesn't have permissions to perform the request.| |404|Not Found|The requested resource doesn't exist.| |405|Method not allowed|The employed HTTP method cannot be used on this resource (for example, using PUT on a read-only resource).| |409|Conflict| The data is correct but it breaks some constraints (for example, application version limit is exceeded). | |422|Invalid data| Invalid data was sent on the request and/or a query could not be understood.                             | |422|Unprocessable Entity| The requested resource cannot be updated or mandatory fields are missing on the executed operation.      | |500<br>503|Server Errors| Something went wrong on Cumulocity IoT's end.                                                            |  ## REST usage  ### Interpretation of HTTP verbs  The semantics described in the [HTTP specification](http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html#sec9) are used:  * POST creates a new resource. In the response \"Location\" header, the URI of the newly created resource is returned. * GET retrieves a resource. * PUT updates an existing resource with the contents of the request. * DELETE removes a resource. The response will be \"204 No Content\".  If a PUT request only contains parts of a resource (also known as fragments), only those parts are updated. To remove such a part, use a PUT request with a null value for it:  ```json {   \"resourcePartName\": null } ```  > **&#9432; Info:** A PUT request cannot update sub-resources that are identified by a separate URI.  ### URI space and URI templates  Clients should not make assumptions on the layout of URIs used in requests, but construct URIs from previously returned URIs or URI templates. The [root interface](#tag/Platform-API) provides the entry point for clients.  URI templates contain placeholders in curly braces (for example, `{type}`), which must be filled by the client to produce a URI. As an example, see the following excerpt from the event API response:  ```json {   \"events\": {       \"self\": \"https://<TENANT_DOMAIN>/event\"   },   \"eventsForSourceAndType\": \"https://<TENANT_DOMAIN>/event/events?type={type}&source={source}\" } ```  The client must fill the `{type}` and `{source}` placeholders with the desired type and source devices of the events to be returned. The meaning of these placeholders is documented in the respective interface descriptions.  ### Interface structure  In general, Cumulocity IoT REST resources are modeled according to the following pattern:  * The starting point are API resources, which will provide access to the actual data through URIs and URI templates to collection resources. For example, the above event API resource provides the `events` URI and the `eventsForSourceAndType` URI to access collections of events. * Collection resources aggregate member resources and allow creating new member resources in the collection. For example, through the `events` collection resource, new events can be created. * Finally, individual resources can be edited.  #### Query result paging  Collection resources support paging of data to avoid passing huge data volumes in one block from client to server. GET requests to collections accept two query parameters:  * `currentPage` defines the slice of data to be returned, starting with 1. By default, the first page is returned. * `pageSize` indicates how many entries of the collection should be returned. By default, 5 entries are returned. The upper limit for one page is currently 2,000 documents. Any larger requested page size is trimmed to the upper limit. * `withTotalElements` will yield the total number of elements in the statistics object. This is only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). * `withTotalPages` will yield the total number of pages in the statistics object. This is only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)).  For convenience, collection resources provide `next` and `prev` links to retrieve the next and previous pages of the results. The following is an example response for managed object collections (the contents of the array `managedObjects` have been omitted):  ```json {   \"self\" : \"https://<TENANT_DOMAIN>/inventory/managedObjects?pageSize=5&currentPage=2\",   \"managedObjects\" : [...],   \"statistics\" : {     \"totalPages\" : 7,     \"pageSize\" : 5,     \"currentPage\" : 2,     \"totalElements\" : 34   },   \"prev\" : \"https://<TENANT_DOMAIN>/inventory/managedObjects?pageSize=5&currentPage=1\",   \"next\" : \"https://<TENANT_DOMAIN>/inventory/managedObjects?pageSize=5&currentPage=3\" } ```  The `totalPages` and `totalElements` properties can be expensive to compute, hence they are not returned by default for [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). To include any of them in the result, add the query parameters `withTotalPages=true` and/or `withTotalElements=true`.  > **&#9432; Info:** If inventory roles are applied to a user, a query by the user may return less than `pageSize` results even if there are more results in total.  #### Query result paging for users with restricted access  If a user does not have a global role for reading data from the API resource but rather has [inventory roles](https://cumulocity.com/guides/users-guide/administration/#inventory) for reading only particular documents, there are some differences in query result paging:  * In some circumstances the response may contain less than `pageSize` and `totalElements` elements though there is more data in the database accessible for the user. * In some circumstances `next` and `prev` links may appear in the response though there is no more data in the database accessible for the user. * The property `currentPage` of the response does not contain the page number but the offset of the next element not yet processed by the querying mechanism. * The query parameters `withTotalPages=true` and `withTotalElements=true` have no effect, and the value of the `totalPages` and `totalElements` properties is always null.  The above behavior results from the fact that the querying mechanism is iterating maximally over 10 * max(pageSize, 100) documents per request, and it stops even though the full page of data accessible for the user could not be collected. When the next page is requested the querying mechanism starts the iteration where it completed the previous time.  #### Query result by time interval  Use the following query parameters to obtain data for a specified time interval:  * `dateFrom` - Start date or date and time. * `dateTo` - End date or date and time.  Example formats:  ```markup dateTo=2019-04-20 dateTo=2019-04-20T08:30:00.000Z ```  Parameters are optional. Values provided with those parameters are inclusive.  > **⚠️ Important:** If your servers are not running in UTC (Coordinated Universal Time), any date passed without timezone will be handled as UTC, regardless of the server local timezone. This might lead to a difference regarding the date/time range included in the results.  ### Root interface  To discover the URIs to the various interfaces of Cumulocity IoT, it provides a \"root\" interface. This root interface aggregates all the underlying API resources. See the [Platform API](#tag/Platform-API) endpoint. For more information on the different API resources, consult the respective API sections.  ## Generic media types  ### Error  The error type provides further information on the reason of a failed request.  Content-Type: application/vnd.com.nsn.cumulocity.error+json  |Name|Type|Description| |- --|- --|- --| |error|string|Error type formatted as `<RESOURCE_TYPE>/<ERROR_NAME>`. For example, an object not found in the inventory is reported as `inventory/notFound`.| |info|string|URL to an error description on the Internet.| |message|string|Short text description of the error|  ### Paging statistics  Paging statistics for collection of resources.  Content-Type: application/vnd.com.nsn.cumulocity.pagingstatistics+json  |Name|Type|Description| |- --|- --|- --| |currentPage|integer|The current returned page within the full result set, starting at \"1\".| |pageSize|integer|Maximum number of records contained in this query.| |totalElements|integer|The total number of results (elements).| |totalPages|integer|The total number of paginated results (pages).|  > **&#9432; Info:** The `totalPages` and `totalElements` properties are not returned by default in the response. To include any of them, add the query parameters `withTotalPages=true` and/or `withTotalElements=true`. Be aware of [differences in query result paging for users with restricted access](#query-result-paging-for-users-with-restricted-access).  > **&#9432; Info:** To improve performance, the `totalPages` and `totalElements` statistics are cached for 10 seconds.  # Device management library  The device management library has moved. Visit the [device management library](https://cumulocity.com/guides/reference/device-management-library/#overview) in the *Reference guide*.  # Sensor library  The sensor library has moved. Visit the [sensor library](https://cumulocity.com/guides/reference/sensor-library/#overview) in the *Reference guide*.  # Login options  When you sign up for an account on the [Cumulocity IoT platform](https://cumulocity.com/), for example, by using a free trial, you will be provided with a dedicated URL address for your tenant. All requests to the platform must be authenticated employing your tenant ID, Cumulocity IoT user (c8yuser for short) and password. Cumulocity IoT offers the following forms of authentication:  * Basic authentication (Basic) * OAI-Secure authentication (OAI-Secure) * SSO with authentication code grant (SSO) * JSON Web Token authentication (JWT, deprecated)  You can check your login options with a GET call to the endpoint <kbd><a href=\"#tag/Login-options\">/tenant/loginOptions</a></kbd>. 
 *
 * The version of the OpenAPI document: Release 10.15.0
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mime;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Client.Auth;
using kern.services.CumulocityClient.Model;

namespace kern.services.CumulocityClient.Api
{

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IUsersApiSync : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Remove a specific user from a specific user group of a specific tenant
        /// </summary>
        /// <remarks>
        /// Remove a specific user (by a given user ID) from a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;b&gt;AND&lt;/b&gt; is not the current user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        void DeleteUserReferenceResource(string tenantId, int groupId, string userId, int operationIndex = 0);

        /// <summary>
        /// Remove a specific user from a specific user group of a specific tenant
        /// </summary>
        /// <remarks>
        /// Remove a specific user (by a given user ID) from a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;b&gt;AND&lt;/b&gt; is not the current user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DeleteUserReferenceResourceWithHttpInfo(string tenantId, int groupId, string userId, int operationIndex = 0);
        /// <summary>
        /// Delete a specific user for a specific tenant
        /// </summary>
        /// <remarks>
        /// Delete a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;b&gt;AND&lt;/b&gt; not the current user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        void DeleteUserResource(string tenantId, string userId, int operationIndex = 0);

        /// <summary>
        /// Delete a specific user for a specific tenant
        /// </summary>
        /// <remarks>
        /// Delete a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;b&gt;AND&lt;/b&gt; not the current user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DeleteUserResourceWithHttpInfo(string tenantId, string userId, int operationIndex = 0);
        /// <summary>
        /// Retrieve all users for a specific tenant
        /// </summary>
        /// <remarks>
        /// Retrieve all users for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="groups">Numeric group identifiers. The response will contain only users which belong to at least one of the specified groups. &gt;**&amp;#9432; Info:** If you query for multiple user groups at once, comma-separate the values.  (optional)</param>
        /// <param name="onlyDevices">If set to &#x60;true&#x60;, the response will only contain users created during bootstrap process (starting with “device_”). If the flag is absent or &#x60;false&#x60; the result will not contain “device_” users.  (optional, default to false)</param>
        /// <param name="owner">Exact username of the owner of the user (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="username">Prefix or full username (optional)</param>
        /// <param name="withSubusersCount">If set to &#x60;true&#x60;, then each of returned user will contain an additional field “subusersCount”. It is the number of direct subusers (users with corresponding “owner”).  (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>UserCollection</returns>
        UserCollection GetUserCollectionResource(string tenantId, int? currentPage = default(int?), List<string>? groups = default(List<string>?), bool? onlyDevices = default(bool?), string? owner = default(string?), int? pageSize = default(int?), string? username = default(string?), bool? withSubusersCount = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0);

        /// <summary>
        /// Retrieve all users for a specific tenant
        /// </summary>
        /// <remarks>
        /// Retrieve all users for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="groups">Numeric group identifiers. The response will contain only users which belong to at least one of the specified groups. &gt;**&amp;#9432; Info:** If you query for multiple user groups at once, comma-separate the values.  (optional)</param>
        /// <param name="onlyDevices">If set to &#x60;true&#x60;, the response will only contain users created during bootstrap process (starting with “device_”). If the flag is absent or &#x60;false&#x60; the result will not contain “device_” users.  (optional, default to false)</param>
        /// <param name="owner">Exact username of the owner of the user (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="username">Prefix or full username (optional)</param>
        /// <param name="withSubusersCount">If set to &#x60;true&#x60;, then each of returned user will contain an additional field “subusersCount”. It is the number of direct subusers (users with corresponding “owner”).  (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of UserCollection</returns>
        ApiResponse<UserCollection> GetUserCollectionResourceWithHttpInfo(string tenantId, int? currentPage = default(int?), List<string>? groups = default(List<string>?), bool? onlyDevices = default(bool?), string? owner = default(string?), int? pageSize = default(int?), string? username = default(string?), bool? withSubusersCount = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0);
        /// <summary>
        /// Retrieve the users of a specific user group of a specific tenant
        /// </summary>
        /// <remarks>
        /// Retrieve the users of a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; (ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; has access to the user group) &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>UserReferenceCollection</returns>
        UserReferenceCollection GetUserReferenceCollectionResource(string tenantId, int groupId, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), int operationIndex = 0);

        /// <summary>
        /// Retrieve the users of a specific user group of a specific tenant
        /// </summary>
        /// <remarks>
        /// Retrieve the users of a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; (ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; has access to the user group) &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of UserReferenceCollection</returns>
        ApiResponse<UserReferenceCollection> GetUserReferenceCollectionResourceWithHttpInfo(string tenantId, int groupId, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), int operationIndex = 0);
        /// <summary>
        /// Retrieve a specific user for a specific tenant
        /// </summary>
        /// <remarks>
        /// Retrieve a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  Users in the response are sorted by username in ascending order. Only objects which the user is allowed to see are returned to the user. The user password is never returned in a GET response. Authentication mechanism is provided by another interface.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>User</returns>
        User GetUserResource(string tenantId, string userId, int operationIndex = 0);

        /// <summary>
        /// Retrieve a specific user for a specific tenant
        /// </summary>
        /// <remarks>
        /// Retrieve a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  Users in the response are sorted by username in ascending order. Only objects which the user is allowed to see are returned to the user. The user password is never returned in a GET response. Authentication mechanism is provided by another interface.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of User</returns>
        ApiResponse<User> GetUserResourceWithHttpInfo(string tenantId, string userId, int operationIndex = 0);
        /// <summary>
        /// Retrieve a user by username in a specific tenant
        /// </summary>
        /// <remarks>
        /// Retrieve a user by username in a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="username">The username of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>User</returns>
        User GetUsersByNameResource(string tenantId, string username, int operationIndex = 0);

        /// <summary>
        /// Retrieve a user by username in a specific tenant
        /// </summary>
        /// <remarks>
        /// Retrieve a user by username in a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="username">The username of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of User</returns>
        ApiResponse<User> GetUsersByNameResourceWithHttpInfo(string tenantId, string username, int operationIndex = 0);
        /// <summary>
        /// Retrieve the TFA settings of a specific user
        /// </summary>
        /// <remarks>
        /// Retrieve the two-factor authentication settings for the specified user.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; (ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user) &lt;b&gt;OR&lt;/b&gt; is the current user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>UserTfaData</returns>
        UserTfaData GetUsersTfaResource(string tenantId, string userId, int operationIndex = 0);

        /// <summary>
        /// Retrieve the TFA settings of a specific user
        /// </summary>
        /// <remarks>
        /// Retrieve the two-factor authentication settings for the specified user.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; (ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user) &lt;b&gt;OR&lt;/b&gt; is the current user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of UserTfaData</returns>
        ApiResponse<UserTfaData> GetUsersTfaResourceWithHttpInfo(string tenantId, string userId, int operationIndex = 0);
        /// <summary>
        /// Terminate a user&#39;s session
        /// </summary>
        /// <remarks>
        /// After logging out, a user has to enter valid credentials again to get access to the platform.  The request is responsible for removing cookies from the browser and invalidating internal platform access tokens. 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cookie">The authorization cookie storing the access token of the user. This parameter is specific to OAI-Secure authentication. (optional)</param>
        /// <param name="X_XSRF_TOKEN">Prevents XRSF attack of the authenticated user. This parameter is specific to OAI-Secure authentication. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        void PostLogoutUser(string? cookie = default(string?), string? X_XSRF_TOKEN = default(string?), int operationIndex = 0);

        /// <summary>
        /// Terminate a user&#39;s session
        /// </summary>
        /// <remarks>
        /// After logging out, a user has to enter valid credentials again to get access to the platform.  The request is responsible for removing cookies from the browser and invalidating internal platform access tokens. 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cookie">The authorization cookie storing the access token of the user. This parameter is specific to OAI-Secure authentication. (optional)</param>
        /// <param name="X_XSRF_TOKEN">Prevents XRSF attack of the authenticated user. This parameter is specific to OAI-Secure authentication. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PostLogoutUserWithHttpInfo(string? cookie = default(string?), string? X_XSRF_TOKEN = default(string?), int operationIndex = 0);
        /// <summary>
        /// Create a user for a specific tenant
        /// </summary>
        /// <remarks>
        /// Create a user for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; has access to roles, groups, device permissions and applications &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="postUserCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>User</returns>
        User PostUserCollectionResource(string tenantId, PostUserCollectionResourceRequest postUserCollectionResourceRequest, string? accept = default(string?), int operationIndex = 0);

        /// <summary>
        /// Create a user for a specific tenant
        /// </summary>
        /// <remarks>
        /// Create a user for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; has access to roles, groups, device permissions and applications &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="postUserCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of User</returns>
        ApiResponse<User> PostUserCollectionResourceWithHttpInfo(string tenantId, PostUserCollectionResourceRequest postUserCollectionResourceRequest, string? accept = default(string?), int operationIndex = 0);
        /// <summary>
        /// Add a user to a specific user group of a specific tenant
        /// </summary>
        /// <remarks>
        /// Add a user to a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to assign root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy to any group&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to assign non-root users in a user hierarchy to groups accessible by the parent of assigned user&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to assign descendants of the current user in a user hierarchy to groups accessible by current user &lt;b&gt;AND&lt;/b&gt; accessible by the parent of assigned user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="subscribedUser"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>UserReference</returns>
        UserReference PostUserReferenceCollectionResource(string tenantId, int groupId, SubscribedUser subscribedUser, string? accept = default(string?), int operationIndex = 0);

        /// <summary>
        /// Add a user to a specific user group of a specific tenant
        /// </summary>
        /// <remarks>
        /// Add a user to a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to assign root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy to any group&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to assign non-root users in a user hierarchy to groups accessible by the parent of assigned user&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to assign descendants of the current user in a user hierarchy to groups accessible by current user &lt;b&gt;AND&lt;/b&gt; accessible by the parent of assigned user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="subscribedUser"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of UserReference</returns>
        ApiResponse<UserReference> PostUserReferenceCollectionResourceWithHttpInfo(string tenantId, int groupId, SubscribedUser subscribedUser, string? accept = default(string?), int operationIndex = 0);
        /// <summary>
        /// Update a specific user&#39;s password of a specific tenant
        /// </summary>
        /// <remarks>
        /// Update a specific user&#39;s password (by a given user ID) of a specific tenant (by a given tenant ID).  Changing the user&#39;s password creates a corresponding audit record of type \&quot;User\&quot; and activity \&quot;User updated\&quot;, and specifying that the password has been changed.  &gt; **⚠️ Important:** If the tenant uses OAI-Secure authentication, the target user will be logged out.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to update root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to update non-root users in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to assigned roles, groups, device permissions and applications&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to update descendants of the current user in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to assigned roles, groups, device permissions and applications &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="passwordChange"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        void PutUserChangePasswordResource(string tenantId, string userId, PasswordChange passwordChange, string? accept = default(string?), int operationIndex = 0);

        /// <summary>
        /// Update a specific user&#39;s password of a specific tenant
        /// </summary>
        /// <remarks>
        /// Update a specific user&#39;s password (by a given user ID) of a specific tenant (by a given tenant ID).  Changing the user&#39;s password creates a corresponding audit record of type \&quot;User\&quot; and activity \&quot;User updated\&quot;, and specifying that the password has been changed.  &gt; **⚠️ Important:** If the tenant uses OAI-Secure authentication, the target user will be logged out.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to update root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to update non-root users in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to assigned roles, groups, device permissions and applications&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to update descendants of the current user in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to assigned roles, groups, device permissions and applications &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="passwordChange"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PutUserChangePasswordResourceWithHttpInfo(string tenantId, string userId, PasswordChange passwordChange, string? accept = default(string?), int operationIndex = 0);
        /// <summary>
        /// Update a specific user for a specific tenant
        /// </summary>
        /// <remarks>
        /// Update a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  Any change in user&#39;s roles, device permissions and groups creates corresponding audit records with type \&quot;User\&quot; and activity \&quot;User updated\&quot; with information which properties have been changed.  When the user is updated with changed permissions or groups, a corresponding audit record is created with type \&quot;User\&quot; and activity \&quot;User updated\&quot;.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to update root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to update non-root users in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to roles, groups, device permissions and applications being assigned&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to update descendants of the current user in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to roles, groups, device permissions and applications being assigned &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="putUserResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>User</returns>
        User PutUserResource(string tenantId, string userId, PutUserResourceRequest putUserResourceRequest, string? accept = default(string?), int operationIndex = 0);

        /// <summary>
        /// Update a specific user for a specific tenant
        /// </summary>
        /// <remarks>
        /// Update a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  Any change in user&#39;s roles, device permissions and groups creates corresponding audit records with type \&quot;User\&quot; and activity \&quot;User updated\&quot; with information which properties have been changed.  When the user is updated with changed permissions or groups, a corresponding audit record is created with type \&quot;User\&quot; and activity \&quot;User updated\&quot;.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to update root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to update non-root users in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to roles, groups, device permissions and applications being assigned&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to update descendants of the current user in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to roles, groups, device permissions and applications being assigned &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="putUserResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of User</returns>
        ApiResponse<User> PutUserResourceWithHttpInfo(string tenantId, string userId, PutUserResourceRequest putUserResourceRequest, string? accept = default(string?), int operationIndex = 0);
        #endregion Synchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IUsersApiAsync : IApiAccessor
    {
        #region Asynchronous Operations
        /// <summary>
        /// Remove a specific user from a specific user group of a specific tenant
        /// </summary>
        /// <remarks>
        /// Remove a specific user (by a given user ID) from a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;b&gt;AND&lt;/b&gt; is not the current user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DeleteUserReferenceResourceAsync(string tenantId, int groupId, string userId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Remove a specific user from a specific user group of a specific tenant
        /// </summary>
        /// <remarks>
        /// Remove a specific user (by a given user ID) from a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;b&gt;AND&lt;/b&gt; is not the current user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DeleteUserReferenceResourceWithHttpInfoAsync(string tenantId, int groupId, string userId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Delete a specific user for a specific tenant
        /// </summary>
        /// <remarks>
        /// Delete a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;b&gt;AND&lt;/b&gt; not the current user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DeleteUserResourceAsync(string tenantId, string userId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Delete a specific user for a specific tenant
        /// </summary>
        /// <remarks>
        /// Delete a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;b&gt;AND&lt;/b&gt; not the current user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DeleteUserResourceWithHttpInfoAsync(string tenantId, string userId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Retrieve all users for a specific tenant
        /// </summary>
        /// <remarks>
        /// Retrieve all users for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="groups">Numeric group identifiers. The response will contain only users which belong to at least one of the specified groups. &gt;**&amp;#9432; Info:** If you query for multiple user groups at once, comma-separate the values.  (optional)</param>
        /// <param name="onlyDevices">If set to &#x60;true&#x60;, the response will only contain users created during bootstrap process (starting with “device_”). If the flag is absent or &#x60;false&#x60; the result will not contain “device_” users.  (optional, default to false)</param>
        /// <param name="owner">Exact username of the owner of the user (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="username">Prefix or full username (optional)</param>
        /// <param name="withSubusersCount">If set to &#x60;true&#x60;, then each of returned user will contain an additional field “subusersCount”. It is the number of direct subusers (users with corresponding “owner”).  (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of UserCollection</returns>
        System.Threading.Tasks.Task<UserCollection> GetUserCollectionResourceAsync(string tenantId, int? currentPage = default(int?), List<string>? groups = default(List<string>?), bool? onlyDevices = default(bool?), string? owner = default(string?), int? pageSize = default(int?), string? username = default(string?), bool? withSubusersCount = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Retrieve all users for a specific tenant
        /// </summary>
        /// <remarks>
        /// Retrieve all users for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="groups">Numeric group identifiers. The response will contain only users which belong to at least one of the specified groups. &gt;**&amp;#9432; Info:** If you query for multiple user groups at once, comma-separate the values.  (optional)</param>
        /// <param name="onlyDevices">If set to &#x60;true&#x60;, the response will only contain users created during bootstrap process (starting with “device_”). If the flag is absent or &#x60;false&#x60; the result will not contain “device_” users.  (optional, default to false)</param>
        /// <param name="owner">Exact username of the owner of the user (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="username">Prefix or full username (optional)</param>
        /// <param name="withSubusersCount">If set to &#x60;true&#x60;, then each of returned user will contain an additional field “subusersCount”. It is the number of direct subusers (users with corresponding “owner”).  (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (UserCollection)</returns>
        System.Threading.Tasks.Task<ApiResponse<UserCollection>> GetUserCollectionResourceWithHttpInfoAsync(string tenantId, int? currentPage = default(int?), List<string>? groups = default(List<string>?), bool? onlyDevices = default(bool?), string? owner = default(string?), int? pageSize = default(int?), string? username = default(string?), bool? withSubusersCount = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Retrieve the users of a specific user group of a specific tenant
        /// </summary>
        /// <remarks>
        /// Retrieve the users of a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; (ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; has access to the user group) &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of UserReferenceCollection</returns>
        System.Threading.Tasks.Task<UserReferenceCollection> GetUserReferenceCollectionResourceAsync(string tenantId, int groupId, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Retrieve the users of a specific user group of a specific tenant
        /// </summary>
        /// <remarks>
        /// Retrieve the users of a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; (ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; has access to the user group) &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (UserReferenceCollection)</returns>
        System.Threading.Tasks.Task<ApiResponse<UserReferenceCollection>> GetUserReferenceCollectionResourceWithHttpInfoAsync(string tenantId, int groupId, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Retrieve a specific user for a specific tenant
        /// </summary>
        /// <remarks>
        /// Retrieve a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  Users in the response are sorted by username in ascending order. Only objects which the user is allowed to see are returned to the user. The user password is never returned in a GET response. Authentication mechanism is provided by another interface.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of User</returns>
        System.Threading.Tasks.Task<User> GetUserResourceAsync(string tenantId, string userId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Retrieve a specific user for a specific tenant
        /// </summary>
        /// <remarks>
        /// Retrieve a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  Users in the response are sorted by username in ascending order. Only objects which the user is allowed to see are returned to the user. The user password is never returned in a GET response. Authentication mechanism is provided by another interface.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (User)</returns>
        System.Threading.Tasks.Task<ApiResponse<User>> GetUserResourceWithHttpInfoAsync(string tenantId, string userId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Retrieve a user by username in a specific tenant
        /// </summary>
        /// <remarks>
        /// Retrieve a user by username in a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="username">The username of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of User</returns>
        System.Threading.Tasks.Task<User> GetUsersByNameResourceAsync(string tenantId, string username, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Retrieve a user by username in a specific tenant
        /// </summary>
        /// <remarks>
        /// Retrieve a user by username in a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="username">The username of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (User)</returns>
        System.Threading.Tasks.Task<ApiResponse<User>> GetUsersByNameResourceWithHttpInfoAsync(string tenantId, string username, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Retrieve the TFA settings of a specific user
        /// </summary>
        /// <remarks>
        /// Retrieve the two-factor authentication settings for the specified user.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; (ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user) &lt;b&gt;OR&lt;/b&gt; is the current user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of UserTfaData</returns>
        System.Threading.Tasks.Task<UserTfaData> GetUsersTfaResourceAsync(string tenantId, string userId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Retrieve the TFA settings of a specific user
        /// </summary>
        /// <remarks>
        /// Retrieve the two-factor authentication settings for the specified user.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; (ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user) &lt;b&gt;OR&lt;/b&gt; is the current user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (UserTfaData)</returns>
        System.Threading.Tasks.Task<ApiResponse<UserTfaData>> GetUsersTfaResourceWithHttpInfoAsync(string tenantId, string userId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Terminate a user&#39;s session
        /// </summary>
        /// <remarks>
        /// After logging out, a user has to enter valid credentials again to get access to the platform.  The request is responsible for removing cookies from the browser and invalidating internal platform access tokens. 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cookie">The authorization cookie storing the access token of the user. This parameter is specific to OAI-Secure authentication. (optional)</param>
        /// <param name="X_XSRF_TOKEN">Prevents XRSF attack of the authenticated user. This parameter is specific to OAI-Secure authentication. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task PostLogoutUserAsync(string? cookie = default(string?), string? X_XSRF_TOKEN = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Terminate a user&#39;s session
        /// </summary>
        /// <remarks>
        /// After logging out, a user has to enter valid credentials again to get access to the platform.  The request is responsible for removing cookies from the browser and invalidating internal platform access tokens. 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cookie">The authorization cookie storing the access token of the user. This parameter is specific to OAI-Secure authentication. (optional)</param>
        /// <param name="X_XSRF_TOKEN">Prevents XRSF attack of the authenticated user. This parameter is specific to OAI-Secure authentication. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PostLogoutUserWithHttpInfoAsync(string? cookie = default(string?), string? X_XSRF_TOKEN = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Create a user for a specific tenant
        /// </summary>
        /// <remarks>
        /// Create a user for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; has access to roles, groups, device permissions and applications &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="postUserCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of User</returns>
        System.Threading.Tasks.Task<User> PostUserCollectionResourceAsync(string tenantId, PostUserCollectionResourceRequest postUserCollectionResourceRequest, string? accept = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Create a user for a specific tenant
        /// </summary>
        /// <remarks>
        /// Create a user for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; has access to roles, groups, device permissions and applications &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="postUserCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (User)</returns>
        System.Threading.Tasks.Task<ApiResponse<User>> PostUserCollectionResourceWithHttpInfoAsync(string tenantId, PostUserCollectionResourceRequest postUserCollectionResourceRequest, string? accept = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Add a user to a specific user group of a specific tenant
        /// </summary>
        /// <remarks>
        /// Add a user to a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to assign root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy to any group&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to assign non-root users in a user hierarchy to groups accessible by the parent of assigned user&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to assign descendants of the current user in a user hierarchy to groups accessible by current user &lt;b&gt;AND&lt;/b&gt; accessible by the parent of assigned user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="subscribedUser"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of UserReference</returns>
        System.Threading.Tasks.Task<UserReference> PostUserReferenceCollectionResourceAsync(string tenantId, int groupId, SubscribedUser subscribedUser, string? accept = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Add a user to a specific user group of a specific tenant
        /// </summary>
        /// <remarks>
        /// Add a user to a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to assign root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy to any group&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to assign non-root users in a user hierarchy to groups accessible by the parent of assigned user&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to assign descendants of the current user in a user hierarchy to groups accessible by current user &lt;b&gt;AND&lt;/b&gt; accessible by the parent of assigned user &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="subscribedUser"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (UserReference)</returns>
        System.Threading.Tasks.Task<ApiResponse<UserReference>> PostUserReferenceCollectionResourceWithHttpInfoAsync(string tenantId, int groupId, SubscribedUser subscribedUser, string? accept = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Update a specific user&#39;s password of a specific tenant
        /// </summary>
        /// <remarks>
        /// Update a specific user&#39;s password (by a given user ID) of a specific tenant (by a given tenant ID).  Changing the user&#39;s password creates a corresponding audit record of type \&quot;User\&quot; and activity \&quot;User updated\&quot;, and specifying that the password has been changed.  &gt; **⚠️ Important:** If the tenant uses OAI-Secure authentication, the target user will be logged out.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to update root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to update non-root users in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to assigned roles, groups, device permissions and applications&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to update descendants of the current user in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to assigned roles, groups, device permissions and applications &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="passwordChange"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task PutUserChangePasswordResourceAsync(string tenantId, string userId, PasswordChange passwordChange, string? accept = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Update a specific user&#39;s password of a specific tenant
        /// </summary>
        /// <remarks>
        /// Update a specific user&#39;s password (by a given user ID) of a specific tenant (by a given tenant ID).  Changing the user&#39;s password creates a corresponding audit record of type \&quot;User\&quot; and activity \&quot;User updated\&quot;, and specifying that the password has been changed.  &gt; **⚠️ Important:** If the tenant uses OAI-Secure authentication, the target user will be logged out.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to update root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to update non-root users in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to assigned roles, groups, device permissions and applications&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to update descendants of the current user in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to assigned roles, groups, device permissions and applications &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="passwordChange"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PutUserChangePasswordResourceWithHttpInfoAsync(string tenantId, string userId, PasswordChange passwordChange, string? accept = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Update a specific user for a specific tenant
        /// </summary>
        /// <remarks>
        /// Update a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  Any change in user&#39;s roles, device permissions and groups creates corresponding audit records with type \&quot;User\&quot; and activity \&quot;User updated\&quot; with information which properties have been changed.  When the user is updated with changed permissions or groups, a corresponding audit record is created with type \&quot;User\&quot; and activity \&quot;User updated\&quot;.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to update root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to update non-root users in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to roles, groups, device permissions and applications being assigned&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to update descendants of the current user in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to roles, groups, device permissions and applications being assigned &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="putUserResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of User</returns>
        System.Threading.Tasks.Task<User> PutUserResourceAsync(string tenantId, string userId, PutUserResourceRequest putUserResourceRequest, string? accept = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Update a specific user for a specific tenant
        /// </summary>
        /// <remarks>
        /// Update a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  Any change in user&#39;s roles, device permissions and groups creates corresponding audit records with type \&quot;User\&quot; and activity \&quot;User updated\&quot; with information which properties have been changed.  When the user is updated with changed permissions or groups, a corresponding audit record is created with type \&quot;User\&quot; and activity \&quot;User updated\&quot;.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to update root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to update non-root users in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to roles, groups, device permissions and applications being assigned&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to update descendants of the current user in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to roles, groups, device permissions and applications being assigned &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="putUserResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (User)</returns>
        System.Threading.Tasks.Task<ApiResponse<User>> PutUserResourceWithHttpInfoAsync(string tenantId, string userId, PutUserResourceRequest putUserResourceRequest, string? accept = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IUsersApi : IUsersApiSync, IUsersApiAsync
    {

    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class UsersApi : IUsersApi
    {
        private kern.services.CumulocityClient.Client.ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersApi"/> class.
        /// </summary>
        /// <returns></returns>
        public UsersApi() : this((string)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersApi"/> class.
        /// </summary>
        /// <returns></returns>
        public UsersApi(string basePath)
        {
            this.Configuration = kern.services.CumulocityClient.Client.Configuration.MergeConfigurations(
                kern.services.CumulocityClient.Client.GlobalConfiguration.Instance,
                new kern.services.CumulocityClient.Client.Configuration { BasePath = basePath }
            );
            this.Client = new kern.services.CumulocityClient.Client.ApiClient(this.Configuration.BasePath);
            this.AsynchronousClient = new kern.services.CumulocityClient.Client.ApiClient(this.Configuration.BasePath);
            this.ExceptionFactory = kern.services.CumulocityClient.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public UsersApi(kern.services.CumulocityClient.Client.Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            this.Configuration = kern.services.CumulocityClient.Client.Configuration.MergeConfigurations(
                kern.services.CumulocityClient.Client.GlobalConfiguration.Instance,
                configuration
            );
            this.Client = new kern.services.CumulocityClient.Client.ApiClient(this.Configuration.BasePath);
            this.AsynchronousClient = new kern.services.CumulocityClient.Client.ApiClient(this.Configuration.BasePath);
            ExceptionFactory = kern.services.CumulocityClient.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersApi"/> class
        /// using a Configuration object and client instance.
        /// </summary>
        /// <param name="client">The client interface for synchronous API access.</param>
        /// <param name="asyncClient">The client interface for asynchronous API access.</param>
        /// <param name="configuration">The configuration object.</param>
        public UsersApi(kern.services.CumulocityClient.Client.ISynchronousClient client, kern.services.CumulocityClient.Client.IAsynchronousClient asyncClient, kern.services.CumulocityClient.Client.IReadableConfiguration configuration)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (asyncClient == null) throw new ArgumentNullException("asyncClient");
            if (configuration == null) throw new ArgumentNullException("configuration");

            this.Client = client;
            this.AsynchronousClient = asyncClient;
            this.Configuration = configuration;
            this.ExceptionFactory = kern.services.CumulocityClient.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// The client for accessing this underlying API asynchronously.
        /// </summary>
        public kern.services.CumulocityClient.Client.IAsynchronousClient AsynchronousClient { get; set; }

        /// <summary>
        /// The client for accessing this underlying API synchronously.
        /// </summary>
        public kern.services.CumulocityClient.Client.ISynchronousClient Client { get; set; }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public string GetBasePath()
        {
            return this.Configuration.BasePath;
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public kern.services.CumulocityClient.Client.IReadableConfiguration Configuration { get; set; }

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
        /// Remove a specific user from a specific user group of a specific tenant Remove a specific user (by a given user ID) from a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;b&gt;AND&lt;/b&gt; is not the current user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        public void DeleteUserReferenceResource(string tenantId, int groupId, string userId, int operationIndex = 0)
        {
            DeleteUserReferenceResourceWithHttpInfo(tenantId, groupId, userId);
        }

        /// <summary>
        /// Remove a specific user from a specific user group of a specific tenant Remove a specific user (by a given user ID) from a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;b&gt;AND&lt;/b&gt; is not the current user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<Object> DeleteUserReferenceResourceWithHttpInfo(string tenantId, int groupId, string userId, int operationIndex = 0)
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->DeleteUserReferenceResource");
            }

            // verify the required parameter 'userId' is set
            if (userId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'userId' when calling UsersApi->DeleteUserReferenceResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            localVarRequestOptions.PathParameters.Add("groupId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(groupId)); // path parameter
            localVarRequestOptions.PathParameters.Add("userId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(userId)); // path parameter

            localVarRequestOptions.Operation = "UsersApi.DeleteUserReferenceResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = this.Client.Delete<Object>("/user/{tenantId}/groups/{groupId}/users/{userId}", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteUserReferenceResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Remove a specific user from a specific user group of a specific tenant Remove a specific user (by a given user ID) from a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;b&gt;AND&lt;/b&gt; is not the current user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DeleteUserReferenceResourceAsync(string tenantId, int groupId, string userId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            await DeleteUserReferenceResourceWithHttpInfoAsync(tenantId, groupId, userId, operationIndex, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Remove a specific user from a specific user group of a specific tenant Remove a specific user (by a given user ID) from a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;b&gt;AND&lt;/b&gt; is not the current user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<Object>> DeleteUserReferenceResourceWithHttpInfoAsync(string tenantId, int groupId, string userId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->DeleteUserReferenceResource");
            }

            // verify the required parameter 'userId' is set
            if (userId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'userId' when calling UsersApi->DeleteUserReferenceResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            localVarRequestOptions.PathParameters.Add("groupId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(groupId)); // path parameter
            localVarRequestOptions.PathParameters.Add("userId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(userId)); // path parameter

            localVarRequestOptions.Operation = "UsersApi.DeleteUserReferenceResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.DeleteAsync<Object>("/user/{tenantId}/groups/{groupId}/users/{userId}", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteUserReferenceResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Delete a specific user for a specific tenant Delete a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;b&gt;AND&lt;/b&gt; not the current user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        public void DeleteUserResource(string tenantId, string userId, int operationIndex = 0)
        {
            DeleteUserResourceWithHttpInfo(tenantId, userId);
        }

        /// <summary>
        /// Delete a specific user for a specific tenant Delete a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;b&gt;AND&lt;/b&gt; not the current user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<Object> DeleteUserResourceWithHttpInfo(string tenantId, string userId, int operationIndex = 0)
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->DeleteUserResource");
            }

            // verify the required parameter 'userId' is set
            if (userId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'userId' when calling UsersApi->DeleteUserResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            localVarRequestOptions.PathParameters.Add("userId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(userId)); // path parameter

            localVarRequestOptions.Operation = "UsersApi.DeleteUserResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = this.Client.Delete<Object>("/user/{tenantId}/users/{userId}", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteUserResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Delete a specific user for a specific tenant Delete a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;b&gt;AND&lt;/b&gt; not the current user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DeleteUserResourceAsync(string tenantId, string userId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            await DeleteUserResourceWithHttpInfoAsync(tenantId, userId, operationIndex, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete a specific user for a specific tenant Delete a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;b&gt;AND&lt;/b&gt; not the current user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<Object>> DeleteUserResourceWithHttpInfoAsync(string tenantId, string userId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->DeleteUserResource");
            }

            // verify the required parameter 'userId' is set
            if (userId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'userId' when calling UsersApi->DeleteUserResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            localVarRequestOptions.PathParameters.Add("userId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(userId)); // path parameter

            localVarRequestOptions.Operation = "UsersApi.DeleteUserResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.DeleteAsync<Object>("/user/{tenantId}/users/{userId}", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteUserResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve all users for a specific tenant Retrieve all users for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="groups">Numeric group identifiers. The response will contain only users which belong to at least one of the specified groups. &gt;**&amp;#9432; Info:** If you query for multiple user groups at once, comma-separate the values.  (optional)</param>
        /// <param name="onlyDevices">If set to &#x60;true&#x60;, the response will only contain users created during bootstrap process (starting with “device_”). If the flag is absent or &#x60;false&#x60; the result will not contain “device_” users.  (optional, default to false)</param>
        /// <param name="owner">Exact username of the owner of the user (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="username">Prefix or full username (optional)</param>
        /// <param name="withSubusersCount">If set to &#x60;true&#x60;, then each of returned user will contain an additional field “subusersCount”. It is the number of direct subusers (users with corresponding “owner”).  (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>UserCollection</returns>
        public UserCollection GetUserCollectionResource(string tenantId, int? currentPage = default(int?), List<string>? groups = default(List<string>?), bool? onlyDevices = default(bool?), string? owner = default(string?), int? pageSize = default(int?), string? username = default(string?), bool? withSubusersCount = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<UserCollection> localVarResponse = GetUserCollectionResourceWithHttpInfo(tenantId, currentPage, groups, onlyDevices, owner, pageSize, username, withSubusersCount, withTotalElements, withTotalPages);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve all users for a specific tenant Retrieve all users for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="groups">Numeric group identifiers. The response will contain only users which belong to at least one of the specified groups. &gt;**&amp;#9432; Info:** If you query for multiple user groups at once, comma-separate the values.  (optional)</param>
        /// <param name="onlyDevices">If set to &#x60;true&#x60;, the response will only contain users created during bootstrap process (starting with “device_”). If the flag is absent or &#x60;false&#x60; the result will not contain “device_” users.  (optional, default to false)</param>
        /// <param name="owner">Exact username of the owner of the user (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="username">Prefix or full username (optional)</param>
        /// <param name="withSubusersCount">If set to &#x60;true&#x60;, then each of returned user will contain an additional field “subusersCount”. It is the number of direct subusers (users with corresponding “owner”).  (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of UserCollection</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<UserCollection> GetUserCollectionResourceWithHttpInfo(string tenantId, int? currentPage = default(int?), List<string>? groups = default(List<string>?), bool? onlyDevices = default(bool?), string? owner = default(string?), int? pageSize = default(int?), string? username = default(string?), bool? withSubusersCount = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0)
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->GetUserCollectionResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.usercollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            if (currentPage != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "currentPage", currentPage));
            }
            if (groups != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "groups", groups));
            }
            if (onlyDevices != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "onlyDevices", onlyDevices));
            }
            if (owner != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "owner", owner));
            }
            if (pageSize != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "pageSize", pageSize));
            }
            if (username != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "username", username));
            }
            if (withSubusersCount != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withSubusersCount", withSubusersCount));
            }
            if (withTotalElements != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withTotalElements", withTotalElements));
            }
            if (withTotalPages != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withTotalPages", withTotalPages));
            }

            localVarRequestOptions.Operation = "UsersApi.GetUserCollectionResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<UserCollection>("/user/{tenantId}/users", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetUserCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve all users for a specific tenant Retrieve all users for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="groups">Numeric group identifiers. The response will contain only users which belong to at least one of the specified groups. &gt;**&amp;#9432; Info:** If you query for multiple user groups at once, comma-separate the values.  (optional)</param>
        /// <param name="onlyDevices">If set to &#x60;true&#x60;, the response will only contain users created during bootstrap process (starting with “device_”). If the flag is absent or &#x60;false&#x60; the result will not contain “device_” users.  (optional, default to false)</param>
        /// <param name="owner">Exact username of the owner of the user (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="username">Prefix or full username (optional)</param>
        /// <param name="withSubusersCount">If set to &#x60;true&#x60;, then each of returned user will contain an additional field “subusersCount”. It is the number of direct subusers (users with corresponding “owner”).  (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of UserCollection</returns>
        public async System.Threading.Tasks.Task<UserCollection> GetUserCollectionResourceAsync(string tenantId, int? currentPage = default(int?), List<string>? groups = default(List<string>?), bool? onlyDevices = default(bool?), string? owner = default(string?), int? pageSize = default(int?), string? username = default(string?), bool? withSubusersCount = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<UserCollection> localVarResponse = await GetUserCollectionResourceWithHttpInfoAsync(tenantId, currentPage, groups, onlyDevices, owner, pageSize, username, withSubusersCount, withTotalElements, withTotalPages, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve all users for a specific tenant Retrieve all users for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="groups">Numeric group identifiers. The response will contain only users which belong to at least one of the specified groups. &gt;**&amp;#9432; Info:** If you query for multiple user groups at once, comma-separate the values.  (optional)</param>
        /// <param name="onlyDevices">If set to &#x60;true&#x60;, the response will only contain users created during bootstrap process (starting with “device_”). If the flag is absent or &#x60;false&#x60; the result will not contain “device_” users.  (optional, default to false)</param>
        /// <param name="owner">Exact username of the owner of the user (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="username">Prefix or full username (optional)</param>
        /// <param name="withSubusersCount">If set to &#x60;true&#x60;, then each of returned user will contain an additional field “subusersCount”. It is the number of direct subusers (users with corresponding “owner”).  (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (UserCollection)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<UserCollection>> GetUserCollectionResourceWithHttpInfoAsync(string tenantId, int? currentPage = default(int?), List<string>? groups = default(List<string>?), bool? onlyDevices = default(bool?), string? owner = default(string?), int? pageSize = default(int?), string? username = default(string?), bool? withSubusersCount = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->GetUserCollectionResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.usercollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            if (currentPage != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "currentPage", currentPage));
            }
            if (groups != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "groups", groups));
            }
            if (onlyDevices != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "onlyDevices", onlyDevices));
            }
            if (owner != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "owner", owner));
            }
            if (pageSize != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "pageSize", pageSize));
            }
            if (username != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "username", username));
            }
            if (withSubusersCount != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withSubusersCount", withSubusersCount));
            }
            if (withTotalElements != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withTotalElements", withTotalElements));
            }
            if (withTotalPages != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withTotalPages", withTotalPages));
            }

            localVarRequestOptions.Operation = "UsersApi.GetUserCollectionResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.GetAsync<UserCollection>("/user/{tenantId}/users", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetUserCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve the users of a specific user group of a specific tenant Retrieve the users of a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; (ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; has access to the user group) &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>UserReferenceCollection</returns>
        public UserReferenceCollection GetUserReferenceCollectionResource(string tenantId, int groupId, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<UserReferenceCollection> localVarResponse = GetUserReferenceCollectionResourceWithHttpInfo(tenantId, groupId, currentPage, pageSize, withTotalElements);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve the users of a specific user group of a specific tenant Retrieve the users of a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; (ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; has access to the user group) &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of UserReferenceCollection</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<UserReferenceCollection> GetUserReferenceCollectionResourceWithHttpInfo(string tenantId, int groupId, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), int operationIndex = 0)
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->GetUserReferenceCollectionResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.userreferencecollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            localVarRequestOptions.PathParameters.Add("groupId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(groupId)); // path parameter
            if (currentPage != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "currentPage", currentPage));
            }
            if (pageSize != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "pageSize", pageSize));
            }
            if (withTotalElements != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withTotalElements", withTotalElements));
            }

            localVarRequestOptions.Operation = "UsersApi.GetUserReferenceCollectionResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<UserReferenceCollection>("/user/{tenantId}/groups/{groupId}/users", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetUserReferenceCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve the users of a specific user group of a specific tenant Retrieve the users of a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; (ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; has access to the user group) &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of UserReferenceCollection</returns>
        public async System.Threading.Tasks.Task<UserReferenceCollection> GetUserReferenceCollectionResourceAsync(string tenantId, int groupId, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<UserReferenceCollection> localVarResponse = await GetUserReferenceCollectionResourceWithHttpInfoAsync(tenantId, groupId, currentPage, pageSize, withTotalElements, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve the users of a specific user group of a specific tenant Retrieve the users of a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; (ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; has access to the user group) &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (UserReferenceCollection)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<UserReferenceCollection>> GetUserReferenceCollectionResourceWithHttpInfoAsync(string tenantId, int groupId, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->GetUserReferenceCollectionResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.userreferencecollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            localVarRequestOptions.PathParameters.Add("groupId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(groupId)); // path parameter
            if (currentPage != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "currentPage", currentPage));
            }
            if (pageSize != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "pageSize", pageSize));
            }
            if (withTotalElements != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withTotalElements", withTotalElements));
            }

            localVarRequestOptions.Operation = "UsersApi.GetUserReferenceCollectionResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.GetAsync<UserReferenceCollection>("/user/{tenantId}/groups/{groupId}/users", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetUserReferenceCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve a specific user for a specific tenant Retrieve a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  Users in the response are sorted by username in ascending order. Only objects which the user is allowed to see are returned to the user. The user password is never returned in a GET response. Authentication mechanism is provided by another interface.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>User</returns>
        public User GetUserResource(string tenantId, string userId, int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<User> localVarResponse = GetUserResourceWithHttpInfo(tenantId, userId);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve a specific user for a specific tenant Retrieve a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  Users in the response are sorted by username in ascending order. Only objects which the user is allowed to see are returned to the user. The user password is never returned in a GET response. Authentication mechanism is provided by another interface.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of User</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<User> GetUserResourceWithHttpInfo(string tenantId, string userId, int operationIndex = 0)
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->GetUserResource");
            }

            // verify the required parameter 'userId' is set
            if (userId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'userId' when calling UsersApi->GetUserResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.user+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            localVarRequestOptions.PathParameters.Add("userId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(userId)); // path parameter

            localVarRequestOptions.Operation = "UsersApi.GetUserResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<User>("/user/{tenantId}/users/{userId}", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetUserResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve a specific user for a specific tenant Retrieve a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  Users in the response are sorted by username in ascending order. Only objects which the user is allowed to see are returned to the user. The user password is never returned in a GET response. Authentication mechanism is provided by another interface.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of User</returns>
        public async System.Threading.Tasks.Task<User> GetUserResourceAsync(string tenantId, string userId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<User> localVarResponse = await GetUserResourceWithHttpInfoAsync(tenantId, userId, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve a specific user for a specific tenant Retrieve a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  Users in the response are sorted by username in ascending order. Only objects which the user is allowed to see are returned to the user. The user password is never returned in a GET response. Authentication mechanism is provided by another interface.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (User)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<User>> GetUserResourceWithHttpInfoAsync(string tenantId, string userId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->GetUserResource");
            }

            // verify the required parameter 'userId' is set
            if (userId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'userId' when calling UsersApi->GetUserResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.user+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            localVarRequestOptions.PathParameters.Add("userId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(userId)); // path parameter

            localVarRequestOptions.Operation = "UsersApi.GetUserResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.GetAsync<User>("/user/{tenantId}/users/{userId}", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetUserResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve a user by username in a specific tenant Retrieve a user by username in a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="username">The username of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>User</returns>
        public User GetUsersByNameResource(string tenantId, string username, int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<User> localVarResponse = GetUsersByNameResourceWithHttpInfo(tenantId, username);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve a user by username in a specific tenant Retrieve a user by username in a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="username">The username of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of User</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<User> GetUsersByNameResourceWithHttpInfo(string tenantId, string username, int operationIndex = 0)
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->GetUsersByNameResource");
            }

            // verify the required parameter 'username' is set
            if (username == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'username' when calling UsersApi->GetUsersByNameResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.user+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            localVarRequestOptions.PathParameters.Add("username", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(username)); // path parameter

            localVarRequestOptions.Operation = "UsersApi.GetUsersByNameResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<User>("/user/{tenantId}/userByName/{username}", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetUsersByNameResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve a user by username in a specific tenant Retrieve a user by username in a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="username">The username of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of User</returns>
        public async System.Threading.Tasks.Task<User> GetUsersByNameResourceAsync(string tenantId, string username, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<User> localVarResponse = await GetUsersByNameResourceWithHttpInfoAsync(tenantId, username, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve a user by username in a specific tenant Retrieve a user by username in a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="username">The username of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (User)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<User>> GetUsersByNameResourceWithHttpInfoAsync(string tenantId, string username, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->GetUsersByNameResource");
            }

            // verify the required parameter 'username' is set
            if (username == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'username' when calling UsersApi->GetUsersByNameResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.user+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            localVarRequestOptions.PathParameters.Add("username", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(username)); // path parameter

            localVarRequestOptions.Operation = "UsersApi.GetUsersByNameResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.GetAsync<User>("/user/{tenantId}/userByName/{username}", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetUsersByNameResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve the TFA settings of a specific user Retrieve the two-factor authentication settings for the specified user.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; (ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user) &lt;b&gt;OR&lt;/b&gt; is the current user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>UserTfaData</returns>
        public UserTfaData GetUsersTfaResource(string tenantId, string userId, int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<UserTfaData> localVarResponse = GetUsersTfaResourceWithHttpInfo(tenantId, userId);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve the TFA settings of a specific user Retrieve the two-factor authentication settings for the specified user.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; (ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user) &lt;b&gt;OR&lt;/b&gt; is the current user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of UserTfaData</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<UserTfaData> GetUsersTfaResourceWithHttpInfo(string tenantId, string userId, int operationIndex = 0)
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->GetUsersTfaResource");
            }

            // verify the required parameter 'userId' is set
            if (userId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'userId' when calling UsersApi->GetUsersTfaResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            localVarRequestOptions.PathParameters.Add("userId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(userId)); // path parameter

            localVarRequestOptions.Operation = "UsersApi.GetUsersTfaResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<UserTfaData>("/user/{tenantId}/users/{userId}/tfa", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetUsersTfaResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve the TFA settings of a specific user Retrieve the two-factor authentication settings for the specified user.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; (ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user) &lt;b&gt;OR&lt;/b&gt; is the current user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of UserTfaData</returns>
        public async System.Threading.Tasks.Task<UserTfaData> GetUsersTfaResourceAsync(string tenantId, string userId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<UserTfaData> localVarResponse = await GetUsersTfaResourceWithHttpInfoAsync(tenantId, userId, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve the TFA settings of a specific user Retrieve the two-factor authentication settings for the specified user.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_READ &lt;b&gt;OR&lt;/b&gt; (ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; is parent of the user) &lt;b&gt;OR&lt;/b&gt; is the current user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (UserTfaData)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<UserTfaData>> GetUsersTfaResourceWithHttpInfoAsync(string tenantId, string userId, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->GetUsersTfaResource");
            }

            // verify the required parameter 'userId' is set
            if (userId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'userId' when calling UsersApi->GetUsersTfaResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            localVarRequestOptions.PathParameters.Add("userId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(userId)); // path parameter

            localVarRequestOptions.Operation = "UsersApi.GetUsersTfaResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.GetAsync<UserTfaData>("/user/{tenantId}/users/{userId}/tfa", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetUsersTfaResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Terminate a user&#39;s session After logging out, a user has to enter valid credentials again to get access to the platform.  The request is responsible for removing cookies from the browser and invalidating internal platform access tokens. 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cookie">The authorization cookie storing the access token of the user. This parameter is specific to OAI-Secure authentication. (optional)</param>
        /// <param name="X_XSRF_TOKEN">Prevents XRSF attack of the authenticated user. This parameter is specific to OAI-Secure authentication. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        public void PostLogoutUser(string? cookie = default(string?), string? X_XSRF_TOKEN = default(string?), int operationIndex = 0)
        {
            PostLogoutUserWithHttpInfo(cookie, X_XSRF_TOKEN);
        }

        /// <summary>
        /// Terminate a user&#39;s session After logging out, a user has to enter valid credentials again to get access to the platform.  The request is responsible for removing cookies from the browser and invalidating internal platform access tokens. 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cookie">The authorization cookie storing the access token of the user. This parameter is specific to OAI-Secure authentication. (optional)</param>
        /// <param name="X_XSRF_TOKEN">Prevents XRSF attack of the authenticated user. This parameter is specific to OAI-Secure authentication. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<Object> PostLogoutUserWithHttpInfo(string? cookie = default(string?), string? X_XSRF_TOKEN = default(string?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            if (cookie != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Cookie", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(cookie)); // header parameter
            }
            if (X_XSRF_TOKEN != null)
            {
                localVarRequestOptions.HeaderParameters.Add("X-XSRF-TOKEN", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(X_XSRF_TOKEN)); // header parameter
            }

            localVarRequestOptions.Operation = "UsersApi.PostLogoutUser";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<Object>("/user/logout", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PostLogoutUser", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Terminate a user&#39;s session After logging out, a user has to enter valid credentials again to get access to the platform.  The request is responsible for removing cookies from the browser and invalidating internal platform access tokens. 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cookie">The authorization cookie storing the access token of the user. This parameter is specific to OAI-Secure authentication. (optional)</param>
        /// <param name="X_XSRF_TOKEN">Prevents XRSF attack of the authenticated user. This parameter is specific to OAI-Secure authentication. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task PostLogoutUserAsync(string? cookie = default(string?), string? X_XSRF_TOKEN = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            await PostLogoutUserWithHttpInfoAsync(cookie, X_XSRF_TOKEN, operationIndex, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Terminate a user&#39;s session After logging out, a user has to enter valid credentials again to get access to the platform.  The request is responsible for removing cookies from the browser and invalidating internal platform access tokens. 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cookie">The authorization cookie storing the access token of the user. This parameter is specific to OAI-Secure authentication. (optional)</param>
        /// <param name="X_XSRF_TOKEN">Prevents XRSF attack of the authenticated user. This parameter is specific to OAI-Secure authentication. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<Object>> PostLogoutUserWithHttpInfoAsync(string? cookie = default(string?), string? X_XSRF_TOKEN = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            if (cookie != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Cookie", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(cookie)); // header parameter
            }
            if (X_XSRF_TOKEN != null)
            {
                localVarRequestOptions.HeaderParameters.Add("X-XSRF-TOKEN", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(X_XSRF_TOKEN)); // header parameter
            }

            localVarRequestOptions.Operation = "UsersApi.PostLogoutUser";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.PostAsync<Object>("/user/logout", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PostLogoutUser", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Create a user for a specific tenant Create a user for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; has access to roles, groups, device permissions and applications &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="postUserCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>User</returns>
        public User PostUserCollectionResource(string tenantId, PostUserCollectionResourceRequest postUserCollectionResourceRequest, string? accept = default(string?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<User> localVarResponse = PostUserCollectionResourceWithHttpInfo(tenantId, postUserCollectionResourceRequest, accept);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Create a user for a specific tenant Create a user for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; has access to roles, groups, device permissions and applications &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="postUserCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of User</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<User> PostUserCollectionResourceWithHttpInfo(string tenantId, PostUserCollectionResourceRequest postUserCollectionResourceRequest, string? accept = default(string?), int operationIndex = 0)
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->PostUserCollectionResource");
            }

            // verify the required parameter 'postUserCollectionResourceRequest' is set
            if (postUserCollectionResourceRequest == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'postUserCollectionResourceRequest' when calling UsersApi->PostUserCollectionResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/vnd.com.nsn.cumulocity.user+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.user+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            if (accept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(accept)); // header parameter
            }
            localVarRequestOptions.Data = postUserCollectionResourceRequest;

            localVarRequestOptions.Operation = "UsersApi.PostUserCollectionResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<User>("/user/{tenantId}/users", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PostUserCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Create a user for a specific tenant Create a user for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; has access to roles, groups, device permissions and applications &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="postUserCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of User</returns>
        public async System.Threading.Tasks.Task<User> PostUserCollectionResourceAsync(string tenantId, PostUserCollectionResourceRequest postUserCollectionResourceRequest, string? accept = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<User> localVarResponse = await PostUserCollectionResourceWithHttpInfoAsync(tenantId, postUserCollectionResourceRequest, accept, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Create a user for a specific tenant Create a user for a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; ROLE_USER_MANAGEMENT_CREATE &lt;b&gt;AND&lt;/b&gt; has access to roles, groups, device permissions and applications &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="postUserCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (User)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<User>> PostUserCollectionResourceWithHttpInfoAsync(string tenantId, PostUserCollectionResourceRequest postUserCollectionResourceRequest, string? accept = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->PostUserCollectionResource");
            }

            // verify the required parameter 'postUserCollectionResourceRequest' is set
            if (postUserCollectionResourceRequest == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'postUserCollectionResourceRequest' when calling UsersApi->PostUserCollectionResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/vnd.com.nsn.cumulocity.user+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.user+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            if (accept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(accept)); // header parameter
            }
            localVarRequestOptions.Data = postUserCollectionResourceRequest;

            localVarRequestOptions.Operation = "UsersApi.PostUserCollectionResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.PostAsync<User>("/user/{tenantId}/users", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PostUserCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Add a user to a specific user group of a specific tenant Add a user to a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to assign root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy to any group&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to assign non-root users in a user hierarchy to groups accessible by the parent of assigned user&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to assign descendants of the current user in a user hierarchy to groups accessible by current user &lt;b&gt;AND&lt;/b&gt; accessible by the parent of assigned user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="subscribedUser"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>UserReference</returns>
        public UserReference PostUserReferenceCollectionResource(string tenantId, int groupId, SubscribedUser subscribedUser, string? accept = default(string?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<UserReference> localVarResponse = PostUserReferenceCollectionResourceWithHttpInfo(tenantId, groupId, subscribedUser, accept);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Add a user to a specific user group of a specific tenant Add a user to a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to assign root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy to any group&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to assign non-root users in a user hierarchy to groups accessible by the parent of assigned user&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to assign descendants of the current user in a user hierarchy to groups accessible by current user &lt;b&gt;AND&lt;/b&gt; accessible by the parent of assigned user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="subscribedUser"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of UserReference</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<UserReference> PostUserReferenceCollectionResourceWithHttpInfo(string tenantId, int groupId, SubscribedUser subscribedUser, string? accept = default(string?), int operationIndex = 0)
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->PostUserReferenceCollectionResource");
            }

            // verify the required parameter 'subscribedUser' is set
            if (subscribedUser == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'subscribedUser' when calling UsersApi->PostUserReferenceCollectionResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/vnd.com.nsn.cumulocity.userreference+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.userreference+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            localVarRequestOptions.PathParameters.Add("groupId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(groupId)); // path parameter
            if (accept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(accept)); // header parameter
            }
            localVarRequestOptions.Data = subscribedUser;

            localVarRequestOptions.Operation = "UsersApi.PostUserReferenceCollectionResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<UserReference>("/user/{tenantId}/groups/{groupId}/users", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PostUserReferenceCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Add a user to a specific user group of a specific tenant Add a user to a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to assign root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy to any group&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to assign non-root users in a user hierarchy to groups accessible by the parent of assigned user&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to assign descendants of the current user in a user hierarchy to groups accessible by current user &lt;b&gt;AND&lt;/b&gt; accessible by the parent of assigned user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="subscribedUser"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of UserReference</returns>
        public async System.Threading.Tasks.Task<UserReference> PostUserReferenceCollectionResourceAsync(string tenantId, int groupId, SubscribedUser subscribedUser, string? accept = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<UserReference> localVarResponse = await PostUserReferenceCollectionResourceWithHttpInfoAsync(tenantId, groupId, subscribedUser, accept, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Add a user to a specific user group of a specific tenant Add a user to a specific user group (by a given user group ID) of a specific tenant (by a given tenant ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to assign root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy to any group&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to assign non-root users in a user hierarchy to groups accessible by the parent of assigned user&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to assign descendants of the current user in a user hierarchy to groups accessible by current user &lt;b&gt;AND&lt;/b&gt; accessible by the parent of assigned user &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="groupId">Unique identifier of the user group.</param>
        /// <param name="subscribedUser"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (UserReference)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<UserReference>> PostUserReferenceCollectionResourceWithHttpInfoAsync(string tenantId, int groupId, SubscribedUser subscribedUser, string? accept = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->PostUserReferenceCollectionResource");
            }

            // verify the required parameter 'subscribedUser' is set
            if (subscribedUser == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'subscribedUser' when calling UsersApi->PostUserReferenceCollectionResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/vnd.com.nsn.cumulocity.userreference+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.userreference+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            localVarRequestOptions.PathParameters.Add("groupId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(groupId)); // path parameter
            if (accept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(accept)); // header parameter
            }
            localVarRequestOptions.Data = subscribedUser;

            localVarRequestOptions.Operation = "UsersApi.PostUserReferenceCollectionResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.PostAsync<UserReference>("/user/{tenantId}/groups/{groupId}/users", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PostUserReferenceCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Update a specific user&#39;s password of a specific tenant Update a specific user&#39;s password (by a given user ID) of a specific tenant (by a given tenant ID).  Changing the user&#39;s password creates a corresponding audit record of type \&quot;User\&quot; and activity \&quot;User updated\&quot;, and specifying that the password has been changed.  &gt; **⚠️ Important:** If the tenant uses OAI-Secure authentication, the target user will be logged out.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to update root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to update non-root users in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to assigned roles, groups, device permissions and applications&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to update descendants of the current user in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to assigned roles, groups, device permissions and applications &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="passwordChange"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        public void PutUserChangePasswordResource(string tenantId, string userId, PasswordChange passwordChange, string? accept = default(string?), int operationIndex = 0)
        {
            PutUserChangePasswordResourceWithHttpInfo(tenantId, userId, passwordChange, accept);
        }

        /// <summary>
        /// Update a specific user&#39;s password of a specific tenant Update a specific user&#39;s password (by a given user ID) of a specific tenant (by a given tenant ID).  Changing the user&#39;s password creates a corresponding audit record of type \&quot;User\&quot; and activity \&quot;User updated\&quot;, and specifying that the password has been changed.  &gt; **⚠️ Important:** If the tenant uses OAI-Secure authentication, the target user will be logged out.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to update root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to update non-root users in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to assigned roles, groups, device permissions and applications&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to update descendants of the current user in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to assigned roles, groups, device permissions and applications &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="passwordChange"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<Object> PutUserChangePasswordResourceWithHttpInfo(string tenantId, string userId, PasswordChange passwordChange, string? accept = default(string?), int operationIndex = 0)
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->PutUserChangePasswordResource");
            }

            // verify the required parameter 'userId' is set
            if (userId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'userId' when calling UsersApi->PutUserChangePasswordResource");
            }

            // verify the required parameter 'passwordChange' is set
            if (passwordChange == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'passwordChange' when calling UsersApi->PutUserChangePasswordResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            localVarRequestOptions.PathParameters.Add("userId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(userId)); // path parameter
            if (accept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(accept)); // header parameter
            }
            localVarRequestOptions.Data = passwordChange;

            localVarRequestOptions.Operation = "UsersApi.PutUserChangePasswordResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Put<Object>("/user/{tenantId}/users/{userId}/password", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PutUserChangePasswordResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Update a specific user&#39;s password of a specific tenant Update a specific user&#39;s password (by a given user ID) of a specific tenant (by a given tenant ID).  Changing the user&#39;s password creates a corresponding audit record of type \&quot;User\&quot; and activity \&quot;User updated\&quot;, and specifying that the password has been changed.  &gt; **⚠️ Important:** If the tenant uses OAI-Secure authentication, the target user will be logged out.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to update root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to update non-root users in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to assigned roles, groups, device permissions and applications&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to update descendants of the current user in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to assigned roles, groups, device permissions and applications &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="passwordChange"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task PutUserChangePasswordResourceAsync(string tenantId, string userId, PasswordChange passwordChange, string? accept = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            await PutUserChangePasswordResourceWithHttpInfoAsync(tenantId, userId, passwordChange, accept, operationIndex, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Update a specific user&#39;s password of a specific tenant Update a specific user&#39;s password (by a given user ID) of a specific tenant (by a given tenant ID).  Changing the user&#39;s password creates a corresponding audit record of type \&quot;User\&quot; and activity \&quot;User updated\&quot;, and specifying that the password has been changed.  &gt; **⚠️ Important:** If the tenant uses OAI-Secure authentication, the target user will be logged out.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to update root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to update non-root users in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to assigned roles, groups, device permissions and applications&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to update descendants of the current user in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to assigned roles, groups, device permissions and applications &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="passwordChange"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<Object>> PutUserChangePasswordResourceWithHttpInfoAsync(string tenantId, string userId, PasswordChange passwordChange, string? accept = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->PutUserChangePasswordResource");
            }

            // verify the required parameter 'userId' is set
            if (userId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'userId' when calling UsersApi->PutUserChangePasswordResource");
            }

            // verify the required parameter 'passwordChange' is set
            if (passwordChange == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'passwordChange' when calling UsersApi->PutUserChangePasswordResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            localVarRequestOptions.PathParameters.Add("userId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(userId)); // path parameter
            if (accept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(accept)); // header parameter
            }
            localVarRequestOptions.Data = passwordChange;

            localVarRequestOptions.Operation = "UsersApi.PutUserChangePasswordResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.PutAsync<Object>("/user/{tenantId}/users/{userId}/password", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PutUserChangePasswordResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Update a specific user for a specific tenant Update a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  Any change in user&#39;s roles, device permissions and groups creates corresponding audit records with type \&quot;User\&quot; and activity \&quot;User updated\&quot; with information which properties have been changed.  When the user is updated with changed permissions or groups, a corresponding audit record is created with type \&quot;User\&quot; and activity \&quot;User updated\&quot;.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to update root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to update non-root users in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to roles, groups, device permissions and applications being assigned&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to update descendants of the current user in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to roles, groups, device permissions and applications being assigned &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="putUserResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>User</returns>
        public User PutUserResource(string tenantId, string userId, PutUserResourceRequest putUserResourceRequest, string? accept = default(string?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<User> localVarResponse = PutUserResourceWithHttpInfo(tenantId, userId, putUserResourceRequest, accept);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Update a specific user for a specific tenant Update a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  Any change in user&#39;s roles, device permissions and groups creates corresponding audit records with type \&quot;User\&quot; and activity \&quot;User updated\&quot; with information which properties have been changed.  When the user is updated with changed permissions or groups, a corresponding audit record is created with type \&quot;User\&quot; and activity \&quot;User updated\&quot;.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to update root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to update non-root users in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to roles, groups, device permissions and applications being assigned&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to update descendants of the current user in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to roles, groups, device permissions and applications being assigned &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="putUserResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of User</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<User> PutUserResourceWithHttpInfo(string tenantId, string userId, PutUserResourceRequest putUserResourceRequest, string? accept = default(string?), int operationIndex = 0)
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->PutUserResource");
            }

            // verify the required parameter 'userId' is set
            if (userId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'userId' when calling UsersApi->PutUserResource");
            }

            // verify the required parameter 'putUserResourceRequest' is set
            if (putUserResourceRequest == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'putUserResourceRequest' when calling UsersApi->PutUserResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/vnd.com.nsn.cumulocity.user+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.user+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            localVarRequestOptions.PathParameters.Add("userId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(userId)); // path parameter
            if (accept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(accept)); // header parameter
            }
            localVarRequestOptions.Data = putUserResourceRequest;

            localVarRequestOptions.Operation = "UsersApi.PutUserResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = this.Client.Put<User>("/user/{tenantId}/users/{userId}", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PutUserResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Update a specific user for a specific tenant Update a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  Any change in user&#39;s roles, device permissions and groups creates corresponding audit records with type \&quot;User\&quot; and activity \&quot;User updated\&quot; with information which properties have been changed.  When the user is updated with changed permissions or groups, a corresponding audit record is created with type \&quot;User\&quot; and activity \&quot;User updated\&quot;.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to update root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to update non-root users in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to roles, groups, device permissions and applications being assigned&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to update descendants of the current user in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to roles, groups, device permissions and applications being assigned &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="putUserResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of User</returns>
        public async System.Threading.Tasks.Task<User> PutUserResourceAsync(string tenantId, string userId, PutUserResourceRequest putUserResourceRequest, string? accept = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<User> localVarResponse = await PutUserResourceWithHttpInfoAsync(tenantId, userId, putUserResourceRequest, accept, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Update a specific user for a specific tenant Update a specific user (by a given user ID) for a specific tenant (by a given tenant ID).  Any change in user&#39;s roles, device permissions and groups creates corresponding audit records with type \&quot;User\&quot; and activity \&quot;User updated\&quot; with information which properties have been changed.  When the user is updated with changed permissions or groups, a corresponding audit record is created with type \&quot;User\&quot; and activity \&quot;User updated\&quot;.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_USER_MANAGEMENT_ADMIN to update root users in a user hierarchy &lt;b&gt;OR&lt;/b&gt; users that are not in any hierarchy&lt;br/&gt; ROLE_USER_MANAGEMENT_ADMIN to update non-root users in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to roles, groups, device permissions and applications being assigned&lt;br/&gt; ROLE_USER_MANAGEMENT_CREATE to update descendants of the current user in a user hierarchy &lt;b&gt;AND&lt;/b&gt; whose parents have access to roles, groups, device permissions and applications being assigned &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="putUserResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (User)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<User>> PutUserResourceWithHttpInfoAsync(string tenantId, string userId, PutUserResourceRequest putUserResourceRequest, string? accept = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'tenantId' when calling UsersApi->PutUserResource");
            }

            // verify the required parameter 'userId' is set
            if (userId == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'userId' when calling UsersApi->PutUserResource");
            }

            // verify the required parameter 'putUserResourceRequest' is set
            if (putUserResourceRequest == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'putUserResourceRequest' when calling UsersApi->PutUserResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/vnd.com.nsn.cumulocity.user+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.user+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };

            var localVarContentType = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = kern.services.CumulocityClient.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.PathParameters.Add("tenantId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(tenantId)); // path parameter
            localVarRequestOptions.PathParameters.Add("userId", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(userId)); // path parameter
            if (accept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(accept)); // header parameter
            }
            localVarRequestOptions.Data = putUserResourceRequest;

            localVarRequestOptions.Operation = "UsersApi.PutUserResource";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Basic) required
            // http basic authentication required
            if (!string.IsNullOrEmpty(this.Configuration.Username) || !string.IsNullOrEmpty(this.Configuration.Password) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Basic " + kern.services.CumulocityClient.Client.ClientUtils.Base64Encode(this.Configuration.Username + ":" + this.Configuration.Password));
            }
            // authentication (OAI-Secure) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }
            // authentication (SSO) required
            // oauth required
            if (!localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                if (!string.IsNullOrEmpty(this.Configuration.AccessToken))
                {
                    localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
                }
                else if (!string.IsNullOrEmpty(this.Configuration.OAuthTokenUrl) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientId) &&
                         !string.IsNullOrEmpty(this.Configuration.OAuthClientSecret) &&
                         this.Configuration.OAuthFlow != null)
                {
                    localVarRequestOptions.OAuth = true;
                }
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.PutAsync<User>("/user/{tenantId}/users/{userId}", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PutUserResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

    }
}
