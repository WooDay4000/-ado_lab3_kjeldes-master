using System;
using System.Collections.Generic;
using System.Text;

using MMABooksTools;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace MMABooksProps
{
    [Serializable()]
    public class CustomerProps : IBaseProps
    {
        // Properties for the customer
        public int CustomerID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string ZipCode { get; set; } = "";

        /// <summary>
        /// ConcurrencyID. Don't manipulate directly.
        /// </summary>
        public int ConcurrencyID { get; set; } = 0;

        // The Clone method will return a object,
        // that will be a a copy of another CustomerProps
        // object. With each field of the going to be
        // copied object set to the matching fields of
        // the going to be created object.
        public object Clone()
        {
            CustomerProps c = new CustomerProps();
            c.CustomerID = this.CustomerID;
            c.Name = this.Name;
            c.Address = this.Address;
            c.City = this.City;
            c.State = this.State;
            c.ZipCode = this.ZipCode;
            c.ConcurrencyID = this.ConcurrencyID;
            return c;
        }

        // The GetState method will return a string,
        // that consists of a JSON-formatted string
        // that is based on the object being used to
        // call the GetState method. With
        // JsonSerializer.Serialize turn the object into
        // a JSON-formatted string.
        public string GetState()
        {
            string jsonString;
            jsonString = JsonSerializer.Serialize(this);
            return jsonString;
        }

        // The SetState method is called on a object
        // turned JSON-formatted string variable, where
        // it's deserialize, meaning the JSON-formatted string
        // is turned back into a object with all
        // it's original fields.
        public void SetState(string jsonString)
        {
            CustomerProps c = JsonSerializer.Deserialize<CustomerProps>(jsonString);
            this.CustomerID = c.CustomerID;
            this.Name = c.Name;
            this.Address = c.Address;
            this.City = c.City;
            this.State = c.State;
            this.ZipCode = c.ZipCode;
            this.ConcurrencyID = c.ConcurrencyID;
        }

        // The SetState method that takes a DBDataReader,
        // a object that was taken from a server such
        // as MySQL where it's converted to a CustomerProps
        // object to now be used for whatever it is needed
        // in this code.
        public void SetState(DBDataReader dr)
        {
            this.CustomerID = (int)dr["CustomerID"];
            this.Name = (string)dr["Name"];
            this.Address = (string)dr["Address"];
            this.City = (string)dr["City"];
            this.State = (string)dr["State"];
            this.ZipCode = (string)dr["ZipCode"];
            this.ConcurrencyID = (Int32)dr["ConcurrencyID"];
        }
    }
}
