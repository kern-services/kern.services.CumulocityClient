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
    /// AuditRecord
    /// </summary>
    [DataContract(Name = "auditRecord")]
    public partial class AuditRecord : Dictionary<String, Object>, IEquatable<AuditRecord>, IValidatableObject
    {
        /// <summary>
        /// The severity of the audit action.
        /// </summary>
        /// <value>The severity of the audit action.</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum SeverityEnum
        {
            /// <summary>
            /// Enum CRITICAL for value: CRITICAL
            /// </summary>
            [EnumMember(Value = "CRITICAL")]
            CRITICAL = 1,

            /// <summary>
            /// Enum MAJOR for value: MAJOR
            /// </summary>
            [EnumMember(Value = "MAJOR")]
            MAJOR = 2,

            /// <summary>
            /// Enum MINOR for value: MINOR
            /// </summary>
            [EnumMember(Value = "MINOR")]
            MINOR = 3,

            /// <summary>
            /// Enum WARNING for value: WARNING
            /// </summary>
            [EnumMember(Value = "WARNING")]
            WARNING = 4,

            /// <summary>
            /// Enum INFORMATION for value: INFORMATION
            /// </summary>
            [EnumMember(Value = "INFORMATION")]
            INFORMATION = 5

        }


        /// <summary>
        /// The severity of the audit action.
        /// </summary>
        /// <value>The severity of the audit action.</value>
        [DataMember(Name = "severity", EmitDefaultValue = false)]
        public SeverityEnum? Severity { get; set; }

        /// <summary>
        /// Returns false as Severity should not be serialized given that it's read-only.
        /// </summary>
        /// <returns>false (boolean)</returns>
        public bool ShouldSerializeSeverity()
        {
            return false;
        }
        /// <summary>
        /// Identifies the platform component of the audit.
        /// </summary>
        /// <value>Identifies the platform component of the audit.</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum TypeEnum
        {
            /// <summary>
            /// Enum Alarm for value: Alarm
            /// </summary>
            [EnumMember(Value = "Alarm")]
            Alarm = 1,

            /// <summary>
            /// Enum Application for value: Application
            /// </summary>
            [EnumMember(Value = "Application")]
            Application = 2,

            /// <summary>
            /// Enum BulkOperation for value: BulkOperation
            /// </summary>
            [EnumMember(Value = "BulkOperation")]
            BulkOperation = 3,

            /// <summary>
            /// Enum CepModule for value: CepModule
            /// </summary>
            [EnumMember(Value = "CepModule")]
            CepModule = 4,

            /// <summary>
            /// Enum Connector for value: Connector
            /// </summary>
            [EnumMember(Value = "Connector")]
            Connector = 5,

            /// <summary>
            /// Enum Event for value: Event
            /// </summary>
            [EnumMember(Value = "Event")]
            Event = 6,

            /// <summary>
            /// Enum Group for value: Group
            /// </summary>
            [EnumMember(Value = "Group")]
            Group = 7,

            /// <summary>
            /// Enum Inventory for value: Inventory
            /// </summary>
            [EnumMember(Value = "Inventory")]
            Inventory = 8,

            /// <summary>
            /// Enum InventoryRole for value: InventoryRole
            /// </summary>
            [EnumMember(Value = "InventoryRole")]
            InventoryRole = 9,

            /// <summary>
            /// Enum Operation for value: Operation
            /// </summary>
            [EnumMember(Value = "Operation")]
            Operation = 10,

            /// <summary>
            /// Enum Option for value: Option
            /// </summary>
            [EnumMember(Value = "Option")]
            Option = 11,

            /// <summary>
            /// Enum Report for value: Report
            /// </summary>
            [EnumMember(Value = "Report")]
            Report = 12,

            /// <summary>
            /// Enum SingleSignOn for value: SingleSignOn
            /// </summary>
            [EnumMember(Value = "SingleSignOn")]
            SingleSignOn = 13,

            /// <summary>
            /// Enum SmartRule for value: SmartRule
            /// </summary>
            [EnumMember(Value = "SmartRule")]
            SmartRule = 14,

            /// <summary>
            /// Enum SYSTEM for value: SYSTEM
            /// </summary>
            [EnumMember(Value = "SYSTEM")]
            SYSTEM = 15,

            /// <summary>
            /// Enum Tenant for value: Tenant
            /// </summary>
            [EnumMember(Value = "Tenant")]
            Tenant = 16,

            /// <summary>
            /// Enum TenantAuthConfig for value: TenantAuthConfig
            /// </summary>
            [EnumMember(Value = "TenantAuthConfig")]
            TenantAuthConfig = 17,

            /// <summary>
            /// Enum TrustedCertificates for value: TrustedCertificates
            /// </summary>
            [EnumMember(Value = "TrustedCertificates")]
            TrustedCertificates = 18,

            /// <summary>
            /// Enum User for value: User
            /// </summary>
            [EnumMember(Value = "User")]
            User = 19,

            /// <summary>
            /// Enum UserAuthentication for value: UserAuthentication
            /// </summary>
            [EnumMember(Value = "UserAuthentication")]
            UserAuthentication = 20

        }


        /// <summary>
        /// Identifies the platform component of the audit.
        /// </summary>
        /// <value>Identifies the platform component of the audit.</value>
        [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
        public TypeEnum Type { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRecord" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected AuditRecord() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRecord" /> class.
        /// </summary>
        /// <param name="activity">Summary of the action that was carried out. (required).</param>
        /// <param name="c8yMetadata">c8yMetadata.</param>
        /// <param name="source">source (required).</param>
        /// <param name="text">Details of the action that was carried out. (required).</param>
        /// <param name="time">The date and time when the audit is updated. (required).</param>
        /// <param name="type">Identifies the platform component of the audit. (required).</param>
        /// <param name="user">The user who carried out the activity..</param>
        public AuditRecord(string activity = default(string), AuditRecordC8yMetadata c8yMetadata = default(AuditRecordC8yMetadata), AuditRecordSource source = default(AuditRecordSource), string text = default(string), DateTime time = default(DateTime), TypeEnum type = default(TypeEnum), string user = default(string)) : base()
        {
            // to ensure "activity" is required (not null)
            if (activity == null)
            {
                throw new ArgumentNullException("activity is a required property for AuditRecord and cannot be null");
            }
            this.Activity = activity;
            // to ensure "source" is required (not null)
            if (source == null)
            {
                throw new ArgumentNullException("source is a required property for AuditRecord and cannot be null");
            }
            this.Source = source;
            // to ensure "text" is required (not null)
            if (text == null)
            {
                throw new ArgumentNullException("text is a required property for AuditRecord and cannot be null");
            }
            this.Text = text;
            this.Time = time;
            this.Type = type;
            this.C8yMetadata = c8yMetadata;
            this.User = user;
        }

        /// <summary>
        /// Summary of the action that was carried out.
        /// </summary>
        /// <value>Summary of the action that was carried out.</value>
        [DataMember(Name = "activity", IsRequired = true, EmitDefaultValue = true)]
        public string Activity { get; set; }

        /// <summary>
        /// Name of the application that performed the action.
        /// </summary>
        /// <value>Name of the application that performed the action.</value>
        [DataMember(Name = "application", EmitDefaultValue = false)]
        public string Application { get; private set; }

        /// <summary>
        /// Returns false as Application should not be serialized given that it's read-only.
        /// </summary>
        /// <returns>false (boolean)</returns>
        public bool ShouldSerializeApplication()
        {
            return false;
        }
        /// <summary>
        /// Gets or Sets C8yMetadata
        /// </summary>
        [DataMember(Name = "c8y_Metadata", EmitDefaultValue = false)]
        public AuditRecordC8yMetadata C8yMetadata { get; set; }

        /// <summary>
        /// Collection of objects describing the changes that were carried out.
        /// </summary>
        /// <value>Collection of objects describing the changes that were carried out.</value>
        [DataMember(Name = "changes", EmitDefaultValue = false)]
        public List<AuditRecordChangesInner> Changes { get; private set; }

        /// <summary>
        /// Returns false as Changes should not be serialized given that it's read-only.
        /// </summary>
        /// <returns>false (boolean)</returns>
        public bool ShouldSerializeChanges()
        {
            return false;
        }
        /// <summary>
        /// The date and time when the audit record was created.
        /// </summary>
        /// <value>The date and time when the audit record was created.</value>
        [DataMember(Name = "creationTime", EmitDefaultValue = false)]
        public DateTime CreationTime { get; private set; }

        /// <summary>
        /// Returns false as CreationTime should not be serialized given that it's read-only.
        /// </summary>
        /// <returns>false (boolean)</returns>
        public bool ShouldSerializeCreationTime()
        {
            return false;
        }
        /// <summary>
        /// Unique identifier of the audit record.
        /// </summary>
        /// <value>Unique identifier of the audit record.</value>
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
        /// Gets or Sets Source
        /// </summary>
        [DataMember(Name = "source", IsRequired = true, EmitDefaultValue = true)]
        public AuditRecordSource Source { get; set; }

        /// <summary>
        /// Details of the action that was carried out.
        /// </summary>
        /// <value>Details of the action that was carried out.</value>
        [DataMember(Name = "text", IsRequired = true, EmitDefaultValue = true)]
        public string Text { get; set; }

        /// <summary>
        /// The date and time when the audit is updated.
        /// </summary>
        /// <value>The date and time when the audit is updated.</value>
        [DataMember(Name = "time", IsRequired = true, EmitDefaultValue = true)]
        public DateTime Time { get; set; }

        /// <summary>
        /// The user who carried out the activity.
        /// </summary>
        /// <value>The user who carried out the activity.</value>
        [DataMember(Name = "user", EmitDefaultValue = false)]
        public string User { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class AuditRecord {\n");
            sb.Append("  ").Append(base.ToString().Replace("\n", "\n  ")).Append("\n");
            sb.Append("  Activity: ").Append(Activity).Append("\n");
            sb.Append("  Application: ").Append(Application).Append("\n");
            sb.Append("  C8yMetadata: ").Append(C8yMetadata).Append("\n");
            sb.Append("  Changes: ").Append(Changes).Append("\n");
            sb.Append("  CreationTime: ").Append(CreationTime).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Self: ").Append(Self).Append("\n");
            sb.Append("  Severity: ").Append(Severity).Append("\n");
            sb.Append("  Source: ").Append(Source).Append("\n");
            sb.Append("  Text: ").Append(Text).Append("\n");
            sb.Append("  Time: ").Append(Time).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  User: ").Append(User).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
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
            return this.Equals(input as AuditRecord);
        }

        /// <summary>
        /// Returns true if AuditRecord instances are equal
        /// </summary>
        /// <param name="input">Instance of AuditRecord to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(AuditRecord input)
        {
            if (input == null)
            {
                return false;
            }
            return base.Equals(input) && 
                (
                    this.Activity == input.Activity ||
                    (this.Activity != null &&
                    this.Activity.Equals(input.Activity))
                ) && base.Equals(input) && 
                (
                    this.Application == input.Application ||
                    (this.Application != null &&
                    this.Application.Equals(input.Application))
                ) && base.Equals(input) && 
                (
                    this.C8yMetadata == input.C8yMetadata ||
                    (this.C8yMetadata != null &&
                    this.C8yMetadata.Equals(input.C8yMetadata))
                ) && base.Equals(input) && 
                (
                    this.Changes == input.Changes ||
                    this.Changes != null &&
                    input.Changes != null &&
                    this.Changes.SequenceEqual(input.Changes)
                ) && base.Equals(input) && 
                (
                    this.CreationTime == input.CreationTime ||
                    (this.CreationTime != null &&
                    this.CreationTime.Equals(input.CreationTime))
                ) && base.Equals(input) && 
                (
                    this.Id == input.Id ||
                    (this.Id != null &&
                    this.Id.Equals(input.Id))
                ) && base.Equals(input) && 
                (
                    this.Self == input.Self ||
                    (this.Self != null &&
                    this.Self.Equals(input.Self))
                ) && base.Equals(input) && 
                (
                    this.Severity == input.Severity ||
                    this.Severity.Equals(input.Severity)
                ) && base.Equals(input) && 
                (
                    this.Source == input.Source ||
                    (this.Source != null &&
                    this.Source.Equals(input.Source))
                ) && base.Equals(input) && 
                (
                    this.Text == input.Text ||
                    (this.Text != null &&
                    this.Text.Equals(input.Text))
                ) && base.Equals(input) && 
                (
                    this.Time == input.Time ||
                    (this.Time != null &&
                    this.Time.Equals(input.Time))
                ) && base.Equals(input) && 
                (
                    this.Type == input.Type ||
                    this.Type.Equals(input.Type)
                ) && base.Equals(input) && 
                (
                    this.User == input.User ||
                    (this.User != null &&
                    this.User.Equals(input.User))
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
                int hashCode = base.GetHashCode();
                if (this.Activity != null)
                {
                    hashCode = (hashCode * 59) + this.Activity.GetHashCode();
                }
                if (this.Application != null)
                {
                    hashCode = (hashCode * 59) + this.Application.GetHashCode();
                }
                if (this.C8yMetadata != null)
                {
                    hashCode = (hashCode * 59) + this.C8yMetadata.GetHashCode();
                }
                if (this.Changes != null)
                {
                    hashCode = (hashCode * 59) + this.Changes.GetHashCode();
                }
                if (this.CreationTime != null)
                {
                    hashCode = (hashCode * 59) + this.CreationTime.GetHashCode();
                }
                if (this.Id != null)
                {
                    hashCode = (hashCode * 59) + this.Id.GetHashCode();
                }
                if (this.Self != null)
                {
                    hashCode = (hashCode * 59) + this.Self.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.Severity.GetHashCode();
                if (this.Source != null)
                {
                    hashCode = (hashCode * 59) + this.Source.GetHashCode();
                }
                if (this.Text != null)
                {
                    hashCode = (hashCode * 59) + this.Text.GetHashCode();
                }
                if (this.Time != null)
                {
                    hashCode = (hashCode * 59) + this.Time.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.Type.GetHashCode();
                if (this.User != null)
                {
                    hashCode = (hashCode * 59) + this.User.GetHashCode();
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
            yield break;
        }
    }

}
