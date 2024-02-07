
# kern.services.CumulocityClient.Model.AuditRecord

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Activity** | **string** | Summary of the action that was carried out. | 
**Application** | **string** | Name of the application that performed the action. | [optional] [readonly] 
**C8yMetadata** | [**AuditRecordC8yMetadata**](AuditRecordC8yMetadata.md) |  | [optional] 
**Changes** | [**List&lt;AuditRecordChangesInner&gt;**](AuditRecordChangesInner.md) | Collection of objects describing the changes that were carried out. | [optional] [readonly] 
**CreationTime** | **DateTime** | The date and time when the audit record was created. | [optional] [readonly] 
**Id** | **string** | Unique identifier of the audit record. | [optional] [readonly] 
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 
**Severity** | **string** | The severity of the audit action. | [optional] [readonly] 
**Source** | [**AuditRecordSource**](AuditRecordSource.md) |  | 
**Text** | **string** | Details of the action that was carried out. | 
**Time** | **DateTime** | The date and time when the audit is updated. | 
**Type** | **string** | Identifies the platform component of the audit. | 
**User** | **string** | The user who carried out the activity. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to README]](../README.md)

