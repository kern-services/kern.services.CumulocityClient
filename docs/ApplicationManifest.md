
# kern.services.CumulocityClient.Model.ApplicationManifest

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**ApiVersion** | **string** | Document type format discriminator, for future changes in format. | [optional] 
**BillingMode** | **string** | The billing mode of the application.  In case of RESOURCES, the number of resources used is exposed for billing calculation per usage. In case of SUBSCRIPTION, all resources usage is counted for the microservice owner and the subtenant is charged for subscription.  | [optional] [default to BillingModeEnum.RESOURCES]
**ContextPath** | **string** | The context path in the URL makes the application accessible. | [optional] 
**Extensions** | [**List&lt;MicroserviceApplicationManifestExtensionsInner&gt;**](MicroserviceApplicationManifestExtensionsInner.md) | A list of URL extensions for this microservice application. | [optional] 
**Isolation** | **string** | Deployment isolation. In case of PER_TENANT, there is a separate instance for each tenant. Otherwise, there is one single instance for all subscribed tenants. This will affect billing.  | [optional] 
**LivenessProbe** | [**MicroserviceApplicationManifestLivenessProbe**](MicroserviceApplicationManifestLivenessProbe.md) |  | [optional] 
**Provider** | [**MicroserviceApplicationManifestProvider**](MicroserviceApplicationManifestProvider.md) |  | [optional] 
**ReadinessProbe** | [**MicroserviceApplicationManifestReadinessProbe**](MicroserviceApplicationManifestReadinessProbe.md) |  | [optional] 
**RequestResources** | [**MicroserviceApplicationManifestRequestResources**](MicroserviceApplicationManifestRequestResources.md) |  | [optional] 
**Resources** | [**MicroserviceApplicationManifestResources**](MicroserviceApplicationManifestResources.md) |  | [optional] 
**Roles** | **List&lt;string&gt;** | Roles provided by the microservice. | [optional] 
**RequiredRoles** | **List&lt;string&gt;** | List of permissions required by a microservice to work. | [optional] 
**Scale** | **string** | Allows to configure a microservice auto scaling policy. If the microservice uses a lot of CPU resources, a second instance will be created automatically when this is set to &#x60;AUTO&#x60;. The default is &#x60;NONE&#x60;, meaning auto scaling will not happen.  | [optional] [default to ScaleEnum.NONE]
**Settings** | [**List&lt;ApplicationSettingsInner&gt;**](ApplicationSettingsInner.md) | A list of settings objects for this microservice application. | [optional] 
**SettingsCategory** | **string** | Allows to specify a custom category for microservice settings. By default, &#x60;contextPath&#x60; is used.  | [optional] 
**_Version** | **string** | Application version. Must be a correct [SemVer](https://semver.org/) value but the \&quot;+\&quot; sign is disallowed.  | [optional] 
**Webpaas** | **bool** | A legacy flag that identified a certain type of web application that would control the behavior of plugin tab in the application details view. It is no longer used.  | [optional] 
**ContentSecurityPolicy** | **string** | The content security policy of the application. | [optional] 
**NoAppSwitcher** | **bool** | A flag that decides if the application is shown in the app switcher on the UI. | [optional] 
**TabsHorizontal** | **bool** | A flag that decides if the application tabs are displayed horizontally or not. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models)
[[Back to API list]](../README.md#documentation-for-api-endpoints)
[[Back to README]](../README.md)

