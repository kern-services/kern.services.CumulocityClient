# kern.services.CumulocityClient.Model.JSONPredicateRepresentation
Represents a predicate for verification. It acts as a condition which is necessary to assign a user to the given groups and permit access to the specified applications.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**ChildPredicates** | [**List&lt;JSONPredicateRepresentation&gt;**](JSONPredicateRepresentation.md) | Nested predicates. | [optional] 
**Operator** | **string** | Operator executed on the parameter from the JWT access token claim pointed by &#x60;parameterPath&#x60; and the provided parameter &#x60;value&#x60;. | [optional] 
**ParameterPath** | **string** | Path to the claim from the JWT access token from the external authorization server. | [optional] 
**Value** | **string** | Given value used for parameter verification. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

