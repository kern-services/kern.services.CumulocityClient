# kern.services.CumulocityClient.Model.PutAlarmCollectionResourceRequest

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Count** | **int** | Number of times that this alarm has been triggered. | [optional] [readonly] 
**CreationTime** | **DateTime** | The date and time when the alarm was created. | [optional] [readonly] 
**FirstOccurrenceTime** | **DateTime** | The time at which the alarm occurred for the first time. Only present when &#x60;count&#x60; is greater than 1. | [optional] [readonly] 
**Id** | **string** | Unique identifier of the alarm. | [optional] [readonly] 
**LastUpdated** | **DateTime** | The date and time when the alarm was last updated. | [optional] [readonly] 
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 
**Severity** | **Object** |  | [optional] [readonly] 
**Source** | **Object** |  | [optional] [readonly] 
**Status** | **string** | The status of the alarm. If not specified, a new alarm will be created as ACTIVE. | [optional] 
**Text** | **Object** |  | [optional] [readonly] 
**Time** | **Object** |  | [optional] [readonly] 
**Type** | **Object** |  | [optional] [readonly] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

