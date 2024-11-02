using System;
using System.Collections.Generic;
using System.Text;

using MMABooksTools;
using MMABooksProps;

using System.Data;

// *** I use an "alias" for the ado.net classes throughout my code
// When I switch to an oracle database, I ONLY have to change the actual classes here
using DBBase = MMABooksTools.BaseSQLDB;
using DBConnection = MySql.Data.MySqlClient.MySqlConnection;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using DBParameter = MySql.Data.MySqlClient.MySqlParameter;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;
using DBDataAdapter = MySql.Data.MySqlClient.MySqlDataAdapter;
using DBDbType = MySql.Data.MySqlClient.MySqlDbType;

namespace MMABooksDB
{
    public class CustomerDB : DBBase, IReadDB, IWriteDB
    {
        // A default constructor that calls the base class
        // class constructor using base() to initialize
        // DBBase to have the have CustomerDB object be
        // able to have the core functionality of being
        // able to form a connection with a database and
        // be able to perform stored procedures in the
        // database.
        public CustomerDB() : base() { }

        // A parameterized constructor with similar function
        // to the default constructor, but now with it taking
        // a DBConnection connection being used in the DBBase
        // to form a connection to a database, and perform
        // procedures that would require that, such as
        // receiving and creating records.
        public CustomerDB(DBConnection cn) : base(cn) { }

        /*
        public IBaseProps Create(IBaseProps p)
        {
            int rowsAffected = 0;
            CustomerProps props = (CustomerProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_CustomerCreate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("custId", DBDbType.Int32);
            command.Parameters.Add("name_p", DBDbType.VarChar);
            ... there are more parameters here
            command.Parameters[0].Direction = ParameterDirection.Output;
            command.Parameters["name_p"].Value = props.Name;
            ... and more values here

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.CustomerID = (int)command.Parameters[0].Value;
                    props.ConcurrencyID = 1;
                    return props;
                }
                else
                    throw new Exception("Unable to insert record. " + props.ToString());
            }
            catch (Exception e)
            {
                // log this error
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }
         */

        // The Create Method is used to create a new customer
        // record into a database such as MySQL, where is calls
        // a stored procedure in the database called usp_CustomerCreate
        // where it takes a IBaseProps object takes the fields of it
        // and makes them into a compatible record that is then
        // sent to the Customers table in the database with a
        // automatically added CustomerID.
        public IBaseProps Create(IBaseProps p)
        {
            int rowsAffected = 0;

            // This casts the IBaseProps as a 
            // CustomerProps object, so you able
            // to access the CustomerProps specific fields
            // and methods for use in this method.
            CustomerProps props = (CustomerProps)p;

            // This creates a DBCommand object hold the
            // command that is going to be run alongside
            // the parameters that it will follow.
            DBCommand command = new DBCommand();
            // The stored procedure/command that is going
            // to be runned. It's located in the database
            command.CommandText = "usp_CustomerCreate";
            // Where then the command is marked as
            // a stored procedure, so it's interpreted as so.
            // Where it tells the database to check the
            // CommandText, what was wrote a line above this one
            // that we are using the stored procedure listed, which
            // for this is usp_CustomerCreate.
            command.CommandType = CommandType.StoredProcedure;
            // Where these are used to specify felids that are
            // going to be passed to the stored procedure,
            // this being what the field is called and the
            // data type that is being used.
            // ----------------------------
            // With the custId one of this being an 
            // output parameter which after the command runs
            // it will be set to the a automatically created
            // CustomerID that the databased created for the
            // Customer record.
            command.Parameters.Add("custId", DBDbType.Int32);
            command.Parameters.Add("Name_p", DBDbType.VarChar);
            command.Parameters.Add("Address_p", DBDbType.VarChar);
            command.Parameters.Add("City_p", DBDbType.VarChar);
            command.Parameters.Add("State_p", DBDbType.VarChar);
            command.Parameters.Add("ZipCode_p", DBDbType.VarChar);
            //... there are more parameters here

            // With this line being used to specify that the data is 
            // is output only to the database, but will return a value
            // such as the amount of rows in the database being
            // affected.
            command.Parameters[0].Direction = ParameterDirection.Output;
            // With these being used to assign values to the
            // specified fields using the corresponding fields
            // in the CustomerProps object to populate the
            // the specific felids that are going to be passed
            // to the stored procedure.
            command.Parameters["Name_p"].Value = props.Name;
            command.Parameters["Address_p"].Value = props.Address;
            command.Parameters["City_p"].Value = props.City;
            command.Parameters["State_p"].Value = props.State;
            command.Parameters["ZipCode_p"].Value = props.ZipCode;
            //... and more values here

            try
            {
                // Where then the created command is ran using
                // RunNonQueryProcedure that will interact and
                // performs it's specified procedure and set the
                // rowsAffected with a int value if it worked.
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    // This is when the CustomerID field of the
                    // CustomerProps object is set to the created
                    // CustomerID that was made by the database.
                    props.CustomerID = (int)command.Parameters[0].Value;
                    props.ConcurrencyID = 1;
                    // This it returns the CustomerProps with the
                    // the created CustomerID and the ConcurrencyID
                    // set to the corresponding felids.
                    return props;
                }
                else
                    // If it failed then it will throw an Exception.
                    throw new Exception("Unable to insert record. " + props.ToString());
            }
            catch (Exception e)
            {
                // log this error
                throw;
            }
            finally
            {
                // Then it closes the connection if it's
                // still open.
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }

        // The Delete method that is used to delete a Customer
        // record from a database where is calls a stored
        // procedure in the database called usp_CustomerDelete
        // where if the entered CustomerID and ConcurrencyID
        // of a IBaseProps object matches those value in field
        // of a given Customer record then it will be delated.
        public bool Delete(IBaseProps p)
        {
            CustomerProps props = (CustomerProps)p;
            int rowsAffected = 0;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_CustomerDelete";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("custId", DBDbType.Int32);
            command.Parameters.Add("conCurrId", DBDbType.Int32);
            command.Parameters["custId"].Value = props.CustomerID;
            command.Parameters["conCurrId"].Value = props.ConcurrencyID;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    return true;
                }
                else
                {
                    string message = "Record cannot be deleted. It has been edited by another user.";
                    throw new Exception(message);
                }

            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }

        // The Retrieve method is used to retrieve a specific
        // customer record from a database where it calls
        // the stored procedure usp_CustomerSelect, where
        // based on the CustomerID that is entered it will
        // return that specific Customer object that has
        // that CustomerID.
        public IBaseProps Retrieve(object key)
        {
            DBDataReader data = null;
            CustomerProps props = new CustomerProps();
            DBCommand command = new DBCommand();

            command.CommandText = "usp_CustomerSelect";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("custID", DBDbType.Int32);
            command.Parameters["custID"].Value = (int)key;

            try
            {
                data = RunProcedure(command);
                if (!data.IsClosed)
                {
                    if (data.Read())
                    {
                        props.SetState(data);
                    }
                    else
                        throw new Exception("Record does not exist in the database.");
                }
                return props;
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (data != null)
                {
                    if (!data.IsClosed)
                        data.Close();
                }
            }
        }

        // The RetrieveAll method is like the Retrieve method
        // but it will return a list of all the current Customer
        // records in the Customers Table turned into CustomerProps
        // objects and then added to a list which will be returned
        // to where this method was called. This is done by using
        // the stored procedure usp_CustomerSelectAll from the
        // database.
        public object RetrieveAll()
        {
            List<CustomerProps> list = new List<CustomerProps>();
            DBDataReader reader = null;
            CustomerProps props;

            try
            {
                reader = RunProcedure("usp_CustomerSelectAll");
                if (!reader.IsClosed)
                {
                    while (reader.Read())
                    {
                        props = new CustomerProps();
                        props.SetState(reader);
                        list.Add(props);
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
        }

        // The Update method is used to update the fields of
        // a Customer record in the Customers Table of the
        // database, using the stored procedure usp_CustomerUpdate
        // where if the fields of record matches the inputted
        // CustomerID and ConcurrencyID, then it will update
        // the name of the Customer record.
        public bool Update(IBaseProps p)
        {
            int rowsAffected = 0;
            CustomerProps props = (CustomerProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_CustomerUpdate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("custID", DBDbType.VarChar);
            command.Parameters.Add("name", DBDbType.VarChar);
            command.Parameters.Add("conCurrId", DBDbType.Int32);
            command.Parameters["custID"].Value = props.CustomerID;
            command.Parameters["name"].Value = props.Name;
            command.Parameters["conCurrId"].Value = props.ConcurrencyID;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.ConcurrencyID++;
                    return true;
                }
                else
                {
                    string message = "Record cannot be updated. It has been edited by another user.";
                    throw new Exception(message);
                }
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }
    }
}