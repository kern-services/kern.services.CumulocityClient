
# kern.services.CumulocityClient.Model.LoginOption

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**AuthenticationRestrictions** | [**BasicAuthenticationRestrictions**](BasicAuthenticationRestrictions.md) |  | [optional] 
**EnforceStrength** | **bool** | Indicates if password strength is enforced. | [optional] 
**GrantType** | **string** | The grant type of the OAuth configuration. | [optional] 
**GreenMinLength** | **int** | Minimum length for the password when the strength validation is enforced. | [optional] 
**Id** | **string** | Unique identifier of this login option. | [optional] 
**InitRequest** | **string** | A URL linking to the token generating endpoint. | [optional] 
**LoginRedirectDomain** | **string** | The tenant domain. | [optional] 
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 
**SessionConfiguration** | [**OAuthSessionConfiguration**](OAuthSessionConfiguration.md) |  | [optional] 
**StrengthValidity** | **bool** | Enforce password strength validation on subtenant level. &#x60;enforceStrength&#x60; enforces it on all tenants in the platform. | [optional] 
**TfaStrategy** | **string** | Two-factor authentication being used by this login option. TFA supported: SMS and TOTP. | [optional] 
**Type** | **string** | The type of authentication. See [Authentication](#section/Authentication) for more details. | [optional] 
**UserManagementSource** | **string** | Specifies if the users are managed internally by Cumulocity IoT (&#x60;INTERNAL&#x60;) or if the users data are managed by a external system (&#x60;REMOTE&#x60;). | [optional] 
**VisibleOnLoginPage** | **bool** | Indicates if this login option is available in the login page (only for SSO). | [optional] 
**Type** | **string** | The type of authentication. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to README]](../README.md)

