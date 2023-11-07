/*
 * Cumulocity IoT
 *
 * # REST implementation  This section describes the aspects common to all REST-based interfaces of Cumulocity IoT. The interfaces are based on the [Hypertext Transfer Protocol 1.1](https://tools.ietf.org/html/rfc2616) using [HTTPS](http://en.wikipedia.org/wiki/HTTP_Secure).  ## HTTP usage  ### Application management  Cumulocity IoT uses a so-called \"application key\" to distinguish requests coming from devices and traffic from applications. If you write an application, pass the following header as part of all requests:  ```markup X-Cumulocity-Application-Key: <APPLICATION_KEY> ```  For example, if you registered your application in the Cumulocity IoT Administration application with the key \"myapp\", your requests should contain the header:  ```markup X-Cumulocity-Application-Key: myapp ```  This makes your application subscribable and billable. If you implement a device, do not pass the key.  > **&#9432; Info:** Make sure that you pass the key in **all** requests coming from an application. If you leave out the key, > the request will be considered as a device request, and the corresponding device will be marked as \"available\".  ### Limited HTTP clients  If you use an HTTP client that can only perform GET and POST methods in HTTP, you can emulate the other methods through an additional \"X-HTTP-METHOD\" header. Simply issue a POST request and add the header, specifying the actual REST method to be executed. For example, to emulate the \"PUT\" (modify) method, you can use:  ```http POST ... X-HTTP-METHOD: PUT ```  ### Processing mode  Every update request (PUT, POST, DELETE) executes with a so-called *processing mode*. The processing modes are as follows:  |Processing mode|Description| |- --|- --| |PERSISTENT (default)|All updates will be send both to the Cumulocity IoT database and to real-time processing.| |TRANSIENT|Updates will be sent only to real-time processing. As part of real-time processing, the user can decide case by case through scripts whether updates should be stored to the database or not.| |QUIESCENT|The QUIESCENT processing mode behaves like the PERSISTENT processing mode with the exception that no real-time notifications will be sent. Currently, the QUIESCENT processing mode is applicable for measurements, events and managed objects.| |CEP| With the CEP processing mode, requests will only be processed by CEP or Apama. Currently, the CEP processing mode is applicable for measurements and events only.|  To explicitly control the processing mode of an update request, you can use the \"X-Cumulocity-Processing-Mode\" header with a value of either \"PERSISTENT\", \"TRANSIENT\", \"QUIESCENT\" or \"CEP\":  ```markup X-Cumulocity-Processing-Mode: PERSISTENT ```  > **&#9432; Info:** Events are always delivered to CEP/Apama for all processing modes. This is independent from real-time notifications.  ### Authorization  All requests issued to Cumulocity IoT are subject to authorization. To determine the required permissions, see the \"Required role\" entries for the individual requests. To learn more about the different permissions and the concept of ownership in Cumulocity IoT, see [Security aspects > Managing roles and assigning permissions](https://cumulocity.com/guides/concepts/security/#managing-roles-and-assigning-permissions)\".  ### Media types  Each type of data is associated with an own media type. The general format of media types is:  ```markup application/vnd.com.nsn.cumulocity.<TYPE>+json;ver=<VERSION>;charset=UTF-8 ```  Each media type contains a parameter `ver` indicating the version of the type. At the time of writing, the latest version is \"0.9\". As an example, the media type for an error message in the current version is:  ```markup application/vnd.com.nsn.cumulocity.error+json;ver=0.9;charset=UTF-8 ```  Media types are used in HTTP \"Content-Type\" and \"Accept\" headers. If you specify an \"Accept\" header in a POST or PUT request, the response will contain the newly created or updated object. If you do not specify the header, the response body will be empty.  If a media type without the `ver` parameter is given, the oldest available version will be returned by the server. If the \"Accept\" header contains the same media type in multiple versions, the server will return a representation in the latest supported version.  Note that media type values should be treated as case insensitive.  ### Date format  Data exchanged with Cumulocity IoT in HTTP requests and responses is encoded in [JSON format](http://www.ietf.org/rfc/rfc4627.txt) and [UTF-8](http://en.wikipedia.org/wiki/UTF-8) character encoding. Timestamps and dates are accepted and emitted by Cumulocity IoT in [ISO 8601](http://www.w3.org/TR/NOTE-datetime) format:  ```markup Date: YYYY-MM-DD Time: hh:mm:ss±hh:mm Timestamp: YYYY-MM-DDThh:mm:ss±hh:mm ```  To avoid ambiguity, all times and timestamps must include timezone information. Please take into account that the plus character \"+\" must be encoded as \"%2B\".  ### Response Codes  Cumulocity IoT uses conventional HTTP response codes to indicate the success or failure of an API request. Codes in the `2xx` range indicate success. Codes in the `4xx` range indicate a user error. The response provides information on why the request failed (for example, a required parameter was omitted). Codes in the `5xx` range indicate an error with Cumulocity IoT's servers ([these are very rare](https://www.softwareag.cloud/site/sla/cumulocity-iot.html#availability)).  #### HTTP status code summary  |Code|Message|Description| |:- --:|:- --|:- --| |200|OK|Everything worked as expected.| |201|Created|A managed object was created.| |204|No content|An object was removed.| |400|Bad Request|The request was unacceptable, often due to missing a required parameter.| |401|Unauthorized|Authentication has failed, or credentials were required but not provided.| |403|Forbidden|The authenticated user doesn't have permissions to perform the request.| |404|Not Found|The requested resource doesn't exist.| |405|Method not allowed|The employed HTTP method cannot be used on this resource (for example, using PUT on a read-only resource).| |409|Conflict| The data is correct but it breaks some constraints (for example, application version limit is exceeded). | |422|Invalid data| Invalid data was sent on the request and/or a query could not be understood.                             | |422|Unprocessable Entity| The requested resource cannot be updated or mandatory fields are missing on the executed operation.      | |500<br>503|Server Errors| Something went wrong on Cumulocity IoT's end.                                                            |  ## REST usage  ### Interpretation of HTTP verbs  The semantics described in the [HTTP specification](http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html#sec9) are used:  * POST creates a new resource. In the response \"Location\" header, the URI of the newly created resource is returned. * GET retrieves a resource. * PUT updates an existing resource with the contents of the request. * DELETE removes a resource. The response will be \"204 No Content\".  If a PUT request only contains parts of a resource (also known as fragments), only those parts are updated. To remove such a part, use a PUT request with a null value for it:  ```json {   \"resourcePartName\": null } ```  > **&#9432; Info:** A PUT request cannot update sub-resources that are identified by a separate URI.  ### URI space and URI templates  Clients should not make assumptions on the layout of URIs used in requests, but construct URIs from previously returned URIs or URI templates. The [root interface](#tag/Platform-API) provides the entry point for clients.  URI templates contain placeholders in curly braces (for example, `{type}`), which must be filled by the client to produce a URI. As an example, see the following excerpt from the event API response:  ```json {   \"events\": {       \"self\": \"https://<TENANT_DOMAIN>/event\"   },   \"eventsForSourceAndType\": \"https://<TENANT_DOMAIN>/event/events?type={type}&source={source}\" } ```  The client must fill the `{type}` and `{source}` placeholders with the desired type and source devices of the events to be returned. The meaning of these placeholders is documented in the respective interface descriptions.  ### Interface structure  In general, Cumulocity IoT REST resources are modeled according to the following pattern:  * The starting point are API resources, which will provide access to the actual data through URIs and URI templates to collection resources. For example, the above event API resource provides the `events` URI and the `eventsForSourceAndType` URI to access collections of events. * Collection resources aggregate member resources and allow creating new member resources in the collection. For example, through the `events` collection resource, new events can be created. * Finally, individual resources can be edited.  #### Query result paging  Collection resources support paging of data to avoid passing huge data volumes in one block from client to server. GET requests to collections accept two query parameters:  * `currentPage` defines the slice of data to be returned, starting with 1. By default, the first page is returned. * `pageSize` indicates how many entries of the collection should be returned. By default, 5 entries are returned. The upper limit for one page is currently 2,000 documents. Any larger requested page size is trimmed to the upper limit. * `withTotalElements` will yield the total number of elements in the statistics object. This is only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). * `withTotalPages` will yield the total number of pages in the statistics object. This is only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)).  For convenience, collection resources provide `next` and `prev` links to retrieve the next and previous pages of the results. The following is an example response for managed object collections (the contents of the array `managedObjects` have been omitted):  ```json {   \"self\" : \"https://<TENANT_DOMAIN>/inventory/managedObjects?pageSize=5&currentPage=2\",   \"managedObjects\" : [...],   \"statistics\" : {     \"totalPages\" : 7,     \"pageSize\" : 5,     \"currentPage\" : 2,     \"totalElements\" : 34   },   \"prev\" : \"https://<TENANT_DOMAIN>/inventory/managedObjects?pageSize=5&currentPage=1\",   \"next\" : \"https://<TENANT_DOMAIN>/inventory/managedObjects?pageSize=5&currentPage=3\" } ```  The `totalPages` and `totalElements` properties can be expensive to compute, hence they are not returned by default for [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). To include any of them in the result, add the query parameters `withTotalPages=true` and/or `withTotalElements=true`.  > **&#9432; Info:** If inventory roles are applied to a user, a query by the user may return less than `pageSize` results even if there are more results in total.  #### Query result paging for users with restricted access  If a user does not have a global role for reading data from the API resource but rather has [inventory roles](https://cumulocity.com/guides/users-guide/administration/#inventory) for reading only particular documents, there are some differences in query result paging:  * In some circumstances the response may contain less than `pageSize` and `totalElements` elements though there is more data in the database accessible for the user. * In some circumstances `next` and `prev` links may appear in the response though there is no more data in the database accessible for the user. * The property `currentPage` of the response does not contain the page number but the offset of the next element not yet processed by the querying mechanism. * The query parameters `withTotalPages=true` and `withTotalElements=true` have no effect, and the value of the `totalPages` and `totalElements` properties is always null.  The above behavior results from the fact that the querying mechanism is iterating maximally over 10 * max(pageSize, 100) documents per request, and it stops even though the full page of data accessible for the user could not be collected. When the next page is requested the querying mechanism starts the iteration where it completed the previous time.  #### Query result by time interval  Use the following query parameters to obtain data for a specified time interval:  * `dateFrom` - Start date or date and time. * `dateTo` - End date or date and time.  Example formats:  ```markup dateTo=2019-04-20 dateTo=2019-04-20T08:30:00.000Z ```  Parameters are optional. Values provided with those parameters are inclusive.  > **⚠️ Important:** If your servers are not running in UTC (Coordinated Universal Time), any date passed without timezone will be handled as UTC, regardless of the server local timezone. This might lead to a difference regarding the date/time range included in the results.  ### Root interface  To discover the URIs to the various interfaces of Cumulocity IoT, it provides a \"root\" interface. This root interface aggregates all the underlying API resources. See the [Platform API](#tag/Platform-API) endpoint. For more information on the different API resources, consult the respective API sections.  ## Generic media types  ### Error  The error type provides further information on the reason of a failed request.  Content-Type: application/vnd.com.nsn.cumulocity.error+json  |Name|Type|Description| |- --|- --|- --| |error|string|Error type formatted as `<RESOURCE_TYPE>/<ERROR_NAME>`. For example, an object not found in the inventory is reported as `inventory/notFound`.| |info|string|URL to an error description on the Internet.| |message|string|Short text description of the error|  ### Paging statistics  Paging statistics for collection of resources.  Content-Type: application/vnd.com.nsn.cumulocity.pagingstatistics+json  |Name|Type|Description| |- --|- --|- --| |currentPage|integer|The current returned page within the full result set, starting at \"1\".| |pageSize|integer|Maximum number of records contained in this query.| |totalElements|integer|The total number of results (elements).| |totalPages|integer|The total number of paginated results (pages).|  > **&#9432; Info:** The `totalPages` and `totalElements` properties are not returned by default in the response. To include any of them, add the query parameters `withTotalPages=true` and/or `withTotalElements=true`. Be aware of [differences in query result paging for users with restricted access](#query-result-paging-for-users-with-restricted-access).  > **&#9432; Info:** To improve performance, the `totalPages` and `totalElements` statistics are cached for 10 seconds.  # Device management library  The device management library has moved. Visit the [device management library](https://cumulocity.com/guides/reference/device-management-library/#overview) in the *Reference guide*.  # Sensor library  The sensor library has moved. Visit the [sensor library](https://cumulocity.com/guides/reference/sensor-library/#overview) in the *Reference guide*.  # Login options  When you sign up for an account on the [Cumulocity IoT platform](https://cumulocity.com/), for example, by using a free trial, you will be provided with a dedicated URL address for your tenant. All requests to the platform must be authenticated employing your tenant ID, Cumulocity IoT user (c8yuser for short) and password. Cumulocity IoT offers the following forms of authentication:  * Basic authentication (Basic) * OAI-Secure authentication (OAI-Secure) * SSO with authentication code grant (SSO) * JSON Web Token authentication (JWT, deprecated)  You can check your login options with a GET call to the endpoint <kbd><a href=\"#tag/Login-options\">/tenant/loginOptions</a></kbd>. 
 *
 * The version of the OpenAPI document: Release 10.15.0
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using OpenAPIDateConverter = kern.services.CumulocityClient.Client.OpenAPIDateConverter;

namespace kern.services.CumulocityClient.Model
{
    /// <summary>
    /// Daily usage statistics.
    /// </summary>
    [DataContract(Name = "DailyUsageStatistics")]
    public partial class DailyUsageStatistics : IEquatable<DailyUsageStatistics>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DailyUsageStatistics" /> class.
        /// </summary>
        /// <param name="alarmsCreatedCount">Number of created alarms..</param>
        /// <param name="alarmsUpdatedCount">Number of updates made to the alarms..</param>
        /// <param name="day">Date of this usage statistics object..</param>
        /// <param name="deviceCount">Number of devices in the tenant identified by the fragment &#x60;c8y_IsDevice&#x60;. Updated only three times a day starting at 8:57, 16:57 and 23:57..</param>
        /// <param name="deviceEndpointCount">Number of devices which do not have child devices. Updated only three times a day starting at 8:57, 16:57 and 23:57..</param>
        /// <param name="deviceRequestCount">Number of requests that were issued only by devices against the tenant. Updated every 5 minutes. The following requests are not included:  * Requests made to &lt;kbd&gt;/user&lt;/kbd&gt;, &lt;kbd&gt;/tenant&lt;/kbd&gt; and &lt;kbd&gt;/application&lt;/kbd&gt; APIs * Application related requests (with &#x60;X-Cumulocity-Application-Key&#x60; header) .</param>
        /// <param name="deviceWithChildrenCount">Number of devices with children. Updated only three times a day starting at 8:57, 16:57 and 23:57..</param>
        /// <param name="eventsCreatedCount">Number of created events..</param>
        /// <param name="eventsUpdatedCount">Number of updates made to the events..</param>
        /// <param name="inventoriesCreatedCount">Number of created managed objects..</param>
        /// <param name="inventoriesUpdatedCount">Number of updates made to the managed objects..</param>
        /// <param name="measurementsCreatedCount">Number of created measurements.  &gt; **&amp;#9432; Info:** Bulk creation of measurements is handled in a way that each measurement is counted individually. .</param>
        /// <param name="requestCount">Number of requests that were made against the tenant. Updated every 5 minutes. The following requests are not included:  *  Internal SmartREST requests used to resolve templates *  Internal SLA monitoring requests *  Calls to any &lt;kbd&gt;/health&lt;/kbd&gt; endpoint *  Device bootstrap process requests related to configuring and retrieving device credentials *  Microservice SDK internal calls for applications and subscriptions .</param>
        /// <param name="resources">resources.</param>
        /// <param name="storageSize">Database storage in use, specified in bytes. It is affected by your retention rules and by the regularly running database optimization functions in Cumulocity IoT. If the size decreases, it does not necessarily mean that data was deleted. Updated only three times a day starting at 8:57, 16:57 and 23:57..</param>
        /// <param name="subscribedApplications">Names of the tenant subscribed applications. Updated only three times a day starting at 8:57, 16:57 and 23:57..</param>
        /// <param name="totalResourceCreateAndUpdateCount">Sum of all inbound transfers..</param>
        public DailyUsageStatistics(int alarmsCreatedCount = default(int), int alarmsUpdatedCount = default(int), DateTime day = default(DateTime), int deviceCount = default(int), int deviceEndpointCount = default(int), int deviceRequestCount = default(int), int deviceWithChildrenCount = default(int), int eventsCreatedCount = default(int), int eventsUpdatedCount = default(int), int inventoriesCreatedCount = default(int), int inventoriesUpdatedCount = default(int), int measurementsCreatedCount = default(int), int requestCount = default(int), UsageStatisticsResources resources = default(UsageStatisticsResources), int storageSize = default(int), List<string> subscribedApplications = default(List<string>), int totalResourceCreateAndUpdateCount = default(int))
        {
            this.AlarmsCreatedCount = alarmsCreatedCount;
            this.AlarmsUpdatedCount = alarmsUpdatedCount;
            this.Day = day;
            this.DeviceCount = deviceCount;
            this.DeviceEndpointCount = deviceEndpointCount;
            this.DeviceRequestCount = deviceRequestCount;
            this.DeviceWithChildrenCount = deviceWithChildrenCount;
            this.EventsCreatedCount = eventsCreatedCount;
            this.EventsUpdatedCount = eventsUpdatedCount;
            this.InventoriesCreatedCount = inventoriesCreatedCount;
            this.InventoriesUpdatedCount = inventoriesUpdatedCount;
            this.MeasurementsCreatedCount = measurementsCreatedCount;
            this.RequestCount = requestCount;
            this.Resources = resources;
            this.StorageSize = storageSize;
            this.SubscribedApplications = subscribedApplications;
            this.TotalResourceCreateAndUpdateCount = totalResourceCreateAndUpdateCount;
        }

        /// <summary>
        /// Number of created alarms.
        /// </summary>
        /// <value>Number of created alarms.</value>
        [DataMember(Name = "alarmsCreatedCount", EmitDefaultValue = false)]
        public int AlarmsCreatedCount { get; set; }

        /// <summary>
        /// Number of updates made to the alarms.
        /// </summary>
        /// <value>Number of updates made to the alarms.</value>
        [DataMember(Name = "alarmsUpdatedCount", EmitDefaultValue = false)]
        public int AlarmsUpdatedCount { get; set; }

        /// <summary>
        /// Date of this usage statistics object.
        /// </summary>
        /// <value>Date of this usage statistics object.</value>
        [DataMember(Name = "day", EmitDefaultValue = false)]
        public DateTime Day { get; set; }

        /// <summary>
        /// Number of devices in the tenant identified by the fragment &#x60;c8y_IsDevice&#x60;. Updated only three times a day starting at 8:57, 16:57 and 23:57.
        /// </summary>
        /// <value>Number of devices in the tenant identified by the fragment &#x60;c8y_IsDevice&#x60;. Updated only three times a day starting at 8:57, 16:57 and 23:57.</value>
        [DataMember(Name = "deviceCount", EmitDefaultValue = false)]
        public int DeviceCount { get; set; }

        /// <summary>
        /// Number of devices which do not have child devices. Updated only three times a day starting at 8:57, 16:57 and 23:57.
        /// </summary>
        /// <value>Number of devices which do not have child devices. Updated only three times a day starting at 8:57, 16:57 and 23:57.</value>
        [DataMember(Name = "deviceEndpointCount", EmitDefaultValue = false)]
        public int DeviceEndpointCount { get; set; }

        /// <summary>
        /// Number of requests that were issued only by devices against the tenant. Updated every 5 minutes. The following requests are not included:  * Requests made to &lt;kbd&gt;/user&lt;/kbd&gt;, &lt;kbd&gt;/tenant&lt;/kbd&gt; and &lt;kbd&gt;/application&lt;/kbd&gt; APIs * Application related requests (with &#x60;X-Cumulocity-Application-Key&#x60; header) 
        /// </summary>
        /// <value>Number of requests that were issued only by devices against the tenant. Updated every 5 minutes. The following requests are not included:  * Requests made to &lt;kbd&gt;/user&lt;/kbd&gt;, &lt;kbd&gt;/tenant&lt;/kbd&gt; and &lt;kbd&gt;/application&lt;/kbd&gt; APIs * Application related requests (with &#x60;X-Cumulocity-Application-Key&#x60; header) </value>
        [DataMember(Name = "deviceRequestCount", EmitDefaultValue = false)]
        public int DeviceRequestCount { get; set; }

        /// <summary>
        /// Number of devices with children. Updated only three times a day starting at 8:57, 16:57 and 23:57.
        /// </summary>
        /// <value>Number of devices with children. Updated only three times a day starting at 8:57, 16:57 and 23:57.</value>
        [DataMember(Name = "deviceWithChildrenCount", EmitDefaultValue = false)]
        public int DeviceWithChildrenCount { get; set; }

        /// <summary>
        /// Number of created events.
        /// </summary>
        /// <value>Number of created events.</value>
        [DataMember(Name = "eventsCreatedCount", EmitDefaultValue = false)]
        public int EventsCreatedCount { get; set; }

        /// <summary>
        /// Number of updates made to the events.
        /// </summary>
        /// <value>Number of updates made to the events.</value>
        [DataMember(Name = "eventsUpdatedCount", EmitDefaultValue = false)]
        public int EventsUpdatedCount { get; set; }

        /// <summary>
        /// Number of created managed objects.
        /// </summary>
        /// <value>Number of created managed objects.</value>
        [DataMember(Name = "inventoriesCreatedCount", EmitDefaultValue = false)]
        public int InventoriesCreatedCount { get; set; }

        /// <summary>
        /// Number of updates made to the managed objects.
        /// </summary>
        /// <value>Number of updates made to the managed objects.</value>
        [DataMember(Name = "inventoriesUpdatedCount", EmitDefaultValue = false)]
        public int InventoriesUpdatedCount { get; set; }

        /// <summary>
        /// Number of created measurements.  &gt; **&amp;#9432; Info:** Bulk creation of measurements is handled in a way that each measurement is counted individually. 
        /// </summary>
        /// <value>Number of created measurements.  &gt; **&amp;#9432; Info:** Bulk creation of measurements is handled in a way that each measurement is counted individually. </value>
        [DataMember(Name = "measurementsCreatedCount", EmitDefaultValue = false)]
        public int MeasurementsCreatedCount { get; set; }

        /// <summary>
        /// Number of requests that were made against the tenant. Updated every 5 minutes. The following requests are not included:  *  Internal SmartREST requests used to resolve templates *  Internal SLA monitoring requests *  Calls to any &lt;kbd&gt;/health&lt;/kbd&gt; endpoint *  Device bootstrap process requests related to configuring and retrieving device credentials *  Microservice SDK internal calls for applications and subscriptions 
        /// </summary>
        /// <value>Number of requests that were made against the tenant. Updated every 5 minutes. The following requests are not included:  *  Internal SmartREST requests used to resolve templates *  Internal SLA monitoring requests *  Calls to any &lt;kbd&gt;/health&lt;/kbd&gt; endpoint *  Device bootstrap process requests related to configuring and retrieving device credentials *  Microservice SDK internal calls for applications and subscriptions </value>
        [DataMember(Name = "requestCount", EmitDefaultValue = false)]
        public int RequestCount { get; set; }

        /// <summary>
        /// Gets or Sets Resources
        /// </summary>
        [DataMember(Name = "resources", EmitDefaultValue = false)]
        public UsageStatisticsResources Resources { get; set; }

        /// <summary>
        /// A URL linking to this resource.
        /// </summary>
        /// <value>A URL linking to this resource.</value>
        [DataMember(Name = "self", EmitDefaultValue = false)]
        public string Self { get; private set; }

        /// <summary>
        /// Returns false as Self should not be serialized given that it's read-only.
        /// </summary>
        /// <returns>false (boolean)</returns>
        public bool ShouldSerializeSelf()
        {
            return false;
        }
        /// <summary>
        /// Database storage in use, specified in bytes. It is affected by your retention rules and by the regularly running database optimization functions in Cumulocity IoT. If the size decreases, it does not necessarily mean that data was deleted. Updated only three times a day starting at 8:57, 16:57 and 23:57.
        /// </summary>
        /// <value>Database storage in use, specified in bytes. It is affected by your retention rules and by the regularly running database optimization functions in Cumulocity IoT. If the size decreases, it does not necessarily mean that data was deleted. Updated only three times a day starting at 8:57, 16:57 and 23:57.</value>
        [DataMember(Name = "storageSize", EmitDefaultValue = false)]
        public int StorageSize { get; set; }

        /// <summary>
        /// Names of the tenant subscribed applications. Updated only three times a day starting at 8:57, 16:57 and 23:57.
        /// </summary>
        /// <value>Names of the tenant subscribed applications. Updated only three times a day starting at 8:57, 16:57 and 23:57.</value>
        [DataMember(Name = "subscribedApplications", EmitDefaultValue = false)]
        public List<string> SubscribedApplications { get; set; }

        /// <summary>
        /// Sum of all inbound transfers.
        /// </summary>
        /// <value>Sum of all inbound transfers.</value>
        [DataMember(Name = "totalResourceCreateAndUpdateCount", EmitDefaultValue = false)]
        public int TotalResourceCreateAndUpdateCount { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class DailyUsageStatistics {\n");
            sb.Append("  AlarmsCreatedCount: ").Append(AlarmsCreatedCount).Append("\n");
            sb.Append("  AlarmsUpdatedCount: ").Append(AlarmsUpdatedCount).Append("\n");
            sb.Append("  Day: ").Append(Day).Append("\n");
            sb.Append("  DeviceCount: ").Append(DeviceCount).Append("\n");
            sb.Append("  DeviceEndpointCount: ").Append(DeviceEndpointCount).Append("\n");
            sb.Append("  DeviceRequestCount: ").Append(DeviceRequestCount).Append("\n");
            sb.Append("  DeviceWithChildrenCount: ").Append(DeviceWithChildrenCount).Append("\n");
            sb.Append("  EventsCreatedCount: ").Append(EventsCreatedCount).Append("\n");
            sb.Append("  EventsUpdatedCount: ").Append(EventsUpdatedCount).Append("\n");
            sb.Append("  InventoriesCreatedCount: ").Append(InventoriesCreatedCount).Append("\n");
            sb.Append("  InventoriesUpdatedCount: ").Append(InventoriesUpdatedCount).Append("\n");
            sb.Append("  MeasurementsCreatedCount: ").Append(MeasurementsCreatedCount).Append("\n");
            sb.Append("  RequestCount: ").Append(RequestCount).Append("\n");
            sb.Append("  Resources: ").Append(Resources).Append("\n");
            sb.Append("  Self: ").Append(Self).Append("\n");
            sb.Append("  StorageSize: ").Append(StorageSize).Append("\n");
            sb.Append("  SubscribedApplications: ").Append(SubscribedApplications).Append("\n");
            sb.Append("  TotalResourceCreateAndUpdateCount: ").Append(TotalResourceCreateAndUpdateCount).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as DailyUsageStatistics);
        }

        /// <summary>
        /// Returns true if DailyUsageStatistics instances are equal
        /// </summary>
        /// <param name="input">Instance of DailyUsageStatistics to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(DailyUsageStatistics input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.AlarmsCreatedCount == input.AlarmsCreatedCount ||
                    this.AlarmsCreatedCount.Equals(input.AlarmsCreatedCount)
                ) && 
                (
                    this.AlarmsUpdatedCount == input.AlarmsUpdatedCount ||
                    this.AlarmsUpdatedCount.Equals(input.AlarmsUpdatedCount)
                ) && 
                (
                    this.Day == input.Day ||
                    (this.Day != null &&
                    this.Day.Equals(input.Day))
                ) && 
                (
                    this.DeviceCount == input.DeviceCount ||
                    this.DeviceCount.Equals(input.DeviceCount)
                ) && 
                (
                    this.DeviceEndpointCount == input.DeviceEndpointCount ||
                    this.DeviceEndpointCount.Equals(input.DeviceEndpointCount)
                ) && 
                (
                    this.DeviceRequestCount == input.DeviceRequestCount ||
                    this.DeviceRequestCount.Equals(input.DeviceRequestCount)
                ) && 
                (
                    this.DeviceWithChildrenCount == input.DeviceWithChildrenCount ||
                    this.DeviceWithChildrenCount.Equals(input.DeviceWithChildrenCount)
                ) && 
                (
                    this.EventsCreatedCount == input.EventsCreatedCount ||
                    this.EventsCreatedCount.Equals(input.EventsCreatedCount)
                ) && 
                (
                    this.EventsUpdatedCount == input.EventsUpdatedCount ||
                    this.EventsUpdatedCount.Equals(input.EventsUpdatedCount)
                ) && 
                (
                    this.InventoriesCreatedCount == input.InventoriesCreatedCount ||
                    this.InventoriesCreatedCount.Equals(input.InventoriesCreatedCount)
                ) && 
                (
                    this.InventoriesUpdatedCount == input.InventoriesUpdatedCount ||
                    this.InventoriesUpdatedCount.Equals(input.InventoriesUpdatedCount)
                ) && 
                (
                    this.MeasurementsCreatedCount == input.MeasurementsCreatedCount ||
                    this.MeasurementsCreatedCount.Equals(input.MeasurementsCreatedCount)
                ) && 
                (
                    this.RequestCount == input.RequestCount ||
                    this.RequestCount.Equals(input.RequestCount)
                ) && 
                (
                    this.Resources == input.Resources ||
                    (this.Resources != null &&
                    this.Resources.Equals(input.Resources))
                ) && 
                (
                    this.Self == input.Self ||
                    (this.Self != null &&
                    this.Self.Equals(input.Self))
                ) && 
                (
                    this.StorageSize == input.StorageSize ||
                    this.StorageSize.Equals(input.StorageSize)
                ) && 
                (
                    this.SubscribedApplications == input.SubscribedApplications ||
                    this.SubscribedApplications != null &&
                    input.SubscribedApplications != null &&
                    this.SubscribedApplications.SequenceEqual(input.SubscribedApplications)
                ) && 
                (
                    this.TotalResourceCreateAndUpdateCount == input.TotalResourceCreateAndUpdateCount ||
                    this.TotalResourceCreateAndUpdateCount.Equals(input.TotalResourceCreateAndUpdateCount)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                hashCode = (hashCode * 59) + this.AlarmsCreatedCount.GetHashCode();
                hashCode = (hashCode * 59) + this.AlarmsUpdatedCount.GetHashCode();
                if (this.Day != null)
                {
                    hashCode = (hashCode * 59) + this.Day.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.DeviceCount.GetHashCode();
                hashCode = (hashCode * 59) + this.DeviceEndpointCount.GetHashCode();
                hashCode = (hashCode * 59) + this.DeviceRequestCount.GetHashCode();
                hashCode = (hashCode * 59) + this.DeviceWithChildrenCount.GetHashCode();
                hashCode = (hashCode * 59) + this.EventsCreatedCount.GetHashCode();
                hashCode = (hashCode * 59) + this.EventsUpdatedCount.GetHashCode();
                hashCode = (hashCode * 59) + this.InventoriesCreatedCount.GetHashCode();
                hashCode = (hashCode * 59) + this.InventoriesUpdatedCount.GetHashCode();
                hashCode = (hashCode * 59) + this.MeasurementsCreatedCount.GetHashCode();
                hashCode = (hashCode * 59) + this.RequestCount.GetHashCode();
                if (this.Resources != null)
                {
                    hashCode = (hashCode * 59) + this.Resources.GetHashCode();
                }
                if (this.Self != null)
                {
                    hashCode = (hashCode * 59) + this.Self.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.StorageSize.GetHashCode();
                if (this.SubscribedApplications != null)
                {
                    hashCode = (hashCode * 59) + this.SubscribedApplications.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.TotalResourceCreateAndUpdateCount.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(ValidationContext validationContext)
        {
            // AlarmsCreatedCount (int) minimum
            if (this.AlarmsCreatedCount < (int)0)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for AlarmsCreatedCount, must be a value greater than or equal to 0.", new [] { "AlarmsCreatedCount" });
            }

            // AlarmsUpdatedCount (int) minimum
            if (this.AlarmsUpdatedCount < (int)0)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for AlarmsUpdatedCount, must be a value greater than or equal to 0.", new [] { "AlarmsUpdatedCount" });
            }

            // DeviceCount (int) minimum
            if (this.DeviceCount < (int)0)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for DeviceCount, must be a value greater than or equal to 0.", new [] { "DeviceCount" });
            }

            // DeviceEndpointCount (int) minimum
            if (this.DeviceEndpointCount < (int)0)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for DeviceEndpointCount, must be a value greater than or equal to 0.", new [] { "DeviceEndpointCount" });
            }

            // DeviceRequestCount (int) minimum
            if (this.DeviceRequestCount < (int)0)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for DeviceRequestCount, must be a value greater than or equal to 0.", new [] { "DeviceRequestCount" });
            }

            // DeviceWithChildrenCount (int) minimum
            if (this.DeviceWithChildrenCount < (int)0)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for DeviceWithChildrenCount, must be a value greater than or equal to 0.", new [] { "DeviceWithChildrenCount" });
            }

            // EventsCreatedCount (int) minimum
            if (this.EventsCreatedCount < (int)0)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for EventsCreatedCount, must be a value greater than or equal to 0.", new [] { "EventsCreatedCount" });
            }

            // EventsUpdatedCount (int) minimum
            if (this.EventsUpdatedCount < (int)0)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for EventsUpdatedCount, must be a value greater than or equal to 0.", new [] { "EventsUpdatedCount" });
            }

            // InventoriesCreatedCount (int) minimum
            if (this.InventoriesCreatedCount < (int)0)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for InventoriesCreatedCount, must be a value greater than or equal to 0.", new [] { "InventoriesCreatedCount" });
            }

            // InventoriesUpdatedCount (int) minimum
            if (this.InventoriesUpdatedCount < (int)0)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for InventoriesUpdatedCount, must be a value greater than or equal to 0.", new [] { "InventoriesUpdatedCount" });
            }

            // MeasurementsCreatedCount (int) minimum
            if (this.MeasurementsCreatedCount < (int)0)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for MeasurementsCreatedCount, must be a value greater than or equal to 0.", new [] { "MeasurementsCreatedCount" });
            }

            // RequestCount (int) minimum
            if (this.RequestCount < (int)0)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for RequestCount, must be a value greater than or equal to 0.", new [] { "RequestCount" });
            }

            // StorageSize (int) minimum
            if (this.StorageSize < (int)0)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for StorageSize, must be a value greater than or equal to 0.", new [] { "StorageSize" });
            }

            // TotalResourceCreateAndUpdateCount (int) minimum
            if (this.TotalResourceCreateAndUpdateCount < (int)0)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for TotalResourceCreateAndUpdateCount, must be a value greater than or equal to 0.", new [] { "TotalResourceCreateAndUpdateCount" });
            }

            yield break;
        }
    }

}
