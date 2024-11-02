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
    public class CustomerDBTests
    {
        // Creates a public CustomerDB class that will be used
        // in all the tests to be able to use and test the
        // CustomerDB methods.
        CustomerDB db;
        [SetUp]
        // This will run before each test to clear the the CustomerDB
        // object and to run the stored procedure usp_testingResetData
        // to restart the tables that theses tests are going to be
        // interacting with back to how they were before running a
        // test.
        public void ResetData()
        {
            db = new CustomerDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        // This is used to test the Retrieve method from the
        // CustomerDB class. Where it retrieves a record from
        // the data, and they using AreEqual to see if the
        // fields from the collected record match to what we
        // expect them to be.
        public void TestRetrieve()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(1);
            Assert.AreEqual(1, p.CustomerID);
            Assert.AreEqual("Molunguri, A", p.Name);
        }

        [Test]
        // This is used to test the RetrieveAll method from
        // the CustomerDB class, where it runs the RetrieveAll
        // method to set the list to all the current records
        // in the Customer table in the database. Using
        // AreEqual to see if it was able to grab all the
        // current record by seeing if the grabbed amount
        // matches the number of records there are
        // supposed to be.
        public void TestRetrieveAll()
        {
            List<CustomerProps> list = (List<CustomerProps>)db.RetrieveAll();
            Assert.AreEqual(696, list.Count);
        }

        [Test]
        // This is used to test the Delete method from
        // the CustomerDB class. Where it first uses the Retrieve
        // method to grab a already existing record, then using
        // the Delete method, then using Retrieve method again on the
        // same record checking to see if it throws an error
        // since the record should not exist anymore..
        public void TestDelete()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(1);
            Assert.True(db.Delete(p));
            Assert.Throws<Exception>(() => db.Retrieve(1));
        }

        [Test]
        // This checks to see if the Delete method will throw
        // an error when it tries to delate a record that has a
        // Foreign key with another table.
        public void TestDeleteForeignKeyConstraint()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(20);
            Assert.Throws<MySqlException>(() => db.Delete(p));
        }

        [Test]
        // This method is used to test the Update method from
        // the CustomerDB class, where it first uses the Retrieve
        // method to grab a already made object, they sets
        // the Name field to something new, running the
        // Update method with the edited record to update the
        // record in the database. Running the Retrieve method
        // again with the same record, using AreEqual to see if
        // it's Name field was updated to what we wanted it to.
        public void TestUpdate()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(1);
            p.Name = "Rudy Red";
            Assert.True(db.Update(p));
            p = (CustomerProps)db.Retrieve(1);
            Assert.AreEqual("Rudy Red", p.Name);
        }

        [Test]
        // This method is used to test the Create method from
        // the CustomerDB class, where we populate the fields
        // of a CustomerProps object which then we run it though
        // the Create method to have it added to the Customer table
        // of the database. Where using the Receive method again we
        // grab the recently created record, using AreEqual to see
        // if the one we made matches the one we just added.
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
