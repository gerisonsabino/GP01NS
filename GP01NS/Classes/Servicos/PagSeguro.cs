using GP01NS.Classes.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace GP01NS.Classes.Servicos
{
    public static class PagSeguro
    {
        private static string SandboxUrl = @"https://ws.sandbox.pagseguro.uol.com.br/v2/checkout/";
        private static string PaymentUrl = @"https://sandbox.pagseguro.uol.com.br/v2/checkout/payment.html?code=";
        private static string Email = "nossoshow@spam4.me";
        private static string Token = "1D29007950E64637A2F2E8227B230E65";

        public static string Checkout(MusicoVM musico)
        {
            NameValueCollection postData = new System.Collections.Specialized.NameValueCollection();

            postData.Add("email", Email);
            postData.Add("token", Token);
            postData.Add("currency", "BRL");
            postData.Add("itemId1", "0001");
            postData.Add("itemDescription1", "Impulsionar perfil - Nosso Show");
            postData.Add("itemAmount1", "10.00");
            postData.Add("itemQuantity1", "1");
            postData.Add("itemWeight1", "200");
            postData.Add("reference", "REF1234");
            postData.Add("senderName", musico.Nome);
            postData.Add("senderAreaCode", "44");
            postData.Add("senderPhone", "999999999");
            postData.Add("senderEmail", musico.NomeArtistico.Replace(" ", string.Empty).ToLower() + "@sandbox.pagseguro.com.br");
            
            postData.Add("shippingAddressRequired", "false");

            string xmlString = null;

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                var result = wc.UploadValues(SandboxUrl, postData);

                xmlString = Encoding.ASCII.GetString(result);
            }

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(xmlString);

            var code = xmlDoc.GetElementsByTagName("code")[0];
            var date = xmlDoc.GetElementsByTagName("date")[0];

            return string.Concat(PaymentUrl, code.InnerText);
        }

        public static string Checkout(EstabelecimentoVM estabelecimento)
        {
            NameValueCollection postData = new System.Collections.Specialized.NameValueCollection();

            postData.Add("email", Email);
            postData.Add("token", Token);
            postData.Add("currency", "BRL");
            postData.Add("itemId1", "0001");
            postData.Add("itemDescription1", "Impulsionar perfil - Nosso Show");
            postData.Add("itemAmount1", "10.00");
            postData.Add("itemQuantity1", "1");
            postData.Add("itemWeight1", "200");
            postData.Add("reference", "REF1234");
            postData.Add("senderName", estabelecimento.Nome);
            postData.Add("senderAreaCode", "44");
            postData.Add("senderPhone", "999999999");
            postData.Add("senderEmail", estabelecimento.Nome.Replace(" ", string.Empty).ToLower() + "@sandbox.pagseguro.com.br");

            postData.Add("shippingAddressRequired", "false");

            string xmlString = null;

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                var result = wc.UploadValues(SandboxUrl, postData);

                xmlString = Encoding.ASCII.GetString(result);
            }

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(xmlString);

            var code = xmlDoc.GetElementsByTagName("code")[0];
            var date = xmlDoc.GetElementsByTagName("date")[0];

            return string.Concat(PaymentUrl, code.InnerText);
        }
    }
}