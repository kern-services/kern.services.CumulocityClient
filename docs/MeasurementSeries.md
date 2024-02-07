
# kern.services.CumulocityClient.Model.MeasurementSeries

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Values** | **Object** | Each property contained here is a date taken from the measurement and it contains an array of objects specifying &#x60;min&#x60; and &#x60;max&#x60; pair of values. Each pair corresponds to a single series object in the &#x60;series&#x60; array. If there is no aggregation used, &#x60;min&#x60; is equal to &#x60;max&#x60; in every pair. | [optional] 
**Series** | [**List&lt;MeasurementFragmentSeries&gt;**](MeasurementFragmentSeries.md) | An array containing the type of series and units. | [optional] 
**Truncated** | **bool** | If there were more than 5000 values, the final result was truncated. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to README]](../README.md)

