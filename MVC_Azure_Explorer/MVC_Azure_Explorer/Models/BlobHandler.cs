using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace MVC_Azure_Explorer.Models
{
    public class BlobHandler
    {
        // Retrieve storage account from connection string.
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["azurestorage"]);

        private string documentsDirectoryUrl;

        /// <summary>
        /// Receives the users Id for where the pictures are and creates 
        /// a blob storage with that name if it does not exist.
        /// </summary>
        /// <param name="imageDirecoryUrl"></param>
        public BlobHandler(string documentsDirectory)
        {
            this.documentsDirectoryUrl = documentsDirectory;
            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container. 
            CloudBlobContainer container = blobClient.GetContainerReference(documentsDirectory);

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();

            //Make available to everyone
            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
        }

        public MemoryStream BlobStream(string filename, string containername)
        {
            try
            {
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containername);
                CloudBlockBlob blockBlob2 = container.GetBlockBlobReference(filename);
                var memoryStream = new MemoryStream();

                blockBlob2.DownloadToStream(memoryStream);
                return memoryStream;

            }
            catch (Exception)
            {

                return null;
            }



        }
        public bool BlobExists(string filename, string containername)
        {
            try
            {
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containername);
                CloudBlockBlob blockBlob2 = container.GetBlockBlobReference(filename);

                return blockBlob2.Exists();

          

            }
            catch (Exception)
            {

                return false;
            }



        }

        public bool Upload(IEnumerable<HttpPostedFileBase> file)
        {
            // Create the blob client.
            try
            {
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve a reference to a container. 
                CloudBlobContainer container = blobClient.GetContainerReference(documentsDirectoryUrl);

                if (file != null)
                {
                    foreach (var f in file)
                    {
                        if (f != null)
                        {
                            CloudBlockBlob blockBlob = container.GetBlockBlobReference(f.FileName);
                            blockBlob.UploadFromStream(f.InputStream);
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }


        }

        public bool Upload(HttpPostedFileBase file, string filename)
        {
            // Create the blob client.


            try
            {
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                if (filename.StartsWith("/"))
                {
                    filename = filename.Remove(0, 1);
                }
                // Retrieve a reference to a container. 
                CloudBlobContainer container = blobClient.GetContainerReference(documentsDirectoryUrl);

                if (file != null)
                {
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
                    blockBlob.UploadFromStream(file.InputStream);
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public CloudBlockBlob UploadToBlob(HttpPostedFileBase file, string filename)
        {
            // Create the blob client.


            try
            {
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                if (filename.StartsWith("/"))
                {
                    filename = filename.Remove(0, 1);
                }
                // Retrieve a reference to a container. 
                CloudBlobContainer container = blobClient.GetContainerReference(documentsDirectoryUrl);

                if (file != null)
                {
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
                    blockBlob.UploadFromStream(file.InputStream);
                   
                    return blockBlob;
                }
                else
                {
                    return null;
                }
          
            }
            catch (Exception)
            {

                return null;
            }
        }

        public CloudBlockBlob UploadToBlobFromStream(Stream file, string filename)
        {
            // Create the blob client.


            try
            {
                if(filename.StartsWith("/"))
                {
                    filename = filename.Remove(0, 1);
                }
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve a reference to a container. 
                CloudBlobContainer container = blobClient.GetContainerReference(documentsDirectoryUrl);

                if (file != null)
                {
                    file.Position = 0;
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
                    blockBlob.UploadFromStream(file, file.Length);

                    return blockBlob;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {

                return null;
            }
        }

        public List<string> GetBlobs()
        {
            // Create the blob client. 
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(documentsDirectoryUrl);

            List<string> blobs = new List<string>();

            // Loop over blobs within the container and output the URI to each of them
            foreach (var blobItem in container.ListBlobs())
                blobs.Add(blobItem.Uri.ToString());

            return blobs;
        }

        public bool ClearContainer(string p)
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(documentsDirectoryUrl);

            try
            {

                foreach (var blobItem in container.ListBlobs())
                {
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobItem.Uri.ToString());
                    blockBlob.Delete();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}