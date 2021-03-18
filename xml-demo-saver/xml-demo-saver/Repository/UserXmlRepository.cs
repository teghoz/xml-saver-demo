using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using xml_demo_saver.Models;

namespace xml_demo_saver.Repository
{
    public class UserXmlRepository : IRepository
    {
        private XDocument _userXMLDocumentStore;
        private List<User> _users;
        private string _filePath;
        private string documentXName = "user";

        public UserXmlRepository(string filePath)
        {
            _filePath = filePath;
            _users = new List<User>();
            _userXMLDocumentStore = XDocument.Load(filePath);
            _users = _userXMLDocumentStore.Descendants(documentXName)
                .Select(t => new User
                {
                    Id = int.Parse(t.Element("id").Value),
                    FirstName = t.Element("firstName").Value,
                    SurName = t.Element("surname").Value,
                    PhoneNumber = t.Element("phoneNumber").Value,
                }).ToList();
        }
        public void AddUser(User user)
        {
            user.Id = GeneratedUserId();

            var elements = (new XElement("user",
                new XElement("id", user.Id),
                new XElement("firstName", user.FirstName),
                new XElement("surname", user.SurName),
                new XElement("phoneNumber", user.PhoneNumber)));

            _userXMLDocumentStore.Root.Add(elements);
            _userXMLDocumentStore.Save(_filePath);
        }
        private int GeneratedUserId()
        {
            int? query = _userXMLDocumentStore.Descendants(documentXName)
                .OrderByDescending(u => (int)(u.Element("id")))
                .Select(u => (int)(u.Element("id"))).FirstOrDefault();

            var lastId = (query ?? 0) + 1;
            return  lastId;
        }
        public void DeleteUser(User user)
        {
            _userXMLDocumentStore.Root.Elements(documentXName).Where(i => (int)i.Element("id") == user.Id).Remove();
            _userXMLDocumentStore.Save(_filePath);
        }
        public void DeleteUser(int id)
        {
            _userXMLDocumentStore.Root.Elements(documentXName).Where(i => (int)i.Element("id") == id).Remove();
            _userXMLDocumentStore.Save(_filePath);
        }
        public User UpdateUser(User user)
        {
            XElement node = _userXMLDocumentStore.Root.Elements(documentXName).Where(i => (int)i.Element("id") == user.Id).FirstOrDefault();
            node.SetElementValue("firstName", user.FirstName);
            node.SetElementValue("surname", user.SurName);
            node.SetElementValue("phoneNumber", user.PhoneNumber);
            _userXMLDocumentStore.Save(_filePath);

            return _userXMLDocumentStore.Descendants(documentXName)
                .Where(u => u.Element("id").Value == user.Id.ToString())
                .Select(t => new User
                {
                    Id = int.Parse(t.Element("id").Value),
                    FirstName = t.Element("firstName").Value,
                    SurName = t.Element("surname").Value,
                    PhoneNumber = t.Element("phoneNumber").Value,
                }).FirstOrDefault();
        }
        public List<User> Users(UserFilter filter)
        {
            Func<User, bool> query = null;

            if (filter.Id != 0)
            {
                query = u => u.Id == filter.Id;
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = u => u.FirstName.Contains(filter.Name) || u.SurName.Contains(filter.Name);
            }

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                query = u => u.PhoneNumber.Contains(filter.PhoneNumber);
            }

            if(query != null)
            {
                return _users.Where(query).ToList();
            }

            return _users.ToList();
        }
    }
}
