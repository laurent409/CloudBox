using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFServiceWebRole1
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [WebGet(UriTemplate = "/{container}/all-folders", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        Resp getAllFolders(string container);

        [OperationContract]
        [WebGet(UriTemplate = "/{container}/{folder}/all-files", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        Resp getAllFiles(string container, string folder);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/{container}/upload-file", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        String uploadFile(string container, Stream streamdata);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/{container}/download-file", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        String downloadFile(string container, Stream streamdata);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/{container}/download-zip", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        String downloadZipFile(string container, Stream streamdata);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/{container}/upload-archive-file", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        String uploadArchiveFile(string container, Stream streamdata);

    }


}
