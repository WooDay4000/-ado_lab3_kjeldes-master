using NUnit.Framework;

using MMABooksProps;
using MMABooksDB;

using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;

using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductDBTests
    {
        ProductDB db;

        public void ResetData()
        {
            db = new ProductDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestRetrieve()
        {
            ProductProps p = (ProductProps)db.Retrieve(1041);
            Assert.AreEqual(1041, p.ProductID);
            Assert.AreEqual("A4CS", p.ProductCode);
        }

        [Test]
        public void TestRetrieveAll()
        {
            List<ProductProps> list = (List<ProductProps>)db.RetrieveAll();
            Assert.AreEqual(16, list.Count);
        }

        [Test]
        public void TestDelete()
        {
            ProductProps p = (ProductProps)db.Retrieve(1041);
            Assert.True(db.Delete(p));
            Assert.Throws<Exception>(() => db.Retrieve(1041));
        }

        [Test]
        public void TestUpdate()
        {
            ProductProps p = (ProductProps)db.Retrieve(1041);
            p.Description = "Book about coding!";
            Assert.True(db.Update(p));
            p = (ProductProps)db.Retrieve(1041);
            Assert.AreEqual("Book about coding!", p.Description);
        }

        [Test]
        public void TestUpdateFieldTooLong()
        {
            // Retrieve the product from the database
            ProductProps p = (ProductProps)db.Retrieve(1041);

            // Check if the retrieved product is null
            if (p == null)
            {
                Assert.Fail("Product with ID 1041 not found.");
            }

            // Check if the Description field is not null before updating
            if (p.Description == null)
            {
                Assert.Fail("Product description is null.");
            }

            // Attempt to update the product with an overly long description
            p.Description = "abcdefghijklmnopqrstuoijshfghoiuhasdifhiaawdawdaddd";

            // Expecting MySqlException to be thrown when updating with long description
            Assert.Throws<MySqlException>(() => db.Update(p));
        }


        [Test]
        public void TestCreate()
        {
            CustomerProps p = new CustomerProps();
            p.Name = "Minnie Mouse";
            p.Address = "101 Main Street";
            p.City = "Orlando";
            p.State = "FL";
            p.ZipCode = "10001";

            db.Create(p);
            CustomerProps p2 = (CustomerProps)db.Retrieve(p.CustomerID);
            Assert.AreEqual(p.GetState(), p2.GetState());
        }
    }
}
