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
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using OpenAPIDateConverter = kern.services.CumulocityClient.Client.OpenAPIDateConverter;

namespace kern.services.CumulocityClient.Model
{
    /// <summary>
    /// PostTenantCollectionResourceRequest
    /// </summary>
    [DataContract]
    public partial class PostTenantCollectionResourceRequest :  IEquatable<PostTenantCollectionResourceRequest>, IValidatableObject
    {
        /// <summary>
        /// Current status of the tenant.
        /// </summary>
        /// <value>Current status of the tenant.</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum StatusEnum
        {
            /// <summary>
            /// Enum ACTIVE for value: ACTIVE
            /// </summary>
            [EnumMember(Value = "ACTIVE")]
            ACTIVE = 1,

            /// <summary>
            /// Enum SUSPENDED for value: SUSPENDED
            /// </summary>
            [EnumMember(Value = "SUSPENDED")]
            SUSPENDED = 2

        }

        /// <summary>
        /// Current status of the tenant.
        /// </summary>
        /// <value>Current status of the tenant.</value>
        [DataMember(Name="status", EmitDefaultValue=false)]
        public StatusEnum? Status { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PostTenantCollectionResourceRequest" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected PostTenantCollectionResourceRequest() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PostTenantCollectionResourceRequest" /> class.
        /// </summary>
        /// <param name="adminEmail">Email address of the tenant&#39;s administrator..</param>
        /// <param name="adminName">Username of the tenant&#39;s administrator. &gt; **&amp;#9432; Info:** When it is provided in the request body, also &#x60;adminEmail&#x60; and &#x60;adminPass&#x60; must be provided.  (required).</param>
        /// <param name="adminPass">Password of the tenant&#39;s administrator..</param>
        /// <param name="applications">applications.</param>
        /// <param name="company">Tenant&#39;s company name. (required).</param>
        /// <param name="contactName">Name of the contact person..</param>
        /// <param name="contactPhone">Phone number of the contact person, provided in the international format, for example, +48 123 456 7890..</param>
        /// <param name="customProperties">customProperties.</param>
        /// <param name="domain">URL of the tenant&#39;s domain. The domain name permits only the use of alphanumeric characters separated by dots &#x60;.&#x60; and hyphens &#x60;-&#x60;. (required).</param>
        /// <param name="ownedApplications">ownedApplications.</param>
        public PostTenantCollectionResourceRequest(string adminEmail = default(string), string adminName = default(string), string adminPass = default(string), TenantApplications applications = default(TenantApplications), string company = default(string), string contactName = default(string), string contactPhone = default(string), CustomProperties customProperties = default(CustomProperties), string domain = default(string), TenantOwnedApplications ownedApplications = default(TenantOwnedApplications))
        {
            // to ensure "adminName" is required (not null)
            if (adminName == null)
            {
                throw new InvalidDataException("adminName is a required property for PostTenantCollectionResourceRequest and cannot be null");
            }
            else
            {
                this.AdminName = adminName;
            }

            // to ensure "company" is required (not null)
            if (company == null)
            {
                throw new InvalidDataException("company is a required property for PostTenantCollectionResourceRequest and cannot be null");
            }
            else
            {
                this.Company = company;
            }

            // to ensure "domain" is required (not null)
            if (domain == null)
            {
                throw new InvalidDataException("domain is a required property for PostTenantCollectionResourceRequest and cannot be null");
            }
            else
            {
                this.Domain = domain;
            }

            this.AdminEmail = adminEmail;
            this.AdminPass = adminPass;
            this.Applications = applications;
            this.ContactName = contactName;
            this.ContactPhone = contactPhone;
            this.CustomProperties = customProperties;
            this.OwnedApplications = ownedApplications;
        }

        /// <summary>
        /// Email address of the tenant&#39;s administrator.
        /// </summary>
        /// <value>Email address of the tenant&#39;s administrator.</value>
        [DataMember(Name="adminEmail", EmitDefaultValue=false)]
        public string AdminEmail { get; set; }

        /// <summary>
        /// Username of the tenant&#39;s administrator. &gt; **&amp;#9432; Info:** When it is provided in the request body, also &#x60;adminEmail&#x60; and &#x60;adminPass&#x60; must be provided. 
        /// </summary>
        /// <value>Username of the tenant&#39;s administrator. &gt; **&amp;#9432; Info:** When it is provided in the request body, also &#x60;adminEmail&#x60; and &#x60;adminPass&#x60; must be provided. </value>
        [DataMember(Name="adminName", EmitDefaultValue=true)]
        public string AdminName { get; set; }

        /// <summary>
        /// Password of the tenant&#39;s administrator.
        /// </summary>
        /// <value>Password of the tenant&#39;s administrator.</value>
        [DataMember(Name="adminPass", EmitDefaultValue=false)]
        public string AdminPass { get; set; }

        /// <summary>
        /// Indicates if this tenant can create subtenants.
        /// </summary>
        /// <value>Indicates if this tenant can create subtenants.</value>
        [DataMember(Name="allowCreateTenants", EmitDefaultValue=false)]
        public bool AllowCreateTenants { get; private set; }

        /// <summary>
        /// Gets or Sets Applications
        /// </summary>
        [DataMember(Name="applications", EmitDefaultValue=false)]
        public TenantApplications Applications { get; set; }

        /// <summary>
        /// Tenant&#39;s company name.
        /// </summary>
        /// <value>Tenant&#39;s company name.</value>
        [DataMember(Name="company", EmitDefaultValue=true)]
        public string Company { get; set; }

        /// <summary>
        /// Name of the contact person.
        /// </summary>
        /// <value>Name of the contact person.</value>
        [DataMember(Name="contactName", EmitDefaultValue=false)]
        public string ContactName { get; set; }

        /// <summary>
        /// Phone number of the contact person, provided in the international format, for example, +48 123 456 7890.
        /// </summary>
        /// <value>Phone number of the contact person, provided in the international format, for example, +48 123 456 7890.</value>
        [DataMember(Name="contactPhone", EmitDefaultValue=false)]
        public string ContactPhone { get; set; }

        /// <summary>
        /// The date and time when the tenant was created.
        /// </summary>
        /// <value>The date and time when the tenant was created.</value>
        [DataMember(Name="creationTime", EmitDefaultValue=false)]
        public DateTime CreationTime { get; private set; }

        /// <summary>
        /// Gets or Sets CustomProperties
        /// </summary>
        [DataMember(Name="customProperties", EmitDefaultValue=false)]
        public CustomProperties CustomProperties { get; set; }

        /// <summary>
        /// URL of the tenant&#39;s domain. The domain name permits only the use of alphanumeric characters separated by dots &#x60;.&#x60; and hyphens &#x60;-&#x60;.
        /// </summary>
        /// <value>URL of the tenant&#39;s domain. The domain name permits only the use of alphanumeric characters separated by dots &#x60;.&#x60; and hyphens &#x60;-&#x60;.</value>
        [DataMember(Name="domain", EmitDefaultValue=true)]
        public string Domain { get; set; }

        /// <summary>
        /// Unique identifier of a Cumulocity IoT tenant.
        /// </summary>
        /// <value>Unique identifier of a Cumulocity IoT tenant.</value>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; private set; }

        /// <summary>
        /// Gets or Sets OwnedApplications
        /// </summary>
        [DataMember(Name="ownedApplications", EmitDefaultValue=false)]
        public TenantOwnedApplications OwnedApplications { get; set; }

        /// <summary>
        /// ID of the parent tenant.
        /// </summary>
        /// <value>ID of the parent tenant.</value>
        [DataMember(Name="parent", EmitDefaultValue=false)]
        public string Parent { get; private set; }

        /// <summary>
        /// A URL linking to this resource.
        /// </summary>
        /// <value>A URL linking to this resource.</value>
        [DataMember(Name="self", EmitDefaultValue=false)]
        public string Self { get; private set; }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PostTenantCollectionResourceRequest {\n");
            sb.Append("  AdminEmail: ").Append(AdminEmail).Append("\n");
            sb.Append("  AdminName: ").Append(AdminName).Append("\n");
            sb.Append("  AdminPass: ").Append(AdminPass).Append("\n");
            sb.Append("  AllowCreateTenants: ").Append(AllowCreateTenants).Append("\n");
            sb.Append("  Applications: ").Append(Applications).Append("\n");
            sb.Append("  Company: ").Append(Company).Append("\n");
            sb.Append("  ContactName: ").Append(ContactName).Append("\n");
            sb.Append("  ContactPhone: ").Append(ContactPhone).Append("\n");
            sb.Append("  CreationTime: ").Append(CreationTime).Append("\n");
            sb.Append("  CustomProperties: ").Append(CustomProperties).Append("\n");
            sb.Append("  Domain: ").Append(Domain).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  OwnedApplications: ").Append(OwnedApplications).Append("\n");
            sb.Append("  Parent: ").Append(Parent).Append("\n");
            sb.Append("  Self: ").Append(Self).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
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
            return this.Equals(input as PostTenantCollectionResourceRequest);
        }

        /// <summary>
        /// Returns true if PostTenantCollectionResourceRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of PostTenantCollectionResourceRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PostTenantCollectionResourceRequest input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.AdminEmail == input.AdminEmail ||
                    (this.AdminEmail != null &&
                    this.AdminEmail.Equals(input.AdminEmail))
                ) && 
                (
                    this.AdminName == input.AdminName ||
                    (this.AdminName != null &&
                    this.AdminName.Equals(input.AdminName))
                ) && 
                (
                    this.AdminPass == input.AdminPass ||
                    (this.AdminPass != null &&
                    this.AdminPass.Equals(input.AdminPass))
                ) && 
                (
                    this.AllowCreateTenants == input.AllowCreateTenants ||
                    (this.AllowCreateTenants != null &&
                    this.AllowCreateTenants.Equals(input.AllowCreateTenants))
                ) && 
                (
                    this.Applications == input.Applications ||
                    (this.Applications != null &&
                    this.Applications.Equals(input.Applications))
                ) && 
                (
                    this.Company == input.Company ||
                    (this.Company != null &&
                    this.Company.Equals(input.Company))
                ) && 
                (
                    this.ContactName == input.ContactName ||
                    (this.ContactName != null &&
                    this.ContactName.Equals(input.ContactName))
                ) && 
                (
                    this.ContactPhone == input.ContactPhone ||
                    (this.ContactPhone != null &&
                    this.ContactPhone.Equals(input.ContactPhone))
                ) && 
                (
                    this.CreationTime == input.CreationTime ||
                    (this.CreationTime != null &&
                    this.CreationTime.Equals(input.CreationTime))
                ) && 
                (
                    this.CustomProperties == input.CustomProperties ||
                    (this.CustomProperties != null &&
                    this.CustomProperties.Equals(input.CustomProperties))
                ) && 
                (
                    this.Domain == input.Domain ||
                    (this.Domain != null &&
                    this.Domain.Equals(input.Domain))
                ) && 
                (
                    this.Id == input.Id ||
                    (this.Id != null &&
                    this.Id.Equals(input.Id))
                ) && 
                (
                    this.OwnedApplications == input.OwnedApplications ||
                    (this.OwnedApplications != null &&
                    this.OwnedApplications.Equals(input.OwnedApplications))
                ) && 
                (
                    this.Parent == input.Parent ||
                    (this.Parent != null &&
                    this.Parent.Equals(input.Parent))
                ) && 
                (
                    this.Self == input.Self ||
                    (this.Self != null &&
                    this.Self.Equals(input.Self))
                ) && 
                (
                    this.Status == input.Status ||
                    (this.Status != null &&
                    this.Status.Equals(input.Status))
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
                if (this.AdminEmail != null)
                    hashCode = hashCode * 59 + this.AdminEmail.GetHashCode();
                if (this.AdminName != null)
                    hashCode = hashCode * 59 + this.AdminName.GetHashCode();
                if (this.AdminPass != null)
                    hashCode = hashCode * 59 + this.AdminPass.GetHashCode();
                if (this.AllowCreateTenants != null)
                    hashCode = hashCode * 59 + this.AllowCreateTenants.GetHashCode();
                if (this.Applications != null)
                    hashCode = hashCode * 59 + this.Applications.GetHashCode();
                if (this.Company != null)
                    hashCode = hashCode * 59 + this.Company.GetHashCode();
                if (this.ContactName != null)
                    hashCode = hashCode * 59 + this.ContactName.GetHashCode();
                if (this.ContactPhone != null)
                    hashCode = hashCode * 59 + this.ContactPhone.GetHashCode();
                if (this.CreationTime != null)
                    hashCode = hashCode * 59 + this.CreationTime.GetHashCode();
                if (this.CustomProperties != null)
                    hashCode = hashCode * 59 + this.CustomProperties.GetHashCode();
                if (this.Domain != null)
                    hashCode = hashCode * 59 + this.Domain.GetHashCode();
                if (this.Id != null)
                    hashCode = hashCode * 59 + this.Id.GetHashCode();
                if (this.OwnedApplications != null)
                    hashCode = hashCode * 59 + this.OwnedApplications.GetHashCode();
                if (this.Parent != null)
                    hashCode = hashCode * 59 + this.Parent.GetHashCode();
                if (this.Self != null)
                    hashCode = hashCode * 59 + this.Self.GetHashCode();
                if (this.Status != null)
                    hashCode = hashCode * 59 + this.Status.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            // AdminName (string) maxLength
            if(this.AdminName != null && this.AdminName.Length > 50)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for AdminName, length must be less than 50.", new [] { "AdminName" });
            }

            // AdminName (string) minLength
            if(this.AdminName != null && this.AdminName.Length < 1)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for AdminName, length must be greater than 1.", new [] { "AdminName" });
            }

            // AdminPass (string) maxLength
            if(this.AdminPass != null && this.AdminPass.Length > 32)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for AdminPass, length must be less than 32.", new [] { "AdminPass" });
            }


            // Company (string) maxLength
            if(this.Company != null && this.Company.Length > 256)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Company, length must be less than 256.", new [] { "Company" });
            }

            // Company (string) minLength
            if(this.Company != null && this.Company.Length < 1)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Company, length must be greater than 1.", new [] { "Company" });
            }

            // ContactName (string) maxLength
            if(this.ContactName != null && this.ContactName.Length > 30)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for ContactName, length must be less than 30.", new [] { "ContactName" });
            }

            // ContactName (string) minLength
            if(this.ContactName != null && this.ContactName.Length < 1)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for ContactName, length must be greater than 1.", new [] { "ContactName" });
            }

            // Domain (string) maxLength
            if(this.Domain != null && this.Domain.Length > 256)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Domain, length must be less than 256.", new [] { "Domain" });
            }

            // Domain (string) minLength
            if(this.Domain != null && this.Domain.Length < 1)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Domain, length must be greater than 1.", new [] { "Domain" });
            }

            // Id (string) maxLength
            if(this.Id != null && this.Id.Length > 32)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Id, length must be less than 32.", new [] { "Id" });
            }

            // Id (string) minLength
            if(this.Id != null && this.Id.Length < 2)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Id, length must be greater than 2.", new [] { "Id" });
            }

            yield break;
        }
    }

}
