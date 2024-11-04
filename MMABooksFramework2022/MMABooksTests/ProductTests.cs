using NUnit.Framework;

using MMABooksBusiness;
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
    public class ProductTests
    {
        [SetUp]
        // This will run before each test to clear the
        // the ProductDB object and to run the stored
        // procedure usp_testingResetProductData to restart
        // the Product table that theses tests are going to
        // be interacting with back to how they were before
        // running a test.
        public void TestResetDatabase()
        {
            ProductDB db = new ProductDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetProductData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        // This test is used to test the Default constructor
        // of the Product Class. Where it's tested first by
        // seeing if it's empty by using AreEqual to check
        // if it's fields are empty. Second, that it tests
        // if it was able to be made, checking if the IsNew
        // variable form BaseBusiness was set to true, and
        // that IsValid is false since it was just made and has
        // none of the required fields.
        public void TestNewStateConstructor()
        {
            // not in Data Store - no code
            Product p = new Product();
            Assert.AreEqual(string.Empty, p.ProductCode);
            Assert.AreEqual(string.Empty, p.Description);
            Assert.IsTrue(p.IsNew);
            Assert.IsFalse(p.IsValid);
        }


        [Test]
        // This tests to see if the Product record is able to receive
        // a Product record from the database when a valid
        // ProductID is entered into the Product constructor.
        // Using AreEqual to see if the received Product record 
        // has all the expected field values. Then checking IsNew
        // is false since it was a already created record, and IsValid
        // is true since it was in the database which means it was
        // validated.
        public void TestRetrieveFromDataStoreConstructor()
        {
            // retrieves from Data Store
            Product p = new Product(1);
            Assert.AreEqual("A4CS", p.ProductCode);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", p.Description);
            Assert.AreEqual(56.5000, p.UnitPrice);
            Assert.AreEqual(4637, p.OnHandQuantity);
            Assert.IsFalse(p.IsNew);
            Assert.IsTrue(p.IsValid);
        }

        [Test]
        // Tests to see if a new Product record is able to be saved
        // to the database using the Save method from BaseBusiness
        // and checking if it was saved correctly by using AreEqual
        // to check if the Product we created match the field values
        // of the saved Product when it's received back from the database.
        public void TestSaveToDataStore()
        {
            Product p = new Product();
            p.ProductCode = "UHBI";
            p.Description = "Book About Coding Or Whatever";
            p.UnitPrice = 10.9900m;
            p.OnHandQuantity = 350;
            p.Save();
            Product p2 = new Product(p.ProductID);
            Assert.AreEqual(p2.ProductCode, p.ProductCode);
            Assert.AreEqual(p2.Description, p.Description);
        }

        [Test]
        // Tests to see if a existing Product record from the database
        // is able to updated and for this change to be saved.
        // Where we first receive a existing Product record,
        // update a field of it, which for this is Description, and
        // then using the Save method to save this change to
        // the database. Where to check if this worked we
        // receive the the updated record from the database,
        // and use AreEqual to check the updated fields are set
        // to what they are supposed to be.
        public void TestUpdate()
        {
            Product p = new Product(1);
            p.Description = "Coding With Richard Stew";
            p.Save();

            Product p2 = new Product(1);
            Assert.AreEqual(p2.ProductCode, p.ProductCode);
            Assert.AreEqual(p2.Description, p.Description);
        }

        [Test]
        // Tests to see if a existing Product record from the database
        // is able to be deleted and for this to be saved. Where
        // it first receives a existing Product record, runs it
        // with the Delete method to have it deleted from the database,
        // and then uses the Save method to have this change saved.
        // Where it should throw an exception when it tries to receive
        // the deleted Product record, since it shouldn't exist anymore.
        public void TestDelete()
        {
            Product p = new Product(16);
            p.Delete();
            p.Save();
            Assert.Throws<Exception>(() => new Product(16));
        }

        [Test]
        // Tests GetList with the Product, and if it's able to receive
        // all the current Product records from the database, where it
        // runs GetList to populate a Product object list with all the
        // current Product records from the database. Using AreEqual
        // to check if it has the right amount of records that should be
        // in the database, then two more times to check two of the fields
        // of the first Product record in the list to see if the data was
        // was correctly received from the database.
        public void TestGetList()
        {
            Product p = new Product();
            List<Product> products = (List<Product>)p.GetList();
            Assert.AreEqual(16, products.Count);
            // There ordered by ProductCode by the procedure.
            Assert.AreEqual("A4CS", products[0].ProductCode);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", products[0].Description);
        }

        [Test]
        // Test used to check if it throws an exception if
        // it doesn't have all the required fields set when it
        // tries to save a Product record to the database.
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - all fields must be provided
            Product p = new Product();
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        // Tests to see if it will still throw an exaction
        // when trying to save a Product record to the database
        // when only some of the required fields are set.
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            Product p = new Product();
            Assert.Throws<Exception>(() => p.Save());
            p.Description = "Something about Coding";
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        // This test is used to check if it throws an
        // ArgumentOutOfRangeException when it tries to
        // set a field to an invalid value given it's
        // validation.
        public void TestInvalidPropertySet()
        {
            Product p = new Product();
            Assert.Throws<ArgumentOutOfRangeException>(() => p.Description = "abcdefghijklmnopqrstuoijshfghoiuhasdifhiaawdawdaddw");
        }

        [Test]
        // This tests to see if a concurrency issue will occur
        // when another instance of the same Product record
        // tries to save, where with this it makes two
        // instances of the Product record. Changes the
        // the first instance Description field to something different
        // then saves this change, then it does the same with the
        // second instance except when it goes to save this change
        // it should throw an exception since it's the same
        // Product record tried to be saved.
        public void TestConcurrencyIssue()
        {
            Product p1 = new Product(1);
            Product p2 = new Product(1);

            p1.Description = "Updated first";
            p1.Save();

            p2.Description = "Updated second";
            Assert.Throws<Exception>(() => p2.Save());
        }
    }
}
