
## MVC-Azure-Explorer
_### This is a simple MVC Web application for browsing azure blob storage files._

**USAGE:**

1. Open the **web.config**
2. Modify the following keys in **appsettings**:

If you have a CDN endpoing this is the place to set it

**<add key="UrlCDN" value="http://MY_CDN_ENPOINT.mydomain.com"/>**

Your blob storage private URI

**<add key="UrlBlobStorage" value="https://YOUR_BLOB_URI.blob.core.windows.net"/>**

Your default container

**<add key="ContenedorImagenes" value="my-container"/>**

**<add key="ContenedorBrowser" value="my-container"/>**

Your azure storage connection string, you can generate it in your azure portal

**<add key="azurestorage" value="DefaultEndpointsProtocol=https;AccountName=YOUR_ACCOUNT_NAME_HERE;AccountKey=YOUR_ACCOUNT_KEY_HERE;"/>**


Build, and launch, that's all.


## Screenshot


![Screenshot]({{site.baseurl}}/Screenshots/azure_mvc_explorer_screenshot.PNG)