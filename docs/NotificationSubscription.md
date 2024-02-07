
# kern.services.CumulocityClient.Model.NotificationSubscription

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Context** | **string** | The context within which the subscription is to be processed. &gt; **&amp;#9432; Info:** If the value is &#x60;mo&#x60;, then &#x60;source&#x60; must also be provided in the request body.  | 
**FragmentsToCopy** | **List&lt;string&gt;** | Transforms the data to *only* include specified custom fragments. Each custom fragment is identified by a unique name. If nothing is specified here, the data is forwarded as-is. | [optional] 
**Id** | **string** | Unique identifier of the subscription. | [optional] [readonly] 
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 
**Source** | [**NotificationSubscriptionSource**](NotificationSubscriptionSource.md) |  | [optional] 
**Subscription** | **string** | The subscription name. Each subscription is identified by a unique name within a specific context. | 
**SubscriptionFilter** | [**NotificationSubscriptionSubscriptionFilter**](NotificationSubscriptionSubscriptionFilter.md) |  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to README]](../README.md)

