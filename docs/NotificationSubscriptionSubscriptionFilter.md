# kern.services.CumulocityClient.Model.NotificationSubscriptionSubscriptionFilter
Applicable filters to the subscription.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Apis** | **List&lt;NotificationSubscriptionSubscriptionFilter.ApisEnum&gt;** | The Notifications are available for Alarms, Alarms with children, Device control, Events, Events with children, Inventory and Measurements for the &#x60;mo&#x60; context and for Alarms and Inventory for the &#x60;tenant&#x60; context. Alternatively, the wildcard &#x60;*&#x60; can be used to match all the permissible APIs within the bound context.  &gt; **&amp;#9432; Info:** the wildcard &#x60;*&#x60; cannot be used in conjunction with other values.  | [optional] 
**TypeFilter** | **string** | The data needs to have the specified value in its &#x60;type&#x60; property to meet the filter criteria. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

