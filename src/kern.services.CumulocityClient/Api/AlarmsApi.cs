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
    public interface IAlarmsApiSync : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Remove alarm collections
        /// </summary>
        /// <remarks>
        /// Remove alarm collections specified by query parameters.  &gt; **⚠️ Important:** Note that it is possible to call this endpoint without providing any parameter - it will result in deleting all alarms and it is not recommended. &gt; Also note that DELETE requests are not synchronous. The response could be returned before the delete request has been completed.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        void DeleteAlarmCollectionResource(string? xCumulocityProcessingMode = default(string?), DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0);

        /// <summary>
        /// Remove alarm collections
        /// </summary>
        /// <remarks>
        /// Remove alarm collections specified by query parameters.  &gt; **⚠️ Important:** Note that it is possible to call this endpoint without providing any parameter - it will result in deleting all alarms and it is not recommended. &gt; Also note that DELETE requests are not synchronous. The response could be returned before the delete request has been completed.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DeleteAlarmCollectionResourceWithHttpInfo(string? xCumulocityProcessingMode = default(string?), DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0);
        /// <summary>
        /// Retrieve the total number of alarms
        /// </summary>
        /// <remarks>
        /// Count the total number of active alarms on your tenant.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; The role ROLE_ALARM_READ is not required, but if a user has this role, all the alarms on the tenant are counted. Otherwise, inventory role permissions are used to count the alarms and the limit is 100. &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>int</returns>
        int GetAlarmCollectionCountResource(DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0);

        /// <summary>
        /// Retrieve the total number of alarms
        /// </summary>
        /// <remarks>
        /// Count the total number of active alarms on your tenant.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; The role ROLE_ALARM_READ is not required, but if a user has this role, all the alarms on the tenant are counted. Otherwise, inventory role permissions are used to count the alarms and the limit is 100. &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of int</returns>
        ApiResponse<int> GetAlarmCollectionCountResourceWithHttpInfo(DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0);
        /// <summary>
        /// Retrieve all alarms
        /// </summary>
        /// <remarks>
        /// Retrieve all alarms on your tenant, or a specific subset based on queries. The results are sorted by the newest alarms first.  #### Query parameters  The query parameter &#x60;withTotalPages&#x60; only works when the user has the ROLE_ALARM_READ role, otherwise it is ignored.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; The role ROLE_ALARM_READ is not required, but if a user has this role, all the alarms on the tenant are returned. If a user has access to alarms through inventory roles, only those alarms are returned. &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="lastUpdatedFrom">Start date or date and time of the last update made. (optional)</param>
        /// <param name="lastUpdatedTo">End date or date and time of the last update made. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>AlarmCollection</returns>
        AlarmCollection GetAlarmCollectionResource(DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), DateTime? lastUpdatedFrom = default(DateTime?), DateTime? lastUpdatedTo = default(DateTime?), int? pageSize = default(int?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0);

        /// <summary>
        /// Retrieve all alarms
        /// </summary>
        /// <remarks>
        /// Retrieve all alarms on your tenant, or a specific subset based on queries. The results are sorted by the newest alarms first.  #### Query parameters  The query parameter &#x60;withTotalPages&#x60; only works when the user has the ROLE_ALARM_READ role, otherwise it is ignored.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; The role ROLE_ALARM_READ is not required, but if a user has this role, all the alarms on the tenant are returned. If a user has access to alarms through inventory roles, only those alarms are returned. &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="lastUpdatedFrom">Start date or date and time of the last update made. (optional)</param>
        /// <param name="lastUpdatedTo">End date or date and time of the last update made. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of AlarmCollection</returns>
        ApiResponse<AlarmCollection> GetAlarmCollectionResourceWithHttpInfo(DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), DateTime? lastUpdatedFrom = default(DateTime?), DateTime? lastUpdatedTo = default(DateTime?), int? pageSize = default(int?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0);
        /// <summary>
        /// Retrieve a specific alarm
        /// </summary>
        /// <remarks>
        /// Retrieve a specific alarm by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_READ permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the alarm.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>Alarm</returns>
        Alarm GetAlarmResource(string id, int operationIndex = 0);

        /// <summary>
        /// Retrieve a specific alarm
        /// </summary>
        /// <remarks>
        /// Retrieve a specific alarm by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_READ permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the alarm.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Alarm</returns>
        ApiResponse<Alarm> GetAlarmResourceWithHttpInfo(string id, int operationIndex = 0);
        /// <summary>
        /// Create an alarm
        /// </summary>
        /// <remarks>
        /// An alarm must be associated with a source (managed object) identified by ID.&lt;br&gt; In general, each alarm may consist of:  *   A status showing whether the alarm is ACTIVE, ACKNOWLEDGED or CLEARED. *   A time stamp to indicate when the alarm was last updated. *   The severity of the alarm: CRITICAL, MAJOR, MINOR or WARNING. *   A history of changes to the event in form of audit logs.  ### Alarm suppression  If the source device is in maintenance mode, the alarm is not created and not reported to the Cumulocity IoT event processing engine. When sending a POST request to create a new alarm and if the source device is in maintenance mode, the self link of the alarm will be:  &#x60;&#x60;&#x60;json \&quot;self\&quot;: \&quot;https://&lt;TENANT_DOMAIN&gt;/alarm/alarms/null\&quot; &#x60;&#x60;&#x60;  ### Alarm de-duplication  If an ACTIVE or ACKNOWLEDGED alarm with the same source and type exists, no new alarm is created. Instead, the existing alarm is updated by incrementing the &#x60;count&#x60; property; the &#x60;time&#x60; property is also updated. Any other changes are ignored, and the alarm history is not updated. Alarms with status CLEARED are not de-duplicated. The first occurrence of the alarm is recorded in the &#x60;firstOccurrenceTime&#x60; property.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_ADMIN permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postAlarmCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>Alarm</returns>
        Alarm PostAlarmCollectionResource(PostAlarmCollectionResourceRequest postAlarmCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0);

        /// <summary>
        /// Create an alarm
        /// </summary>
        /// <remarks>
        /// An alarm must be associated with a source (managed object) identified by ID.&lt;br&gt; In general, each alarm may consist of:  *   A status showing whether the alarm is ACTIVE, ACKNOWLEDGED or CLEARED. *   A time stamp to indicate when the alarm was last updated. *   The severity of the alarm: CRITICAL, MAJOR, MINOR or WARNING. *   A history of changes to the event in form of audit logs.  ### Alarm suppression  If the source device is in maintenance mode, the alarm is not created and not reported to the Cumulocity IoT event processing engine. When sending a POST request to create a new alarm and if the source device is in maintenance mode, the self link of the alarm will be:  &#x60;&#x60;&#x60;json \&quot;self\&quot;: \&quot;https://&lt;TENANT_DOMAIN&gt;/alarm/alarms/null\&quot; &#x60;&#x60;&#x60;  ### Alarm de-duplication  If an ACTIVE or ACKNOWLEDGED alarm with the same source and type exists, no new alarm is created. Instead, the existing alarm is updated by incrementing the &#x60;count&#x60; property; the &#x60;time&#x60; property is also updated. Any other changes are ignored, and the alarm history is not updated. Alarms with status CLEARED are not de-duplicated. The first occurrence of the alarm is recorded in the &#x60;firstOccurrenceTime&#x60; property.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_ADMIN permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postAlarmCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Alarm</returns>
        ApiResponse<Alarm> PostAlarmCollectionResourceWithHttpInfo(PostAlarmCollectionResourceRequest postAlarmCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0);
        /// <summary>
        /// Update alarm collections
        /// </summary>
        /// <remarks>
        /// Update alarm collections specified by query parameters. At least one query parameter is required to avoid accidentally updating all existing alarms.&lt;br&gt; Currently, only the status of alarms can be modified.  &gt; **&amp;#9432; Info:** Since this operation can take considerable time, the request returns after maximum 0.5 seconds of processing, and the update operation continues as a background process in the platform.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="putAlarmCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        void PutAlarmCollectionResource(PutAlarmCollectionResourceRequest putAlarmCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0);

        /// <summary>
        /// Update alarm collections
        /// </summary>
        /// <remarks>
        /// Update alarm collections specified by query parameters. At least one query parameter is required to avoid accidentally updating all existing alarms.&lt;br&gt; Currently, only the status of alarms can be modified.  &gt; **&amp;#9432; Info:** Since this operation can take considerable time, the request returns after maximum 0.5 seconds of processing, and the update operation continues as a background process in the platform.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="putAlarmCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PutAlarmCollectionResourceWithHttpInfo(PutAlarmCollectionResourceRequest putAlarmCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0);
        /// <summary>
        /// Update a specific alarm
        /// </summary>
        /// <remarks>
        /// Update a specific alarm by a given ID. Only text, status, severity and custom properties can be modified. A request will be rejected when non-modifiable properties are provided in the request body.  &gt; **&amp;#9432; Info:** Changes to alarms will generate a new audit record. The audit record will include the username and application that triggered the update, if applicable. If the update operation doesn’t change anything (that is, the request body contains data that is identical to the already present in the database), there will be no audit record added and no notifications will be sent.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_ADMIN permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the alarm.</param>
        /// <param name="putAlarmResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>Alarm</returns>
        Alarm PutAlarmResource(string id, PutAlarmResourceRequest putAlarmResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0);

        /// <summary>
        /// Update a specific alarm
        /// </summary>
        /// <remarks>
        /// Update a specific alarm by a given ID. Only text, status, severity and custom properties can be modified. A request will be rejected when non-modifiable properties are provided in the request body.  &gt; **&amp;#9432; Info:** Changes to alarms will generate a new audit record. The audit record will include the username and application that triggered the update, if applicable. If the update operation doesn’t change anything (that is, the request body contains data that is identical to the already present in the database), there will be no audit record added and no notifications will be sent.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_ADMIN permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the alarm.</param>
        /// <param name="putAlarmResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Alarm</returns>
        ApiResponse<Alarm> PutAlarmResourceWithHttpInfo(string id, PutAlarmResourceRequest putAlarmResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0);
        #endregion Synchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IAlarmsApiAsync : IApiAccessor
    {
        #region Asynchronous Operations
        /// <summary>
        /// Remove alarm collections
        /// </summary>
        /// <remarks>
        /// Remove alarm collections specified by query parameters.  &gt; **⚠️ Important:** Note that it is possible to call this endpoint without providing any parameter - it will result in deleting all alarms and it is not recommended. &gt; Also note that DELETE requests are not synchronous. The response could be returned before the delete request has been completed.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DeleteAlarmCollectionResourceAsync(string? xCumulocityProcessingMode = default(string?), DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Remove alarm collections
        /// </summary>
        /// <remarks>
        /// Remove alarm collections specified by query parameters.  &gt; **⚠️ Important:** Note that it is possible to call this endpoint without providing any parameter - it will result in deleting all alarms and it is not recommended. &gt; Also note that DELETE requests are not synchronous. The response could be returned before the delete request has been completed.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DeleteAlarmCollectionResourceWithHttpInfoAsync(string? xCumulocityProcessingMode = default(string?), DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Retrieve the total number of alarms
        /// </summary>
        /// <remarks>
        /// Count the total number of active alarms on your tenant.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; The role ROLE_ALARM_READ is not required, but if a user has this role, all the alarms on the tenant are counted. Otherwise, inventory role permissions are used to count the alarms and the limit is 100. &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of int</returns>
        System.Threading.Tasks.Task<int> GetAlarmCollectionCountResourceAsync(DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Retrieve the total number of alarms
        /// </summary>
        /// <remarks>
        /// Count the total number of active alarms on your tenant.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; The role ROLE_ALARM_READ is not required, but if a user has this role, all the alarms on the tenant are counted. Otherwise, inventory role permissions are used to count the alarms and the limit is 100. &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (int)</returns>
        System.Threading.Tasks.Task<ApiResponse<int>> GetAlarmCollectionCountResourceWithHttpInfoAsync(DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Retrieve all alarms
        /// </summary>
        /// <remarks>
        /// Retrieve all alarms on your tenant, or a specific subset based on queries. The results are sorted by the newest alarms first.  #### Query parameters  The query parameter &#x60;withTotalPages&#x60; only works when the user has the ROLE_ALARM_READ role, otherwise it is ignored.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; The role ROLE_ALARM_READ is not required, but if a user has this role, all the alarms on the tenant are returned. If a user has access to alarms through inventory roles, only those alarms are returned. &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="lastUpdatedFrom">Start date or date and time of the last update made. (optional)</param>
        /// <param name="lastUpdatedTo">End date or date and time of the last update made. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of AlarmCollection</returns>
        System.Threading.Tasks.Task<AlarmCollection> GetAlarmCollectionResourceAsync(DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), DateTime? lastUpdatedFrom = default(DateTime?), DateTime? lastUpdatedTo = default(DateTime?), int? pageSize = default(int?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Retrieve all alarms
        /// </summary>
        /// <remarks>
        /// Retrieve all alarms on your tenant, or a specific subset based on queries. The results are sorted by the newest alarms first.  #### Query parameters  The query parameter &#x60;withTotalPages&#x60; only works when the user has the ROLE_ALARM_READ role, otherwise it is ignored.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; The role ROLE_ALARM_READ is not required, but if a user has this role, all the alarms on the tenant are returned. If a user has access to alarms through inventory roles, only those alarms are returned. &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="lastUpdatedFrom">Start date or date and time of the last update made. (optional)</param>
        /// <param name="lastUpdatedTo">End date or date and time of the last update made. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (AlarmCollection)</returns>
        System.Threading.Tasks.Task<ApiResponse<AlarmCollection>> GetAlarmCollectionResourceWithHttpInfoAsync(DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), DateTime? lastUpdatedFrom = default(DateTime?), DateTime? lastUpdatedTo = default(DateTime?), int? pageSize = default(int?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Retrieve a specific alarm
        /// </summary>
        /// <remarks>
        /// Retrieve a specific alarm by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_READ permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the alarm.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Alarm</returns>
        System.Threading.Tasks.Task<Alarm> GetAlarmResourceAsync(string id, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Retrieve a specific alarm
        /// </summary>
        /// <remarks>
        /// Retrieve a specific alarm by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_READ permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the alarm.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Alarm)</returns>
        System.Threading.Tasks.Task<ApiResponse<Alarm>> GetAlarmResourceWithHttpInfoAsync(string id, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Create an alarm
        /// </summary>
        /// <remarks>
        /// An alarm must be associated with a source (managed object) identified by ID.&lt;br&gt; In general, each alarm may consist of:  *   A status showing whether the alarm is ACTIVE, ACKNOWLEDGED or CLEARED. *   A time stamp to indicate when the alarm was last updated. *   The severity of the alarm: CRITICAL, MAJOR, MINOR or WARNING. *   A history of changes to the event in form of audit logs.  ### Alarm suppression  If the source device is in maintenance mode, the alarm is not created and not reported to the Cumulocity IoT event processing engine. When sending a POST request to create a new alarm and if the source device is in maintenance mode, the self link of the alarm will be:  &#x60;&#x60;&#x60;json \&quot;self\&quot;: \&quot;https://&lt;TENANT_DOMAIN&gt;/alarm/alarms/null\&quot; &#x60;&#x60;&#x60;  ### Alarm de-duplication  If an ACTIVE or ACKNOWLEDGED alarm with the same source and type exists, no new alarm is created. Instead, the existing alarm is updated by incrementing the &#x60;count&#x60; property; the &#x60;time&#x60; property is also updated. Any other changes are ignored, and the alarm history is not updated. Alarms with status CLEARED are not de-duplicated. The first occurrence of the alarm is recorded in the &#x60;firstOccurrenceTime&#x60; property.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_ADMIN permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postAlarmCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Alarm</returns>
        System.Threading.Tasks.Task<Alarm> PostAlarmCollectionResourceAsync(PostAlarmCollectionResourceRequest postAlarmCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Create an alarm
        /// </summary>
        /// <remarks>
        /// An alarm must be associated with a source (managed object) identified by ID.&lt;br&gt; In general, each alarm may consist of:  *   A status showing whether the alarm is ACTIVE, ACKNOWLEDGED or CLEARED. *   A time stamp to indicate when the alarm was last updated. *   The severity of the alarm: CRITICAL, MAJOR, MINOR or WARNING. *   A history of changes to the event in form of audit logs.  ### Alarm suppression  If the source device is in maintenance mode, the alarm is not created and not reported to the Cumulocity IoT event processing engine. When sending a POST request to create a new alarm and if the source device is in maintenance mode, the self link of the alarm will be:  &#x60;&#x60;&#x60;json \&quot;self\&quot;: \&quot;https://&lt;TENANT_DOMAIN&gt;/alarm/alarms/null\&quot; &#x60;&#x60;&#x60;  ### Alarm de-duplication  If an ACTIVE or ACKNOWLEDGED alarm with the same source and type exists, no new alarm is created. Instead, the existing alarm is updated by incrementing the &#x60;count&#x60; property; the &#x60;time&#x60; property is also updated. Any other changes are ignored, and the alarm history is not updated. Alarms with status CLEARED are not de-duplicated. The first occurrence of the alarm is recorded in the &#x60;firstOccurrenceTime&#x60; property.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_ADMIN permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postAlarmCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Alarm)</returns>
        System.Threading.Tasks.Task<ApiResponse<Alarm>> PostAlarmCollectionResourceWithHttpInfoAsync(PostAlarmCollectionResourceRequest postAlarmCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Update alarm collections
        /// </summary>
        /// <remarks>
        /// Update alarm collections specified by query parameters. At least one query parameter is required to avoid accidentally updating all existing alarms.&lt;br&gt; Currently, only the status of alarms can be modified.  &gt; **&amp;#9432; Info:** Since this operation can take considerable time, the request returns after maximum 0.5 seconds of processing, and the update operation continues as a background process in the platform.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="putAlarmCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task PutAlarmCollectionResourceAsync(PutAlarmCollectionResourceRequest putAlarmCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Update alarm collections
        /// </summary>
        /// <remarks>
        /// Update alarm collections specified by query parameters. At least one query parameter is required to avoid accidentally updating all existing alarms.&lt;br&gt; Currently, only the status of alarms can be modified.  &gt; **&amp;#9432; Info:** Since this operation can take considerable time, the request returns after maximum 0.5 seconds of processing, and the update operation continues as a background process in the platform.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="putAlarmCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PutAlarmCollectionResourceWithHttpInfoAsync(PutAlarmCollectionResourceRequest putAlarmCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Update a specific alarm
        /// </summary>
        /// <remarks>
        /// Update a specific alarm by a given ID. Only text, status, severity and custom properties can be modified. A request will be rejected when non-modifiable properties are provided in the request body.  &gt; **&amp;#9432; Info:** Changes to alarms will generate a new audit record. The audit record will include the username and application that triggered the update, if applicable. If the update operation doesn’t change anything (that is, the request body contains data that is identical to the already present in the database), there will be no audit record added and no notifications will be sent.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_ADMIN permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the alarm.</param>
        /// <param name="putAlarmResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Alarm</returns>
        System.Threading.Tasks.Task<Alarm> PutAlarmResourceAsync(string id, PutAlarmResourceRequest putAlarmResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Update a specific alarm
        /// </summary>
        /// <remarks>
        /// Update a specific alarm by a given ID. Only text, status, severity and custom properties can be modified. A request will be rejected when non-modifiable properties are provided in the request body.  &gt; **&amp;#9432; Info:** Changes to alarms will generate a new audit record. The audit record will include the username and application that triggered the update, if applicable. If the update operation doesn’t change anything (that is, the request body contains data that is identical to the already present in the database), there will be no audit record added and no notifications will be sent.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_ADMIN permission on the source &lt;/section&gt; 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the alarm.</param>
        /// <param name="putAlarmResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Alarm)</returns>
        System.Threading.Tasks.Task<ApiResponse<Alarm>> PutAlarmResourceWithHttpInfoAsync(string id, PutAlarmResourceRequest putAlarmResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IAlarmsApi : IAlarmsApiSync, IAlarmsApiAsync
    {

    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class AlarmsApi : IAlarmsApi
    {
        private kern.services.CumulocityClient.Client.ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlarmsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public AlarmsApi() : this((string)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlarmsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public AlarmsApi(string basePath)
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
        /// Initializes a new instance of the <see cref="AlarmsApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public AlarmsApi(kern.services.CumulocityClient.Client.Configuration configuration)
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
        /// Initializes a new instance of the <see cref="AlarmsApi"/> class
        /// using a Configuration object and client instance.
        /// </summary>
        /// <param name="client">The client interface for synchronous API access.</param>
        /// <param name="asyncClient">The client interface for asynchronous API access.</param>
        /// <param name="configuration">The configuration object.</param>
        public AlarmsApi(kern.services.CumulocityClient.Client.ISynchronousClient client, kern.services.CumulocityClient.Client.IAsynchronousClient asyncClient, kern.services.CumulocityClient.Client.IReadableConfiguration configuration)
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
        /// Remove alarm collections Remove alarm collections specified by query parameters.  &gt; **⚠️ Important:** Note that it is possible to call this endpoint without providing any parameter - it will result in deleting all alarms and it is not recommended. &gt; Also note that DELETE requests are not synchronous. The response could be returned before the delete request has been completed.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        public void DeleteAlarmCollectionResource(string? xCumulocityProcessingMode = default(string?), DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0)
        {
            DeleteAlarmCollectionResourceWithHttpInfo(xCumulocityProcessingMode, createdFrom, createdTo, dateFrom, dateTo, resolved, severity, source, status, type, withSourceAssets, withSourceDevices);
        }

        /// <summary>
        /// Remove alarm collections Remove alarm collections specified by query parameters.  &gt; **⚠️ Important:** Note that it is possible to call this endpoint without providing any parameter - it will result in deleting all alarms and it is not recommended. &gt; Also note that DELETE requests are not synchronous. The response could be returned before the delete request has been completed.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<Object> DeleteAlarmCollectionResourceWithHttpInfo(string? xCumulocityProcessingMode = default(string?), DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0)
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

            if (createdFrom != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "createdFrom", createdFrom));
            }
            if (createdTo != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "createdTo", createdTo));
            }
            if (dateFrom != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateFrom", dateFrom));
            }
            if (dateTo != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateTo", dateTo));
            }
            if (resolved != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "resolved", resolved));
            }
            if (severity != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "severity", severity));
            }
            if (source != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "source", source));
            }
            if (status != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "status", status));
            }
            if (type != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "type", type));
            }
            if (withSourceAssets != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withSourceAssets", withSourceAssets));
            }
            if (withSourceDevices != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withSourceDevices", withSourceDevices));
            }
            if (xCumulocityProcessingMode != null)
            {
                localVarRequestOptions.HeaderParameters.Add("X-Cumulocity-Processing-Mode", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(xCumulocityProcessingMode)); // header parameter
            }

            localVarRequestOptions.Operation = "AlarmsApi.DeleteAlarmCollectionResource";
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
            var localVarResponse = this.Client.Delete<Object>("/alarm/alarms", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteAlarmCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Remove alarm collections Remove alarm collections specified by query parameters.  &gt; **⚠️ Important:** Note that it is possible to call this endpoint without providing any parameter - it will result in deleting all alarms and it is not recommended. &gt; Also note that DELETE requests are not synchronous. The response could be returned before the delete request has been completed.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DeleteAlarmCollectionResourceAsync(string? xCumulocityProcessingMode = default(string?), DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            await DeleteAlarmCollectionResourceWithHttpInfoAsync(xCumulocityProcessingMode, createdFrom, createdTo, dateFrom, dateTo, resolved, severity, source, status, type, withSourceAssets, withSourceDevices, operationIndex, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Remove alarm collections Remove alarm collections specified by query parameters.  &gt; **⚠️ Important:** Note that it is possible to call this endpoint without providing any parameter - it will result in deleting all alarms and it is not recommended. &gt; Also note that DELETE requests are not synchronous. The response could be returned before the delete request has been completed.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<Object>> DeleteAlarmCollectionResourceWithHttpInfoAsync(string? xCumulocityProcessingMode = default(string?), DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
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

            if (createdFrom != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "createdFrom", createdFrom));
            }
            if (createdTo != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "createdTo", createdTo));
            }
            if (dateFrom != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateFrom", dateFrom));
            }
            if (dateTo != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateTo", dateTo));
            }
            if (resolved != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "resolved", resolved));
            }
            if (severity != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "severity", severity));
            }
            if (source != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "source", source));
            }
            if (status != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "status", status));
            }
            if (type != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "type", type));
            }
            if (withSourceAssets != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withSourceAssets", withSourceAssets));
            }
            if (withSourceDevices != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withSourceDevices", withSourceDevices));
            }
            if (xCumulocityProcessingMode != null)
            {
                localVarRequestOptions.HeaderParameters.Add("X-Cumulocity-Processing-Mode", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(xCumulocityProcessingMode)); // header parameter
            }

            localVarRequestOptions.Operation = "AlarmsApi.DeleteAlarmCollectionResource";
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
            var localVarResponse = await this.AsynchronousClient.DeleteAsync<Object>("/alarm/alarms", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("DeleteAlarmCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve the total number of alarms Count the total number of active alarms on your tenant.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; The role ROLE_ALARM_READ is not required, but if a user has this role, all the alarms on the tenant are counted. Otherwise, inventory role permissions are used to count the alarms and the limit is 100. &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>int</returns>
        public int GetAlarmCollectionCountResource(DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<int> localVarResponse = GetAlarmCollectionCountResourceWithHttpInfo(dateFrom, dateTo, resolved, severity, source, status, type, withSourceAssets, withSourceDevices);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve the total number of alarms Count the total number of active alarms on your tenant.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; The role ROLE_ALARM_READ is not required, but if a user has this role, all the alarms on the tenant are counted. Otherwise, inventory role permissions are used to count the alarms and the limit is 100. &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of int</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<int> GetAlarmCollectionCountResourceWithHttpInfo(DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "text/plain, application/json",
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
            if (resolved != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "resolved", resolved));
            }
            if (severity != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "severity", severity));
            }
            if (source != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "source", source));
            }
            if (status != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "status", status));
            }
            if (type != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "type", type));
            }
            if (withSourceAssets != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withSourceAssets", withSourceAssets));
            }
            if (withSourceDevices != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withSourceDevices", withSourceDevices));
            }

            localVarRequestOptions.Operation = "AlarmsApi.GetAlarmCollectionCountResource";
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
            var localVarResponse = this.Client.Get<int>("/alarm/alarms/count", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetAlarmCollectionCountResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve the total number of alarms Count the total number of active alarms on your tenant.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; The role ROLE_ALARM_READ is not required, but if a user has this role, all the alarms on the tenant are counted. Otherwise, inventory role permissions are used to count the alarms and the limit is 100. &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of int</returns>
        public async System.Threading.Tasks.Task<int> GetAlarmCollectionCountResourceAsync(DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<int> localVarResponse = await GetAlarmCollectionCountResourceWithHttpInfoAsync(dateFrom, dateTo, resolved, severity, source, status, type, withSourceAssets, withSourceDevices, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve the total number of alarms Count the total number of active alarms on your tenant.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; The role ROLE_ALARM_READ is not required, but if a user has this role, all the alarms on the tenant are counted. Otherwise, inventory role permissions are used to count the alarms and the limit is 100. &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (int)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<int>> GetAlarmCollectionCountResourceWithHttpInfoAsync(DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "text/plain, application/json",
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
            if (resolved != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "resolved", resolved));
            }
            if (severity != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "severity", severity));
            }
            if (source != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "source", source));
            }
            if (status != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "status", status));
            }
            if (type != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "type", type));
            }
            if (withSourceAssets != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withSourceAssets", withSourceAssets));
            }
            if (withSourceDevices != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withSourceDevices", withSourceDevices));
            }

            localVarRequestOptions.Operation = "AlarmsApi.GetAlarmCollectionCountResource";
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
            var localVarResponse = await this.AsynchronousClient.GetAsync<int>("/alarm/alarms/count", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetAlarmCollectionCountResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve all alarms Retrieve all alarms on your tenant, or a specific subset based on queries. The results are sorted by the newest alarms first.  #### Query parameters  The query parameter &#x60;withTotalPages&#x60; only works when the user has the ROLE_ALARM_READ role, otherwise it is ignored.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; The role ROLE_ALARM_READ is not required, but if a user has this role, all the alarms on the tenant are returned. If a user has access to alarms through inventory roles, only those alarms are returned. &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="lastUpdatedFrom">Start date or date and time of the last update made. (optional)</param>
        /// <param name="lastUpdatedTo">End date or date and time of the last update made. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>AlarmCollection</returns>
        public AlarmCollection GetAlarmCollectionResource(DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), DateTime? lastUpdatedFrom = default(DateTime?), DateTime? lastUpdatedTo = default(DateTime?), int? pageSize = default(int?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<AlarmCollection> localVarResponse = GetAlarmCollectionResourceWithHttpInfo(createdFrom, createdTo, currentPage, dateFrom, dateTo, lastUpdatedFrom, lastUpdatedTo, pageSize, resolved, severity, source, status, type, withSourceAssets, withSourceDevices, withTotalElements, withTotalPages);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve all alarms Retrieve all alarms on your tenant, or a specific subset based on queries. The results are sorted by the newest alarms first.  #### Query parameters  The query parameter &#x60;withTotalPages&#x60; only works when the user has the ROLE_ALARM_READ role, otherwise it is ignored.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; The role ROLE_ALARM_READ is not required, but if a user has this role, all the alarms on the tenant are returned. If a user has access to alarms through inventory roles, only those alarms are returned. &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="lastUpdatedFrom">Start date or date and time of the last update made. (optional)</param>
        /// <param name="lastUpdatedTo">End date or date and time of the last update made. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of AlarmCollection</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<AlarmCollection> GetAlarmCollectionResourceWithHttpInfo(DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), DateTime? lastUpdatedFrom = default(DateTime?), DateTime? lastUpdatedTo = default(DateTime?), int? pageSize = default(int?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.alarmcollection+json",
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

            if (createdFrom != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "createdFrom", createdFrom));
            }
            if (createdTo != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "createdTo", createdTo));
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
            if (lastUpdatedFrom != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "lastUpdatedFrom", lastUpdatedFrom));
            }
            if (lastUpdatedTo != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "lastUpdatedTo", lastUpdatedTo));
            }
            if (pageSize != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "pageSize", pageSize));
            }
            if (resolved != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "resolved", resolved));
            }
            if (severity != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "severity", severity));
            }
            if (source != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "source", source));
            }
            if (status != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "status", status));
            }
            if (type != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "type", type));
            }
            if (withSourceAssets != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withSourceAssets", withSourceAssets));
            }
            if (withSourceDevices != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withSourceDevices", withSourceDevices));
            }
            if (withTotalElements != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withTotalElements", withTotalElements));
            }
            if (withTotalPages != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withTotalPages", withTotalPages));
            }

            localVarRequestOptions.Operation = "AlarmsApi.GetAlarmCollectionResource";
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
            var localVarResponse = this.Client.Get<AlarmCollection>("/alarm/alarms", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetAlarmCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve all alarms Retrieve all alarms on your tenant, or a specific subset based on queries. The results are sorted by the newest alarms first.  #### Query parameters  The query parameter &#x60;withTotalPages&#x60; only works when the user has the ROLE_ALARM_READ role, otherwise it is ignored.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; The role ROLE_ALARM_READ is not required, but if a user has this role, all the alarms on the tenant are returned. If a user has access to alarms through inventory roles, only those alarms are returned. &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="lastUpdatedFrom">Start date or date and time of the last update made. (optional)</param>
        /// <param name="lastUpdatedTo">End date or date and time of the last update made. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of AlarmCollection</returns>
        public async System.Threading.Tasks.Task<AlarmCollection> GetAlarmCollectionResourceAsync(DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), DateTime? lastUpdatedFrom = default(DateTime?), DateTime? lastUpdatedTo = default(DateTime?), int? pageSize = default(int?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<AlarmCollection> localVarResponse = await GetAlarmCollectionResourceWithHttpInfoAsync(createdFrom, createdTo, currentPage, dateFrom, dateTo, lastUpdatedFrom, lastUpdatedTo, pageSize, resolved, severity, source, status, type, withSourceAssets, withSourceDevices, withTotalElements, withTotalPages, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve all alarms Retrieve all alarms on your tenant, or a specific subset based on queries. The results are sorted by the newest alarms first.  #### Query parameters  The query parameter &#x60;withTotalPages&#x60; only works when the user has the ROLE_ALARM_READ role, otherwise it is ignored.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; The role ROLE_ALARM_READ is not required, but if a user has this role, all the alarms on the tenant are returned. If a user has access to alarms through inventory roles, only those alarms are returned. &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="lastUpdatedFrom">Start date or date and time of the last update made. (optional)</param>
        /// <param name="lastUpdatedTo">End date or date and time of the last update made. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="type">The types of alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm types at once, comma-separate the values. Space characters in alarm types must be escaped.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (AlarmCollection)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<AlarmCollection>> GetAlarmCollectionResourceWithHttpInfoAsync(DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), int? currentPage = default(int?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), DateTime? lastUpdatedFrom = default(DateTime?), DateTime? lastUpdatedTo = default(DateTime?), int? pageSize = default(int?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), List<string>? type = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.alarmcollection+json",
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

            if (createdFrom != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "createdFrom", createdFrom));
            }
            if (createdTo != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "createdTo", createdTo));
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
            if (lastUpdatedFrom != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "lastUpdatedFrom", lastUpdatedFrom));
            }
            if (lastUpdatedTo != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "lastUpdatedTo", lastUpdatedTo));
            }
            if (pageSize != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "pageSize", pageSize));
            }
            if (resolved != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "resolved", resolved));
            }
            if (severity != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "severity", severity));
            }
            if (source != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "source", source));
            }
            if (status != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "status", status));
            }
            if (type != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "type", type));
            }
            if (withSourceAssets != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withSourceAssets", withSourceAssets));
            }
            if (withSourceDevices != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withSourceDevices", withSourceDevices));
            }
            if (withTotalElements != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withTotalElements", withTotalElements));
            }
            if (withTotalPages != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withTotalPages", withTotalPages));
            }

            localVarRequestOptions.Operation = "AlarmsApi.GetAlarmCollectionResource";
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
            var localVarResponse = await this.AsynchronousClient.GetAsync<AlarmCollection>("/alarm/alarms", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetAlarmCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve a specific alarm Retrieve a specific alarm by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_READ permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the alarm.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>Alarm</returns>
        public Alarm GetAlarmResource(string id, int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<Alarm> localVarResponse = GetAlarmResourceWithHttpInfo(id);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve a specific alarm Retrieve a specific alarm by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_READ permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the alarm.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Alarm</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<Alarm> GetAlarmResourceWithHttpInfo(string id, int operationIndex = 0)
        {
            // verify the required parameter 'id' is set
            if (id == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'id' when calling AlarmsApi->GetAlarmResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.alarm+json",
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

            localVarRequestOptions.Operation = "AlarmsApi.GetAlarmResource";
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
            var localVarResponse = this.Client.Get<Alarm>("/alarm/alarms/{id}", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetAlarmResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Retrieve a specific alarm Retrieve a specific alarm by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_READ permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the alarm.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Alarm</returns>
        public async System.Threading.Tasks.Task<Alarm> GetAlarmResourceAsync(string id, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<Alarm> localVarResponse = await GetAlarmResourceWithHttpInfoAsync(id, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve a specific alarm Retrieve a specific alarm by a given ID.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_READ &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_READ permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the alarm.</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Alarm)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<Alarm>> GetAlarmResourceWithHttpInfoAsync(string id, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'id' when calling AlarmsApi->GetAlarmResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.alarm+json",
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

            localVarRequestOptions.Operation = "AlarmsApi.GetAlarmResource";
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
            var localVarResponse = await this.AsynchronousClient.GetAsync<Alarm>("/alarm/alarms/{id}", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetAlarmResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Create an alarm An alarm must be associated with a source (managed object) identified by ID.&lt;br&gt; In general, each alarm may consist of:  *   A status showing whether the alarm is ACTIVE, ACKNOWLEDGED or CLEARED. *   A time stamp to indicate when the alarm was last updated. *   The severity of the alarm: CRITICAL, MAJOR, MINOR or WARNING. *   A history of changes to the event in form of audit logs.  ### Alarm suppression  If the source device is in maintenance mode, the alarm is not created and not reported to the Cumulocity IoT event processing engine. When sending a POST request to create a new alarm and if the source device is in maintenance mode, the self link of the alarm will be:  &#x60;&#x60;&#x60;json \&quot;self\&quot;: \&quot;https://&lt;TENANT_DOMAIN&gt;/alarm/alarms/null\&quot; &#x60;&#x60;&#x60;  ### Alarm de-duplication  If an ACTIVE or ACKNOWLEDGED alarm with the same source and type exists, no new alarm is created. Instead, the existing alarm is updated by incrementing the &#x60;count&#x60; property; the &#x60;time&#x60; property is also updated. Any other changes are ignored, and the alarm history is not updated. Alarms with status CLEARED are not de-duplicated. The first occurrence of the alarm is recorded in the &#x60;firstOccurrenceTime&#x60; property.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_ADMIN permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postAlarmCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>Alarm</returns>
        public Alarm PostAlarmCollectionResource(PostAlarmCollectionResourceRequest postAlarmCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<Alarm> localVarResponse = PostAlarmCollectionResourceWithHttpInfo(postAlarmCollectionResourceRequest, accept, xCumulocityProcessingMode);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Create an alarm An alarm must be associated with a source (managed object) identified by ID.&lt;br&gt; In general, each alarm may consist of:  *   A status showing whether the alarm is ACTIVE, ACKNOWLEDGED or CLEARED. *   A time stamp to indicate when the alarm was last updated. *   The severity of the alarm: CRITICAL, MAJOR, MINOR or WARNING. *   A history of changes to the event in form of audit logs.  ### Alarm suppression  If the source device is in maintenance mode, the alarm is not created and not reported to the Cumulocity IoT event processing engine. When sending a POST request to create a new alarm and if the source device is in maintenance mode, the self link of the alarm will be:  &#x60;&#x60;&#x60;json \&quot;self\&quot;: \&quot;https://&lt;TENANT_DOMAIN&gt;/alarm/alarms/null\&quot; &#x60;&#x60;&#x60;  ### Alarm de-duplication  If an ACTIVE or ACKNOWLEDGED alarm with the same source and type exists, no new alarm is created. Instead, the existing alarm is updated by incrementing the &#x60;count&#x60; property; the &#x60;time&#x60; property is also updated. Any other changes are ignored, and the alarm history is not updated. Alarms with status CLEARED are not de-duplicated. The first occurrence of the alarm is recorded in the &#x60;firstOccurrenceTime&#x60; property.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_ADMIN permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postAlarmCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Alarm</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<Alarm> PostAlarmCollectionResourceWithHttpInfo(PostAlarmCollectionResourceRequest postAlarmCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0)
        {
            // verify the required parameter 'postAlarmCollectionResourceRequest' is set
            if (postAlarmCollectionResourceRequest == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'postAlarmCollectionResourceRequest' when calling AlarmsApi->PostAlarmCollectionResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/vnd.com.nsn.cumulocity.alarm+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.alarm+json",
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
            localVarRequestOptions.Data = postAlarmCollectionResourceRequest;

            localVarRequestOptions.Operation = "AlarmsApi.PostAlarmCollectionResource";
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
            var localVarResponse = this.Client.Post<Alarm>("/alarm/alarms", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PostAlarmCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Create an alarm An alarm must be associated with a source (managed object) identified by ID.&lt;br&gt; In general, each alarm may consist of:  *   A status showing whether the alarm is ACTIVE, ACKNOWLEDGED or CLEARED. *   A time stamp to indicate when the alarm was last updated. *   The severity of the alarm: CRITICAL, MAJOR, MINOR or WARNING. *   A history of changes to the event in form of audit logs.  ### Alarm suppression  If the source device is in maintenance mode, the alarm is not created and not reported to the Cumulocity IoT event processing engine. When sending a POST request to create a new alarm and if the source device is in maintenance mode, the self link of the alarm will be:  &#x60;&#x60;&#x60;json \&quot;self\&quot;: \&quot;https://&lt;TENANT_DOMAIN&gt;/alarm/alarms/null\&quot; &#x60;&#x60;&#x60;  ### Alarm de-duplication  If an ACTIVE or ACKNOWLEDGED alarm with the same source and type exists, no new alarm is created. Instead, the existing alarm is updated by incrementing the &#x60;count&#x60; property; the &#x60;time&#x60; property is also updated. Any other changes are ignored, and the alarm history is not updated. Alarms with status CLEARED are not de-duplicated. The first occurrence of the alarm is recorded in the &#x60;firstOccurrenceTime&#x60; property.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_ADMIN permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postAlarmCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Alarm</returns>
        public async System.Threading.Tasks.Task<Alarm> PostAlarmCollectionResourceAsync(PostAlarmCollectionResourceRequest postAlarmCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<Alarm> localVarResponse = await PostAlarmCollectionResourceWithHttpInfoAsync(postAlarmCollectionResourceRequest, accept, xCumulocityProcessingMode, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Create an alarm An alarm must be associated with a source (managed object) identified by ID.&lt;br&gt; In general, each alarm may consist of:  *   A status showing whether the alarm is ACTIVE, ACKNOWLEDGED or CLEARED. *   A time stamp to indicate when the alarm was last updated. *   The severity of the alarm: CRITICAL, MAJOR, MINOR or WARNING. *   A history of changes to the event in form of audit logs.  ### Alarm suppression  If the source device is in maintenance mode, the alarm is not created and not reported to the Cumulocity IoT event processing engine. When sending a POST request to create a new alarm and if the source device is in maintenance mode, the self link of the alarm will be:  &#x60;&#x60;&#x60;json \&quot;self\&quot;: \&quot;https://&lt;TENANT_DOMAIN&gt;/alarm/alarms/null\&quot; &#x60;&#x60;&#x60;  ### Alarm de-duplication  If an ACTIVE or ACKNOWLEDGED alarm with the same source and type exists, no new alarm is created. Instead, the existing alarm is updated by incrementing the &#x60;count&#x60; property; the &#x60;time&#x60; property is also updated. Any other changes are ignored, and the alarm history is not updated. Alarms with status CLEARED are not de-duplicated. The first occurrence of the alarm is recorded in the &#x60;firstOccurrenceTime&#x60; property.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_ADMIN permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postAlarmCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Alarm)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<Alarm>> PostAlarmCollectionResourceWithHttpInfoAsync(PostAlarmCollectionResourceRequest postAlarmCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'postAlarmCollectionResourceRequest' is set
            if (postAlarmCollectionResourceRequest == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'postAlarmCollectionResourceRequest' when calling AlarmsApi->PostAlarmCollectionResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/vnd.com.nsn.cumulocity.alarm+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.alarm+json",
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
            localVarRequestOptions.Data = postAlarmCollectionResourceRequest;

            localVarRequestOptions.Operation = "AlarmsApi.PostAlarmCollectionResource";
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
            var localVarResponse = await this.AsynchronousClient.PostAsync<Alarm>("/alarm/alarms", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PostAlarmCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Update alarm collections Update alarm collections specified by query parameters. At least one query parameter is required to avoid accidentally updating all existing alarms.&lt;br&gt; Currently, only the status of alarms can be modified.  &gt; **&amp;#9432; Info:** Since this operation can take considerable time, the request returns after maximum 0.5 seconds of processing, and the update operation continues as a background process in the platform.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="putAlarmCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns></returns>
        public void PutAlarmCollectionResource(PutAlarmCollectionResourceRequest putAlarmCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0)
        {
            PutAlarmCollectionResourceWithHttpInfo(putAlarmCollectionResourceRequest, accept, xCumulocityProcessingMode, createdFrom, createdTo, dateFrom, dateTo, resolved, severity, source, status, withSourceAssets, withSourceDevices);
        }

        /// <summary>
        /// Update alarm collections Update alarm collections specified by query parameters. At least one query parameter is required to avoid accidentally updating all existing alarms.&lt;br&gt; Currently, only the status of alarms can be modified.  &gt; **&amp;#9432; Info:** Since this operation can take considerable time, the request returns after maximum 0.5 seconds of processing, and the update operation continues as a background process in the platform.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="putAlarmCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<Object> PutAlarmCollectionResourceWithHttpInfo(PutAlarmCollectionResourceRequest putAlarmCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0)
        {
            // verify the required parameter 'putAlarmCollectionResourceRequest' is set
            if (putAlarmCollectionResourceRequest == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'putAlarmCollectionResourceRequest' when calling AlarmsApi->PutAlarmCollectionResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/vnd.com.nsn.cumulocity.alarm+json"
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

            if (createdFrom != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "createdFrom", createdFrom));
            }
            if (createdTo != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "createdTo", createdTo));
            }
            if (dateFrom != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateFrom", dateFrom));
            }
            if (dateTo != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateTo", dateTo));
            }
            if (resolved != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "resolved", resolved));
            }
            if (severity != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "severity", severity));
            }
            if (source != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "source", source));
            }
            if (status != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "status", status));
            }
            if (withSourceAssets != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withSourceAssets", withSourceAssets));
            }
            if (withSourceDevices != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withSourceDevices", withSourceDevices));
            }
            if (accept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(accept)); // header parameter
            }
            if (xCumulocityProcessingMode != null)
            {
                localVarRequestOptions.HeaderParameters.Add("X-Cumulocity-Processing-Mode", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(xCumulocityProcessingMode)); // header parameter
            }
            localVarRequestOptions.Data = putAlarmCollectionResourceRequest;

            localVarRequestOptions.Operation = "AlarmsApi.PutAlarmCollectionResource";
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
            var localVarResponse = this.Client.Put<Object>("/alarm/alarms", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PutAlarmCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Update alarm collections Update alarm collections specified by query parameters. At least one query parameter is required to avoid accidentally updating all existing alarms.&lt;br&gt; Currently, only the status of alarms can be modified.  &gt; **&amp;#9432; Info:** Since this operation can take considerable time, the request returns after maximum 0.5 seconds of processing, and the update operation continues as a background process in the platform.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="putAlarmCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task PutAlarmCollectionResourceAsync(PutAlarmCollectionResourceRequest putAlarmCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            await PutAlarmCollectionResourceWithHttpInfoAsync(putAlarmCollectionResourceRequest, accept, xCumulocityProcessingMode, createdFrom, createdTo, dateFrom, dateTo, resolved, severity, source, status, withSourceAssets, withSourceDevices, operationIndex, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Update alarm collections Update alarm collections specified by query parameters. At least one query parameter is required to avoid accidentally updating all existing alarms.&lt;br&gt; Currently, only the status of alarms can be modified.  &gt; **&amp;#9432; Info:** Since this operation can take considerable time, the request returns after maximum 0.5 seconds of processing, and the update operation continues as a background process in the platform.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="putAlarmCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="createdFrom">Start date or date and time of the alarm creation. (optional)</param>
        /// <param name="createdTo">End date or date and time of the alarm creation. (optional)</param>
        /// <param name="dateFrom">Start date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="dateTo">End date or date and time of the alarm occurrence. (optional)</param>
        /// <param name="resolved">When set to &#x60;true&#x60; only alarms with status CLEARED will be fetched, whereas &#x60;false&#x60; will fetch all alarms with status ACTIVE or ACKNOWLEDGED. (optional, default to false)</param>
        /// <param name="severity">The severity of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm severities at once, comma-separate the values.  (optional)</param>
        /// <param name="source">The managed object ID to which the alarm is associated. (optional)</param>
        /// <param name="status">The status of the alarm to search for. &gt;**&amp;#9432; Info:** If you query for multiple alarm statuses at once, comma-separate the values.  (optional)</param>
        /// <param name="withSourceAssets">When set to &#x60;true&#x60; also alarms for related source assets will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="withSourceDevices">When set to &#x60;true&#x60; also alarms for related source devices will be included in the request. When this parameter is provided a &#x60;source&#x60; must be specified. (optional, default to false)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<Object>> PutAlarmCollectionResourceWithHttpInfoAsync(PutAlarmCollectionResourceRequest putAlarmCollectionResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), DateTime? createdFrom = default(DateTime?), DateTime? createdTo = default(DateTime?), DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool? resolved = default(bool?), List<string>? severity = default(List<string>?), string? source = default(string?), List<string>? status = default(List<string>?), bool? withSourceAssets = default(bool?), bool? withSourceDevices = default(bool?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'putAlarmCollectionResourceRequest' is set
            if (putAlarmCollectionResourceRequest == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'putAlarmCollectionResourceRequest' when calling AlarmsApi->PutAlarmCollectionResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/vnd.com.nsn.cumulocity.alarm+json"
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

            if (createdFrom != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "createdFrom", createdFrom));
            }
            if (createdTo != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "createdTo", createdTo));
            }
            if (dateFrom != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateFrom", dateFrom));
            }
            if (dateTo != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "dateTo", dateTo));
            }
            if (resolved != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "resolved", resolved));
            }
            if (severity != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "severity", severity));
            }
            if (source != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "source", source));
            }
            if (status != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("csv", "status", status));
            }
            if (withSourceAssets != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withSourceAssets", withSourceAssets));
            }
            if (withSourceDevices != null)
            {
                localVarRequestOptions.QueryParameters.Add(kern.services.CumulocityClient.Client.ClientUtils.ParameterToMultiMap("", "withSourceDevices", withSourceDevices));
            }
            if (accept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(accept)); // header parameter
            }
            if (xCumulocityProcessingMode != null)
            {
                localVarRequestOptions.HeaderParameters.Add("X-Cumulocity-Processing-Mode", kern.services.CumulocityClient.Client.ClientUtils.ParameterToString(xCumulocityProcessingMode)); // header parameter
            }
            localVarRequestOptions.Data = putAlarmCollectionResourceRequest;

            localVarRequestOptions.Operation = "AlarmsApi.PutAlarmCollectionResource";
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
            var localVarResponse = await this.AsynchronousClient.PutAsync<Object>("/alarm/alarms", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PutAlarmCollectionResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Update a specific alarm Update a specific alarm by a given ID. Only text, status, severity and custom properties can be modified. A request will be rejected when non-modifiable properties are provided in the request body.  &gt; **&amp;#9432; Info:** Changes to alarms will generate a new audit record. The audit record will include the username and application that triggered the update, if applicable. If the update operation doesn’t change anything (that is, the request body contains data that is identical to the already present in the database), there will be no audit record added and no notifications will be sent.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_ADMIN permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the alarm.</param>
        /// <param name="putAlarmResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>Alarm</returns>
        public Alarm PutAlarmResource(string id, PutAlarmResourceRequest putAlarmResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0)
        {
            kern.services.CumulocityClient.Client.ApiResponse<Alarm> localVarResponse = PutAlarmResourceWithHttpInfo(id, putAlarmResourceRequest, accept, xCumulocityProcessingMode);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Update a specific alarm Update a specific alarm by a given ID. Only text, status, severity and custom properties can be modified. A request will be rejected when non-modifiable properties are provided in the request body.  &gt; **&amp;#9432; Info:** Changes to alarms will generate a new audit record. The audit record will include the username and application that triggered the update, if applicable. If the update operation doesn’t change anything (that is, the request body contains data that is identical to the already present in the database), there will be no audit record added and no notifications will be sent.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_ADMIN permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the alarm.</param>
        /// <param name="putAlarmResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of Alarm</returns>
        public kern.services.CumulocityClient.Client.ApiResponse<Alarm> PutAlarmResourceWithHttpInfo(string id, PutAlarmResourceRequest putAlarmResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0)
        {
            // verify the required parameter 'id' is set
            if (id == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'id' when calling AlarmsApi->PutAlarmResource");
            }

            // verify the required parameter 'putAlarmResourceRequest' is set
            if (putAlarmResourceRequest == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'putAlarmResourceRequest' when calling AlarmsApi->PutAlarmResource");
            }

            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/vnd.com.nsn.cumulocity.alarm+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.alarm+json",
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
            localVarRequestOptions.Data = putAlarmResourceRequest;

            localVarRequestOptions.Operation = "AlarmsApi.PutAlarmResource";
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
            var localVarResponse = this.Client.Put<Alarm>("/alarm/alarms/{id}", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PutAlarmResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Update a specific alarm Update a specific alarm by a given ID. Only text, status, severity and custom properties can be modified. A request will be rejected when non-modifiable properties are provided in the request body.  &gt; **&amp;#9432; Info:** Changes to alarms will generate a new audit record. The audit record will include the username and application that triggered the update, if applicable. If the update operation doesn’t change anything (that is, the request body contains data that is identical to the already present in the database), there will be no audit record added and no notifications will be sent.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_ADMIN permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the alarm.</param>
        /// <param name="putAlarmResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Alarm</returns>
        public async System.Threading.Tasks.Task<Alarm> PutAlarmResourceAsync(string id, PutAlarmResourceRequest putAlarmResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            kern.services.CumulocityClient.Client.ApiResponse<Alarm> localVarResponse = await PutAlarmResourceWithHttpInfoAsync(id, putAlarmResourceRequest, accept, xCumulocityProcessingMode, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Update a specific alarm Update a specific alarm by a given ID. Only text, status, severity and custom properties can be modified. A request will be rejected when non-modifiable properties are provided in the request body.  &gt; **&amp;#9432; Info:** Changes to alarms will generate a new audit record. The audit record will include the username and application that triggered the update, if applicable. If the update operation doesn’t change anything (that is, the request body contains data that is identical to the already present in the database), there will be no audit record added and no notifications will be sent.  &lt;section&gt;&lt;h5&gt;Required roles&lt;/h5&gt; ROLE_ALARM_ADMIN &lt;b&gt;OR&lt;/b&gt; owner of the source &lt;b&gt;OR&lt;/b&gt; ALARM_ADMIN permission on the source &lt;/section&gt; 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the alarm.</param>
        /// <param name="putAlarmResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Alarm)</returns>
        public async System.Threading.Tasks.Task<kern.services.CumulocityClient.Client.ApiResponse<Alarm>> PutAlarmResourceWithHttpInfoAsync(string id, PutAlarmResourceRequest putAlarmResourceRequest, string? accept = default(string?), string? xCumulocityProcessingMode = default(string?), int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'id' when calling AlarmsApi->PutAlarmResource");
            }

            // verify the required parameter 'putAlarmResourceRequest' is set
            if (putAlarmResourceRequest == null)
            {
                throw new kern.services.CumulocityClient.Client.ApiException(400, "Missing required parameter 'putAlarmResourceRequest' when calling AlarmsApi->PutAlarmResource");
            }


            kern.services.CumulocityClient.Client.RequestOptions localVarRequestOptions = new kern.services.CumulocityClient.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/vnd.com.nsn.cumulocity.alarm+json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/vnd.com.nsn.cumulocity.alarm+json",
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
            localVarRequestOptions.Data = putAlarmResourceRequest;

            localVarRequestOptions.Operation = "AlarmsApi.PutAlarmResource";
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
            var localVarResponse = await this.AsynchronousClient.PutAsync<Alarm>("/alarm/alarms/{id}", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("PutAlarmResource", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

    }
}
