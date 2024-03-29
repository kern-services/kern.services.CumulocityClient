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
    public interface IMeasurementsApiSync : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Remove measurement collections
        /// </summary>
        /// <remarks>
        /// Remove measurement collections specified by query parameters.  DELETE requests are not synchronous. The response could be returned before the delete request has been completed. This may happen especially when there are a lot of measurements to be deleted.  &gt; **⚠️ Important:** Note that it is possible to call this endpoint without providing any parameter - it may result in deleting all measurements and it is not recommended.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="dateFrom">Start date or date and time of the measurement. (optional)</param>
        /// <param name="dateTo">End date or date and time of the measurement. (optional)</param>
        /// <param name="fragmentType">A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional)</param>
        /// <param name="source">The managed object ID to which the measurement is associated. (optional)</param>
        /// <param name="type">The type of measurement to search for. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        void DeleteMeasurementCollectionResource(string? xCumulocityProcessingMode = default(string?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? fragmentType = default(string?), string? source = default(string?), string? type = default(string?), int operationIndex = 0);

        /// <summary>
        /// Remove measurement collections
        /// </summary>
        /// <remarks>
        /// Remove measurement collections specified by query parameters.  DELETE requests are not synchronous. The response could be returned before the delete request has been completed. This may happen especially when there are a lot of measurements to be deleted.  &gt; **⚠️ Important:** Note that it is possible to call this endpoint without providing any parameter - it may result in deleting all measurements and it is not recommended.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="dateFrom">Start date or date and time of the measurement. (optional)</param>
        /// <param name="dateTo">End date or date and time of the measurement. (optional)</param>
        /// <param name="fragmentType">A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional)</param>
        /// <param name="source">The managed object ID to which the measurement is associated. (optional)</param>
        /// <param name="type">The type of measurement to search for. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DeleteMeasurementCollectionResourceWithHttpInfo(string? xCumulocityProcessingMode = default(string?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? fragmentType = default(string?), string? source = default(string?), string? type = default(string?), int operationIndex = 0);
        /// <summary>
        /// Remove a specific measurement
        /// </summary>
        /// <remarks>
        /// Remove a specific measurement by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_ADMIN permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the measurement.</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        void DeleteMeasurementResource(string id, string? xCumulocityProcessingMode = default(string?), int operationIndex = 0);

        /// <summary>
        /// Remove a specific measurement
        /// </summary>
        /// <remarks>
        /// Remove a specific measurement by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_ADMIN permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the measurement.</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DeleteMeasurementResourceWithHttpInfo(string id, string? xCumulocityProcessingMode = default(string?), int operationIndex = 0);
        /// <summary>
        /// Retrieve all measurements
        /// </summary>
        /// <remarks>
        /// Retrieve all measurements on your tenant, or a specific subset based on queries.  In case of executing [range queries](https://en.wikipedia.org/wiki/Range_query_(database)) between an upper and lower boundary, for example, querying using &#x60;dateFrom&#x60;–&#x60;dateTo&#x60;, the oldest registered measurements are returned first. It is possible to change the order using the query parameter &#x60;revert&#x3D;true&#x60;.  For large measurement collections, querying older records without filters can be slow as the server needs to scan from the beginning of the input results set before beginning to return the results. For cases when older measurements should be retrieved, it is recommended to narrow the scope by using range queries based on the time stamp reported by a device. The scope of query can also be reduced significantly when a source device is provided.  Review [Measurements Specifics](#tag/Measurements-specifics) for details about data streaming and response formats.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the measurement. (optional)</param>
        /// <param name="dateTo">End date or date and time of the measurement. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  (optional, default to false)</param>
        /// <param name="source">The managed object ID to which the measurement is associated. (optional)</param>
        /// <param name="type">The type of measurement to search for. (optional)</param>
        /// <param name="valueFragmentSeries">The specific series to search for. (optional)</param>
        /// <param name="valueFragmentType">A characteristic which identifies the measurement. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>MeasurementCollection</returns>
        MeasurementCollection GetMeasurementCollectionResource(int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), int? pageSize = default(int?), bool? revert = default(bool?), string? source = default(string?), string? type = default(string?), string? valueFragmentSeries = default(string?), string? valueFragmentType = default(string?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0);

        /// <summary>
        /// Retrieve all measurements
        /// </summary>
        /// <remarks>
        /// Retrieve all measurements on your tenant, or a specific subset based on queries.  In case of executing [range queries](https://en.wikipedia.org/wiki/Range_query_(database)) between an upper and lower boundary, for example, querying using &#x60;dateFrom&#x60;–&#x60;dateTo&#x60;, the oldest registered measurements are returned first. It is possible to change the order using the query parameter &#x60;revert&#x3D;true&#x60;.  For large measurement collections, querying older records without filters can be slow as the server needs to scan from the beginning of the input results set before beginning to return the results. For cases when older measurements should be retrieved, it is recommended to narrow the scope by using range queries based on the time stamp reported by a device. The scope of query can also be reduced significantly when a source device is provided.  Review [Measurements Specifics](#tag/Measurements-specifics) for details about data streaming and response formats.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the measurement. (optional)</param>
        /// <param name="dateTo">End date or date and time of the measurement. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  (optional, default to false)</param>
        /// <param name="source">The managed object ID to which the measurement is associated. (optional)</param>
        /// <param name="type">The type of measurement to search for. (optional)</param>
        /// <param name="valueFragmentSeries">The specific series to search for. (optional)</param>
        /// <param name="valueFragmentType">A characteristic which identifies the measurement. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of MeasurementCollection</returns>
        ApiResponse<MeasurementCollection> GetMeasurementCollectionResourceWithHttpInfo(int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), int? pageSize = default(int?), bool? revert = default(bool?), string? source = default(string?), string? type = default(string?), string? valueFragmentSeries = default(string?), string? valueFragmentType = default(string?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0);
        /// <summary>
        /// Retrieve a specific measurement
        /// </summary>
        /// <remarks>
        /// Retrieve a specific measurement by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_READ permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the measurement.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>Measurement</returns>
        Measurement GetMeasurementResource(string id, int operationIndex = 0);

        /// <summary>
        /// Retrieve a specific measurement
        /// </summary>
        /// <remarks>
        /// Retrieve a specific measurement by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_READ permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the measurement.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Measurement</returns>
        ApiResponse<Measurement> GetMeasurementResourceWithHttpInfo(string id, int operationIndex = 0);
        /// <summary>
        /// Retrieve a list of series and their values
        /// </summary>
        /// <remarks>
        /// Retrieve a list of series (all or only those matching the specified names) and their values within a given period of a specific managed object (source).&lt;br&gt; A series is any fragment in measurement that contains a &#x60;value&#x60; property.  It is possible to fetch aggregated results using the &#x60;aggregationType&#x60; parameter. If the aggregation is not specified, the result will contain no more than 5000 values.  &gt; **⚠️ Important:** For the aggregation to be done correctly, a device shall always use the same time zone when it sends dates.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_READ permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="dateFrom">Start date or date and time of the measurement.</param>
        /// <param name="dateTo">End date or date and time of the measurement.</param>
        /// <param name="source">The managed object ID to which the measurement is associated.</param>
        /// <param name="aggregationType">Fetch aggregated results as specified. (optional)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  (optional, default to false)</param>
        /// <param name="series">The specific series to search for. &gt;**&amp;#9432; Info:** If you query for multiple series at once, comma-separate the values.  (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>MeasurementSeries</returns>
        MeasurementSeries GetMeasurementSeriesResource(DateTime dateFrom, DateTime dateTo, string source, string? aggregationType = default(string?), bool? revert = default(bool?), List<string>? series = default(List<string>?), int operationIndex = 0);

        /// <summary>
        /// Retrieve a list of series and their values
        /// </summary>
        /// <remarks>
        /// Retrieve a list of series (all or only those matching the specified names) and their values within a given period of a specific managed object (source).&lt;br&gt; A series is any fragment in measurement that contains a &#x60;value&#x60; property.  It is possible to fetch aggregated results using the &#x60;aggregationType&#x60; parameter. If the aggregation is not specified, the result will contain no more than 5000 values.  &gt; **⚠️ Important:** For the aggregation to be done correctly, a device shall always use the same time zone when it sends dates.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_READ permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="dateFrom">Start date or date and time of the measurement.</param>
        /// <param name="dateTo">End date or date and time of the measurement.</param>
        /// <param name="source">The managed object ID to which the measurement is associated.</param>
        /// <param name="aggregationType">Fetch aggregated results as specified. (optional)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  (optional, default to false)</param>
        /// <param name="series">The specific series to search for. &gt;**&amp;#9432; Info:** If you query for multiple series at once, comma-separate the values.  (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of MeasurementSeries</returns>
        ApiResponse<MeasurementSeries> GetMeasurementSeriesResourceWithHttpInfo(DateTime dateFrom, DateTime dateTo, string source, string? aggregationType = default(string?), bool? revert = default(bool?), List<string>? series = default(List<string>?), int operationIndex = 0);
        /// <summary>
        /// Create a measurement
        /// </summary>
        /// <remarks>
        /// A measurement must be associated with a source (managed object) identified by ID, and must specify the type of measurement and the time when it was measured by the device (for example, a thermometer).  Each measurement fragment is an object (for example, &#x60;c8y_Steam&#x60;) containing the actual measurements as properties. The property name represents the name of the measurement (for example, &#x60;Temperature&#x60;) and it contains two properties:  *   &#x60;value&#x60; - The value of the individual measurement. The maximum precision for floating point numbers is 64-bit IEEE 754. For integers it&#39;s a 64-bit two&#39;s complement integer. The &#x60;value&#x60; is mandatory for a fragment. *   &#x60;unit&#x60; - The unit of the measurements.  Review the [System of units](#section/System-of-units) section for details about the conversions of units. Also review the [Naming conventions of fragments](https://cumulocity.com/guides/concepts/domain-model/#naming-conventions-of-fragments) in the Concepts guide.  The example below uses &#x60;c8y_Steam&#x60; in the request body to illustrate a fragment for recording temperature measurements.  &gt; **⚠️ Important:** Property names used for fragment and series must not contain whitespaces nor the special characters &#x60;. , * [ ] ( ) @ $&#x60;. This is required to ensure a correct processing and visualization of measurement series on UI graphs.  ### Create multiple measurements  It is also possible to create multiple measurements at once by sending a &#x60;measurements&#x60; array containing all the measurements to be created. The content type must be &#x60;application/vnd.com.nsn.cumulocity.measurementcollection+json&#x60;.  &gt; **&amp;#9432; Info:** For more details about fragments with specific meanings, review the sections [Device management library](#section/Device-management-library) and [Sensor library](#section/Sensor-library).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_ADMIN permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="measurement"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>Measurement</returns>
        Measurement PostMeasurementCollectionResource(Measurement measurement, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0);

        /// <summary>
        /// Create a measurement
        /// </summary>
        /// <remarks>
        /// A measurement must be associated with a source (managed object) identified by ID, and must specify the type of measurement and the time when it was measured by the device (for example, a thermometer).  Each measurement fragment is an object (for example, &#x60;c8y_Steam&#x60;) containing the actual measurements as properties. The property name represents the name of the measurement (for example, &#x60;Temperature&#x60;) and it contains two properties:  *   &#x60;value&#x60; - The value of the individual measurement. The maximum precision for floating point numbers is 64-bit IEEE 754. For integers it&#39;s a 64-bit two&#39;s complement integer. The &#x60;value&#x60; is mandatory for a fragment. *   &#x60;unit&#x60; - The unit of the measurements.  Review the [System of units](#section/System-of-units) section for details about the conversions of units. Also review the [Naming conventions of fragments](https://cumulocity.com/guides/concepts/domain-model/#naming-conventions-of-fragments) in the Concepts guide.  The example below uses &#x60;c8y_Steam&#x60; in the request body to illustrate a fragment for recording temperature measurements.  &gt; **⚠️ Important:** Property names used for fragment and series must not contain whitespaces nor the special characters &#x60;. , * [ ] ( ) @ $&#x60;. This is required to ensure a correct processing and visualization of measurement series on UI graphs.  ### Create multiple measurements  It is also possible to create multiple measurements at once by sending a &#x60;measurements&#x60; array containing all the measurements to be created. The content type must be &#x60;application/vnd.com.nsn.cumulocity.measurementcollection+json&#x60;.  &gt; **&amp;#9432; Info:** For more details about fragments with specific meanings, review the sections [Device management library](#section/Device-management-library) and [Sensor library](#section/Sensor-library).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_ADMIN permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="measurement"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Measurement</returns>
        ApiResponse<Measurement> PostMeasurementCollectionResourceWithHttpInfo(Measurement measurement, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0);
        #endregion Synchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IMeasurementsApiAsync : IApiAccessor
    {
        #region Asynchronous Operations
        /// <summary>
        /// Remove measurement collections
        /// </summary>
        /// <remarks>
        /// Remove measurement collections specified by query parameters.  DELETE requests are not synchronous. The response could be returned before the delete request has been completed. This may happen especially when there are a lot of measurements to be deleted.  &gt; **⚠️ Important:** Note that it is possible to call this endpoint without providing any parameter - it may result in deleting all measurements and it is not recommended.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="dateFrom">Start date or date and time of the measurement. (optional)</param>
        /// <param name="dateTo">End date or date and time of the measurement. (optional)</param>
        /// <param name="fragmentType">A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional)</param>
        /// <param name="source">The managed object ID to which the measurement is associated. (optional)</param>
        /// <param name="type">The type of measurement to search for. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DeleteMeasurementCollectionResourceAsync(string? xCumulocityProcessingMode = default(string?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? fragmentType = default(string?), string? source = default(string?), string? type = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Remove measurement collections
        /// </summary>
        /// <remarks>
        /// Remove measurement collections specified by query parameters.  DELETE requests are not synchronous. The response could be returned before the delete request has been completed. This may happen especially when there are a lot of measurements to be deleted.  &gt; **⚠️ Important:** Note that it is possible to call this endpoint without providing any parameter - it may result in deleting all measurements and it is not recommended.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="dateFrom">Start date or date and time of the measurement. (optional)</param>
        /// <param name="dateTo">End date or date and time of the measurement. (optional)</param>
        /// <param name="fragmentType">A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional)</param>
        /// <param name="source">The managed object ID to which the measurement is associated. (optional)</param>
        /// <param name="type">The type of measurement to search for. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DeleteMeasurementCollectionResourceWithHttpInfoAsync(string? xCumulocityProcessingMode = default(string?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? fragmentType = default(string?), string? source = default(string?), string? type = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Remove a specific measurement
        /// </summary>
        /// <remarks>
        /// Remove a specific measurement by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_ADMIN permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the measurement.</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DeleteMeasurementResourceAsync(string id, string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Remove a specific measurement
        /// </summary>
        /// <remarks>
        /// Remove a specific measurement by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_ADMIN permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the measurement.</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DeleteMeasurementResourceWithHttpInfoAsync(string id, string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Retrieve all measurements
        /// </summary>
        /// <remarks>
        /// Retrieve all measurements on your tenant, or a specific subset based on queries.  In case of executing [range queries](https://en.wikipedia.org/wiki/Range_query_(database)) between an upper and lower boundary, for example, querying using &#x60;dateFrom&#x60;–&#x60;dateTo&#x60;, the oldest registered measurements are returned first. It is possible to change the order using the query parameter &#x60;revert&#x3D;true&#x60;.  For large measurement collections, querying older records without filters can be slow as the server needs to scan from the beginning of the input results set before beginning to return the results. For cases when older measurements should be retrieved, it is recommended to narrow the scope by using range queries based on the time stamp reported by a device. The scope of query can also be reduced significantly when a source device is provided.  Review [Measurements Specifics](#tag/Measurements-specifics) for details about data streaming and response formats.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the measurement. (optional)</param>
        /// <param name="dateTo">End date or date and time of the measurement. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  (optional, default to false)</param>
        /// <param name="source">The managed object ID to which the measurement is associated. (optional)</param>
        /// <param name="type">The type of measurement to search for. (optional)</param>
        /// <param name="valueFragmentSeries">The specific series to search for. (optional)</param>
        /// <param name="valueFragmentType">A characteristic which identifies the measurement. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of MeasurementCollection</returns>
        System.Threading.Tasks.Task<MeasurementCollection> GetMeasurementCollectionResourceAsync(int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), int? pageSize = default(int?), bool? revert = default(bool?), string? source = default(string?), string? type = default(string?), string? valueFragmentSeries = default(string?), string? valueFragmentType = default(string?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Retrieve all measurements
        /// </summary>
        /// <remarks>
        /// Retrieve all measurements on your tenant, or a specific subset based on queries.  In case of executing [range queries](https://en.wikipedia.org/wiki/Range_query_(database)) between an upper and lower boundary, for example, querying using &#x60;dateFrom&#x60;–&#x60;dateTo&#x60;, the oldest registered measurements are returned first. It is possible to change the order using the query parameter &#x60;revert&#x3D;true&#x60;.  For large measurement collections, querying older records without filters can be slow as the server needs to scan from the beginning of the input results set before beginning to return the results. For cases when older measurements should be retrieved, it is recommended to narrow the scope by using range queries based on the time stamp reported by a device. The scope of query can also be reduced significantly when a source device is provided.  Review [Measurements Specifics](#tag/Measurements-specifics) for details about data streaming and response formats.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the measurement. (optional)</param>
        /// <param name="dateTo">End date or date and time of the measurement. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  (optional, default to false)</param>
        /// <param name="source">The managed object ID to which the measurement is associated. (optional)</param>
        /// <param name="type">The type of measurement to search for. (optional)</param>
        /// <param name="valueFragmentSeries">The specific series to search for. (optional)</param>
        /// <param name="valueFragmentType">A characteristic which identifies the measurement. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (MeasurementCollection)</returns>
        System.Threading.Tasks.Task<ApiResponse<MeasurementCollection>> GetMeasurementCollectionResourceWithHttpInfoAsync(int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), int? pageSize = default(int?), bool? revert = default(bool?), string? source = default(string?), string? type = default(string?), string? valueFragmentSeries = default(string?), string? valueFragmentType = default(string?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Retrieve a specific measurement
        /// </summary>
        /// <remarks>
        /// Retrieve a specific measurement by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_READ permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the measurement.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Measurement</returns>
        System.Threading.Tasks.Task<Measurement> GetMeasurementResourceAsync(string id, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Retrieve a specific measurement
        /// </summary>
        /// <remarks>
        /// Retrieve a specific measurement by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_READ permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the measurement.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Measurement)</returns>
        System.Threading.Tasks.Task<ApiResponse<Measurement>> GetMeasurementResourceWithHttpInfoAsync(string id, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Retrieve a list of series and their values
        /// </summary>
        /// <remarks>
        /// Retrieve a list of series (all or only those matching the specified names) and their values within a given period of a specific managed object (source).&lt;br&gt; A series is any fragment in measurement that contains a &#x60;value&#x60; property.  It is possible to fetch aggregated results using the &#x60;aggregationType&#x60; parameter. If the aggregation is not specified, the result will contain no more than 5000 values.  &gt; **⚠️ Important:** For the aggregation to be done correctly, a device shall always use the same time zone when it sends dates.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_READ permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="dateFrom">Start date or date and time of the measurement.</param>
        /// <param name="dateTo">End date or date and time of the measurement.</param>
        /// <param name="source">The managed object ID to which the measurement is associated.</param>
        /// <param name="aggregationType">Fetch aggregated results as specified. (optional)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  (optional, default to false)</param>
        /// <param name="series">The specific series to search for. &gt;**&amp;#9432; Info:** If you query for multiple series at once, comma-separate the values.  (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of MeasurementSeries</returns>
        System.Threading.Tasks.Task<MeasurementSeries> GetMeasurementSeriesResourceAsync(DateTime dateFrom, DateTime dateTo, string source, string? aggregationType = default(string?), bool? revert = default(bool?), List<string>? series = default(List<string>?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Retrieve a list of series and their values
        /// </summary>
        /// <remarks>
        /// Retrieve a list of series (all or only those matching the specified names) and their values within a given period of a specific managed object (source).&lt;br&gt; A series is any fragment in measurement that contains a &#x60;value&#x60; property.  It is possible to fetch aggregated results using the &#x60;aggregationType&#x60; parameter. If the aggregation is not specified, the result will contain no more than 5000 values.  &gt; **⚠️ Important:** For the aggregation to be done correctly, a device shall always use the same time zone when it sends dates.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_READ permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="dateFrom">Start date or date and time of the measurement.</param>
        /// <param name="dateTo">End date or date and time of the measurement.</param>
        /// <param name="source">The managed object ID to which the measurement is associated.</param>
        /// <param name="aggregationType">Fetch aggregated results as specified. (optional)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  (optional, default to false)</param>
        /// <param name="series">The specific series to search for. &gt;**&amp;#9432; Info:** If you query for multiple series at once, comma-separate the values.  (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (MeasurementSeries)</returns>
        System.Threading.Tasks.Task<ApiResponse<MeasurementSeries>> GetMeasurementSeriesResourceWithHttpInfoAsync(DateTime dateFrom, DateTime dateTo, string source, string? aggregationType = default(string?), bool? revert = default(bool?), List<string>? series = default(List<string>?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Create a measurement
        /// </summary>
        /// <remarks>
        /// A measurement must be associated with a source (managed object) identified by ID, and must specify the type of measurement and the time when it was measured by the device (for example, a thermometer).  Each measurement fragment is an object (for example, &#x60;c8y_Steam&#x60;) containing the actual measurements as properties. The property name represents the name of the measurement (for example, &#x60;Temperature&#x60;) and it contains two properties:  *   &#x60;value&#x60; - The value of the individual measurement. The maximum precision for floating point numbers is 64-bit IEEE 754. For integers it&#39;s a 64-bit two&#39;s complement integer. The &#x60;value&#x60; is mandatory for a fragment. *   &#x60;unit&#x60; - The unit of the measurements.  Review the [System of units](#section/System-of-units) section for details about the conversions of units. Also review the [Naming conventions of fragments](https://cumulocity.com/guides/concepts/domain-model/#naming-conventions-of-fragments) in the Concepts guide.  The example below uses &#x60;c8y_Steam&#x60; in the request body to illustrate a fragment for recording temperature measurements.  &gt; **⚠️ Important:** Property names used for fragment and series must not contain whitespaces nor the special characters &#x60;. , * [ ] ( ) @ $&#x60;. This is required to ensure a correct processing and visualization of measurement series on UI graphs.  ### Create multiple measurements  It is also possible to create multiple measurements at once by sending a &#x60;measurements&#x60; array containing all the measurements to be created. The content type must be &#x60;application/vnd.com.nsn.cumulocity.measurementcollection+json&#x60;.  &gt; **&amp;#9432; Info:** For more details about fragments with specific meanings, review the sections [Device management library](#section/Device-management-library) and [Sensor library](#section/Sensor-library).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_ADMIN permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="measurement"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Measurement</returns>
        System.Threading.Tasks.Task<Measurement> PostMeasurementCollectionResourceAsync(Measurement measurement, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Create a measurement
        /// </summary>
        /// <remarks>
        /// A measurement must be associated with a source (managed object) identified by ID, and must specify the type of measurement and the time when it was measured by the device (for example, a thermometer).  Each measurement fragment is an object (for example, &#x60;c8y_Steam&#x60;) containing the actual measurements as properties. The property name represents the name of the measurement (for example, &#x60;Temperature&#x60;) and it contains two properties:  *   &#x60;value&#x60; - The value of the individual measurement. The maximum precision for floating point numbers is 64-bit IEEE 754. For integers it&#39;s a 64-bit two&#39;s complement integer. The &#x60;value&#x60; is mandatory for a fragment. *   &#x60;unit&#x60; - The unit of the measurements.  Review the [System of units](#section/System-of-units) section for details about the conversions of units. Also review the [Naming conventions of fragments](https://cumulocity.com/guides/concepts/domain-model/#naming-conventions-of-fragments) in the Concepts guide.  The example below uses &#x60;c8y_Steam&#x60; in the request body to illustrate a fragment for recording temperature measurements.  &gt; **⚠️ Important:** Property names used for fragment and series must not contain whitespaces nor the special characters &#x60;. , * [ ] ( ) @ $&#x60;. This is required to ensure a correct processing and visualization of measurement series on UI graphs.  ### Create multiple measurements  It is also possible to create multiple measurements at once by sending a &#x60;measurements&#x60; array containing all the measurements to be created. The content type must be &#x60;application/vnd.com.nsn.cumulocity.measurementcollection+json&#x60;.  &gt; **&amp;#9432; Info:** For more details about fragments with specific meanings, review the sections [Device management library](#section/Device-management-library) and [Sensor library](#section/Sensor-library).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_ADMIN permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="measurement"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Measurement)</returns>
        System.Threading.Tasks.Task<ApiResponse<Measurement>> PostMeasurementCollectionResourceWithHttpInfoAsync(Measurement measurement, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IMeasurementsApi : IMeasurementsApiSync, IMeasurementsApiAsync
    {

    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class MeasurementsApi : IMeasurementsApi
    {
        private kern.services.CumulocityClient.Client.ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasurementsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public MeasurementsApi() : this((string)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasurementsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public MeasurementsApi(string basePath)
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
        /// Initializes a new instance of the <see cref="MeasurementsApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public MeasurementsApi(kern.services.CumulocityClient.Client.Configuration configuration)
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
        /// Initializes a new instance of the <see cref="MeasurementsApi"/> class
        /// using a Configuration object and client instance.
        /// </summary>
        /// <param name="client">The client interface for synchronous API access.</param>
        /// <param name="asyncClient">The client interface for asynchronous API access.</param>
        /// <param name="configuration">The configuration object.</param>
        public MeasurementsApi(kern.services.CumulocityClient.Client.ISynchronousClient client, kern.services.CumulocityClient.Client.IAsynchronousClient asyncClient, kern.services.CumulocityClient.Client.IReadableConfiguration configuration)
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
        /// Remove measurement collections Remove measurement collections specified by query parameters.  DELETE requests are not synchronous. The response could be returned before the delete request has been completed. This may happen especially when there are a lot of measurements to be deleted.  &gt; **⚠️ Important:** Note that it is possible to call this endpoint without providing any parameter - it may result in deleting all measurements and it is not recommended.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="dateFrom">Start date or date and time of the measurement. (optional)</param>
        /// <param name="dateTo">End date or date and time of the measurement. (optional)</param>
        /// <param name="fragmentType">A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional)</param>
        /// <param name="source">The managed object ID to which the measurement is associated. (optional)</param>
        /// <param name="type">The type of measurement to search for. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        public void DeleteMeasurementCollectionResource(string? xCumulocityProcessingMode = default(string?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? fragmentType = default(string?), string? source = default(string?), string? type = default(string?), int operationIndex = 0)
        {
            DeleteMeasurementCollectionResourceWithHttpInfo(xCumulocityProcessingMode, dateFrom, dateTo, fragmentType, source, type);
        }

        /// <summary>
        /// Remove measurement collections Remove measurement collections specified by query parameters.  DELETE requests are not synchronous. The response could be returned before the delete request has been completed. This may happen especially when there are a lot of measurements to be deleted.  &gt; **⚠️ Important:** Note that it is possible to call this endpoint without providing any parameter - it may result in deleting all measurements and it is not recommended.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="dateFrom">Start date or date and time of the measurement. (optional)</param>
        /// <param name="dateTo">End date or date and time of the measurement. (optional)</param>
        /// <param name="fragmentType">A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional)</param>
        /// <param name="source">The managed object ID to which the measurement is associated. (optional)</param>
        /// <param name="type">The type of measurement to search for. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<Object> DeleteMeasurementCollectionResourceWithHttpInfo(string? xCumulocityProcessingMode = default(string?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? fragmentType = default(string?), string? source = default(string?), string? type = default(string?), int operationIndex = 0)
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

            if (dateFrom != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateFrom", dateFrom));
            }
            if (dateTo != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateTo", dateTo));
            }
            if (fragmentType != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "fragmentType", fragmentType));
            }
            if (source != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "source", source));
            }
            if (type != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "type", type));
            }
            if (xCumulocityProcessingMode != null)
            {
                localVarRequestOptions.HeaderParameters.Add("X-Cumulocity-Processing-Mode", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(xCumulocityProcessingMode)); // header parameter
            }

            localVarRequestOptions.Operation = "MeasurementsApi.DeleteMeasurementCollectionResource";
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
            var localVarResponse = this.Client.Delete<Object>("/measurement/measurements", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteMeasurementCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Remove measurement collections Remove measurement collections specified by query parameters.  DELETE requests are not synchronous. The response could be returned before the delete request has been completed. This may happen especially when there are a lot of measurements to be deleted.  &gt; **⚠️ Important:** Note that it is possible to call this endpoint without providing any parameter - it may result in deleting all measurements and it is not recommended.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="dateFrom">Start date or date and time of the measurement. (optional)</param>
        /// <param name="dateTo">End date or date and time of the measurement. (optional)</param>
        /// <param name="fragmentType">A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional)</param>
        /// <param name="source">The managed object ID to which the measurement is associated. (optional)</param>
        /// <param name="type">The type of measurement to search for. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DeleteMeasurementCollectionResourceAsync(string? xCumulocityProcessingMode = default(string?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? fragmentType = default(string?), string? source = default(string?), string? type = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            await DeleteMeasurementCollectionResourceWithHttpInfoAsync(xCumulocityProcessingMode, dateFrom, dateTo, fragmentType, source, type, operationIndex, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Remove measurement collections Remove measurement collections specified by query parameters.  DELETE requests are not synchronous. The response could be returned before the delete request has been completed. This may happen especially when there are a lot of measurements to be deleted.  &gt; **⚠️ Important:** Note that it is possible to call this endpoint without providing any parameter - it may result in deleting all measurements and it is not recommended.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="dateFrom">Start date or date and time of the measurement. (optional)</param>
        /// <param name="dateTo">End date or date and time of the measurement. (optional)</param>
        /// <param name="fragmentType">A characteristic which identifies a managed object or event, for example, geolocation, electricity sensor, relay state. (optional)</param>
        /// <param name="source">The managed object ID to which the measurement is associated. (optional)</param>
        /// <param name="type">The type of measurement to search for. (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<Object>> DeleteMeasurementCollectionResourceWithHttpInfoAsync(string? xCumulocityProcessingMode = default(string?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), string? fragmentType = default(string?), string? source = default(string?), string? type = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
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

            if (dateFrom != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateFrom", dateFrom));
            }
            if (dateTo != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateTo", dateTo));
            }
            if (fragmentType != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "fragmentType", fragmentType));
            }
            if (source != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "source", source));
            }
            if (type != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "type", type));
            }
            if (xCumulocityProcessingMode != null)
            {
                localVarRequestOptions.HeaderParameters.Add("X-Cumulocity-Processing-Mode", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(xCumulocityProcessingMode)); // header parameter
            }

            localVarRequestOptions.Operation = "MeasurementsApi.DeleteMeasurementCollectionResource";
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
            var localVarResponse = await this.AsynchronousClient.DeleteAsync<Object>("/measurement/measurements", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteMeasurementCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Remove a specific measurement Remove a specific measurement by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_ADMIN permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the measurement.</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        public void DeleteMeasurementResource(string id, string? xCumulocityProcessingMode = default(string?), int operationIndex = 0)
        {
            DeleteMeasurementResourceWithHttpInfo(id, xCumulocityProcessingMode);
        }

        /// <summary>
        /// Remove a specific measurement Remove a specific measurement by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_ADMIN permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the measurement.</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<Object> DeleteMeasurementResourceWithHttpInfo(string id, string? xCumulocityProcessingMode = default(string?), int operationIndex = 0)
        {
            // verify the required parameter 'id' is set
            if (id == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'id' when calling MeasurementsApi->DeleteMeasurementResource");
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

            localVarRequestOptions.PathParameters.Add("id", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(id)); // path parameter
            if (xCumulocityProcessingMode != null)
            {
                localVarRequestOptions.HeaderParameters.Add("X-Cumulocity-Processing-Mode", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(xCumulocityProcessingMode)); // header parameter
            }

            localVarRequestOptions.Operation = "MeasurementsApi.DeleteMeasurementResource";
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
            var localVarResponse = this.Client.Delete<Object>("/measurement/measurements/{id}", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteMeasurementResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Remove a specific measurement Remove a specific measurement by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_ADMIN permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the measurement.</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DeleteMeasurementResourceAsync(string id, string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            await DeleteMeasurementResourceWithHttpInfoAsync(id, xCumulocityProcessingMode, operationIndex, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Remove a specific measurement Remove a specific measurement by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_ADMIN permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the measurement.</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<Object>> DeleteMeasurementResourceWithHttpInfoAsync(string id, string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'id' when calling MeasurementsApi->DeleteMeasurementResource");
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

            localVarRequestOptions.PathParameters.Add("id", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(id)); // path parameter
            if (xCumulocityProcessingMode != null)
            {
                localVarRequestOptions.HeaderParameters.Add("X-Cumulocity-Processing-Mode", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(xCumulocityProcessingMode)); // header parameter
            }

            localVarRequestOptions.Operation = "MeasurementsApi.DeleteMeasurementResource";
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
            var localVarResponse = await this.AsynchronousClient.DeleteAsync<Object>("/measurement/measurements/{id}", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteMeasurementResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve all measurements Retrieve all measurements on your tenant, or a specific subset based on queries.  In case of executing [range queries](https://en.wikipedia.org/wiki/Range_query_(database)) between an upper and lower boundary, for example, querying using &#x60;dateFrom&#x60;–&#x60;dateTo&#x60;, the oldest registered measurements are returned first. It is possible to change the order using the query parameter &#x60;revert&#x3D;true&#x60;.  For large measurement collections, querying older records without filters can be slow as the server needs to scan from the beginning of the input results set before beginning to return the results. For cases when older measurements should be retrieved, it is recommended to narrow the scope by using range queries based on the time stamp reported by a device. The scope of query can also be reduced significantly when a source device is provided.  Review [Measurements Specifics](#tag/Measurements-specifics) for details about data streaming and response formats.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the measurement. (optional)</param>
        /// <param name="dateTo">End date or date and time of the measurement. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  (optional, default to false)</param>
        /// <param name="source">The managed object ID to which the measurement is associated. (optional)</param>
        /// <param name="type">The type of measurement to search for. (optional)</param>
        /// <param name="valueFragmentSeries">The specific series to search for. (optional)</param>
        /// <param name="valueFragmentType">A characteristic which identifies the measurement. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>MeasurementCollection</returns>
        public MeasurementCollection GetMeasurementCollectionResource(int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), int? pageSize = default(int?), bool? revert = default(bool?), string? source = default(string?), string? type = default(string?), string? valueFragmentSeries = default(string?), string? valueFragmentType = default(string?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<MeasurementCollection> localVarResponse = GetMeasurementCollectionResourceWithHttpInfo(currentPage, dateFrom, dateTo, pageSize, revert, source, type, valueFragmentSeries, valueFragmentType, withTotalElements, withTotalPages);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve all measurements Retrieve all measurements on your tenant, or a specific subset based on queries.  In case of executing [range queries](https://en.wikipedia.org/wiki/Range_query_(database)) between an upper and lower boundary, for example, querying using &#x60;dateFrom&#x60;–&#x60;dateTo&#x60;, the oldest registered measurements are returned first. It is possible to change the order using the query parameter &#x60;revert&#x3D;true&#x60;.  For large measurement collections, querying older records without filters can be slow as the server needs to scan from the beginning of the input results set before beginning to return the results. For cases when older measurements should be retrieved, it is recommended to narrow the scope by using range queries based on the time stamp reported by a device. The scope of query can also be reduced significantly when a source device is provided.  Review [Measurements Specifics](#tag/Measurements-specifics) for details about data streaming and response formats.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the measurement. (optional)</param>
        /// <param name="dateTo">End date or date and time of the measurement. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  (optional, default to false)</param>
        /// <param name="source">The managed object ID to which the measurement is associated. (optional)</param>
        /// <param name="type">The type of measurement to search for. (optional)</param>
        /// <param name="valueFragmentSeries">The specific series to search for. (optional)</param>
        /// <param name="valueFragmentType">A characteristic which identifies the measurement. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of MeasurementCollection</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<MeasurementCollection> GetMeasurementCollectionResourceWithHttpInfo(int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), int? pageSize = default(int?), bool? revert = default(bool?), string? source = default(string?), string? type = default(string?), string? valueFragmentSeries = default(string?), string? valueFragmentType = default(string?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.measurementcollection+json",
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
            if (pageSize != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "pageSize", pageSize));
            }
            if (revert != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "revert", revert));
            }
            if (source != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "source", source));
            }
            if (type != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "type", type));
            }
            if (valueFragmentSeries != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "valueFragmentSeries", valueFragmentSeries));
            }
            if (valueFragmentType != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "valueFragmentType", valueFragmentType));
            }
            if (withTotalElements != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withTotalElements", withTotalElements));
            }
            if (withTotalPages != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withTotalPages", withTotalPages));
            }

            localVarRequestOptions.Operation = "MeasurementsApi.GetMeasurementCollectionResource";
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
            var localVarResponse = this.Client.Get<MeasurementCollection>("/measurement/measurements", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetMeasurementCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve all measurements Retrieve all measurements on your tenant, or a specific subset based on queries.  In case of executing [range queries](https://en.wikipedia.org/wiki/Range_query_(database)) between an upper and lower boundary, for example, querying using &#x60;dateFrom&#x60;–&#x60;dateTo&#x60;, the oldest registered measurements are returned first. It is possible to change the order using the query parameter &#x60;revert&#x3D;true&#x60;.  For large measurement collections, querying older records without filters can be slow as the server needs to scan from the beginning of the input results set before beginning to return the results. For cases when older measurements should be retrieved, it is recommended to narrow the scope by using range queries based on the time stamp reported by a device. The scope of query can also be reduced significantly when a source device is provided.  Review [Measurements Specifics](#tag/Measurements-specifics) for details about data streaming and response formats.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the measurement. (optional)</param>
        /// <param name="dateTo">End date or date and time of the measurement. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  (optional, default to false)</param>
        /// <param name="source">The managed object ID to which the measurement is associated. (optional)</param>
        /// <param name="type">The type of measurement to search for. (optional)</param>
        /// <param name="valueFragmentSeries">The specific series to search for. (optional)</param>
        /// <param name="valueFragmentType">A characteristic which identifies the measurement. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of MeasurementCollection</returns>
        public async System.Threading.Tasks.Task<MeasurementCollection> GetMeasurementCollectionResourceAsync(int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), int? pageSize = default(int?), bool? revert = default(bool?), string? source = default(string?), string? type = default(string?), string? valueFragmentSeries = default(string?), string? valueFragmentType = default(string?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<MeasurementCollection> localVarResponse = await GetMeasurementCollectionResourceWithHttpInfoAsync(currentPage, dateFrom, dateTo, pageSize, revert, source, type, valueFragmentSeries, valueFragmentType, withTotalElements, withTotalPages, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve all measurements Retrieve all measurements on your tenant, or a specific subset based on queries.  In case of executing [range queries](https://en.wikipedia.org/wiki/Range_query_(database)) between an upper and lower boundary, for example, querying using &#x60;dateFrom&#x60;–&#x60;dateTo&#x60;, the oldest registered measurements are returned first. It is possible to change the order using the query parameter &#x60;revert&#x3D;true&#x60;.  For large measurement collections, querying older records without filters can be slow as the server needs to scan from the beginning of the input results set before beginning to return the results. For cases when older measurements should be retrieved, it is recommended to narrow the scope by using range queries based on the time stamp reported by a device. The scope of query can also be reduced significantly when a source device is provided.  Review [Measurements Specifics](#tag/Measurements-specifics) for details about data streaming and response formats.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the measurement. (optional)</param>
        /// <param name="dateTo">End date or date and time of the measurement. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  (optional, default to false)</param>
        /// <param name="source">The managed object ID to which the measurement is associated. (optional)</param>
        /// <param name="type">The type of measurement to search for. (optional)</param>
        /// <param name="valueFragmentSeries">The specific series to search for. (optional)</param>
        /// <param name="valueFragmentType">A characteristic which identifies the measurement. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (MeasurementCollection)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<MeasurementCollection>> GetMeasurementCollectionResourceWithHttpInfoAsync(int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), int? pageSize = default(int?), bool? revert = default(bool?), string? source = default(string?), string? type = default(string?), string? valueFragmentSeries = default(string?), string? valueFragmentType = default(string?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.measurementcollection+json",
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
            if (pageSize != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "pageSize", pageSize));
            }
            if (revert != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "revert", revert));
            }
            if (source != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "source", source));
            }
            if (type != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "type", type));
            }
            if (valueFragmentSeries != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "valueFragmentSeries", valueFragmentSeries));
            }
            if (valueFragmentType != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "valueFragmentType", valueFragmentType));
            }
            if (withTotalElements != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withTotalElements", withTotalElements));
            }
            if (withTotalPages != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withTotalPages", withTotalPages));
            }

            localVarRequestOptions.Operation = "MeasurementsApi.GetMeasurementCollectionResource";
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
            var localVarResponse = await this.AsynchronousClient.GetAsync<MeasurementCollection>("/measurement/measurements", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetMeasurementCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve a specific measurement Retrieve a specific measurement by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_READ permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the measurement.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>Measurement</returns>
        public Measurement GetMeasurementResource(string id, int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<Measurement> localVarResponse = GetMeasurementResourceWithHttpInfo(id);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve a specific measurement Retrieve a specific measurement by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_READ permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the measurement.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Measurement</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<Measurement> GetMeasurementResourceWithHttpInfo(string id, int operationIndex = 0)
        {
            // verify the required parameter 'id' is set
            if (id == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'id' when calling MeasurementsApi->GetMeasurementResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.measurement+json",
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

            localVarRequestOptions.Operation = "MeasurementsApi.GetMeasurementResource";
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
            var localVarResponse = this.Client.Get<Measurement>("/measurement/measurements/{id}", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetMeasurementResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve a specific measurement Retrieve a specific measurement by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_READ permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the measurement.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Measurement</returns>
        public async System.Threading.Tasks.Task<Measurement> GetMeasurementResourceAsync(string id, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<Measurement> localVarResponse = await GetMeasurementResourceWithHttpInfoAsync(id, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve a specific measurement Retrieve a specific measurement by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_READ permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the measurement.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Measurement)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<Measurement>> GetMeasurementResourceWithHttpInfoAsync(string id, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'id' when calling MeasurementsApi->GetMeasurementResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.measurement+json",
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

            localVarRequestOptions.Operation = "MeasurementsApi.GetMeasurementResource";
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
            var localVarResponse = await this.AsynchronousClient.GetAsync<Measurement>("/measurement/measurements/{id}", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetMeasurementResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve a list of series and their values Retrieve a list of series (all or only those matching the specified names) and their values within a given period of a specific managed object (source).&lt;br&gt; A series is any fragment in measurement that contains a &#x60;value&#x60; property.  It is possible to fetch aggregated results using the &#x60;aggregationType&#x60; parameter. If the aggregation is not specified, the result will contain no more than 5000 values.  &gt; **⚠️ Important:** For the aggregation to be done correctly, a device shall always use the same time zone when it sends dates.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_READ permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="dateFrom">Start date or date and time of the measurement.</param>
        /// <param name="dateTo">End date or date and time of the measurement.</param>
        /// <param name="source">The managed object ID to which the measurement is associated.</param>
        /// <param name="aggregationType">Fetch aggregated results as specified. (optional)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  (optional, default to false)</param>
        /// <param name="series">The specific series to search for. &gt;**&amp;#9432; Info:** If you query for multiple series at once, comma-separate the values.  (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>MeasurementSeries</returns>
        public MeasurementSeries GetMeasurementSeriesResource(DateTime dateFrom, DateTime dateTo, string source, string? aggregationType = default(string?), bool? revert = default(bool?), List<string>? series = default(List<string>?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<MeasurementSeries> localVarResponse = GetMeasurementSeriesResourceWithHttpInfo(dateFrom, dateTo, source, aggregationType, revert, series);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve a list of series and their values Retrieve a list of series (all or only those matching the specified names) and their values within a given period of a specific managed object (source).&lt;br&gt; A series is any fragment in measurement that contains a &#x60;value&#x60; property.  It is possible to fetch aggregated results using the &#x60;aggregationType&#x60; parameter. If the aggregation is not specified, the result will contain no more than 5000 values.  &gt; **⚠️ Important:** For the aggregation to be done correctly, a device shall always use the same time zone when it sends dates.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_READ permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="dateFrom">Start date or date and time of the measurement.</param>
        /// <param name="dateTo">End date or date and time of the measurement.</param>
        /// <param name="source">The managed object ID to which the measurement is associated.</param>
        /// <param name="aggregationType">Fetch aggregated results as specified. (optional)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  (optional, default to false)</param>
        /// <param name="series">The specific series to search for. &gt;**&amp;#9432; Info:** If you query for multiple series at once, comma-separate the values.  (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of MeasurementSeries</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<MeasurementSeries> GetMeasurementSeriesResourceWithHttpInfo(DateTime dateFrom, DateTime dateTo, string source, string? aggregationType = default(string?), bool? revert = default(bool?), List<string>? series = default(List<string>?), int operationIndex = 0)
        {
            // verify the required parameter 'source' is set
            if (source == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'source' when calling MeasurementsApi->GetMeasurementSeriesResource");
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

            if (aggregationType != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "aggregationType", aggregationType));
            }
            localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateFrom", dateFrom));
            localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateTo", dateTo));
            if (revert != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "revert", revert));
            }
            if (series != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "series", series));
            }
            localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "source", source));

            localVarRequestOptions.Operation = "MeasurementsApi.GetMeasurementSeriesResource";
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
            var localVarResponse = this.Client.Get<MeasurementSeries>("/measurement/measurements/series", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetMeasurementSeriesResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve a list of series and their values Retrieve a list of series (all or only those matching the specified names) and their values within a given period of a specific managed object (source).&lt;br&gt; A series is any fragment in measurement that contains a &#x60;value&#x60; property.  It is possible to fetch aggregated results using the &#x60;aggregationType&#x60; parameter. If the aggregation is not specified, the result will contain no more than 5000 values.  &gt; **⚠️ Important:** For the aggregation to be done correctly, a device shall always use the same time zone when it sends dates.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_READ permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="dateFrom">Start date or date and time of the measurement.</param>
        /// <param name="dateTo">End date or date and time of the measurement.</param>
        /// <param name="source">The managed object ID to which the measurement is associated.</param>
        /// <param name="aggregationType">Fetch aggregated results as specified. (optional)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  (optional, default to false)</param>
        /// <param name="series">The specific series to search for. &gt;**&amp;#9432; Info:** If you query for multiple series at once, comma-separate the values.  (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of MeasurementSeries</returns>
        public async System.Threading.Tasks.Task<MeasurementSeries> GetMeasurementSeriesResourceAsync(DateTime dateFrom, DateTime dateTo, string source, string? aggregationType = default(string?), bool? revert = default(bool?), List<string>? series = default(List<string>?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<MeasurementSeries> localVarResponse = await GetMeasurementSeriesResourceWithHttpInfoAsync(dateFrom, dateTo, source, aggregationType, revert, series, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve a list of series and their values Retrieve a list of series (all or only those matching the specified names) and their values within a given period of a specific managed object (source).&lt;br&gt; A series is any fragment in measurement that contains a &#x60;value&#x60; property.  It is possible to fetch aggregated results using the &#x60;aggregationType&#x60; parameter. If the aggregation is not specified, the result will contain no more than 5000 values.  &gt; **⚠️ Important:** For the aggregation to be done correctly, a device shall always use the same time zone when it sends dates.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_READ permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="dateFrom">Start date or date and time of the measurement.</param>
        /// <param name="dateTo">End date or date and time of the measurement.</param>
        /// <param name="source">The managed object ID to which the measurement is associated.</param>
        /// <param name="aggregationType">Fetch aggregated results as specified. (optional)</param>
        /// <param name="revert">If you are using a range query (that is, at least one of the &#x60;dateFrom&#x60; or &#x60;dateTo&#x60; parameters is included in the request), then setting &#x60;revert&#x3D;true&#x60; will sort the results by the newest measurements first. By default, the results are sorted by the oldest measurements first.  (optional, default to false)</param>
        /// <param name="series">The specific series to search for. &gt;**&amp;#9432; Info:** If you query for multiple series at once, comma-separate the values.  (optional)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (MeasurementSeries)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<MeasurementSeries>> GetMeasurementSeriesResourceWithHttpInfoAsync(DateTime dateFrom, DateTime dateTo, string source, string? aggregationType = default(string?), bool? revert = default(bool?), List<string>? series = default(List<string>?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'source' is set
            if (source == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'source' when calling MeasurementsApi->GetMeasurementSeriesResource");
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

            if (aggregationType != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "aggregationType", aggregationType));
            }
            localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateFrom", dateFrom));
            localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateTo", dateTo));
            if (revert != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "revert", revert));
            }
            if (series != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "series", series));
            }
            localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "source", source));

            localVarRequestOptions.Operation = "MeasurementsApi.GetMeasurementSeriesResource";
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
            var localVarResponse = await this.AsynchronousClient.GetAsync<MeasurementSeries>("/measurement/measurements/series", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetMeasurementSeriesResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Create a measurement A measurement must be associated with a source (managed object) identified by ID, and must specify the type of measurement and the time when it was measured by the device (for example, a thermometer).  Each measurement fragment is an object (for example, &#x60;c8y_Steam&#x60;) containing the actual measurements as properties. The property name represents the name of the measurement (for example, &#x60;Temperature&#x60;) and it contains two properties:  *   &#x60;value&#x60; - The value of the individual measurement. The maximum precision for floating point numbers is 64-bit IEEE 754. For integers it&#39;s a 64-bit two&#39;s complement integer. The &#x60;value&#x60; is mandatory for a fragment. *   &#x60;unit&#x60; - The unit of the measurements.  Review the [System of units](#section/System-of-units) section for details about the conversions of units. Also review the [Naming conventions of fragments](https://cumulocity.com/guides/concepts/domain-model/#naming-conventions-of-fragments) in the Concepts guide.  The example below uses &#x60;c8y_Steam&#x60; in the request body to illustrate a fragment for recording temperature measurements.  &gt; **⚠️ Important:** Property names used for fragment and series must not contain whitespaces nor the special characters &#x60;. , * [ ] ( ) @ $&#x60;. This is required to ensure a correct processing and visualization of measurement series on UI graphs.  ### Create multiple measurements  It is also possible to create multiple measurements at once by sending a &#x60;measurements&#x60; array containing all the measurements to be created. The content type must be &#x60;application/vnd.com.nsn.cumulocity.measurementcollection+json&#x60;.  &gt; **&amp;#9432; Info:** For more details about fragments with specific meanings, review the sections [Device management library](#section/Device-management-library) and [Sensor library](#section/Sensor-library).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_ADMIN permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="measurement"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>Measurement</returns>
        public Measurement PostMeasurementCollectionResource(Measurement measurement, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<Measurement> localVarResponse = PostMeasurementCollectionResourceWithHttpInfo(measurement, accept, xCumulocityProcessingMode);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Create a measurement A measurement must be associated with a source (managed object) identified by ID, and must specify the type of measurement and the time when it was measured by the device (for example, a thermometer).  Each measurement fragment is an object (for example, &#x60;c8y_Steam&#x60;) containing the actual measurements as properties. The property name represents the name of the measurement (for example, &#x60;Temperature&#x60;) and it contains two properties:  *   &#x60;value&#x60; - The value of the individual measurement. The maximum precision for floating point numbers is 64-bit IEEE 754. For integers it&#39;s a 64-bit two&#39;s complement integer. The &#x60;value&#x60; is mandatory for a fragment. *   &#x60;unit&#x60; - The unit of the measurements.  Review the [System of units](#section/System-of-units) section for details about the conversions of units. Also review the [Naming conventions of fragments](https://cumulocity.com/guides/concepts/domain-model/#naming-conventions-of-fragments) in the Concepts guide.  The example below uses &#x60;c8y_Steam&#x60; in the request body to illustrate a fragment for recording temperature measurements.  &gt; **⚠️ Important:** Property names used for fragment and series must not contain whitespaces nor the special characters &#x60;. , * [ ] ( ) @ $&#x60;. This is required to ensure a correct processing and visualization of measurement series on UI graphs.  ### Create multiple measurements  It is also possible to create multiple measurements at once by sending a &#x60;measurements&#x60; array containing all the measurements to be created. The content type must be &#x60;application/vnd.com.nsn.cumulocity.measurementcollection+json&#x60;.  &gt; **&amp;#9432; Info:** For more details about fragments with specific meanings, review the sections [Device management library](#section/Device-management-library) and [Sensor library](#section/Sensor-library).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_ADMIN permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="measurement"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Measurement</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<Measurement> PostMeasurementCollectionResourceWithHttpInfo(Measurement measurement, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0)
        {
            // verify the required parameter 'measurement' is set
            if (measurement == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'measurement' when calling MeasurementsApi->PostMeasurementCollectionResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/vnd.com.nsn.cumulocity.measurement+json",
                "application/vnd.com.nsn.cumulocity.measurementcollection+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.measurement+json",
                "application/vnd.com.nsn.cumulocity.measurementcollection+json",
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
            localVarRequestOptions.Data = measurement;

            localVarRequestOptions.Operation = "MeasurementsApi.PostMeasurementCollectionResource";
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
            var localVarResponse = this.Client.Post<Measurement>("/measurement/measurements", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PostMeasurementCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Create a measurement A measurement must be associated with a source (managed object) identified by ID, and must specify the type of measurement and the time when it was measured by the device (for example, a thermometer).  Each measurement fragment is an object (for example, &#x60;c8y_Steam&#x60;) containing the actual measurements as properties. The property name represents the name of the measurement (for example, &#x60;Temperature&#x60;) and it contains two properties:  *   &#x60;value&#x60; - The value of the individual measurement. The maximum precision for floating point numbers is 64-bit IEEE 754. For integers it&#39;s a 64-bit two&#39;s complement integer. The &#x60;value&#x60; is mandatory for a fragment. *   &#x60;unit&#x60; - The unit of the measurements.  Review the [System of units](#section/System-of-units) section for details about the conversions of units. Also review the [Naming conventions of fragments](https://cumulocity.com/guides/concepts/domain-model/#naming-conventions-of-fragments) in the Concepts guide.  The example below uses &#x60;c8y_Steam&#x60; in the request body to illustrate a fragment for recording temperature measurements.  &gt; **⚠️ Important:** Property names used for fragment and series must not contain whitespaces nor the special characters &#x60;. , * [ ] ( ) @ $&#x60;. This is required to ensure a correct processing and visualization of measurement series on UI graphs.  ### Create multiple measurements  It is also possible to create multiple measurements at once by sending a &#x60;measurements&#x60; array containing all the measurements to be created. The content type must be &#x60;application/vnd.com.nsn.cumulocity.measurementcollection+json&#x60;.  &gt; **&amp;#9432; Info:** For more details about fragments with specific meanings, review the sections [Device management library](#section/Device-management-library) and [Sensor library](#section/Sensor-library).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_ADMIN permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="measurement"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Measurement</returns>
        public async System.Threading.Tasks.Task<Measurement> PostMeasurementCollectionResourceAsync(Measurement measurement, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<Measurement> localVarResponse = await PostMeasurementCollectionResourceWithHttpInfoAsync(measurement, accept, xCumulocityProcessingMode, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Create a measurement A measurement must be associated with a source (managed object) identified by ID, and must specify the type of measurement and the time when it was measured by the device (for example, a thermometer).  Each measurement fragment is an object (for example, &#x60;c8y_Steam&#x60;) containing the actual measurements as properties. The property name represents the name of the measurement (for example, &#x60;Temperature&#x60;) and it contains two properties:  *   &#x60;value&#x60; - The value of the individual measurement. The maximum precision for floating point numbers is 64-bit IEEE 754. For integers it&#39;s a 64-bit two&#39;s complement integer. The &#x60;value&#x60; is mandatory for a fragment. *   &#x60;unit&#x60; - The unit of the measurements.  Review the [System of units](#section/System-of-units) section for details about the conversions of units. Also review the [Naming conventions of fragments](https://cumulocity.com/guides/concepts/domain-model/#naming-conventions-of-fragments) in the Concepts guide.  The example below uses &#x60;c8y_Steam&#x60; in the request body to illustrate a fragment for recording temperature measurements.  &gt; **⚠️ Important:** Property names used for fragment and series must not contain whitespaces nor the special characters &#x60;. , * [ ] ( ) @ $&#x60;. This is required to ensure a correct processing and visualization of measurement series on UI graphs.  ### Create multiple measurements  It is also possible to create multiple measurements at once by sending a &#x60;measurements&#x60; array containing all the measurements to be created. The content type must be &#x60;application/vnd.com.nsn.cumulocity.measurementcollection+json&#x60;.  &gt; **&amp;#9432; Info:** For more details about fragments with specific meanings, review the sections [Device management library](#section/Device-management-library) and [Sensor library](#section/Sensor-library).  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_MEASUREMENT_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; MEASUREMENT_ADMIN permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="measurement"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Measurement)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<Measurement>> PostMeasurementCollectionResourceWithHttpInfoAsync(Measurement measurement, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'measurement' is set
            if (measurement == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'measurement' when calling MeasurementsApi->PostMeasurementCollectionResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/vnd.com.nsn.cumulocity.measurement+json", 
                "application/vnd.com.nsn.cumulocity.measurementcollection+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.measurement+json",
                "application/vnd.com.nsn.cumulocity.measurementcollection+json",
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
            localVarRequestOptions.Data = measurement;

            localVarRequestOptions.Operation = "MeasurementsApi.PostMeasurementCollectionResource";
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
            var localVarResponse = await this.AsynchronousClient.PostAsync<Measurement>("/measurement/measurements", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PostMeasurementCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

    }
}
