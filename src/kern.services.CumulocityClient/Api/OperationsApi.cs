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
    public interface IOperationsApiSync : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Delete a list of operations
        /// </summary>
        /// <remarks>
        /// Delete a list of operations.  The DELETE method allows for deletion of operation collections.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="agentId">An agent ID that may be part of the operation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the operation. (optional)</param>
        /// <param name="dateTo">End date or date and time of the operation. (optional)</param>
        /// <param name="deviceId">The ID of the device the operation is performed for. (optional)</param>
        /// <param name="status">Status of the operation. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        void DeleteOperationCollectionResource(string? xCumulocityProcessingMode = default(string?), string? agentId = default(string?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? deviceId = default(string?), string? status = default(string?), int operationIndex = 0);

        /// <summary>
        /// Delete a list of operations
        /// </summary>
        /// <remarks>
        /// Delete a list of operations.  The DELETE method allows for deletion of operation collections.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="agentId">An agent ID that may be part of the operation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the operation. (optional)</param>
        /// <param name="dateTo">End date or date and time of the operation. (optional)</param>
        /// <param name="deviceId">The ID of the device the operation is performed for. (optional)</param>
        /// <param name="status">Status of the operation. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DeleteOperationCollectionResourceWithHttpInfo(string? xCumulocityProcessingMode = default(string?), string? agentId = default(string?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? deviceId = default(string?), string? status = default(string?), int operationIndex = 0);
        /// <summary>
        /// Retrieve a list of operations
        /// </summary>
        /// <remarks>
        /// Retrieve a list of operations.  Notes about operation collections:  * The embedded operation object contains &#x60;deviceExternalIDs&#x60; only when queried with an &#x60;agentId&#x60; parameter. * The embedded operation object is filled with &#x60;deviceName&#x60;, but only when requesting resource: Get a collection of operations. * Operations are returned in the order of their ascending IDs.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_READ &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="agentId">An agent ID that may be part of the operation. If this parameter is set, the operation response objects contain the &#x60;deviceExternalIDs&#x60; object. (optional)</param>
        /// <param name="bulkOperationId">The bulk operation ID that this operation belongs to. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the operation. (optional)</param>
        /// <param name="dateTo">End date or date and time of the operation. (optional)</param>
        /// <param name="deviceId">The ID of the device the operation is performed for. (optional)</param>
        /// <param name="fragmentType">The type of fragment that must be part of the operation. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest operations first. By default, the results are sorted by the oldest operations first.  (optional, default to false)</param>
        /// <param name="status">Status of the operation. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>OperationCollection</returns>
        OperationCollection GetOperationCollectionResource(string? agentId = default(string?), string? bulkOperationId = default(string?), int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? deviceId = default(string?), string? fragmentType = default(string?), int? pageSize = default(int?), bool? revert = default(bool?), string? status = default(string?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0);

        /// <summary>
        /// Retrieve a list of operations
        /// </summary>
        /// <remarks>
        /// Retrieve a list of operations.  Notes about operation collections:  * The embedded operation object contains &#x60;deviceExternalIDs&#x60; only when queried with an &#x60;agentId&#x60; parameter. * The embedded operation object is filled with &#x60;deviceName&#x60;, but only when requesting resource: Get a collection of operations. * Operations are returned in the order of their ascending IDs.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_READ &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="agentId">An agent ID that may be part of the operation. If this parameter is set, the operation response objects contain the &#x60;deviceExternalIDs&#x60; object. (optional)</param>
        /// <param name="bulkOperationId">The bulk operation ID that this operation belongs to. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the operation. (optional)</param>
        /// <param name="dateTo">End date or date and time of the operation. (optional)</param>
        /// <param name="deviceId">The ID of the device the operation is performed for. (optional)</param>
        /// <param name="fragmentType">The type of fragment that must be part of the operation. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest operations first. By default, the results are sorted by the oldest operations first.  (optional, default to false)</param>
        /// <param name="status">Status of the operation. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of OperationCollection</returns>
        ApiResponse<OperationCollection> GetOperationCollectionResourceWithHttpInfo(string? agentId = default(string?), string? bulkOperationId = default(string?), int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? deviceId = default(string?), string? fragmentType = default(string?), int? pageSize = default(int?), bool? revert = default(bool?), string? status = default(string?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0);
        /// <summary>
        /// Retrieve a specific operation
        /// </summary>
        /// <remarks>
        /// Retrieve a specific operation (by a given ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_READ &lt;b&gt;OR&lt;/b&gt; owner of the resource &lt;b&gt;OR&lt;/b&gt; ADMIN permission on the device &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the operation.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>Operation</returns>
        Operation GetOperationResource(string id, int operationIndex = 0);

        /// <summary>
        /// Retrieve a specific operation
        /// </summary>
        /// <remarks>
        /// Retrieve a specific operation (by a given ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_READ &lt;b&gt;OR&lt;/b&gt; owner of the resource &lt;b&gt;OR&lt;/b&gt; ADMIN permission on the device &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the operation.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Operation</returns>
        ApiResponse<Operation> GetOperationResourceWithHttpInfo(string id, int operationIndex = 0);
        /// <summary>
        /// Create an operation
        /// </summary>
        /// <remarks>
        /// Create an operation.  It is possible to add custom fragments to operations, for example &#x60;com_cumulocity_model_WebCamDevice&#x60; is a custom object of the webcam operation.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the device &lt;b&gt;OR&lt;/b&gt; ADMIN permissions on the device &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postOperationCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>Operation</returns>
        Operation PostOperationCollectionResource(PostOperationCollectionResourceRequest postOperationCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0);

        /// <summary>
        /// Create an operation
        /// </summary>
        /// <remarks>
        /// Create an operation.  It is possible to add custom fragments to operations, for example &#x60;com_cumulocity_model_WebCamDevice&#x60; is a custom object of the webcam operation.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the device &lt;b&gt;OR&lt;/b&gt; ADMIN permissions on the device &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postOperationCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Operation</returns>
        ApiResponse<Operation> PostOperationCollectionResourceWithHttpInfo(PostOperationCollectionResourceRequest postOperationCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0);
        /// <summary>
        /// Update a specific operation status
        /// </summary>
        /// <remarks>
        /// Update a specific operation (by a given ID). You can only update its status.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the resource &lt;b&gt;OR&lt;/b&gt; ADMIN permission on the device &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the operation.</param>
        /// <param name="putOperationResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>Operation</returns>
        Operation PutOperationResource(string id, PutOperationResourceRequest putOperationResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0);

        /// <summary>
        /// Update a specific operation status
        /// </summary>
        /// <remarks>
        /// Update a specific operation (by a given ID). You can only update its status.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the resource &lt;b&gt;OR&lt;/b&gt; ADMIN permission on the device &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the operation.</param>
        /// <param name="putOperationResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Operation</returns>
        ApiResponse<Operation> PutOperationResourceWithHttpInfo(string id, PutOperationResourceRequest putOperationResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0);
        #endregion Synchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IOperationsApiAsync : IApiAccessor
    {
        #region Asynchronous Operations
        /// <summary>
        /// Delete a list of operations
        /// </summary>
        /// <remarks>
        /// Delete a list of operations.  The DELETE method allows for deletion of operation collections.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="agentId">An agent ID that may be part of the operation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the operation. (optional)</param>
        /// <param name="dateTo">End date or date and time of the operation. (optional)</param>
        /// <param name="deviceId">The ID of the device the operation is performed for. (optional)</param>
        /// <param name="status">Status of the operation. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DeleteOperationCollectionResourceAsync(string? xCumulocityProcessingMode = default(string?), string? agentId = default(string?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? deviceId = default(string?), string? status = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Delete a list of operations
        /// </summary>
        /// <remarks>
        /// Delete a list of operations.  The DELETE method allows for deletion of operation collections.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="agentId">An agent ID that may be part of the operation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the operation. (optional)</param>
        /// <param name="dateTo">End date or date and time of the operation. (optional)</param>
        /// <param name="deviceId">The ID of the device the operation is performed for. (optional)</param>
        /// <param name="status">Status of the operation. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DeleteOperationCollectionResourceWithHttpInfoAsync(string? xCumulocityProcessingMode = default(string?), string? agentId = default(string?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? deviceId = default(string?), string? status = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Retrieve a list of operations
        /// </summary>
        /// <remarks>
        /// Retrieve a list of operations.  Notes about operation collections:  * The embedded operation object contains &#x60;deviceExternalIDs&#x60; only when queried with an &#x60;agentId&#x60; parameter. * The embedded operation object is filled with &#x60;deviceName&#x60;, but only when requesting resource: Get a collection of operations. * Operations are returned in the order of their ascending IDs.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_READ &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="agentId">An agent ID that may be part of the operation. If this parameter is set, the operation response objects contain the &#x60;deviceExternalIDs&#x60; object. (optional)</param>
        /// <param name="bulkOperationId">The bulk operation ID that this operation belongs to. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the operation. (optional)</param>
        /// <param name="dateTo">End date or date and time of the operation. (optional)</param>
        /// <param name="deviceId">The ID of the device the operation is performed for. (optional)</param>
        /// <param name="fragmentType">The type of fragment that must be part of the operation. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest operations first. By default, the results are sorted by the oldest operations first.  (optional, default to false)</param>
        /// <param name="status">Status of the operation. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of OperationCollection</returns>
        System.Threading.Tasks.Task<OperationCollection> GetOperationCollectionResourceAsync(string? agentId = default(string?), string? bulkOperationId = default(string?), int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? deviceId = default(string?), string? fragmentType = default(string?), int? pageSize = default(int?), bool? revert = default(bool?), string? status = default(string?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Retrieve a list of operations
        /// </summary>
        /// <remarks>
        /// Retrieve a list of operations.  Notes about operation collections:  * The embedded operation object contains &#x60;deviceExternalIDs&#x60; only when queried with an &#x60;agentId&#x60; parameter. * The embedded operation object is filled with &#x60;deviceName&#x60;, but only when requesting resource: Get a collection of operations. * Operations are returned in the order of their ascending IDs.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_READ &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="agentId">An agent ID that may be part of the operation. If this parameter is set, the operation response objects contain the &#x60;deviceExternalIDs&#x60; object. (optional)</param>
        /// <param name="bulkOperationId">The bulk operation ID that this operation belongs to. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the operation. (optional)</param>
        /// <param name="dateTo">End date or date and time of the operation. (optional)</param>
        /// <param name="deviceId">The ID of the device the operation is performed for. (optional)</param>
        /// <param name="fragmentType">The type of fragment that must be part of the operation. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest operations first. By default, the results are sorted by the oldest operations first.  (optional, default to false)</param>
        /// <param name="status">Status of the operation. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (OperationCollection)</returns>
        System.Threading.Tasks.Task<ApiResponse<OperationCollection>> GetOperationCollectionResourceWithHttpInfoAsync(string? agentId = default(string?), string? bulkOperationId = default(string?), int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? deviceId = default(string?), string? fragmentType = default(string?), int? pageSize = default(int?), bool? revert = default(bool?), string? status = default(string?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Retrieve a specific operation
        /// </summary>
        /// <remarks>
        /// Retrieve a specific operation (by a given ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_READ &lt;b&gt;OR&lt;/b&gt; owner of the resource &lt;b&gt;OR&lt;/b&gt; ADMIN permission on the device &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the operation.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Operation</returns>
        System.Threading.Tasks.Task<Operation> GetOperationResourceAsync(string id, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Retrieve a specific operation
        /// </summary>
        /// <remarks>
        /// Retrieve a specific operation (by a given ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_READ &lt;b&gt;OR&lt;/b&gt; owner of the resource &lt;b&gt;OR&lt;/b&gt; ADMIN permission on the device &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the operation.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Operation)</returns>
        System.Threading.Tasks.Task<ApiResponse<Operation>> GetOperationResourceWithHttpInfoAsync(string id, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Create an operation
        /// </summary>
        /// <remarks>
        /// Create an operation.  It is possible to add custom fragments to operations, for example &#x60;com_cumulocity_model_WebCamDevice&#x60; is a custom object of the webcam operation.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the device &lt;b&gt;OR&lt;/b&gt; ADMIN permissions on the device &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postOperationCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Operation</returns>
        System.Threading.Tasks.Task<Operation> PostOperationCollectionResourceAsync(PostOperationCollectionResourceRequest postOperationCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Create an operation
        /// </summary>
        /// <remarks>
        /// Create an operation.  It is possible to add custom fragments to operations, for example &#x60;com_cumulocity_model_WebCamDevice&#x60; is a custom object of the webcam operation.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the device &lt;b&gt;OR&lt;/b&gt; ADMIN permissions on the device &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postOperationCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Operation)</returns>
        System.Threading.Tasks.Task<ApiResponse<Operation>> PostOperationCollectionResourceWithHttpInfoAsync(PostOperationCollectionResourceRequest postOperationCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Update a specific operation status
        /// </summary>
        /// <remarks>
        /// Update a specific operation (by a given ID). You can only update its status.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the resource &lt;b&gt;OR&lt;/b&gt; ADMIN permission on the device &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the operation.</param>
        /// <param name="putOperationResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Operation</returns>
        System.Threading.Tasks.Task<Operation> PutOperationResourceAsync(string id, PutOperationResourceRequest putOperationResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Update a specific operation status
        /// </summary>
        /// <remarks>
        /// Update a specific operation (by a given ID). You can only update its status.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the resource &lt;b&gt;OR&lt;/b&gt; ADMIN permission on the device &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the operation.</param>
        /// <param name="putOperationResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Operation)</returns>
        System.Threading.Tasks.Task<ApiResponse<Operation>> PutOperationResourceWithHttpInfoAsync(string id, PutOperationResourceRequest putOperationResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IOperationsApi : IOperationsApiSync, IOperationsApiAsync
    {

    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class OperationsApi : IOperationsApi
    {
        private kern.services.CumulocityClient.Client.ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public OperationsApi() : this((string)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public OperationsApi(string basePath)
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
        /// Initializes a new instance of the <see cref="OperationsApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public OperationsApi(kern.services.CumulocityClient.Client.Configuration configuration)
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
        /// Initializes a new instance of the <see cref="OperationsApi"/> class
        /// using a Configuration object and client instance.
        /// </summary>
        /// <param name="client">The client interface for synchronous API access.</param>
        /// <param name="asyncClient">The client interface for asynchronous API access.</param>
        /// <param name="configuration">The configuration object.</param>
        public OperationsApi(kern.services.CumulocityClient.Client.ISynchronousClient client, kern.services.CumulocityClient.Client.IAsynchronousClient asyncClient, kern.services.CumulocityClient.Client.IReadableConfiguration configuration)
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
        /// Delete a list of operations Delete a list of operations.  The DELETE method allows for deletion of operation collections.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="agentId">An agent ID that may be part of the operation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the operation. (optional)</param>
        /// <param name="dateTo">End date or date and time of the operation. (optional)</param>
        /// <param name="deviceId">The ID of the device the operation is performed for. (optional)</param>
        /// <param name="status">Status of the operation. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        public void DeleteOperationCollectionResource(string? xCumulocityProcessingMode = default(string?), string? agentId = default(string?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? deviceId = default(string?), string? status = default(string?), int operationIndex = 0)
        {
            DeleteOperationCollectionResourceWithHttpInfo(xCumulocityProcessingMode, agentId, dateFrom, dateTo, deviceId, status);
        }

        /// <summary>
        /// Delete a list of operations Delete a list of operations.  The DELETE method allows for deletion of operation collections.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="agentId">An agent ID that may be part of the operation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the operation. (optional)</param>
        /// <param name="dateTo">End date or date and time of the operation. (optional)</param>
        /// <param name="deviceId">The ID of the device the operation is performed for. (optional)</param>
        /// <param name="status">Status of the operation. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<Object> DeleteOperationCollectionResourceWithHttpInfo(string? xCumulocityProcessingMode = default(string?), string? agentId = default(string?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? deviceId = default(string?), string? status = default(string?), int operationIndex = 0)
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

            if (agentId != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "agentId", agentId));
            }
            if (dateFrom != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateFrom", dateFrom));
            }
            if (dateTo != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateTo", dateTo));
            }
            if (deviceId != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "deviceId", deviceId));
            }
            if (status != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "status", status));
            }
            if (xCumulocityProcessingMode != null)
            {
                localVarRequestOptions.HeaderParameters.Add("X-Cumulocity-Processing-Mode", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(xCumulocityProcessingMode)); // header parameter
            }

            localVarRequestOptions.Operation = "OperationsApi.DeleteOperationCollectionResource";
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
            var localVarResponse = this.Client.Delete<Object>("/devicecontrol/operations", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteOperationCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Delete a list of operations Delete a list of operations.  The DELETE method allows for deletion of operation collections.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="agentId">An agent ID that may be part of the operation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the operation. (optional)</param>
        /// <param name="dateTo">End date or date and time of the operation. (optional)</param>
        /// <param name="deviceId">The ID of the device the operation is performed for. (optional)</param>
        /// <param name="status">Status of the operation. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DeleteOperationCollectionResourceAsync(string? xCumulocityProcessingMode = default(string?), string? agentId = default(string?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? deviceId = default(string?), string? status = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            await DeleteOperationCollectionResourceWithHttpInfoAsync(xCumulocityProcessingMode, agentId, dateFrom, dateTo, deviceId, status, operationIndex, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete a list of operations Delete a list of operations.  The DELETE method allows for deletion of operation collections.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="agentId">An agent ID that may be part of the operation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the operation. (optional)</param>
        /// <param name="dateTo">End date or date and time of the operation. (optional)</param>
        /// <param name="deviceId">The ID of the device the operation is performed for. (optional)</param>
        /// <param name="status">Status of the operation. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<Object>> DeleteOperationCollectionResourceWithHttpInfoAsync(string? xCumulocityProcessingMode = default(string?), string? agentId = default(string?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? deviceId = default(string?), string? status = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
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

            if (agentId != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "agentId", agentId));
            }
            if (dateFrom != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateFrom", dateFrom));
            }
            if (dateTo != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateTo", dateTo));
            }
            if (deviceId != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "deviceId", deviceId));
            }
            if (status != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "status", status));
            }
            if (xCumulocityProcessingMode != null)
            {
                localVarRequestOptions.HeaderParameters.Add("X-Cumulocity-Processing-Mode", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(xCumulocityProcessingMode)); // header parameter
            }

            localVarRequestOptions.Operation = "OperationsApi.DeleteOperationCollectionResource";
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
            var localVarResponse = await this.AsynchronousClient.DeleteAsync<Object>("/devicecontrol/operations", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteOperationCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve a list of operations Retrieve a list of operations.  Notes about operation collections:  * The embedded operation object contains &#x60;deviceExternalIDs&#x60; only when queried with an &#x60;agentId&#x60; parameter. * The embedded operation object is filled with &#x60;deviceName&#x60;, but only when requesting resource: Get a collection of operations. * Operations are returned in the order of their ascending IDs.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_READ &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="agentId">An agent ID that may be part of the operation. If this parameter is set, the operation response objects contain the &#x60;deviceExternalIDs&#x60; object. (optional)</param>
        /// <param name="bulkOperationId">The bulk operation ID that this operation belongs to. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the operation. (optional)</param>
        /// <param name="dateTo">End date or date and time of the operation. (optional)</param>
        /// <param name="deviceId">The ID of the device the operation is performed for. (optional)</param>
        /// <param name="fragmentType">The type of fragment that must be part of the operation. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest operations first. By default, the results are sorted by the oldest operations first.  (optional, default to false)</param>
        /// <param name="status">Status of the operation. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>OperationCollection</returns>
        public OperationCollection GetOperationCollectionResource(string? agentId = default(string?), string? bulkOperationId = default(string?), int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? deviceId = default(string?), string? fragmentType = default(string?), int? pageSize = default(int?), bool? revert = default(bool?), string? status = default(string?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<OperationCollection> localVarResponse = GetOperationCollectionResourceWithHttpInfo(agentId, bulkOperationId, currentPage, dateFrom, dateTo, deviceId, fragmentType, pageSize, revert, status, withTotalElements, withTotalPages);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve a list of operations Retrieve a list of operations.  Notes about operation collections:  * The embedded operation object contains &#x60;deviceExternalIDs&#x60; only when queried with an &#x60;agentId&#x60; parameter. * The embedded operation object is filled with &#x60;deviceName&#x60;, but only when requesting resource: Get a collection of operations. * Operations are returned in the order of their ascending IDs.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_READ &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="agentId">An agent ID that may be part of the operation. If this parameter is set, the operation response objects contain the &#x60;deviceExternalIDs&#x60; object. (optional)</param>
        /// <param name="bulkOperationId">The bulk operation ID that this operation belongs to. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the operation. (optional)</param>
        /// <param name="dateTo">End date or date and time of the operation. (optional)</param>
        /// <param name="deviceId">The ID of the device the operation is performed for. (optional)</param>
        /// <param name="fragmentType">The type of fragment that must be part of the operation. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest operations first. By default, the results are sorted by the oldest operations first.  (optional, default to false)</param>
        /// <param name="status">Status of the operation. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of OperationCollection</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<OperationCollection> GetOperationCollectionResourceWithHttpInfo(string? agentId = default(string?), string? bulkOperationId = default(string?), int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? deviceId = default(string?), string? fragmentType = default(string?), int? pageSize = default(int?), bool? revert = default(bool?), string? status = default(string?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.operationcollection+json",
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

            if (agentId != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "agentId", agentId));
            }
            if (bulkOperationId != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "bulkOperationId", bulkOperationId));
            }
            if (currentPage != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "currentPage", currentPage));
            }
            if (dateFrom != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateFrom", dateFrom));
            }
            if (dateTo != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateTo", dateTo));
            }
            if (deviceId != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "deviceId", deviceId));
            }
            if (fragmentType != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "fragmentType", fragmentType));
            }
            if (pageSize != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "pageSize", pageSize));
            }
            if (revert != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "revert", revert));
            }
            if (status != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "status", status));
            }
            if (withTotalElements != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withTotalElements", withTotalElements));
            }
            if (withTotalPages != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withTotalPages", withTotalPages));
            }

            localVarRequestOptions.Operation = "OperationsApi.GetOperationCollectionResource";
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
            var localVarResponse = this.Client.Get<OperationCollection>("/devicecontrol/operations", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetOperationCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve a list of operations Retrieve a list of operations.  Notes about operation collections:  * The embedded operation object contains &#x60;deviceExternalIDs&#x60; only when queried with an &#x60;agentId&#x60; parameter. * The embedded operation object is filled with &#x60;deviceName&#x60;, but only when requesting resource: Get a collection of operations. * Operations are returned in the order of their ascending IDs.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_READ &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="agentId">An agent ID that may be part of the operation. If this parameter is set, the operation response objects contain the &#x60;deviceExternalIDs&#x60; object. (optional)</param>
        /// <param name="bulkOperationId">The bulk operation ID that this operation belongs to. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the operation. (optional)</param>
        /// <param name="dateTo">End date or date and time of the operation. (optional)</param>
        /// <param name="deviceId">The ID of the device the operation is performed for. (optional)</param>
        /// <param name="fragmentType">The type of fragment that must be part of the operation. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest operations first. By default, the results are sorted by the oldest operations first.  (optional, default to false)</param>
        /// <param name="status">Status of the operation. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of OperationCollection</returns>
        public async System.Threading.Tasks.Task<OperationCollection> GetOperationCollectionResourceAsync(string? agentId = default(string?), string? bulkOperationId = default(string?), int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? deviceId = default(string?), string? fragmentType = default(string?), int? pageSize = default(int?), bool? revert = default(bool?), string? status = default(string?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<OperationCollection> localVarResponse = await GetOperationCollectionResourceWithHttpInfoAsync(agentId, bulkOperationId, currentPage, dateFrom, dateTo, deviceId, fragmentType, pageSize, revert, status, withTotalElements, withTotalPages, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve a list of operations Retrieve a list of operations.  Notes about operation collections:  * The embedded operation object contains &#x60;deviceExternalIDs&#x60; only when queried with an &#x60;agentId&#x60; parameter. * The embedded operation object is filled with &#x60;deviceName&#x60;, but only when requesting resource: Get a collection of operations. * Operations are returned in the order of their ascending IDs.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_READ &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="agentId">An agent ID that may be part of the operation. If this parameter is set, the operation response objects contain the &#x60;deviceExternalIDs&#x60; object. (optional)</param>
        /// <param name="bulkOperationId">The bulk operation ID that this operation belongs to. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the operation. (optional)</param>
        /// <param name="dateTo">End date or date and time of the operation. (optional)</param>
        /// <param name="deviceId">The ID of the device the operation is performed for. (optional)</param>
        /// <param name="fragmentType">The type of fragment that must be part of the operation. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest operations first. By default, the results are sorted by the oldest operations first.  (optional, default to false)</param>
        /// <param name="status">Status of the operation. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (OperationCollection)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<OperationCollection>> GetOperationCollectionResourceWithHttpInfoAsync(string? agentId = default(string?), string? bulkOperationId = default(string?), int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? deviceId = default(string?), string? fragmentType = default(string?), int? pageSize = default(int?), bool? revert = default(bool?), string? status = default(string?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.operationcollection+json",
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

            if (agentId != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "agentId", agentId));
            }
            if (bulkOperationId != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "bulkOperationId", bulkOperationId));
            }
            if (currentPage != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "currentPage", currentPage));
            }
            if (dateFrom != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateFrom", dateFrom));
            }
            if (dateTo != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateTo", dateTo));
            }
            if (deviceId != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "deviceId", deviceId));
            }
            if (fragmentType != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "fragmentType", fragmentType));
            }
            if (pageSize != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "pageSize", pageSize));
            }
            if (revert != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "revert", revert));
            }
            if (status != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "status", status));
            }
            if (withTotalElements != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withTotalElements", withTotalElements));
            }
            if (withTotalPages != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withTotalPages", withTotalPages));
            }

            localVarRequestOptions.Operation = "OperationsApi.GetOperationCollectionResource";
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
            var localVarResponse = await this.AsynchronousClient.GetAsync<OperationCollection>("/devicecontrol/operations", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetOperationCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve a specific operation Retrieve a specific operation (by a given ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_READ &lt;b&gt;OR&lt;/b&gt; owner of the resource &lt;b&gt;OR&lt;/b&gt; ADMIN permission on the device &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the operation.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>Operation</returns>
        public Operation GetOperationResource(string id, int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<Operation> localVarResponse = GetOperationResourceWithHttpInfo(id);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve a specific operation Retrieve a specific operation (by a given ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_READ &lt;b&gt;OR&lt;/b&gt; owner of the resource &lt;b&gt;OR&lt;/b&gt; ADMIN permission on the device &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the operation.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Operation</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<Operation> GetOperationResourceWithHttpInfo(string id, int operationIndex = 0)
        {
            // verify the required parameter 'id' is set
            if (id == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'id' when calling OperationsApi->GetOperationResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.operation+json",
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

            localVarRequestOptions.PathParameters.Add("id", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(id)); // path parameter

            localVarRequestOptions.Operation = "OperationsApi.GetOperationResource";
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
            var localVarResponse = this.Client.Get<Operation>("/devicecontrol/operations/{id}", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetOperationResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve a specific operation Retrieve a specific operation (by a given ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_READ &lt;b&gt;OR&lt;/b&gt; owner of the resource &lt;b&gt;OR&lt;/b&gt; ADMIN permission on the device &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the operation.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Operation</returns>
        public async System.Threading.Tasks.Task<Operation> GetOperationResourceAsync(string id, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<Operation> localVarResponse = await GetOperationResourceWithHttpInfoAsync(id, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve a specific operation Retrieve a specific operation (by a given ID).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_READ &lt;b&gt;OR&lt;/b&gt; owner of the resource &lt;b&gt;OR&lt;/b&gt; ADMIN permission on the device &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the operation.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Operation)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<Operation>> GetOperationResourceWithHttpInfoAsync(string id, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'id' when calling OperationsApi->GetOperationResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.operation+json",
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

            localVarRequestOptions.PathParameters.Add("id", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(id)); // path parameter

            localVarRequestOptions.Operation = "OperationsApi.GetOperationResource";
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
            var localVarResponse = await this.AsynchronousClient.GetAsync<Operation>("/devicecontrol/operations/{id}", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetOperationResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Create an operation Create an operation.  It is possible to add custom fragments to operations, for example &#x60;com_cumulocity_model_WebCamDevice&#x60; is a custom object of the webcam operation.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the device &lt;b&gt;OR&lt;/b&gt; ADMIN permissions on the device &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postOperationCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>Operation</returns>
        public Operation PostOperationCollectionResource(PostOperationCollectionResourceRequest postOperationCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<Operation> localVarResponse = PostOperationCollectionResourceWithHttpInfo(postOperationCollectionResourceRequest, accept, xCumulocityProcessingMode);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Create an operation Create an operation.  It is possible to add custom fragments to operations, for example &#x60;com_cumulocity_model_WebCamDevice&#x60; is a custom object of the webcam operation.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the device &lt;b&gt;OR&lt;/b&gt; ADMIN permissions on the device &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postOperationCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Operation</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<Operation> PostOperationCollectionResourceWithHttpInfo(PostOperationCollectionResourceRequest postOperationCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0)
        {
            // verify the required parameter 'postOperationCollectionResourceRequest' is set
            if (postOperationCollectionResourceRequest == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'postOperationCollectionResourceRequest' when calling OperationsApi->PostOperationCollectionResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/vnd.com.nsn.cumulocity.operation+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.operation+json",
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

            if (accept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(accept)); // header parameter
            }
            if (xCumulocityProcessingMode != null)
            {
                localVarRequestOptions.HeaderParameters.Add("X-Cumulocity-Processing-Mode", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(xCumulocityProcessingMode)); // header parameter
            }
            localVarRequestOptions.Data = postOperationCollectionResourceRequest;

            localVarRequestOptions.Operation = "OperationsApi.PostOperationCollectionResource";
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
            var localVarResponse = this.Client.Post<Operation>("/devicecontrol/operations", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PostOperationCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Create an operation Create an operation.  It is possible to add custom fragments to operations, for example &#x60;com_cumulocity_model_WebCamDevice&#x60; is a custom object of the webcam operation.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the device &lt;b&gt;OR&lt;/b&gt; ADMIN permissions on the device &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postOperationCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Operation</returns>
        public async System.Threading.Tasks.Task<Operation> PostOperationCollectionResourceAsync(PostOperationCollectionResourceRequest postOperationCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<Operation> localVarResponse = await PostOperationCollectionResourceWithHttpInfoAsync(postOperationCollectionResourceRequest, accept, xCumulocityProcessingMode, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Create an operation Create an operation.  It is possible to add custom fragments to operations, for example &#x60;com_cumulocity_model_WebCamDevice&#x60; is a custom object of the webcam operation.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the device &lt;b&gt;OR&lt;/b&gt; ADMIN permissions on the device &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postOperationCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Operation)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<Operation>> PostOperationCollectionResourceWithHttpInfoAsync(PostOperationCollectionResourceRequest postOperationCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'postOperationCollectionResourceRequest' is set
            if (postOperationCollectionResourceRequest == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'postOperationCollectionResourceRequest' when calling OperationsApi->PostOperationCollectionResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/vnd.com.nsn.cumulocity.operation+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.operation+json",
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

            if (accept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(accept)); // header parameter
            }
            if (xCumulocityProcessingMode != null)
            {
                localVarRequestOptions.HeaderParameters.Add("X-Cumulocity-Processing-Mode", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(xCumulocityProcessingMode)); // header parameter
            }
            localVarRequestOptions.Data = postOperationCollectionResourceRequest;

            localVarRequestOptions.Operation = "OperationsApi.PostOperationCollectionResource";
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
            var localVarResponse = await this.AsynchronousClient.PostAsync<Operation>("/devicecontrol/operations", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PostOperationCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Update a specific operation status Update a specific operation (by a given ID). You can only update its status.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the resource &lt;b&gt;OR&lt;/b&gt; ADMIN permission on the device &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the operation.</param>
        /// <param name="putOperationResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>Operation</returns>
        public Operation PutOperationResource(string id, PutOperationResourceRequest putOperationResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<Operation> localVarResponse = PutOperationResourceWithHttpInfo(id, putOperationResourceRequest, accept, xCumulocityProcessingMode);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Update a specific operation status Update a specific operation (by a given ID). You can only update its status.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the resource &lt;b&gt;OR&lt;/b&gt; ADMIN permission on the device &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the operation.</param>
        /// <param name="putOperationResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Operation</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<Operation> PutOperationResourceWithHttpInfo(string id, PutOperationResourceRequest putOperationResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0)
        {
            // verify the required parameter 'id' is set
            if (id == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'id' when calling OperationsApi->PutOperationResource");
            }

            // verify the required parameter 'putOperationResourceRequest' is set
            if (putOperationResourceRequest == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'putOperationResourceRequest' when calling OperationsApi->PutOperationResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/vnd.com.nsn.cumulocity.operation+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.operation+json",
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

            localVarRequestOptions.PathParameters.Add("id", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(id)); // path parameter
            if (accept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(accept)); // header parameter
            }
            if (xCumulocityProcessingMode != null)
            {
                localVarRequestOptions.HeaderParameters.Add("X-Cumulocity-Processing-Mode", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(xCumulocityProcessingMode)); // header parameter
            }
            localVarRequestOptions.Data = putOperationResourceRequest;

            localVarRequestOptions.Operation = "OperationsApi.PutOperationResource";
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
            var localVarResponse = this.Client.Put<Operation>("/devicecontrol/operations/{id}", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PutOperationResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Update a specific operation status Update a specific operation (by a given ID). You can only update its status.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the resource &lt;b&gt;OR&lt;/b&gt; ADMIN permission on the device &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the operation.</param>
        /// <param name="putOperationResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Operation</returns>
        public async System.Threading.Tasks.Task<Operation> PutOperationResourceAsync(string id, PutOperationResourceRequest putOperationResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<Operation> localVarResponse = await PutOperationResourceWithHttpInfoAsync(id, putOperationResourceRequest, accept, xCumulocityProcessingMode, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Update a specific operation status Update a specific operation (by a given ID). You can only update its status.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_DEVICE_CONTROL_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the resource &lt;b&gt;OR&lt;/b&gt; ADMIN permission on the device &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the operation.</param>
        /// <param name="putOperationResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Operation)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<Operation>> PutOperationResourceWithHttpInfoAsync(string id, PutOperationResourceRequest putOperationResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'id' when calling OperationsApi->PutOperationResource");
            }

            // verify the required parameter 'putOperationResourceRequest' is set
            if (putOperationResourceRequest == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'putOperationResourceRequest' when calling OperationsApi->PutOperationResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/vnd.com.nsn.cumulocity.operation+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.operation+json",
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

            localVarRequestOptions.PathParameters.Add("id", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(id)); // path parameter
            if (accept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(accept)); // header parameter
            }
            if (xCumulocityProcessingMode != null)
            {
                localVarRequestOptions.HeaderParameters.Add("X-Cumulocity-Processing-Mode", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(xCumulocityProcessingMode)); // header parameter
            }
            localVarRequestOptions.Data = putOperationResourceRequest;

            localVarRequestOptions.Operation = "OperationsApi.PutOperationResource";
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
            var localVarResponse = await this.AsynchronousClient.PutAsync<Operation>("/devicecontrol/operations/{id}", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PutOperationResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

    }
}
