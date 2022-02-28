using System;
using SmartVault.Core;
using SmartVault.Proto;
using SmartVault.Rest;
using System.IO;
using System.Net;

namespace ASPNET_MVC_HelloWorldSVRest.Models
{
    public class SmartVault
    {
        private readonly PublicClientProtocol _protocol;
        private DelegatedProtocol _delegation;
        private readonly PublicClientCfg _clientCfg;

        public SmartVault()
        {
            const string clientId = "Test234"; // Change this to your client ID
            _clientCfg = new PublicClientCfg(new Uri("https://rest.smartvault.com"), clientId);
            _protocol = new PublicClientProtocol(_clientCfg);
        }

        public string RequestPin(string email)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            return _protocol.GetPinRequestEndpoint(email);
        }
        
        public void AuthenticateWithToken(string email, string token)
        {
            DelegationTokenResponse.Types.Message.Builder messageBuilder = new DelegationTokenResponse.Types.Message.Builder();
            messageBuilder.SetToken(token);
            messageBuilder.SetUserEmail(email);

            DelegationTokenResponse.Builder responseBuilder = new DelegationTokenResponse.Builder();
            responseBuilder.SetMessage(messageBuilder.Build());

            UserCfg cfg = new UserCfg(email, responseBuilder.Build(), _clientCfg);
            _delegation = new DelegatedProtocol(cfg);
        }

        public NodeResponse BrowsePath(string path)
        {
            const int children = 1; // Immediate contents only
            return _delegation.Navigate(new AbsolutePath(path), children);
        }

        public void UploadFile(Stream localFile, string remoteFolder, string fileName)
        {
            UploadFileRequest.Builder builder = UploadFileRequest.CreateBuilder();
            builder.Name = fileName;

            _delegation.PostFile(new AbsolutePath(remoteFolder), builder.Build(), localFile);

            localFile.Close();
        }

        public string GetDownloadLink(string remoteFile)
        {
            NodeResponse fileNode = BrowsePath(remoteFile);
            return fileNode.Message.DownloadLinkUri;
        }

    }
}
