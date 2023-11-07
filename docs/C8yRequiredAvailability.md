# kern.services.CumulocityClient.Model.C8yRequiredAvailability
Devices can be monitored for availability by adding a `c8y_RequiredAvailability` fragment to the device.  Devices that have not sent any message in the response interval are considered disconnected. The response interval can have a value between `-32768` and `32767` and any values out of range will be shrunk to the range borders. Such devices are marked as unavailable and an unavailability alarm is raised. 

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**ResponseInterval** | **int** |  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

