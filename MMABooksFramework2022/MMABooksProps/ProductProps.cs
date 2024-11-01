using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MMABooksTools;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;

using System.Text.Json;
using System.Threading.Tasks;

namespace MMABooksProps
{
    [Serializable()]
    public class ProductProps : IBaseProps
    {
        // Properties for the Product
        public int ProductID { get; set; } = 0;
        public string ProductCode { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal UnitPrice { get; set; } = 0.00m;
        public int OnHandQuantity { get; set; } = 0;

        /// <summary>
        /// ConcurrencyID. Don't manipulate directly.
        /// </summary>
        public int ConcurrencyID { get; set; } = 0;

        // The Clone method will return a object,
        // that will be a a copy of another ProductProps
        // object. With each field of the going to be
        // copied object set to the matching fields of
        // the going to be created object.
        public object Clone()
        {
            ProductProps p = new ProductProps();
            p.ProductID = ProductID;
            p.ProductCode = ProductCode;
            p.Description = Description;
            p.UnitPrice = UnitPrice;
            p.OnHandQuantity = OnHandQuantity;
            p.ConcurrencyID = ConcurrencyID;
            return p;
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
            ProductProps p = JsonSerializer.Deserialize<ProductProps>(jsonString);
            this.ProductID = p.ProductID;
            this.ProductCode = p.ProductCode;
            this.Description = p.Description;
            this.UnitPrice = p.UnitPrice;
            this.OnHandQuantity= p.OnHandQuantity;
            this.ConcurrencyID = p.ConcurrencyID;
        }

        // The SetState method that takes a DBDataReader,
        // a object that was taken from a server such
        // as MySQL where it's converted to a ProductProps
        // object to now be used for whatever it is needed
        // in this code.
        public void SetState(DBDataReader dr)
        {
            this.ProductID = (int)dr["ProductID"];
            this.ProductCode = (string)dr["ProductCode"];
            this.Description = (string)dr["Description"];
            this.UnitPrice = (decimal)dr["UnitPrice"];
            this.OnHandQuantity = (int)dr["OnHandQuantity"];
            this.ConcurrencyID = (Int32)dr["ConcurrencyID"];
        }
    }
}
