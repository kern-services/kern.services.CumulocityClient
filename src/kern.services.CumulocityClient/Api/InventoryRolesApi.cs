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
    public interface IInventoryRolesApi : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Remove a specific inventory role assigned to a user
        /// </summary>
        /// <remarks>
        /// Remove a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN <b>AND</b> (is not in user hierarchy <b>OR</b> is root in the user hierarchy) <b>OR</b> ROLE_USER_MANAGEMENT_ADMIN <b>AND</b> is in user hiararchy <b>AND</b> has parent access to inventory assignments <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is parent of the user <b>AND</b> is not the current user <b>AND</b> has current user access to inventory assignments <b>AND</b> has parent access to inventory assignments </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <returns></returns>
        void DeleteInventoryAssignmentResourceById (string tenantId, string userId, int id);

        /// <summary>
        /// Remove a specific inventory role assigned to a user
        /// </summary>
        /// <remarks>
        /// Remove a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN <b>AND</b> (is not in user hierarchy <b>OR</b> is root in the user hierarchy) <b>OR</b> ROLE_USER_MANAGEMENT_ADMIN <b>AND</b> is in user hiararchy <b>AND</b> has parent access to inventory assignments <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is parent of the user <b>AND</b> is not the current user <b>AND</b> has current user access to inventory assignments <b>AND</b> has parent access to inventory assignments </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DeleteInventoryAssignmentResourceByIdWithHttpInfo (string tenantId, string userId, int id);
        /// <summary>
        /// Remove a specific inventory role
        /// </summary>
        /// <remarks>
        /// Remove a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <returns></returns>
        void DeleteInventoryRoleResourceId (int id);

        /// <summary>
        /// Remove a specific inventory role
        /// </summary>
        /// <remarks>
        /// Remove a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DeleteInventoryRoleResourceIdWithHttpInfo (int id);
        /// <summary>
        /// Retrieve all inventory roles assigned to a user
        /// </summary>
        /// <remarks>
        /// Retrieve all inventory roles assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is the parent of the user </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <returns>InventoryAssignmentCollection</returns>
        InventoryAssignmentCollection GetInventoryAssignmentResource (string tenantId, string userId);

        /// <summary>
        /// Retrieve all inventory roles assigned to a user
        /// </summary>
        /// <remarks>
        /// Retrieve all inventory roles assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is the parent of the user </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <returns>ApiResponse of InventoryAssignmentCollection</returns>
        ApiResponse<InventoryAssignmentCollection> GetInventoryAssignmentResourceWithHttpInfo (string tenantId, string userId);
        /// <summary>
        /// Retrieve a specific inventory role assigned to a user
        /// </summary>
        /// <remarks>
        /// Retrieve a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is the parent of the user </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <returns>InventoryAssignment</returns>
        InventoryAssignment GetInventoryAssignmentResourceById (string tenantId, string userId, int id);

        /// <summary>
        /// Retrieve a specific inventory role assigned to a user
        /// </summary>
        /// <remarks>
        /// Retrieve a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is the parent of the user </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <returns>ApiResponse of InventoryAssignment</returns>
        ApiResponse<InventoryAssignment> GetInventoryAssignmentResourceByIdWithHttpInfo (string tenantId, string userId, int id);
        /// <summary>
        /// Retrieve all inventory roles
        /// </summary>
        /// <remarks>
        /// Retrieve all inventory roles.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <returns>InventoryRoleCollection</returns>
        InventoryRoleCollection GetInventoryRoleResource (int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?));

        /// <summary>
        /// Retrieve all inventory roles
        /// </summary>
        /// <remarks>
        /// Retrieve all inventory roles.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <returns>ApiResponse of InventoryRoleCollection</returns>
        ApiResponse<InventoryRoleCollection> GetInventoryRoleResourceWithHttpInfo (int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?));
        /// <summary>
        /// Retrieve a specific inventory role
        /// </summary>
        /// <remarks>
        /// Retrieve a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> has access to the inventory role </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <returns>InventoryRole</returns>
        InventoryRole GetInventoryRoleResourceId (int id);

        /// <summary>
        /// Retrieve a specific inventory role
        /// </summary>
        /// <remarks>
        /// Retrieve a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> has access to the inventory role </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <returns>ApiResponse of InventoryRole</returns>
        ApiResponse<InventoryRole> GetInventoryRoleResourceIdWithHttpInfo (int id);
        /// <summary>
        /// Assign an inventory role to a user
        /// </summary>
        /// <remarks>
        /// Assign an existing inventory role to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to assign any inventory role to root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to assign inventory roles accessible by the parent of the assigned user to non-root users in a user hierarchy<br/> ROLE_USER_MANAGEMENT_CREATE to assign inventory roles accessible by the current user <b>AND</b> accessible by the parent of the assigned user to the descendants of the current user in a user hierarchy </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="postInventoryAssignmentResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <returns>InventoryAssignment</returns>
        InventoryAssignment PostInventoryAssignmentResource (string tenantId, string userId, PostInventoryAssignmentResourceRequest postInventoryAssignmentResourceRequest, string accept = default(string));

        /// <summary>
        /// Assign an inventory role to a user
        /// </summary>
        /// <remarks>
        /// Assign an existing inventory role to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to assign any inventory role to root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to assign inventory roles accessible by the parent of the assigned user to non-root users in a user hierarchy<br/> ROLE_USER_MANAGEMENT_CREATE to assign inventory roles accessible by the current user <b>AND</b> accessible by the parent of the assigned user to the descendants of the current user in a user hierarchy </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="postInventoryAssignmentResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <returns>ApiResponse of InventoryAssignment</returns>
        ApiResponse<InventoryAssignment> PostInventoryAssignmentResourceWithHttpInfo (string tenantId, string userId, PostInventoryAssignmentResourceRequest postInventoryAssignmentResourceRequest, string accept = default(string));
        /// <summary>
        /// Create an inventory role
        /// </summary>
        /// <remarks>
        /// Create an inventory role.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postInventoryRoleResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <returns>InventoryRole</returns>
        InventoryRole PostInventoryRoleResource (PostInventoryRoleResourceRequest postInventoryRoleResourceRequest, string accept = default(string));

        /// <summary>
        /// Create an inventory role
        /// </summary>
        /// <remarks>
        /// Create an inventory role.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postInventoryRoleResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <returns>ApiResponse of InventoryRole</returns>
        ApiResponse<InventoryRole> PostInventoryRoleResourceWithHttpInfo (PostInventoryRoleResourceRequest postInventoryRoleResourceRequest, string accept = default(string));
        /// <summary>
        /// Update a specific inventory role assigned to a user
        /// </summary>
        /// <remarks>
        /// Update a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to update the assignment of any inventory roles to root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to update the assignment of inventory roles accessible by the assigned user parent, to non-root users in a user hierarchy<br/> ROLE_USER_MANAGEMENT_CREATE to update the assignment of inventory roles accessible by the current user <b>AND</b> the parent of the assigned user to the descendants of the current user in the user hierarchy </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <param name="inventoryAssignmentReference"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <returns>InventoryAssignment</returns>
        InventoryAssignment PutInventoryAssignmentResourceById (string tenantId, string userId, int id, InventoryAssignmentReference inventoryAssignmentReference, string accept = default(string));

        /// <summary>
        /// Update a specific inventory role assigned to a user
        /// </summary>
        /// <remarks>
        /// Update a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to update the assignment of any inventory roles to root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to update the assignment of inventory roles accessible by the assigned user parent, to non-root users in a user hierarchy<br/> ROLE_USER_MANAGEMENT_CREATE to update the assignment of inventory roles accessible by the current user <b>AND</b> the parent of the assigned user to the descendants of the current user in the user hierarchy </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <param name="inventoryAssignmentReference"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <returns>ApiResponse of InventoryAssignment</returns>
        ApiResponse<InventoryAssignment> PutInventoryAssignmentResourceByIdWithHttpInfo (string tenantId, string userId, int id, InventoryAssignmentReference inventoryAssignmentReference, string accept = default(string));
        /// <summary>
        /// Update a specific inventory role
        /// </summary>
        /// <remarks>
        /// Update a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <param name="inventoryRole"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <returns>InventoryRole</returns>
        InventoryRole PutInventoryRoleResourceId (int id, InventoryRole inventoryRole, string accept = default(string));

        /// <summary>
        /// Update a specific inventory role
        /// </summary>
        /// <remarks>
        /// Update a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <param name="inventoryRole"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <returns>ApiResponse of InventoryRole</returns>
        ApiResponse<InventoryRole> PutInventoryRoleResourceIdWithHttpInfo (int id, InventoryRole inventoryRole, string accept = default(string));
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Remove a specific inventory role assigned to a user
        /// </summary>
        /// <remarks>
        /// Remove a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN <b>AND</b> (is not in user hierarchy <b>OR</b> is root in the user hierarchy) <b>OR</b> ROLE_USER_MANAGEMENT_ADMIN <b>AND</b> is in user hiararchy <b>AND</b> has parent access to inventory assignments <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is parent of the user <b>AND</b> is not the current user <b>AND</b> has current user access to inventory assignments <b>AND</b> has parent access to inventory assignments </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DeleteInventoryAssignmentResourceByIdAsync (string tenantId, string userId, int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Remove a specific inventory role assigned to a user
        /// </summary>
        /// <remarks>
        /// Remove a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN <b>AND</b> (is not in user hierarchy <b>OR</b> is root in the user hierarchy) <b>OR</b> ROLE_USER_MANAGEMENT_ADMIN <b>AND</b> is in user hiararchy <b>AND</b> has parent access to inventory assignments <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is parent of the user <b>AND</b> is not the current user <b>AND</b> has current user access to inventory assignments <b>AND</b> has parent access to inventory assignments </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DeleteInventoryAssignmentResourceByIdWithHttpInfoAsync (string tenantId, string userId, int id, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Remove a specific inventory role
        /// </summary>
        /// <remarks>
        /// Remove a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DeleteInventoryRoleResourceIdAsync (int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Remove a specific inventory role
        /// </summary>
        /// <remarks>
        /// Remove a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DeleteInventoryRoleResourceIdWithHttpInfoAsync (int id, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Retrieve all inventory roles assigned to a user
        /// </summary>
        /// <remarks>
        /// Retrieve all inventory roles assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is the parent of the user </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of InventoryAssignmentCollection</returns>
        System.Threading.Tasks.Task<InventoryAssignmentCollection> GetInventoryAssignmentResourceAsync (string tenantId, string userId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieve all inventory roles assigned to a user
        /// </summary>
        /// <remarks>
        /// Retrieve all inventory roles assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is the parent of the user </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (InventoryAssignmentCollection)</returns>
        System.Threading.Tasks.Task<ApiResponse<InventoryAssignmentCollection>> GetInventoryAssignmentResourceWithHttpInfoAsync (string tenantId, string userId, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Retrieve a specific inventory role assigned to a user
        /// </summary>
        /// <remarks>
        /// Retrieve a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is the parent of the user </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of InventoryAssignment</returns>
        System.Threading.Tasks.Task<InventoryAssignment> GetInventoryAssignmentResourceByIdAsync (string tenantId, string userId, int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieve a specific inventory role assigned to a user
        /// </summary>
        /// <remarks>
        /// Retrieve a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is the parent of the user </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (InventoryAssignment)</returns>
        System.Threading.Tasks.Task<ApiResponse<InventoryAssignment>> GetInventoryAssignmentResourceByIdWithHttpInfoAsync (string tenantId, string userId, int id, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Retrieve all inventory roles
        /// </summary>
        /// <remarks>
        /// Retrieve all inventory roles.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of InventoryRoleCollection</returns>
        System.Threading.Tasks.Task<InventoryRoleCollection> GetInventoryRoleResourceAsync (int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieve all inventory roles
        /// </summary>
        /// <remarks>
        /// Retrieve all inventory roles.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (InventoryRoleCollection)</returns>
        System.Threading.Tasks.Task<ApiResponse<InventoryRoleCollection>> GetInventoryRoleResourceWithHttpInfoAsync (int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Retrieve a specific inventory role
        /// </summary>
        /// <remarks>
        /// Retrieve a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> has access to the inventory role </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of InventoryRole</returns>
        System.Threading.Tasks.Task<InventoryRole> GetInventoryRoleResourceIdAsync (int id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieve a specific inventory role
        /// </summary>
        /// <remarks>
        /// Retrieve a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> has access to the inventory role </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (InventoryRole)</returns>
        System.Threading.Tasks.Task<ApiResponse<InventoryRole>> GetInventoryRoleResourceIdWithHttpInfoAsync (int id, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Assign an inventory role to a user
        /// </summary>
        /// <remarks>
        /// Assign an existing inventory role to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to assign any inventory role to root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to assign inventory roles accessible by the parent of the assigned user to non-root users in a user hierarchy<br/> ROLE_USER_MANAGEMENT_CREATE to assign inventory roles accessible by the current user <b>AND</b> accessible by the parent of the assigned user to the descendants of the current user in a user hierarchy </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="postInventoryAssignmentResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of InventoryAssignment</returns>
        System.Threading.Tasks.Task<InventoryAssignment> PostInventoryAssignmentResourceAsync (string tenantId, string userId, PostInventoryAssignmentResourceRequest postInventoryAssignmentResourceRequest, string accept = default(string), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Assign an inventory role to a user
        /// </summary>
        /// <remarks>
        /// Assign an existing inventory role to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to assign any inventory role to root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to assign inventory roles accessible by the parent of the assigned user to non-root users in a user hierarchy<br/> ROLE_USER_MANAGEMENT_CREATE to assign inventory roles accessible by the current user <b>AND</b> accessible by the parent of the assigned user to the descendants of the current user in a user hierarchy </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="postInventoryAssignmentResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (InventoryAssignment)</returns>
        System.Threading.Tasks.Task<ApiResponse<InventoryAssignment>> PostInventoryAssignmentResourceWithHttpInfoAsync (string tenantId, string userId, PostInventoryAssignmentResourceRequest postInventoryAssignmentResourceRequest, string accept = default(string), CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Create an inventory role
        /// </summary>
        /// <remarks>
        /// Create an inventory role.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postInventoryRoleResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of InventoryRole</returns>
        System.Threading.Tasks.Task<InventoryRole> PostInventoryRoleResourceAsync (PostInventoryRoleResourceRequest postInventoryRoleResourceRequest, string accept = default(string), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Create an inventory role
        /// </summary>
        /// <remarks>
        /// Create an inventory role.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postInventoryRoleResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (InventoryRole)</returns>
        System.Threading.Tasks.Task<ApiResponse<InventoryRole>> PostInventoryRoleResourceWithHttpInfoAsync (PostInventoryRoleResourceRequest postInventoryRoleResourceRequest, string accept = default(string), CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Update a specific inventory role assigned to a user
        /// </summary>
        /// <remarks>
        /// Update a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to update the assignment of any inventory roles to root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to update the assignment of inventory roles accessible by the assigned user parent, to non-root users in a user hierarchy<br/> ROLE_USER_MANAGEMENT_CREATE to update the assignment of inventory roles accessible by the current user <b>AND</b> the parent of the assigned user to the descendants of the current user in the user hierarchy </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <param name="inventoryAssignmentReference"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of InventoryAssignment</returns>
        System.Threading.Tasks.Task<InventoryAssignment> PutInventoryAssignmentResourceByIdAsync (string tenantId, string userId, int id, InventoryAssignmentReference inventoryAssignmentReference, string accept = default(string), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update a specific inventory role assigned to a user
        /// </summary>
        /// <remarks>
        /// Update a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to update the assignment of any inventory roles to root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to update the assignment of inventory roles accessible by the assigned user parent, to non-root users in a user hierarchy<br/> ROLE_USER_MANAGEMENT_CREATE to update the assignment of inventory roles accessible by the current user <b>AND</b> the parent of the assigned user to the descendants of the current user in the user hierarchy </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <param name="inventoryAssignmentReference"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (InventoryAssignment)</returns>
        System.Threading.Tasks.Task<ApiResponse<InventoryAssignment>> PutInventoryAssignmentResourceByIdWithHttpInfoAsync (string tenantId, string userId, int id, InventoryAssignmentReference inventoryAssignmentReference, string accept = default(string), CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Update a specific inventory role
        /// </summary>
        /// <remarks>
        /// Update a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <param name="inventoryRole"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of InventoryRole</returns>
        System.Threading.Tasks.Task<InventoryRole> PutInventoryRoleResourceIdAsync (int id, InventoryRole inventoryRole, string accept = default(string), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update a specific inventory role
        /// </summary>
        /// <remarks>
        /// Update a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <param name="inventoryRole"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (InventoryRole)</returns>
        System.Threading.Tasks.Task<ApiResponse<InventoryRole>> PutInventoryRoleResourceIdWithHttpInfoAsync (int id, InventoryRole inventoryRole, string accept = default(string), CancellationToken cancellationToken = default(CancellationToken));
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class InventoryRolesApi : IInventoryRolesApi
    {
        private kern.services.CumulocityClient.Client.ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryRolesApi"/> class.
        /// </summary>
        /// <returns></returns>
        public InventoryRolesApi(String basePath)
        {
            this.Configuration = new kern.services.CumulocityClient.Client.Configuration { BasePath = basePath };

            ExceptionFactory = kern.services.CumulocityClient.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryRolesApi"/> class
        /// </summary>
        /// <returns></returns>
        public InventoryRolesApi()
        {
            this.Configuration = kern.services.CumulocityClient.Client.Configuration.Default;

            ExceptionFactory = kern.services.CumulocityClient.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryRolesApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public InventoryRolesApi(kern.services.CumulocityClient.Client.Configuration configuration = null)
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
        /// Remove a specific inventory role assigned to a user Remove a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN <b>AND</b> (is not in user hierarchy <b>OR</b> is root in the user hierarchy) <b>OR</b> ROLE_USER_MANAGEMENT_ADMIN <b>AND</b> is in user hiararchy <b>AND</b> has parent access to inventory assignments <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is parent of the user <b>AND</b> is not the current user <b>AND</b> has current user access to inventory assignments <b>AND</b> has parent access to inventory assignments </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <returns></returns>
        public void DeleteInventoryAssignmentResourceById (string tenantId, string userId, int id)
        {
             DeleteInventoryAssignmentResourceByIdWithHttpInfo(tenantId, userId, id);
        }

        /// <summary>
        /// Remove a specific inventory role assigned to a user Remove a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN <b>AND</b> (is not in user hierarchy <b>OR</b> is root in the user hierarchy) <b>OR</b> ROLE_USER_MANAGEMENT_ADMIN <b>AND</b> is in user hiararchy <b>AND</b> has parent access to inventory assignments <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is parent of the user <b>AND</b> is not the current user <b>AND</b> has current user access to inventory assignments <b>AND</b> has parent access to inventory assignments </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> DeleteInventoryAssignmentResourceByIdWithHttpInfo (string tenantId, string userId, int id)
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
                throw new ApiException(400, "Missing required parameter 'tenantId' when calling InventoryRolesApi->DeleteInventoryAssignmentResourceById");
            // verify the required parameter 'userId' is set
            if (userId == null)
                throw new ApiException(400, "Missing required parameter 'userId' when calling InventoryRolesApi->DeleteInventoryAssignmentResourceById");
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling InventoryRolesApi->DeleteInventoryAssignmentResourceById");

            var localVarPath = "/user/{tenantId}/users/{userId}/roles/inventory/{id}";
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

            if (tenantId != null) localVarPathParams.Add("tenantId", this.Configuration.ApiClient.ParameterToString(tenantId)); // path parameter
            if (userId != null) localVarPathParams.Add("userId", this.Configuration.ApiClient.ParameterToString(userId)); // path parameter
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
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("DeleteInventoryAssignmentResourceById", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                null);
        }

        /// <summary>
        /// Remove a specific inventory role assigned to a user Remove a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN <b>AND</b> (is not in user hierarchy <b>OR</b> is root in the user hierarchy) <b>OR</b> ROLE_USER_MANAGEMENT_ADMIN <b>AND</b> is in user hiararchy <b>AND</b> has parent access to inventory assignments <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is parent of the user <b>AND</b> is not the current user <b>AND</b> has current user access to inventory assignments <b>AND</b> has parent access to inventory assignments </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DeleteInventoryAssignmentResourceByIdAsync (string tenantId, string userId, int id, CancellationToken cancellationToken = default(CancellationToken))
        {
             await DeleteInventoryAssignmentResourceByIdWithHttpInfoAsync(tenantId, userId, id, cancellationToken);

        }

        /// <summary>
        /// Remove a specific inventory role assigned to a user Remove a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN <b>AND</b> (is not in user hierarchy <b>OR</b> is root in the user hierarchy) <b>OR</b> ROLE_USER_MANAGEMENT_ADMIN <b>AND</b> is in user hiararchy <b>AND</b> has parent access to inventory assignments <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is parent of the user <b>AND</b> is not the current user <b>AND</b> has current user access to inventory assignments <b>AND</b> has parent access to inventory assignments </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> DeleteInventoryAssignmentResourceByIdWithHttpInfoAsync (string tenantId, string userId, int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
                throw new ApiException(400, "Missing required parameter 'tenantId' when calling InventoryRolesApi->DeleteInventoryAssignmentResourceById");
            // verify the required parameter 'userId' is set
            if (userId == null)
                throw new ApiException(400, "Missing required parameter 'userId' when calling InventoryRolesApi->DeleteInventoryAssignmentResourceById");
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling InventoryRolesApi->DeleteInventoryAssignmentResourceById");

            var localVarPath = "/user/{tenantId}/users/{userId}/roles/inventory/{id}";
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

            if (tenantId != null) localVarPathParams.Add("tenantId", this.Configuration.ApiClient.ParameterToString(tenantId)); // path parameter
            if (userId != null) localVarPathParams.Add("userId", this.Configuration.ApiClient.ParameterToString(userId)); // path parameter
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
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType, cancellationToken);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("DeleteInventoryAssignmentResourceById", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                null);
        }

        /// <summary>
        /// Remove a specific inventory role Remove a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <returns></returns>
        public void DeleteInventoryRoleResourceId (int id)
        {
             DeleteInventoryRoleResourceIdWithHttpInfo(id);
        }

        /// <summary>
        /// Remove a specific inventory role Remove a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> DeleteInventoryRoleResourceIdWithHttpInfo (int id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling InventoryRolesApi->DeleteInventoryRoleResourceId");

            var localVarPath = "/user/inventoryroles/{id}";
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
                Exception exception = ExceptionFactory("DeleteInventoryRoleResourceId", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                null);
        }

        /// <summary>
        /// Remove a specific inventory role Remove a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DeleteInventoryRoleResourceIdAsync (int id, CancellationToken cancellationToken = default(CancellationToken))
        {
             await DeleteInventoryRoleResourceIdWithHttpInfoAsync(id, cancellationToken);

        }

        /// <summary>
        /// Remove a specific inventory role Remove a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> DeleteInventoryRoleResourceIdWithHttpInfoAsync (int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling InventoryRolesApi->DeleteInventoryRoleResourceId");

            var localVarPath = "/user/inventoryroles/{id}";
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
                Exception exception = ExceptionFactory("DeleteInventoryRoleResourceId", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                null);
        }

        /// <summary>
        /// Retrieve all inventory roles assigned to a user Retrieve all inventory roles assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is the parent of the user </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <returns>InventoryAssignmentCollection</returns>
        public InventoryAssignmentCollection GetInventoryAssignmentResource (string tenantId, string userId)
        {
             ApiResponse<InventoryAssignmentCollection> localVarResponse = GetInventoryAssignmentResourceWithHttpInfo(tenantId, userId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve all inventory roles assigned to a user Retrieve all inventory roles assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is the parent of the user </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <returns>ApiResponse of InventoryAssignmentCollection</returns>
        public ApiResponse<InventoryAssignmentCollection> GetInventoryAssignmentResourceWithHttpInfo (string tenantId, string userId)
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
                throw new ApiException(400, "Missing required parameter 'tenantId' when calling InventoryRolesApi->GetInventoryAssignmentResource");
            // verify the required parameter 'userId' is set
            if (userId == null)
                throw new ApiException(400, "Missing required parameter 'userId' when calling InventoryRolesApi->GetInventoryAssignmentResource");

            var localVarPath = "/user/{tenantId}/users/{userId}/roles/inventory";
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
                "application/vnd.com.nsn.cumulocity.inventoryassignmentcollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (tenantId != null) localVarPathParams.Add("tenantId", this.Configuration.ApiClient.ParameterToString(tenantId)); // path parameter
            if (userId != null) localVarPathParams.Add("userId", this.Configuration.ApiClient.ParameterToString(userId)); // path parameter

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
                Exception exception = ExceptionFactory("GetInventoryAssignmentResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<InventoryAssignmentCollection>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (InventoryAssignmentCollection) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(InventoryAssignmentCollection)));
        }

        /// <summary>
        /// Retrieve all inventory roles assigned to a user Retrieve all inventory roles assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is the parent of the user </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of InventoryAssignmentCollection</returns>
        public async System.Threading.Tasks.Task<InventoryAssignmentCollection> GetInventoryAssignmentResourceAsync (string tenantId, string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<InventoryAssignmentCollection> localVarResponse = await GetInventoryAssignmentResourceWithHttpInfoAsync(tenantId, userId, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Retrieve all inventory roles assigned to a user Retrieve all inventory roles assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is the parent of the user </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (InventoryAssignmentCollection)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<InventoryAssignmentCollection>> GetInventoryAssignmentResourceWithHttpInfoAsync (string tenantId, string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
                throw new ApiException(400, "Missing required parameter 'tenantId' when calling InventoryRolesApi->GetInventoryAssignmentResource");
            // verify the required parameter 'userId' is set
            if (userId == null)
                throw new ApiException(400, "Missing required parameter 'userId' when calling InventoryRolesApi->GetInventoryAssignmentResource");

            var localVarPath = "/user/{tenantId}/users/{userId}/roles/inventory";
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
                "application/vnd.com.nsn.cumulocity.inventoryassignmentcollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (tenantId != null) localVarPathParams.Add("tenantId", this.Configuration.ApiClient.ParameterToString(tenantId)); // path parameter
            if (userId != null) localVarPathParams.Add("userId", this.Configuration.ApiClient.ParameterToString(userId)); // path parameter

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
                Exception exception = ExceptionFactory("GetInventoryAssignmentResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<InventoryAssignmentCollection>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (InventoryAssignmentCollection) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(InventoryAssignmentCollection)));
        }

        /// <summary>
        /// Retrieve a specific inventory role assigned to a user Retrieve a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is the parent of the user </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <returns>InventoryAssignment</returns>
        public InventoryAssignment GetInventoryAssignmentResourceById (string tenantId, string userId, int id)
        {
             ApiResponse<InventoryAssignment> localVarResponse = GetInventoryAssignmentResourceByIdWithHttpInfo(tenantId, userId, id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve a specific inventory role assigned to a user Retrieve a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is the parent of the user </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <returns>ApiResponse of InventoryAssignment</returns>
        public ApiResponse<InventoryAssignment> GetInventoryAssignmentResourceByIdWithHttpInfo (string tenantId, string userId, int id)
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
                throw new ApiException(400, "Missing required parameter 'tenantId' when calling InventoryRolesApi->GetInventoryAssignmentResourceById");
            // verify the required parameter 'userId' is set
            if (userId == null)
                throw new ApiException(400, "Missing required parameter 'userId' when calling InventoryRolesApi->GetInventoryAssignmentResourceById");
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling InventoryRolesApi->GetInventoryAssignmentResourceById");

            var localVarPath = "/user/{tenantId}/users/{userId}/roles/inventory/{id}";
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
                "application/vnd.com.nsn.cumulocity.inventoryassignment+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (tenantId != null) localVarPathParams.Add("tenantId", this.Configuration.ApiClient.ParameterToString(tenantId)); // path parameter
            if (userId != null) localVarPathParams.Add("userId", this.Configuration.ApiClient.ParameterToString(userId)); // path parameter
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
                Exception exception = ExceptionFactory("GetInventoryAssignmentResourceById", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<InventoryAssignment>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (InventoryAssignment) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(InventoryAssignment)));
        }

        /// <summary>
        /// Retrieve a specific inventory role assigned to a user Retrieve a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is the parent of the user </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of InventoryAssignment</returns>
        public async System.Threading.Tasks.Task<InventoryAssignment> GetInventoryAssignmentResourceByIdAsync (string tenantId, string userId, int id, CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<InventoryAssignment> localVarResponse = await GetInventoryAssignmentResourceByIdWithHttpInfoAsync(tenantId, userId, id, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Retrieve a specific inventory role assigned to a user Retrieve a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> is the parent of the user </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (InventoryAssignment)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<InventoryAssignment>> GetInventoryAssignmentResourceByIdWithHttpInfoAsync (string tenantId, string userId, int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
                throw new ApiException(400, "Missing required parameter 'tenantId' when calling InventoryRolesApi->GetInventoryAssignmentResourceById");
            // verify the required parameter 'userId' is set
            if (userId == null)
                throw new ApiException(400, "Missing required parameter 'userId' when calling InventoryRolesApi->GetInventoryAssignmentResourceById");
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling InventoryRolesApi->GetInventoryAssignmentResourceById");

            var localVarPath = "/user/{tenantId}/users/{userId}/roles/inventory/{id}";
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
                "application/vnd.com.nsn.cumulocity.inventoryassignment+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (tenantId != null) localVarPathParams.Add("tenantId", this.Configuration.ApiClient.ParameterToString(tenantId)); // path parameter
            if (userId != null) localVarPathParams.Add("userId", this.Configuration.ApiClient.ParameterToString(userId)); // path parameter
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
                Exception exception = ExceptionFactory("GetInventoryAssignmentResourceById", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<InventoryAssignment>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (InventoryAssignment) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(InventoryAssignment)));
        }

        /// <summary>
        /// Retrieve all inventory roles Retrieve all inventory roles.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <returns>InventoryRoleCollection</returns>
        public InventoryRoleCollection GetInventoryRoleResource (int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?))
        {
             ApiResponse<InventoryRoleCollection> localVarResponse = GetInventoryRoleResourceWithHttpInfo(currentPage, pageSize, withTotalElements);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve all inventory roles Retrieve all inventory roles.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <returns>ApiResponse of InventoryRoleCollection</returns>
        public ApiResponse<InventoryRoleCollection> GetInventoryRoleResourceWithHttpInfo (int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?))
        {

            var localVarPath = "/user/inventoryroles";
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
                "application/vnd.com.nsn.cumulocity.inventoryrolecollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (currentPage != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "currentPage", currentPage)); // query parameter
            if (pageSize != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "pageSize", pageSize)); // query parameter
            if (withTotalElements != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withTotalElements", withTotalElements)); // query parameter

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
                Exception exception = ExceptionFactory("GetInventoryRoleResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<InventoryRoleCollection>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (InventoryRoleCollection) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(InventoryRoleCollection)));
        }

        /// <summary>
        /// Retrieve all inventory roles Retrieve all inventory roles.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of InventoryRoleCollection</returns>
        public async System.Threading.Tasks.Task<InventoryRoleCollection> GetInventoryRoleResourceAsync (int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<InventoryRoleCollection> localVarResponse = await GetInventoryRoleResourceWithHttpInfoAsync(currentPage, pageSize, withTotalElements, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Retrieve all inventory roles Retrieve all inventory roles.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (InventoryRoleCollection)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<InventoryRoleCollection>> GetInventoryRoleResourceWithHttpInfoAsync (int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), CancellationToken cancellationToken = default(CancellationToken))
        {

            var localVarPath = "/user/inventoryroles";
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
                "application/vnd.com.nsn.cumulocity.inventoryrolecollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (currentPage != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "currentPage", currentPage)); // query parameter
            if (pageSize != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "pageSize", pageSize)); // query parameter
            if (withTotalElements != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withTotalElements", withTotalElements)); // query parameter

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
                Exception exception = ExceptionFactory("GetInventoryRoleResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<InventoryRoleCollection>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (InventoryRoleCollection) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(InventoryRoleCollection)));
        }

        /// <summary>
        /// Retrieve a specific inventory role Retrieve a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> has access to the inventory role </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <returns>InventoryRole</returns>
        public InventoryRole GetInventoryRoleResourceId (int id)
        {
             ApiResponse<InventoryRole> localVarResponse = GetInventoryRoleResourceIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve a specific inventory role Retrieve a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> has access to the inventory role </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <returns>ApiResponse of InventoryRole</returns>
        public ApiResponse<InventoryRole> GetInventoryRoleResourceIdWithHttpInfo (int id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling InventoryRolesApi->GetInventoryRoleResourceId");

            var localVarPath = "/user/inventoryroles/{id}";
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
                "application/vnd.com.nsn.cumulocity.inventoryrole+json",
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
                Exception exception = ExceptionFactory("GetInventoryRoleResourceId", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<InventoryRole>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (InventoryRole) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(InventoryRole)));
        }

        /// <summary>
        /// Retrieve a specific inventory role Retrieve a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> has access to the inventory role </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of InventoryRole</returns>
        public async System.Threading.Tasks.Task<InventoryRole> GetInventoryRoleResourceIdAsync (int id, CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<InventoryRole> localVarResponse = await GetInventoryRoleResourceIdWithHttpInfoAsync(id, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Retrieve a specific inventory role Retrieve a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_READ <b>OR</b> ROLE_USER_MANAGEMENT_CREATE <b>AND</b> has access to the inventory role </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (InventoryRole)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<InventoryRole>> GetInventoryRoleResourceIdWithHttpInfoAsync (int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling InventoryRolesApi->GetInventoryRoleResourceId");

            var localVarPath = "/user/inventoryroles/{id}";
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
                "application/vnd.com.nsn.cumulocity.inventoryrole+json",
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
                Exception exception = ExceptionFactory("GetInventoryRoleResourceId", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<InventoryRole>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (InventoryRole) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(InventoryRole)));
        }

        /// <summary>
        /// Assign an inventory role to a user Assign an existing inventory role to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to assign any inventory role to root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to assign inventory roles accessible by the parent of the assigned user to non-root users in a user hierarchy<br/> ROLE_USER_MANAGEMENT_CREATE to assign inventory roles accessible by the current user <b>AND</b> accessible by the parent of the assigned user to the descendants of the current user in a user hierarchy </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="postInventoryAssignmentResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <returns>InventoryAssignment</returns>
        public InventoryAssignment PostInventoryAssignmentResource (string tenantId, string userId, PostInventoryAssignmentResourceRequest postInventoryAssignmentResourceRequest, string accept = default(string))
        {
             ApiResponse<InventoryAssignment> localVarResponse = PostInventoryAssignmentResourceWithHttpInfo(tenantId, userId, postInventoryAssignmentResourceRequest, accept);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Assign an inventory role to a user Assign an existing inventory role to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to assign any inventory role to root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to assign inventory roles accessible by the parent of the assigned user to non-root users in a user hierarchy<br/> ROLE_USER_MANAGEMENT_CREATE to assign inventory roles accessible by the current user <b>AND</b> accessible by the parent of the assigned user to the descendants of the current user in a user hierarchy </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="postInventoryAssignmentResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <returns>ApiResponse of InventoryAssignment</returns>
        public ApiResponse<InventoryAssignment> PostInventoryAssignmentResourceWithHttpInfo (string tenantId, string userId, PostInventoryAssignmentResourceRequest postInventoryAssignmentResourceRequest, string accept = default(string))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
                throw new ApiException(400, "Missing required parameter 'tenantId' when calling InventoryRolesApi->PostInventoryAssignmentResource");
            // verify the required parameter 'userId' is set
            if (userId == null)
                throw new ApiException(400, "Missing required parameter 'userId' when calling InventoryRolesApi->PostInventoryAssignmentResource");
            // verify the required parameter 'postInventoryAssignmentResourceRequest' is set
            if (postInventoryAssignmentResourceRequest == null)
                throw new ApiException(400, "Missing required parameter 'postInventoryAssignmentResourceRequest' when calling InventoryRolesApi->PostInventoryAssignmentResource");

            var localVarPath = "/user/{tenantId}/users/{userId}/roles/inventory";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.inventoryassignment+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.inventoryassignment+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (tenantId != null) localVarPathParams.Add("tenantId", this.Configuration.ApiClient.ParameterToString(tenantId)); // path parameter
            if (userId != null) localVarPathParams.Add("userId", this.Configuration.ApiClient.ParameterToString(userId)); // path parameter
            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (postInventoryAssignmentResourceRequest != null && postInventoryAssignmentResourceRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(postInventoryAssignmentResourceRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = postInventoryAssignmentResourceRequest; // byte array
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
                Exception exception = ExceptionFactory("PostInventoryAssignmentResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<InventoryAssignment>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (InventoryAssignment) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(InventoryAssignment)));
        }

        /// <summary>
        /// Assign an inventory role to a user Assign an existing inventory role to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to assign any inventory role to root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to assign inventory roles accessible by the parent of the assigned user to non-root users in a user hierarchy<br/> ROLE_USER_MANAGEMENT_CREATE to assign inventory roles accessible by the current user <b>AND</b> accessible by the parent of the assigned user to the descendants of the current user in a user hierarchy </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="postInventoryAssignmentResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of InventoryAssignment</returns>
        public async System.Threading.Tasks.Task<InventoryAssignment> PostInventoryAssignmentResourceAsync (string tenantId, string userId, PostInventoryAssignmentResourceRequest postInventoryAssignmentResourceRequest, string accept = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<InventoryAssignment> localVarResponse = await PostInventoryAssignmentResourceWithHttpInfoAsync(tenantId, userId, postInventoryAssignmentResourceRequest, accept, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Assign an inventory role to a user Assign an existing inventory role to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to assign any inventory role to root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to assign inventory roles accessible by the parent of the assigned user to non-root users in a user hierarchy<br/> ROLE_USER_MANAGEMENT_CREATE to assign inventory roles accessible by the current user <b>AND</b> accessible by the parent of the assigned user to the descendants of the current user in a user hierarchy </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="postInventoryAssignmentResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (InventoryAssignment)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<InventoryAssignment>> PostInventoryAssignmentResourceWithHttpInfoAsync (string tenantId, string userId, PostInventoryAssignmentResourceRequest postInventoryAssignmentResourceRequest, string accept = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
                throw new ApiException(400, "Missing required parameter 'tenantId' when calling InventoryRolesApi->PostInventoryAssignmentResource");
            // verify the required parameter 'userId' is set
            if (userId == null)
                throw new ApiException(400, "Missing required parameter 'userId' when calling InventoryRolesApi->PostInventoryAssignmentResource");
            // verify the required parameter 'postInventoryAssignmentResourceRequest' is set
            if (postInventoryAssignmentResourceRequest == null)
                throw new ApiException(400, "Missing required parameter 'postInventoryAssignmentResourceRequest' when calling InventoryRolesApi->PostInventoryAssignmentResource");

            var localVarPath = "/user/{tenantId}/users/{userId}/roles/inventory";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.inventoryassignment+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.inventoryassignment+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (tenantId != null) localVarPathParams.Add("tenantId", this.Configuration.ApiClient.ParameterToString(tenantId)); // path parameter
            if (userId != null) localVarPathParams.Add("userId", this.Configuration.ApiClient.ParameterToString(userId)); // path parameter
            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (postInventoryAssignmentResourceRequest != null && postInventoryAssignmentResourceRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(postInventoryAssignmentResourceRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = postInventoryAssignmentResourceRequest; // byte array
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
                Exception exception = ExceptionFactory("PostInventoryAssignmentResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<InventoryAssignment>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (InventoryAssignment) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(InventoryAssignment)));
        }

        /// <summary>
        /// Create an inventory role Create an inventory role.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postInventoryRoleResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <returns>InventoryRole</returns>
        public InventoryRole PostInventoryRoleResource (PostInventoryRoleResourceRequest postInventoryRoleResourceRequest, string accept = default(string))
        {
             ApiResponse<InventoryRole> localVarResponse = PostInventoryRoleResourceWithHttpInfo(postInventoryRoleResourceRequest, accept);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create an inventory role Create an inventory role.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postInventoryRoleResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <returns>ApiResponse of InventoryRole</returns>
        public ApiResponse<InventoryRole> PostInventoryRoleResourceWithHttpInfo (PostInventoryRoleResourceRequest postInventoryRoleResourceRequest, string accept = default(string))
        {
            // verify the required parameter 'postInventoryRoleResourceRequest' is set
            if (postInventoryRoleResourceRequest == null)
                throw new ApiException(400, "Missing required parameter 'postInventoryRoleResourceRequest' when calling InventoryRolesApi->PostInventoryRoleResource");

            var localVarPath = "/user/inventoryroles";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.inventoryrole+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.inventoryrole+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (postInventoryRoleResourceRequest != null && postInventoryRoleResourceRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(postInventoryRoleResourceRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = postInventoryRoleResourceRequest; // byte array
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
                Exception exception = ExceptionFactory("PostInventoryRoleResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<InventoryRole>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (InventoryRole) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(InventoryRole)));
        }

        /// <summary>
        /// Create an inventory role Create an inventory role.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postInventoryRoleResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of InventoryRole</returns>
        public async System.Threading.Tasks.Task<InventoryRole> PostInventoryRoleResourceAsync (PostInventoryRoleResourceRequest postInventoryRoleResourceRequest, string accept = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<InventoryRole> localVarResponse = await PostInventoryRoleResourceWithHttpInfoAsync(postInventoryRoleResourceRequest, accept, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create an inventory role Create an inventory role.  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postInventoryRoleResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (InventoryRole)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<InventoryRole>> PostInventoryRoleResourceWithHttpInfoAsync (PostInventoryRoleResourceRequest postInventoryRoleResourceRequest, string accept = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'postInventoryRoleResourceRequest' is set
            if (postInventoryRoleResourceRequest == null)
                throw new ApiException(400, "Missing required parameter 'postInventoryRoleResourceRequest' when calling InventoryRolesApi->PostInventoryRoleResource");

            var localVarPath = "/user/inventoryroles";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.inventoryrole+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.inventoryrole+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (postInventoryRoleResourceRequest != null && postInventoryRoleResourceRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(postInventoryRoleResourceRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = postInventoryRoleResourceRequest; // byte array
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
                Exception exception = ExceptionFactory("PostInventoryRoleResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<InventoryRole>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (InventoryRole) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(InventoryRole)));
        }

        /// <summary>
        /// Update a specific inventory role assigned to a user Update a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to update the assignment of any inventory roles to root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to update the assignment of inventory roles accessible by the assigned user parent, to non-root users in a user hierarchy<br/> ROLE_USER_MANAGEMENT_CREATE to update the assignment of inventory roles accessible by the current user <b>AND</b> the parent of the assigned user to the descendants of the current user in the user hierarchy </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <param name="inventoryAssignmentReference"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <returns>InventoryAssignment</returns>
        public InventoryAssignment PutInventoryAssignmentResourceById (string tenantId, string userId, int id, InventoryAssignmentReference inventoryAssignmentReference, string accept = default(string))
        {
             ApiResponse<InventoryAssignment> localVarResponse = PutInventoryAssignmentResourceByIdWithHttpInfo(tenantId, userId, id, inventoryAssignmentReference, accept);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Update a specific inventory role assigned to a user Update a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to update the assignment of any inventory roles to root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to update the assignment of inventory roles accessible by the assigned user parent, to non-root users in a user hierarchy<br/> ROLE_USER_MANAGEMENT_CREATE to update the assignment of inventory roles accessible by the current user <b>AND</b> the parent of the assigned user to the descendants of the current user in the user hierarchy </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <param name="inventoryAssignmentReference"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <returns>ApiResponse of InventoryAssignment</returns>
        public ApiResponse<InventoryAssignment> PutInventoryAssignmentResourceByIdWithHttpInfo (string tenantId, string userId, int id, InventoryAssignmentReference inventoryAssignmentReference, string accept = default(string))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
                throw new ApiException(400, "Missing required parameter 'tenantId' when calling InventoryRolesApi->PutInventoryAssignmentResourceById");
            // verify the required parameter 'userId' is set
            if (userId == null)
                throw new ApiException(400, "Missing required parameter 'userId' when calling InventoryRolesApi->PutInventoryAssignmentResourceById");
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling InventoryRolesApi->PutInventoryAssignmentResourceById");
            // verify the required parameter 'inventoryAssignmentReference' is set
            if (inventoryAssignmentReference == null)
                throw new ApiException(400, "Missing required parameter 'inventoryAssignmentReference' when calling InventoryRolesApi->PutInventoryAssignmentResourceById");

            var localVarPath = "/user/{tenantId}/users/{userId}/roles/inventory/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.inventoryassignment+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.inventoryassignment+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (tenantId != null) localVarPathParams.Add("tenantId", this.Configuration.ApiClient.ParameterToString(tenantId)); // path parameter
            if (userId != null) localVarPathParams.Add("userId", this.Configuration.ApiClient.ParameterToString(userId)); // path parameter
            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (inventoryAssignmentReference != null && inventoryAssignmentReference.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(inventoryAssignmentReference); // http body (model) parameter
            }
            else
            {
                localVarPostBody = inventoryAssignmentReference; // byte array
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
                Exception exception = ExceptionFactory("PutInventoryAssignmentResourceById", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<InventoryAssignment>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (InventoryAssignment) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(InventoryAssignment)));
        }

        /// <summary>
        /// Update a specific inventory role assigned to a user Update a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to update the assignment of any inventory roles to root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to update the assignment of inventory roles accessible by the assigned user parent, to non-root users in a user hierarchy<br/> ROLE_USER_MANAGEMENT_CREATE to update the assignment of inventory roles accessible by the current user <b>AND</b> the parent of the assigned user to the descendants of the current user in the user hierarchy </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <param name="inventoryAssignmentReference"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of InventoryAssignment</returns>
        public async System.Threading.Tasks.Task<InventoryAssignment> PutInventoryAssignmentResourceByIdAsync (string tenantId, string userId, int id, InventoryAssignmentReference inventoryAssignmentReference, string accept = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<InventoryAssignment> localVarResponse = await PutInventoryAssignmentResourceByIdWithHttpInfoAsync(tenantId, userId, id, inventoryAssignmentReference, accept, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Update a specific inventory role assigned to a user Update a specific inventory role (by a given ID) assigned to a specific user (by a given user ID) in a specific tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN to update the assignment of any inventory roles to root users in a user hierarchy <b>OR</b> users that are not in any hierarchy<br/> ROLE_USER_MANAGEMENT_ADMIN to update the assignment of inventory roles accessible by the assigned user parent, to non-root users in a user hierarchy<br/> ROLE_USER_MANAGEMENT_CREATE to update the assignment of inventory roles accessible by the current user <b>AND</b> the parent of the assigned user to the descendants of the current user in the user hierarchy </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="userId">Unique identifier of the a user.</param>
        /// <param name="id">Unique identifier of the inventory assignment.</param>
        /// <param name="inventoryAssignmentReference"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (InventoryAssignment)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<InventoryAssignment>> PutInventoryAssignmentResourceByIdWithHttpInfoAsync (string tenantId, string userId, int id, InventoryAssignmentReference inventoryAssignmentReference, string accept = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
                throw new ApiException(400, "Missing required parameter 'tenantId' when calling InventoryRolesApi->PutInventoryAssignmentResourceById");
            // verify the required parameter 'userId' is set
            if (userId == null)
                throw new ApiException(400, "Missing required parameter 'userId' when calling InventoryRolesApi->PutInventoryAssignmentResourceById");
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling InventoryRolesApi->PutInventoryAssignmentResourceById");
            // verify the required parameter 'inventoryAssignmentReference' is set
            if (inventoryAssignmentReference == null)
                throw new ApiException(400, "Missing required parameter 'inventoryAssignmentReference' when calling InventoryRolesApi->PutInventoryAssignmentResourceById");

            var localVarPath = "/user/{tenantId}/users/{userId}/roles/inventory/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.inventoryassignment+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.inventoryassignment+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (tenantId != null) localVarPathParams.Add("tenantId", this.Configuration.ApiClient.ParameterToString(tenantId)); // path parameter
            if (userId != null) localVarPathParams.Add("userId", this.Configuration.ApiClient.ParameterToString(userId)); // path parameter
            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (inventoryAssignmentReference != null && inventoryAssignmentReference.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(inventoryAssignmentReference); // http body (model) parameter
            }
            else
            {
                localVarPostBody = inventoryAssignmentReference; // byte array
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
                Exception exception = ExceptionFactory("PutInventoryAssignmentResourceById", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<InventoryAssignment>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (InventoryAssignment) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(InventoryAssignment)));
        }

        /// <summary>
        /// Update a specific inventory role Update a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <param name="inventoryRole"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <returns>InventoryRole</returns>
        public InventoryRole PutInventoryRoleResourceId (int id, InventoryRole inventoryRole, string accept = default(string))
        {
             ApiResponse<InventoryRole> localVarResponse = PutInventoryRoleResourceIdWithHttpInfo(id, inventoryRole, accept);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Update a specific inventory role Update a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <param name="inventoryRole"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <returns>ApiResponse of InventoryRole</returns>
        public ApiResponse<InventoryRole> PutInventoryRoleResourceIdWithHttpInfo (int id, InventoryRole inventoryRole, string accept = default(string))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling InventoryRolesApi->PutInventoryRoleResourceId");
            // verify the required parameter 'inventoryRole' is set
            if (inventoryRole == null)
                throw new ApiException(400, "Missing required parameter 'inventoryRole' when calling InventoryRolesApi->PutInventoryRoleResourceId");

            var localVarPath = "/user/inventoryroles/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.inventoryrole+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.inventoryrole+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (inventoryRole != null && inventoryRole.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(inventoryRole); // http body (model) parameter
            }
            else
            {
                localVarPostBody = inventoryRole; // byte array
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
                Exception exception = ExceptionFactory("PutInventoryRoleResourceId", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<InventoryRole>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (InventoryRole) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(InventoryRole)));
        }

        /// <summary>
        /// Update a specific inventory role Update a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <param name="inventoryRole"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of InventoryRole</returns>
        public async System.Threading.Tasks.Task<InventoryRole> PutInventoryRoleResourceIdAsync (int id, InventoryRole inventoryRole, string accept = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<InventoryRole> localVarResponse = await PutInventoryRoleResourceIdWithHttpInfoAsync(id, inventoryRole, accept, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Update a specific inventory role Update a specific inventory role (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the inventory role.</param>
        /// <param name="inventoryRole"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (InventoryRole)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<InventoryRole>> PutInventoryRoleResourceIdWithHttpInfoAsync (int id, InventoryRole inventoryRole, string accept = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling InventoryRolesApi->PutInventoryRoleResourceId");
            // verify the required parameter 'inventoryRole' is set
            if (inventoryRole == null)
                throw new ApiException(400, "Missing required parameter 'inventoryRole' when calling InventoryRolesApi->PutInventoryRoleResourceId");

            var localVarPath = "/user/inventoryroles/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.inventoryrole+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.inventoryrole+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (inventoryRole != null && inventoryRole.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(inventoryRole); // http body (model) parameter
            }
            else
            {
                localVarPostBody = inventoryRole; // byte array
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
                Exception exception = ExceptionFactory("PutInventoryRoleResourceId", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<InventoryRole>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (InventoryRole) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(InventoryRole)));
        }

    }
}
