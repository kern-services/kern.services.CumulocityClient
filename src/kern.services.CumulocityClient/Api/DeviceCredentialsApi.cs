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
    public interface IDeviceCredentialsApi : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Create a bulk device credentials request
        /// </summary>
        /// <remarks>
        /// Create a bulk device credentials request.  Device credentials and basic device representation can be provided within a CSV file which must be UTF-8 or ANSI encoded. The CSV file must have two sections.  The first section is the first line of the CSV file. This line contains column names (headers):  |Name|Mandatory|Description| |- -- |- -- |- -- | |ID|Yes|The external ID of a device.| |CREDENTIALS|Yes*|Password for the device's user. Mandatory, unless AUTH_TYPE is \"CERTIFICATES\", then CREDENTIALS can be skipped.| |AUTH_TYPE|No|Required authentication type for the device's user. If the device uses credentials, this can be skipped or filled with \"BASIC\". Devices that use certificates must set \"CERTIFICATES\".| |TENANT|No|The ID of the tenant for which the registration is executed (only allowed for the management tenant).| |TYPE|No|The type of the device representation.| |NAME|No|The name of the device representation.| |ICCID|No|The ICCID of the device (SIM card number). If the ICCID appears in file, the import adds a fragment `c8y_Mobile.iccid`. The ICCID value is not mandatory for each row, see the example for an HTTP request below.| |IDTYPE|No|The type of the external ID. If IDTYPE doesn't appear in the file, the default value is used. The default value is `c8y_Serial`. The IDTYPE value is not mandatory for each row, see the example for an HTTP request below.| |PATH|No|The path in the groups hierarchy where the device is added. PATH contains the name of each group separated by `/`, that is: `main_group/sub_group/.../last_sub_group`. If a group does not exist, the import creates the group.| |SHELL|No|If this column contains a value of 1, the import adds the SHELL feature to the device (specifically the `c8y_SupportedOperations` fragment). The SHELL value is not mandatory for each row, see the example for an HTTP request below.|  Section two is the rest of the CSV file. Section two contains the device information. The order and quantity of the values must be the same as of the headers.  A separator is automatically obtained from the CSV file. Valid separator values are: `\\t` (tabulation mark), `;` (semicolon) and `,` (comma).  > **⚠️ Important:** The CSV file needs the \"com_cumulocity_model_Agent.active\" header with a value of \"true\" to be added to the request.  > **&#9432; Info:** A bulk registration creates an elementary representation of the device. Then, the device needs to update it to a full representation with its own status. The device is ready to use only after it is updated to the full representation. Also see [credentials upload](https://cumulocity.com/guides/users-guide/device-management/#creds-upload) and [device integration](https://cumulocity.com/guides/device-sdk/rest/#device-integration).  A CSV file can appear in many forms (with regard to the optional tenant column and the occurrence of device information):  * If a user is logged in as the management tenant, then the columns ID, CREDENTIALS and TENANT are mandatory, and the device credentials will be created for the tenant mentioned in the TENANT column. * If a user is logged in as a different tenant, for example, as `sample_tenant`, then the columns ID and CREDENTIALS are mandatory (if the file contains the TENANT column, it is ignored and the device credentials will be created for the tenant that is logged in). * If a user wants to add information about the device, the columns TYPE and NAME must appear in the CSV file. * If a user wants to add information about a SIM card number, the columns TYPE, NAME and ICCID must appear in the CSV file. * If a user wants to change the type of external ID, the columns TYPE, NAME and IDTYPE must appear in the CSV file. * If a user wants to add a device to a group, the columns TYPE, NAME and PATH must appear in the CSV file. * If a user wants to add the SHELL feature, the columns TYPE, NAME and SHELL must appear in the CSV file and the column SHELL must contain a value of 1.  It is possible to define a custom [external ID](#tag/External-IDs) mapping and some custom device properties which are added to newly created devices during registration:  * To add a custom external ID mapping, enter the external ID type as the header of the last column with the prefix \"external-\", for example, to add an external ID mapping of type `c8y_Imei`, enter `external-c8y_Imei` in the last column header. The value of this external ID type should be set in the corresponding column of the data rows. * To add a custom property to a registered device, enter the custom property name as a header, for example, \"myCustomProperty\", and the value would be in the rows below.  The custom device properties mapping has the following limitations:  * Braces '{}' used in data rows will be interpreted as strings of \"{}\". The system will interpret the value as an object when some custom property is added, for example, put `com_cumulocity_model_Agent.active` into the headers row and `true` into the data row to create an object `\"com_cumulocity_model_Agent\": {\"active\": \"true\"}\"`. * It is not possible to add array values via bulk registration.  Example file:  ```csv ID;CREDENTIALS;TYPE;NAME;ICCID;IDTYPE;PATH;SHELL id_101;AbcD1234!1234AbcD;type_of_device;Device 101;111111111;;csv device/subgroup0;1 id_102;AbcD1234!1234AbcD;type_of_device;Device 102;222222222;;csv device/subgroup0;0 id_111;AbcD1234!1234AbcD;type_of_device;Device 111;333333333;c8y_Imei;csv device1/subgroup1;0 id_112;AbcD1234!1234AbcD;type_of_device;Device 112;444444444;;csv device1/subgroup1;1 id_121;AbcD1234!1234AbcD;type_of_device;Device 121;555555555;;csv device1/subgroup2;1 id_122;AbcD1234!1234AbcD;type_of_device;Device 122;;;csv device1/subgroup2; id_131;AbcD1234!1234AbcD;type_of_device;Device 131;;;csv device1/subgroup3; ```  There is also a simple registration method that creates all registration requests at once, then each one needs to go through regular acceptance. This simple registration only makes use of the ID and PATH fields from the list above.  <section><h5>Required roles</h5> ROLE_DEVICE_CONTROL_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="file">The CSV file to be uploaded.</param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>BulkNewDeviceRequest</returns>
        BulkNewDeviceRequest PostBulkNewDeviceRequestCollectionResource (System.IO.Stream file, string accept = default(string), string xCumulocityProcessingMode = default(string));

        /// <summary>
        /// Create a bulk device credentials request
        /// </summary>
        /// <remarks>
        /// Create a bulk device credentials request.  Device credentials and basic device representation can be provided within a CSV file which must be UTF-8 or ANSI encoded. The CSV file must have two sections.  The first section is the first line of the CSV file. This line contains column names (headers):  |Name|Mandatory|Description| |- -- |- -- |- -- | |ID|Yes|The external ID of a device.| |CREDENTIALS|Yes*|Password for the device's user. Mandatory, unless AUTH_TYPE is \"CERTIFICATES\", then CREDENTIALS can be skipped.| |AUTH_TYPE|No|Required authentication type for the device's user. If the device uses credentials, this can be skipped or filled with \"BASIC\". Devices that use certificates must set \"CERTIFICATES\".| |TENANT|No|The ID of the tenant for which the registration is executed (only allowed for the management tenant).| |TYPE|No|The type of the device representation.| |NAME|No|The name of the device representation.| |ICCID|No|The ICCID of the device (SIM card number). If the ICCID appears in file, the import adds a fragment `c8y_Mobile.iccid`. The ICCID value is not mandatory for each row, see the example for an HTTP request below.| |IDTYPE|No|The type of the external ID. If IDTYPE doesn't appear in the file, the default value is used. The default value is `c8y_Serial`. The IDTYPE value is not mandatory for each row, see the example for an HTTP request below.| |PATH|No|The path in the groups hierarchy where the device is added. PATH contains the name of each group separated by `/`, that is: `main_group/sub_group/.../last_sub_group`. If a group does not exist, the import creates the group.| |SHELL|No|If this column contains a value of 1, the import adds the SHELL feature to the device (specifically the `c8y_SupportedOperations` fragment). The SHELL value is not mandatory for each row, see the example for an HTTP request below.|  Section two is the rest of the CSV file. Section two contains the device information. The order and quantity of the values must be the same as of the headers.  A separator is automatically obtained from the CSV file. Valid separator values are: `\\t` (tabulation mark), `;` (semicolon) and `,` (comma).  > **⚠️ Important:** The CSV file needs the \"com_cumulocity_model_Agent.active\" header with a value of \"true\" to be added to the request.  > **&#9432; Info:** A bulk registration creates an elementary representation of the device. Then, the device needs to update it to a full representation with its own status. The device is ready to use only after it is updated to the full representation. Also see [credentials upload](https://cumulocity.com/guides/users-guide/device-management/#creds-upload) and [device integration](https://cumulocity.com/guides/device-sdk/rest/#device-integration).  A CSV file can appear in many forms (with regard to the optional tenant column and the occurrence of device information):  * If a user is logged in as the management tenant, then the columns ID, CREDENTIALS and TENANT are mandatory, and the device credentials will be created for the tenant mentioned in the TENANT column. * If a user is logged in as a different tenant, for example, as `sample_tenant`, then the columns ID and CREDENTIALS are mandatory (if the file contains the TENANT column, it is ignored and the device credentials will be created for the tenant that is logged in). * If a user wants to add information about the device, the columns TYPE and NAME must appear in the CSV file. * If a user wants to add information about a SIM card number, the columns TYPE, NAME and ICCID must appear in the CSV file. * If a user wants to change the type of external ID, the columns TYPE, NAME and IDTYPE must appear in the CSV file. * If a user wants to add a device to a group, the columns TYPE, NAME and PATH must appear in the CSV file. * If a user wants to add the SHELL feature, the columns TYPE, NAME and SHELL must appear in the CSV file and the column SHELL must contain a value of 1.  It is possible to define a custom [external ID](#tag/External-IDs) mapping and some custom device properties which are added to newly created devices during registration:  * To add a custom external ID mapping, enter the external ID type as the header of the last column with the prefix \"external-\", for example, to add an external ID mapping of type `c8y_Imei`, enter `external-c8y_Imei` in the last column header. The value of this external ID type should be set in the corresponding column of the data rows. * To add a custom property to a registered device, enter the custom property name as a header, for example, \"myCustomProperty\", and the value would be in the rows below.  The custom device properties mapping has the following limitations:  * Braces '{}' used in data rows will be interpreted as strings of \"{}\". The system will interpret the value as an object when some custom property is added, for example, put `com_cumulocity_model_Agent.active` into the headers row and `true` into the data row to create an object `\"com_cumulocity_model_Agent\": {\"active\": \"true\"}\"`. * It is not possible to add array values via bulk registration.  Example file:  ```csv ID;CREDENTIALS;TYPE;NAME;ICCID;IDTYPE;PATH;SHELL id_101;AbcD1234!1234AbcD;type_of_device;Device 101;111111111;;csv device/subgroup0;1 id_102;AbcD1234!1234AbcD;type_of_device;Device 102;222222222;;csv device/subgroup0;0 id_111;AbcD1234!1234AbcD;type_of_device;Device 111;333333333;c8y_Imei;csv device1/subgroup1;0 id_112;AbcD1234!1234AbcD;type_of_device;Device 112;444444444;;csv device1/subgroup1;1 id_121;AbcD1234!1234AbcD;type_of_device;Device 121;555555555;;csv device1/subgroup2;1 id_122;AbcD1234!1234AbcD;type_of_device;Device 122;;;csv device1/subgroup2; id_131;AbcD1234!1234AbcD;type_of_device;Device 131;;;csv device1/subgroup3; ```  There is also a simple registration method that creates all registration requests at once, then each one needs to go through regular acceptance. This simple registration only makes use of the ID and PATH fields from the list above.  <section><h5>Required roles</h5> ROLE_DEVICE_CONTROL_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="file">The CSV file to be uploaded.</param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ApiResponse of BulkNewDeviceRequest</returns>
        ApiResponse<BulkNewDeviceRequest> PostBulkNewDeviceRequestCollectionResourceWithHttpInfo (System.IO.Stream file, string accept = default(string), string xCumulocityProcessingMode = default(string));
        /// <summary>
        /// Create device credentials
        /// </summary>
        /// <remarks>
        /// Create device credentials.  <section><h5>Required roles</h5> ROLE_DEVICE_BOOTSTRAP </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postDeviceCredentialsCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>DeviceCredentials</returns>
        DeviceCredentials PostDeviceCredentialsCollectionResource (PostDeviceCredentialsCollectionResourceRequest postDeviceCredentialsCollectionResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string));

        /// <summary>
        /// Create device credentials
        /// </summary>
        /// <remarks>
        /// Create device credentials.  <section><h5>Required roles</h5> ROLE_DEVICE_BOOTSTRAP </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postDeviceCredentialsCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ApiResponse of DeviceCredentials</returns>
        ApiResponse<DeviceCredentials> PostDeviceCredentialsCollectionResourceWithHttpInfo (PostDeviceCredentialsCollectionResourceRequest postDeviceCredentialsCollectionResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string));
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Create a bulk device credentials request
        /// </summary>
        /// <remarks>
        /// Create a bulk device credentials request.  Device credentials and basic device representation can be provided within a CSV file which must be UTF-8 or ANSI encoded. The CSV file must have two sections.  The first section is the first line of the CSV file. This line contains column names (headers):  |Name|Mandatory|Description| |- -- |- -- |- -- | |ID|Yes|The external ID of a device.| |CREDENTIALS|Yes*|Password for the device's user. Mandatory, unless AUTH_TYPE is \"CERTIFICATES\", then CREDENTIALS can be skipped.| |AUTH_TYPE|No|Required authentication type for the device's user. If the device uses credentials, this can be skipped or filled with \"BASIC\". Devices that use certificates must set \"CERTIFICATES\".| |TENANT|No|The ID of the tenant for which the registration is executed (only allowed for the management tenant).| |TYPE|No|The type of the device representation.| |NAME|No|The name of the device representation.| |ICCID|No|The ICCID of the device (SIM card number). If the ICCID appears in file, the import adds a fragment `c8y_Mobile.iccid`. The ICCID value is not mandatory for each row, see the example for an HTTP request below.| |IDTYPE|No|The type of the external ID. If IDTYPE doesn't appear in the file, the default value is used. The default value is `c8y_Serial`. The IDTYPE value is not mandatory for each row, see the example for an HTTP request below.| |PATH|No|The path in the groups hierarchy where the device is added. PATH contains the name of each group separated by `/`, that is: `main_group/sub_group/.../last_sub_group`. If a group does not exist, the import creates the group.| |SHELL|No|If this column contains a value of 1, the import adds the SHELL feature to the device (specifically the `c8y_SupportedOperations` fragment). The SHELL value is not mandatory for each row, see the example for an HTTP request below.|  Section two is the rest of the CSV file. Section two contains the device information. The order and quantity of the values must be the same as of the headers.  A separator is automatically obtained from the CSV file. Valid separator values are: `\\t` (tabulation mark), `;` (semicolon) and `,` (comma).  > **⚠️ Important:** The CSV file needs the \"com_cumulocity_model_Agent.active\" header with a value of \"true\" to be added to the request.  > **&#9432; Info:** A bulk registration creates an elementary representation of the device. Then, the device needs to update it to a full representation with its own status. The device is ready to use only after it is updated to the full representation. Also see [credentials upload](https://cumulocity.com/guides/users-guide/device-management/#creds-upload) and [device integration](https://cumulocity.com/guides/device-sdk/rest/#device-integration).  A CSV file can appear in many forms (with regard to the optional tenant column and the occurrence of device information):  * If a user is logged in as the management tenant, then the columns ID, CREDENTIALS and TENANT are mandatory, and the device credentials will be created for the tenant mentioned in the TENANT column. * If a user is logged in as a different tenant, for example, as `sample_tenant`, then the columns ID and CREDENTIALS are mandatory (if the file contains the TENANT column, it is ignored and the device credentials will be created for the tenant that is logged in). * If a user wants to add information about the device, the columns TYPE and NAME must appear in the CSV file. * If a user wants to add information about a SIM card number, the columns TYPE, NAME and ICCID must appear in the CSV file. * If a user wants to change the type of external ID, the columns TYPE, NAME and IDTYPE must appear in the CSV file. * If a user wants to add a device to a group, the columns TYPE, NAME and PATH must appear in the CSV file. * If a user wants to add the SHELL feature, the columns TYPE, NAME and SHELL must appear in the CSV file and the column SHELL must contain a value of 1.  It is possible to define a custom [external ID](#tag/External-IDs) mapping and some custom device properties which are added to newly created devices during registration:  * To add a custom external ID mapping, enter the external ID type as the header of the last column with the prefix \"external-\", for example, to add an external ID mapping of type `c8y_Imei`, enter `external-c8y_Imei` in the last column header. The value of this external ID type should be set in the corresponding column of the data rows. * To add a custom property to a registered device, enter the custom property name as a header, for example, \"myCustomProperty\", and the value would be in the rows below.  The custom device properties mapping has the following limitations:  * Braces '{}' used in data rows will be interpreted as strings of \"{}\". The system will interpret the value as an object when some custom property is added, for example, put `com_cumulocity_model_Agent.active` into the headers row and `true` into the data row to create an object `\"com_cumulocity_model_Agent\": {\"active\": \"true\"}\"`. * It is not possible to add array values via bulk registration.  Example file:  ```csv ID;CREDENTIALS;TYPE;NAME;ICCID;IDTYPE;PATH;SHELL id_101;AbcD1234!1234AbcD;type_of_device;Device 101;111111111;;csv device/subgroup0;1 id_102;AbcD1234!1234AbcD;type_of_device;Device 102;222222222;;csv device/subgroup0;0 id_111;AbcD1234!1234AbcD;type_of_device;Device 111;333333333;c8y_Imei;csv device1/subgroup1;0 id_112;AbcD1234!1234AbcD;type_of_device;Device 112;444444444;;csv device1/subgroup1;1 id_121;AbcD1234!1234AbcD;type_of_device;Device 121;555555555;;csv device1/subgroup2;1 id_122;AbcD1234!1234AbcD;type_of_device;Device 122;;;csv device1/subgroup2; id_131;AbcD1234!1234AbcD;type_of_device;Device 131;;;csv device1/subgroup3; ```  There is also a simple registration method that creates all registration requests at once, then each one needs to go through regular acceptance. This simple registration only makes use of the ID and PATH fields from the list above.  <section><h5>Required roles</h5> ROLE_DEVICE_CONTROL_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="file">The CSV file to be uploaded.</param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of BulkNewDeviceRequest</returns>
        System.Threading.Tasks.Task<BulkNewDeviceRequest> PostBulkNewDeviceRequestCollectionResourceAsync (System.IO.Stream file, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Create a bulk device credentials request
        /// </summary>
        /// <remarks>
        /// Create a bulk device credentials request.  Device credentials and basic device representation can be provided within a CSV file which must be UTF-8 or ANSI encoded. The CSV file must have two sections.  The first section is the first line of the CSV file. This line contains column names (headers):  |Name|Mandatory|Description| |- -- |- -- |- -- | |ID|Yes|The external ID of a device.| |CREDENTIALS|Yes*|Password for the device's user. Mandatory, unless AUTH_TYPE is \"CERTIFICATES\", then CREDENTIALS can be skipped.| |AUTH_TYPE|No|Required authentication type for the device's user. If the device uses credentials, this can be skipped or filled with \"BASIC\". Devices that use certificates must set \"CERTIFICATES\".| |TENANT|No|The ID of the tenant for which the registration is executed (only allowed for the management tenant).| |TYPE|No|The type of the device representation.| |NAME|No|The name of the device representation.| |ICCID|No|The ICCID of the device (SIM card number). If the ICCID appears in file, the import adds a fragment `c8y_Mobile.iccid`. The ICCID value is not mandatory for each row, see the example for an HTTP request below.| |IDTYPE|No|The type of the external ID. If IDTYPE doesn't appear in the file, the default value is used. The default value is `c8y_Serial`. The IDTYPE value is not mandatory for each row, see the example for an HTTP request below.| |PATH|No|The path in the groups hierarchy where the device is added. PATH contains the name of each group separated by `/`, that is: `main_group/sub_group/.../last_sub_group`. If a group does not exist, the import creates the group.| |SHELL|No|If this column contains a value of 1, the import adds the SHELL feature to the device (specifically the `c8y_SupportedOperations` fragment). The SHELL value is not mandatory for each row, see the example for an HTTP request below.|  Section two is the rest of the CSV file. Section two contains the device information. The order and quantity of the values must be the same as of the headers.  A separator is automatically obtained from the CSV file. Valid separator values are: `\\t` (tabulation mark), `;` (semicolon) and `,` (comma).  > **⚠️ Important:** The CSV file needs the \"com_cumulocity_model_Agent.active\" header with a value of \"true\" to be added to the request.  > **&#9432; Info:** A bulk registration creates an elementary representation of the device. Then, the device needs to update it to a full representation with its own status. The device is ready to use only after it is updated to the full representation. Also see [credentials upload](https://cumulocity.com/guides/users-guide/device-management/#creds-upload) and [device integration](https://cumulocity.com/guides/device-sdk/rest/#device-integration).  A CSV file can appear in many forms (with regard to the optional tenant column and the occurrence of device information):  * If a user is logged in as the management tenant, then the columns ID, CREDENTIALS and TENANT are mandatory, and the device credentials will be created for the tenant mentioned in the TENANT column. * If a user is logged in as a different tenant, for example, as `sample_tenant`, then the columns ID and CREDENTIALS are mandatory (if the file contains the TENANT column, it is ignored and the device credentials will be created for the tenant that is logged in). * If a user wants to add information about the device, the columns TYPE and NAME must appear in the CSV file. * If a user wants to add information about a SIM card number, the columns TYPE, NAME and ICCID must appear in the CSV file. * If a user wants to change the type of external ID, the columns TYPE, NAME and IDTYPE must appear in the CSV file. * If a user wants to add a device to a group, the columns TYPE, NAME and PATH must appear in the CSV file. * If a user wants to add the SHELL feature, the columns TYPE, NAME and SHELL must appear in the CSV file and the column SHELL must contain a value of 1.  It is possible to define a custom [external ID](#tag/External-IDs) mapping and some custom device properties which are added to newly created devices during registration:  * To add a custom external ID mapping, enter the external ID type as the header of the last column with the prefix \"external-\", for example, to add an external ID mapping of type `c8y_Imei`, enter `external-c8y_Imei` in the last column header. The value of this external ID type should be set in the corresponding column of the data rows. * To add a custom property to a registered device, enter the custom property name as a header, for example, \"myCustomProperty\", and the value would be in the rows below.  The custom device properties mapping has the following limitations:  * Braces '{}' used in data rows will be interpreted as strings of \"{}\". The system will interpret the value as an object when some custom property is added, for example, put `com_cumulocity_model_Agent.active` into the headers row and `true` into the data row to create an object `\"com_cumulocity_model_Agent\": {\"active\": \"true\"}\"`. * It is not possible to add array values via bulk registration.  Example file:  ```csv ID;CREDENTIALS;TYPE;NAME;ICCID;IDTYPE;PATH;SHELL id_101;AbcD1234!1234AbcD;type_of_device;Device 101;111111111;;csv device/subgroup0;1 id_102;AbcD1234!1234AbcD;type_of_device;Device 102;222222222;;csv device/subgroup0;0 id_111;AbcD1234!1234AbcD;type_of_device;Device 111;333333333;c8y_Imei;csv device1/subgroup1;0 id_112;AbcD1234!1234AbcD;type_of_device;Device 112;444444444;;csv device1/subgroup1;1 id_121;AbcD1234!1234AbcD;type_of_device;Device 121;555555555;;csv device1/subgroup2;1 id_122;AbcD1234!1234AbcD;type_of_device;Device 122;;;csv device1/subgroup2; id_131;AbcD1234!1234AbcD;type_of_device;Device 131;;;csv device1/subgroup3; ```  There is also a simple registration method that creates all registration requests at once, then each one needs to go through regular acceptance. This simple registration only makes use of the ID and PATH fields from the list above.  <section><h5>Required roles</h5> ROLE_DEVICE_CONTROL_ADMIN </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="file">The CSV file to be uploaded.</param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (BulkNewDeviceRequest)</returns>
        System.Threading.Tasks.Task<ApiResponse<BulkNewDeviceRequest>> PostBulkNewDeviceRequestCollectionResourceWithHttpInfoAsync (System.IO.Stream file, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Create device credentials
        /// </summary>
        /// <remarks>
        /// Create device credentials.  <section><h5>Required roles</h5> ROLE_DEVICE_BOOTSTRAP </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postDeviceCredentialsCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of DeviceCredentials</returns>
        System.Threading.Tasks.Task<DeviceCredentials> PostDeviceCredentialsCollectionResourceAsync (PostDeviceCredentialsCollectionResourceRequest postDeviceCredentialsCollectionResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Create device credentials
        /// </summary>
        /// <remarks>
        /// Create device credentials.  <section><h5>Required roles</h5> ROLE_DEVICE_BOOTSTRAP </section> 
        /// </remarks>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postDeviceCredentialsCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (DeviceCredentials)</returns>
        System.Threading.Tasks.Task<ApiResponse<DeviceCredentials>> PostDeviceCredentialsCollectionResourceWithHttpInfoAsync (PostDeviceCredentialsCollectionResourceRequest postDeviceCredentialsCollectionResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken));
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class DeviceCredentialsApi : IDeviceCredentialsApi
    {
        private kern.services.CumulocityClient.Client.ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceCredentialsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public DeviceCredentialsApi(String basePath)
        {
            this.Configuration = new kern.services.CumulocityClient.Client.Configuration { BasePath = basePath };

            ExceptionFactory = kern.services.CumulocityClient.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceCredentialsApi"/> class
        /// </summary>
        /// <returns></returns>
        public DeviceCredentialsApi()
        {
            this.Configuration = kern.services.CumulocityClient.Client.Configuration.Default;

            ExceptionFactory = kern.services.CumulocityClient.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceCredentialsApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public DeviceCredentialsApi(kern.services.CumulocityClient.Client.Configuration configuration = null)
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
        /// Create a bulk device credentials request Create a bulk device credentials request.  Device credentials and basic device representation can be provided within a CSV file which must be UTF-8 or ANSI encoded. The CSV file must have two sections.  The first section is the first line of the CSV file. This line contains column names (headers):  |Name|Mandatory|Description| |- -- |- -- |- -- | |ID|Yes|The external ID of a device.| |CREDENTIALS|Yes*|Password for the device's user. Mandatory, unless AUTH_TYPE is \"CERTIFICATES\", then CREDENTIALS can be skipped.| |AUTH_TYPE|No|Required authentication type for the device's user. If the device uses credentials, this can be skipped or filled with \"BASIC\". Devices that use certificates must set \"CERTIFICATES\".| |TENANT|No|The ID of the tenant for which the registration is executed (only allowed for the management tenant).| |TYPE|No|The type of the device representation.| |NAME|No|The name of the device representation.| |ICCID|No|The ICCID of the device (SIM card number). If the ICCID appears in file, the import adds a fragment `c8y_Mobile.iccid`. The ICCID value is not mandatory for each row, see the example for an HTTP request below.| |IDTYPE|No|The type of the external ID. If IDTYPE doesn't appear in the file, the default value is used. The default value is `c8y_Serial`. The IDTYPE value is not mandatory for each row, see the example for an HTTP request below.| |PATH|No|The path in the groups hierarchy where the device is added. PATH contains the name of each group separated by `/`, that is: `main_group/sub_group/.../last_sub_group`. If a group does not exist, the import creates the group.| |SHELL|No|If this column contains a value of 1, the import adds the SHELL feature to the device (specifically the `c8y_SupportedOperations` fragment). The SHELL value is not mandatory for each row, see the example for an HTTP request below.|  Section two is the rest of the CSV file. Section two contains the device information. The order and quantity of the values must be the same as of the headers.  A separator is automatically obtained from the CSV file. Valid separator values are: `\\t` (tabulation mark), `;` (semicolon) and `,` (comma).  > **⚠️ Important:** The CSV file needs the \"com_cumulocity_model_Agent.active\" header with a value of \"true\" to be added to the request.  > **&#9432; Info:** A bulk registration creates an elementary representation of the device. Then, the device needs to update it to a full representation with its own status. The device is ready to use only after it is updated to the full representation. Also see [credentials upload](https://cumulocity.com/guides/users-guide/device-management/#creds-upload) and [device integration](https://cumulocity.com/guides/device-sdk/rest/#device-integration).  A CSV file can appear in many forms (with regard to the optional tenant column and the occurrence of device information):  * If a user is logged in as the management tenant, then the columns ID, CREDENTIALS and TENANT are mandatory, and the device credentials will be created for the tenant mentioned in the TENANT column. * If a user is logged in as a different tenant, for example, as `sample_tenant`, then the columns ID and CREDENTIALS are mandatory (if the file contains the TENANT column, it is ignored and the device credentials will be created for the tenant that is logged in). * If a user wants to add information about the device, the columns TYPE and NAME must appear in the CSV file. * If a user wants to add information about a SIM card number, the columns TYPE, NAME and ICCID must appear in the CSV file. * If a user wants to change the type of external ID, the columns TYPE, NAME and IDTYPE must appear in the CSV file. * If a user wants to add a device to a group, the columns TYPE, NAME and PATH must appear in the CSV file. * If a user wants to add the SHELL feature, the columns TYPE, NAME and SHELL must appear in the CSV file and the column SHELL must contain a value of 1.  It is possible to define a custom [external ID](#tag/External-IDs) mapping and some custom device properties which are added to newly created devices during registration:  * To add a custom external ID mapping, enter the external ID type as the header of the last column with the prefix \"external-\", for example, to add an external ID mapping of type `c8y_Imei`, enter `external-c8y_Imei` in the last column header. The value of this external ID type should be set in the corresponding column of the data rows. * To add a custom property to a registered device, enter the custom property name as a header, for example, \"myCustomProperty\", and the value would be in the rows below.  The custom device properties mapping has the following limitations:  * Braces '{}' used in data rows will be interpreted as strings of \"{}\". The system will interpret the value as an object when some custom property is added, for example, put `com_cumulocity_model_Agent.active` into the headers row and `true` into the data row to create an object `\"com_cumulocity_model_Agent\": {\"active\": \"true\"}\"`. * It is not possible to add array values via bulk registration.  Example file:  ```csv ID;CREDENTIALS;TYPE;NAME;ICCID;IDTYPE;PATH;SHELL id_101;AbcD1234!1234AbcD;type_of_device;Device 101;111111111;;csv device/subgroup0;1 id_102;AbcD1234!1234AbcD;type_of_device;Device 102;222222222;;csv device/subgroup0;0 id_111;AbcD1234!1234AbcD;type_of_device;Device 111;333333333;c8y_Imei;csv device1/subgroup1;0 id_112;AbcD1234!1234AbcD;type_of_device;Device 112;444444444;;csv device1/subgroup1;1 id_121;AbcD1234!1234AbcD;type_of_device;Device 121;555555555;;csv device1/subgroup2;1 id_122;AbcD1234!1234AbcD;type_of_device;Device 122;;;csv device1/subgroup2; id_131;AbcD1234!1234AbcD;type_of_device;Device 131;;;csv device1/subgroup3; ```  There is also a simple registration method that creates all registration requests at once, then each one needs to go through regular acceptance. This simple registration only makes use of the ID and PATH fields from the list above.  <section><h5>Required roles</h5> ROLE_DEVICE_CONTROL_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="file">The CSV file to be uploaded.</param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>BulkNewDeviceRequest</returns>
        public BulkNewDeviceRequest PostBulkNewDeviceRequestCollectionResource (System.IO.Stream file, string accept = default(string), string xCumulocityProcessingMode = default(string))
        {
             ApiResponse<BulkNewDeviceRequest> localVarResponse = PostBulkNewDeviceRequestCollectionResourceWithHttpInfo(file, accept, xCumulocityProcessingMode);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create a bulk device credentials request Create a bulk device credentials request.  Device credentials and basic device representation can be provided within a CSV file which must be UTF-8 or ANSI encoded. The CSV file must have two sections.  The first section is the first line of the CSV file. This line contains column names (headers):  |Name|Mandatory|Description| |- -- |- -- |- -- | |ID|Yes|The external ID of a device.| |CREDENTIALS|Yes*|Password for the device's user. Mandatory, unless AUTH_TYPE is \"CERTIFICATES\", then CREDENTIALS can be skipped.| |AUTH_TYPE|No|Required authentication type for the device's user. If the device uses credentials, this can be skipped or filled with \"BASIC\". Devices that use certificates must set \"CERTIFICATES\".| |TENANT|No|The ID of the tenant for which the registration is executed (only allowed for the management tenant).| |TYPE|No|The type of the device representation.| |NAME|No|The name of the device representation.| |ICCID|No|The ICCID of the device (SIM card number). If the ICCID appears in file, the import adds a fragment `c8y_Mobile.iccid`. The ICCID value is not mandatory for each row, see the example for an HTTP request below.| |IDTYPE|No|The type of the external ID. If IDTYPE doesn't appear in the file, the default value is used. The default value is `c8y_Serial`. The IDTYPE value is not mandatory for each row, see the example for an HTTP request below.| |PATH|No|The path in the groups hierarchy where the device is added. PATH contains the name of each group separated by `/`, that is: `main_group/sub_group/.../last_sub_group`. If a group does not exist, the import creates the group.| |SHELL|No|If this column contains a value of 1, the import adds the SHELL feature to the device (specifically the `c8y_SupportedOperations` fragment). The SHELL value is not mandatory for each row, see the example for an HTTP request below.|  Section two is the rest of the CSV file. Section two contains the device information. The order and quantity of the values must be the same as of the headers.  A separator is automatically obtained from the CSV file. Valid separator values are: `\\t` (tabulation mark), `;` (semicolon) and `,` (comma).  > **⚠️ Important:** The CSV file needs the \"com_cumulocity_model_Agent.active\" header with a value of \"true\" to be added to the request.  > **&#9432; Info:** A bulk registration creates an elementary representation of the device. Then, the device needs to update it to a full representation with its own status. The device is ready to use only after it is updated to the full representation. Also see [credentials upload](https://cumulocity.com/guides/users-guide/device-management/#creds-upload) and [device integration](https://cumulocity.com/guides/device-sdk/rest/#device-integration).  A CSV file can appear in many forms (with regard to the optional tenant column and the occurrence of device information):  * If a user is logged in as the management tenant, then the columns ID, CREDENTIALS and TENANT are mandatory, and the device credentials will be created for the tenant mentioned in the TENANT column. * If a user is logged in as a different tenant, for example, as `sample_tenant`, then the columns ID and CREDENTIALS are mandatory (if the file contains the TENANT column, it is ignored and the device credentials will be created for the tenant that is logged in). * If a user wants to add information about the device, the columns TYPE and NAME must appear in the CSV file. * If a user wants to add information about a SIM card number, the columns TYPE, NAME and ICCID must appear in the CSV file. * If a user wants to change the type of external ID, the columns TYPE, NAME and IDTYPE must appear in the CSV file. * If a user wants to add a device to a group, the columns TYPE, NAME and PATH must appear in the CSV file. * If a user wants to add the SHELL feature, the columns TYPE, NAME and SHELL must appear in the CSV file and the column SHELL must contain a value of 1.  It is possible to define a custom [external ID](#tag/External-IDs) mapping and some custom device properties which are added to newly created devices during registration:  * To add a custom external ID mapping, enter the external ID type as the header of the last column with the prefix \"external-\", for example, to add an external ID mapping of type `c8y_Imei`, enter `external-c8y_Imei` in the last column header. The value of this external ID type should be set in the corresponding column of the data rows. * To add a custom property to a registered device, enter the custom property name as a header, for example, \"myCustomProperty\", and the value would be in the rows below.  The custom device properties mapping has the following limitations:  * Braces '{}' used in data rows will be interpreted as strings of \"{}\". The system will interpret the value as an object when some custom property is added, for example, put `com_cumulocity_model_Agent.active` into the headers row and `true` into the data row to create an object `\"com_cumulocity_model_Agent\": {\"active\": \"true\"}\"`. * It is not possible to add array values via bulk registration.  Example file:  ```csv ID;CREDENTIALS;TYPE;NAME;ICCID;IDTYPE;PATH;SHELL id_101;AbcD1234!1234AbcD;type_of_device;Device 101;111111111;;csv device/subgroup0;1 id_102;AbcD1234!1234AbcD;type_of_device;Device 102;222222222;;csv device/subgroup0;0 id_111;AbcD1234!1234AbcD;type_of_device;Device 111;333333333;c8y_Imei;csv device1/subgroup1;0 id_112;AbcD1234!1234AbcD;type_of_device;Device 112;444444444;;csv device1/subgroup1;1 id_121;AbcD1234!1234AbcD;type_of_device;Device 121;555555555;;csv device1/subgroup2;1 id_122;AbcD1234!1234AbcD;type_of_device;Device 122;;;csv device1/subgroup2; id_131;AbcD1234!1234AbcD;type_of_device;Device 131;;;csv device1/subgroup3; ```  There is also a simple registration method that creates all registration requests at once, then each one needs to go through regular acceptance. This simple registration only makes use of the ID and PATH fields from the list above.  <section><h5>Required roles</h5> ROLE_DEVICE_CONTROL_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="file">The CSV file to be uploaded.</param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ApiResponse of BulkNewDeviceRequest</returns>
        public ApiResponse<BulkNewDeviceRequest> PostBulkNewDeviceRequestCollectionResourceWithHttpInfo (System.IO.Stream file, string accept = default(string), string xCumulocityProcessingMode = default(string))
        {
            // verify the required parameter 'file' is set
            if (file == null)
                throw new ApiException(400, "Missing required parameter 'file' when calling DeviceCredentialsApi->PostBulkNewDeviceRequestCollectionResource");

            var localVarPath = "/devicecontrol/bulkNewDeviceRequests";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "multipart/form-data"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.bulknewdevicerequest+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter
            if (file != null) localVarFileParams.Add("file", this.Configuration.ApiClient.ParameterToFile("file", file));

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
                Exception exception = ExceptionFactory("PostBulkNewDeviceRequestCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<BulkNewDeviceRequest>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (BulkNewDeviceRequest) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(BulkNewDeviceRequest)));
        }

        /// <summary>
        /// Create a bulk device credentials request Create a bulk device credentials request.  Device credentials and basic device representation can be provided within a CSV file which must be UTF-8 or ANSI encoded. The CSV file must have two sections.  The first section is the first line of the CSV file. This line contains column names (headers):  |Name|Mandatory|Description| |- -- |- -- |- -- | |ID|Yes|The external ID of a device.| |CREDENTIALS|Yes*|Password for the device's user. Mandatory, unless AUTH_TYPE is \"CERTIFICATES\", then CREDENTIALS can be skipped.| |AUTH_TYPE|No|Required authentication type for the device's user. If the device uses credentials, this can be skipped or filled with \"BASIC\". Devices that use certificates must set \"CERTIFICATES\".| |TENANT|No|The ID of the tenant for which the registration is executed (only allowed for the management tenant).| |TYPE|No|The type of the device representation.| |NAME|No|The name of the device representation.| |ICCID|No|The ICCID of the device (SIM card number). If the ICCID appears in file, the import adds a fragment `c8y_Mobile.iccid`. The ICCID value is not mandatory for each row, see the example for an HTTP request below.| |IDTYPE|No|The type of the external ID. If IDTYPE doesn't appear in the file, the default value is used. The default value is `c8y_Serial`. The IDTYPE value is not mandatory for each row, see the example for an HTTP request below.| |PATH|No|The path in the groups hierarchy where the device is added. PATH contains the name of each group separated by `/`, that is: `main_group/sub_group/.../last_sub_group`. If a group does not exist, the import creates the group.| |SHELL|No|If this column contains a value of 1, the import adds the SHELL feature to the device (specifically the `c8y_SupportedOperations` fragment). The SHELL value is not mandatory for each row, see the example for an HTTP request below.|  Section two is the rest of the CSV file. Section two contains the device information. The order and quantity of the values must be the same as of the headers.  A separator is automatically obtained from the CSV file. Valid separator values are: `\\t` (tabulation mark), `;` (semicolon) and `,` (comma).  > **⚠️ Important:** The CSV file needs the \"com_cumulocity_model_Agent.active\" header with a value of \"true\" to be added to the request.  > **&#9432; Info:** A bulk registration creates an elementary representation of the device. Then, the device needs to update it to a full representation with its own status. The device is ready to use only after it is updated to the full representation. Also see [credentials upload](https://cumulocity.com/guides/users-guide/device-management/#creds-upload) and [device integration](https://cumulocity.com/guides/device-sdk/rest/#device-integration).  A CSV file can appear in many forms (with regard to the optional tenant column and the occurrence of device information):  * If a user is logged in as the management tenant, then the columns ID, CREDENTIALS and TENANT are mandatory, and the device credentials will be created for the tenant mentioned in the TENANT column. * If a user is logged in as a different tenant, for example, as `sample_tenant`, then the columns ID and CREDENTIALS are mandatory (if the file contains the TENANT column, it is ignored and the device credentials will be created for the tenant that is logged in). * If a user wants to add information about the device, the columns TYPE and NAME must appear in the CSV file. * If a user wants to add information about a SIM card number, the columns TYPE, NAME and ICCID must appear in the CSV file. * If a user wants to change the type of external ID, the columns TYPE, NAME and IDTYPE must appear in the CSV file. * If a user wants to add a device to a group, the columns TYPE, NAME and PATH must appear in the CSV file. * If a user wants to add the SHELL feature, the columns TYPE, NAME and SHELL must appear in the CSV file and the column SHELL must contain a value of 1.  It is possible to define a custom [external ID](#tag/External-IDs) mapping and some custom device properties which are added to newly created devices during registration:  * To add a custom external ID mapping, enter the external ID type as the header of the last column with the prefix \"external-\", for example, to add an external ID mapping of type `c8y_Imei`, enter `external-c8y_Imei` in the last column header. The value of this external ID type should be set in the corresponding column of the data rows. * To add a custom property to a registered device, enter the custom property name as a header, for example, \"myCustomProperty\", and the value would be in the rows below.  The custom device properties mapping has the following limitations:  * Braces '{}' used in data rows will be interpreted as strings of \"{}\". The system will interpret the value as an object when some custom property is added, for example, put `com_cumulocity_model_Agent.active` into the headers row and `true` into the data row to create an object `\"com_cumulocity_model_Agent\": {\"active\": \"true\"}\"`. * It is not possible to add array values via bulk registration.  Example file:  ```csv ID;CREDENTIALS;TYPE;NAME;ICCID;IDTYPE;PATH;SHELL id_101;AbcD1234!1234AbcD;type_of_device;Device 101;111111111;;csv device/subgroup0;1 id_102;AbcD1234!1234AbcD;type_of_device;Device 102;222222222;;csv device/subgroup0;0 id_111;AbcD1234!1234AbcD;type_of_device;Device 111;333333333;c8y_Imei;csv device1/subgroup1;0 id_112;AbcD1234!1234AbcD;type_of_device;Device 112;444444444;;csv device1/subgroup1;1 id_121;AbcD1234!1234AbcD;type_of_device;Device 121;555555555;;csv device1/subgroup2;1 id_122;AbcD1234!1234AbcD;type_of_device;Device 122;;;csv device1/subgroup2; id_131;AbcD1234!1234AbcD;type_of_device;Device 131;;;csv device1/subgroup3; ```  There is also a simple registration method that creates all registration requests at once, then each one needs to go through regular acceptance. This simple registration only makes use of the ID and PATH fields from the list above.  <section><h5>Required roles</h5> ROLE_DEVICE_CONTROL_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="file">The CSV file to be uploaded.</param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of BulkNewDeviceRequest</returns>
        public async System.Threading.Tasks.Task<BulkNewDeviceRequest> PostBulkNewDeviceRequestCollectionResourceAsync (System.IO.Stream file, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<BulkNewDeviceRequest> localVarResponse = await PostBulkNewDeviceRequestCollectionResourceWithHttpInfoAsync(file, accept, xCumulocityProcessingMode, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create a bulk device credentials request Create a bulk device credentials request.  Device credentials and basic device representation can be provided within a CSV file which must be UTF-8 or ANSI encoded. The CSV file must have two sections.  The first section is the first line of the CSV file. This line contains column names (headers):  |Name|Mandatory|Description| |- -- |- -- |- -- | |ID|Yes|The external ID of a device.| |CREDENTIALS|Yes*|Password for the device's user. Mandatory, unless AUTH_TYPE is \"CERTIFICATES\", then CREDENTIALS can be skipped.| |AUTH_TYPE|No|Required authentication type for the device's user. If the device uses credentials, this can be skipped or filled with \"BASIC\". Devices that use certificates must set \"CERTIFICATES\".| |TENANT|No|The ID of the tenant for which the registration is executed (only allowed for the management tenant).| |TYPE|No|The type of the device representation.| |NAME|No|The name of the device representation.| |ICCID|No|The ICCID of the device (SIM card number). If the ICCID appears in file, the import adds a fragment `c8y_Mobile.iccid`. The ICCID value is not mandatory for each row, see the example for an HTTP request below.| |IDTYPE|No|The type of the external ID. If IDTYPE doesn't appear in the file, the default value is used. The default value is `c8y_Serial`. The IDTYPE value is not mandatory for each row, see the example for an HTTP request below.| |PATH|No|The path in the groups hierarchy where the device is added. PATH contains the name of each group separated by `/`, that is: `main_group/sub_group/.../last_sub_group`. If a group does not exist, the import creates the group.| |SHELL|No|If this column contains a value of 1, the import adds the SHELL feature to the device (specifically the `c8y_SupportedOperations` fragment). The SHELL value is not mandatory for each row, see the example for an HTTP request below.|  Section two is the rest of the CSV file. Section two contains the device information. The order and quantity of the values must be the same as of the headers.  A separator is automatically obtained from the CSV file. Valid separator values are: `\\t` (tabulation mark), `;` (semicolon) and `,` (comma).  > **⚠️ Important:** The CSV file needs the \"com_cumulocity_model_Agent.active\" header with a value of \"true\" to be added to the request.  > **&#9432; Info:** A bulk registration creates an elementary representation of the device. Then, the device needs to update it to a full representation with its own status. The device is ready to use only after it is updated to the full representation. Also see [credentials upload](https://cumulocity.com/guides/users-guide/device-management/#creds-upload) and [device integration](https://cumulocity.com/guides/device-sdk/rest/#device-integration).  A CSV file can appear in many forms (with regard to the optional tenant column and the occurrence of device information):  * If a user is logged in as the management tenant, then the columns ID, CREDENTIALS and TENANT are mandatory, and the device credentials will be created for the tenant mentioned in the TENANT column. * If a user is logged in as a different tenant, for example, as `sample_tenant`, then the columns ID and CREDENTIALS are mandatory (if the file contains the TENANT column, it is ignored and the device credentials will be created for the tenant that is logged in). * If a user wants to add information about the device, the columns TYPE and NAME must appear in the CSV file. * If a user wants to add information about a SIM card number, the columns TYPE, NAME and ICCID must appear in the CSV file. * If a user wants to change the type of external ID, the columns TYPE, NAME and IDTYPE must appear in the CSV file. * If a user wants to add a device to a group, the columns TYPE, NAME and PATH must appear in the CSV file. * If a user wants to add the SHELL feature, the columns TYPE, NAME and SHELL must appear in the CSV file and the column SHELL must contain a value of 1.  It is possible to define a custom [external ID](#tag/External-IDs) mapping and some custom device properties which are added to newly created devices during registration:  * To add a custom external ID mapping, enter the external ID type as the header of the last column with the prefix \"external-\", for example, to add an external ID mapping of type `c8y_Imei`, enter `external-c8y_Imei` in the last column header. The value of this external ID type should be set in the corresponding column of the data rows. * To add a custom property to a registered device, enter the custom property name as a header, for example, \"myCustomProperty\", and the value would be in the rows below.  The custom device properties mapping has the following limitations:  * Braces '{}' used in data rows will be interpreted as strings of \"{}\". The system will interpret the value as an object when some custom property is added, for example, put `com_cumulocity_model_Agent.active` into the headers row and `true` into the data row to create an object `\"com_cumulocity_model_Agent\": {\"active\": \"true\"}\"`. * It is not possible to add array values via bulk registration.  Example file:  ```csv ID;CREDENTIALS;TYPE;NAME;ICCID;IDTYPE;PATH;SHELL id_101;AbcD1234!1234AbcD;type_of_device;Device 101;111111111;;csv device/subgroup0;1 id_102;AbcD1234!1234AbcD;type_of_device;Device 102;222222222;;csv device/subgroup0;0 id_111;AbcD1234!1234AbcD;type_of_device;Device 111;333333333;c8y_Imei;csv device1/subgroup1;0 id_112;AbcD1234!1234AbcD;type_of_device;Device 112;444444444;;csv device1/subgroup1;1 id_121;AbcD1234!1234AbcD;type_of_device;Device 121;555555555;;csv device1/subgroup2;1 id_122;AbcD1234!1234AbcD;type_of_device;Device 122;;;csv device1/subgroup2; id_131;AbcD1234!1234AbcD;type_of_device;Device 131;;;csv device1/subgroup3; ```  There is also a simple registration method that creates all registration requests at once, then each one needs to go through regular acceptance. This simple registration only makes use of the ID and PATH fields from the list above.  <section><h5>Required roles</h5> ROLE_DEVICE_CONTROL_ADMIN </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="file">The CSV file to be uploaded.</param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (BulkNewDeviceRequest)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<BulkNewDeviceRequest>> PostBulkNewDeviceRequestCollectionResourceWithHttpInfoAsync (System.IO.Stream file, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'file' is set
            if (file == null)
                throw new ApiException(400, "Missing required parameter 'file' when calling DeviceCredentialsApi->PostBulkNewDeviceRequestCollectionResource");

            var localVarPath = "/devicecontrol/bulkNewDeviceRequests";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "multipart/form-data"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.bulknewdevicerequest+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter
            if (file != null) localVarFileParams.Add("file", this.Configuration.ApiClient.ParameterToFile("file", file));

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
                Exception exception = ExceptionFactory("PostBulkNewDeviceRequestCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<BulkNewDeviceRequest>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (BulkNewDeviceRequest) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(BulkNewDeviceRequest)));
        }

        /// <summary>
        /// Create device credentials Create device credentials.  <section><h5>Required roles</h5> ROLE_DEVICE_BOOTSTRAP </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postDeviceCredentialsCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>DeviceCredentials</returns>
        public DeviceCredentials PostDeviceCredentialsCollectionResource (PostDeviceCredentialsCollectionResourceRequest postDeviceCredentialsCollectionResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string))
        {
             ApiResponse<DeviceCredentials> localVarResponse = PostDeviceCredentialsCollectionResourceWithHttpInfo(postDeviceCredentialsCollectionResourceRequest, accept, xCumulocityProcessingMode);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create device credentials Create device credentials.  <section><h5>Required roles</h5> ROLE_DEVICE_BOOTSTRAP </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postDeviceCredentialsCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <returns>ApiResponse of DeviceCredentials</returns>
        public ApiResponse<DeviceCredentials> PostDeviceCredentialsCollectionResourceWithHttpInfo (PostDeviceCredentialsCollectionResourceRequest postDeviceCredentialsCollectionResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string))
        {
            // verify the required parameter 'postDeviceCredentialsCollectionResourceRequest' is set
            if (postDeviceCredentialsCollectionResourceRequest == null)
                throw new ApiException(400, "Missing required parameter 'postDeviceCredentialsCollectionResourceRequest' when calling DeviceCredentialsApi->PostDeviceCredentialsCollectionResource");

            var localVarPath = "/devicecontrol/deviceCredentials";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.devicecredentials+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.devicecredentials+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter
            if (postDeviceCredentialsCollectionResourceRequest != null && postDeviceCredentialsCollectionResourceRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(postDeviceCredentialsCollectionResourceRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = postDeviceCredentialsCollectionResourceRequest; // byte array
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
                Exception exception = ExceptionFactory("PostDeviceCredentialsCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<DeviceCredentials>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (DeviceCredentials) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(DeviceCredentials)));
        }

        /// <summary>
        /// Create device credentials Create device credentials.  <section><h5>Required roles</h5> ROLE_DEVICE_BOOTSTRAP </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postDeviceCredentialsCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of DeviceCredentials</returns>
        public async System.Threading.Tasks.Task<DeviceCredentials> PostDeviceCredentialsCollectionResourceAsync (PostDeviceCredentialsCollectionResourceRequest postDeviceCredentialsCollectionResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
             ApiResponse<DeviceCredentials> localVarResponse = await PostDeviceCredentialsCollectionResourceWithHttpInfoAsync(postDeviceCredentialsCollectionResourceRequest, accept, xCumulocityProcessingMode, cancellationToken);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create device credentials Create device credentials.  <section><h5>Required roles</h5> ROLE_DEVICE_BOOTSTRAP </section> 
        /// </summary>
        /// <exception cref="kern.services.CumulocityClient.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="postDeviceCredentialsCollectionResourceRequest"></param>
        /// <param name="accept">Advertises which content types, expressed as MIME types, the client is able to understand. (optional)</param>
        /// <param name="xCumulocityProcessingMode">Used to explicitly control the processing mode of the request. See [Processing mode](#processing-mode) for more details. (optional, default to PERSISTENT)</param>
        /// <param name="cancellationToken">Cancellation Token to cancel request (optional) </param>
        /// <returns>Task of ApiResponse (DeviceCredentials)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<DeviceCredentials>> PostDeviceCredentialsCollectionResourceWithHttpInfoAsync (PostDeviceCredentialsCollectionResourceRequest postDeviceCredentialsCollectionResourceRequest, string accept = default(string), string xCumulocityProcessingMode = default(string), CancellationToken cancellationToken = default(CancellationToken))
        {
            // verify the required parameter 'postDeviceCredentialsCollectionResourceRequest' is set
            if (postDeviceCredentialsCollectionResourceRequest == null)
                throw new ApiException(400, "Missing required parameter 'postDeviceCredentialsCollectionResourceRequest' when calling DeviceCredentialsApi->PostDeviceCredentialsCollectionResource");

            var localVarPath = "/devicecontrol/deviceCredentials";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new List<KeyValuePair<String, String>>();
            var localVarHeaderParams = new Dictionary<String, String>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/vnd.com.nsn.cumulocity.devicecredentials+json"
            };
            String localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/vnd.com.nsn.cumulocity.devicecredentials+json",
                "application/vnd.com.nsn.cumulocity.error+json"
            };
            String localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (accept != null) localVarHeaderParams.Add("Accept", this.Configuration.ApiClient.ParameterToString(accept)); // header parameter
            if (xCumulocityProcessingMode != null) localVarHeaderParams.Add("X-Cumulocity-Processing-Mode", this.Configuration.ApiClient.ParameterToString(xCumulocityProcessingMode)); // header parameter
            if (postDeviceCredentialsCollectionResourceRequest != null && postDeviceCredentialsCollectionResourceRequest.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(postDeviceCredentialsCollectionResourceRequest); // http body (model) parameter
            }
            else
            {
                localVarPostBody = postDeviceCredentialsCollectionResourceRequest; // byte array
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
                Exception exception = ExceptionFactory("PostDeviceCredentialsCollectionResource", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<DeviceCredentials>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => string.Join(",", x.Value)),
                (DeviceCredentials) this.Configuration.ApiClient.Deserialize(localVarResponse, typeof(DeviceCredentials)));
        }

    }
}
