# kern.services.CumulocityClient.Model.PutApplicationResourceRequest

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Availability** | **string** | Application access level for other tenants. | [optional] [default to AvailabilityEnum.PRIVATE]
**ContextPath** | **string** | The context path in the URL makes the application accessible. Mandatory when the type of the application is &#x60;HOSTED&#x60;. | [optional] 
**Description** | **string** | Description of the application. | [optional] 
**Id** | **string** | Unique identifier of the application. | [optional] [readonly] 
**Key** | **string** | Applications, regardless of their form, are identified by an application key. | [optional] 
**Name** | **string** | Name of the application. | [optional] 
**Owner** | [**ApplicationOwner**](ApplicationOwner.md) |  | [optional] 
**Self** | **string** | A URL linking to this resource. | [optional] [readonly] 
**Type** | **Object** |  | [optional] [readonly] 
**Manifest** | [**ApplicationManifest**](ApplicationManifest.md) |  | [optional] 
**Roles** | **List&lt;string&gt;** | Roles provided by the microservice. | [optional] 
**RequiredRoles** | **List&lt;string&gt;** | List of permissions required by a microservice to work. | [optional] 
**Breadcrumbs** | **bool** | A flag to indicate if the application has a breadcrumbs navigation on the UI. &gt; **&amp;#9432; Info:** This property is specific to the web application type.  | [optional] 
**ContentSecurityPolicy** | **string** | The content security policy of the application. &gt; **&amp;#9432; Info:** This property is specific to the web application type.  | [optional] 
**DynamicOptionsUrl** | **string** | A URL to a JSON object with dynamic content options. &gt; **&amp;#9432; Info:** This property is specific to the web application type.  | [optional] 
**GlobalTitle** | **string** | The global title of the application. &gt; **&amp;#9432; Info:** This property is specific to the web application type.  | [optional] 
**Legacy** | **bool** | A flag that shows if the application is a legacy application or not. &gt; **&amp;#9432; Info:** This property is specific to the web application type.  | [optional] 
**RightDrawer** | **bool** | A flag to indicate if the application uses the UI context menu on the right side. &gt; **&amp;#9432; Info:** This property is specific to the web application type.  | [optional] 
**Upgrade** | **bool** | A flag that shows if the application is hybrid and using Angular and AngularJS simultaneously. &gt; **&amp;#9432; Info:** This property is specific to the web application type.  | [optional] 
**ActiveVersionId** | **string** | The active version ID of the application. For microservice applications the active version ID is the microservice manifest version ID. | [optional] [readonly] 
**ResourcesUrl** | **string** | URL to the application base directory hosted on an external server. Only present in legacy hosted applications. | [optional] [readonly] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

