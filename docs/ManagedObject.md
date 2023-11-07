# kern.services.CumulocityClient.Model.ManagedObject

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**CreationTime** | **DateTime** | The date and time when the object was created. | [optional] [readonly] 
**Id** | **string** | Unique identifier of the object. | [optional] [readonly] 
**LastUpdated** | **DateTime** | The date and time when the object was updated for the last time. | [optional] [readonly] 
**Name** | **string** | Human-readable name that is used for representing the object in user interfaces. | [optional] 
**Owner** | **string** | Username of the device&#39;s owner. | [optional] [readonly] 
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 
**Type** | **string** | The fragment type can be interpreted as _device class_, this means, devices with the same type can receive the same types of configuration, software, firmware and operations. The type value is indexed and is therefore used for queries. | [optional] 
**ChildAdditions** | [**ObjectChildAdditions**](ObjectChildAdditions.md) |  | [optional] 
**ChildAssets** | [**ObjectChildAssets**](ObjectChildAssets.md) |  | [optional] 
**ChildDevices** | [**ObjectChildDevices**](ObjectChildDevices.md) |  | [optional] 
**AdditionParents** | [**ObjectAdditionParents**](ObjectAdditionParents.md) |  | [optional] 
**AssetParents** | [**ObjectAssetParents**](ObjectAssetParents.md) |  | [optional] 
**DeviceParents** | [**ObjectDeviceParents**](ObjectDeviceParents.md) |  | [optional] 
**C8yIsDevice** | **Object** | A fragment which identifies this managed object as a device. | [optional] 
**C8yDeviceTypes** | **List&lt;string&gt;** | This fragment must be added in order to publish sample commands for a subset of devices sharing the same device type. If the fragment is present, the list of sample commands for a device type will be extended with the sample commands for the &#x60;c8y_DeviceTypes&#x60;. New sample commands created from the user interface will be created in the context of the &#x60;c8y_DeviceTypes&#x60;. | [optional] 
**C8ySupportedOperations** | **List&lt;string&gt;** | Lists the operations that are available for a particular device, so that applications can trigger the operations. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

