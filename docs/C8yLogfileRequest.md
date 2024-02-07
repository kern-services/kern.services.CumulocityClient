# kern.services.CumulocityClient.Model.C8yLogfileRequest
Request a device to send a log file and view it in Cumulocity IoT's log viewer.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**LogFile** | **string** | Indicates the log file to select. | [optional] 
**DateFrom** | **DateTime** | Start date and time of log entries in the log file to be sent. | [optional] 
**DateTo** | **DateTime** | End date and time of log entries in the log file to be sent. | [optional] 
**SearchText** | **string** | Provide a text that needs to be present in the log entry. | [optional] 
**MaximumLines** | **int** | Upper limit of the number of lines that should be sent to Cumulocity IoT after filtering. | [optional] 
**File** | **string** | A link to the log file request. | [optional] [readonly] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

