# kern.services.CumulocityClient.Model.BulkNewDeviceRequest

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**NumberOfAll** | **int** | Number of lines processed from the CSV file, without the first line (column headers). | [optional] 
**NumberOfCreated** | **int** | Number of created device credentials. | [optional] 
**NumberOfFailed** | **int** | Number of failed creations of device credentials. | [optional] 
**NumberOfSuccessful** | **int** | Number of successful creations of device credentials. This counts both create and update operations. | [optional] 
**CredentialUpdatedList** | [**List&lt;BulkNewDeviceRequestCredentialUpdatedListInner&gt;**](BulkNewDeviceRequestCredentialUpdatedListInner.md) | An array with the updated device credentials. | [optional] 
**FailedCreationList** | [**List&lt;BulkNewDeviceRequestFailedCreationListInner&gt;**](BulkNewDeviceRequestFailedCreationListInner.md) | An array with details of the failed device credentials. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

