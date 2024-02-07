# kern.services.CumulocityClient.Model.C8yCommand
To carry out interactive sessions with a device, use the `c8y_Command` fragment. If this fragment is in the list of supported operations for a device, a tab `Shell` will be shown. Using the `Shell` tab, the user can send commands in an arbitrary, device-specific syntax to the device. The command is sent to the device in a property `text`.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Type** | **string** | The command sent to the device. | [optional] 
**Result** | **string** | To communicate the results of a particular command, the device adds a property &#x60;result&#x60;. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

