# kern.services.CumulocityClient.Model.C8yPosition
Reports the geographical location of an asset in terms of latitude, longitude and altitude.  Altitude is given in meters. To report the current location of an asset or a device, `c8y_Position` is added to the managed object representing the asset or device. To trace the position of an asset or a device, `c8y_Position` is sent as part of an event of type `c8y_LocationUpdate`. 

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Alt** | **decimal** | In meters. | [optional] 
**Lng** | **decimal** |  | [optional] 
**Lat** | **decimal** |  | [optional] 
**TrackingProtocol** | **string** | Describes in which protocol the tracking context of a positioning report was sent. | [optional] 
**ReportReason** | **string** | Describes why the tracking context of a positioning report was sent. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

