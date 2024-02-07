# kern.services.CumulocityClient.Api.TrustedCertificatesApi

All URIs are relative to *https://<TENANT_DOMAIN>*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**DeleteTrustedCertificateResource**](TrustedCertificatesApi.md#deletetrustedcertificateresource) | **DELETE** /tenant/tenants/{tenantId}/trusted-certificates/{fingerprint} | Remove a stored certificate |
| [**GetTrustedCertificateCollectionResource**](TrustedCertificatesApi.md#gettrustedcertificatecollectionresource) | **GET** /tenant/tenants/{tenantId}/trusted-certificates | Retrieve all stored certificates |
| [**GetTrustedCertificateResource**](TrustedCertificatesApi.md#gettrustedcertificateresource) | **GET** /tenant/tenants/{tenantId}/trusted-certificates/{fingerprint} | Retrieve a stored certificate |
| [**PostConfirmedTrustedCertificatePopResource**](TrustedCertificatesApi.md#postconfirmedtrustedcertificatepopresource) | **POST** /tenant/tenants/{tenantId}/trusted-certificates-pop/{fingerprint}/confirmed | Confirm an already uploaded certificate |
| [**PostTrustedCertificateCollectionResource**](TrustedCertificatesApi.md#posttrustedcertificatecollectionresource) | **POST** /tenant/tenants/{tenantId}/trusted-certificates | Add a new certificate |
| [**PostTrustedCertificateCollectionResourceBulk**](TrustedCertificatesApi.md#posttrustedcertificatecollectionresourcebulk) | **POST** /tenant/tenants/{tenantId}/trusted-certificates/bulk | Add multiple certificates |
| [**PostTrustedCertificatePopResource**](TrustedCertificatesApi.md#posttrustedcertificatepopresource) | **POST** /tenant/tenants/{tenantId}/trusted-certificates-pop/{fingerprint}/pop | Provide the proof of possession for an already uploaded certificate |
| [**PostVerificationCodeTrustedCertificatesPopResource**](TrustedCertificatesApi.md#postverificationcodetrustedcertificatespopresource) | **POST** /tenant/tenants/{tenantId}/trusted-certificates-pop/{fingerprint}/verification-code | Generate a verification code for the proof of possession operation for the given certificate |
| [**PutTrustedCertificateResource**](TrustedCertificatesApi.md#puttrustedcertificateresource) | **PUT** /tenant/tenants/{tenantId}/trusted-certificates/{fingerprint} | Update a stored certificate |

<a id="deletetrustedcertificateresource"></a>
# **DeleteTrustedCertificateResource**
> void DeleteTrustedCertificateResource (string tenantId, string fingerprint)

Remove a stored certificate

Remove a stored trusted certificate (by a given fingerprint) from a specific tenant (by a given ID).  When a trusted certificate is deleted, the established MQTT connection to all devices that are using the corresponding certificate are closed.  <section><h5>Required roles</h5> (ROLE_TENANT_MANAGEMENT_ADMIN <b>OR</b> ROLE_TENANT_ADMIN) <b>AND</b> (is the current tenant <b>OR</b> is the management tenant) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class DeleteTrustedCertificateResourceExample
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

            var apiInstance = new TrustedCertificatesApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var fingerprint = df9c19e0433c6861fak899078b76fe56a3e7dd14;  // string | Unique identifier of a trusted certificate.

            try
            {
                // Remove a stored certificate
                apiInstance.DeleteTrustedCertificateResource(tenantId, fingerprint);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TrustedCertificatesApi.DeleteTrustedCertificateResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteTrustedCertificateResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove a stored certificate
    apiInstance.DeleteTrustedCertificateResourceWithHttpInfo(tenantId, fingerprint);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TrustedCertificatesApi.DeleteTrustedCertificateResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **fingerprint** | **string** | Unique identifier of a trusted certificate. |  |

### Return type

void (empty response body)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **204** | The trusted certificate was removed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Certificate not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="gettrustedcertificatecollectionresource"></a>
# **GetTrustedCertificateCollectionResource**
> TrustedCertificateCollection GetTrustedCertificateCollectionResource (string tenantId, int? currentPage = null, int? pageSize = null, bool? withTotalElements = null, bool? withTotalPages = null)

Retrieve all stored certificates

Retrieve all the trusted certificates of a specific tenant (by a given ID).  <section><h5>Required roles</h5> (ROLE_TENANT_MANAGEMENT_ADMIN <b>OR</b> ROLE_TENANT_ADMIN) <b>AND</b> (is the current tenant) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetTrustedCertificateCollectionResourceExample
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

            var apiInstance = new TrustedCertificatesApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var currentPage = 3;  // int? | The current page of the paginated results. (optional)  (default to 1)
            var pageSize = 10;  // int? | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. (optional)  (default to 5)
            var withTotalElements = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)
            var withTotalPages = true;  // bool? | When set to `true`, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). (optional)  (default to false)

            try
            {
                // Retrieve all stored certificates
                TrustedCertificateCollection result = apiInstance.GetTrustedCertificateCollectionResource(tenantId, currentPage, pageSize, withTotalElements, withTotalPages);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TrustedCertificatesApi.GetTrustedCertificateCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetTrustedCertificateCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve all stored certificates
    ApiResponse<TrustedCertificateCollection> response = apiInstance.GetTrustedCertificateCollectionResourceWithHttpInfo(tenantId, currentPage, pageSize, withTotalElements, withTotalPages);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TrustedCertificatesApi.GetTrustedCertificateCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **currentPage** | **int?** | The current page of the paginated results. | [optional] [default to 1] |
| **pageSize** | **int?** | Indicates how many entries of the collection shall be returned. The upper limit for one page is 2,000 objects. | [optional] [default to 5] |
| **withTotalElements** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of elements. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |
| **withTotalPages** | **bool?** | When set to &#x60;true&#x60;, the returned result will contain in the statistics object the total number of pages. Only applicable on [range queries](https://en.wikipedia.org/wiki/Range_query_(database)). | [optional] [default to false] |

### Return type

[**TrustedCertificateCollection**](TrustedCertificateCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the trusted certificates are sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **403** | Not authorized to perform this operation. |  -  |
| **404** | Tenant not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="gettrustedcertificateresource"></a>
# **GetTrustedCertificateResource**
> TrustedCertificate GetTrustedCertificateResource (string tenantId, string fingerprint)

Retrieve a stored certificate

Retrieve the data of a stored trusted certificate (by a given fingerprint) of a specific tenant (by a given ID).  <section><h5>Required roles</h5> (ROLE_TENANT_MANAGEMENT_ADMIN <b>OR</b> ROLE_TENANT_ADMIN) <b>AND</b> (is the current tenant <b>OR</b> is the management tenant) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class GetTrustedCertificateResourceExample
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

            var apiInstance = new TrustedCertificatesApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var fingerprint = df9c19e0433c6861fak899078b76fe56a3e7dd14;  // string | Unique identifier of a trusted certificate.

            try
            {
                // Retrieve a stored certificate
                TrustedCertificate result = apiInstance.GetTrustedCertificateResource(tenantId, fingerprint);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TrustedCertificatesApi.GetTrustedCertificateResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetTrustedCertificateResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve a stored certificate
    ApiResponse<TrustedCertificate> response = apiInstance.GetTrustedCertificateResourceWithHttpInfo(tenantId, fingerprint);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TrustedCertificatesApi.GetTrustedCertificateResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **fingerprint** | **string** | Unique identifier of a trusted certificate. |  |

### Return type

[**TrustedCertificate**](TrustedCertificate.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The request has succeeded and the trusted certificate is sent in the response. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="postconfirmedtrustedcertificatepopresource"></a>
# **PostConfirmedTrustedCertificatePopResource**
> TrustedCertificate PostConfirmedTrustedCertificatePopResource (string tenantId, string fingerprint, string? accept = null)

Confirm an already uploaded certificate

Confirm an already uploaded certificate (by a given fingerprint) for a specific tenant (by a given ID).  <div class=\"reqRoles\"><div><h5></h5></div><div> (ROLE_TENANT_MANAGEMENT_ADMIN <b>OR</b> ROLE_TENANT_ADMIN) <b>AND</b> is the management tenant </div></div> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostConfirmedTrustedCertificatePopResourceExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://<TENANT_DOMAIN>";
            // Configure HTTP basic authorization: Basic
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: SSO
            config.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new TrustedCertificatesApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var fingerprint = df9c19e0433c6861fak899078b76fe56a3e7dd14;  // string | Unique identifier of a trusted certificate.
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Confirm an already uploaded certificate
                TrustedCertificate result = apiInstance.PostConfirmedTrustedCertificatePopResource(tenantId, fingerprint, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TrustedCertificatesApi.PostConfirmedTrustedCertificatePopResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostConfirmedTrustedCertificatePopResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Confirm an already uploaded certificate
    ApiResponse<TrustedCertificate> response = apiInstance.PostConfirmedTrustedCertificatePopResourceWithHttpInfo(tenantId, fingerprint, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TrustedCertificatesApi.PostConfirmedTrustedCertificatePopResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **fingerprint** | **string** | Unique identifier of a trusted certificate. |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**TrustedCertificate**](TrustedCertificate.md)

### Authorization

[Basic](../README.md#Basic), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The certificate is confirmed. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Trusted certificate not found. |  -  |
| **422** | The verification was not successful. Certificate not confirmed. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="posttrustedcertificatecollectionresource"></a>
# **PostTrustedCertificateCollectionResource**
> TrustedCertificate PostTrustedCertificateCollectionResource (string tenantId, TrustedCertificate trustedCertificate, string? accept = null)

Add a new certificate

Add a new trusted certificate to a specific tenant (by a given ID) which can be further used by the devices to establish connections with the Cumulocity IoT platform.  <section><h5>Required roles</h5> (ROLE_TENANT_MANAGEMENT_ADMIN <b>OR</b> ROLE_TENANT_ADMIN) <b>AND</b> (is the current tenant) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostTrustedCertificateCollectionResourceExample
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

            var apiInstance = new TrustedCertificatesApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var trustedCertificate = new TrustedCertificate(); // TrustedCertificate | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Add a new certificate
                TrustedCertificate result = apiInstance.PostTrustedCertificateCollectionResource(tenantId, trustedCertificate, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TrustedCertificatesApi.PostTrustedCertificateCollectionResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostTrustedCertificateCollectionResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Add a new certificate
    ApiResponse<TrustedCertificate> response = apiInstance.PostTrustedCertificateCollectionResourceWithHttpInfo(tenantId, trustedCertificate, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TrustedCertificatesApi.PostTrustedCertificateCollectionResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **trustedCertificate** | [**TrustedCertificate**](TrustedCertificate.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**TrustedCertificate**](TrustedCertificate.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | The certificate was added to the tenant. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Tenant not found. |  -  |
| **409** | Duplicate – A certificate with the same fingerprint already exists. |  -  |
| **422** | Unprocessable Entity – Invalid certificate data. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="posttrustedcertificatecollectionresourcebulk"></a>
# **PostTrustedCertificateCollectionResourceBulk**
> TrustedCertificateCollection PostTrustedCertificateCollectionResourceBulk (string tenantId, TrustedCertificateCollection trustedCertificateCollection, string? accept = null)

Add multiple certificates

Add multiple trusted certificates to a specific tenant (by a given ID) which can be further used by the devices to establish connections with the Cumulocity IoT platform.  <section><h5>Required roles</h5> (ROLE_TENANT_MANAGEMENT_ADMIN <b>OR</b> ROLE_TENANT_ADMIN) <b>AND</b> (is the current tenant) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostTrustedCertificateCollectionResourceBulkExample
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

            var apiInstance = new TrustedCertificatesApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var trustedCertificateCollection = new TrustedCertificateCollection(); // TrustedCertificateCollection | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Add multiple certificates
                TrustedCertificateCollection result = apiInstance.PostTrustedCertificateCollectionResourceBulk(tenantId, trustedCertificateCollection, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TrustedCertificatesApi.PostTrustedCertificateCollectionResourceBulk: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostTrustedCertificateCollectionResourceBulkWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Add multiple certificates
    ApiResponse<TrustedCertificateCollection> response = apiInstance.PostTrustedCertificateCollectionResourceBulkWithHttpInfo(tenantId, trustedCertificateCollection, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TrustedCertificatesApi.PostTrustedCertificateCollectionResourceBulkWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **trustedCertificateCollection** | [**TrustedCertificateCollection**](TrustedCertificateCollection.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**TrustedCertificateCollection**](TrustedCertificateCollection.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | The certificates were added to the tenant. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Tenant not found. |  -  |
| **409** | Duplicate – A certificate with the same fingerprint already exists. |  -  |
| **422** | Unprocessable Entity – Invalid certificates data. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="posttrustedcertificatepopresource"></a>
# **PostTrustedCertificatePopResource**
> TrustedCertificate PostTrustedCertificatePopResource (string tenantId, string fingerprint, UploadedTrustedCertSignedVerificationCode uploadedTrustedCertSignedVerificationCode, string? accept = null)

Provide the proof of possession for an already uploaded certificate

Provide the proof of possession for a specific uploaded certificate (by a given fingerprint) for a specific tenant (by a given ID).  <div class=\"reqRoles\"><div><h5></h5></div><div> (ROLE_TENANT_MANAGEMENT_ADMIN <b>OR</b> ROLE_TENANT_ADMIN) <b>AND</b> is the current tenant </div></div> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostTrustedCertificatePopResourceExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://<TENANT_DOMAIN>";
            // Configure HTTP basic authorization: Basic
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: SSO
            config.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new TrustedCertificatesApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var fingerprint = df9c19e0433c6861fak899078b76fe56a3e7dd14;  // string | Unique identifier of a trusted certificate.
            var uploadedTrustedCertSignedVerificationCode = new UploadedTrustedCertSignedVerificationCode(); // UploadedTrustedCertSignedVerificationCode | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Provide the proof of possession for an already uploaded certificate
                TrustedCertificate result = apiInstance.PostTrustedCertificatePopResource(tenantId, fingerprint, uploadedTrustedCertSignedVerificationCode, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TrustedCertificatesApi.PostTrustedCertificatePopResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostTrustedCertificatePopResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Provide the proof of possession for an already uploaded certificate
    ApiResponse<TrustedCertificate> response = apiInstance.PostTrustedCertificatePopResourceWithHttpInfo(tenantId, fingerprint, uploadedTrustedCertSignedVerificationCode, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TrustedCertificatesApi.PostTrustedCertificatePopResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **fingerprint** | **string** | Unique identifier of a trusted certificate. |  |
| **uploadedTrustedCertSignedVerificationCode** | [**UploadedTrustedCertSignedVerificationCode**](UploadedTrustedCertSignedVerificationCode.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**TrustedCertificate**](TrustedCertificate.md)

### Authorization

[Basic](../README.md#Basic), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The provided signed verification code check was successful. |  -  |
| **400** | The provided signed verification code is not correct. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Trusted certificate not found. |  -  |
| **422** | Proof of possession for the certificate was not confirmed. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="postverificationcodetrustedcertificatespopresource"></a>
# **PostVerificationCodeTrustedCertificatesPopResource**
> TrustedCertificate PostVerificationCodeTrustedCertificatesPopResource (string tenantId, string fingerprint, string? accept = null)

Generate a verification code for the proof of possession operation for the given certificate

Generate a verification code for the proof of possession operation for the certificate (by a given fingerprint).  <div class=\"reqRoles\"><div><h5></h5></div><div> (ROLE_TENANT_MANAGEMENT_ADMIN <b>OR</b> ROLE_TENANT_ADMIN) <b>AND</b> is the current tenant </div></div> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PostVerificationCodeTrustedCertificatesPopResourceExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://<TENANT_DOMAIN>";
            // Configure HTTP basic authorization: Basic
            config.Username = "YOUR_USERNAME";
            config.Password = "YOUR_PASSWORD";
            // Configure OAuth2 access token for authorization: SSO
            config.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new TrustedCertificatesApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var fingerprint = df9c19e0433c6861fak899078b76fe56a3e7dd14;  // string | Unique identifier of a trusted certificate.
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Generate a verification code for the proof of possession operation for the given certificate
                TrustedCertificate result = apiInstance.PostVerificationCodeTrustedCertificatesPopResource(tenantId, fingerprint, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TrustedCertificatesApi.PostVerificationCodeTrustedCertificatesPopResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PostVerificationCodeTrustedCertificatesPopResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Generate a verification code for the proof of possession operation for the given certificate
    ApiResponse<TrustedCertificate> response = apiInstance.PostVerificationCodeTrustedCertificatesPopResourceWithHttpInfo(tenantId, fingerprint, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TrustedCertificatesApi.PostVerificationCodeTrustedCertificatesPopResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **fingerprint** | **string** | Unique identifier of a trusted certificate. |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**TrustedCertificate**](TrustedCertificate.md)

### Authorization

[Basic](../README.md#Basic), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The verification code was generated. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Trusted certificate not found. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="puttrustedcertificateresource"></a>
# **PutTrustedCertificateResource**
> TrustedCertificate PutTrustedCertificateResource (string tenantId, string fingerprint, PutTrustedCertificateResourceRequest putTrustedCertificateResourceRequest, string? accept = null)

Update a stored certificate

Update the data of a stored trusted certificate (by a given fingerprint) of a specific tenant (by a given ID).  <section><h5>Required roles</h5> (ROLE_TENANT_MANAGEMENT_ADMIN <b>OR</b> ROLE_TENANT_ADMIN) <b>AND</b> (is the current tenant <b>OR</b> is the management tenant) </section> 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using kern.services.CumulocityClient.Api;
using kern.services.CumulocityClient.Client;
using kern.services.CumulocityClient.Model;

namespace Example
{
    public class PutTrustedCertificateResourceExample
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

            var apiInstance = new TrustedCertificatesApi(config);
            var tenantId = t07007007;  // string | Unique identifier of a Cumulocity IoT tenant.
            var fingerprint = df9c19e0433c6861fak899078b76fe56a3e7dd14;  // string | Unique identifier of a trusted certificate.
            var putTrustedCertificateResourceRequest = new PutTrustedCertificateResourceRequest(); // PutTrustedCertificateResourceRequest | 
            var accept = application/json;  // string? | Advertises which content types, expressed as MIME types, the client is able to understand. (optional) 

            try
            {
                // Update a stored certificate
                TrustedCertificate result = apiInstance.PutTrustedCertificateResource(tenantId, fingerprint, putTrustedCertificateResourceRequest, accept);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TrustedCertificatesApi.PutTrustedCertificateResource: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PutTrustedCertificateResourceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update a stored certificate
    ApiResponse<TrustedCertificate> response = apiInstance.PutTrustedCertificateResourceWithHttpInfo(tenantId, fingerprint, putTrustedCertificateResourceRequest, accept);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TrustedCertificatesApi.PutTrustedCertificateResourceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. |  |
| **fingerprint** | **string** | Unique identifier of a trusted certificate. |  |
| **putTrustedCertificateResourceRequest** | [**PutTrustedCertificateResourceRequest**](PutTrustedCertificateResourceRequest.md) |  |  |
| **accept** | **string?** | Advertises which content types, expressed as MIME types, the client is able to understand. | [optional]  |

### Return type

[**TrustedCertificate**](TrustedCertificate.md)

### Authorization

[Basic](../README.md#Basic), [OAI-Secure](../README.md#OAI-Secure), [SSO](../README.md#SSO)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, application/vnd.com.nsn.cumulocity.error+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The certificate was updated on the tenant. |  -  |
| **401** | Authentication information is missing or invalid. |  -  |
| **404** | Certificate not found. |  -  |
| **422** | Unprocessable Entity – invalid payload. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

