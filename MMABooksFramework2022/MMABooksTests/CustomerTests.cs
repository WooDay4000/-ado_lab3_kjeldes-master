using NUnit.Framework;

using MMABooksBusiness;
using MMABooksProps;
using MMABooksDB;

using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMABooksTests
{
    public class CustomerTests
    {
        [SetUp]
        // This will run before each test to clear the the CustomerDB
        // object and to run the stored procedures usp_testingResetCustomer1Data
        // and usp_testingResetCustomer2Data to restart the Customer
        // table that theses tests are going to be interacting with
        // back to how they were before running a test.
        public void TestResetDatabase()
        {
            CustomerDB db = new CustomerDB();

            // Having the first procedure run.
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetCustomer1Data";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);

            // Having the second procedure run.
            DBCommand command2 = new DBCommand();
            command2.CommandText = "usp_testingResetCustomer2Data";
            command2.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command2);
        }

        [Test]
        // This test is used to test the Default constructor
        // of the Customer Class. Where it's tested first by
        // seeing if it's empty by using AreEqual to check
        // if it's fields are empty. Second, that it tests
        // if it was able to be made, checking if the IsNew
        // variable form BaseBusiness was set to true, and
        // that IsValid is false since it was just made and has
        // none of the required fields.
        public void TestNewCustomerConstructor()
        {
            // not in Data Store - no code
            // Should ask if there is more to this.
            Customer c = new Customer();
            Assert.AreEqual(string.Empty, c.Name);
            Assert.AreEqual(string.Empty, c.Address);
            Assert.IsTrue(c.IsNew);
            Assert.IsFalse(c.IsValid);
        }

        [Test]
        // This tests to see if the Customer record is able to receive
        // a Customer record from the database when a valid
        // CustomerID is entered into the Customer constructor.
        // Using AreEqual to see if the received Customer record 
        // has all the expected field values. Then checking IsNew
        // is false since it was a already created record, and IsValid
        // is true since it was in the database which means it was
        // validated.
        public void TestRetrieveFromDataStoreConstructor()
        {
            // retrieves from Data Store
            Customer c = new Customer(1);
            Assert.AreEqual("Molunguri, A", c.Name);
            Assert.AreEqual("1108 Johanna Bay Drive", c.Address);
            Assert.AreEqual("Birmingham", c.City);
            Assert.AreEqual("AL", c.State);
            Assert.AreEqual("35216-6909", c.ZipCode);
            Assert.IsFalse(c.IsNew);
            Assert.IsTrue(c.IsValid);
        }

        [Test]
        // Tests to see if a new Customer record is able to be saved
        // to the database using the Save method from BaseBusiness
        // and checking if it was saved correctly by using AreEqual
        // to check if the Customer we created match the field values
        // of the saved Customer when it's received back from the database.
        public void TestSaveToDataStore()
        {
            Customer c = new Customer();
            c.Name = "Minnie Mouse";
            c.Address = "101 Main Street";
            c.City = "Orlando";
            c.State = "FL";
            c.ZipCode = "10001";
            c.Save();
            Customer c2 = new Customer(c.CustomerID);
            Assert.AreEqual(c2.Name, c.Name);
            Assert.AreEqual(c2.Address, c.Address);
        }

        [Test]
        // Tests to see if a existing Customer record from the database
        // is able to updated and for this change to be saved.
        // Where we first receive a existing Customer record,
        // update a field of it, which for this is Name, and
        // then using the Save method to save this change to
        // the database. Where to check if this worked we
        // receive the the updated record from the database,
        // and use AreEqual to check the updated fields are set
        // to what they are supposed to be.
        public void TestUpdate()
        {
            Customer c = new Customer(1);
            c.Name = "Richard Stew";
            c.Save();

            Customer c2 = new Customer(1);
            Assert.AreEqual(c2.Name, c.Name);
            Assert.AreEqual(c2.Address, c.Address);
        }

        [Test]
        // Tests to see if a existing Customer record from the database
        // is able to be deleted and for this to be saved. Where
        // it first receives a existing Customer record, runs it
        // with the Delete method to have it deleted from the database,
        // and then uses the Save method to have this change saved.
        // Where it should throw an exception when it tries to receive
        // the deleted Customer record, since it shouldn't exist anymore.
        public void TestDelete()
        {
            Customer c = new Customer(5);
            c.Delete();
            c.Save();
            Assert.Throws<Exception>(() => new Customer(5));
        }

        [Test]
        // Tests GetList with the Customer, and if it's able to receive
        // all the current Customer records from the database, where it
        // runs GetList to populate a Customer object list with all the
        // current Customer records from the database. Using AreEqual
        // to check if it has the right amount of records that should be
        // in the database, then two more times to check two of the fields
        // of the first Customer record in the list to see if the data was
        // was correctly received from the database.
        public void TestGetList()
        {
            Customer c = new Customer();
            List<Customer> customers = (List<Customer>)c.GetList();
            Assert.AreEqual(696, customers.Count);
            // There ordered by Name by the procedure.
            Assert.AreEqual("Abeyatunge, Derek", customers[0].Name);
            Assert.AreEqual("1414 S. Dairy Ashford", customers[0].Address);
        }

        [Test]
        // Test used to check if it throws an exception if
        // it doesn't have all the required fields set when it
        // tries to save a Customer record to the database.
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            Customer c = new Customer();
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        // Tests to see if it will still throw an exaction
        // when trying to save a Customer record to the database
        // when only some of the required fields are set.
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            Customer c = new Customer();
            Assert.Throws<Exception>(() => c.Save());
            c.Name = "Garber Newman";
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        // This test is used to check if it throws an
        // ArgumentOutOfRangeException when it tries to
        // set a field to an invalid value given it's
        // validation.
        public void TestInvalidPropertySet()
        {
            Customer c = new Customer();
            Assert.Throws<ArgumentOutOfRangeException>(() => c.Name = "abcdefghijklmnopqrstuoijshfghoiuhasdifhiasdfifsagifhghsidhfhksdfhkjhkljhlkjhlkjhlkjhlkjhlkjhlkjhhhhhe");
        }

        [Test]
        // This tests to see if a concurrency issue will occur
        // when another instance of the same Customer record
        // tries to save, where with this it makes two
        // instances of the Customer record. Changes the
        // the first instance Name field to something different
        // then saves this change, then it does the same with the
        // second instance except when it goes to save this change
        // it should throw an exception since it's the same
        // Customer record tried to be saved.
        public void TestConcurrencyIssue()
        {
            Customer c1 = new Customer(1);
            Customer c2 = new Customer(1);

            c1.Name = "Hector Femur";
            c1.Save();

            c2.Name = "Susie Lovely";
            Assert.Throws<Exception>(() => c2.Save());
        }
    }
}