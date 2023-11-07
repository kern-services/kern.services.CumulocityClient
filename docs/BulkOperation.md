# kern.services.CumulocityClient.Model.BulkOperation

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 
**Id** | **string** | Unique identifier of this bulk operation. | [optional] [readonly] 
**GroupId** | **string** | Identifies the target group on which this operation should be performed. &gt;**&amp;#9432; Info:** &#x60;groupId&#x60; and &#x60;failedParentId&#x60; are mutually exclusive. Use only one of them in your request.  | [optional] 
**FailedParentId** | **string** | Identifies the failed bulk operation from which the failed operations should be rescheduled. &gt;**&amp;#9432; Info:** &#x60;groupId&#x60; and &#x60;failedParentId&#x60; are mutually exclusive. Use only one of them in your request.  | [optional] 
**StartDate** | **DateTime** | Date and time when the operations of this bulk operation should be created. | [optional] 
**CreationRamp** | **float** | Delay between every operation creation in seconds. | [optional] 
**OperationPrototype** | **Object** | Operation to be executed for every device in a group. | [optional] 
**Status** | **string** | The status of this bulk operation, in context of the execution of all its single operations. | [optional] [readonly] 
**GeneralStatus** | **string** | The general status of this bulk operation. The general status is visible for end users and they can filter and evaluate bulk operations by this status. | [optional] [readonly] 
**Progress** | [**BulkOperationProgress**](BulkOperationProgress.md) |  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

