
# kern.services.CumulocityClient.Model.PostRetentionRuleCollectionResourceRequest

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**DataType** | **string** | The data type(s) to which the rule is applied. | [optional] [default to DataTypeEnum.Star]
**Editable** | **bool** | Indicates whether the rule is editable or not. It can be updated only by the Management tenant. | [optional] [default to true]
**FragmentType** | **string** | The fragment type(s) to which the rule is applied. Used by the data types EVENT, MEASUREMENT, OPERATION and BULK_OPERATION. | [optional] [default to "*"]
**Id** | **string** | Unique identifier of the retention rule. | [optional] [readonly] 
**MaximumAge** | **int** | Maximum age expressed in number of days. | 
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 
**Source** | **string** | The source(s) to which the rule is applied. Used by all data types. | [optional] [default to "*"]
**Type** | **string** | The type(s) to which the rule is applied. Used by the data types ALARM, AUDIT, EVENT and MEASUREMENT. | [optional] [default to "*"]

[[Back to Model list]](../README.md#documentation-for-models)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to README]](../README.md)

