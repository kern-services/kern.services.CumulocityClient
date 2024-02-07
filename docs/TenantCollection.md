
# kern.services.CumulocityClient.Model.TenantCollection

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Next** | **string** | A URI reference [[RFC3986](https://tools.ietf.org/html/rfc3986)] to a potential next page of managed objects. | [optional] [readonly] 
**Prev** | **string** | A URI reference [[RFC3986](https://tools.ietf.org/html/rfc3986)] to a potential previous page of managed objects. | [optional] [readonly] 
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 
**Statistics** | [**PageStatistics**](PageStatistics.md) |  | [optional] 
**Tenants** | [**List&lt;Tenant&gt;**](Tenant.md) | An array containing the results (subtenants) of the request. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to README]](../README.md)

