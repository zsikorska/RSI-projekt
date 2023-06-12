using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace MyWebService
{
    [ServiceContract]
    public interface IRestService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/persons")]
        List<Person> getAllXml();

        [OperationContract]
        [WebGet(UriTemplate = "/persons/{id}", ResponseFormat = WebMessageFormat.Xml)]
        Person getByIdXml(string Id);

        [OperationContract]
        [WebInvoke(UriTemplate = "/persons", Method = "PUT", ResponseFormat = WebMessageFormat.Xml)]
        string editXml(Person item);

        [OperationContract]
        [WebInvoke(UriTemplate = "/persons", Method = "POST", RequestFormat = WebMessageFormat.Xml)]
        string addXml(Person item);

        [OperationContract]
        [WebInvoke(UriTemplate = "/persons/{id}", Method = "DELETE", RequestFormat = WebMessageFormat.Xml)]
        string deleteXml(string Id);

        [OperationContract]
        [WebGet(UriTemplate = "/persons/size", ResponseFormat = WebMessageFormat.Xml)]
        int getSizeXml();

        [OperationContract]
        [WebGet(UriTemplate = "/persons/name/{name}", ResponseFormat = WebMessageFormat.Xml)]
        List<Person> getByNameXml(string Name);


        [OperationContract]
        [WebGet(UriTemplate = "/json/persons", ResponseFormat = WebMessageFormat.Json)]
        List<Person> getAllJson();

        [OperationContract]
        [WebGet(UriTemplate = "/json/persons/{id}", ResponseFormat = WebMessageFormat.Json)]
        Person getByIdJson(string Id);

        [OperationContract]
        [WebInvoke(UriTemplate = "/json/persons", Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string editJson(Person item);

        [OperationContract]
        [WebInvoke(UriTemplate = "/json/persons", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string addJson(Person item);

        [OperationContract]
        [WebInvoke(UriTemplate = "/json/persons/{id}", Method = "DELETE", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string deleteJson(string Id);

        [OperationContract]
        [WebGet(UriTemplate = "/json/persons/size", ResponseFormat = WebMessageFormat.Json)]
        int getSizeJson();

        [OperationContract]
        [WebGet(UriTemplate = "/json/persons/name/{name}", ResponseFormat = WebMessageFormat.Json)]
        List<Person> getByNameJson(string Name);

        [OperationContract]
        [WebGet(UriTemplate = "/authors", ResponseFormat = WebMessageFormat.Xml)]
        string getAuthorsXml();

        [OperationContract]
        [WebGet(UriTemplate = "/json/authors", ResponseFormat = WebMessageFormat.Json)]
        string getAuthorsJson();

    }


    [DataContract]
    public class Person
    {
        [DataMember(Order = 0)]
        public int Id { get; set; }

        [DataMember(Order = 1)]
        public string Name { get; set; }

        [DataMember(Order = 2)]
        public int Age { get; set; }

        [DataMember(Order = 3)]
        public string Email { get; set; }
    }
}
