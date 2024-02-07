# kern.services.CumulocityClient.Model.MeasurementCollectionStatistics

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**CurrentPage** | **int** | The current page of the paginated results. | [optional] 
**PageSize** | **int** | Indicates the number of objects that the collection may contain per page. The upper limit for one page is 2,000 objects. | [optional] 
**TotalElements** | **int** | The total number of results (elements). | [optional] 
**TotalPages** | **int** | The total number of paginated results (pages).  &gt; **&amp;#9432; Info:** This property is returned by default except when an operation retrieves all records where values are between an upper and lower boundary, for example, querying ranges using &#x60;dateFrom&#x60;–&#x60;dateTo&#x60;. In such cases, the query parameter &#x60;withTotalPages&#x3D;true&#x60; should be used to include the total number of pages (at the expense of slightly slower performance).  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

