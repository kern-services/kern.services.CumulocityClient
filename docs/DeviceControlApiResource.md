# kern.services.CumulocityClient.Model.DeviceControlApiResource

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Operations** | [**DeviceControlApiResourceOperations**](DeviceControlApiResourceOperations.md) |  | [optional] 
**OperationsByStatus** | **string** | Read-only collection of all operations with a particular status. | [optional] 
**OperationsByAgentId** | **string** | Read-only collection of all operations targeting a particular agent. | [optional] 
**OperationsByAgentIdAndStatus** | **string** | Read-only collection of all operations targeting a particular agent and with a particular status. | [optional] 
**OperationsByDeviceId** | **string** | Read-only collection of all operations to be executed on a particular device. | [optional] 
**OperationsByDeviceIdAndStatus** | **string** | Read-only collection of all operations with a particular status, that should be executed on a particular device. | [optional] 
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

