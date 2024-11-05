using System;
using System.Collections.Generic;
using System.Text;

using MMABooksTools;
using MMABooksProps;
using MMABooksDB;
using System.Data;

namespace MMABooksBusiness
{
    // Creates a Customer class that inherits the
    // fields, methods, and properties form the
    // BaseBusiness class. An abstract class that isn't
    // used to make an object but to consist of abstract
    // methods. With BaseBusiness providing functionality
    // for business classes, such as handling properties,
    // database connections, and business rules.
    public class Customer : BaseBusiness
    {
        // Getter field of the CustomerID field
        // of the CustomerProps object.
        public int CustomerID
        {
            get
            {
                // Where it receive data from a CustomerProps object
                // where it is a data model holding the actual
                // values. With this being used to access and return
                // the specific field listed, which for this one is
                // customerID.
                return ((CustomerProps)mProps).CustomerID;
            }
        }

        // Getter and setter field of the Name field
        // of the CustomerProps object.
        public String Name
        {
            get
            {
                return ((CustomerProps)mProps).Name;
            }

            set
            {
                // Checks to see if there isn't already a value
                // set to this field. Where if there isn't
                // then it will set the new value being inputted
                // to this field.
                if (!(value == ((CustomerProps)mProps).Name))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 100)
                    {
                        // Then will it set the mRules.RuleBroken to false
                        // saying that the value entered is valid, so that
                        // it's able to set this value to a specific field
                        // of this object.  
                        mRules.RuleBroken("Name", false);
                        ((CustomerProps)mProps).Name = value;
                        // Then setting mIsDirty to false to track that this
                        // object was modified from it's last saved state.
                        mIsDirty = true;
                    }

                    else
                    {
                        // Where if the value entered isn't in the range we
                        // want, specified in the if part of if else statement,
                        // then then it will throw an ArgumentOutOfRangeException
                        // with a massage about what is allowed or needed for
                        // the values to be set to the CustomerProps object.
                        throw new ArgumentOutOfRangeException("Name must be no more than 100 characters long.");
                    }
                }
            }
        }

        // Getter and setter field of the Address field
        // of the CustomerProps object.
        public String Address
        {
            get
            {
                return ((CustomerProps)mProps).Address;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).Address))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 50)
                    {
                        mRules.RuleBroken("Address", false);
                        ((CustomerProps)mProps).Address = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("Address must be no more than 50 characters long.");
                    }
                }
            }
        }

        // Getter and setter field of the City field
        // of the CustomerProps object.
        public String City
        {
            get
            {
                return ((CustomerProps)mProps).City;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).City))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 20)
                    {
                        mRules.RuleBroken("City", false);
                        ((CustomerProps)mProps).City = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("City must be no more than 20 characters long.");
                    }
                }
            }
        }

        // Getter and setter field of the State field
        // of the CustomerProps object.
        public String State
        {
            get
            {
                return ((CustomerProps)mProps).State;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).State))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 2)
                    {
                        mRules.RuleBroken("State", false);
                        ((CustomerProps)mProps).State = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("State must be no more than 2 characters long.");
                    }
                }
            }
        }

        // Getter and setter field of the ZipCode field
        // of the CustomerProps object.
        public String ZipCode
        {
            get
            {
                return ((CustomerProps)mProps).ZipCode;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).ZipCode))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 15)
                    {
                        mRules.RuleBroken("ZipCode", false);
                        ((CustomerProps)mProps).ZipCode = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("ZipCode must be no more than 15 characters long.");
                    }
                }
            }
        }

        // The GetList method is used to get a list of Customer Objects
        // using the RetrieveAll method to get a list of the current
        // customer records, turn them into CustomerProps objects, add
        // them to CustomerProps list, which is then ran though a foreach
        // loop that goes though each CustomerProps object and sets it to a
        // Customer object that is then added to Customer object list that
        // is then returned to where this method is called.
        public override object GetList()
        {
            List<Customer> customers = new List<Customer>();
            List<CustomerProps> props = new List<CustomerProps>();


            props = (List<CustomerProps>)mdbReadable.RetrieveAll();
            foreach (CustomerProps prop in props)
            {
                Customer c = new Customer(prop);
                customers.Add(c);
            }

            return customers;
        }

        protected override void SetDefaultProperties()
        {
            // Not needed since CustomerProps already sets
            // them for the constructors
        }

        // The SetRequiredRules method defines business rules
        // for required fields, marking each one as initially
        // "broken" or invalid using mRules.RuleBroken with
        // the field name and a true value. This indicates
        // that these fields are required but currently lack
        // valid data. When a valid value is assigned to one
        // of these fields, RuleBroken is updated to false,
        // meaning the field now satisfies the rule and the
        // object can be considered valid for saving if all
        // the other RuleBroken are false as well.
        protected override void SetRequiredRules()
        {
            mRules.RuleBroken("Name", true);
            mRules.RuleBroken("Address", true);
            mRules.RuleBroken("City", true);
            mRules.RuleBroken("State", true);
            mRules.RuleBroken("ZipCode", true);
        }

        // The SetUp method is used to initialize essential
        // fields and properties for the Customer class to
        // handle data and to interact with a database.
        protected override void SetUp()
        {
            // With this creating a instance of the CustomerProps
            // assigned to mProps where the current values
            // of the customer’s data are held. Being used to
            // interact with and manage data related to an
            // individual Customer record.
            mProps = new CustomerProps();
            // With this one is a separate instance of CustomerProps
            // assigned to mOldProps where it's used to store the
            // original data values of a Customer record before any changes
            // where made. Used to revert changes and tracking before
            // the record is saved to the database.
            mOldProps = new CustomerProps();

            // This creates an instance of the CustomerDB assigned
            // to mdbReadable, so the Customer object can use the Retrieve
            // and ReceiveAll method to be able fetch customer
            // data from the database.
            mdbReadable = new CustomerDB();
            // This creates another instance of CustomerDB but
            // now assigned to mdbWriteable, so the Customer object
            // can use the Create, Update, and Delete method
            // to be able to interact with database.
            mdbWriteable = new CustomerDB();
        }
        #region constructors
        /// <summary>
        /// Default constructor - gets the connection string - assumes a new record that is not in the database.
        /// </summary>
        public Customer() : base()
        {
        }

        /// <summary>
        /// Calls methods SetUp() and Load().
        /// Use this constructor when the object is in the database AND the connection string is in a config file
        /// </summary>
        /// <param name="key">ID number of a record in the database.
        /// Sent as an arg to Load() to set values of record to properties of an 
        /// object.</param>
        public Customer(int key)
            : base(key)
        {
        }

        private Customer(CustomerProps props)
            : base(props)
        {
        }

        #endregion
    }
}
