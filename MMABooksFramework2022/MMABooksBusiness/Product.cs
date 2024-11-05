using System;

using MMABooksTools;
using MMABooksProps;
using MMABooksDB;

using System.Collections.Generic;

namespace MMABooksBusiness
{
    // Creates a Product class that inherits the
    // fields, methods, and properties form the
    // BaseBusiness class. An abstract class that isn't
    // used to make an object but to consist of abstract
    // methods. With BaseBusiness providing functionality
    // for business classes, such as handling properties,
    // database connections, and business rules.
    public class Product : BaseBusiness
    {
        // Getter field of the ProductID field
        // of the ProductProps object.
        public int ProductID
        {
            get
            {
                // Where it receive data from a ProductProps object
                // where it is a data model holding the actual
                // values. With this being used to access and return
                // the specific field listed, which for this one is
                // ProductID.
                return ((ProductProps)mProps).ProductID;
            }
        }

        // Getter field of the ProductCode field
        // of the ProductProps object.
        public String ProductCode
        {
            get
            {
                return ((ProductProps)mProps).ProductCode;
            }

            set
            {
                // Checks to see if there isn't already a value
                // set to this field. Where if there isn't
                // then it will set the new value being inputted
                // to this field.
                if (!(value == ((ProductProps)mProps).ProductCode))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 10)
                    {
                        // Then will it set the mRules.RuleBroken to false
                        // saying that the value entered is valid, so that
                        // it's able to set this value to a specific field
                        // of this object. 
                        mRules.RuleBroken("ProductCode", false);
                        ((ProductProps)mProps).ProductCode = value;
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
                        // the values to be set to the ProductProps object.
                        throw new ArgumentOutOfRangeException("ProductCode must be no more than 10 characters long.");
                    }
                }
            }
        }

        // Getter field of the Description field
        // of the ProductProps object.
        public String Description
        {
            get
            {
                return ((ProductProps)mProps).Description;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).Description))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 50)
                    {
                        mRules.RuleBroken("Description", false);
                        ((ProductProps)mProps).Description = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("Description must be no more than 50 characters long.");
                    }
                }
            }
        }

        // Getter field of the UnitPrice field
        // of the ProductProps object.
        public decimal UnitPrice
        {
            get
            {
                return ((ProductProps)mProps).UnitPrice;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).UnitPrice))
                {
                    if (value > 0m && value <= 99999999.9999m)
                    {
                        mRules.RuleBroken("UnitPrice", false);
                        ((ProductProps)mProps).UnitPrice = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("UnitPrice must be no more than 99999999.9999m characters long.");
                    }
                }
            }
        }

        // Getter field of the OnHandQuantity field
        // of the ProductProps object.
        public int OnHandQuantity
        {
            get
            {
                return ((ProductProps)mProps).OnHandQuantity;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).OnHandQuantity))
                {
                    if (value >= 0)
                    {
                        mRules.RuleBroken("OnHandQuantity", false);
                        ((ProductProps)mProps).OnHandQuantity = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("OnHandQuantity must be a positive int.");
                    }
                }
            }
        }

        // The GetList method is used to get a list of Product Objects
        // using the RetrieveAll method to get a list of the current
        // Product records, turn them into ProductProps objects, add
        // them to ProductProps list, which is then ran though a foreach
        // loop that goes though each ProductProps object and sets it to a
        // Product object that is then added to Product object list that
        // is then returned to where this method is called.
        public override object GetList()
        {
            List<Product> products = new List<Product>();
            List<ProductProps> props = new List<ProductProps>();

            props = (List<ProductProps>)mdbReadable.RetrieveAll();
            foreach (ProductProps prop in props)
            {
                Product c = new Product(prop);
                products.Add(c);
            }
            return products;
        }

        protected override void SetDefaultProperties()
        {
            // Not needed since ProductProps already sets
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
            mRules.RuleBroken("ProductCode", true);
            mRules.RuleBroken("Description", true);
            mRules.RuleBroken("UnitPrice", true);
            mRules.RuleBroken("OnHandQuantity", true);
        }

        // The SetUp method is used to initialize essential
        // fields and properties for the Product class to
        // handle data and to interact with a database.
        protected override void SetUp()
        {
            // With this creating a instance of the ProductProps
            // assigned to mProps where the current values
            // of the product’s data are held. Being used to
            // interact with and manage data related to an
            // individual Product record.
            mProps = new ProductProps();
            // With this one is a separate instance of ProductProps
            // assigned to mOldProps where it's used to store the
            // original data values of a Product record before any changes
            // where made. Used to revert changes and tracking before
            // the record is saved to the database.
            mOldProps = new ProductProps();

            // This creates an instance of the ProductDB assigned
            // to mdbReadable, so the Product object can use the Retrieve
            // and ReceiveAll method to be able fetch product
            // data from the database.
            mdbReadable = new ProductDB();
            // This creates another instance of ProductDB but
            // now assigned to mdbWriteable, so the Product object
            // can use the Create, Update, and Delete method
            // to be able to interact with database.
            mdbWriteable = new ProductDB();
        }

        #region constructors
        /// <summary>
        /// Default constructor - gets the connection string - assumes a new record that is not in the database.
        /// </summary>
        public Product() : base()
        {
        }

        /// <summary>
        /// Calls methods SetUp() and Load().
        /// Use this constructor when the object is in the database AND the connection string is in a config file
        /// </summary>
        /// <param name="key">ID number of a record in the database.
        /// Sent as an arg to Load() to set values of record to properties of an 
        /// object.</param>
        public Product(int key)
            : base(key)
        {
        }

        private Product(ProductProps props)
            : base(props)
        {
        }

        #endregion
    }
}
