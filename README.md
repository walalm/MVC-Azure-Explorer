
## MVC-Azure-Explorer
_This is a simple MVC Web application for browsing azure blob storage files._

**USAGE:**

1. Open the **web.config**
2. Modify the following keys in **appsettings**:

If you have a CDN endpoing this is the place to set it

**key="UrlCDN" value="http://MY_CDN_ENPOINT.mydomain.com"**

Your blob storage private URI

**key="UrlBlobStorage" value="https://YOUR_BLOB_URI.blob.core.windows.net"**

Your default container

**key="ContenedorImagenes" value="my-container"**

**key="ContenedorBrowser" value="my-container"**

Your azure storage connection string, you can generate it in your azure portal

**key="azurestorage" value="DefaultEndpointsProtocol=https;AccountName=YOUR_ACCOUNT_NAME_HERE;AccountKey=YOUR_ACCOUNT_KEY_HERE;"**


Build, and launch, that's all.


## Screenshot

![Screenshot](https://raw.githubusercontent.com/walalm/MVC-Azure-Explorer/master/Screenshots/azure_mvc_explorer_screenshot.PNG)
