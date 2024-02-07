
# kern.services.CumulocityClient.Model.PostOperationCollectionResourceRequest

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**BulkOperationId** | **string** | Reference to a bulk operation ID if this operation was scheduled from a bulk operation. | [optional] [readonly] 
**CreationTime** | **DateTime** | Date and time when the operation was created in the database. | [optional] [readonly] 
**DeviceId** | **string** | Identifier of the target device where the operation should be performed. | 
**DeviceExternalIDs** | [**ExternalIds**](ExternalIds.md) |  | [optional] 
**FailureReason** | **string** | Reason for the failure. | [optional] [readonly] 
**Id** | **string** | Unique identifier of this operation. | [optional] [readonly] 
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 
**Status** | **Object** |  | [optional] [readonly] 

[[Back to Model list]](../README.md#documentation-for-models)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to README]](../README.md)

