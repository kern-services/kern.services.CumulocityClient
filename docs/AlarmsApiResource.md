
# kern.services.CumulocityClient.Model.AlarmsApiResource

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Alarms** | [**AlarmsApiResourceAlarms**](AlarmsApiResourceAlarms.md) |  | [optional] 
**AlarmsForSource** | **string** | Read-only collection of all alarms for a specific source object. The placeholder {source} must be a unique ID of an object in the inventory. | [optional] 
**AlarmsForStatus** | **string** | Read-only collection of all alarms in a particular status. The placeholder {status} can be one of the following values: ACTIVE, ACKNOWLEDGED or CLEARED | [optional] 
**AlarmsForSourceAndStatusAndTime** | **string** | Read-only collection of all alarms for a specific source, status and time range. | [optional] 
**AlarmsForStatusAndTime** | **string** | Read-only collection of all alarms for a particular status and time range. | [optional] 
**AlarmsForSourceAndTime** | **string** | Read-only collection of all alarms for a specific source and time range. | [optional] 
**AlarmsForTime** | **string** | Read-only collection of all alarms for a particular time range. | [optional] 
**AlarmsForSourceAndStatus** | **string** | Read-only collection of all alarms for a specific source object in a particular status. | [optional] 
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 

[[Back to Model list]](../README.md#documentation-for-models)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to README]](../README.md)

