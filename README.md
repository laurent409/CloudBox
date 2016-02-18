# CloudBox

## What's CloubBox ? 

CloudBox is a WCF (Windows Communication Foundation) which is made for being used with Azure Store Emulator blobs. 
You can interact with your blobs into containers like upload, download, zip and unzip. 

## How to use it ?

### - List Blobs Folders (GET Method) 

#### URI
You can list your folders into your container using this URI : 

`/Service1.svc/{container}/all-folders`

#### Specifications 
You have to specify the container in the URI which you want to get folders list.

#### Return 
This API will return to you an object with all details of folders into your specific container.

### - List Blobs Files (GET Method)

#### URI
You can list your files into your container using this URI : 

`/Service1.svc/{container}/{folder}/all-files`

#### Specifications 
You have to specify the container and the folder in the URI which you want to get files list.

#### Return 
This API will return to you an object with all details of folders into your specific container.

### - Upload a specific file (POST Method)

#### URI
You can upload a specific file into your container using this URI : 

`/Service1.svc/{container}/upload-file`

#### Specifications 
You have to specify the container in the URI which you want to upload a specific file.
You have to pass a JSON too for giving some descriptions of your file to upload : 

```
{
	"blob": "nameOfYourBlob",
	"pathFile": "C:\\Users\\Me\\Desktop",
	"nameFile": "example.txt"
}
```

If the file is a `.zip`, it will be unzipped on your local folder and files into your `.zip` file will be uploaded.

#### Return 
This API will return to you a String which give you your transaction status or an error report.

### - Download a specific file (POST Method)

#### URI
You can download a specific file from your container using this URI : 

`/Service1.svc/{container}/download-file`

#### Specifications 
You have to specify the container in the URI which you want to upload a specific file.
You have to pass a JSON too for giving some descriptions of your file to download : 

```
{
	"blob": "nameOfYourBlob",
	"pathBlobFile": "facultative\\Path",
	"nameBlobFile": "example.txt",
	"pathLocalFolder": "C:\\Users\\Me\\Desktop\\downloadHere"
}
```

#### Return 
This API will return to you a String which give you your transaction status or an error report.

### - Download a zip-file (POST Method)

#### URI
You can download a specific zip-file from your container using this URI : 

`/Service1.svc/{container}/download-zip`

#### Specifications 
You have to specify the container in the URI which you want to upload a zip-file.
You have to pass a JSON too for giving some descriptions of your zip to download : 

```
{
	"blob": "nameOfYourBlob",
	"pathBlobFile": "facultative\\Path",
	"nameBlobFile": "example.zip",
	"pathLocalFolder": "C:\\Users\\Me\\Desktop\\downloadHere"
}
```

#### Return 
This API will return to you a String which give you your transaction status or an error report.

### - Upload a folder to archives-blob (POST Method)

#### URI
You can zip a specific folder and upload zip-file to your container using this URI : 

`/Service1.svc/{container}/upload-archive-file`

#### Specifications 
You have to specify the container in the URI which you want to upload the zip-file.
You have to pass a JSON too for giving some descriptions of your folder to zip and upload these : 

```
{
	"pathFolder": "C:\\path\\of\\a\\folder\\to\\zip\\and\\upload"
}
```

#### Return 
This API will return to you a String which give you your transaction status or an error report.
