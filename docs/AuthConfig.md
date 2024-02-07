# kern.services.CumulocityClient.Model.AuthConfig
Parameters determining the authentication process.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**AccessTokenToUserDataMapping** | [**AuthConfigAccessTokenToUserDataMapping**](AuthConfigAccessTokenToUserDataMapping.md) |  | [optional] 
**Audience** | **string** | SSO specific. Token audience. | [optional] 
**AuthorizationRequest** | [**AuthConfigAuthorizationRequest**](AuthConfigAuthorizationRequest.md) |  | [optional] 
**AuthenticationRestrictions** | [**BasicAuthenticationRestrictions**](BasicAuthenticationRestrictions.md) |  | [optional] 
**ButtonName** | **string** | SSO specific. Information for the UI about the name displayed on the external server login button. | [optional] 
**ClientId** | **string** | SSO specific. The identifier of the Cumulocity IoT tenant on the external authorization server. | [optional] 
**GrantType** | **string** | The authentication configuration grant type identifier. | [optional] 
**Id** | **string** | Unique identifier of this login option. | [optional] 
**Issuer** | **string** | SSO specific. External token issuer. | [optional] 
**LogoutRequest** | [**AuthConfigLogoutRequest**](AuthConfigLogoutRequest.md) |  | [optional] 
**OnlyManagementTenantAccess** | **bool** | Indicates whether the configuration is only accessible to the management tenant. | [optional] 
**OnNewUser** | [**AuthConfigOnNewUser**](AuthConfigOnNewUser.md) |  | [optional] 
**ProviderName** | **string** | The name of the authentication provider. | 
**RedirectToPlatform** | **string** | SSO specific. URL used for redirecting to the Cumulocity IoT platform. | [optional] 
**RefreshRequest** | [**AuthConfigRefreshRequest**](AuthConfigRefreshRequest.md) |  | [optional] 
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 
**SessionConfiguration** | [**OAuthSessionConfiguration**](OAuthSessionConfiguration.md) |  | [optional] 
**SignatureVerificationConfig** | [**AuthConfigSignatureVerificationConfig**](AuthConfigSignatureVerificationConfig.md) |  | [optional] 
**Template** | **string** | SSO specific. Template name used by the UI. | [optional] 
**TokenRequest** | [**AuthConfigTokenRequest**](AuthConfigTokenRequest.md) |  | [optional] 
**Type** | **string** | The authentication configuration type. Note that the value is case insensitive. | 
**UserIdConfig** | [**AuthConfigUserIdConfig**](AuthConfigUserIdConfig.md) |  | [optional] 
**UserManagementSource** | **string** | Indicates whether user data are managed internally by the Cumulocity IoT platform or by an external server. Note that the value is case insensitive. | [optional] 
**VisibleOnLoginPage** | **bool** | Information for the UI if the respective authentication form should be visible for the user. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

