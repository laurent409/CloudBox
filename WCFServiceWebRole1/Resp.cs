using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WCFServiceWebRole1
{
    [DataContract]
    public class Resp
    {

        public Resp(string container)
        {
            this.files = new List<string>();
            this.folders = new List<string>();

            this.storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            this.blobClient = storageAccount.CreateCloudBlobClient();

            this._container = blobClient.GetContainerReference(container);
            this._container.CreateIfNotExists();
        }

        public CloudStorageAccount storageAccount { get; set; }

        public CloudBlobClient blobClient { get; set; }

        public CloudBlobContainer _container { get; set; }

        [DataMember(Name = "files")]
        public List<string> files { get; set; }

        [DataMember(Name = "folders")]
        public List<string> folders { get; set; }
    }
}