# kern.services.CumulocityClient.Model.CurrentTenant

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**AllowCreateTenants** | **bool** | Indicates if this tenant can create subtenants. | [optional] [default to false]
**Applications** | [**CurrentTenantApplications**](CurrentTenantApplications.md) |  | [optional] 
**CustomProperties** | [**CustomProperties**](CustomProperties.md) |  | [optional] 
**DomainName** | **string** | URL of the tenant&#39;s domain. The domain name permits only the use of alphanumeric characters separated by dots &#x60;.&#x60;, hyphens &#x60;-&#x60; and underscores &#x60;_&#x60;. | [optional] 
**Name** | **string** | Unique identifier of a Cumulocity IoT tenant. | [optional] [readonly] 
**Parent** | **string** | ID of the parent tenant. | [optional] 
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

