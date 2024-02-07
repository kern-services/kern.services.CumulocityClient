
# kern.services.CumulocityClient.Model.MeasurementApiResource

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Measurements** | [**MeasurementApiResourceMeasurements**](MeasurementApiResourceMeasurements.md) |  | [optional] 
**MeasurementsForSource** | **string** | Read-only collection of all measurements for a specific source object. The placeholder {source} must be a unique ID of an object in the inventory. | [optional] 
**MeasurementsForSourceAndType** | **string** | Read-only collection of all measurements of a particular type and a specific source object. | [optional] 
**MeasurementsForType** | **string** | Read-only collection of all measurements of a particular type. | [optional] 
**MeasurementsForValueFragmentType** | **string** | Read-only collection of all measurements containing a particular fragment type. | [optional] 
**MeasurementsForDate** | **string** | Read-only collection of all measurements for a particular time range. | [optional] 
**MeasurementsForSourceAndDate** | **string** | Read-only collection of all measurements for a specific source object in a particular time range. | [optional] 
**MeasurementsForDateAndFragmentType** | **string** | Read-only collection of all measurements for a specific fragment type and a particular time range. | [optional] 
**MeasurementsForSourceAndValueFragmentTypeAndValueFragmentSeries** | **string** | Read-only collection of all measurements for a specific source object, particular fragment type and series, and an event type. | [optional] 
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 

[[Back to Model list]](../README.md#documentation-for-models)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to README]](../README.md)

