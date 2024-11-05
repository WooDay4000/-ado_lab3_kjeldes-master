using NUnit.Framework;

using MMABooksProps;
using System;

namespace MMABooksTests
{
    [TestFixture]
    public class CustomerPropsTests
    {
        // A public CustomerProps object that will be used
        // in most of the CustomerProps tests.
        CustomerProps props;
        [SetUp]
        // This runs before each test and sets all the fields
        // in the public CustomerProps props object to the one specified
        // in the SetUp method.
        public void Setup()
        {
            props = new CustomerProps();
            props.CustomerID = 1;
            props.Name = "Ryan Andrew";
            props.Address = "123 Main Street";
            props.City = "Orlando";
            props.ZipCode = "10001";
        }

        [Test]
        // This is used to the test the GetState method
        // from the CustomerProps class. By checking the
        // JSON-formatted string for each field that was 
        // int the serialized object, where we check if it
        // it contains all the correct fields by using
        // Contains and the field values we are looking
        // for.
        public void TestGetState()
        {
            string jsonString = props.GetState();
            Console.WriteLine(jsonString);
            Assert.IsTrue(jsonString.Contains(props.CustomerID.ToString()));
            Assert.IsTrue(jsonString.Contains(props.Name));
            Assert.IsTrue(jsonString.Contains(props.Address));
            Assert.IsTrue(jsonString.Contains(props.City));
            Assert.IsTrue(jsonString.Contains(props.State));
            Assert.IsTrue(jsonString.Contains(props.ZipCode));
        }

        [Test]
        // This is used to the test the SetState method
        // from the CustomerProps class. Where it checks
        // to see if it's able to covert the JSON-formatted
        // string back into a CustomerProps object. Using
        // AreEqual tests to see if the fields of the
        // CustomerProps object are what they are
        // supposed to be given the JSON-formatted string.
        public void TestSetState()
        {
            string jsonString = props.GetState();
            CustomerProps newProps = new CustomerProps();
            newProps.SetState(jsonString);
            Assert.AreEqual(props.CustomerID, newProps.CustomerID);
            Assert.AreEqual(props.Name, newProps.Name);
            Assert.AreEqual(props.Address, newProps.Address);
            Assert.AreEqual(props.City, newProps.City);
            Assert.AreEqual(props.State, newProps.State);
            Assert.AreEqual(props.ZipCode, newProps.ZipCode);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }

        [Test]
        // This is used to test the Clone method from the 
        // CustomerProps class, to see if it's able to
        // copy field of one CustomerProps object to
        // another CustomerProps object. Using AreEqual
        // tests to see if the field values with correctly
        // copied over.
        public void TestClone()
        {
            CustomerProps newProps = (CustomerProps)props.Clone();
            Assert.AreEqual(props.CustomerID, newProps.CustomerID);
            Assert.AreEqual(props.Name, newProps.Name);
            Assert.AreEqual(props.Address, newProps.Address);
            Assert.AreEqual(props.City, newProps.City);
            Assert.AreEqual(props.State, newProps.State);
            Assert.AreEqual(props.ZipCode, newProps.ZipCode);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }
    }
}