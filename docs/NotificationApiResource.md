
# kern.services.CumulocityClient.Model.NotificationApiResource

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 
**NotificationSubscriptions** | [**NotificationApiResourceNotificationSubscriptions**](NotificationApiResourceNotificationSubscriptions.md) |  | [optional] 
**NotificationSubscriptionsBySource** | **string** | Read-only collection of all notification subscriptions for a specific source object. The placeholder {source} must be a unique ID of an object in the inventory. | [optional] 
**NotificationSubscriptionsBySourceAndContext** | **string** | Read-only collection of all notification subscriptions of a particular context and a specific source object. | [optional] 
**NotificationSubscriptionsByContext** | **string** | Read-only collection of all notification subscriptions of a particular context. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to README]](../README.md)

