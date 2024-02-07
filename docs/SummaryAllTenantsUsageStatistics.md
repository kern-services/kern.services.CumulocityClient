# kern.services.CumulocityClient.Model.SummaryAllTenantsUsageStatistics

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**AlarmsCreatedCount** | **int** | Number of created alarms. | [optional] 
**AlarmsUpdatedCount** | **int** | Number of updates made to the alarms. | [optional] 
**CreationTime** | **DateTime** | Date and time of the tenant&#39;s creation. | [optional] 
**DeviceCount** | **int** | Number of devices in the tenant identified by the fragment &#x60;c8y_IsDevice&#x60;. Updated only three times a day starting at 8:57, 16:57 and 23:57. | [optional] 
**DeviceEndpointCount** | **int** | Number of devices which do not have child devices. Updated only three times a day starting at 8:57, 16:57 and 23:57. | [optional] 
**DeviceRequestCount** | **int** | Number of requests that were issued only by devices against the tenant. Updated every 5 minutes. The following requests are not included:  * Requests made to &lt;kbd&gt;/user&lt;/kbd&gt;, &lt;kbd&gt;/tenant&lt;/kbd&gt; and &lt;kbd&gt;/application&lt;/kbd&gt; APIs * Application related requests (with &#x60;X-Cumulocity-Application-Key&#x60; header)  | [optional] 
**DeviceWithChildrenCount** | **int** | Number of devices with children. Updated only three times a day starting at 8:57, 16:57 and 23:57. | [optional] 
**ExternalReference** | **string** | Tenant external reference. | [optional] 
**EventsCreatedCount** | **int** | Number of created events. | [optional] 
**EventsUpdatedCount** | **int** | Number of updates made to the events. | [optional] 
**InventoriesCreatedCount** | **int** | Number of created managed objects. | [optional] 
**InventoriesUpdatedCount** | **int** | Number of updates made to the managed objects. | [optional] 
**MeasurementsCreatedCount** | **int** | Number of created measurements.  &gt; **&amp;#9432; Info:** Bulk creation of measurements is handled in a way that each measurement is counted individually.  | [optional] 
**ParentTenantId** | **string** | ID of the parent tenant. | [optional] [readonly] 
**PeakDeviceCount** | **int** | Peak value of &#x60;deviceCount&#x60; calculated for the requested time period of the summary. | [optional] 
**PeakDeviceWithChildrenCount** | **int** | Peak value of &#x60;deviceWithChildrenCount&#x60; calculated for the requested time period of the summary. | [optional] 
**PeakStorageSize** | **int** | Peak value of used storage size in bytes, calculated for the requested time period of the summary. | [optional] 
**RequestCount** | **int** | Number of requests that were made against the tenant. Updated every 5 minutes. The following requests are not included:  *  Internal SmartREST requests used to resolve templates *  Internal SLA monitoring requests *  Calls to any &lt;kbd&gt;/health&lt;/kbd&gt; endpoint *  Device bootstrap process requests related to configuring and retrieving device credentials *  Microservice SDK internal calls for applications and subscriptions  | [optional] 
**Resources** | [**UsageStatisticsResources**](UsageStatisticsResources.md) |  | [optional] 
**StorageSize** | **int** | Database storage in use, specified in bytes. It is affected by your retention rules and by the regularly running database optimization functions in Cumulocity IoT. If the size decreases, it does not necessarily mean that data was deleted. Updated only three times a day starting at 8:57, 16:57 and 23:57. | [optional] 
**SubscribedApplications** | **List&lt;string&gt;** | Names of the tenant subscribed applications. Updated only three times a day starting at 8:57, 16:57 and 23:57. | [optional] 
**TenantCompany** | **string** | The tenant&#39;s company name. | [optional] 
**TenantCustomProperties** | [**CustomProperties**](CustomProperties.md) |  | [optional] 
**TenantDomain** | **string** | URL of the tenant&#39;s domain. The domain name permits only the use of alphanumeric characters separated by dots &#x60;.&#x60;, hyphens &#x60;-&#x60; and underscores &#x60;_&#x60;. | [optional] 
**TenantId** | **string** | Unique identifier of a Cumulocity IoT tenant. | [optional] [readonly] 
**TotalResourceCreateAndUpdateCount** | **int** | Sum of all inbound transfers. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

