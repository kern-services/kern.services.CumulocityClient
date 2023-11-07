# kern.services.CumulocityClient.Model.C8yCellTower
Detailed information about a neighbouring cell tower.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**RadioType** | **string** | The radio type of this cell tower. Can also be put directly in root JSON element if all cellTowers have same radioType. | [optional] 
**MobileCountryCode** | **decimal** | The Mobile Country Code (MCC). | 
**MobileNetworkCode** | **decimal** | The Mobile Network Code (MNC) for GSM, WCDMA and LTE. The SystemID (sid) for CDMA. | 
**LocationAreaCode** | **decimal** | The Location Area Code (LAC) for GSM, WCDMA and LTE. The Network ID for CDMA. | 
**CellId** | **decimal** | The Cell ID (CID) for GSM, WCDMA and LTE. The base station ID for CDMA. | 
**TimingAdvance** | **decimal** | The timing advance value for this cell tower when available. | [optional] 
**SignalStrength** | **decimal** | The signal strength for this cell tower in dBm. | [optional] 
**PrimaryScramblingCode** | **decimal** | The primary scrambling code for WCDMA and physical CellId for LTE. | [optional] 
**Serving** | **decimal** | Specify with 0/1 if the cell is serving or not. If not specified, the first cell is assumed to be serving. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

