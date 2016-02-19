using Ionic.Zip;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;

namespace WCFServiceWebRole1
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez Service1.svc ou Service1.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public class Service1 : IService1
    {
        public Resp getAllFolders(string container)
        {
            try
            {
                Resp r = new Resp(container);

                foreach (IListBlobItem item in r._container.ListBlobs(null, false))
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory)item;
                    r.folders.Add(directory.Prefix.ToString());
                }

                return r;
            }
            catch (Exception e)
            {
                Resp r = new Resp(container);
                r.folders.Add("Error : " + e.ToString());
                return r;
            }
        }

        public Resp getAllFiles(string container, string folder)
        {
            try
            {
                Resp r = new Resp(container);

                foreach (IListBlobItem item in r._container.ListBlobs(null, false))
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory)item;

                    if (directory.Prefix.ToString().Equals(folder + "/"))
                    {
                        var blobs = directory.ListBlobs();
                        foreach (var blobItem in blobs)
                        {
                            string[] tNameFile = blobItem.Uri.ToString().Split(new[] { "/" }, StringSplitOptions.None);
                            r.files.Add(tNameFile[tNameFile.Count() - 1].ToString());
                        }
                    }
                }
                return r;
            }
            catch (Exception e)
            {
                Resp r = new Resp(container);
                r.files.Add("Error : " + e.ToString());
                return r;
            }

        }

        public String uploadFile(string container, Stream streamdata)
        {
            try
            {
                Resp r = new Resp(container);

                StreamReader reader = new StreamReader(streamdata);
                string json = reader.ReadToEnd();

                //var javascriptSerializer = new JavaScriptSerializer();

                Dictionary<String, String> data = JsonConvert.DeserializeObject<Dictionary<String, String>>(json);

                String blob = data["blob"].ToString();
                if (blob == "")
                {
                    return "Forbidden, it's root folder !";
                }
                String pathFile = data["pathFile"].ToString();
                String nameFile = data["nameFile"].ToString();

                String pathToLocalFile = pathFile + "\\" + nameFile;
                if (Path.GetExtension(pathToLocalFile) == ".zip")
                {
                    DirectoryInfo di = new DirectoryInfo(pathToLocalFile.Replace(".zip", string.Empty));
                    di.Create();
                    ZipFile zf = ZipFile.Read(pathToLocalFile);
                    zf.ExtractAll(di.ToString(), ExtractExistingFileAction.OverwriteSilently);
                    String nameFileWithoutZip = nameFile.Replace(".zip", string.Empty);
                    String pathFileWithoutZip = pathToLocalFile.Replace(".zip", string.Empty);

                    foreach (string file in Directory.EnumerateFiles(pathFileWithoutZip))
                    {
                        string[] tNameFile = file.Split(new[] { "\\" }, StringSplitOptions.None);
                        string newNameFile = tNameFile[tNameFile.Count() - 1].ToString();
                        CloudBlockBlob blockBlob = r._container.GetBlockBlobReference(blob + "\\" + newNameFile);
                        using (var fileStream = System.IO.File.OpenRead(file))
                        {
                            blockBlob.UploadFromStream(fileStream);
                        }
                    }
                    return "Uploaded";
                }
                else
                {
                    CloudBlockBlob blockBlob = r._container.GetBlockBlobReference(blob + "\\" + nameFile);
                    using (var fileStream = System.IO.File.OpenRead(pathToLocalFile))
                    {
                        blockBlob.UploadFromStream(fileStream);
                    }
                    return "Uploaded";
                }

                reader.Close();
                reader.Dispose();

            }
            catch (Exception e)
            {
                return "Error " + e.ToString();
            }
        }

        public String downloadFile(string container, Stream streamdata)
        {
            try
            {
                Resp r = new Resp(container);

                StreamReader reader = new StreamReader(streamdata);
                string json = reader.ReadToEnd();

                Dictionary<String, String> data = JsonConvert.DeserializeObject<Dictionary<String, String>>(json);

                String blob = data["blob"].ToString();
                String pathToBlobFile = data["pathBlobFile"].ToString();
                String nameBlobFile = data["nameBlobFile"].ToString();
                String pathLocalFolder = data["pathLocalFolder"].ToString();
                CloudBlockBlob blockBlob;
                if ( pathToBlobFile == "" ) {
                    blockBlob = r._container.GetBlockBlobReference(blob + "\\" + nameBlobFile);
                } else {
                    blockBlob = r._container.GetBlockBlobReference(blob + "\\" + pathToBlobFile + "\\" + nameBlobFile);
                }
                using (var fileStream = System.IO.File.OpenWrite(pathLocalFolder + "\\" + nameBlobFile))
                {
                    blockBlob.DownloadToStream(fileStream);
                }
                return "Downloaded";
            }
            catch (Exception e)
            {
                return "Error : " + e.ToString();
            }
        }

        public String downloadZipFile(string container, Stream streamdata)
        {
            try
            {
                Resp r = new Resp(container);

                StreamReader reader = new StreamReader(streamdata);
                string json = reader.ReadToEnd();

                Dictionary<String, String> data = JsonConvert.DeserializeObject<Dictionary<String, String>>(json);

                String blob = data["blob"].ToString();
                String pathToBlobFile = data["pathBlobFile"].ToString();
                String nameBlobFile = data["nameBlobFile"].ToString();
                String pathLocalFolder = data["pathLocalFolder"].ToString();

                if (Path.GetExtension(nameBlobFile) != ".zip")
                {
                    return "Error, it's not a zip file. Make sure you're trying to download a zip file !";
                }

                CloudBlockBlob blockBlob;
                if (pathToBlobFile == "")
                {
                    blockBlob = r._container.GetBlockBlobReference(blob + "\\" + nameBlobFile);
                }
                else
                {
                    blockBlob = r._container.GetBlockBlobReference(blob + "\\" + pathToBlobFile + "\\" + nameBlobFile);
                }
                String localPathFileToUnzip = pathLocalFolder + "\\" + nameBlobFile;
                using (var fileStream = System.IO.File.OpenWrite(localPathFileToUnzip))
                {
                    blockBlob.DownloadToStream(fileStream);
                }

                DirectoryInfo di = new DirectoryInfo(localPathFileToUnzip.Replace(".zip", string.Empty));
                di.Create();
                ZipFile zf = ZipFile.Read(localPathFileToUnzip);
                zf.ExtractAll(di.ToString(), ExtractExistingFileAction.DoNotOverwrite);

                return "Downloaded and unzipped";
            }
            catch (Exception e)
            {
                return "Error : " + e.ToString();
            }
        }

        public String uploadArchiveFile(string container, Stream streamdata)
        {
            try
            {
                Resp r = new Resp(container);

                StreamReader reader = new StreamReader(streamdata);
                string json = reader.ReadToEnd();

                Dictionary<String, String> data = JsonConvert.DeserializeObject<Dictionary<String, String>>(json);

                String blob = "archives";
                String namePathFolder = data["namePathFolder"].ToString();
                String nameLocalPath = "C:\\" + blob + "\\" + namePathFolder;
                DirectoryInfo di = new DirectoryInfo(nameLocalPath);
                di.Create();

                foreach (IListBlobItem item in r._container.ListBlobs(namePathFolder, true))
                {
                    string nameFileDownload = item.Uri.Segments.Last();
                    CloudBlockBlob blockBlobDl = r._container.GetBlockBlobReference(namePathFolder + "\\" + nameFileDownload);
                    using (var fileStream = System.IO.File.OpenWrite(nameLocalPath+ "\\" + nameFileDownload))
                    {
                        blockBlobDl.DownloadToStream(fileStream);
                    }
                    
                }
                String pathFolderZipped = Path.GetFileName(namePathFolder) + ".zip";
                String pathLocalZipped = nameLocalPath + ".zip";
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(nameLocalPath);
                    zip.Save(pathLocalZipped);
                }

                CloudBlockBlob blockBlobUpl = r._container.GetBlockBlobReference(blob + "\\" + pathFolderZipped);
                using (var fileStream = System.IO.File.OpenRead(pathLocalZipped))
                {
                    blockBlobUpl.UploadFromStream(fileStream);
                }

                return "Your folder was zipped and uploaded to archive blob - A folder \"archives\" was created in disk C:";
            }
            catch (Exception e)
            {
                return "Error : " + e.ToString();
            }
        }
    }
}
