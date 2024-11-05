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
    public class ProductDB : DBBase, IReadDB, IWriteDB
    {
        // A default constructor that calls the base class
        // class constructor using base() to initialize
        // DBBase to have the have ProductDB object be
        // able to have the core functionality of being
        // able to form a connection with a database and
        // be able to perform stored procedures in the
        // database.
        public ProductDB() : base() { }

        // A parameterized constructor with similar function
        // to the default constructor, but now with it taking
        // a DBConnection connection being used in the DBBase
        // to form a connection to a database, and perform
        // procedures that would require that, such as
        // receiving and creating records.
        public ProductDB(DBConnection cn) : base(cn) { }

        // The Create Method is used to create a new customer
        // record into a database such as MySQL, where is calls
        // a stored procedure in the database called usp_ProductCreate
        // where it takes a IBaseProps object takes the fields of it
        // and makes them into a compatible record that is then
        // sent to the Products table in the database with a
        // automatically added ProductID.
        public IBaseProps Create(IBaseProps p)
        {
            int rowsAffected = 0;

            // This casts the IBaseProps as a 
            // ProductProps object, so you able
            // to access the ProductProps specific fields
            // and methods for use in this method.
            ProductProps props = (ProductProps)p;

            // This creates a DBCommand object hold the
            // command that is going to be run alongside
            // the parameters that it will follow.
            DBCommand command = new DBCommand();
            // The stored procedure/command that is going
            // to be runned. It's located in the database
            command.CommandText = "usp_ProductCreate";
            // Where then the command is marked as
            // a stored procedure, so it's interpreted as so.
            // Where it tells the database to check the
            // CommandText, what was wrote a line above this one
            // that we are using the stored procedure listed, which
            // for this is usp_ProductCreate.
            command.CommandType = CommandType.StoredProcedure;
            // Where these are used to specify felids that are
            // going to be passed to the stored procedure,
            // this being what the field is called and the
            // data type that is being used.
            // ----------------------------
            // With the prodId one of this being used as a 
            // output parameter which after the command runs
            // it will be set to the automatically created
            // ProductID that the databased created for the
            // Product record.
            command.Parameters.Add("prodId", DBDbType.Int32);
            command.Parameters.Add("prodCode_p", DBDbType.VarChar);
            command.Parameters.Add("description_p", DBDbType.VarChar);
            command.Parameters.Add("unitPrice_p", DBDbType.Decimal);
            command.Parameters.Add("onHandQuantity_p", DBDbType.Int32);
            //... there are more parameters here

            // With this line being used to specify that the data is 
            // is output only to the database, but will return a value
            // such as the amount of rows in the database being
            // affected.
            command.Parameters[0].Direction = ParameterDirection.Output;
            // With these being used to assign values to the
            // specified fields using the corresponding fields
            // in the ProductProps object to populate the
            // the specific felids that are going to be passed
            // to the stored procedure.
            command.Parameters["prodCode_p"].Value = props.ProductCode;
            command.Parameters["description_p"].Value = props.Description;
            command.Parameters["unitPrice_p"].Value = props.UnitPrice;
            command.Parameters["onHandQuantity_p"].Value = props.OnHandQuantity;
            //... and more values here

            try
            {
                // Where then the created command is ran using
                // RunNonQueryProcedure that will interact and
                // performs it's specified procedure and set the
                // rowsAffected with a int value of 1 if it worked.
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    // This is when the ProductID field of the
                    // ProductProps object is set to the created
                    // ProductID that was made by the database.
                    props.ProductID = (int)command.Parameters[0].Value;
                    props.ConcurrencyID = 1;
                    // Then it returns the ProductProps object with the
                    // the created ProductID and the ConcurrencyID
                    // set to the corresponding felids.
                    return props;
                }
                else
                    // If it failed then it will throw an Exception, with
                    // a message that includes ProductProps object
                    // with it's current fields in string format to
                    // help with debugging.
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

        // The Delete method that is used to delete a Product
        // record from a database where is calls a stored
        // procedure in the database called usp_ProductDelete
        // where if the entered ProductID and ConcurrencyID
        // of a IBaseProps object matches those values in field
        // of a given Product record then it will be delated.
        public bool Delete(IBaseProps p)
        {
            ProductProps props = (ProductProps)p;
            int rowsAffected = 0;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductDelete";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("prodId", DBDbType.Int32);
            command.Parameters.Add("conCurrId", DBDbType.Int32);
            command.Parameters["prodId"].Value = props.ProductID;
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
        // Product record from a database where it calls
        // the stored procedure usp_ProductSelect, where
        // based on the ProductID that is entered it will
        // return that specific Product object that has
        // that ProductID.
        public IBaseProps Retrieve(object key)
        {
            DBDataReader data = null;
            ProductProps props = new ProductProps();
            DBCommand command = new DBCommand();

            command.CommandText = "usp_ProductSelect";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("prodId", DBDbType.Int32);
            command.Parameters["prodId"].Value = (int)key;

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
        // but it will return a list of all the current Product
        // records in the Products Table turned into ProductProps
        // objects and then added to a list which will be returned
        // to where this method was called. This is done by using
        // the stored procedure usp_ProductSelectAll from the
        // database.
        public object RetrieveAll()
        {
            List<ProductProps> list = new List<ProductProps>();
            DBDataReader reader = null;
            ProductProps props;

            try
            {
                reader = RunProcedure("usp_ProductSelectAll");
                if (!reader.IsClosed)
                {
                    while (reader.Read())
                    {
                        props = new ProductProps();
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
        // database, using the stored procedure usp_ProductUpdate
        // where if the fields of the record matches the inputted
        // ProductID and ConcurrencyID, then it will update
        // the name field of the Product record.
        public bool Update(IBaseProps p)
        {
            int rowsAffected = 0;
            ProductProps props = (ProductProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductUpdate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("prodId", DBDbType.VarChar);
            command.Parameters.Add("description", DBDbType.VarChar);
            command.Parameters.Add("conCurrId", DBDbType.Int32);
            command.Parameters["prodId"].Value = props.ProductID;
            command.Parameters["description"].Value = props.Description;
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
