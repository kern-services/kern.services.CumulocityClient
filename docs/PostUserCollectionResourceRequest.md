
# kern.services.CumulocityClient.Model.PostUserCollectionResourceRequest

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Applications** | [**List&lt;Application&gt;**](Application.md) | A list of applications for this user. | [optional] [readonly] 
**CustomProperties** | [**CustomProperties**](CustomProperties.md) |  | [optional] 
**DisplayName** | **string** | The user&#39;s display name in Cumulocity IoT. | [optional] 
**Email** | **string** | The user&#39;s email address. | 
**Enabled** | **bool** | Indicates whether the user is enabled or not. Disabled users cannot log in or perform API requests. | [optional] [default to true]
**FirstName** | **string** | The user&#39;s first name. | [optional] 
**Groups** | [**UserGroups**](UserGroups.md) |  | [optional] 
**Id** | **string** | A unique identifier for this user. | [optional] [readonly] 
**LastName** | **string** | The user&#39;s last name. | [optional] 
**LastPasswordChange** | **DateTime** | The date and time when the user&#39;s password was last changed, in [ISO 8601 datetime format](https://www.w3.org/TR/NOTE-datetime). | [optional] [readonly] 
**Newsletter** | **bool** | Indicates whether the user is subscribed to the newsletter or not. | [optional] 
**Owner** | **string** | Identifier of the parent user. If present, indicates that a user belongs to a user hierarchy by pointing to its direct ancestor. Can only be set by users with role USER_MANAGEMENT_ADMIN during user creation. Otherwise it&#39;s assigned automatically. | [optional] [readonly] 
**Password** | **string** | The user&#39;s password. Only Latin1 characters are allowed.  If you do not specify a password when creating a new user with a POST request, it must contain the property &#x60;sendPasswordResetEmail&#x60; with a value of &#x60;true&#x60;.  | [optional] 
**PasswordStrength** | **string** | Indicates the password strength. The value can be GREEN, YELLOW or RED for decreasing password strengths. | [optional] [readonly] 
**Phone** | **string** | The user&#39;s phone number. | [optional] 
**Roles** | [**UserRoles**](UserRoles.md) |  | [optional] 
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 
**SendPasswordResetEmail** | **bool** | When set to &#x60;true&#x60;, this field will cause Cumulocity IoT to send a password reset email to the email address specified.  If there is no password specified when creating a new user with a POST request, this must be specified and it must be set to &#x60;true&#x60;.  | [optional] 
**ShouldResetPassword** | **bool** | Indicates if the user should reset the password on the next login. | [optional] [readonly] 
**TwoFactorAuthenticationEnabled** | **bool** | Indicates if the user has to use two-factor authentication to log in. | [optional] [readonly] [default to false]
**UserName** | **string** | The user&#39;s username. It can have a maximum of 1000 characters. | 
**DevicePermissions** | **Object** |  | [optional] [readonly] 

[[Back to Model list]](../README.md#documentation-for-models)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to README]](../README.md)

