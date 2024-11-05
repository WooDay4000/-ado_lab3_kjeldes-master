using NUnit.Framework;

using MMABooksProps;
using MMABooksDB;

using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;

using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductDBTests
    {
        // Creates a public ProductDB class that will be used
        // in all the tests to be able to use and test the
        // ProductDB methods.
        ProductDB db;
        [SetUp]
        // This will run before each test to clear the the ProductDB
        // object and to run the stored procedure usp_testingResetProductData
        // to restart the Product table that theses tests are going to be
        // interacting with back to how they were before running a test.
        public void ResetData()
        {
            db = new ProductDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetProductData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        // This is used to test the Retrieve method from the
        // ProductDB class. Where it retrieves a record from
        // the data, and they using AreEqual to see if the
        // fields from the collected record match to what we
        // expect them to be.
        public void TestRetrieve()
        {
            ProductProps p = (ProductProps)db.Retrieve(1);
            Assert.AreEqual(1, p.ProductID);
            Assert.AreEqual("A4CS", p.ProductCode);
        }

        [Test]
        // This is used to test the RetrieveAll method from
        // the ProductDB class, where it runs the RetrieveAll
        // method to set the list to all the current records
        // in the Product table in the database. Using
        // AreEqual to see if it was able to grab all the
        // current record by seeing if the grabbed amount
        // matches the number of records there are
        // supposed to be.
        public void TestRetrieveAll()
        {
            List<ProductProps> list = (List<ProductProps>)db.RetrieveAll();
            Assert.AreEqual(16, list.Count);
        }

        [Test]
        // This is used to test the Delete method from
        // the ProductDB class. Where it first uses the Retrieve
        // method to grab a already existing record, then using
        // the Delete method, then using Retrieve method again on the
        // same record checking to see if it throws an error
        // since the record should not exist anymore.
        public void TestDelete()
        {
            ProductProps p = (ProductProps)db.Retrieve(16);
            Assert.True(db.Delete(p));
            Assert.Throws<Exception>(() => db.Retrieve(16));
        }

        [Test]
        // This method is used to test the Update method from
        // the ProductDB class, where it first uses the Retrieve
        // method to grab a already made object, they sets
        // the Name field to something new, running the
        // Update method with the edited record to update the
        // record in the database. Running the Retrieve method
        // again with the same record, using AreEqual to see if
        // it's Name field was updated to what we wanted it to.
        public void TestUpdate()
        {
            ProductProps p = (ProductProps)db.Retrieve(1);
            p.Description = "Book about coding!";
            Assert.True(db.Update(p));
            p = (ProductProps)db.Retrieve(1);
            Assert.AreEqual("Book about coding!", p.Description);
        }

        [Test]
        // This test is used to see if the Update method won't
        // save a change to a record, when a specified field's
        // value is way too long.
        public void TestUpdateFieldTooLong()
        {
            ProductProps p = (ProductProps)db.Retrieve(1);
            p.Description = "abcdefghijklmnopqrstuoijshfghoiuhasdifhiasawdawdawd";
            Assert.Throws<MySqlException>(() => db.Update(p));
        }
        // Don't need to do a test on the updated field being too small
        // or wrong data type, because MySQL is only looking for a certain
        // datatype and allows for small or even empty fields.

        [Test]
        // This method is used to test the Create method from
        // the ProductDB class, where we populate the fields
        // of a ProductProps object which then we run it though
        // the Create method to have it added to the Product table
        // of the database. Where using the Receive method again we
        // grab the recently created record, using AreEqual to see
        // if the one we made matches the one we just added.
        public void TestCreate()
        {
            ProductProps p = new ProductProps();
            p.ProductCode = "UHBI";
            p.Description = "Book About Coding Or Whatever";
            p.UnitPrice = 10.9900m;
            p.OnHandQuantity = 350;

            db.Create(p);
            ProductProps p2 = (ProductProps)db.Retrieve(p.ProductID);
            Assert.AreEqual(p.GetState(), p2.GetState());
        }
    }
}
