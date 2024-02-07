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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serializers;
using RestSharpMethod = RestSharp.Method;
using Polly;
using kern.services.CumulocityClient.Client.Auth;

namespace kern.services.CumulocityClient.Client
{
    /// <summary>
    /// Allows RestSharp to Serialize/Deserialize JSON using our custom logic, but only when ContentType is JSON.
    /// </summary>
    internal class CustomJsonCodec : IRestSerializer, ISerializer, IDeserializer
    {
        private readonly IReadableConfiguration _configuration;
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            // OpenAPI generated types generally hide default constructors.
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    OverrideSpecifiedNames = false
                }
            }
        };

        public CustomJsonCodec(IReadableConfiguration configuration)
        {
            _configuration = configuration;
        }

        public CustomJsonCodec(JsonSerializerSettings serializerSettings, IReadableConfiguration configuration)
        {
            _serializerSettings = serializerSettings;
            _configuration = configuration;
        }

        /// <summary>
        /// Serialize the object into a JSON string.
        /// </summary>
        /// <param name="obj">Object to be serialized.</param>
        /// <returns>A JSON string.</returns>
        public string Serialize(object obj)
        {
            if (obj != null && obj is kern.services.CumulocityClient.Model.AbstractOpenAPISchema)
            {
                // the object to be serialized is an oneOf/anyOf schema
                return ((kern.services.CumulocityClient.Model.AbstractOpenAPISchema)obj).ToJson();
            }
            else
            {
                return JsonConvert.SerializeObject(obj, _serializerSettings);
            }
        }

        public string Serialize(Parameter bodyParameter) => Serialize(bodyParameter.Value);

        public T Deserialize<T>(RestResponse response)
        {
            var result = (T)Deserialize(response, typeof(T));
            return result;
        }

        /// <summary>
        /// Deserialize the JSON string into a proper object.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <param name="type">Object type.</param>
        /// <returns>Object representation of the JSON string.</returns>
        internal object Deserialize(RestResponse response, Type type)
        {
            if (type == typeof(byte[])) // return byte array
            {
                return response.RawBytes;
            }

            // TODO: ? if (type.IsAssignableFrom(typeof(Stream)))
            if (type == typeof(Stream))
            {
                var bytes = response.RawBytes;
                if (response.Headers != null)
                {
                    var filePath = string.IsNullOrEmpty(_configuration.TempFolderPath)
                        ? Path.GetTempPath()
                        : _configuration.TempFolderPath;
                    var regex = new Regex(@"Content-Disposition=.*filename=['""]?([^'""\s]+)['""]?$");
                    foreach (var header in response.Headers)
                    {
                        var match = regex.Match(header.ToString());
                        if (match.Success)
                        {
                            string fileName = filePath + ClientUtils.SanitizeFilename(match.Groups[1].Value.Replace("\"", "").Replace("'", ""));
                            File.WriteAllBytes(fileName, bytes);
                            return new FileStream(fileName, FileMode.Open);
                        }
                    }
                }
                var stream = new MemoryStream(bytes);
                return stream;
            }

            if (type.Name.StartsWith("System.Nullable`1[[System.DateTime")) // return a datetime object
            {
                return DateTime.Parse(response.Content, null, System.Globalization.DateTimeStyles.RoundtripKind);
            }

            if (type == typeof(string) || type.Name.StartsWith("System.Nullable")) // return primitive type
            {
                return Convert.ChangeType(response.Content, type);
            }

            // at this point, it must be a model (json)
            try
            {
                return JsonConvert.DeserializeObject(response.Content, type, _serializerSettings);
            }
            catch (Exception e)
            {
                throw new ApiException(500, e.Message);
            }
        }

        public ISerializer Serializer => this;
        public IDeserializer Deserializer => this;

        public string[] AcceptedContentTypes => RestSharp.ContentType.JsonAccept;

        public SupportsContentType SupportsContentType => contentType =>
            contentType.Value.EndsWith("json", StringComparison.InvariantCultureIgnoreCase) ||
            contentType.Value.EndsWith("javascript", StringComparison.InvariantCultureIgnoreCase);

        public ContentType ContentType { get; set; } = RestSharp.ContentType.Json;

        public DataFormat DataFormat => DataFormat.Json;
    }
    /// <summary>
    /// Provides a default implementation of an Api client (both synchronous and asynchronous implementations),
    /// encapsulating general REST accessor use cases.
    /// </summary>
    public partial class ApiClient : ISynchronousClient, IAsynchronousClient
    {
        private readonly string _baseUrl;

        /// <summary>
        /// Specifies the settings on a <see cref="JsonSerializer" /> object.
        /// These settings can be adjusted to accommodate custom serialization rules.
        /// </summary>
        public JsonSerializerSettings SerializerSettings { get; set; } = new JsonSerializerSettings
        {
            // OpenAPI generated types generally hide default constructors.
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    OverrideSpecifiedNames = false
                }
            }
        };

        /// <summary>
        /// Allows for extending request processing for <see cref="ApiClient"/> generated code.
        /// </summary>
        /// <param name="request">The RestSharp request object</param>
        partial void InterceptRequest(RestRequest request);

        /// <summary>
        /// Allows for extending response processing for <see cref="ApiClient"/> generated code.
        /// </summary>
        /// <param name="request">The RestSharp request object</param>
        /// <param name="response">The RestSharp response object</param>
        partial void InterceptResponse(RestRequest request, RestResponse response);

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient" />, defaulting to the global configurations' base url.
        /// </summary>
        public ApiClient()
        {
            _baseUrl = kern.services.CumulocityClient.Client.GlobalConfiguration.Instance.BasePath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient" />
        /// </summary>
        /// <param name="basePath">The target service's base path in URL format.</param>
        /// <exception cref="ArgumentException"></exception>
        public ApiClient(string basePath)
        {
            if (string.IsNullOrEmpty(basePath))
                throw new ArgumentException("basePath cannot be empty");

            _baseUrl = basePath;
        }

        /// <summary>
        /// Constructs the RestSharp version of an http method
        /// </summary>
        /// <param name="method">Swagger Client Custom HttpMethod</param>
        /// <returns>RestSharp's HttpMethod instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private RestSharpMethod Method(HttpMethod method)
        {
            RestSharpMethod other;
            switch (method)
            {
                case HttpMethod.Get:
                    other = RestSharpMethod.Get;
                    break;
                case HttpMethod.Post:
                    other = RestSharpMethod.Post;
                    break;
                case HttpMethod.Put:
                    other = RestSharpMethod.Put;
                    break;
                case HttpMethod.Delete:
                    other = RestSharpMethod.Delete;
                    break;
                case HttpMethod.Head:
                    other = RestSharpMethod.Head;
                    break;
                case HttpMethod.Options:
                    other = RestSharpMethod.Options;
                    break;
                case HttpMethod.Patch:
                    other = RestSharpMethod.Patch;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("method", method, null);
            }

            return other;
        }

        /// <summary>
        /// Provides all logic for constructing a new RestSharp <see cref="RestRequest"/>.
        /// At this point, all information for querying the service is known. Here, it is simply
        /// mapped into the RestSharp request.
        /// </summary>
        /// <param name="method">The http verb.</param>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <returns>[private] A new RestRequest instance.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private RestRequest NewRequest(
            HttpMethod method,
            string path,
            RequestOptions options,
            IReadableConfiguration configuration)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (options == null) throw new ArgumentNullException("options");
            if (configuration == null) throw new ArgumentNullException("configuration");

            RestRequest request = new RestRequest(path, Method(method));

            if (options.PathParameters != null)
            {
                foreach (var pathParam in options.PathParameters)
                {
                    request.AddParameter(pathParam.Key, pathParam.Value, ParameterType.UrlSegment);
                }
            }

            if (options.QueryParameters != null)
            {
                foreach (var queryParam in options.QueryParameters)
                {
                    foreach (var value in queryParam.Value)
                    {
                        request.AddQueryParameter(queryParam.Key, value);
                    }
                }
            }

            if (configuration.DefaultHeaders != null)
            {
                foreach (var headerParam in configuration.DefaultHeaders)
                {
                    request.AddHeader(headerParam.Key, headerParam.Value);
                }
            }

            if (options.HeaderParameters != null)
            {
                foreach (var headerParam in options.HeaderParameters)
                {
                    foreach (var value in headerParam.Value)
                    {
                        request.AddHeader(headerParam.Key, value);
                    }
                }
            }

            if (options.FormParameters != null)
            {
                foreach (var formParam in options.FormParameters)
                {
                    request.AddParameter(formParam.Key, formParam.Value);
                }
            }

            if (options.Data != null)
            {
                if (options.Data is Stream stream)
                {
                    var contentType = "application/octet-stream";
                    if (options.HeaderParameters != null)
                    {
                        var contentTypes = options.HeaderParameters["Content-Type"];
                        contentType = contentTypes[0];
                    }

                    var bytes = ClientUtils.ReadAsBytes(stream);
                    request.AddParameter(contentType, bytes, ParameterType.RequestBody);
                }
                else
                {
                    if (options.HeaderParameters != null)
                    {
                        var contentTypes = options.HeaderParameters["Content-Type"];
                        if (contentTypes == null || contentTypes.Any(header => header.Contains("application/json")))
                        {
                            request.RequestFormat = DataFormat.Json;
                        }
                        else
                        {
                            // TODO: Generated client user should add additional handlers. RestSharp only supports XML and JSON, with XML as default.
                        }
                    }
                    else
                    {
                        // Here, we'll assume JSON APIs are more common. XML can be forced by adding produces/consumes to openapi spec explicitly.
                        request.RequestFormat = DataFormat.Json;
                    }

                    request.AddJsonBody(options.Data);
                }
            }

            if (options.FileParameters != null)
            {
                foreach (var fileParam in options.FileParameters)
                {
                    foreach (var file in fileParam.Value)
                    {
                        var bytes = ClientUtils.ReadAsBytes(file);
                        var fileStream = file as FileStream;
                        if (fileStream != null)
                            request.AddFile(fileParam.Key, bytes, System.IO.Path.GetFileName(fileStream.Name));
                        else
                            request.AddFile(fileParam.Key, bytes, "no_file_name_provided");
                    }
                }
            }

            return request;
        }

        private ApiResponse<T> ToApiResponse<T>(RestResponse<T> response)
        {
            T result = response.Data;
            string rawContent = response.Content;

            var transformed = new ApiResponse<T>(response.StatusCode, new Multimap<string, string>(), result, rawContent)
            {
                ErrorText = response.ErrorMessage,
                Cookies = new List<Cookie>()
            };

            if (response.Headers != null)
            {
                foreach (var responseHeader in response.Headers)
                {
                    transformed.Headers.Add(responseHeader.Name, ClientUtils.ParameterToString(responseHeader.Value));
                }
            }

            if (response.ContentHeaders != null)
            {
                foreach (var responseHeader in response.ContentHeaders)
                {
                    transformed.Headers.Add(responseHeader.Name, ClientUtils.ParameterToString(responseHeader.Value));
                }
            }

            if (response.Cookies != null)
            {
                foreach (var responseCookies in response.Cookies.Cast<Cookie>())
                {
                    transformed.Cookies.Add(
                        new Cookie(
                            responseCookies.Name,
                            responseCookies.Value,
                            responseCookies.Path,
                            responseCookies.Domain)
                        );
                }
            }

            return transformed;
        }

        private ApiResponse<T> Exec<T>(RestRequest request, RequestOptions options, IReadableConfiguration configuration)
        {
            var baseUrl = configuration.GetOperationServerUrl(options.Operation, options.OperationIndex) ?? _baseUrl;

            var cookies = new CookieContainer();

            if (options.Cookies != null && options.Cookies.Count > 0)
            {
                foreach (var cookie in options.Cookies)
                {
                    cookies.Add(new Cookie(cookie.Name, cookie.Value));
                }
            }

            var clientOptions = new RestClientOptions(baseUrl)
            {
                ClientCertificates = configuration.ClientCertificates,
                CookieContainer = cookies,
                MaxTimeout = configuration.Timeout,
                Proxy = configuration.Proxy,
                UserAgent = configuration.UserAgent,
                UseDefaultCredentials = configuration.UseDefaultCredentials,
                RemoteCertificateValidationCallback = configuration.RemoteCertificateValidationCallback
            };

            if (!string.IsNullOrEmpty(configuration.OAuthTokenUrl) &&
                !string.IsNullOrEmpty(configuration.OAuthClientId) &&
                !string.IsNullOrEmpty(configuration.OAuthClientSecret) &&
                configuration.OAuthFlow != null)
            {
                clientOptions.Authenticator = new OAuthAuthenticator(
                    configuration.OAuthTokenUrl,
                    configuration.OAuthClientId,
                    configuration.OAuthClientSecret,
                    configuration.OAuthFlow,
                    SerializerSettings,
                    configuration);
            }

            using (RestClient client = new RestClient(clientOptions,
                configureSerialization: serializerConfig => serializerConfig.UseSerializer(() => new CustomJsonCodec(SerializerSettings, configuration))))
            {
                InterceptRequest(request);

                RestResponse<T> response;
                if (RetryConfiguration.RetryPolicy != null)
                {
                    var policy = RetryConfiguration.RetryPolicy;
                    var policyResult = policy.ExecuteAndCapture(() => client.Execute(request));
                    response = (policyResult.Outcome == OutcomeType.Successful) ? client.Deserialize<T>(policyResult.Result) : new RestResponse<T>(request)
                    {
                        ErrorException = policyResult.FinalException
                    };
                }
                else
                {
                    response = client.Execute<T>(request);
                }

                // if the response type is oneOf/anyOf, call FromJSON to deserialize the data
                if (typeof(kern.services.CumulocityClient.Model.AbstractOpenAPISchema).IsAssignableFrom(typeof(T)))
                {
                    try
                    {
                        response.Data = (T) typeof(T).GetMethod("FromJson").Invoke(null, new object[] { response.Content });
                    }
                    catch (Exception ex)
                    {
                        throw ex.InnerException != null ? ex.InnerException : ex;
                    }
                }
                else if (typeof(T).Name == "Stream") // for binary response
                {
                    response.Data = (T)(object)new MemoryStream(response.RawBytes);
                }
                else if (typeof(T).Name == "Byte[]") // for byte response
                {
                    response.Data = (T)(object)response.RawBytes;
                }
                else if (typeof(T).Name == "String") // for string response
                {
                    response.Data = (T)(object)response.Content;
                }

                InterceptResponse(request, response);

                var result = ToApiResponse(response);
                if (response.ErrorMessage != null)
                {
                    result.ErrorText = response.ErrorMessage;
                }

                if (response.Cookies != null && response.Cookies.Count > 0)
                {
                    if (result.Cookies == null) result.Cookies = new List<Cookie>();
                    foreach (var restResponseCookie in response.Cookies.Cast<Cookie>())
                    {
                        var cookie = new Cookie(
                            restResponseCookie.Name,
                            restResponseCookie.Value,
                            restResponseCookie.Path,
                            restResponseCookie.Domain
                        )
                        {
                            Comment = restResponseCookie.Comment,
                            CommentUri = restResponseCookie.CommentUri,
                            Discard = restResponseCookie.Discard,
                            Expired = restResponseCookie.Expired,
                            Expires = restResponseCookie.Expires,
                            HttpOnly = restResponseCookie.HttpOnly,
                            Port = restResponseCookie.Port,
                            Secure = restResponseCookie.Secure,
                            Version = restResponseCookie.Version
                        };

                        result.Cookies.Add(cookie);
                    }
                }
                return result;
            }
        }

        private async Task<ApiResponse<T>> ExecAsync<T>(RestRequest request, RequestOptions options, IReadableConfiguration configuration, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var baseUrl = configuration.GetOperationServerUrl(options.Operation, options.OperationIndex) ?? _baseUrl;

            var clientOptions = new RestClientOptions(baseUrl)
            {
                ClientCertificates = configuration.ClientCertificates,
                MaxTimeout = configuration.Timeout,
                Proxy = configuration.Proxy,
                UserAgent = configuration.UserAgent,
                UseDefaultCredentials = configuration.UseDefaultCredentials,
                RemoteCertificateValidationCallback = configuration.RemoteCertificateValidationCallback
            };

            if (!string.IsNullOrEmpty(configuration.OAuthTokenUrl) &&
                !string.IsNullOrEmpty(configuration.OAuthClientId) &&
                !string.IsNullOrEmpty(configuration.OAuthClientSecret) &&
                configuration.OAuthFlow != null)
            {
                clientOptions.Authenticator = new OAuthAuthenticator(
                    configuration.OAuthTokenUrl,
                    configuration.OAuthClientId,
                    configuration.OAuthClientSecret,
                    configuration.OAuthFlow,
                    SerializerSettings,
                    configuration);
            }

            using (RestClient client = new RestClient(clientOptions,
                configureSerialization: serializerConfig => serializerConfig.UseSerializer(() => new CustomJsonCodec(SerializerSettings, configuration))))
            {
                InterceptRequest(request);

                RestResponse<T> response;
                if (RetryConfiguration.AsyncRetryPolicy != null)
                {
                    var policy = RetryConfiguration.AsyncRetryPolicy;
                    var policyResult = await policy.ExecuteAndCaptureAsync((ct) => client.ExecuteAsync(request, ct), cancellationToken).ConfigureAwait(false);
                    response = (policyResult.Outcome == OutcomeType.Successful) ? client.Deserialize<T>(policyResult.Result) : new RestResponse<T>(request)
                    {
                        ErrorException = policyResult.FinalException
                    };
                }
                else
                {
                    response = await client.ExecuteAsync<T>(request, cancellationToken).ConfigureAwait(false);
                }

                // if the response type is oneOf/anyOf, call FromJSON to deserialize the data
                if (typeof(kern.services.CumulocityClient.Model.AbstractOpenAPISchema).IsAssignableFrom(typeof(T)))
                {
                    response.Data = (T) typeof(T).GetMethod("FromJson").Invoke(null, new object[] { response.Content });
                }
                else if (typeof(T).Name == "Stream") // for binary response
                {
                    response.Data = (T)(object)new MemoryStream(response.RawBytes);
                }
                else if (typeof(T).Name == "Byte[]") // for byte response
                {
                    response.Data = (T)(object)response.RawBytes;
                }

                InterceptResponse(request, response);

                var result = ToApiResponse(response);
                if (response.ErrorMessage != null)
                {
                    result.ErrorText = response.ErrorMessage;
                }

                if (response.Cookies != null && response.Cookies.Count > 0)
                {
                    if (result.Cookies == null) result.Cookies = new List<Cookie>();
                    foreach (var restResponseCookie in response.Cookies.Cast<Cookie>())
                    {
                        var cookie = new Cookie(
                            restResponseCookie.Name,
                            restResponseCookie.Value,
                            restResponseCookie.Path,
                            restResponseCookie.Domain
                        )
                        {
                            Comment = restResponseCookie.Comment,
                            CommentUri = restResponseCookie.CommentUri,
                            Discard = restResponseCookie.Discard,
                            Expired = restResponseCookie.Expired,
                            Expires = restResponseCookie.Expires,
                            HttpOnly = restResponseCookie.HttpOnly,
                            Port = restResponseCookie.Port,
                            Secure = restResponseCookie.Secure,
                            Version = restResponseCookie.Version
                        };

                        result.Cookies.Add(cookie);
                    }
                }
                return result;
            }
        }

        #region IAsynchronousClient
        /// <summary>
        /// Make a HTTP GET request (async).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public Task<ApiResponse<T>> GetAsync<T>(string path, RequestOptions options, IReadableConfiguration configuration = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return ExecAsync<T>(NewRequest(HttpMethod.Get, path, options, config), options, config, cancellationToken);
        }

        /// <summary>
        /// Make a HTTP POST request (async).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public Task<ApiResponse<T>> PostAsync<T>(string path, RequestOptions options, IReadableConfiguration configuration = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return ExecAsync<T>(NewRequest(HttpMethod.Post, path, options, config), options, config, cancellationToken);
        }

        /// <summary>
        /// Make a HTTP PUT request (async).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public Task<ApiResponse<T>> PutAsync<T>(string path, RequestOptions options, IReadableConfiguration configuration = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return ExecAsync<T>(NewRequest(HttpMethod.Put, path, options, config), options, config, cancellationToken);
        }

        /// <summary>
        /// Make a HTTP DELETE request (async).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public Task<ApiResponse<T>> DeleteAsync<T>(string path, RequestOptions options, IReadableConfiguration configuration = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return ExecAsync<T>(NewRequest(HttpMethod.Delete, path, options, config), options, config, cancellationToken);
        }

        /// <summary>
        /// Make a HTTP HEAD request (async).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public Task<ApiResponse<T>> HeadAsync<T>(string path, RequestOptions options, IReadableConfiguration configuration = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return ExecAsync<T>(NewRequest(HttpMethod.Head, path, options, config), options, config, cancellationToken);
        }

        /// <summary>
        /// Make a HTTP OPTION request (async).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public Task<ApiResponse<T>> OptionsAsync<T>(string path, RequestOptions options, IReadableConfiguration configuration = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return ExecAsync<T>(NewRequest(HttpMethod.Options, path, options, config), options, config, cancellationToken);
        }

        /// <summary>
        /// Make a HTTP PATCH request (async).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public Task<ApiResponse<T>> PatchAsync<T>(string path, RequestOptions options, IReadableConfiguration configuration = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return ExecAsync<T>(NewRequest(HttpMethod.Patch, path, options, config), options, config, cancellationToken);
        }
        #endregion IAsynchronousClient

        #region ISynchronousClient
        /// <summary>
        /// Make a HTTP GET request (synchronous).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public ApiResponse<T> Get<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return Exec<T>(NewRequest(HttpMethod.Get, path, options, config), options, config);
        }

        /// <summary>
        /// Make a HTTP POST request (synchronous).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public ApiResponse<T> Post<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return Exec<T>(NewRequest(HttpMethod.Post, path, options, config), options, config);
        }

        /// <summary>
        /// Make a HTTP PUT request (synchronous).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public ApiResponse<T> Put<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return Exec<T>(NewRequest(HttpMethod.Put, path, options, config), options, config);
        }

        /// <summary>
        /// Make a HTTP DELETE request (synchronous).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public ApiResponse<T> Delete<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return Exec<T>(NewRequest(HttpMethod.Delete, path, options, config), options, config);
        }

        /// <summary>
        /// Make a HTTP HEAD request (synchronous).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public ApiResponse<T> Head<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return Exec<T>(NewRequest(HttpMethod.Head, path, options, config), options, config);
        }

        /// <summary>
        /// Make a HTTP OPTION request (synchronous).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public ApiResponse<T> Options<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return Exec<T>(NewRequest(HttpMethod.Options, path, options, config), options, config);
        }

        /// <summary>
        /// Make a HTTP PATCH request (synchronous).
        /// </summary>
        /// <param name="path">The target path (or resource).</param>
        /// <param name="options">The additional request options.</param>
        /// <param name="configuration">A per-request configuration object. It is assumed that any merge with
        /// GlobalConfiguration has been done before calling this method.</param>
        /// <returns>A Task containing ApiResponse</returns>
        public ApiResponse<T> Patch<T>(string path, RequestOptions options, IReadableConfiguration configuration = null)
        {
            var config = configuration ?? GlobalConfiguration.Instance;
            return Exec<T>(NewRequest(HttpMethod.Patch, path, options, config), options, config);
        }
        #endregion ISynchronousClient
    }
}
