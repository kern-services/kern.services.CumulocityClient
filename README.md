# kern.services.CumulocityClient - the C# library for the Cumulocity IoT

This library was generated using the [OpenAPI generator](https://openapi-generator.tech) project.

The command to generate this library can be found in the file `openapi/generate.sh`:

```shell
docker run --rm \
    -v $PARENT_DIR:/local openapitools/openapi-generator-cli generate \
    -i /local/openapi/c8y-oas-10.15.0.json \
    -g csharp-netcore \
    --additional-properties=netCoreProjectFile=true,packageName=kern.services.CumulocityClient,packageVersion=10.15.0,targetFramework=net7.0 \
    -o /local/
```

The "csharp-netcore" generator does not seem to be documented on the website of the OpenAPI generator project, but it works fine and generates
a dotnet core compatible project while the default "csharp" generator generates a dotnet framework project.

# Cumulocity IoT REST API

The source of the generated API can be found here:
https://cumulocity.com/api/core/10.15.0/

The version of the API is 10.15.0, which is not the latest version of the API, but it is the version currently deployed on the Instance
that we are using. Feel free to create newer versions and provide a pull request.

# REST implementation

This section describes the aspects common to all REST-based interfaces of Cumulocity IoT. The interfaces are based on the [Hypertext Transfer Protocol 1.1](https://tools.ietf.org/html/rfc2616) using [HTTPS](http://en.wikipedia.org/wiki/HTTP_Secure).

## HTTP usage

### Application management

Cumulocity IoT uses a so-called \"application key\" to distinguish requests coming from devices and traffic from applications. If you write an application, pass the following header as part of all requests:

```markup
X-Cumulocity-Application-Key: <APPLICATION_KEY>
```

For example, if you registered your application in the Cumulocity IoT Administration application with the key \"myapp\", your requests should contain the header:

```markup
X-Cumulocity-Application-Key: myapp
```

This makes your application subscribable and billable. If you implement a device, do not pass the key.

> **&#9432; Info:** Make sure that you pass the key in **all** requests coming from an application. If you leave out the key,
> the request will be considered as a device request, and the corresponding device will be marked as \"available\".

### Limited HTTP clients

If you use an HTTP client that can only perform GET and POST methods in HTTP, you can emulate the other methods through an additional \"X-HTTP-METHOD\" header. Simply issue a POST request and add the header, specifying the actual REST method to be executed. For example, to emulate the \"PUT\" (modify) method, you can use:

```http
POST ...
X-HTTP-METHOD: PUT
```

### Processing mode

Every update request (PUT, POST, DELETE) executes with a so-called *processing mode*. The processing modes are as follows:

|Processing mode|Description|
|- --|- --|
|PERSISTENT (default)|All updates will be send both to the Cumulocity IoT database and to real-time processing.|
|TRANSIENT|Updates will be sent only to real-time processing. As part of real-time processing, the user can decide case by case through scripts whether updates should be stored to the database or not.|
|QUIESCENT|The QUIESCENT processing mode behaves like the PERSISTENT processing mode with the exception that no real-time notifications will be sent. Currently, the QUIESCENT processing mode is applicable for measurements, events and managed objects.|
|CEP| With the CEP processing mode, requests will only be processed by CEP or Apama. Currently, the CEP processing mode is applicable for measurements and events only.|

To explicitly control the processing mode of an update request, you can use the \"X-Cumulocity-Processing-Mode\" header with a value of either \"PERSISTENT\", \"TRANSIENT\", \"QUIESCENT\" or \"CEP\":

```markup
X-Cumulocity-Processing-Mode: PERSISTENT
```

> **&#9432; Info:** Events are always delivered to CEP/Apama for all processing modes. This is independent from real-time notifications.

### Authorization

All requests issued to Cumulocity IoT are subject to authorization. To determine the required permissions, see the \"Required role\" entries for the individual requests. To learn more about the different permissions and the concept of ownership in Cumulocity IoT, see [Security aspects > Managing roles and assigning permissions](https://cumulocity.com/guides/concepts/security/#managing-roles-and-assigning-permissions)\".

### Media types

Each type of data is associated with an own media type. The general format of media types is:

```markup
application/vnd.com.nsn.cumulocity.<TYPE>+json;ver=<VERSION>;charset=UTF-8
```

Each media type contains a parameter `ver` indicating the version of the type. At the time of writing, the latest version is \"0.9\". As an example, the media type for an error message in the current version is:

```markup
application/vnd.com.nsn.cumulocity.error+json;ver=0.9;charset=UTF-8
```

Media types are used in HTTP \"Content-Type\" and \"Accept\" headers. If you specify an \"Accept\" header in a POST or PUT request, the response will contain the newly created or updated object. If you do not specify the header, the response body will be empty.

If a media type without the `ver` parameter is given, the oldest available version will be returned by the server. If the \"Accept\" header contains the same media type in multiple versions, the server will return a representation in the latest supported version.

Note that media type values should be treated as case insensitive.

### Date format

Data exchanged with Cumulocity IoT in HTTP requests and responses is encoded in [JSON format](http://www.ietf.org/rfc/rfc4627.txt) and [UTF-8](http://en.wikipedia.org/wiki/UTF-8) character encoding. Timestamps and dates are accepted and emitted by Cumulocity IoT in [ISO 8601](http://www.w3.org/TR/NOTE-datetime) format:

```markup
Date: YYYY-MM-DD
Time: hh:mm:ss±hh:mm
Timestamp: YYYY-MM-DDThh:mm:ss±hh:mm
```

To avoid ambiguity, all times and timestamps must include timezone information. Please take into account that the plus character \"+\" must be encoded as \"%2B\".

### Response Codes

Cumulocity IoT uses conventional HTTP response codes to indicate the success or failure of an API request. Codes in the `2xx` range indicate success. Codes in the `4xx` range indicate a user error. The response provides information on why the request failed (for example, a required parameter was omitted). Codes in the `5xx` range indicate an error with Cumulocity IoT's servers ([these are very rare](https://www.softwareag.cloud/site/sla/cumulocity-iot.html#availability)).

#### HTTP status code summary

|Code|Message|Description|
|:- --:|:- --|:- --|
|200|OK|Everything worked as expected.|
|201|Created|A managed object was created.|
|204|No content|An object was removed.|
|400|Bad Request|The request was unacceptable, often due to missing a required parameter.|
|401|Unauthorized|Authentication has failed, or credentials were required but not provided.|
|403|Forbidden|The authenticated user doesn't have permissions to perform the request.|
|404|Not Found|The requested resource doesn't exist.|
|405|Method not allowed|The employed HTTP method cannot be used on this resource (for example, using PUT on a read-only resource).|
|409|Conflict| The data is correct but it breaks some constraints (for example, application version limit is exceeded). |
|422|Invalid data| Invalid data was sent on the request and/or a query could not be understood.                             |
|422|Unprocessable Entity| The requested resource cannot be updated or mandatory fields are missing on the executed operation.      |
|500<br>503|Server Errors| Something went wrong on Cumulocity IoT's end.                                                            |

## REST usage

### Interpretation of HTTP verbs

The semantics described in the [HTTP specification](http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html#sec9) are used:

* POST creates a new resource. In the response \"Location\" header, the URI of the newly created resource is returned.
* GET retrieves a resource.
* PUT updates an existing resource with the contents of the request.
* DELETE removes a resource. The response will be \"204 No Content\".

If a PUT request only contains parts of a resource (also known as fragments), only those parts are updated. To remove such a part, use a PUT request with a null value for it:

```json
{
  \"resourcePartName\": null
}
```

> **&#9432; Info:** A PUT request cannot update sub-resources that are identified by a separate URI.

### URI space and URI templates

Clients should not make assumptions on the layout of URIs used in requests, but construct URIs from previously returned URIs or URI templates. The [root interface](#tag/Platform-API) provides the entry point for clients.

URI templates contain placeholders in curly braces (for example, `{type}`), which must be filled by the client to produce a URI. As an example, see the following excerpt from the event API response:

```json
{
  \"events\": {
      \"self\": \"https://<TENANT_DOMAIN>/event\"
  },
  \"eventsForSourceAndType\": \"https://<TENANT_DOMAIN>/event/events?type={type}&source={source}\"
}
```

The client must fill the `{type}` and `{source}` placeholders with the desired type and source devices of the events to be returned. The meaning of these placeholders is documented in the respective interface descriptions.

### Interface structure

In general, Cumulocity IoT REST resources are modeled according to the following pattern:

* The starting point are API resources, which will provide access to the actual data through URIs and URI templates to collection resources. For example, the above event API resource provides the `events` URI and the `eventsForSourceAndType` URI to access collections of events.
* Collection resources aggregate member resources and allow creating new member resources in the collection. For example, through the `events` collection resource, new events can be created.
* Finally, individual resources can be edited.

#### Query result paging

Collection resources support paging of data to avoid passing huge data volumes in one block from client to server. GET requests to collections accept two query parameters:

* `currentPage` defines the slice of data to be returned, starting with 1. By default, the first page is returned.
* `pageSize` indicates how many entries of the collection should be returned. By default, 5 entries are returned. The upper limit for one page is currently 2,000 documents. Any larger requested page size is trimmed to the upper limit.
* `withTotalElements` will yield the total number of elements in the statistics object. This is only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)).
* `withTotalPages` will yield the total number of pages in the statistics object. This is only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)).

For convenience, collection resources provide `next` and `prev` links to retrieve the next and previous pages of the results. The following is an example response for managed object collections (the contents of the array `managedObjects` have been omitted):

```json
{
  \"self\" : \"https://<TENANT_DOMAIN>/inventory/managedObjects?pageSize=5&currentPage=2\",
  \"managedObjects\" : [...],
  \"statistics\" : {
    \"totalPages\" : 7,
    \"pageSize\" : 5,
    \"currentPage\" : 2,
    \"totalElements\" : 34
  },
  \"prev\" : \"https://<TENANT_DOMAIN>/inventory/managedObjects?pageSize=5&currentPage=1\",
  \"next\" : \"https://<TENANT_DOMAIN>/inventory/managedObjects?pageSize=5&currentPage=3\"
}
```

The `totalPages` and `totalElements` properties can be expensive to compute, hence they are not returned by default for [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). To include any of them in the result, add the query parameters `withTotalPages=true` and/or `withTotalElements=true`.

> **&#9432; Info:** If inventory roles are applied to a user, a query by the user may return less than `pageSize` results even if there are more results in total.

#### Query result paging for users with restricted access

If a user does not have a global role for reading data from the API resource but rather has [inventory roles](https://cumulocity.com/guides/users-guide/administration/#inventory) for reading only particular documents, there are some differences in query result paging:

* In some circumstances the response may contain less than `pageSize` and `totalElements` elements though there is more data in the database accessible for the user.
* In some circumstances `next` and `prev` links may appear in the response though there is no more data in the database accessible for the user.
* The property `currentPage` of the response does not contain the page number but the offset of the next element not yet processed by the querying mechanism.
* The query parameters `withTotalPages=true` and `withTotalElements=true` have no effect, and the value of the `totalPages` and `totalElements` properties is always null.

The above behavior results from the fact that the querying mechanism is iterating maximally over 10 * max(pageSize, 100) documents per request, and it stops even though the full page of data accessible for the user could not be collected. When the next page is requested the querying mechanism starts the iteration where it completed the previous time.

#### Query result by time interval

Use the following query parameters to obtain data for a specified time interval:

* `dateFrom` - Start date or date and time.
* `dateTo` - End date or date and time.

Example formats:

```markup
dateTo=2019-04-20
dateTo=2019-04-20T08:30:00.000Z
```

Parameters are optional. Values provided with those parameters are inclusive.

> **⚠️ Important:** If your servers are not running in UTC (Coordinated Universal Time), any date passed without timezone will be handled as UTC, regardless of the server local timezone. This might lead to a difference regarding the date/time range included in the results.

### Root interface

To discover the URIs to the various interfaces of Cumulocity IoT, it provides a \"root\" interface.
This root interface aggregates all the underlying API resources.
See the [Platform API](#tag/Platform-API) endpoint.
For more information on the different API resources, consult the respective API sections.

## Generic media types

### Error

The error type provides further information on the reason of a failed request.

Content-Type: application/vnd.com.nsn.cumulocity.error+json

|Name|Type|Description|
|- --|- --|- --|
|error|string|Error type formatted as `<RESOURCE_TYPE>/<ERROR_NAME>`. For example, an object not found in the inventory is reported as `inventory/notFound`.|
|info|string|URL to an error description on the Internet.|
|message|string|Short text description of the error|

### Paging statistics

Paging statistics for collection of resources.

Content-Type: application/vnd.com.nsn.cumulocity.pagingstatistics+json

|Name|Type|Description|
|- --|- --|- --|
|currentPage|integer|The current returned page within the full result set, starting at \"1\".|
|pageSize|integer|Maximum number of records contained in this query.|
|totalElements|integer|The total number of results (elements).|
|totalPages|integer|The total number of paginated results (pages).|

> **&#9432; Info:** The `totalPages` and `totalElements` properties are not returned by default in the response. To include any of them, add the query parameters `withTotalPages=true` and/or `withTotalElements=true`. Be aware of [differences in query result paging for users with restricted access](#query-result-paging-for-users-with-restricted-access).

> **&#9432; Info:** To improve performance, the `totalPages` and `totalElements` statistics are cached for 10 seconds.

# Device management library

The device management library has moved.
Visit the [device management library](https://cumulocity.com/guides/reference/device-management-library/#overview) in the *Reference guide*.

# Sensor library

The sensor library has moved.
Visit the [sensor library](https://cumulocity.com/guides/reference/sensor-library/#overview) in the *Reference guide*.

# Login options

When you sign up for an account on the [Cumulocity IoT platform](https://cumulocity.com/), for example, by using a free trial, you will be provided with a dedicated URL address for your tenant. All requests to the platform must be authenticated employing your tenant ID, Cumulocity IoT user (c8yuser for short) and password. Cumulocity IoT offers the following forms of authentication:

* Basic authentication (Basic)
* OAI-Secure authentication (OAI-Secure)
* SSO with authentication code grant (SSO)
* JSON Web Token authentication (JWT, deprecated)

You can check your login options with a GET call to the endpoint <kbd><a href=\"#tag/Login-options\">/tenant/loginOptions</a></kbd>.


This C# SDK is automatically generated by the [OpenAPI Generator](https://openapi-generator.tech) project:

- API version: Release 10.15.0
- SDK version: 10.15.0
- Build package: org.openapitools.codegen.languages.CSharpNetCoreClientCodegen
    For more information, please visit [https://cumulocity.com/guides/about-doc/contacting-support/](https://cumulocity.com/guides/about-doc/contacting-support/)

<a name="frameworks-supported"></a>
## Frameworks supported

<a name="dependencies"></a>
## Dependencies

- [RestSharp](https://www.nuget.org/packages/RestSharp) - 106.13.0 or later
- [Json.NET](https://www.nuget.org/packages/Newtonsoft.Json/) - 13.0.1 or later
- [JsonSubTypes](https://www.nuget.org/packages/JsonSubTypes/) - 1.8.0 or later
- [System.ComponentModel.Annotations](https://www.nuget.org/packages/System.ComponentModel.Annotations) - 5.0.0 or later

The DLLs included in the package may not be the latest version. We recommend using [NuGet](https://docs.nuget.org/consume/installing-nuget) to obtain the latest version of the packages:
```
Install-Package RestSharp
Install-Package Newtonsoft.Json
Install-Package JsonSubTypes
Install-Package System.ComponentModel.Annotations
```

NOTE: RestSharp versions greater than 105.1.0 have a bug which causes file uploads to fail. See [RestSharp#742](https://github.com/restsharp/RestSharp/issues/742).
NOTE: RestSharp for .Net Core creates a new socket for each api call, which can lead to a socket exhaustion problem. See [RestSharp#1406](https://github.com/restsharp/RestSharp/issues/1406).

<a name="installation"></a>
## Installation
Run the following command to generate the DLL
- [Mac/Linux] `/bin/sh build.sh`
- [Windows] `build.bat`

Then include the DLL (under the `bin` folder) in the C# project, and use the namespaces:
```csharp
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;
```
<a name="packaging"></a>
## Packaging

A `.nuspec` is included with the project. You can follow the Nuget quickstart to [create](https://docs.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package#create-the-package) and [publish](https://docs.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package#publish-the-package) packages.

This `.nuspec` uses placeholders from the `.csproj`, so build the `.csproj` directly:

```
nuget pack -Build -OutputDirectory out kern.services.CumulocityClient.csproj
```

Then, publish to a [local feed](https://docs.microsoft.com/en-us/nuget/hosting-packages/local-feeds) or [other host](https://docs.microsoft.com/en-us/nuget/hosting-packages/overview) and consume the new package via Nuget as usual.

<a name="usage"></a>
## Usage

To use the API client with a HTTP proxy, setup a `System.Net.WebProxy`
```csharp
Configuration c = new Configuration();
System.Net.WebProxy webProxy = new System.Net.WebProxy("http://myProxyUrl:80/");
webProxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
c.Proxy = webProxy;
```

<a name="getting-started"></a>
## Getting Started

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class Example
    {
        public static void Main()
        {

            Configuration config = new Configuration();
            config.BasePath = "https://<TENANT_DOMAIN>";
            // Configure HTTP basic authorization: Basic
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";
            // Configure Bearer token for authorization: OAI-Secure
            config.AccessToken = "YOUR_BEARER_TOKEN";
            // Configure OAuth2 access token for authorization: SSO
            config.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new AlarmAPIApi(config);

            try
            {
                // Retrieve URIs to collections of alarms
                AlarmsApiResource result = apiInstance.GetAlarmsApiResource();
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling AlarmAPIApi.GetAlarmsApiResource: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }

        }
    }
}
```

<a name="documentation-for-api-endpoints"></a>
## Documentation for API Endpoints

All URIs are relative to *https://<TENANT_DOMAIN>*

Class | Method | HTTP request | Description
------------ | ------------- | ------------- | -------------
*AlarmAPIApi* | [**GetAlarmsApiResource**](docs/AlarmAPIApi.md#getalarmsapiresource) | **GET** /alarm | Retrieve URIs to collections of alarms
*AlarmsApi* | [**DeleteAlarmCollectionResource**](docs/AlarmsApi.md#deletealarmcollectionresource) | **DELETE** /alarm/alarms | Remove alarm collections
*AlarmsApi* | [**GetAlarmCollectionCountResource**](docs/AlarmsApi.md#getalarmcollectioncountresource) | **GET** /alarm/alarms/count | Retrieve the total number of alarms
*AlarmsApi* | [**GetAlarmCollectionResource**](docs/AlarmsApi.md#getalarmcollectionresource) | **GET** /alarm/alarms | Retrieve all alarms
*AlarmsApi* | [**GetAlarmResource**](docs/AlarmsApi.md#getalarmresource) | **GET** /alarm/alarms/{id} | Retrieve a specific alarm
*AlarmsApi* | [**PostAlarmCollectionResource**](docs/AlarmsApi.md#postalarmcollectionresource) | **POST** /alarm/alarms | Create an alarm
*AlarmsApi* | [**PutAlarmCollectionResource**](docs/AlarmsApi.md#putalarmcollectionresource) | **PUT** /alarm/alarms | Update alarm collections
*AlarmsApi* | [**PutAlarmResource**](docs/AlarmsApi.md#putalarmresource) | **PUT** /alarm/alarms/{id} | Update a specific alarm
*ApplicationAPIApi* | [**GetApplicationManagementApiResource**](docs/ApplicationAPIApi.md#getapplicationmanagementapiresource) | **GET** /application | Retrieve URIs to collections of applications
*ApplicationBinariesApi* | [**DeleteBinaryApplicationContentResourceById**](docs/ApplicationBinariesApi.md#deletebinaryapplicationcontentresourcebyid) | **DELETE** /application/applications/{id}/binaries/{binaryId} | Delete a specific application attachment
*ApplicationBinariesApi* | [**GetBinaryApplicationContentResource**](docs/ApplicationBinariesApi.md#getbinaryapplicationcontentresource) | **GET** /application/applications/{id}/binaries | Retrieve all application attachments
*ApplicationBinariesApi* | [**GetBinaryApplicationContentResourceById**](docs/ApplicationBinariesApi.md#getbinaryapplicationcontentresourcebyid) | **GET** /application/applications/{id}/binaries/{binaryId} | Retrieve a specific application attachment
*ApplicationBinariesApi* | [**PostBinaryApplicationContentResource**](docs/ApplicationBinariesApi.md#postbinaryapplicationcontentresource) | **POST** /application/applications/{id}/binaries | Upload an application attachment
*ApplicationVersionsApi* | [**DeleteApplicationVersionResource**](docs/ApplicationVersionsApi.md#deleteapplicationversionresource) | **DELETE** /application/applications/{id}/versions | Delete a specific version of an application
*ApplicationVersionsApi* | [**GetApplicationVersionCollectionResource**](docs/ApplicationVersionsApi.md#getapplicationversioncollectionresource) | **GET** /application/applications/{id}/versions | Retrieve all versions of an application
*ApplicationVersionsApi* | [**GetApplicationVersionResource**](docs/ApplicationVersionsApi.md#getapplicationversionresource) | **GET** /application/applications/{id}/versions?version&#x3D;1.0 | Retrieve a specific version of an application
*ApplicationVersionsApi* | [**PostApplicationVersionResource**](docs/ApplicationVersionsApi.md#postapplicationversionresource) | **POST** /application/applications/{id}/versions | Create an application version
*ApplicationVersionsApi* | [**PutApplicationVersionResource**](docs/ApplicationVersionsApi.md#putapplicationversionresource) | **PUT** /application/applications/{id}/versions/{version} | Replace an application version's tags
*ApplicationsApi* | [**DeleteApplicationResource**](docs/ApplicationsApi.md#deleteapplicationresource) | **DELETE** /application/applications/{id} | Delete an application
*ApplicationsApi* | [**GetAbstractApplicationCollectionResource**](docs/ApplicationsApi.md#getabstractapplicationcollectionresource) | **GET** /application/applications | Retrieve all applications
*ApplicationsApi* | [**GetApplicationResource**](docs/ApplicationsApi.md#getapplicationresource) | **GET** /application/applications/{id} | Retrieve a specific application
*ApplicationsApi* | [**GetApplicationsByNameCollectionResource**](docs/ApplicationsApi.md#getapplicationsbynamecollectionresource) | **GET** /application/applicationsByName/{name} | Retrieve applications by name
*ApplicationsApi* | [**GetApplicationsByOwnerCollectionResource**](docs/ApplicationsApi.md#getapplicationsbyownercollectionresource) | **GET** /application/applicationsByOwner/{tenantId} | Retrieve applications by owner
*ApplicationsApi* | [**GetApplicationsByTenantCollectionResource**](docs/ApplicationsApi.md#getapplicationsbytenantcollectionresource) | **GET** /application/applicationsByTenant/{tenantId} | Retrieve applications by tenant
*ApplicationsApi* | [**GetApplicationsByUserCollectionResource**](docs/ApplicationsApi.md#getapplicationsbyusercollectionresource) | **GET** /application/applicationsByUser/{username} | Retrieve applications by user
*ApplicationsApi* | [**PostApplicationCollectionResource**](docs/ApplicationsApi.md#postapplicationcollectionresource) | **POST** /application/applications | Create an application
*ApplicationsApi* | [**PostApplicationResource**](docs/ApplicationsApi.md#postapplicationresource) | **POST** /application/applications/{id}/clone | Copy an application
*ApplicationsApi* | [**PutApplicationResource**](docs/ApplicationsApi.md#putapplicationresource) | **PUT** /application/applications/{id} | Update a specific application
*AttachmentsApi* | [**DeleteEventBinaryResource**](docs/AttachmentsApi.md#deleteeventbinaryresource) | **DELETE** /event/events/{id}/binaries | Remove the attached file from a specific event
*AttachmentsApi* | [**GetEventBinaryResource**](docs/AttachmentsApi.md#geteventbinaryresource) | **GET** /event/events/{id}/binaries | Retrieve the attached file of a specific event
*AttachmentsApi* | [**PostEventBinaryResource**](docs/AttachmentsApi.md#posteventbinaryresource) | **POST** /event/events/{id}/binaries | Attach a file to a specific event
*AttachmentsApi* | [**PutEventBinaryResource**](docs/AttachmentsApi.md#puteventbinaryresource) | **PUT** /event/events/{id}/binaries | Replace the attached file of a specific event
*AuditAPIApi* | [**GetAuditApiResource**](docs/AuditAPIApi.md#getauditapiresource) | **GET** /audit | Retrieve URIs to collections of audits
*AuditsApi* | [**GetAuditRecordCollectionResource**](docs/AuditsApi.md#getauditrecordcollectionresource) | **GET** /audit/auditRecords | Retrieve all audit records
*AuditsApi* | [**GetAuditRecordResource**](docs/AuditsApi.md#getauditrecordresource) | **GET** /audit/auditRecords/{id} | Retrieve a specific audit record
*AuditsApi* | [**PostAuditRecordCollectionResource**](docs/AuditsApi.md#postauditrecordcollectionresource) | **POST** /audit/auditRecords | Create an audit record
*BinariesApi* | [**DeleteBinariesResource**](docs/BinariesApi.md#deletebinariesresource) | **DELETE** /inventory/binaries/{id} | Remove a stored file
*BinariesApi* | [**GetBinariesCollectionResource**](docs/BinariesApi.md#getbinariescollectionresource) | **GET** /inventory/binaries | Retrieve the stored files
*BinariesApi* | [**GetBinariesResource**](docs/BinariesApi.md#getbinariesresource) | **GET** /inventory/binaries/{id} | Retrieve a stored file
*BinariesApi* | [**PostBinariesCollectionResource**](docs/BinariesApi.md#postbinariescollectionresource) | **POST** /inventory/binaries | Upload a file
*BinariesApi* | [**PutBinariesResource**](docs/BinariesApi.md#putbinariesresource) | **PUT** /inventory/binaries/{id} | Replace a file
*BootstrapUserApi* | [**GetApplicationUserRepresentation**](docs/BootstrapUserApi.md#getapplicationuserrepresentation) | **GET** /application/applications/{id}/bootstrapUser | Retrieve the bootstrap user for a specific application
*BulkOperationsApi* | [**DeleteBulkOperationResource**](docs/BulkOperationsApi.md#deletebulkoperationresource) | **DELETE** /devicecontrol/bulkoperations/{id} | Delete a specific bulk operation
*BulkOperationsApi* | [**GetBulkOperationCollectionResource**](docs/BulkOperationsApi.md#getbulkoperationcollectionresource) | **GET** /devicecontrol/bulkoperations | Retrieve a list of bulk operations
*BulkOperationsApi* | [**GetBulkOperationResource**](docs/BulkOperationsApi.md#getbulkoperationresource) | **GET** /devicecontrol/bulkoperations/{id} | Retrieve a specific bulk operation
*BulkOperationsApi* | [**PostBulkOperationCollectionResource**](docs/BulkOperationsApi.md#postbulkoperationcollectionresource) | **POST** /devicecontrol/bulkoperations | Create a bulk operation
*BulkOperationsApi* | [**PutBulkOperationResource**](docs/BulkOperationsApi.md#putbulkoperationresource) | **PUT** /devicecontrol/bulkoperations/{id} | Update a specific bulk operation
*ChildOperationsApi* | [**DeleteManagedObjectChildAdditionResource**](docs/ChildOperationsApi.md#deletemanagedobjectchildadditionresource) | **DELETE** /inventory/managedObjects/{id}/childAdditions/{childId} | Remove a specific child addition from its parent
*ChildOperationsApi* | [**DeleteManagedObjectChildAdditionResourceMultiple**](docs/ChildOperationsApi.md#deletemanagedobjectchildadditionresourcemultiple) | **DELETE** /inventory/managedObjects/{id}/childAdditions | Remove specific child additions from its parent
*ChildOperationsApi* | [**DeleteManagedObjectChildAssetResource**](docs/ChildOperationsApi.md#deletemanagedobjectchildassetresource) | **DELETE** /inventory/managedObjects/{id}/childAssets/{childId} | Remove a specific child asset from its parent
*ChildOperationsApi* | [**DeleteManagedObjectChildAssetResourceMultiple**](docs/ChildOperationsApi.md#deletemanagedobjectchildassetresourcemultiple) | **DELETE** /inventory/managedObjects/{id}/childAssets | Remove specific child assets from its parent
*ChildOperationsApi* | [**DeleteManagedObjectChildDeviceResource**](docs/ChildOperationsApi.md#deletemanagedobjectchilddeviceresource) | **DELETE** /inventory/managedObjects/{id}/childDevices/{childId} | Remove a specific child device from its parent
*ChildOperationsApi* | [**DeleteManagedObjectChildDeviceResourceMultiple**](docs/ChildOperationsApi.md#deletemanagedobjectchilddeviceresourcemultiple) | **DELETE** /inventory/managedObjects/{id}/childDevices | Remove specific child devices from its parent
*ChildOperationsApi* | [**GetManagedObjectChildAdditionResource**](docs/ChildOperationsApi.md#getmanagedobjectchildadditionresource) | **GET** /inventory/managedObjects/{id}/childAdditions/{childId} | Retrieve a specific child addition of a specific managed object
*ChildOperationsApi* | [**GetManagedObjectChildAdditionsResource**](docs/ChildOperationsApi.md#getmanagedobjectchildadditionsresource) | **GET** /inventory/managedObjects/{id}/childAdditions | Retrieve all child additions of a specific managed object
*ChildOperationsApi* | [**GetManagedObjectChildAssetResource**](docs/ChildOperationsApi.md#getmanagedobjectchildassetresource) | **GET** /inventory/managedObjects/{id}/childAssets/{childId} | Retrieve a specific child asset of a specific managed object
*ChildOperationsApi* | [**GetManagedObjectChildAssetsResource**](docs/ChildOperationsApi.md#getmanagedobjectchildassetsresource) | **GET** /inventory/managedObjects/{id}/childAssets | Retrieve all child assets of a specific managed object
*ChildOperationsApi* | [**GetManagedObjectChildDeviceResource**](docs/ChildOperationsApi.md#getmanagedobjectchilddeviceresource) | **GET** /inventory/managedObjects/{id}/childDevices/{childId} | Retrieve a specific child device of a specific managed object
*ChildOperationsApi* | [**GetManagedObjectChildDevicesResource**](docs/ChildOperationsApi.md#getmanagedobjectchilddevicesresource) | **GET** /inventory/managedObjects/{id}/childDevices | Retrieve all child devices of a specific managed object
*ChildOperationsApi* | [**PostManagedObjectChildAdditionsResource**](docs/ChildOperationsApi.md#postmanagedobjectchildadditionsresource) | **POST** /inventory/managedObjects/{id}/childAdditions | Assign a managed object as child addition
*ChildOperationsApi* | [**PostManagedObjectChildAssetsResource**](docs/ChildOperationsApi.md#postmanagedobjectchildassetsresource) | **POST** /inventory/managedObjects/{id}/childAssets | Assign a managed object as child asset
*ChildOperationsApi* | [**PostManagedObjectChildDevicesResource**](docs/ChildOperationsApi.md#postmanagedobjectchilddevicesresource) | **POST** /inventory/managedObjects/{id}/childDevices | Assign a managed object as child device
*CurrentApplicationApi* | [**GetApplicationUserCollectionRepresentation**](docs/CurrentApplicationApi.md#getapplicationusercollectionrepresentation) | **GET** /application/currentApplication/subscriptions | Retrieve the subscribed users of the current application
*CurrentApplicationApi* | [**GetCurrentApplicationResource**](docs/CurrentApplicationApi.md#getcurrentapplicationresource) | **GET** /application/currentApplication | Retrieve the current application
*CurrentApplicationApi* | [**GetCurrentApplicationResourceSettings**](docs/CurrentApplicationApi.md#getcurrentapplicationresourcesettings) | **GET** /application/currentApplication/settings | Retrieve the current application settings
*CurrentApplicationApi* | [**PutCurrentApplicationResource**](docs/CurrentApplicationApi.md#putcurrentapplicationresource) | **PUT** /application/currentApplication | Update the current application
*CurrentUserApi* | [**GetCurrentUserResource**](docs/CurrentUserApi.md#getcurrentuserresource) | **GET** /user/currentUser | Retrieve the current user
*CurrentUserApi* | [**GetCurrentUserTfaTotpResourceActivity**](docs/CurrentUserApi.md#getcurrentusertfatotpresourceactivity) | **GET** /user/currentUser/totpSecret/activity | Returns the activation state of the two-factor authentication feature.
*CurrentUserApi* | [**PostCurrentUserTfaTotpResource**](docs/CurrentUserApi.md#postcurrentusertfatotpresource) | **POST** /user/currentUser/totpSecret | Generate secret to set up TFA
*CurrentUserApi* | [**PostCurrentUserTfaTotpResourceActivity**](docs/CurrentUserApi.md#postcurrentusertfatotpresourceactivity) | **POST** /user/currentUser/totpSecret/activity | Activates or deactivates the two-factor authentication feature
*CurrentUserApi* | [**PostCurrentUserTfaTotpResourceVerify**](docs/CurrentUserApi.md#postcurrentusertfatotpresourceverify) | **POST** /user/currentUser/totpSecret/verify | Verify TFA code
*CurrentUserApi* | [**PutCurrentUserPasswordResource**](docs/CurrentUserApi.md#putcurrentuserpasswordresource) | **PUT** /user/currentUser/password | Update the current user's password
*CurrentUserApi* | [**PutCurrentUserResource**](docs/CurrentUserApi.md#putcurrentuserresource) | **PUT** /user/currentUser | Update the current user
*DeviceControlAPIApi* | [**GetDeviceControlApiResource**](docs/DeviceControlAPIApi.md#getdevicecontrolapiresource) | **GET** /devicecontrol | Retrieve URIs to collections of operations
*DeviceCredentialsApi* | [**PostBulkNewDeviceRequestCollectionResource**](docs/DeviceCredentialsApi.md#postbulknewdevicerequestcollectionresource) | **POST** /devicecontrol/bulkNewDeviceRequests | Create a bulk device credentials request
*DeviceCredentialsApi* | [**PostDeviceCredentialsCollectionResource**](docs/DeviceCredentialsApi.md#postdevicecredentialscollectionresource) | **POST** /devicecontrol/deviceCredentials | Create device credentials
*DevicePermissionsApi* | [**GetDevicePermissionsResource**](docs/DevicePermissionsApi.md#getdevicepermissionsresource) | **GET** /user/devicePermissions/{id} | Returns all device permissions assignments
*DevicePermissionsApi* | [**PutDevicePermissionsResource**](docs/DevicePermissionsApi.md#putdevicepermissionsresource) | **PUT** /user/devicePermissions/{id} | Updates the device permissions assignments
*DeviceStatisticsApi* | [**GetTenantDeviceStatisticsDailyCollection**](docs/DeviceStatisticsApi.md#gettenantdevicestatisticsdailycollection) | **GET** /tenant/statistics/device/{tenantId}/daily/{date} | Retrieve daily device statistics
*DeviceStatisticsApi* | [**GetTenantDeviceStatisticsMonthlyCollection**](docs/DeviceStatisticsApi.md#gettenantdevicestatisticsmonthlycollection) | **GET** /tenant/statistics/device/{tenantId}/monthly/{date} | Retrieve monthly device statistics
*EventAPIApi* | [**GetEventsApiResource**](docs/EventAPIApi.md#geteventsapiresource) | **GET** /event | Retrieve URIs to collections of events
*EventsApi* | [**DeleteEventCollectionResource**](docs/EventsApi.md#deleteeventcollectionresource) | **DELETE** /event/events | Remove event collections
*EventsApi* | [**DeleteEventResource**](docs/EventsApi.md#deleteeventresource) | **DELETE** /event/events/{id} | Remove a specific event
*EventsApi* | [**GetEventCollectionResource**](docs/EventsApi.md#geteventcollectionresource) | **GET** /event/events | Retrieve all events
*EventsApi* | [**GetEventResource**](docs/EventsApi.md#geteventresource) | **GET** /event/events/{id} | Retrieve a specific event
*EventsApi* | [**PostEventCollectionResource**](docs/EventsApi.md#posteventcollectionresource) | **POST** /event/events | Create an event
*EventsApi* | [**PutEventResource**](docs/EventsApi.md#puteventresource) | **PUT** /event/events/{id} | Update a specific event
*ExternalIDsApi* | [**DeleteExternalIDResource**](docs/ExternalIDsApi.md#deleteexternalidresource) | **DELETE** /identity/externalIds/{type}/{externalId} | Remove a specific external ID
*ExternalIDsApi* | [**GetExternalIDCollectionResource**](docs/ExternalIDsApi.md#getexternalidcollectionresource) | **GET** /identity/globalIds/{id}/externalIds | Retrieve all external IDs of a specific managed object
*ExternalIDsApi* | [**GetExternalIDResource**](docs/ExternalIDsApi.md#getexternalidresource) | **GET** /identity/externalIds/{type}/{externalId} | Retrieve a specific external ID
*ExternalIDsApi* | [**PostExternalIDCollectionResource**](docs/ExternalIDsApi.md#postexternalidcollectionresource) | **POST** /identity/globalIds/{id}/externalIds | Create an external ID
*GroupsApi* | [**DeleteGroupByIdResource**](docs/GroupsApi.md#deletegroupbyidresource) | **DELETE** /user/{tenantId}/groups/{groupId} | Delete a specific user group for a specific tenant
*GroupsApi* | [**GetGroupByIdResource**](docs/GroupsApi.md#getgroupbyidresource) | **GET** /user/{tenantId}/groups/{groupId} | Retrieve a specific user group for a specific tenant
*GroupsApi* | [**GetGroupByNameResource**](docs/GroupsApi.md#getgroupbynameresource) | **GET** /user/{tenantId}/groupByName/{groupName} | Retrieve a user group by group name for a specific tenant
*GroupsApi* | [**GetGroupCollectionResource**](docs/GroupsApi.md#getgroupcollectionresource) | **GET** /user/{tenantId}/groups | Retrieve all user groups of a specific tenant
*GroupsApi* | [**GetGroupReferenceCollectionResource**](docs/GroupsApi.md#getgroupreferencecollectionresource) | **GET** /user/{tenantId}/users/{userId}/groups | Get all user groups for specific user in a specific tenant
*GroupsApi* | [**PostGroupCollectionResource**](docs/GroupsApi.md#postgroupcollectionresource) | **POST** /user/{tenantId}/groups | Create a user group for a specific tenant
*GroupsApi* | [**PutGroupByIdResource**](docs/GroupsApi.md#putgroupbyidresource) | **PUT** /user/{tenantId}/groups/{groupId} | Update a specific user group for a specific tenant
*IdentityAPIApi* | [**GetIdentityApiResource**](docs/IdentityAPIApi.md#getidentityapiresource) | **GET** /identity | Retrieve URIs to collections of external IDs
*InventoryAPIApi* | [**GetInventoryApiResource**](docs/InventoryAPIApi.md#getinventoryapiresource) | **GET** /inventory | Retrieve URIs to collections of managed objects
*InventoryRolesApi* | [**DeleteInventoryAssignmentResourceById**](docs/InventoryRolesApi.md#deleteinventoryassignmentresourcebyid) | **DELETE** /user/{tenantId}/users/{userId}/roles/inventory/{id} | Remove a specific inventory role assigned to a user
*InventoryRolesApi* | [**DeleteInventoryRoleResourceId**](docs/InventoryRolesApi.md#deleteinventoryroleresourceid) | **DELETE** /user/inventoryroles/{id} | Remove a specific inventory role
*InventoryRolesApi* | [**GetInventoryAssignmentResource**](docs/InventoryRolesApi.md#getinventoryassignmentresource) | **GET** /user/{tenantId}/users/{userId}/roles/inventory | Retrieve all inventory roles assigned to a user
*InventoryRolesApi* | [**GetInventoryAssignmentResourceById**](docs/InventoryRolesApi.md#getinventoryassignmentresourcebyid) | **GET** /user/{tenantId}/users/{userId}/roles/inventory/{id} | Retrieve a specific inventory role assigned to a user
*InventoryRolesApi* | [**GetInventoryRoleResource**](docs/InventoryRolesApi.md#getinventoryroleresource) | **GET** /user/inventoryroles | Retrieve all inventory roles
*InventoryRolesApi* | [**GetInventoryRoleResourceId**](docs/InventoryRolesApi.md#getinventoryroleresourceid) | **GET** /user/inventoryroles/{id} | Retrieve a specific inventory role
*InventoryRolesApi* | [**PostInventoryAssignmentResource**](docs/InventoryRolesApi.md#postinventoryassignmentresource) | **POST** /user/{tenantId}/users/{userId}/roles/inventory | Assign an inventory role to a user
*InventoryRolesApi* | [**PostInventoryRoleResource**](docs/InventoryRolesApi.md#postinventoryroleresource) | **POST** /user/inventoryroles | Create an inventory role
*InventoryRolesApi* | [**PutInventoryAssignmentResourceById**](docs/InventoryRolesApi.md#putinventoryassignmentresourcebyid) | **PUT** /user/{tenantId}/users/{userId}/roles/inventory/{id} | Update a specific inventory role assigned to a user
*InventoryRolesApi* | [**PutInventoryRoleResourceId**](docs/InventoryRolesApi.md#putinventoryroleresourceid) | **PUT** /user/inventoryroles/{id} | Update a specific inventory role
*LoginOptionsApi* | [**GetLoginOptionCollectionResource**](docs/LoginOptionsApi.md#getloginoptioncollectionresource) | **GET** /tenant/loginOptions | Retrieve the login options
*LoginOptionsApi* | [**PostLoginOptionCollectionResource**](docs/LoginOptionsApi.md#postloginoptioncollectionresource) | **POST** /tenant/loginOptions | Create a login option
*LoginOptionsApi* | [**PutAccessLoginOptionResource**](docs/LoginOptionsApi.md#putaccessloginoptionresource) | **PUT** /tenant/loginOptions/{type_or_id}/restrict | Update a tenant's access to the login option
*LoginTokensApi* | [**PostLoginFormBody**](docs/LoginTokensApi.md#postloginformbody) | **POST** /tenant/oauth/token | Obtain an access token
*LoginTokensApi* | [**PostLoginFormCookie**](docs/LoginTokensApi.md#postloginformcookie) | **POST** /tenant/oauth | Obtain access tokens in cookies
*ManagedObjectsApi* | [**DeleteManagedObjectResource**](docs/ManagedObjectsApi.md#deletemanagedobjectresource) | **DELETE** /inventory/managedObjects/{id} | Remove a specific managed object
*ManagedObjectsApi* | [**GetLastAvailabilityManagedObjectResource**](docs/ManagedObjectsApi.md#getlastavailabilitymanagedobjectresource) | **GET** /inventory/managedObjects/{id}/availability | Retrieve the latest availability date of a specific managed object
*ManagedObjectsApi* | [**GetManagedObjectCollectionResource**](docs/ManagedObjectsApi.md#getmanagedobjectcollectionresource) | **GET** /inventory/managedObjects | Retrieve all managed objects
*ManagedObjectsApi* | [**GetManagedObjectResource**](docs/ManagedObjectsApi.md#getmanagedobjectresource) | **GET** /inventory/managedObjects/{id} | Retrieve a specific managed object
*ManagedObjectsApi* | [**GetManagedObjectUserResource**](docs/ManagedObjectsApi.md#getmanagedobjectuserresource) | **GET** /inventory/managedObjects/{id}/user | Retrieve the username and state of a specific managed object
*ManagedObjectsApi* | [**GetSupportedMeasurementsManagedObjectResource**](docs/ManagedObjectsApi.md#getsupportedmeasurementsmanagedobjectresource) | **GET** /inventory/managedObjects/{id}/supportedMeasurements | Retrieve all supported measurement fragments of a specific managed object
*ManagedObjectsApi* | [**GetSupportedSeriesManagedObjectResource**](docs/ManagedObjectsApi.md#getsupportedseriesmanagedobjectresource) | **GET** /inventory/managedObjects/{id}/supportedSeries | Retrieve all supported measurement fragments and series of a specific managed object
*ManagedObjectsApi* | [**PostManagedObjectCollectionResource**](docs/ManagedObjectsApi.md#postmanagedobjectcollectionresource) | **POST** /inventory/managedObjects | Create a managed object
*ManagedObjectsApi* | [**PutManagedObjectResource**](docs/ManagedObjectsApi.md#putmanagedobjectresource) | **PUT** /inventory/managedObjects/{id} | Update a specific managed object
*ManagedObjectsApi* | [**PutManagedObjectUserResource**](docs/ManagedObjectsApi.md#putmanagedobjectuserresource) | **PUT** /inventory/managedObjects/{id}/user | Update the user's details of a specific managed object
*MeasurementAPIApi* | [**GetMeasurementApiResource**](docs/MeasurementAPIApi.md#getmeasurementapiresource) | **GET** /measurement | Retrieve URIs to collections of measurements
*MeasurementsApi* | [**DeleteMeasurementCollectionResource**](docs/MeasurementsApi.md#deletemeasurementcollectionresource) | **DELETE** /measurement/measurements | Remove measurement collections
*MeasurementsApi* | [**DeleteMeasurementResource**](docs/MeasurementsApi.md#deletemeasurementresource) | **DELETE** /measurement/measurements/{id} | Remove a specific measurement
*MeasurementsApi* | [**GetMeasurementCollectionResource**](docs/MeasurementsApi.md#getmeasurementcollectionresource) | **GET** /measurement/measurements | Retrieve all measurements
*MeasurementsApi* | [**GetMeasurementResource**](docs/MeasurementsApi.md#getmeasurementresource) | **GET** /measurement/measurements/{id} | Retrieve a specific measurement
*MeasurementsApi* | [**GetMeasurementSeriesResource**](docs/MeasurementsApi.md#getmeasurementseriesresource) | **GET** /measurement/measurements/series | Retrieve a list of series and their values
*MeasurementsApi* | [**PostMeasurementCollectionResource**](docs/MeasurementsApi.md#postmeasurementcollectionresource) | **POST** /measurement/measurements | Create a measurement
*NewDeviceRequestsApi* | [**DeleteNewDeviceRequestResource**](docs/NewDeviceRequestsApi.md#deletenewdevicerequestresource) | **DELETE** /devicecontrol/newDeviceRequests/{requestId} | Delete a specific new device request
*NewDeviceRequestsApi* | [**GetNewDeviceRequestCollectionResource**](docs/NewDeviceRequestsApi.md#getnewdevicerequestcollectionresource) | **GET** /devicecontrol/newDeviceRequests | Retrieve a list of new device requests
*NewDeviceRequestsApi* | [**GetNewDeviceRequestResource**](docs/NewDeviceRequestsApi.md#getnewdevicerequestresource) | **GET** /devicecontrol/newDeviceRequests/{requestId} | Retrieve a specific new device request
*NewDeviceRequestsApi* | [**PostNewDeviceRequestCollectionResource**](docs/NewDeviceRequestsApi.md#postnewdevicerequestcollectionresource) | **POST** /devicecontrol/newDeviceRequests | Create a new device request
*NewDeviceRequestsApi* | [**PutNewDeviceRequestResource**](docs/NewDeviceRequestsApi.md#putnewdevicerequestresource) | **PUT** /devicecontrol/newDeviceRequests/{requestId} | Update a specific new device request status
*Notification20APIApi* | [**GetNotificationApiResource**](docs/Notification20APIApi.md#getnotificationapiresource) | **GET** /notification2 | Retrieve URIs to collections of notification subscriptions
*OperationsApi* | [**DeleteOperationCollectionResource**](docs/OperationsApi.md#deleteoperationcollectionresource) | **DELETE** /devicecontrol/operations | Delete a list of operations
*OperationsApi* | [**GetOperationCollectionResource**](docs/OperationsApi.md#getoperationcollectionresource) | **GET** /devicecontrol/operations | Retrieve a list of operations
*OperationsApi* | [**GetOperationResource**](docs/OperationsApi.md#getoperationresource) | **GET** /devicecontrol/operations/{id} | Retrieve a specific operation
*OperationsApi* | [**PostOperationCollectionResource**](docs/OperationsApi.md#postoperationcollectionresource) | **POST** /devicecontrol/operations | Create an operation
*OperationsApi* | [**PutOperationResource**](docs/OperationsApi.md#putoperationresource) | **PUT** /devicecontrol/operations/{id} | Update a specific operation status
*OptionsApi* | [**DeleteOptionResource**](docs/OptionsApi.md#deleteoptionresource) | **DELETE** /tenant/options/{category}/{key} | Remove a specific option
*OptionsApi* | [**GetCategoryOptionResource**](docs/OptionsApi.md#getcategoryoptionresource) | **GET** /tenant/options/{category} | Retrieve all options by category
*OptionsApi* | [**GetOptionCollectionResource**](docs/OptionsApi.md#getoptioncollectionresource) | **GET** /tenant/options | Retrieve all options
*OptionsApi* | [**GetOptionResource**](docs/OptionsApi.md#getoptionresource) | **GET** /tenant/options/{category}/{key} | Retrieve a specific option
*OptionsApi* | [**PostOptionCollectionResource**](docs/OptionsApi.md#postoptioncollectionresource) | **POST** /tenant/options | Create an option
*OptionsApi* | [**PutCategoryOptionResource**](docs/OptionsApi.md#putcategoryoptionresource) | **PUT** /tenant/options/{category} | Update options by category
*OptionsApi* | [**PutOptionResource**](docs/OptionsApi.md#putoptionresource) | **PUT** /tenant/options/{category}/{key} | Update a specific option
*PlatformAPIApi* | [**GetPlatformApiResource**](docs/PlatformAPIApi.md#getplatformapiresource) | **GET** /platform | Retrieve URIs to collection platform objects
*RealTimeNotificationAPIApi* | [**PostNotificationRealtimeResource**](docs/RealTimeNotificationAPIApi.md#postnotificationrealtimeresource) | **POST** /notification/realtime | Responsive communication
*RetentionRulesApi* | [**DeleteRetentionRuleResource**](docs/RetentionRulesApi.md#deleteretentionruleresource) | **DELETE** /retention/retentions/{id} | Remove a retention rule
*RetentionRulesApi* | [**GetRetentionRuleCollectionResource**](docs/RetentionRulesApi.md#getretentionrulecollectionresource) | **GET** /retention/retentions | Retrieve all retention rules
*RetentionRulesApi* | [**GetRetentionRuleResource**](docs/RetentionRulesApi.md#getretentionruleresource) | **GET** /retention/retentions/{id} | Retrieve a retention rule
*RetentionRulesApi* | [**PostRetentionRuleCollectionResource**](docs/RetentionRulesApi.md#postretentionrulecollectionresource) | **POST** /retention/retentions | Create a retention rule
*RetentionRulesApi* | [**PutRetentionRuleResource**](docs/RetentionRulesApi.md#putretentionruleresource) | **PUT** /retention/retentions/{id} | Update a retention rule
*RolesApi* | [**DeleteGroupRoleReferenceResource**](docs/RolesApi.md#deletegrouprolereferenceresource) | **DELETE** /user/{tenantId}/groups/{groupId}/roles/{roleId} | Unassign a specific role for a specific user group in a specific tenant
*RolesApi* | [**DeleteUserRoleReferenceResource**](docs/RolesApi.md#deleteuserrolereferenceresource) | **DELETE** /user/{tenantId}/users/{userId}/roles/{roleId} | Unassign a specific role from a specific user in a specific tenant
*RolesApi* | [**GetGroupsRoleReferenceCollectionResource**](docs/RolesApi.md#getgroupsrolereferencecollectionresource) | **GET** /user/{tenantId}/groups/{groupId}/roles | Retrieve all roles assigned to a specific user group in a specific tenant
*RolesApi* | [**GetRoleCollectionResource**](docs/RolesApi.md#getrolecollectionresource) | **GET** /user/roles | Retrieve all user roles
*RolesApi* | [**GetRoleCollectionResourceByName**](docs/RolesApi.md#getrolecollectionresourcebyname) | **GET** /user/roles/{name} | Retrieve a user role by name
*RolesApi* | [**PostGroupsRoleReferenceCollectionResource**](docs/RolesApi.md#postgroupsrolereferencecollectionresource) | **POST** /user/{tenantId}/groups/{groupId}/roles | Assign a role to a specific user group in a specific tenant
*RolesApi* | [**PostUsersRoleReferenceCollectionResource**](docs/RolesApi.md#postusersrolereferencecollectionresource) | **POST** /user/{tenantId}/users/{userId}/roles | Assign a role to specific user in a specific tenant
*SubscriptionsApi* | [**DeleteNotificationSubscriptionBySourceResource**](docs/SubscriptionsApi.md#deletenotificationsubscriptionbysourceresource) | **DELETE** /notification2/subscriptions | Remove subscriptions by source
*SubscriptionsApi* | [**DeleteNotificationSubscriptionResource**](docs/SubscriptionsApi.md#deletenotificationsubscriptionresource) | **DELETE** /notification2/subscriptions/{id} | Remove a specific subscription
*SubscriptionsApi* | [**GetNotificationSubscriptionCollectionResource**](docs/SubscriptionsApi.md#getnotificationsubscriptioncollectionresource) | **GET** /notification2/subscriptions | Retrieve all subscriptions
*SubscriptionsApi* | [**GetNotificationSubscriptionResource**](docs/SubscriptionsApi.md#getnotificationsubscriptionresource) | **GET** /notification2/subscriptions/{id} | Retrieve a specific subscription
*SubscriptionsApi* | [**PostNotificationSubscriptionResource**](docs/SubscriptionsApi.md#postnotificationsubscriptionresource) | **POST** /notification2/subscriptions | Create a subscription
*SystemOptionsApi* | [**GetSystemOptionCollectionResource**](docs/SystemOptionsApi.md#getsystemoptioncollectionresource) | **GET** /tenant/system/options | Retrieve all system options
*SystemOptionsApi* | [**GetSystemOptionResource**](docs/SystemOptionsApi.md#getsystemoptionresource) | **GET** /tenant/system/options/{category}/{key} | Retrieve a specific system option
*TenantAPIApi* | [**GetTenantsApiResource**](docs/TenantAPIApi.md#gettenantsapiresource) | **GET** /tenant | Retrieve URIs to collections of tenants
*TenantApplicationsApi* | [**DeleteTenantApplicationReferenceResource**](docs/TenantApplicationsApi.md#deletetenantapplicationreferenceresource) | **DELETE** /tenant/tenants/{tenantId}/applications/{applicationId} | Unsubscribe from an application
*TenantApplicationsApi* | [**GetTenantApplicationReferenceCollectionResource**](docs/TenantApplicationsApi.md#gettenantapplicationreferencecollectionresource) | **GET** /tenant/tenants/{tenantId}/applications | Retrieve subscribed applications
*TenantApplicationsApi* | [**PostTenantApplicationReferenceCollectionResource**](docs/TenantApplicationsApi.md#posttenantapplicationreferencecollectionresource) | **POST** /tenant/tenants/{tenantId}/applications | Subscribe to an application
*TenantsApi* | [**DeleteTenantResource**](docs/TenantsApi.md#deletetenantresource) | **DELETE** /tenant/tenants/{tenantId} | Remove a specific tenant
*TenantsApi* | [**GetCurrentTenantResource**](docs/TenantsApi.md#getcurrenttenantresource) | **GET** /tenant/currentTenant | Retrieve the current tenant
*TenantsApi* | [**GetTenantCollectionResource**](docs/TenantsApi.md#gettenantcollectionresource) | **GET** /tenant/tenants | Retrieve all subtenants
*TenantsApi* | [**GetTenantResource**](docs/TenantsApi.md#gettenantresource) | **GET** /tenant/tenants/{tenantId} | Retrieve a specific tenant
*TenantsApi* | [**GetTenantsTfaResourceTfa**](docs/TenantsApi.md#gettenantstfaresourcetfa) | **GET** /tenant/tenants/{tenantId}/tfa | Retrieve TFA settings of a specific tenant
*TenantsApi* | [**PostTenantCollectionResource**](docs/TenantsApi.md#posttenantcollectionresource) | **POST** /tenant/tenants | Create a tenant
*TenantsApi* | [**PutTenantResource**](docs/TenantsApi.md#puttenantresource) | **PUT** /tenant/tenants/{tenantId} | Update a specific tenant
*TokensApi* | [**PostNotificationTokenResource**](docs/TokensApi.md#postnotificationtokenresource) | **POST** /notification2/token | Create a notification token
*TokensApi* | [**PostNotificationTokenUnsubscribeResource**](docs/TokensApi.md#postnotificationtokenunsubscriberesource) | **POST** /notification2/unsubscribe | Unsubscribe a subscriber
*TrustedCertificatesApi* | [**DeleteTrustedCertificateResource**](docs/TrustedCertificatesApi.md#deletetrustedcertificateresource) | **DELETE** /tenant/tenants/{tenantId}/trusted-certificates/{fingerprint} | Remove a stored certificate
*TrustedCertificatesApi* | [**GetTrustedCertificateCollectionResource**](docs/TrustedCertificatesApi.md#gettrustedcertificatecollectionresource) | **GET** /tenant/tenants/{tenantId}/trusted-certificates | Retrieve all stored certificates
*TrustedCertificatesApi* | [**GetTrustedCertificateResource**](docs/TrustedCertificatesApi.md#gettrustedcertificateresource) | **GET** /tenant/tenants/{tenantId}/trusted-certificates/{fingerprint} | Retrieve a stored certificate
*TrustedCertificatesApi* | [**PostConfirmedTrustedCertificatePopResource**](docs/TrustedCertificatesApi.md#postconfirmedtrustedcertificatepopresource) | **POST** /tenant/tenants/{tenantId}/trusted-certificates-pop/{fingerprint}/confirmed | Confirm an already uploaded certificate
*TrustedCertificatesApi* | [**PostTrustedCertificateCollectionResource**](docs/TrustedCertificatesApi.md#posttrustedcertificatecollectionresource) | **POST** /tenant/tenants/{tenantId}/trusted-certificates | Add a new certificate
*TrustedCertificatesApi* | [**PostTrustedCertificateCollectionResourceBulk**](docs/TrustedCertificatesApi.md#posttrustedcertificatecollectionresourcebulk) | **POST** /tenant/tenants/{tenantId}/trusted-certificates/bulk | Add multiple certificates
*TrustedCertificatesApi* | [**PostTrustedCertificatePopResource**](docs/TrustedCertificatesApi.md#posttrustedcertificatepopresource) | **POST** /tenant/tenants/{tenantId}/trusted-certificates-pop/{fingerprint}/pop | Provide the proof of possession for an already uploaded certificate
*TrustedCertificatesApi* | [**PostVerificationCodeTrustedCertificatesPopResource**](docs/TrustedCertificatesApi.md#postverificationcodetrustedcertificatespopresource) | **POST** /tenant/tenants/{tenantId}/trusted-certificates-pop/{fingerprint}/verification-code | Generate a verification code for the proof of possession operation for the given certificate
*TrustedCertificatesApi* | [**PutTrustedCertificateResource**](docs/TrustedCertificatesApi.md#puttrustedcertificateresource) | **PUT** /tenant/tenants/{tenantId}/trusted-certificates/{fingerprint} | Update a stored certificate
*UsageStatisticsApi* | [**GetSummaryAllTenantsUsageStatistics**](docs/UsageStatisticsApi.md#getsummaryalltenantsusagestatistics) | **GET** /tenant/statistics/allTenantsSummary | Retrieve a summary of all usage statistics
*UsageStatisticsApi* | [**GetSummaryUsageStatistics**](docs/UsageStatisticsApi.md#getsummaryusagestatistics) | **GET** /tenant/statistics/summary | Retrieve a usage statistics summary
*UsageStatisticsApi* | [**GetTenantUsageStatisticsCollectionResource**](docs/UsageStatisticsApi.md#gettenantusagestatisticscollectionresource) | **GET** /tenant/statistics | Retrieve statistics of the current tenant
*UsageStatisticsApi* | [**GetTenantUsageStatisticsFileById**](docs/UsageStatisticsApi.md#gettenantusagestatisticsfilebyid) | **GET** /tenant/statistics/files/{id} | Retrieve a usage statistics file
*UsageStatisticsApi* | [**GetTenantUsageStatisticsFileCollectionResource**](docs/UsageStatisticsApi.md#gettenantusagestatisticsfilecollectionresource) | **GET** /tenant/statistics/files | Retrieve usage statistics files metadata
*UsageStatisticsApi* | [**GetTenantUsageStatisticsLatestFile**](docs/UsageStatisticsApi.md#gettenantusagestatisticslatestfile) | **GET** /tenant/statistics/files/latest/{month} | Retrieve the latest usage statistics file
*UsageStatisticsApi* | [**PostGenerateStatisticsFileRequest**](docs/UsageStatisticsApi.md#postgeneratestatisticsfilerequest) | **POST** /tenant/statistics/files | Generate a statistics file report
*UserAPIApi* | [**GetUserApiResource**](docs/UserAPIApi.md#getuserapiresource) | **GET** /user | Retrieve URIs to collections of users, groups and roles
*UsersApi* | [**DeleteUserReferenceResource**](docs/UsersApi.md#deleteuserreferenceresource) | **DELETE** /user/{tenantId}/groups/{groupId}/users/{userId} | Remove a specific user from a specific user group of a specific tenant
*UsersApi* | [**DeleteUserResource**](docs/UsersApi.md#deleteuserresource) | **DELETE** /user/{tenantId}/users/{userId} | Delete a specific user for a specific tenant
*UsersApi* | [**GetUserCollectionResource**](docs/UsersApi.md#getusercollectionresource) | **GET** /user/{tenantId}/users | Retrieve all users for a specific tenant
*UsersApi* | [**GetUserReferenceCollectionResource**](docs/UsersApi.md#getuserreferencecollectionresource) | **GET** /user/{tenantId}/groups/{groupId}/users | Retrieve the users of a specific user group of a specific tenant
*UsersApi* | [**GetUserResource**](docs/UsersApi.md#getuserresource) | **GET** /user/{tenantId}/users/{userId} | Retrieve a specific user for a specific tenant
*UsersApi* | [**GetUsersByNameResource**](docs/UsersApi.md#getusersbynameresource) | **GET** /user/{tenantId}/userByName/{username} | Retrieve a user by username in a specific tenant
*UsersApi* | [**GetUsersTfaResource**](docs/UsersApi.md#getuserstfaresource) | **GET** /user/{tenantId}/users/{userId}/tfa | Retrieve the TFA settings of a specific user
*UsersApi* | [**PostLogoutUser**](docs/UsersApi.md#postlogoutuser) | **POST** /user/logout | Terminate a user's session
*UsersApi* | [**PostUserCollectionResource**](docs/UsersApi.md#postusercollectionresource) | **POST** /user/{tenantId}/users | Create a user for a specific tenant
*UsersApi* | [**PostUserReferenceCollectionResource**](docs/UsersApi.md#postuserreferencecollectionresource) | **POST** /user/{tenantId}/groups/{groupId}/users | Add a user to a specific user group of a specific tenant
*UsersApi* | [**PutUserChangePasswordResource**](docs/UsersApi.md#putuserchangepasswordresource) | **PUT** /user/{tenantId}/users/{userId}/password | Update a specific user's password of a specific tenant
*UsersApi* | [**PutUserResource**](docs/UsersApi.md#putuserresource) | **PUT** /user/{tenantId}/users/{userId} | Update a specific user for a specific tenant


<a name="documentation-for-models"></a>
## Documentation for Models

 - [Model.AccessToken](docs/AccessToken.md)
 - [Model.Alarm](docs/Alarm.md)
 - [Model.AlarmCollection](docs/AlarmCollection.md)
 - [Model.AlarmSource](docs/AlarmSource.md)
 - [Model.AlarmsApiResource](docs/AlarmsApiResource.md)
 - [Model.AlarmsApiResourceAlarms](docs/AlarmsApiResourceAlarms.md)
 - [Model.Application](docs/Application.md)
 - [Model.ApplicationApiResource](docs/ApplicationApiResource.md)
 - [Model.ApplicationBinaries](docs/ApplicationBinaries.md)
 - [Model.ApplicationBinariesAttachmentsInner](docs/ApplicationBinariesAttachmentsInner.md)
 - [Model.ApplicationCollection](docs/ApplicationCollection.md)
 - [Model.ApplicationManifest](docs/ApplicationManifest.md)
 - [Model.ApplicationManifestProbe](docs/ApplicationManifestProbe.md)
 - [Model.ApplicationManifestProbeHttpGet](docs/ApplicationManifestProbeHttpGet.md)
 - [Model.ApplicationOwner](docs/ApplicationOwner.md)
 - [Model.ApplicationOwnerTenant](docs/ApplicationOwnerTenant.md)
 - [Model.ApplicationReference](docs/ApplicationReference.md)
 - [Model.ApplicationReferenceCollection](docs/ApplicationReferenceCollection.md)
 - [Model.ApplicationSettingsInner](docs/ApplicationSettingsInner.md)
 - [Model.ApplicationSettingsInnerValueSchema](docs/ApplicationSettingsInnerValueSchema.md)
 - [Model.ApplicationUserCollection](docs/ApplicationUserCollection.md)
 - [Model.ApplicationUserCollectionUsersInner](docs/ApplicationUserCollectionUsersInner.md)
 - [Model.ApplicationVersion](docs/ApplicationVersion.md)
 - [Model.ApplicationVersionCollection](docs/ApplicationVersionCollection.md)
 - [Model.ApplicationVersionTag](docs/ApplicationVersionTag.md)
 - [Model.AuditApiResource](docs/AuditApiResource.md)
 - [Model.AuditApiResourceAuditRecords](docs/AuditApiResourceAuditRecords.md)
 - [Model.AuditRecord](docs/AuditRecord.md)
 - [Model.AuditRecordC8yMetadata](docs/AuditRecordC8yMetadata.md)
 - [Model.AuditRecordChangesInner](docs/AuditRecordChangesInner.md)
 - [Model.AuditRecordChangesInnerNewValue](docs/AuditRecordChangesInnerNewValue.md)
 - [Model.AuditRecordChangesInnerNewValueOneOf](docs/AuditRecordChangesInnerNewValueOneOf.md)
 - [Model.AuditRecordChangesInnerPreviousValue](docs/AuditRecordChangesInnerPreviousValue.md)
 - [Model.AuditRecordCollection](docs/AuditRecordCollection.md)
 - [Model.AuditRecordSource](docs/AuditRecordSource.md)
 - [Model.AuthConfig](docs/AuthConfig.md)
 - [Model.AuthConfigAccess](docs/AuthConfigAccess.md)
 - [Model.AuthConfigAccessTokenToUserDataMapping](docs/AuthConfigAccessTokenToUserDataMapping.md)
 - [Model.AuthConfigAuthorizationRequest](docs/AuthConfigAuthorizationRequest.md)
 - [Model.AuthConfigLogoutRequest](docs/AuthConfigLogoutRequest.md)
 - [Model.AuthConfigOnNewUser](docs/AuthConfigOnNewUser.md)
 - [Model.AuthConfigOnNewUserDynamicMapping](docs/AuthConfigOnNewUserDynamicMapping.md)
 - [Model.AuthConfigOnNewUserDynamicMappingConfiguration](docs/AuthConfigOnNewUserDynamicMappingConfiguration.md)
 - [Model.AuthConfigOnNewUserDynamicMappingMappingsInner](docs/AuthConfigOnNewUserDynamicMappingMappingsInner.md)
 - [Model.AuthConfigRefreshRequest](docs/AuthConfigRefreshRequest.md)
 - [Model.AuthConfigSignatureVerificationConfig](docs/AuthConfigSignatureVerificationConfig.md)
 - [Model.AuthConfigSignatureVerificationConfigAad](docs/AuthConfigSignatureVerificationConfigAad.md)
 - [Model.AuthConfigSignatureVerificationConfigAdfsManifest](docs/AuthConfigSignatureVerificationConfigAdfsManifest.md)
 - [Model.AuthConfigSignatureVerificationConfigJwks](docs/AuthConfigSignatureVerificationConfigJwks.md)
 - [Model.AuthConfigSignatureVerificationConfigManual](docs/AuthConfigSignatureVerificationConfigManual.md)
 - [Model.AuthConfigSignatureVerificationConfigManualCertificates](docs/AuthConfigSignatureVerificationConfigManualCertificates.md)
 - [Model.AuthConfigTokenRequest](docs/AuthConfigTokenRequest.md)
 - [Model.AuthConfigUserIdConfig](docs/AuthConfigUserIdConfig.md)
 - [Model.BasicAuthenticationRestrictions](docs/BasicAuthenticationRestrictions.md)
 - [Model.Binary](docs/Binary.md)
 - [Model.BinaryCollection](docs/BinaryCollection.md)
 - [Model.BootstrapUser](docs/BootstrapUser.md)
 - [Model.BulkNewDeviceRequest](docs/BulkNewDeviceRequest.md)
 - [Model.BulkNewDeviceRequestCredentialUpdatedListInner](docs/BulkNewDeviceRequestCredentialUpdatedListInner.md)
 - [Model.BulkNewDeviceRequestFailedCreationListInner](docs/BulkNewDeviceRequestFailedCreationListInner.md)
 - [Model.BulkOperation](docs/BulkOperation.md)
 - [Model.BulkOperationCollection](docs/BulkOperationCollection.md)
 - [Model.BulkOperationProgress](docs/BulkOperationProgress.md)
 - [Model.C8yAccelerationMeasurement](docs/C8yAccelerationMeasurement.md)
 - [Model.C8yAccelerationMeasurementAcceleration](docs/C8yAccelerationMeasurementAcceleration.md)
 - [Model.C8yActiveAlarmsStatus](docs/C8yActiveAlarmsStatus.md)
 - [Model.C8yAgent](docs/C8yAgent.md)
 - [Model.C8yAvailability](docs/C8yAvailability.md)
 - [Model.C8yAvailabilityStatus](docs/C8yAvailabilityStatus.md)
 - [Model.C8yCellInfo](docs/C8yCellInfo.md)
 - [Model.C8yCellTower](docs/C8yCellTower.md)
 - [Model.C8yCommand](docs/C8yCommand.md)
 - [Model.C8yCommunicationMode](docs/C8yCommunicationMode.md)
 - [Model.C8yConfiguration](docs/C8yConfiguration.md)
 - [Model.C8yConnection](docs/C8yConnection.md)
 - [Model.C8yCurrentMeasurement](docs/C8yCurrentMeasurement.md)
 - [Model.C8yCurrentMeasurementCurrent](docs/C8yCurrentMeasurementCurrent.md)
 - [Model.C8yDistanceMeasurement](docs/C8yDistanceMeasurement.md)
 - [Model.C8yDistanceMeasurementDistance](docs/C8yDistanceMeasurementDistance.md)
 - [Model.C8yFirmware](docs/C8yFirmware.md)
 - [Model.C8yHardware](docs/C8yHardware.md)
 - [Model.C8yHumidityMeasurement](docs/C8yHumidityMeasurement.md)
 - [Model.C8yHumidityMeasurementH](docs/C8yHumidityMeasurementH.md)
 - [Model.C8yLightMeasurement](docs/C8yLightMeasurement.md)
 - [Model.C8yLightMeasurementE](docs/C8yLightMeasurementE.md)
 - [Model.C8yLogfileRequest](docs/C8yLogfileRequest.md)
 - [Model.C8yMeasurementValue](docs/C8yMeasurementValue.md)
 - [Model.C8yMobile](docs/C8yMobile.md)
 - [Model.C8yMoistureMeasurement](docs/C8yMoistureMeasurement.md)
 - [Model.C8yMoistureMeasurementMoisture](docs/C8yMoistureMeasurementMoisture.md)
 - [Model.C8yMotionMeasurement](docs/C8yMotionMeasurement.md)
 - [Model.C8yMotionMeasurementMotionDetected](docs/C8yMotionMeasurementMotionDetected.md)
 - [Model.C8yMotionMeasurementSpeed](docs/C8yMotionMeasurementSpeed.md)
 - [Model.C8yNetwork](docs/C8yNetwork.md)
 - [Model.C8yNetworkC8yDHCP](docs/C8yNetworkC8yDHCP.md)
 - [Model.C8yNetworkC8yDHCPAddressRange](docs/C8yNetworkC8yDHCPAddressRange.md)
 - [Model.C8yNetworkC8yLAN](docs/C8yNetworkC8yLAN.md)
 - [Model.C8yNetworkC8yWAN](docs/C8yNetworkC8yWAN.md)
 - [Model.C8yPosition](docs/C8yPosition.md)
 - [Model.C8yProfile](docs/C8yProfile.md)
 - [Model.C8yRequiredAvailability](docs/C8yRequiredAvailability.md)
 - [Model.C8ySoftwareListInner](docs/C8ySoftwareListInner.md)
 - [Model.C8ySteam](docs/C8ySteam.md)
 - [Model.C8ySteamTemperature](docs/C8ySteamTemperature.md)
 - [Model.C8yTemperatureMeasurement](docs/C8yTemperatureMeasurement.md)
 - [Model.C8yTemperatureMeasurementT](docs/C8yTemperatureMeasurementT.md)
 - [Model.C8yVoltageMeasurement](docs/C8yVoltageMeasurement.md)
 - [Model.C8yVoltageMeasurementVoltage](docs/C8yVoltageMeasurementVoltage.md)
 - [Model.CategoryKeyOption](docs/CategoryKeyOption.md)
 - [Model.ChildOperationsAddMultiple](docs/ChildOperationsAddMultiple.md)
 - [Model.ChildOperationsAddMultipleReferencesInner](docs/ChildOperationsAddMultipleReferencesInner.md)
 - [Model.ChildOperationsAddMultipleReferencesInnerManagedObject](docs/ChildOperationsAddMultipleReferencesInnerManagedObject.md)
 - [Model.ChildOperationsAddOne](docs/ChildOperationsAddOne.md)
 - [Model.ChildOperationsAddOneManagedObject](docs/ChildOperationsAddOneManagedObject.md)
 - [Model.CurrentTenant](docs/CurrentTenant.md)
 - [Model.CurrentTenantApplications](docs/CurrentTenantApplications.md)
 - [Model.CurrentUser](docs/CurrentUser.md)
 - [Model.CurrentUserTotpCode](docs/CurrentUserTotpCode.md)
 - [Model.CurrentUserTotpSecret](docs/CurrentUserTotpSecret.md)
 - [Model.CurrentUserTotpSecretActivity](docs/CurrentUserTotpSecretActivity.md)
 - [Model.CustomProperties](docs/CustomProperties.md)
 - [Model.DailyUsageStatistics](docs/DailyUsageStatistics.md)
 - [Model.DeviceControlApiResource](docs/DeviceControlApiResource.md)
 - [Model.DeviceControlApiResourceOperations](docs/DeviceControlApiResourceOperations.md)
 - [Model.DeviceCredentials](docs/DeviceCredentials.md)
 - [Model.DevicePermissionOwners](docs/DevicePermissionOwners.md)
 - [Model.DeviceStatistics](docs/DeviceStatistics.md)
 - [Model.DeviceStatisticsCollection](docs/DeviceStatisticsCollection.md)
 - [Model.Error](docs/Error.md)
 - [Model.Event](docs/Event.md)
 - [Model.EventBinary](docs/EventBinary.md)
 - [Model.EventCollection](docs/EventCollection.md)
 - [Model.EventSource](docs/EventSource.md)
 - [Model.EventsApiResource](docs/EventsApiResource.md)
 - [Model.EventsApiResourceEvents](docs/EventsApiResourceEvents.md)
 - [Model.ExternalId](docs/ExternalId.md)
 - [Model.ExternalIdManagedObject](docs/ExternalIdManagedObject.md)
 - [Model.ExternalIds](docs/ExternalIds.md)
 - [Model.GetApplicationsByNameCollectionResource200Response](docs/GetApplicationsByNameCollectionResource200Response.md)
 - [Model.GetApplicationsByNameCollectionResource200ResponseAllOf](docs/GetApplicationsByNameCollectionResource200ResponseAllOf.md)
 - [Model.GetApplicationsByOwnerCollectionResource200Response](docs/GetApplicationsByOwnerCollectionResource200Response.md)
 - [Model.GetApplicationsByOwnerCollectionResource200ResponseAllOf](docs/GetApplicationsByOwnerCollectionResource200ResponseAllOf.md)
 - [Model.GetApplicationsByTenantCollectionResource200Response](docs/GetApplicationsByTenantCollectionResource200Response.md)
 - [Model.GetApplicationsByTenantCollectionResource200ResponseAllOf](docs/GetApplicationsByTenantCollectionResource200ResponseAllOf.md)
 - [Model.GetApplicationsByUserCollectionResource200Response](docs/GetApplicationsByUserCollectionResource200Response.md)
 - [Model.GetApplicationsByUserCollectionResource200ResponseAllOf](docs/GetApplicationsByUserCollectionResource200ResponseAllOf.md)
 - [Model.GetManagedObjectChildAdditionResource200Response](docs/GetManagedObjectChildAdditionResource200Response.md)
 - [Model.GetManagedObjectChildAdditionResource200ResponseAllOf](docs/GetManagedObjectChildAdditionResource200ResponseAllOf.md)
 - [Model.GetManagedObjectChildAssetResource200Response](docs/GetManagedObjectChildAssetResource200Response.md)
 - [Model.GetManagedObjectChildAssetResource200ResponseAllOf](docs/GetManagedObjectChildAssetResource200ResponseAllOf.md)
 - [Model.GetManagedObjectChildAssetsResource200Response](docs/GetManagedObjectChildAssetsResource200Response.md)
 - [Model.GetManagedObjectChildAssetsResource200ResponseAllOf](docs/GetManagedObjectChildAssetsResource200ResponseAllOf.md)
 - [Model.GetManagedObjectChildDeviceResource200Response](docs/GetManagedObjectChildDeviceResource200Response.md)
 - [Model.GetManagedObjectChildDeviceResource200ResponseAllOf](docs/GetManagedObjectChildDeviceResource200ResponseAllOf.md)
 - [Model.GetManagedObjectChildDevicesResource200Response](docs/GetManagedObjectChildDevicesResource200Response.md)
 - [Model.GetManagedObjectChildDevicesResource200ResponseAllOf](docs/GetManagedObjectChildDevicesResource200ResponseAllOf.md)
 - [Model.Group](docs/Group.md)
 - [Model.GroupReference](docs/GroupReference.md)
 - [Model.GroupReferenceCollection](docs/GroupReferenceCollection.md)
 - [Model.GroupRoles](docs/GroupRoles.md)
 - [Model.GroupUsers](docs/GroupUsers.md)
 - [Model.IdentityApiResource](docs/IdentityApiResource.md)
 - [Model.InventoryApiResource](docs/InventoryApiResource.md)
 - [Model.InventoryApiResourceManagedObjects](docs/InventoryApiResourceManagedObjects.md)
 - [Model.InventoryAssignment](docs/InventoryAssignment.md)
 - [Model.InventoryAssignmentCollection](docs/InventoryAssignmentCollection.md)
 - [Model.InventoryAssignmentReference](docs/InventoryAssignmentReference.md)
 - [Model.InventoryAssignmentReferenceRolesInner](docs/InventoryAssignmentReferenceRolesInner.md)
 - [Model.InventoryRole](docs/InventoryRole.md)
 - [Model.InventoryRoleCollection](docs/InventoryRoleCollection.md)
 - [Model.InventoryRolePermission](docs/InventoryRolePermission.md)
 - [Model.JSONPredicateRepresentation](docs/JSONPredicateRepresentation.md)
 - [Model.LoginOption](docs/LoginOption.md)
 - [Model.LoginOptionCollection](docs/LoginOptionCollection.md)
 - [Model.ManagedObject](docs/ManagedObject.md)
 - [Model.ManagedObjectCollection](docs/ManagedObjectCollection.md)
 - [Model.ManagedObjectReference](docs/ManagedObjectReference.md)
 - [Model.ManagedObjectReferenceCollection](docs/ManagedObjectReferenceCollection.md)
 - [Model.ManagedObjectReferenceCollectionReferencesInner](docs/ManagedObjectReferenceCollectionReferencesInner.md)
 - [Model.ManagedObjectReferenceTuple](docs/ManagedObjectReferenceTuple.md)
 - [Model.ManagedObjectReferenceTupleManagedObject](docs/ManagedObjectReferenceTupleManagedObject.md)
 - [Model.ManagedObjectUser](docs/ManagedObjectUser.md)
 - [Model.Measurement](docs/Measurement.md)
 - [Model.MeasurementApiResource](docs/MeasurementApiResource.md)
 - [Model.MeasurementApiResourceMeasurements](docs/MeasurementApiResourceMeasurements.md)
 - [Model.MeasurementCollection](docs/MeasurementCollection.md)
 - [Model.MeasurementCollectionStatistics](docs/MeasurementCollectionStatistics.md)
 - [Model.MeasurementFragmentSeries](docs/MeasurementFragmentSeries.md)
 - [Model.MeasurementSeries](docs/MeasurementSeries.md)
 - [Model.MeasurementSource](docs/MeasurementSource.md)
 - [Model.MicroserviceApplicationManifest](docs/MicroserviceApplicationManifest.md)
 - [Model.MicroserviceApplicationManifestExtensionsInner](docs/MicroserviceApplicationManifestExtensionsInner.md)
 - [Model.MicroserviceApplicationManifestLivenessProbe](docs/MicroserviceApplicationManifestLivenessProbe.md)
 - [Model.MicroserviceApplicationManifestProvider](docs/MicroserviceApplicationManifestProvider.md)
 - [Model.MicroserviceApplicationManifestReadinessProbe](docs/MicroserviceApplicationManifestReadinessProbe.md)
 - [Model.MicroserviceApplicationManifestRequestResources](docs/MicroserviceApplicationManifestRequestResources.md)
 - [Model.MicroserviceApplicationManifestResources](docs/MicroserviceApplicationManifestResources.md)
 - [Model.NewDeviceRequest](docs/NewDeviceRequest.md)
 - [Model.NewDeviceRequestCollection](docs/NewDeviceRequestCollection.md)
 - [Model.NotificationApiResource](docs/NotificationApiResource.md)
 - [Model.NotificationApiResourceNotificationSubscriptions](docs/NotificationApiResourceNotificationSubscriptions.md)
 - [Model.NotificationSubscription](docs/NotificationSubscription.md)
 - [Model.NotificationSubscriptionCollection](docs/NotificationSubscriptionCollection.md)
 - [Model.NotificationSubscriptionResult](docs/NotificationSubscriptionResult.md)
 - [Model.NotificationSubscriptionSource](docs/NotificationSubscriptionSource.md)
 - [Model.NotificationSubscriptionSubscriptionFilter](docs/NotificationSubscriptionSubscriptionFilter.md)
 - [Model.NotificationToken](docs/NotificationToken.md)
 - [Model.NotificationTokenClaims](docs/NotificationTokenClaims.md)
 - [Model.OAuthSessionConfiguration](docs/OAuthSessionConfiguration.md)
 - [Model.ObjectAdditionParents](docs/ObjectAdditionParents.md)
 - [Model.ObjectAssetParents](docs/ObjectAssetParents.md)
 - [Model.ObjectChildAdditions](docs/ObjectChildAdditions.md)
 - [Model.ObjectChildAssets](docs/ObjectChildAssets.md)
 - [Model.ObjectChildDevices](docs/ObjectChildDevices.md)
 - [Model.ObjectDeviceParents](docs/ObjectDeviceParents.md)
 - [Model.Operation](docs/Operation.md)
 - [Model.OperationCollection](docs/OperationCollection.md)
 - [Model.OperationReference](docs/OperationReference.md)
 - [Model.OperationReferenceOperation](docs/OperationReferenceOperation.md)
 - [Model.Option](docs/Option.md)
 - [Model.OptionCollection](docs/OptionCollection.md)
 - [Model.PageStatistics](docs/PageStatistics.md)
 - [Model.PasswordChange](docs/PasswordChange.md)
 - [Model.PlatformApiResource](docs/PlatformApiResource.md)
 - [Model.PostAlarmCollectionResourceRequest](docs/PostAlarmCollectionResourceRequest.md)
 - [Model.PostAlarmCollectionResourceRequestAllOf](docs/PostAlarmCollectionResourceRequestAllOf.md)
 - [Model.PostAlarmCollectionResourceRequestAllOfSource](docs/PostAlarmCollectionResourceRequestAllOfSource.md)
 - [Model.PostApplicationCollectionResourceRequest](docs/PostApplicationCollectionResourceRequest.md)
 - [Model.PostDeviceCredentialsCollectionResourceRequest](docs/PostDeviceCredentialsCollectionResourceRequest.md)
 - [Model.PostEventCollectionResourceRequest](docs/PostEventCollectionResourceRequest.md)
 - [Model.PostEventCollectionResourceRequestAllOf](docs/PostEventCollectionResourceRequestAllOf.md)
 - [Model.PostGroupCollectionResourceRequest](docs/PostGroupCollectionResourceRequest.md)
 - [Model.PostInventoryAssignmentResourceRequest](docs/PostInventoryAssignmentResourceRequest.md)
 - [Model.PostInventoryAssignmentResourceRequestAllOf](docs/PostInventoryAssignmentResourceRequestAllOf.md)
 - [Model.PostInventoryAssignmentResourceRequestAllOfRolesInner](docs/PostInventoryAssignmentResourceRequestAllOfRolesInner.md)
 - [Model.PostInventoryRoleResourceRequest](docs/PostInventoryRoleResourceRequest.md)
 - [Model.PostNewDeviceRequestCollectionResourceRequest](docs/PostNewDeviceRequestCollectionResourceRequest.md)
 - [Model.PostNewDeviceRequestCollectionResourceRequestAllOf](docs/PostNewDeviceRequestCollectionResourceRequestAllOf.md)
 - [Model.PostOperationCollectionResourceRequest](docs/PostOperationCollectionResourceRequest.md)
 - [Model.PostOperationCollectionResourceRequestAllOf](docs/PostOperationCollectionResourceRequestAllOf.md)
 - [Model.PostOptionCollectionResourceRequest](docs/PostOptionCollectionResourceRequest.md)
 - [Model.PostRetentionRuleCollectionResourceRequest](docs/PostRetentionRuleCollectionResourceRequest.md)
 - [Model.PostTenantCollectionResourceRequest](docs/PostTenantCollectionResourceRequest.md)
 - [Model.PostUserCollectionResourceRequest](docs/PostUserCollectionResourceRequest.md)
 - [Model.PostUserCollectionResourceRequestAllOf](docs/PostUserCollectionResourceRequestAllOf.md)
 - [Model.PutAlarmCollectionResourceRequest](docs/PutAlarmCollectionResourceRequest.md)
 - [Model.PutAlarmCollectionResourceRequestAllOf](docs/PutAlarmCollectionResourceRequestAllOf.md)
 - [Model.PutAlarmResourceRequest](docs/PutAlarmResourceRequest.md)
 - [Model.PutAlarmResourceRequestAllOf](docs/PutAlarmResourceRequestAllOf.md)
 - [Model.PutApplicationResourceRequest](docs/PutApplicationResourceRequest.md)
 - [Model.PutApplicationResourceRequestAllOf](docs/PutApplicationResourceRequestAllOf.md)
 - [Model.PutEventResourceRequest](docs/PutEventResourceRequest.md)
 - [Model.PutNewDeviceRequestResourceRequest](docs/PutNewDeviceRequestResourceRequest.md)
 - [Model.PutNewDeviceRequestResourceRequestAllOf](docs/PutNewDeviceRequestResourceRequestAllOf.md)
 - [Model.PutOperationResourceRequest](docs/PutOperationResourceRequest.md)
 - [Model.PutOperationResourceRequestAllOf](docs/PutOperationResourceRequestAllOf.md)
 - [Model.PutTenantResourceRequest](docs/PutTenantResourceRequest.md)
 - [Model.PutTenantResourceRequestAllOf](docs/PutTenantResourceRequestAllOf.md)
 - [Model.PutTrustedCertificateResourceRequest](docs/PutTrustedCertificateResourceRequest.md)
 - [Model.PutTrustedCertificateResourceRequestAllOf](docs/PutTrustedCertificateResourceRequestAllOf.md)
 - [Model.PutUserResourceRequest](docs/PutUserResourceRequest.md)
 - [Model.PutUserResourceRequestAllOf](docs/PutUserResourceRequestAllOf.md)
 - [Model.RangeStatisticsFile](docs/RangeStatisticsFile.md)
 - [Model.RealtimeNotification](docs/RealtimeNotification.md)
 - [Model.RealtimeNotificationAdvice](docs/RealtimeNotificationAdvice.md)
 - [Model.RealtimeNotificationExt](docs/RealtimeNotificationExt.md)
 - [Model.RealtimeNotificationExtComCumulocityAuthn](docs/RealtimeNotificationExtComCumulocityAuthn.md)
 - [Model.RequestRepresentation](docs/RequestRepresentation.md)
 - [Model.RetentionRule](docs/RetentionRule.md)
 - [Model.RetentionRuleCollection](docs/RetentionRuleCollection.md)
 - [Model.Role](docs/Role.md)
 - [Model.RoleReference](docs/RoleReference.md)
 - [Model.RoleReferenceCollection](docs/RoleReferenceCollection.md)
 - [Model.StatisticsFile](docs/StatisticsFile.md)
 - [Model.SubscribedApplicationReference](docs/SubscribedApplicationReference.md)
 - [Model.SubscribedApplicationReferenceApplication](docs/SubscribedApplicationReferenceApplication.md)
 - [Model.SubscribedRole](docs/SubscribedRole.md)
 - [Model.SubscribedRoleRole](docs/SubscribedRoleRole.md)
 - [Model.SubscribedUser](docs/SubscribedUser.md)
 - [Model.SubscribedUserUser](docs/SubscribedUserUser.md)
 - [Model.SummaryAllTenantsUsageStatistics](docs/SummaryAllTenantsUsageStatistics.md)
 - [Model.SummaryTenantUsageStatistics](docs/SummaryTenantUsageStatistics.md)
 - [Model.SupportedMeasurements](docs/SupportedMeasurements.md)
 - [Model.SupportedSeries](docs/SupportedSeries.md)
 - [Model.SystemOption](docs/SystemOption.md)
 - [Model.SystemOptionCollection](docs/SystemOptionCollection.md)
 - [Model.Tenant](docs/Tenant.md)
 - [Model.TenantApiResource](docs/TenantApiResource.md)
 - [Model.TenantApiResourceOptions](docs/TenantApiResourceOptions.md)
 - [Model.TenantApiResourceTenants](docs/TenantApiResourceTenants.md)
 - [Model.TenantApplications](docs/TenantApplications.md)
 - [Model.TenantCollection](docs/TenantCollection.md)
 - [Model.TenantOwnedApplications](docs/TenantOwnedApplications.md)
 - [Model.TenantTfaData](docs/TenantTfaData.md)
 - [Model.TenantUsageStatisticsCollection](docs/TenantUsageStatisticsCollection.md)
 - [Model.TenantUsageStatisticsFileCollection](docs/TenantUsageStatisticsFileCollection.md)
 - [Model.TrustedCertificate](docs/TrustedCertificate.md)
 - [Model.TrustedCertificateCollection](docs/TrustedCertificateCollection.md)
 - [Model.UpdatedDevicePermissions](docs/UpdatedDevicePermissions.md)
 - [Model.UpdatedDevicePermissionsGroupsInner](docs/UpdatedDevicePermissionsGroupsInner.md)
 - [Model.UpdatedDevicePermissionsUsersInner](docs/UpdatedDevicePermissionsUsersInner.md)
 - [Model.UploadedTrustedCertSignedVerificationCode](docs/UploadedTrustedCertSignedVerificationCode.md)
 - [Model.UsageStatisticsResources](docs/UsageStatisticsResources.md)
 - [Model.UsageStatisticsResourcesUsedBy](docs/UsageStatisticsResourcesUsedBy.md)
 - [Model.User](docs/User.md)
 - [Model.UserApiResource](docs/UserApiResource.md)
 - [Model.UserCollection](docs/UserCollection.md)
 - [Model.UserGroupCollection](docs/UserGroupCollection.md)
 - [Model.UserGroups](docs/UserGroups.md)
 - [Model.UserReference](docs/UserReference.md)
 - [Model.UserReferenceCollection](docs/UserReferenceCollection.md)
 - [Model.UserRoleCollection](docs/UserRoleCollection.md)
 - [Model.UserRoles](docs/UserRoles.md)
 - [Model.UserTfaData](docs/UserTfaData.md)
 - [Model.WebApplicationManifest](docs/WebApplicationManifest.md)


<a name="documentation-for-authorization"></a>
## Documentation for Authorization

<a name="Basic"></a>
### Basic

- **Type**: HTTP basic authentication

<a name="JWT"></a>
### JWT

- **Type**: Bearer Authentication

<a name="OAI-Secure"></a>
### OAI-Secure

- **Type**: Bearer Authentication

<a name="SSO"></a>
### SSO

- **Type**: OAuth
- **Flow**: application
- **Authorization URL**: 
- **Scopes**: N/A

