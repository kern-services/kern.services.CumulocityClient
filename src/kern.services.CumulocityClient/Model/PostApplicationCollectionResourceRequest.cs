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
    /// PostApplicationCollectionResourceRequest
    /// </summary>
    [DataContract(Name = "postApplicationCollectionResource_request")]
    public partial class PostApplicationCollectionResourceRequest : IEquatable<PostApplicationCollectionResourceRequest>, IValidatableObject
    {
        /// <summary>
        /// Application access level for other tenants.
        /// </summary>
        /// <value>Application access level for other tenants.</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum AvailabilityEnum
        {
            /// <summary>
            /// Enum MARKET for value: MARKET
            /// </summary>
            [EnumMember(Value = "MARKET")]
            MARKET = 1,

            /// <summary>
            /// Enum PRIVATE for value: PRIVATE
            /// </summary>
            [EnumMember(Value = "PRIVATE")]
            PRIVATE = 2

        }


        /// <summary>
        /// Application access level for other tenants.
        /// </summary>
        /// <value>Application access level for other tenants.</value>
        [DataMember(Name = "availability", EmitDefaultValue = false)]
        public AvailabilityEnum? Availability { get; set; }
        /// <summary>
        /// The type of the application.
        /// </summary>
        /// <value>The type of the application.</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum TypeEnum
        {
            /// <summary>
            /// Enum EXTERNAL for value: EXTERNAL
            /// </summary>
            [EnumMember(Value = "EXTERNAL")]
            EXTERNAL = 1,

            /// <summary>
            /// Enum HOSTED for value: HOSTED
            /// </summary>
            [EnumMember(Value = "HOSTED")]
            HOSTED = 2,

            /// <summary>
            /// Enum MICROSERVICE for value: MICROSERVICE
            /// </summary>
            [EnumMember(Value = "MICROSERVICE")]
            MICROSERVICE = 3

        }


        /// <summary>
        /// The type of the application.
        /// </summary>
        /// <value>The type of the application.</value>
        [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
        public TypeEnum Type { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PostApplicationCollectionResourceRequest" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected PostApplicationCollectionResourceRequest() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PostApplicationCollectionResourceRequest" /> class.
        /// </summary>
        /// <param name="availability">Application access level for other tenants. (default to AvailabilityEnum.PRIVATE).</param>
        /// <param name="contextPath">The context path in the URL makes the application accessible. Mandatory when the type of the application is &#x60;HOSTED&#x60;..</param>
        /// <param name="description">Description of the application..</param>
        /// <param name="key">Applications, regardless of their form, are identified by an application key. (required).</param>
        /// <param name="name">Name of the application. (required).</param>
        /// <param name="owner">owner.</param>
        /// <param name="type">The type of the application. (required).</param>
        /// <param name="manifest">manifest.</param>
        /// <param name="roles">Roles provided by the microservice..</param>
        /// <param name="requiredRoles">List of permissions required by a microservice to work..</param>
        /// <param name="breadcrumbs">A flag to indicate if the application has a breadcrumbs navigation on the UI. &gt; **&amp;#9432; Info:** This property is specific to the web application type. .</param>
        /// <param name="contentSecurityPolicy">The content security policy of the application. &gt; **&amp;#9432; Info:** This property is specific to the web application type. .</param>
        /// <param name="dynamicOptionsUrl">A URL to a JSON object with dynamic content options. &gt; **&amp;#9432; Info:** This property is specific to the web application type. .</param>
        /// <param name="globalTitle">The global title of the application. &gt; **&amp;#9432; Info:** This property is specific to the web application type. .</param>
        /// <param name="legacy">A flag that shows if the application is a legacy application or not. &gt; **&amp;#9432; Info:** This property is specific to the web application type. .</param>
        /// <param name="rightDrawer">A flag to indicate if the application uses the UI context menu on the right side. &gt; **&amp;#9432; Info:** This property is specific to the web application type. .</param>
        /// <param name="upgrade">A flag that shows if the application is hybrid and using Angular and AngularJS simultaneously. &gt; **&amp;#9432; Info:** This property is specific to the web application type. .</param>
        public PostApplicationCollectionResourceRequest(AvailabilityEnum? availability = AvailabilityEnum.PRIVATE, string contextPath = default(string), string description = default(string), string key = default(string), string name = default(string), ApplicationOwner owner = default(ApplicationOwner), TypeEnum type = default(TypeEnum), ApplicationManifest manifest = default(ApplicationManifest), List<string> roles = default(List<string>), List<string> requiredRoles = default(List<string>), bool breadcrumbs = default(bool), string contentSecurityPolicy = default(string), string dynamicOptionsUrl = default(string), string globalTitle = default(string), bool legacy = default(bool), bool rightDrawer = default(bool), bool upgrade = default(bool))
        {
            // to ensure "key" is required (not null)
            if (key == null)
            {
                throw new ArgumentNullException("key is a required property for PostApplicationCollectionResourceRequest and cannot be null");
            }
            this.Key = key;
            // to ensure "name" is required (not null)
            if (name == null)
            {
                throw new ArgumentNullException("name is a required property for PostApplicationCollectionResourceRequest and cannot be null");
            }
            this.Name = name;
            this.Type = type;
            this.Availability = availability;
            this.ContextPath = contextPath;
            this.Description = description;
            this.Owner = owner;
            this.Manifest = manifest;
            this.Roles = roles;
            this.RequiredRoles = requiredRoles;
            this.Breadcrumbs = breadcrumbs;
            this.ContentSecurityPolicy = contentSecurityPolicy;
            this.DynamicOptionsUrl = dynamicOptionsUrl;
            this.GlobalTitle = globalTitle;
            this.Legacy = legacy;
            this.RightDrawer = rightDrawer;
            this.Upgrade = upgrade;
        }

        /// <summary>
        /// The context path in the URL makes the application accessible. Mandatory when the type of the application is &#x60;HOSTED&#x60;.
        /// </summary>
        /// <value>The context path in the URL makes the application accessible. Mandatory when the type of the application is &#x60;HOSTED&#x60;.</value>
        [DataMember(Name = "contextPath", EmitDefaultValue = false)]
        public string ContextPath { get; set; }

        /// <summary>
        /// Description of the application.
        /// </summary>
        /// <value>Description of the application.</value>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Unique identifier of the application.
        /// </summary>
        /// <value>Unique identifier of the application.</value>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; private set; }

        /// <summary>
        /// Returns false as Id should not be serialized given that it's read-only.
        /// </summary>
        /// <returns>false (boolean)</returns>
        public bool ShouldSerializeId()
        {
            return false;
        }
        /// <summary>
        /// Applications, regardless of their form, are identified by an application key.
        /// </summary>
        /// <value>Applications, regardless of their form, are identified by an application key.</value>
        [DataMember(Name = "key", IsRequired = true, EmitDefaultValue = true)]
        public string Key { get; set; }

        /// <summary>
        /// Name of the application.
        /// </summary>
        /// <value>Name of the application.</value>
        [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Owner
        /// </summary>
        [DataMember(Name = "owner", EmitDefaultValue = false)]
        public ApplicationOwner Owner { get; set; }

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
        /// Gets or Sets Manifest
        /// </summary>
        [DataMember(Name = "manifest", EmitDefaultValue = false)]
        public ApplicationManifest Manifest { get; set; }

        /// <summary>
        /// Roles provided by the microservice.
        /// </summary>
        /// <value>Roles provided by the microservice.</value>
        [DataMember(Name = "roles", EmitDefaultValue = false)]
        public List<string> Roles { get; set; }

        /// <summary>
        /// List of permissions required by a microservice to work.
        /// </summary>
        /// <value>List of permissions required by a microservice to work.</value>
        [DataMember(Name = "requiredRoles", EmitDefaultValue = false)]
        public List<string> RequiredRoles { get; set; }

        /// <summary>
        /// A flag to indicate if the application has a breadcrumbs navigation on the UI. &gt; **&amp;#9432; Info:** This property is specific to the web application type. 
        /// </summary>
        /// <value>A flag to indicate if the application has a breadcrumbs navigation on the UI. &gt; **&amp;#9432; Info:** This property is specific to the web application type. </value>
        [DataMember(Name = "breadcrumbs", EmitDefaultValue = true)]
        public bool Breadcrumbs { get; set; }

        /// <summary>
        /// The content security policy of the application. &gt; **&amp;#9432; Info:** This property is specific to the web application type. 
        /// </summary>
        /// <value>The content security policy of the application. &gt; **&amp;#9432; Info:** This property is specific to the web application type. </value>
        [DataMember(Name = "contentSecurityPolicy", EmitDefaultValue = false)]
        public string ContentSecurityPolicy { get; set; }

        /// <summary>
        /// A URL to a JSON object with dynamic content options. &gt; **&amp;#9432; Info:** This property is specific to the web application type. 
        /// </summary>
        /// <value>A URL to a JSON object with dynamic content options. &gt; **&amp;#9432; Info:** This property is specific to the web application type. </value>
        [DataMember(Name = "dynamicOptionsUrl", EmitDefaultValue = false)]
        public string DynamicOptionsUrl { get; set; }

        /// <summary>
        /// The global title of the application. &gt; **&amp;#9432; Info:** This property is specific to the web application type. 
        /// </summary>
        /// <value>The global title of the application. &gt; **&amp;#9432; Info:** This property is specific to the web application type. </value>
        [DataMember(Name = "globalTitle", EmitDefaultValue = false)]
        public string GlobalTitle { get; set; }

        /// <summary>
        /// A flag that shows if the application is a legacy application or not. &gt; **&amp;#9432; Info:** This property is specific to the web application type. 
        /// </summary>
        /// <value>A flag that shows if the application is a legacy application or not. &gt; **&amp;#9432; Info:** This property is specific to the web application type. </value>
        [DataMember(Name = "legacy", EmitDefaultValue = true)]
        public bool Legacy { get; set; }

        /// <summary>
        /// A flag to indicate if the application uses the UI context menu on the right side. &gt; **&amp;#9432; Info:** This property is specific to the web application type. 
        /// </summary>
        /// <value>A flag to indicate if the application uses the UI context menu on the right side. &gt; **&amp;#9432; Info:** This property is specific to the web application type. </value>
        [DataMember(Name = "rightDrawer", EmitDefaultValue = true)]
        public bool RightDrawer { get; set; }

        /// <summary>
        /// A flag that shows if the application is hybrid and using Angular and AngularJS simultaneously. &gt; **&amp;#9432; Info:** This property is specific to the web application type. 
        /// </summary>
        /// <value>A flag that shows if the application is hybrid and using Angular and AngularJS simultaneously. &gt; **&amp;#9432; Info:** This property is specific to the web application type. </value>
        [DataMember(Name = "upgrade", EmitDefaultValue = true)]
        public bool Upgrade { get; set; }

        /// <summary>
        /// The active version ID of the application. For microservice applications the active version ID is the microservice manifest version ID.
        /// </summary>
        /// <value>The active version ID of the application. For microservice applications the active version ID is the microservice manifest version ID.</value>
        [DataMember(Name = "activeVersionId", EmitDefaultValue = false)]
        public string ActiveVersionId { get; private set; }

        /// <summary>
        /// Returns false as ActiveVersionId should not be serialized given that it's read-only.
        /// </summary>
        /// <returns>false (boolean)</returns>
        public bool ShouldSerializeActiveVersionId()
        {
            return false;
        }
        /// <summary>
        /// URL to the application base directory hosted on an external server. Only present in legacy hosted applications.
        /// </summary>
        /// <value>URL to the application base directory hosted on an external server. Only present in legacy hosted applications.</value>
        [DataMember(Name = "resourcesUrl", EmitDefaultValue = false)]
        [Obsolete]
        public string ResourcesUrl { get; private set; }

        /// <summary>
        /// Returns false as ResourcesUrl should not be serialized given that it's read-only.
        /// </summary>
        /// <returns>false (boolean)</returns>
        public bool ShouldSerializeResourcesUrl()
        {
            return false;
        }
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class PostApplicationCollectionResourceRequest {\n");
            sb.Append("  Availability: ").Append(Availability).Append("\n");
            sb.Append("  ContextPath: ").Append(ContextPath).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Key: ").Append(Key).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Owner: ").Append(Owner).Append("\n");
            sb.Append("  Self: ").Append(Self).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Manifest: ").Append(Manifest).Append("\n");
            sb.Append("  Roles: ").Append(Roles).Append("\n");
            sb.Append("  RequiredRoles: ").Append(RequiredRoles).Append("\n");
            sb.Append("  Breadcrumbs: ").Append(Breadcrumbs).Append("\n");
            sb.Append("  ContentSecurityPolicy: ").Append(ContentSecurityPolicy).Append("\n");
            sb.Append("  DynamicOptionsUrl: ").Append(DynamicOptionsUrl).Append("\n");
            sb.Append("  GlobalTitle: ").Append(GlobalTitle).Append("\n");
            sb.Append("  Legacy: ").Append(Legacy).Append("\n");
            sb.Append("  RightDrawer: ").Append(RightDrawer).Append("\n");
            sb.Append("  Upgrade: ").Append(Upgrade).Append("\n");
            sb.Append("  ActiveVersionId: ").Append(ActiveVersionId).Append("\n");
            sb.Append("  ResourcesUrl: ").Append(ResourcesUrl).Append("\n");
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
            return this.Equals(input as PostApplicationCollectionResourceRequest);
        }

        /// <summary>
        /// Returns true if PostApplicationCollectionResourceRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of PostApplicationCollectionResourceRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PostApplicationCollectionResourceRequest input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.Availability == input.Availability ||
                    this.Availability.Equals(input.Availability)
                ) && 
                (
                    this.ContextPath == input.ContextPath ||
                    (this.ContextPath != null &&
                    this.ContextPath.Equals(input.ContextPath))
                ) && 
                (
                    this.Description == input.Description ||
                    (this.Description != null &&
                    this.Description.Equals(input.Description))
                ) && 
                (
                    this.Id == input.Id ||
                    (this.Id != null &&
                    this.Id.Equals(input.Id))
                ) && 
                (
                    this.Key == input.Key ||
                    (this.Key != null &&
                    this.Key.Equals(input.Key))
                ) && 
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) && 
                (
                    this.Owner == input.Owner ||
                    (this.Owner != null &&
                    this.Owner.Equals(input.Owner))
                ) && 
                (
                    this.Self == input.Self ||
                    (this.Self != null &&
                    this.Self.Equals(input.Self))
                ) && 
                (
                    this.Type == input.Type ||
                    this.Type.Equals(input.Type)
                ) && 
                (
                    this.Manifest == input.Manifest ||
                    (this.Manifest != null &&
                    this.Manifest.Equals(input.Manifest))
                ) && 
                (
                    this.Roles == input.Roles ||
                    this.Roles != null &&
                    input.Roles != null &&
                    this.Roles.SequenceEqual(input.Roles)
                ) && 
                (
                    this.RequiredRoles == input.RequiredRoles ||
                    this.RequiredRoles != null &&
                    input.RequiredRoles != null &&
                    this.RequiredRoles.SequenceEqual(input.RequiredRoles)
                ) && 
                (
                    this.Breadcrumbs == input.Breadcrumbs ||
                    this.Breadcrumbs.Equals(input.Breadcrumbs)
                ) && 
                (
                    this.ContentSecurityPolicy == input.ContentSecurityPolicy ||
                    (this.ContentSecurityPolicy != null &&
                    this.ContentSecurityPolicy.Equals(input.ContentSecurityPolicy))
                ) && 
                (
                    this.DynamicOptionsUrl == input.DynamicOptionsUrl ||
                    (this.DynamicOptionsUrl != null &&
                    this.DynamicOptionsUrl.Equals(input.DynamicOptionsUrl))
                ) && 
                (
                    this.GlobalTitle == input.GlobalTitle ||
                    (this.GlobalTitle != null &&
                    this.GlobalTitle.Equals(input.GlobalTitle))
                ) && 
                (
                    this.Legacy == input.Legacy ||
                    this.Legacy.Equals(input.Legacy)
                ) && 
                (
                    this.RightDrawer == input.RightDrawer ||
                    this.RightDrawer.Equals(input.RightDrawer)
                ) && 
                (
                    this.Upgrade == input.Upgrade ||
                    this.Upgrade.Equals(input.Upgrade)
                ) && 
                (
                    this.ActiveVersionId == input.ActiveVersionId ||
                    (this.ActiveVersionId != null &&
                    this.ActiveVersionId.Equals(input.ActiveVersionId))
                ) && 
                (
                    this.ResourcesUrl == input.ResourcesUrl ||
                    (this.ResourcesUrl != null &&
                    this.ResourcesUrl.Equals(input.ResourcesUrl))
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
                hashCode = (hashCode * 59) + this.Availability.GetHashCode();
                if (this.ContextPath != null)
                {
                    hashCode = (hashCode * 59) + this.ContextPath.GetHashCode();
                }
                if (this.Description != null)
                {
                    hashCode = (hashCode * 59) + this.Description.GetHashCode();
                }
                if (this.Id != null)
                {
                    hashCode = (hashCode * 59) + this.Id.GetHashCode();
                }
                if (this.Key != null)
                {
                    hashCode = (hashCode * 59) + this.Key.GetHashCode();
                }
                if (this.Name != null)
                {
                    hashCode = (hashCode * 59) + this.Name.GetHashCode();
                }
                if (this.Owner != null)
                {
                    hashCode = (hashCode * 59) + this.Owner.GetHashCode();
                }
                if (this.Self != null)
                {
                    hashCode = (hashCode * 59) + this.Self.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.Type.GetHashCode();
                if (this.Manifest != null)
                {
                    hashCode = (hashCode * 59) + this.Manifest.GetHashCode();
                }
                if (this.Roles != null)
                {
                    hashCode = (hashCode * 59) + this.Roles.GetHashCode();
                }
                if (this.RequiredRoles != null)
                {
                    hashCode = (hashCode * 59) + this.RequiredRoles.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.Breadcrumbs.GetHashCode();
                if (this.ContentSecurityPolicy != null)
                {
                    hashCode = (hashCode * 59) + this.ContentSecurityPolicy.GetHashCode();
                }
                if (this.DynamicOptionsUrl != null)
                {
                    hashCode = (hashCode * 59) + this.DynamicOptionsUrl.GetHashCode();
                }
                if (this.GlobalTitle != null)
                {
                    hashCode = (hashCode * 59) + this.GlobalTitle.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.Legacy.GetHashCode();
                hashCode = (hashCode * 59) + this.RightDrawer.GetHashCode();
                hashCode = (hashCode * 59) + this.Upgrade.GetHashCode();
                if (this.ActiveVersionId != null)
                {
                    hashCode = (hashCode * 59) + this.ActiveVersionId.GetHashCode();
                }
                if (this.ResourcesUrl != null)
                {
                    hashCode = (hashCode * 59) + this.ResourcesUrl.GetHashCode();
                }
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
            // ContextPath (string) minLength
            if (this.ContextPath != null && this.ContextPath.Length < 1)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for ContextPath, length must be greater than 1.", new [] { "ContextPath" });
            }

            // Key (string) minLength
            if (this.Key != null && this.Key.Length < 1)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Key, length must be greater than 1.", new [] { "Key" });
            }

            // Name (string) minLength
            if (this.Name != null && this.Name.Length < 1)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Name, length must be greater than 1.", new [] { "Name" });
            }

            yield break;
        }
    }

}
