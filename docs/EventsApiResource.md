
# kern.services.CumulocityClient.Model.EventsApiResource

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Events** | [**EventsApiResourceEvents**](EventsApiResourceEvents.md) |  | [optional] 
**EventsForSource** | **string** | Read-only collection of all events for a specific source object. The placeholder {source} must be a unique ID of an object in the inventory. | [optional] 
**EventsForSourceAndType** | **string** | Read-only collection of all events of a particular type and a specific source object. | [optional] 
**EventsForType** | **string** | Read-only collection of all events of a particular type. | [optional] 
**EventsForFragmentType** | **string** | Read-only collection of all events containing a particular fragment type. | [optional] 
**EventsForTime** | **string** | Read-only collection of all events for a particular time range. | [optional] 
**EventsForSourceAndTime** | **string** | Read-only collection of all events for a specific source object in a particular time range. | [optional] 
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 

[[Back to Model list]](../README.md#documentation-for-models)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to README]](../README.md)

