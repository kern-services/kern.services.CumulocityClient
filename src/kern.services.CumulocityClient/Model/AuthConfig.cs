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
    /// Parameters determining the authentication process.
    /// </summary>
    [DataContract(Name = "authConfig")]
    public partial class AuthConfig : IValidatableObject
    {
        /// <summary>
        /// The authentication configuration grant type identifier.
        /// </summary>
        /// <value>The authentication configuration grant type identifier.</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum GrantTypeEnum
        {
            /// <summary>
            /// Enum AUTHORIZATIONCODE for value: AUTHORIZATION_CODE
            /// </summary>
            [EnumMember(Value = "AUTHORIZATION_CODE")]
            AUTHORIZATIONCODE = 1,

            /// <summary>
            /// Enum PASSWORD for value: PASSWORD
            /// </summary>
            [EnumMember(Value = "PASSWORD")]
            PASSWORD = 2
        }


        /// <summary>
        /// The authentication configuration grant type identifier.
        /// </summary>
        /// <value>The authentication configuration grant type identifier.</value>
        [DataMember(Name = "grantType", EmitDefaultValue = false)]
        public GrantTypeEnum? GrantType { get; set; }
        /// <summary>
        /// The authentication configuration type. Note that the value is case insensitive.
        /// </summary>
        /// <value>The authentication configuration type. Note that the value is case insensitive.</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum TypeEnum
        {
            /// <summary>
            /// Enum BASIC for value: BASIC
            /// </summary>
            [EnumMember(Value = "BASIC")]
            BASIC = 1,

            /// <summary>
            /// Enum OAUTH2 for value: OAUTH2
            /// </summary>
            [EnumMember(Value = "OAUTH2")]
            OAUTH2 = 2,

            /// <summary>
            /// Enum OAUTH2INTERNAL for value: OAUTH2_INTERNAL
            /// </summary>
            [EnumMember(Value = "OAUTH2_INTERNAL")]
            OAUTH2INTERNAL = 3
        }


        /// <summary>
        /// The authentication configuration type. Note that the value is case insensitive.
        /// </summary>
        /// <value>The authentication configuration type. Note that the value is case insensitive.</value>
        [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
        public TypeEnum Type { get; set; }
        /// <summary>
        /// Indicates whether user data are managed internally by the Cumulocity IoT platform or by an external server. Note that the value is case insensitive.
        /// </summary>
        /// <value>Indicates whether user data are managed internally by the Cumulocity IoT platform or by an external server. Note that the value is case insensitive.</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum UserManagementSourceEnum
        {
            /// <summary>
            /// Enum INTERNAL for value: INTERNAL
            /// </summary>
            [EnumMember(Value = "INTERNAL")]
            INTERNAL = 1,

            /// <summary>
            /// Enum REMOTE for value: REMOTE
            /// </summary>
            [EnumMember(Value = "REMOTE")]
            REMOTE = 2
        }


        /// <summary>
        /// Indicates whether user data are managed internally by the Cumulocity IoT platform or by an external server. Note that the value is case insensitive.
        /// </summary>
        /// <value>Indicates whether user data are managed internally by the Cumulocity IoT platform or by an external server. Note that the value is case insensitive.</value>
        [DataMember(Name = "userManagementSource", EmitDefaultValue = false)]
        public UserManagementSourceEnum? UserManagementSource { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthConfig" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected AuthConfig() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthConfig" /> class.
        /// </summary>
        /// <param name="accessTokenToUserDataMapping">accessTokenToUserDataMapping.</param>
        /// <param name="audience">SSO specific. Token audience..</param>
        /// <param name="authorizationRequest">authorizationRequest.</param>
        /// <param name="authenticationRestrictions">authenticationRestrictions.</param>
        /// <param name="buttonName">SSO specific. Information for the UI about the name displayed on the external server login button..</param>
        /// <param name="clientId">SSO specific. The identifier of the Cumulocity IoT tenant on the external authorization server..</param>
        /// <param name="grantType">The authentication configuration grant type identifier..</param>
        /// <param name="id">Unique identifier of this login option..</param>
        /// <param name="issuer">SSO specific. External token issuer..</param>
        /// <param name="logoutRequest">logoutRequest.</param>
        /// <param name="onlyManagementTenantAccess">Indicates whether the configuration is only accessible to the management tenant..</param>
        /// <param name="onNewUser">onNewUser.</param>
        /// <param name="providerName">The name of the authentication provider. (required).</param>
        /// <param name="redirectToPlatform">SSO specific. URL used for redirecting to the Cumulocity IoT platform..</param>
        /// <param name="refreshRequest">refreshRequest.</param>
        /// <param name="sessionConfiguration">sessionConfiguration.</param>
        /// <param name="signatureVerificationConfig">signatureVerificationConfig.</param>
        /// <param name="template">SSO specific. Template name used by the UI..</param>
        /// <param name="tokenRequest">tokenRequest.</param>
        /// <param name="type">The authentication configuration type. Note that the value is case insensitive. (required).</param>
        /// <param name="userIdConfig">userIdConfig.</param>
        /// <param name="userManagementSource">Indicates whether user data are managed internally by the Cumulocity IoT platform or by an external server. Note that the value is case insensitive..</param>
        /// <param name="visibleOnLoginPage">Information for the UI if the respective authentication form should be visible for the user..</param>
        public AuthConfig(AuthConfigAccessTokenToUserDataMapping accessTokenToUserDataMapping = default(AuthConfigAccessTokenToUserDataMapping), string audience = default(string), AuthConfigAuthorizationRequest authorizationRequest = default(AuthConfigAuthorizationRequest), BasicAuthenticationRestrictions authenticationRestrictions = default(BasicAuthenticationRestrictions), string buttonName = default(string), string clientId = default(string), GrantTypeEnum? grantType = default(GrantTypeEnum?), string id = default(string), string issuer = default(string), AuthConfigLogoutRequest logoutRequest = default(AuthConfigLogoutRequest), bool onlyManagementTenantAccess = default(bool), AuthConfigOnNewUser onNewUser = default(AuthConfigOnNewUser), string providerName = default(string), string redirectToPlatform = default(string), AuthConfigRefreshRequest refreshRequest = default(AuthConfigRefreshRequest), OAuthSessionConfiguration sessionConfiguration = default(OAuthSessionConfiguration), AuthConfigSignatureVerificationConfig signatureVerificationConfig = default(AuthConfigSignatureVerificationConfig), string template = default(string), AuthConfigTokenRequest tokenRequest = default(AuthConfigTokenRequest), TypeEnum type = default(TypeEnum), AuthConfigUserIdConfig userIdConfig = default(AuthConfigUserIdConfig), UserManagementSourceEnum? userManagementSource = default(UserManagementSourceEnum?), bool visibleOnLoginPage = default(bool))
        {
            // to ensure "providerName" is required (not null)
            if (providerName == null)
            {
                throw new ArgumentNullException("providerName is a required property for AuthConfig and cannot be null");
            }
            this.ProviderName = providerName;
            this.Type = type;
            this.AccessTokenToUserDataMapping = accessTokenToUserDataMapping;
            this.Audience = audience;
            this.AuthorizationRequest = authorizationRequest;
            this.AuthenticationRestrictions = authenticationRestrictions;
            this.ButtonName = buttonName;
            this.ClientId = clientId;
            this.GrantType = grantType;
            this.Id = id;
            this.Issuer = issuer;
            this.LogoutRequest = logoutRequest;
            this.OnlyManagementTenantAccess = onlyManagementTenantAccess;
            this.OnNewUser = onNewUser;
            this.RedirectToPlatform = redirectToPlatform;
            this.RefreshRequest = refreshRequest;
            this.SessionConfiguration = sessionConfiguration;
            this.SignatureVerificationConfig = signatureVerificationConfig;
            this.Template = template;
            this.TokenRequest = tokenRequest;
            this.UserIdConfig = userIdConfig;
            this.UserManagementSource = userManagementSource;
            this.VisibleOnLoginPage = visibleOnLoginPage;
        }

        /// <summary>
        /// Gets or Sets AccessTokenToUserDataMapping
        /// </summary>
        [DataMember(Name = "accessTokenToUserDataMapping", EmitDefaultValue = false)]
        public AuthConfigAccessTokenToUserDataMapping AccessTokenToUserDataMapping { get; set; }

        /// <summary>
        /// SSO specific. Token audience.
        /// </summary>
        /// <value>SSO specific. Token audience.</value>
        [DataMember(Name = "audience", EmitDefaultValue = false)]
        public string Audience { get; set; }

        /// <summary>
        /// Gets or Sets AuthorizationRequest
        /// </summary>
        [DataMember(Name = "authorizationRequest", EmitDefaultValue = false)]
        public AuthConfigAuthorizationRequest AuthorizationRequest { get; set; }

        /// <summary>
        /// Gets or Sets AuthenticationRestrictions
        /// </summary>
        [DataMember(Name = "authenticationRestrictions", EmitDefaultValue = false)]
        public BasicAuthenticationRestrictions AuthenticationRestrictions { get; set; }

        /// <summary>
        /// SSO specific. Information for the UI about the name displayed on the external server login button.
        /// </summary>
        /// <value>SSO specific. Information for the UI about the name displayed on the external server login button.</value>
        [DataMember(Name = "buttonName", EmitDefaultValue = false)]
        public string ButtonName { get; set; }

        /// <summary>
        /// SSO specific. The identifier of the Cumulocity IoT tenant on the external authorization server.
        /// </summary>
        /// <value>SSO specific. The identifier of the Cumulocity IoT tenant on the external authorization server.</value>
        [DataMember(Name = "clientId", EmitDefaultValue = false)]
        public string ClientId { get; set; }

        /// <summary>
        /// Unique identifier of this login option.
        /// </summary>
        /// <value>Unique identifier of this login option.</value>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>
        /// SSO specific. External token issuer.
        /// </summary>
        /// <value>SSO specific. External token issuer.</value>
        [DataMember(Name = "issuer", EmitDefaultValue = false)]
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or Sets LogoutRequest
        /// </summary>
        [DataMember(Name = "logoutRequest", EmitDefaultValue = false)]
        public AuthConfigLogoutRequest LogoutRequest { get; set; }

        /// <summary>
        /// Indicates whether the configuration is only accessible to the management tenant.
        /// </summary>
        /// <value>Indicates whether the configuration is only accessible to the management tenant.</value>
        [DataMember(Name = "onlyManagementTenantAccess", EmitDefaultValue = true)]
        public bool OnlyManagementTenantAccess { get; set; }

        /// <summary>
        /// Gets or Sets OnNewUser
        /// </summary>
        [DataMember(Name = "onNewUser", EmitDefaultValue = false)]
        public AuthConfigOnNewUser OnNewUser { get; set; }

        /// <summary>
        /// The name of the authentication provider.
        /// </summary>
        /// <value>The name of the authentication provider.</value>
        [DataMember(Name = "providerName", IsRequired = true, EmitDefaultValue = true)]
        public string ProviderName { get; set; }

        /// <summary>
        /// SSO specific. URL used for redirecting to the Cumulocity IoT platform.
        /// </summary>
        /// <value>SSO specific. URL used for redirecting to the Cumulocity IoT platform.</value>
        [DataMember(Name = "redirectToPlatform", EmitDefaultValue = false)]
        public string RedirectToPlatform { get; set; }

        /// <summary>
        /// Gets or Sets RefreshRequest
        /// </summary>
        [DataMember(Name = "refreshRequest", EmitDefaultValue = false)]
        public AuthConfigRefreshRequest RefreshRequest { get; set; }

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
        /// Gets or Sets SessionConfiguration
        /// </summary>
        [DataMember(Name = "sessionConfiguration", EmitDefaultValue = false)]
        public OAuthSessionConfiguration SessionConfiguration { get; set; }

        /// <summary>
        /// Gets or Sets SignatureVerificationConfig
        /// </summary>
        [DataMember(Name = "signatureVerificationConfig", EmitDefaultValue = false)]
        public AuthConfigSignatureVerificationConfig SignatureVerificationConfig { get; set; }

        /// <summary>
        /// SSO specific. Template name used by the UI.
        /// </summary>
        /// <value>SSO specific. Template name used by the UI.</value>
        [DataMember(Name = "template", EmitDefaultValue = false)]
        public string Template { get; set; }

        /// <summary>
        /// Gets or Sets TokenRequest
        /// </summary>
        [DataMember(Name = "tokenRequest", EmitDefaultValue = false)]
        public AuthConfigTokenRequest TokenRequest { get; set; }

        /// <summary>
        /// Gets or Sets UserIdConfig
        /// </summary>
        [DataMember(Name = "userIdConfig", EmitDefaultValue = false)]
        public AuthConfigUserIdConfig UserIdConfig { get; set; }

        /// <summary>
        /// Information for the UI if the respective authentication form should be visible for the user.
        /// </summary>
        /// <value>Information for the UI if the respective authentication form should be visible for the user.</value>
        [DataMember(Name = "visibleOnLoginPage", EmitDefaultValue = true)]
        public bool VisibleOnLoginPage { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class AuthConfig {\n");
            sb.Append("  AccessTokenToUserDataMapping: ").Append(AccessTokenToUserDataMapping).Append("\n");
            sb.Append("  Audience: ").Append(Audience).Append("\n");
            sb.Append("  AuthorizationRequest: ").Append(AuthorizationRequest).Append("\n");
            sb.Append("  AuthenticationRestrictions: ").Append(AuthenticationRestrictions).Append("\n");
            sb.Append("  ButtonName: ").Append(ButtonName).Append("\n");
            sb.Append("  ClientId: ").Append(ClientId).Append("\n");
            sb.Append("  GrantType: ").Append(GrantType).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Issuer: ").Append(Issuer).Append("\n");
            sb.Append("  LogoutRequest: ").Append(LogoutRequest).Append("\n");
            sb.Append("  OnlyManagementTenantAccess: ").Append(OnlyManagementTenantAccess).Append("\n");
            sb.Append("  OnNewUser: ").Append(OnNewUser).Append("\n");
            sb.Append("  ProviderName: ").Append(ProviderName).Append("\n");
            sb.Append("  RedirectToPlatform: ").Append(RedirectToPlatform).Append("\n");
            sb.Append("  RefreshRequest: ").Append(RefreshRequest).Append("\n");
            sb.Append("  Self: ").Append(Self).Append("\n");
            sb.Append("  SessionConfiguration: ").Append(SessionConfiguration).Append("\n");
            sb.Append("  SignatureVerificationConfig: ").Append(SignatureVerificationConfig).Append("\n");
            sb.Append("  Template: ").Append(Template).Append("\n");
            sb.Append("  TokenRequest: ").Append(TokenRequest).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  UserIdConfig: ").Append(UserIdConfig).Append("\n");
            sb.Append("  UserManagementSource: ").Append(UserManagementSource).Append("\n");
            sb.Append("  VisibleOnLoginPage: ").Append(VisibleOnLoginPage).Append("\n");
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
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}
