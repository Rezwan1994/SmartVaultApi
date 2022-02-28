using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ASPNET_MVC_HelloWorldSVRest.Models
{
    public class ClientTokenResponse
    {
		// using System.Xml.Serialization;
		// XmlSerializer serializer = new XmlSerializer(typeof(Envelope));
		// using (StringReader reader = new StringReader(xml))
		// {
		//    var test = (Envelope)serializer.Deserialize(reader);
		// }

		[XmlRoot(ElementName = "error")]
		public class Error
		{

			[XmlElement(ElementName = "success")]
			public bool Success { get; set; }
		}

		[XmlRoot(ElementName = "message")]
		public class Message
		{

			[XmlElement(ElementName = "token")]
			public string Token { get; set; }

			[XmlElement(ElementName = "expires")]
			public int Expires { get; set; }
		}

		[XmlRoot(ElementName = "envelope")]
		public class Envelope
		{

			[XmlElement(ElementName = "error")]
			public Error Error { get; set; }

			[XmlElement(ElementName = "message")]
			public Message Message { get; set; }
		}


	}
}