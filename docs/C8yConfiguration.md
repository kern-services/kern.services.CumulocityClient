# kern.services.CumulocityClient.Model.C8yConfiguration
Text configuration fragment that allows you to configure parameters and initial settings of your device.  In the inventory, `c8y_Configuration` represents the currently active configuration on the device. As part of an operation, `c8y_Configuration` requests the device to switch the transmitted configuration to the currently active one. To enable configuration through the user interface, add `c8y_Configuration` to the list of supported operations. 

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Config** | **string** | A text in a device-specific format, representing the configuration of the device. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

