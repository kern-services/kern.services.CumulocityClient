# kern.services.CumulocityClient.Model.CurrentUser
The current user.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**EffectiveRoles** | [**List&lt;Role&gt;**](Role.md) | A list of user roles. | [optional] [readonly] 
**Email** | **string** | The user&#39;s email address. | [optional] 
**FirstName** | **string** | The user&#39;s first name. | [optional] 
**Id** | **string** | A unique identifier for this user. | [optional] [readonly] 
**LastName** | **string** | The user&#39;s last name. | [optional] 
**LastPasswordChange** | **DateTime** | The date and time when the user&#39;s password was last changed, in [ISO 8601 datetime format](https://www.w3.org/TR/NOTE-datetime). | [optional] [readonly] 
**Password** | **string** | The user&#39;s password. Only Latin1 characters are allowed. | [optional] 
**Phone** | **string** | The user&#39;s phone number. | [optional] 
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 
**ShouldResetPassword** | **bool** | Indicates if the user should reset the password on the next login. | [optional] [readonly] 
**TwoFactorAuthenticationEnabled** | **bool** | Indicates if the user has to use two-factor authentication to log in. | [optional] [readonly] [default to false]
**UserName** | **string** | The user&#39;s username. It can have a maximum of 1000 characters. | [optional] 
**DevicePermissions** | **Dictionary&lt;string, List&lt;string&gt;&gt;** | An object with a list of the user&#39;s device permissions. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

