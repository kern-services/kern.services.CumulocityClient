# kern.services.CumulocityClient.Model.C8yFirmware
Contains information on a device's firmware. In the inventory, `c8y_Firmware` represents the currently installed firmware on the device. As part of an operation, `c8y_Firmware` requests the device to install the indicated firmware. To enable firmware installation through the user interface, add `c8y_Firmware` to the list of supported operations.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Name** | **string** | Name of the firmware. | [optional] 
**_Version** | **string** | A version identifier of the firmware. | [optional] 
**Url** | **string** | A URI linking to the location to download the firmware from. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

