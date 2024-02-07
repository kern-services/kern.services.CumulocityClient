# kern.services.CumulocityClient.Model.SummaryTenantUsageStatistics
Summary of the usage statistics.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**AlarmsCreatedCount** | **int** | Number of created alarms. | [optional] 
**AlarmsUpdatedCount** | **int** | Number of updates made to the alarms. | [optional] 
**Day** | **DateTime** | Date of this usage statistics summary. | [optional] 
**DeviceCount** | **int** | Number of devices in the tenant identified by the fragment &#x60;c8y_IsDevice&#x60;. Updated only three times a day starting at 8:57, 16:57 and 23:57. | [optional] 
**DeviceEndpointCount** | **int** | Number of devices which do not have child devices. Updated only three times a day starting at 8:57, 16:57 and 23:57. | [optional] 
**DeviceRequestCount** | **int** | Number of requests that were issued only by devices against the tenant. Updated every 5 minutes. The following requests are not included:  * Requests made to &lt;kbd&gt;/user&lt;/kbd&gt;, &lt;kbd&gt;/tenant&lt;/kbd&gt; and &lt;kbd&gt;/application&lt;/kbd&gt; APIs * Application related requests (with &#x60;X-Cumulocity-Application-Key&#x60; header)  | [optional] 
**DeviceWithChildrenCount** | **int** | Number of devices with children. Updated only three times a day starting at 8:57, 16:57 and 23:57. | [optional] 
**EventsCreatedCount** | **int** | Number of created events. | [optional] 
**EventsUpdatedCount** | **int** | Number of updates made to the events. | [optional] 
**InventoriesCreatedCount** | **int** | Number of created managed objects. | [optional] 
**InventoriesUpdatedCount** | **int** | Number of updates made to the managed objects. | [optional] 
**MeasurementsCreatedCount** | **int** | Number of created measurements.  &gt; **&amp;#9432; Info:** Bulk creation of measurements is handled in a way that each measurement is counted individually.  | [optional] 
**RequestCount** | **int** | Number of requests that were made against the tenant. Updated every 5 minutes. The following requests are not included:  *  Internal SmartREST requests used to resolve templates *  Internal SLA monitoring requests *  Calls to any &lt;kbd&gt;/health&lt;/kbd&gt; endpoint *  Device bootstrap process requests related to configuring and retrieving device credentials *  Microservice SDK internal calls for applications and subscriptions  | [optional] 
**Resources** | [**UsageStatisticsResources**](UsageStatisticsResources.md) |  | [optional] 
**StorageSize** | **int** | Database storage in use, specified in bytes. It is affected by your retention rules and by the regularly running database optimization functions in Cumulocity IoT. If the size decreases, it does not necessarily mean that data was deleted. Updated only three times a day starting at 8:57, 16:57 and 23:57. | [optional] 
**SubscribedApplications** | **List&lt;string&gt;** | Names of the tenant subscribed applications. Updated only three times a day starting at 8:57, 16:57 and 23:57. | [optional] 
**TotalResourceCreateAndUpdateCount** | **int** | Sum of all inbound transfers. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

