
# kern.services.CumulocityClient.Model.PutTenantResourceRequest

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**AdminEmail** | **string** | Email address of the tenant&#39;s administrator. | [optional] 
**AdminName** | **Object** |  | [optional] [readonly] 
**AdminPass** | **string** | Password of the tenant&#39;s administrator. | [optional] 
**AllowCreateTenants** | **bool** | Indicates if this tenant can create subtenants. | [optional] [readonly] [default to false]
**Applications** | [**TenantApplications**](TenantApplications.md) |  | [optional] 
**Company** | **string** | Tenant&#39;s company name. | [optional] 
**ContactName** | **string** | Name of the contact person. | [optional] 
**ContactPhone** | **string** | Phone number of the contact person, provided in the international format, for example, +48 123 456 7890. | [optional] 
**CreationTime** | **DateTime** | The date and time when the tenant was created. | [optional] [readonly] 
**CustomProperties** | [**CustomProperties**](CustomProperties.md) |  | [optional] 
**Domain** | **string** | URL of the tenant&#39;s domain. The domain name permits only the use of alphanumeric characters separated by dots &#x60;.&#x60; and hyphens &#x60;-&#x60;. | [optional] 
**Id** | **string** | Unique identifier of a Cumulocity IoT tenant. | [optional] [readonly] 
**OwnedApplications** | [**TenantOwnedApplications**](TenantOwnedApplications.md) |  | [optional] 
**Parent** | **string** | ID of the parent tenant. | [optional] [readonly] 
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 
**Status** | **string** | Current status of the tenant. | [optional] [readonly] [default to StatusEnum.ACTIVE]

[[Back to Model list]](../README.md#documentation-for-models)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to README]](../README.md)

