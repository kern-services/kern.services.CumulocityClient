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
    public interface IApplicationsApi : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Delete an application
        /// </summary>
        /// <remarks>
        /// Delete an application (by a given ID). This method is not supported by microservice applications.  > **&#9432; Info:** With regards to a hosted application, there is a caching mechanism in place that keeps the information about the placement of application files (html, javascript, css, fonts, etc.). Removing a hosted application, in normal circumstances, will cause the subsequent requests for application files to fail with an HTTP 404 error because the application is removed synchronously, its files are immediately removed on the node serving the request and at the same time the information is propagated to other nodes – but in rare cases there might be a delay with this propagation. In such situations, the files of the removed application can be served from those nodes up until the aforementioned cache expires. For the same reason, the cache can also cause HTTP 404 errors when the application is updated as it will keep the path to the files of the old version of the application. The cache is filled on demand, so there should not be issues if application files were not accessed prior to the delete request. The expiration delay of the cache can differ, but should not take more than one minute.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN <b>AND</b> tenant is the owner of the application </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="force">Force deletion by unsubscribing all tenants from the application first and then deleting the application itself. (optional, default to false)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns></returns>
        void DeleteApplicationResource (string id, bool? force = default(bool?), string xCumulocityProcessingMode = default(string));

        /// <summary>
        /// Delete an application
        /// </summary>
        /// <remarks>
        /// Delete an application (by a given ID). This method is not supported by microservice applications.  > **&#9432; Info:** With regards to a hosted application, there is a caching mechanism in place that keeps the information about the placement of application files (html, javascript, css, fonts, etc.). Removing a hosted application, in normal circumstances, will cause the subsequent requests for application files to fail with an HTTP 404 error because the application is removed synchronously, its files are immediately removed on the node serving the request and at the same time the information is propagated to other nodes – but in rare cases there might be a delay with this propagation. In such situations, the files of the removed application can be served from those nodes up until the aforementioned cache expires. For the same reason, the cache can also cause HTTP 404 errors when the application is updated as it will keep the path to the files of the old version of the application. The cache is filled on demand, so there should not be issues if application files were not accessed prior to the delete request. The expiration delay of the cache can differ, but should not take more than one minute.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN <b>AND</b> tenant is the owner of the application </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="force">Force deletion by unsubscribing all tenants from the application first and then deleting the application itself. (optional, default to false)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> DeleteApplicationResourceWithHttpInfo (string id, bool? force = default(bool?), string xCumulocityProcessingMode = default(string));
        /// <summary>
        /// Retrieve all applications
        /// </summary>
        /// <remarks>
        /// Retrieve all applications on your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="name">The name of the application. (optional)</param>
        /// <param name="owner">The ID of the tenant that owns the applications. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="providedFor">The ID of a tenant that is subscribed to the applications but doesn&#39;t own them. (optional)</param>
        /// <param name="subscriber">The ID of a tenant that is subscribed to the applications. (optional)</param>
        /// <param name="tenant">The ID of a tenant that either owns the application or is subscribed to the applications. (optional)</param>
        /// <param name="type">The type of the application. It is possible to use multiple values separated by a comma. For example, &#x60;EXTERNAL,HOSTED&#x60; will return only applications with type &#x60;EXTERNAL&#x60; or &#x60;HOSTED&#x60;. (optional)</param>
        /// <param name="user">The ID of a user that has access to the applications. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="hasVersions">When set to &#x60;true&#x60;, the returned result contains applications with an &#x60;applicationVersions&#x60; field that is not empty. When set to &#x60;false&#x60;, the result will contain applications with an empty &#x60;applicationVersions&#x60; field. (optional)</param>
        /// <returns>ApplicationCollection</returns>
        ApplicationCollection GetAbstractApplicationCollectionResource (int? currentPage = default(int?), string name = default(string), string owner = default(string), int? pageSize = default(int?), string providedFor = default(string), string subscriber = default(string), string tenant = default(string), string type = default(string), string user = default(string), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), bool? hasVersions = default(bool?));

        /// <summary>
        /// Retrieve all applications
        /// </summary>
        /// <remarks>
        /// Retrieve all applications on your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="name">The name of the application. (optional)</param>
        /// <param name="owner">The ID of the tenant that owns the applications. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="providedFor">The ID of a tenant that is subscribed to the applications but doesn&#39;t own them. (optional)</param>
        /// <param name="subscriber">The ID of a tenant that is subscribed to the applications. (optional)</param>
        /// <param name="tenant">The ID of a tenant that either owns the application or is subscribed to the applications. (optional)</param>
        /// <param name="type">The type of the application. It is possible to use multiple values separated by a comma. For example, &#x60;EXTERNAL,HOSTED&#x60; will return only applications with type &#x60;EXTERNAL&#x60; or &#x60;HOSTED&#x60;. (optional)</param>
        /// <param name="user">The ID of a user that has access to the applications. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="hasVersions">When set to &#x60;true&#x60;, the returned result contains applications with an &#x60;applicationVersions&#x60; field that is not empty. When set to &#x60;false&#x60;, the result will contain applications with an empty &#x60;applicationVersions&#x60; field. (optional)</param>
        /// <returns>ApiResponse of ApplicationCollection</returns>
        ApiResponse<ApplicationCollection> GetAbstractApplicationCollectionResourceWithHttpInfo (int? currentPage = default(int?), string name = default(string), string owner = default(string), int? pageSize = default(int?), string providedFor = default(string), string subscriber = default(string), string tenant = default(string), string type = default(string), string user = default(string), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), bool? hasVersions = default(bool?));
        /// <summary>
        /// Retrieve a specific application
        /// </summary>
        /// <remarks>
        /// Retrieve a specific application (by a given ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ <b>OR</b> current user has explicit access to the application </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <returns>Application</returns>
        Application GetApplicationResource (string id);

        /// <summary>
        /// Retrieve a specific application
        /// </summary>
        /// <remarks>
        /// Retrieve a specific application (by a given ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ <b>OR</b> current user has explicit access to the application </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <returns>ApiResponse of Application</returns>
        ApiResponse<Application> GetApplicationResourceWithHttpInfo (string id);
        /// <summary>
        /// Retrieve applications by name
        /// </summary>
        /// <remarks>
        /// Retrieve applications by name.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="name">The name of the application.</param>
        /// <returns>GetApplicationsByNameCollectionResource200Response</returns>
        GetApplicationsByNameCollectionResource200Response GetApplicationsByNameCollectionResource (string name);

        /// <summary>
        /// Retrieve applications by name
        /// </summary>
        /// <remarks>
        /// Retrieve applications by name.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="name">The name of the application.</param>
        /// <returns>ApiResponse of GetApplicationsByNameCollectionResource200Response</returns>
        ApiResponse<GetApplicationsByNameCollectionResource200Response> GetApplicationsByNameCollectionResourceWithHttpInfo (string name);
        /// <summary>
        /// Retrieve applications by owner
        /// </summary>
        /// <remarks>
        /// Retrieve all applications owned by a particular tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <returns>GetApplicationsByOwnerCollectionResource200Response</returns>
        GetApplicationsByOwnerCollectionResource200Response GetApplicationsByOwnerCollectionResource (string tenantId, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?));

        /// <summary>
        /// Retrieve applications by owner
        /// </summary>
        /// <remarks>
        /// Retrieve all applications owned by a particular tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <returns>ApiResponse of GetApplicationsByOwnerCollectionResource200Response</returns>
        ApiResponse<GetApplicationsByOwnerCollectionResource200Response> GetApplicationsByOwnerCollectionResourceWithHttpInfo (string tenantId, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?));
        /// <summary>
        /// Retrieve applications by tenant
        /// </summary>
        /// <remarks>
        /// Retrieve applications subscribed or owned by a particular tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <returns>GetApplicationsByTenantCollectionResource200Response</returns>
        GetApplicationsByTenantCollectionResource200Response GetApplicationsByTenantCollectionResource (string tenantId);

        /// <summary>
        /// Retrieve applications by tenant
        /// </summary>
        /// <remarks>
        /// Retrieve applications subscribed or owned by a particular tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <returns>ApiResponse of GetApplicationsByTenantCollectionResource200Response</returns>
        ApiResponse<GetApplicationsByTenantCollectionResource200Response> GetApplicationsByTenantCollectionResourceWithHttpInfo (string tenantId);
        /// <summary>
        /// Retrieve applications by user
        /// </summary>
        /// <remarks>
        /// Retrieve all applications for a particular user (by a given username).  <section><h5>Required roles</h5> (ROLE_USER_MANAGEMENT_OWN_READ <b>AND</b> is the current user) <b>OR</b> (ROLE_USER_MANAGEMENT_READ <b>AND</b> ROLE_APPLICATION_MANAGEMENT_READ) </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="username">The username of the a user.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <returns>GetApplicationsByUserCollectionResource200Response</returns>
        GetApplicationsByUserCollectionResource200Response GetApplicationsByUserCollectionResource (string username, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?));

        /// <summary>
        /// Retrieve applications by user
        /// </summary>
        /// <remarks>
        /// Retrieve all applications for a particular user (by a given username).  <section><h5>Required roles</h5> (ROLE_USER_MANAGEMENT_OWN_READ <b>AND</b> is the current user) <b>OR</b> (ROLE_USER_MANAGEMENT_READ <b>AND</b> ROLE_APPLICATION_MANAGEMENT_READ) </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="username">The username of the a user.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <returns>ApiResponse of GetApplicationsByUserCollectionResource200Response</returns>
        ApiResponse<GetApplicationsByUserCollectionResource200Response> GetApplicationsByUserCollectionResourceWithHttpInfo (string username, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?));
        /// <summary>
        /// Create an application
        /// </summary>
        /// <remarks>
        /// Create an application on your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postApplicationCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>Application</returns>
        Application PostApplicationCollectionResource (PostApplicationCollectionResourceRequest postApplicationCollectionResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string));

        /// <summary>
        /// Create an application
        /// </summary>
        /// <remarks>
        /// Create an application on your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postApplicationCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ApiResponse of Application</returns>
        ApiResponse<Application> PostApplicationCollectionResourceWithHttpInfo (PostApplicationCollectionResourceRequest postApplicationCollectionResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string));
        /// <summary>
        /// Copy an application
        /// </summary>
        /// <remarks>
        /// Copy an application (by a given ID).  This method is not supported by microservice applications.  A request to the \"clone\" resource creates a new application based on an already existing one.  The properties are copied to the newly created application and the prefix \"clone\" is added to the properties `name`, `key` and `contextPath` in order to be unique.  If the target application is hosted and has an active version, the new application will have the active version with the same content. <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>Application</returns>
        Application PostApplicationResource (string id, string accept = default(string), string xCumulocityProcessingMode = default(string));

        /// <summary>
        /// Copy an application
        /// </summary>
        /// <remarks>
        /// Copy an application (by a given ID).  This method is not supported by microservice applications.  A request to the \"clone\" resource creates a new application based on an already existing one.  The properties are copied to the newly created application and the prefix \"clone\" is added to the properties `name`, `key` and `contextPath` in order to be unique.  If the target application is hosted and has an active version, the new application will have the active version with the same content. <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ApiResponse of Application</returns>
        ApiResponse<Application> PostApplicationResourceWithHttpInfo (string id, string accept = default(string), string xCumulocityProcessingMode = default(string));
        /// <summary>
        /// Update a specific application
        /// </summary>
        /// <remarks>
        /// Update a specific application (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="putApplicationResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>Application</returns>
        Application PutApplicationResource (string id, PutApplicationResourceRequest putApplicationResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string));

        /// <summary>
        /// Update a specific application
        /// </summary>
        /// <remarks>
        /// Update a specific application (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="putApplicationResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ApiResponse of Application</returns>
        ApiResponse<Application> PutApplicationResourceWithHttpInfo (string id, PutApplicationResourceRequest putApplicationResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string));
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Delete an application
        /// </summary>
        /// <remarks>
        /// Delete an application (by a given ID). This method is not supported by microservice applications.  > **&#9432; Info:** With regards to a hosted application, there is a caching mechanism in place that keeps the information about the placement of application files (html, javascript, css, fonts, etc.). Removing a hosted application, in normal circumstances, will cause the subsequent requests for application files to fail with an HTTP 404 error because the application is removed synchronously, its files are immediately removed on the node serving the request and at the same time the information is propagated to other nodes – but in rare cases there might be a delay with this propagation. In such situations, the files of the removed application can be served from those nodes up until the aforementioned cache expires. For the same reason, the cache can also cause HTTP 404 errors when the application is updated as it will keep the path to the files of the old version of the application. The cache is filled on demand, so there should not be issues if application files were not accessed prior to the delete request. The expiration delay of the cache can differ, but should not take more than one minute.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN <b>AND</b> tenant is the owner of the application </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="force">Force deletion by unsubscribing all tenants from the application first and then deleting the application itself. (optional, default to false)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task DeleteApplicationResourceAsync (string id, bool? force = default(bool?), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete an application
        /// </summary>
        /// <remarks>
        /// Delete an application (by a given ID). This method is not supported by microservice applications.  > **&#9432; Info:** With regards to a hosted application, there is a caching mechanism in place that keeps the information about the placement of application files (html, javascript, css, fonts, etc.). Removing a hosted application, in normal circumstances, will cause the subsequent requests for application files to fail with an HTTP 404 error because the application is removed synchronously, its files are immediately removed on the node serving the request and at the same time the information is propagated to other nodes – but in rare cases there might be a delay with this propagation. In such situations, the files of the removed application can be served from those nodes up until the aforementioned cache expires. For the same reason, the cache can also cause HTTP 404 errors when the application is updated as it will keep the path to the files of the old version of the application. The cache is filled on demand, so there should not be issues if application files were not accessed prior to the delete request. The expiration delay of the cache can differ, but should not take more than one minute.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN <b>AND</b> tenant is the owner of the application </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="force">Force deletion by unsubscribing all tenants from the application first and then deleting the application itself. (optional, default to false)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> DeleteApplicationResourceWithHttpInfoAsync (string id, bool? force = default(bool?), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Retrieve all applications
        /// </summary>
        /// <remarks>
        /// Retrieve all applications on your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="name">The name of the application. (optional)</param>
        /// <param name="owner">The ID of the tenant that owns the applications. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="providedFor">The ID of a tenant that is subscribed to the applications but doesn&#39;t own them. (optional)</param>
        /// <param name="subscriber">The ID of a tenant that is subscribed to the applications. (optional)</param>
        /// <param name="tenant">The ID of a tenant that either owns the application or is subscribed to the applications. (optional)</param>
        /// <param name="type">The type of the application. It is possible to use multiple values separated by a comma. For example, &#x60;EXTERNAL,HOSTED&#x60; will return only applications with type &#x60;EXTERNAL&#x60; or &#x60;HOSTED&#x60;. (optional)</param>
        /// <param name="user">The ID of a user that has access to the applications. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="hasVersions">When set to &#x60;true&#x60;, the returned result contains applications with an &#x60;applicationVersions&#x60; field that is not empty. When set to &#x60;false&#x60;, the result will contain applications with an empty &#x60;applicationVersions&#x60; field. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApplicationCollection</returns>
        System.Threading.Tasks.Task<ApplicationCollection> GetAbstractApplicationCollectionResourceAsync (int? currentPage = default(int?), string name = default(string), string owner = default(string), int? pageSize = default(int?), string providedFor = default(string), string subscriber = default(string), string tenant = default(string), string type = default(string), string user = default(string), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), bool? hasVersions = default(bool?), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieve all applications
        /// </summary>
        /// <remarks>
        /// Retrieve all applications on your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="name">The name of the application. (optional)</param>
        /// <param name="owner">The ID of the tenant that owns the applications. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="providedFor">The ID of a tenant that is subscribed to the applications but doesn&#39;t own them. (optional)</param>
        /// <param name="subscriber">The ID of a tenant that is subscribed to the applications. (optional)</param>
        /// <param name="tenant">The ID of a tenant that either owns the application or is subscribed to the applications. (optional)</param>
        /// <param name="type">The type of the application. It is possible to use multiple values separated by a comma. For example, &#x60;EXTERNAL,HOSTED&#x60; will return only applications with type &#x60;EXTERNAL&#x60; or &#x60;HOSTED&#x60;. (optional)</param>
        /// <param name="user">The ID of a user that has access to the applications. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="hasVersions">When set to &#x60;true&#x60;, the returned result contains applications with an &#x60;applicationVersions&#x60; field that is not empty. When set to &#x60;false&#x60;, the result will contain applications with an empty &#x60;applicationVersions&#x60; field. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (ApplicationCollection)</returns>
        System.Threading.Tasks.Task<ApiResponse<ApplicationCollection>> GetAbstractApplicationCollectionResourceWithHttpInfoAsync (int? currentPage = default(int?), string name = default(string), string owner = default(string), int? pageSize = default(int?), string providedFor = default(string), string subscriber = default(string), string tenant = default(string), string type = default(string), string user = default(string), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), bool? hasVersions = default(bool?), CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Retrieve a specific application
        /// </summary>
        /// <remarks>
        /// Retrieve a specific application (by a given ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ <b>OR</b> current user has explicit access to the application </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of Application</returns>
        System.Threading.Tasks.Task<Application> GetApplicationResourceAsync (string id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieve a specific application
        /// </summary>
        /// <remarks>
        /// Retrieve a specific application (by a given ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ <b>OR</b> current user has explicit access to the application </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (Application)</returns>
        System.Threading.Tasks.Task<ApiResponse<Application>> GetApplicationResourceWithHttpInfoAsync (string id, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Retrieve applications by name
        /// </summary>
        /// <remarks>
        /// Retrieve applications by name.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="name">The name of the application.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of GetApplicationsByNameCollectionResource200Response</returns>
        System.Threading.Tasks.Task<GetApplicationsByNameCollectionResource200Response> GetApplicationsByNameCollectionResourceAsync (string name, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieve applications by name
        /// </summary>
        /// <remarks>
        /// Retrieve applications by name.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="name">The name of the application.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (GetApplicationsByNameCollectionResource200Response)</returns>
        System.Threading.Tasks.Task<ApiResponse<GetApplicationsByNameCollectionResource200Response>> GetApplicationsByNameCollectionResourceWithHttpInfoAsync (string name, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Retrieve applications by owner
        /// </summary>
        /// <remarks>
        /// Retrieve all applications owned by a particular tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of GetApplicationsByOwnerCollectionResource200Response</returns>
        System.Threading.Tasks.Task<GetApplicationsByOwnerCollectionResource200Response> GetApplicationsByOwnerCollectionResourceAsync (string tenantId, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieve applications by owner
        /// </summary>
        /// <remarks>
        /// Retrieve all applications owned by a particular tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (GetApplicationsByOwnerCollectionResource200Response)</returns>
        System.Threading.Tasks.Task<ApiResponse<GetApplicationsByOwnerCollectionResource200Response>> GetApplicationsByOwnerCollectionResourceWithHttpInfoAsync (string tenantId, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Retrieve applications by tenant
        /// </summary>
        /// <remarks>
        /// Retrieve applications subscribed or owned by a particular tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of GetApplicationsByTenantCollectionResource200Response</returns>
        System.Threading.Tasks.Task<GetApplicationsByTenantCollectionResource200Response> GetApplicationsByTenantCollectionResourceAsync (string tenantId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieve applications by tenant
        /// </summary>
        /// <remarks>
        /// Retrieve applications subscribed or owned by a particular tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (GetApplicationsByTenantCollectionResource200Response)</returns>
        System.Threading.Tasks.Task<ApiResponse<GetApplicationsByTenantCollectionResource200Response>> GetApplicationsByTenantCollectionResourceWithHttpInfoAsync (string tenantId, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Retrieve applications by user
        /// </summary>
        /// <remarks>
        /// Retrieve all applications for a particular user (by a given username).  <section><h5>Required roles</h5> (ROLE_USER_MANAGEMENT_OWN_READ <b>AND</b> is the current user) <b>OR</b> (ROLE_USER_MANAGEMENT_READ <b>AND</b> ROLE_APPLICATION_MANAGEMENT_READ) </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="username">The username of the a user.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of GetApplicationsByUserCollectionResource200Response</returns>
        System.Threading.Tasks.Task<GetApplicationsByUserCollectionResource200Response> GetApplicationsByUserCollectionResourceAsync (string username, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieve applications by user
        /// </summary>
        /// <remarks>
        /// Retrieve all applications for a particular user (by a given username).  <section><h5>Required roles</h5> (ROLE_USER_MANAGEMENT_OWN_READ <b>AND</b> is the current user) <b>OR</b> (ROLE_USER_MANAGEMENT_READ <b>AND</b> ROLE_APPLICATION_MANAGEMENT_READ) </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="username">The username of the a user.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (GetApplicationsByUserCollectionResource200Response)</returns>
        System.Threading.Tasks.Task<ApiResponse<GetApplicationsByUserCollectionResource200Response>> GetApplicationsByUserCollectionResourceWithHttpInfoAsync (string username, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Create an application
        /// </summary>
        /// <remarks>
        /// Create an application on your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postApplicationCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of Application</returns>
        System.Threading.Tasks.Task<Application> PostApplicationCollectionResourceAsync (PostApplicationCollectionResourceRequest postApplicationCollectionResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Create an application
        /// </summary>
        /// <remarks>
        /// Create an application on your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postApplicationCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (Application)</returns>
        System.Threading.Tasks.Task<ApiResponse<Application>> PostApplicationCollectionResourceWithHttpInfoAsync (PostApplicationCollectionResourceRequest postApplicationCollectionResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Copy an application
        /// </summary>
        /// <remarks>
        /// Copy an application (by a given ID).  This method is not supported by microservice applications.  A request to the \"clone\" resource creates a new application based on an already existing one.  The properties are copied to the newly created application and the prefix \"clone\" is added to the properties `name`, `key` and `contextPath` in order to be unique.  If the target application is hosted and has an active version, the new application will have the active version with the same content. <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of Application</returns>
        System.Threading.Tasks.Task<Application> PostApplicationResourceAsync (string id, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Copy an application
        /// </summary>
        /// <remarks>
        /// Copy an application (by a given ID).  This method is not supported by microservice applications.  A request to the \"clone\" resource creates a new application based on an already existing one.  The properties are copied to the newly created application and the prefix \"clone\" is added to the properties `name`, `key` and `contextPath` in order to be unique.  If the target application is hosted and has an active version, the new application will have the active version with the same content. <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (Application)</returns>
        System.Threading.Tasks.Task<ApiResponse<Application>> PostApplicationResourceWithHttpInfoAsync (string id, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Update a specific application
        /// </summary>
        /// <remarks>
        /// Update a specific application (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="putApplicationResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of Application</returns>
        System.Threading.Tasks.Task<Application> PutApplicationResourceAsync (string id, PutApplicationResourceRequest putApplicationResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update a specific application
        /// </summary>
        /// <remarks>
        /// Update a specific application (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="putApplicationResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (Application)</returns>
        System.Threading.Tasks.Task<ApiResponse<Application>> PutApplicationResourceWithHttpInfoAsync (string id, PutApplicationResourceRequest putApplicationResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken));
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class ApplicationsApi : IApplicationsApi
    {
        private kern.services.CumulocityClient.Client.ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public ApplicationsApi(String basePath)
        {
            this.Configuration = new kern.services.CumulocityClient.Client.Configuration { BasePath = basePath };

            ExceptionFactory = kern.services.CumulocityClient.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationsApi"/> class
        /// </summary>
        /// <returns></returns>
        public ApplicationsApi()
        {
            this.Configuration = kern.services.CumulocityClient.Client.Configuration.Default;

            ExceptionFactory = kern.services.CumulocityClient.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationsApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public ApplicationsApi(kern.services.CumulocityClient.Client.Configuration configuration = null)
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
        /// Delete an application Delete an application (by a given ID). This method is not supported by microservice applications.  > **&#9432; Info:** With regards to a hosted application, there is a caching mechanism in place that keeps the information about the placement of application files (html, javascript, css, fonts, etc.). Removing a hosted application, in normal circumstances, will cause the subsequent requests for application files to fail with an HTTP 404 error because the application is removed synchronously, its files are immediately removed on the node serving the request and at the same time the information is propagated to other nodes – but in rare cases there might be a delay with this propagation. In such situations, the files of the removed application can be served from those nodes up until the aforementioned cache expires. For the same reason, the cache can also cause HTTP 404 errors when the application is updated as it will keep the path to the files of the old version of the application. The cache is filled on demand, so there should not be issues if application files were not accessed prior to the delete request. The expiration delay of the cache can differ, but should not take more than one minute.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN <b>AND</b> tenant is the owner of the application </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="force">Force deletion by unsubscribing all tenants from the application first and then deleting the application itself. (optional, default to false)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns></returns>
        public void DeleteApplicationResource (string id, bool? force = default(bool?), string xCumulocityProcessingMode = default(string))
        {
             DeleteApplicationResourceWithHttpInfo(id, force, xCumulocityProcessingMode);
        }

        /// <summary>
        /// Delete an application Delete an application (by a given ID). This method is not supported by microservice applications.  > **&#9432; Info:** With regards to a hosted application, there is a caching mechanism in place that keeps the information about the placement of application files (html, javascript, css, fonts, etc.). Removing a hosted application, in normal circumstances, will cause the subsequent requests for application files to fail with an HTTP 404 error because the application is removed synchronously, its files are immediately removed on the node serving the request and at the same time the information is propagated to other nodes – but in rare cases there might be a delay with this propagation. In such situations, the files of the removed application can be served from those nodes up until the aforementioned cache expires. For the same reason, the cache can also cause HTTP 404 errors when the application is updated as it will keep the path to the files of the old version of the application. The cache is filled on demand, so there should not be issues if application files were not accessed prior to the delete request. The expiration delay of the cache can differ, but should not take more than one minute.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN <b>AND</b> tenant is the owner of the application </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="force">Force deletion by unsubscribing all tenants from the application first and then deleting the application itself. (optional, default to false)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> DeleteApplicationResourceWithHttpInfo (string id, bool? force = default(bool?), string xCumulocityProcessingMode = default(string))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ApplicationsApi->DeleteApplicationResource");

            var localVarPath = "/application/applications/{id}";
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
            if (force != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "force", force)); // query parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter

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
                Exception exception = ExceptionFactory("DeleteApplicationResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                null);
        }

        /// <summary>
        /// Delete an application Delete an application (by a given ID). This method is not supported by microservice applications.  > **&#9432; Info:** With regards to a hosted application, there is a caching mechanism in place that keeps the information about the placement of application files (html, javascript, css, fonts, etc.). Removing a hosted application, in normal circumstances, will cause the subsequent requests for application files to fail with an HTTP 404 error because the application is removed synchronously, its files are immediately removed on the node serving the request and at the same time the information is propagated to other nodes – but in rare cases there might be a delay with this propagation. In such situations, the files of the removed application can be served from those nodes up until the aforementioned cache expires. For the same reason, the cache can also cause HTTP 404 errors when the application is updated as it will keep the path to the files of the old version of the application. The cache is filled on demand, so there should not be issues if application files were not accessed prior to the delete request. The expiration delay of the cache can differ, but should not take more than one minute.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN <b>AND</b> tenant is the owner of the application </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="force">Force deletion by unsubscribing all tenants from the application first and then deleting the application itself. (optional, default to false)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task DeleteApplicationResourceAsync (string id, bool? force = default(bool?), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
             await DeleteApplicationResourceWithHttpInfoAsync(id, force, xCumulocityProcessingMode, cancellationToken);

        }

        /// <summary>
        /// Delete an application Delete an application (by a given ID). This method is not supported by microservice applications.  > **&#9432; Info:** With regards to a hosted application, there is a caching mechanism in place that keeps the information about the placement of application files (html, javascript, css, fonts, etc.). Removing a hosted application, in normal circumstances, will cause the subsequent requests for application files to fail with an HTTP 404 error because the application is removed synchronously, its files are immediately removed on the node serving the request and at the same time the information is propagated to other nodes – but in rare cases there might be a delay with this propagation. In such situations, the files of the removed application can be served from those nodes up until the aforementioned cache expires. For the same reason, the cache can also cause HTTP 404 errors when the application is updated as it will keep the path to the files of the old version of the application. The cache is filled on demand, so there should not be issues if application files were not accessed prior to the delete request. The expiration delay of the cache can differ, but should not take more than one minute.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN <b>AND</b> tenant is the owner of the application </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="force">Force deletion by unsubscribing all tenants from the application first and then deleting the application itself. (optional, default to false)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> DeleteApplicationResourceWithHttpInfoAsync (string id, bool? force = default(bool?), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ApplicationsApi->DeleteApplicationResource");

            var localVarPath = "/application/applications/{id}";
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
            if (force != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "force", force)); // query parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter

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
                Exception exception = ExceptionFactory("DeleteApplicationResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                null);
        }

        /// <summary>
        /// Retrieve all applications Retrieve all applications on your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="name">The name of the application. (optional)</param>
        /// <param name="owner">The ID of the tenant that owns the applications. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="providedFor">The ID of a tenant that is subscribed to the applications but doesn&#39;t own them. (optional)</param>
        /// <param name="subscriber">The ID of a tenant that is subscribed to the applications. (optional)</param>
        /// <param name="tenant">The ID of a tenant that either owns the application or is subscribed to the applications. (optional)</param>
        /// <param name="type">The type of the application. It is possible to use multiple values separated by a comma. For example, &#x60;EXTERNAL,HOSTED&#x60; will return only applications with type &#x60;EXTERNAL&#x60; or &#x60;HOSTED&#x60;. (optional)</param>
        /// <param name="user">The ID of a user that has access to the applications. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="hasVersions">When set to &#x60;true&#x60;, the returned result contains applications with an &#x60;applicationVersions&#x60; field that is not empty. When set to &#x60;false&#x60;, the result will contain applications with an empty &#x60;applicationVersions&#x60; field. (optional)</param>
        /// <returns>ApplicationCollection</returns>
        public ApplicationCollection GetAbstractApplicationCollectionResource (int? currentPage = default(int?), string name = default(string), string owner = default(string), int? pageSize = default(int?), string providedFor = default(string), string subscriber = default(string), string tenant = default(string), string type = default(string), string user = default(string), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), bool? hasVersions = default(bool?))
        {
             ApiResponse<ApplicationCollection> localVarResponse = GetAbstractApplicationCollectionResourceWithHttpInfo(currentPage, name, owner, pageSize, providedFor, subscriber, tenant, type, user, withTotalElements, withTotalPages, hasVersions);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve all applications Retrieve all applications on your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="name">The name of the application. (optional)</param>
        /// <param name="owner">The ID of the tenant that owns the applications. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="providedFor">The ID of a tenant that is subscribed to the applications but doesn&#39;t own them. (optional)</param>
        /// <param name="subscriber">The ID of a tenant that is subscribed to the applications. (optional)</param>
        /// <param name="tenant">The ID of a tenant that either owns the application or is subscribed to the applications. (optional)</param>
        /// <param name="type">The type of the application. It is possible to use multiple values separated by a comma. For example, &#x60;EXTERNAL,HOSTED&#x60; will return only applications with type &#x60;EXTERNAL&#x60; or &#x60;HOSTED&#x60;. (optional)</param>
        /// <param name="user">The ID of a user that has access to the applications. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="hasVersions">When set to &#x60;true&#x60;, the returned result contains applications with an &#x60;applicationVersions&#x60; field that is not empty. When set to &#x60;false&#x60;, the result will contain applications with an empty &#x60;applicationVersions&#x60; field. (optional)</param>
        /// <returns>ApiResponse of ApplicationCollection</returns>
        public ApiResponse<ApplicationCollection> GetAbstractApplicationCollectionResourceWithHttpInfo (int? currentPage = default(int?), string name = default(string), string owner = default(string), int? pageSize = default(int?), string providedFor = default(string), string subscriber = default(string), string tenant = default(string), string type = default(string), string user = default(string), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), bool? hasVersions = default(bool?))
        {

            var localVarPath = "/application/applications";
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
                "application/vnd.com.nsn.cumulocity.applicationcollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (currentPage != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "currentPage", currentPage)); // query parameter
            if (name != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "name", name)); // query parameter
            if (owner != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "owner", owner)); // query parameter
            if (pageSize != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "pageSize", pageSize)); // query parameter
            if (providedFor != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "providedFor", providedFor)); // query parameter
            if (subscriber != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "subscriber", subscriber)); // query parameter
            if (tenant != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "tenant", tenant)); // query parameter
            if (type != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "type", type)); // query parameter
            if (user != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "user", user)); // query parameter
            if (withTotalElements != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withTotalElements", withTotalElements)); // query parameter
            if (withTotalPages != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withTotalPages", withTotalPages)); // query parameter
            if (hasVersions != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "hasVersions", hasVersions)); // query parameter

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
                Exception exception = ExceptionFactory("GetAbstractApplicationCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<ApplicationCollection>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (ApplicationCollection) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(ApplicationCollection)));
        }

        /// <summary>
        /// Retrieve all applications Retrieve all applications on your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="name">The name of the application. (optional)</param>
        /// <param name="owner">The ID of the tenant that owns the applications. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="providedFor">The ID of a tenant that is subscribed to the applications but doesn&#39;t own them. (optional)</param>
        /// <param name="subscriber">The ID of a tenant that is subscribed to the applications. (optional)</param>
        /// <param name="tenant">The ID of a tenant that either owns the application or is subscribed to the applications. (optional)</param>
        /// <param name="type">The type of the application. It is possible to use multiple values separated by a comma. For example, &#x60;EXTERNAL,HOSTED&#x60; will return only applications with type &#x60;EXTERNAL&#x60; or &#x60;HOSTED&#x60;. (optional)</param>
        /// <param name="user">The ID of a user that has access to the applications. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="hasVersions">When set to &#x60;true&#x60;, the returned result contains applications with an &#x60;applicationVersions&#x60; field that is not empty. When set to &#x60;false&#x60;, the result will contain applications with an empty &#x60;applicationVersions&#x60; field. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApplicationCollection</returns>
        public async System.Threading.Tasks.Task<ApplicationCollection> GetAbstractApplicationCollectionResourceAsync (int? currentPage = default(int?), string name = default(string), string owner = default(string), int? pageSize = default(int?), string providedFor = default(string), string subscriber = default(string), string tenant = default(string), string type = default(string), string user = default(string), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), bool? hasVersions = default(bool?), CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<ApplicationCollection> localVarResponse = await GetAbstractApplicationCollectionResourceWithHttpInfoAsync(currentPage, name, owner, pageSize, providedFor, subscriber, tenant, type, user, withTotalElements, withTotalPages, hasVersions, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Retrieve all applications Retrieve all applications on your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="name">The name of the application. (optional)</param>
        /// <param name="owner">The ID of the tenant that owns the applications. (optional)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="providedFor">The ID of a tenant that is subscribed to the applications but doesn&#39;t own them. (optional)</param>
        /// <param name="subscriber">The ID of a tenant that is subscribed to the applications. (optional)</param>
        /// <param name="tenant">The ID of a tenant that either owns the application or is subscribed to the applications. (optional)</param>
        /// <param name="type">The type of the application. It is possible to use multiple values separated by a comma. For example, &#x60;EXTERNAL,HOSTED&#x60; will return only applications with type &#x60;EXTERNAL&#x60; or &#x60;HOSTED&#x60;. (optional)</param>
        /// <param name="user">The ID of a user that has access to the applications. (optional)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="hasVersions">When set to &#x60;true&#x60;, the returned result contains applications with an &#x60;applicationVersions&#x60; field that is not empty. When set to &#x60;false&#x60;, the result will contain applications with an empty &#x60;applicationVersions&#x60; field. (optional)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (ApplicationCollection)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<ApplicationCollection>> GetAbstractApplicationCollectionResourceWithHttpInfoAsync (int? currentPage = default(int?), string name = default(string), string owner = default(string), int? pageSize = default(int?), string providedFor = default(string), string subscriber = default(string), string tenant = default(string), string type = default(string), string user = default(string), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), bool? hasVersions = default(bool?), CancellationToken cancellationToken = default(CancellationToken))
        {

            var localVarPath = "/application/applications";
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
                "application/vnd.com.nsn.cumulocity.applicationcollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (currentPage != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "currentPage", currentPage)); // query parameter
            if (name != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "name", name)); // query parameter
            if (owner != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "owner", owner)); // query parameter
            if (pageSize != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "pageSize", pageSize)); // query parameter
            if (providedFor != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "providedFor", providedFor)); // query parameter
            if (subscriber != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "subscriber", subscriber)); // query parameter
            if (tenant != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "tenant", tenant)); // query parameter
            if (type != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "type", type)); // query parameter
            if (user != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "user", user)); // query parameter
            if (withTotalElements != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withTotalElements", withTotalElements)); // query parameter
            if (withTotalPages != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withTotalPages", withTotalPages)); // query parameter
            if (hasVersions != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "hasVersions", hasVersions)); // query parameter

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
                Exception exception = ExceptionFactory("GetAbstractApplicationCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<ApplicationCollection>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (ApplicationCollection) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(ApplicationCollection)));
        }

        /// <summary>
        /// Retrieve a specific application Retrieve a specific application (by a given ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ <b>OR</b> current user has explicit access to the application </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <returns>Application</returns>
        public Application GetApplicationResource (string id)
        {
             ApiResponse<Application> localVarResponse = GetApplicationResourceWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve a specific application Retrieve a specific application (by a given ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ <b>OR</b> current user has explicit access to the application </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <returns>ApiResponse of Application</returns>
        public ApiResponse<Application> GetApplicationResourceWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ApplicationsApi->GetApplicationResource");

            var localVarPath = "/application/applications/{id}";
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
                "application/vnd.com.nsn.cumulocity.application+json",
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
                Exception exception = ExceptionFactory("GetApplicationResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Application>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (Application) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Application)));
        }

        /// <summary>
        /// Retrieve a specific application Retrieve a specific application (by a given ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ <b>OR</b> current user has explicit access to the application </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of Application</returns>
        public async System.Threading.Tasks.Task<Application> GetApplicationResourceAsync (string id, CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<Application> localVarResponse = await GetApplicationResourceWithHttpInfoAsync(id, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Retrieve a specific application Retrieve a specific application (by a given ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ <b>OR</b> current user has explicit access to the application </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (Application)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Application>> GetApplicationResourceWithHttpInfoAsync (string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ApplicationsApi->GetApplicationResource");

            var localVarPath = "/application/applications/{id}";
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
                "application/vnd.com.nsn.cumulocity.application+json",
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
                Exception exception = ExceptionFactory("GetApplicationResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Application>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (Application) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Application)));
        }

        /// <summary>
        /// Retrieve applications by name Retrieve applications by name.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="name">The name of the application.</param>
        /// <returns>GetApplicationsByNameCollectionResource200Response</returns>
        public GetApplicationsByNameCollectionResource200Response GetApplicationsByNameCollectionResource (string name)
        {
             ApiResponse<GetApplicationsByNameCollectionResource200Response> localVarResponse = GetApplicationsByNameCollectionResourceWithHttpInfo(name);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve applications by name Retrieve applications by name.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="name">The name of the application.</param>
        /// <returns>ApiResponse of GetApplicationsByNameCollectionResource200Response</returns>
        public ApiResponse<GetApplicationsByNameCollectionResource200Response> GetApplicationsByNameCollectionResourceWithHttpInfo (string name)
        {
            // verify the required parameter 'name' is set
            if (name == null)
                throw new ApiException(400, "Missing required parameter 'name' when calling ApplicationsApi->GetApplicationsByNameCollectionResource");

            var localVarPath = "/application/applicationsByName/{name}";
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
                "application/vnd.com.nsn.cumulocity.applicationcollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (name != null) localVarPathParams.Add("name", this.Configuration.ApiClient.ParameterToString(name)); // path parameter

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
                Exception exception = ExceptionFactory("GetApplicationsByNameCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<GetApplicationsByNameCollectionResource200Response>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (GetApplicationsByNameCollectionResource200Response) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(GetApplicationsByNameCollectionResource200Response)));
        }

        /// <summary>
        /// Retrieve applications by name Retrieve applications by name.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="name">The name of the application.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of GetApplicationsByNameCollectionResource200Response</returns>
        public async System.Threading.Tasks.Task<GetApplicationsByNameCollectionResource200Response> GetApplicationsByNameCollectionResourceAsync (string name, CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<GetApplicationsByNameCollectionResource200Response> localVarResponse = await GetApplicationsByNameCollectionResourceWithHttpInfoAsync(name, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Retrieve applications by name Retrieve applications by name.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="name">The name of the application.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (GetApplicationsByNameCollectionResource200Response)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<GetApplicationsByNameCollectionResource200Response>> GetApplicationsByNameCollectionResourceWithHttpInfoAsync (string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'name' is set
            if (name == null)
                throw new ApiException(400, "Missing required parameter 'name' when calling ApplicationsApi->GetApplicationsByNameCollectionResource");

            var localVarPath = "/application/applicationsByName/{name}";
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
                "application/vnd.com.nsn.cumulocity.applicationcollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (name != null) localVarPathParams.Add("name", this.Configuration.ApiClient.ParameterToString(name)); // path parameter

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
                Exception exception = ExceptionFactory("GetApplicationsByNameCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<GetApplicationsByNameCollectionResource200Response>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (GetApplicationsByNameCollectionResource200Response) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(GetApplicationsByNameCollectionResource200Response)));
        }

        /// <summary>
        /// Retrieve applications by owner Retrieve all applications owned by a particular tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <returns>GetApplicationsByOwnerCollectionResource200Response</returns>
        public GetApplicationsByOwnerCollectionResource200Response GetApplicationsByOwnerCollectionResource (string tenantId, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?))
        {
             ApiResponse<GetApplicationsByOwnerCollectionResource200Response> localVarResponse = GetApplicationsByOwnerCollectionResourceWithHttpInfo(tenantId, currentPage, pageSize, withTotalElements, withTotalPages);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve applications by owner Retrieve all applications owned by a particular tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <returns>ApiResponse of GetApplicationsByOwnerCollectionResource200Response</returns>
        public ApiResponse<GetApplicationsByOwnerCollectionResource200Response> GetApplicationsByOwnerCollectionResourceWithHttpInfo (string tenantId, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
                throw new ApiException(400, "Missing required parameter 'tenantId' when calling ApplicationsApi->GetApplicationsByOwnerCollectionResource");

            var localVarPath = "/application/applicationsByOwner/{tenantId}";
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
                "application/vnd.com.nsn.cumulocity.applicationcollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (tenantId != null) localVarPathParams.Add("tenantId", this.Configuration.ApiClient.ParameterToString(tenantId)); // path parameter
            if (currentPage != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "currentPage", currentPage)); // query parameter
            if (pageSize != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "pageSize", pageSize)); // query parameter
            if (withTotalElements != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withTotalElements", withTotalElements)); // query parameter
            if (withTotalPages != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withTotalPages", withTotalPages)); // query parameter

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
                Exception exception = ExceptionFactory("GetApplicationsByOwnerCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<GetApplicationsByOwnerCollectionResource200Response>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (GetApplicationsByOwnerCollectionResource200Response) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(GetApplicationsByOwnerCollectionResource200Response)));
        }

        /// <summary>
        /// Retrieve applications by owner Retrieve all applications owned by a particular tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of GetApplicationsByOwnerCollectionResource200Response</returns>
        public async System.Threading.Tasks.Task<GetApplicationsByOwnerCollectionResource200Response> GetApplicationsByOwnerCollectionResourceAsync (string tenantId, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<GetApplicationsByOwnerCollectionResource200Response> localVarResponse = await GetApplicationsByOwnerCollectionResourceWithHttpInfoAsync(tenantId, currentPage, pageSize, withTotalElements, withTotalPages, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Retrieve applications by owner Retrieve all applications owned by a particular tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (GetApplicationsByOwnerCollectionResource200Response)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<GetApplicationsByOwnerCollectionResource200Response>> GetApplicationsByOwnerCollectionResourceWithHttpInfoAsync (string tenantId, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
                throw new ApiException(400, "Missing required parameter 'tenantId' when calling ApplicationsApi->GetApplicationsByOwnerCollectionResource");

            var localVarPath = "/application/applicationsByOwner/{tenantId}";
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
                "application/vnd.com.nsn.cumulocity.applicationcollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (tenantId != null) localVarPathParams.Add("tenantId", this.Configuration.ApiClient.ParameterToString(tenantId)); // path parameter
            if (currentPage != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "currentPage", currentPage)); // query parameter
            if (pageSize != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "pageSize", pageSize)); // query parameter
            if (withTotalElements != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withTotalElements", withTotalElements)); // query parameter
            if (withTotalPages != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withTotalPages", withTotalPages)); // query parameter

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
                Exception exception = ExceptionFactory("GetApplicationsByOwnerCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<GetApplicationsByOwnerCollectionResource200Response>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (GetApplicationsByOwnerCollectionResource200Response) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(GetApplicationsByOwnerCollectionResource200Response)));
        }

        /// <summary>
        /// Retrieve applications by tenant Retrieve applications subscribed or owned by a particular tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <returns>GetApplicationsByTenantCollectionResource200Response</returns>
        public GetApplicationsByTenantCollectionResource200Response GetApplicationsByTenantCollectionResource (string tenantId)
        {
             ApiResponse<GetApplicationsByTenantCollectionResource200Response> localVarResponse = GetApplicationsByTenantCollectionResourceWithHttpInfo(tenantId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve applications by tenant Retrieve applications subscribed or owned by a particular tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <returns>ApiResponse of GetApplicationsByTenantCollectionResource200Response</returns>
        public ApiResponse<GetApplicationsByTenantCollectionResource200Response> GetApplicationsByTenantCollectionResourceWithHttpInfo (string tenantId)
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
                throw new ApiException(400, "Missing required parameter 'tenantId' when calling ApplicationsApi->GetApplicationsByTenantCollectionResource");

            var localVarPath = "/application/applicationsByTenant/{tenantId}";
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
                "application/vnd.com.nsn.cumulocity.applicationcollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (tenantId != null) localVarPathParams.Add("tenantId", this.Configuration.ApiClient.ParameterToString(tenantId)); // path parameter

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
                Exception exception = ExceptionFactory("GetApplicationsByTenantCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<GetApplicationsByTenantCollectionResource200Response>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (GetApplicationsByTenantCollectionResource200Response) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(GetApplicationsByTenantCollectionResource200Response)));
        }

        /// <summary>
        /// Retrieve applications by tenant Retrieve applications subscribed or owned by a particular tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of GetApplicationsByTenantCollectionResource200Response</returns>
        public async System.Threading.Tasks.Task<GetApplicationsByTenantCollectionResource200Response> GetApplicationsByTenantCollectionResourceAsync (string tenantId, CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<GetApplicationsByTenantCollectionResource200Response> localVarResponse = await GetApplicationsByTenantCollectionResourceWithHttpInfoAsync(tenantId, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Retrieve applications by tenant Retrieve applications subscribed or owned by a particular tenant (by a given tenant ID).  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_READ </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="tenantId">Unique identifier of a Cumulocity IoT tenant.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (GetApplicationsByTenantCollectionResource200Response)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<GetApplicationsByTenantCollectionResource200Response>> GetApplicationsByTenantCollectionResourceWithHttpInfoAsync (string tenantId, CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'tenantId' is set
            if (tenantId == null)
                throw new ApiException(400, "Missing required parameter 'tenantId' when calling ApplicationsApi->GetApplicationsByTenantCollectionResource");

            var localVarPath = "/application/applicationsByTenant/{tenantId}";
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
                "application/vnd.com.nsn.cumulocity.applicationcollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (tenantId != null) localVarPathParams.Add("tenantId", this.Configuration.ApiClient.ParameterToString(tenantId)); // path parameter

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
                Exception exception = ExceptionFactory("GetApplicationsByTenantCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<GetApplicationsByTenantCollectionResource200Response>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (GetApplicationsByTenantCollectionResource200Response) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(GetApplicationsByTenantCollectionResource200Response)));
        }

        /// <summary>
        /// Retrieve applications by user Retrieve all applications for a particular user (by a given username).  <section><h5>Required roles</h5> (ROLE_USER_MANAGEMENT_OWN_READ <b>AND</b> is the current user) <b>OR</b> (ROLE_USER_MANAGEMENT_READ <b>AND</b> ROLE_APPLICATION_MANAGEMENT_READ) </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="username">The username of the a user.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <returns>GetApplicationsByUserCollectionResource200Response</returns>
        public GetApplicationsByUserCollectionResource200Response GetApplicationsByUserCollectionResource (string username, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?))
        {
             ApiResponse<GetApplicationsByUserCollectionResource200Response> localVarResponse = GetApplicationsByUserCollectionResourceWithHttpInfo(username, currentPage, pageSize, withTotalElements, withTotalPages);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Retrieve applications by user Retrieve all applications for a particular user (by a given username).  <section><h5>Required roles</h5> (ROLE_USER_MANAGEMENT_OWN_READ <b>AND</b> is the current user) <b>OR</b> (ROLE_USER_MANAGEMENT_READ <b>AND</b> ROLE_APPLICATION_MANAGEMENT_READ) </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="username">The username of the a user.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <returns>ApiResponse of GetApplicationsByUserCollectionResource200Response</returns>
        public ApiResponse<GetApplicationsByUserCollectionResource200Response> GetApplicationsByUserCollectionResourceWithHttpInfo (string username, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?))
        {
            // verify the required parameter 'username' is set
            if (username == null)
                throw new ApiException(400, "Missing required parameter 'username' when calling ApplicationsApi->GetApplicationsByUserCollectionResource");

            var localVarPath = "/application/applicationsByUser/{username}";
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
                "application/vnd.com.nsn.cumulocity.applicationcollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (username != null) localVarPathParams.Add("username", this.Configuration.ApiClient.ParameterToString(username)); // path parameter
            if (currentPage != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "currentPage", currentPage)); // query parameter
            if (pageSize != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "pageSize", pageSize)); // query parameter
            if (withTotalElements != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withTotalElements", withTotalElements)); // query parameter
            if (withTotalPages != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withTotalPages", withTotalPages)); // query parameter

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
                Exception exception = ExceptionFactory("GetApplicationsByUserCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<GetApplicationsByUserCollectionResource200Response>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (GetApplicationsByUserCollectionResource200Response) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(GetApplicationsByUserCollectionResource200Response)));
        }

        /// <summary>
        /// Retrieve applications by user Retrieve all applications for a particular user (by a given username).  <section><h5>Required roles</h5> (ROLE_USER_MANAGEMENT_OWN_READ <b>AND</b> is the current user) <b>OR</b> (ROLE_USER_MANAGEMENT_READ <b>AND</b> ROLE_APPLICATION_MANAGEMENT_READ) </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="username">The username of the a user.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of GetApplicationsByUserCollectionResource200Response</returns>
        public async System.Threading.Tasks.Task<GetApplicationsByUserCollectionResource200Response> GetApplicationsByUserCollectionResourceAsync (string username, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<GetApplicationsByUserCollectionResource200Response> localVarResponse = await GetApplicationsByUserCollectionResourceWithHttpInfoAsync(username, currentPage, pageSize, withTotalElements, withTotalPages, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Retrieve applications by user Retrieve all applications for a particular user (by a given username).  <section><h5>Required roles</h5> (ROLE_USER_MANAGEMENT_OWN_READ <b>AND</b> is the current user) <b>OR</b> (ROLE_USER_MANAGEMENT_READ <b>AND</b> ROLE_APPLICATION_MANAGEMENT_READ) </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="username">The username of the a user.</param>
        /// <param name="currentPage">The current page of the paginated results. (optional, default to 1)</param>
        /// <param name="pageSize">Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional, default to 5)</param>
        /// <param name="withTotalElements">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="withTotalPages">When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional, default to false)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (GetApplicationsByUserCollectionResource200Response)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<GetApplicationsByUserCollectionResource200Response>> GetApplicationsByUserCollectionResourceWithHttpInfoAsync (string username, int? currentPage = default(int?), int? pageSize = default(int?), bool? withTotalElements = default(bool?), bool? withTotalPages = default(bool?), CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'username' is set
            if (username == null)
                throw new ApiException(400, "Missing required parameter 'username' when calling ApplicationsApi->GetApplicationsByUserCollectionResource");

            var localVarPath = "/application/applicationsByUser/{username}";
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
                "application/vnd.com.nsn.cumulocity.applicationcollection+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (username != null) localVarPathParams.Add("username", this.Configuration.ApiClient.ParameterToString(username)); // path parameter
            if (currentPage != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "currentPage", currentPage)); // query parameter
            if (pageSize != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "pageSize", pageSize)); // query parameter
            if (withTotalElements != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withTotalElements", withTotalElements)); // query parameter
            if (withTotalPages != null) localVarQueryParams.AddRange(this.Configuration.ApiClient.ParameterToKeyValuePairs("", "withTotalPages", withTotalPages)); // query parameter

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
                Exception exception = ExceptionFactory("GetApplicationsByUserCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<GetApplicationsByUserCollectionResource200Response>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (GetApplicationsByUserCollectionResource200Response) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(GetApplicationsByUserCollectionResource200Response)));
        }

        /// <summary>
        /// Create an application Create an application on your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postApplicationCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>Application</returns>
        public Application PostApplicationCollectionResource (PostApplicationCollectionResourceRequest postApplicationCollectionResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string))
        {
             ApiResponse<Application> localVarResponse = PostApplicationCollectionResourceWithHttpInfo(postApplicationCollectionResourceRequest, accept, xCumulocityProcessingMode);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create an application Create an application on your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postApplicationCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ApiResponse of Application</returns>
        public ApiResponse<Application> PostApplicationCollectionResourceWithHttpInfo (PostApplicationCollectionResourceRequest postApplicationCollectionResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string))
        {
            // verify the required parameter 'postApplicationCollectionResourceRequest' is set
            if (postApplicationCollectionResourceRequest == null)
                throw new ApiException(400, "Missing required parameter 'postApplicationCollectionResourceRequest' when calling ApplicationsApi->PostApplicationCollectionResource");

            var localVarPath = "/application/applications";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.application+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.application+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter
            if (postApplicationCollectionResourceRequest != null && postApplicationCollectionResourceRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(postApplicationCollectionResourceRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = postApplicationCollectionResourceRequest; // byte array
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
                Exception exception = ExceptionFactory("PostApplicationCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Application>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (Application) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Application)));
        }

        /// <summary>
        /// Create an application Create an application on your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postApplicationCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of Application</returns>
        public async System.Threading.Tasks.Task<Application> PostApplicationCollectionResourceAsync (PostApplicationCollectionResourceRequest postApplicationCollectionResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<Application> localVarResponse = await PostApplicationCollectionResourceWithHttpInfoAsync(postApplicationCollectionResourceRequest, accept, xCumulocityProcessingMode, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create an application Create an application on your tenant.  <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postApplicationCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (Application)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Application>> PostApplicationCollectionResourceWithHttpInfoAsync (PostApplicationCollectionResourceRequest postApplicationCollectionResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'postApplicationCollectionResourceRequest' is set
            if (postApplicationCollectionResourceRequest == null)
                throw new ApiException(400, "Missing required parameter 'postApplicationCollectionResourceRequest' when calling ApplicationsApi->PostApplicationCollectionResource");

            var localVarPath = "/application/applications";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.application+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.application+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter
            if (postApplicationCollectionResourceRequest != null && postApplicationCollectionResourceRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(postApplicationCollectionResourceRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = postApplicationCollectionResourceRequest; // byte array
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
                Exception exception = ExceptionFactory("PostApplicationCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Application>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (Application) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Application)));
        }

        /// <summary>
        /// Copy an application Copy an application (by a given ID).  This method is not supported by microservice applications.  A request to the \"clone\" resource creates a new application based on an already existing one.  The properties are copied to the newly created application and the prefix \"clone\" is added to the properties `name`, `key` and `contextPath` in order to be unique.  If the target application is hosted and has an active version, the new application will have the active version with the same content. <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>Application</returns>
        public Application PostApplicationResource (string id, string accept = default(string), string xCumulocityProcessingMode = default(string))
        {
             ApiResponse<Application> localVarResponse = PostApplicationResourceWithHttpInfo(id, accept, xCumulocityProcessingMode);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Copy an application Copy an application (by a given ID).  This method is not supported by microservice applications.  A request to the \"clone\" resource creates a new application based on an already existing one.  The properties are copied to the newly created application and the prefix \"clone\" is added to the properties `name`, `key` and `contextPath` in order to be unique.  If the target application is hosted and has an active version, the new application will have the active version with the same content. <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ApiResponse of Application</returns>
        public ApiResponse<Application> PostApplicationResourceWithHttpInfo (string id, string accept = default(string), string xCumulocityProcessingMode = default(string))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ApplicationsApi->PostApplicationResource");

            var localVarPath = "/application/applications/{id}/clone";
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
                "application/vnd.com.nsn.cumulocity.application+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter

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
                Exception exception = ExceptionFactory("PostApplicationResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Application>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (Application) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Application)));
        }

        /// <summary>
        /// Copy an application Copy an application (by a given ID).  This method is not supported by microservice applications.  A request to the \"clone\" resource creates a new application based on an already existing one.  The properties are copied to the newly created application and the prefix \"clone\" is added to the properties `name`, `key` and `contextPath` in order to be unique.  If the target application is hosted and has an active version, the new application will have the active version with the same content. <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of Application</returns>
        public async System.Threading.Tasks.Task<Application> PostApplicationResourceAsync (string id, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<Application> localVarResponse = await PostApplicationResourceWithHttpInfoAsync(id, accept, xCumulocityProcessingMode, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Copy an application Copy an application (by a given ID).  This method is not supported by microservice applications.  A request to the \"clone\" resource creates a new application based on an already existing one.  The properties are copied to the newly created application and the prefix \"clone\" is added to the properties `name`, `key` and `contextPath` in order to be unique.  If the target application is hosted and has an active version, the new application will have the active version with the same content. <section><h5>Required roles</h5> ROLE_APPLICATION_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (Application)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Application>> PostApplicationResourceWithHttpInfoAsync (string id, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ApplicationsApi->PostApplicationResource");

            var localVarPath = "/application/applications/{id}/clone";
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
                "application/vnd.com.nsn.cumulocity.application+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter

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
                Exception exception = ExceptionFactory("PostApplicationResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Application>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (Application) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Application)));
        }

        /// <summary>
        /// Update a specific application Update a specific application (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="putApplicationResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>Application</returns>
        public Application PutApplicationResource (string id, PutApplicationResourceRequest putApplicationResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string))
        {
             ApiResponse<Application> localVarResponse = PutApplicationResourceWithHttpInfo(id, putApplicationResourceRequest, accept, xCumulocityProcessingMode);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Update a specific application Update a specific application (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="putApplicationResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ApiResponse of Application</returns>
        public ApiResponse<Application> PutApplicationResourceWithHttpInfo (string id, PutApplicationResourceRequest putApplicationResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ApplicationsApi->PutApplicationResource");
            // verify the required parameter 'putApplicationResourceRequest' is set
            if (putApplicationResourceRequest == null)
                throw new ApiException(400, "Missing required parameter 'putApplicationResourceRequest' when calling ApplicationsApi->PutApplicationResource");

            var localVarPath = "/application/applications/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.application+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.application+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter
            if (putApplicationResourceRequest != null && putApplicationResourceRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(putApplicationResourceRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = putApplicationResourceRequest; // byte array
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
                Exception exception = ExceptionFactory("PutApplicationResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Application>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (Application) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Application)));
        }

        /// <summary>
        /// Update a specific application Update a specific application (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="putApplicationResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of Application</returns>
        public async System.Threading.Tasks.Task<Application> PutApplicationResourceAsync (string id, PutApplicationResourceRequest putApplicationResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<Application> localVarResponse = await PutApplicationResourceWithHttpInfoAsync(id, putApplicationResourceRequest, accept, xCumulocityProcessingMode, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Update a specific application Update a specific application (by a given ID).  <section><h5>Required roles</h5> ROLE_USER_MANAGEMENT_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Unique identifier of the application.</param>
        /// <param name="putApplicationResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (Application)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Application>> PutApplicationResourceWithHttpInfoAsync (string id, PutApplicationResourceRequest putApplicationResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ApplicationsApi->PutApplicationResource");
            // verify the required parameter 'putApplicationResourceRequest' is set
            if (putApplicationResourceRequest == null)
                throw new ApiException(400, "Missing required parameter 'putApplicationResourceRequest' when calling ApplicationsApi->PutApplicationResource");

            var localVarPath = "/application/applications/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.application+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.application+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (id != null) localVarPathParams.Add("id", this.Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter
            if (putApplicationResourceRequest != null && putApplicationResourceRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(putApplicationResourceRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = putApplicationResourceRequest; // byte array
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
                Exception exception = ExceptionFactory("PutApplicationResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Application>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (Application) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(Application)));
        }

    }
}
