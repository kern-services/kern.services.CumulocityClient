# kern.services.CumulocityClient.Model.TrustedCertificate

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**AlgorithmName** | **string** | Algorithm used to decode/encode the certificate. | [optional] [readonly] 
**AutoRegistrationEnabled** | **bool** | Indicates whether the automatic device registration is enabled or not. | [optional] [default to false]
**CertInPemFormat** | **string** | Trusted certificate in PEM format. | [optional] 
**Fingerprint** | **string** | Unique identifier of the trusted certificate. | [optional] [readonly] 
**Issuer** | **string** | The name of the organization which signed the certificate. | [optional] [readonly] 
**Name** | **string** | Name of the certificate. | [optional] 
**NotAfter** | **DateTime** | The end date and time of the certificate&#39;s validity. | [optional] [readonly] 
**NotBefore** | **DateTime** | The start date and time of the certificate&#39;s validity. | [optional] [readonly] 
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 
**SerialNumber** | **string** | The certificate&#39;s serial number. | [optional] [readonly] 
**Status** | **string** | Indicates if the certificate is active and can be used by the device to establish a connection to the Cumulocity IoT platform. | [optional] 
**Subject** | **string** | Name of the organization to which the certificate belongs. | [optional] [readonly] 
**_Version** | **int** | Version of the X.509 certificate standard. | [optional] [readonly] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

