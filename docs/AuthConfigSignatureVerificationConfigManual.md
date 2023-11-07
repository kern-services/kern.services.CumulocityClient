# kern.services.CumulocityClient.Model.AuthConfigSignatureVerificationConfigManual
Describes the process of verification of JWT access token with the public keys embedded in the provided X.509 certificates.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**CertIdField** | **string** | The name of the field in the JWT access token containing the certificate identifier. | [optional] 
**CertIdFromField** | **bool** | Indicates whether the certificate identifier should be read from the JWT access token. | [optional] 
**Certificates** | [**AuthConfigSignatureVerificationConfigManualCertificates**](AuthConfigSignatureVerificationConfigManualCertificates.md) |  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

