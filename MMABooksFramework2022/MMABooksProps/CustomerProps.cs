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
        public string City { get; set; }
        public string State { get; set; } = "";
        public string ZipCode { get; set; } = "";

        /// <summary>
        /// ConcurrencyID. Don't manipulate directly.
        /// </summary>
        public int ConcurrencyID { get; set; } = 0;

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

        public string GetState()
        {
            string jsonString;
            jsonString = JsonSerializer.Serialize(this);
            return jsonString;
        }

        public void SetState(string jsonString)
        {
            CustomerProps p = JsonSerializer.Deserialize<CustomerProps>(jsonString);
            this.CustomerID = p.CustomerID;
            this.Name = p.Name;
            this.Address = p.Address;
            this.City = p.City;
            this.State = p.State;
            this.ZipCode = p.ZipCode;
            this.ConcurrencyID = p.ConcurrencyID;
        }

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
