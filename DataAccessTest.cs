using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;
using Microsoft.Data.Sqlite;

namespace Expense_Tracker_v1._0;
public class DataAccessTest
{
    public static async void InitializeDatabase()
    {
        //does not work in unpackaged apps.
        //DECIDE WHETHER TO USE PACKAGED ON UNPACKAGED BEFORE PROCEEDING TO AVOID PAIN
        await ApplicationData.Current.LocalFolder.CreateFileAsync("balancesheet.db", CreationCollisionOption.OpenIfExists);

        string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "balancesheet.db");
        using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
        {
            db.Open();

            createTables(db);
        }
    }

    private static void createTables(SqliteConnection db)
    {
        string createAccountTable = "CREATE TABLE IF NOT " +
        "EXISTS Accounts (acc_id INTEGER PRIMARY KEY, " +
        "account_name NVARCHAR(2048) NULL," +
        "balance DOUBLE NULL)";

        string createTransactionTable = "CREATE TABLE IF NOT " +
            "EXISTS Transactions (tx_id INTEGER PRIMARY KEY, " +
            "from_acc NVARCHAR(2048) NULL," +
            "to_payee NVARCHAR(2048) NULL," +
            "tx_value DOUBLE NULL)";

        string createPayeeTable = "CREATE TABLE IF NOT " +
            "EXISTS Payees (pay_id INTEGER PRIMARY KEY," +
            "payee_name NVARCHAR(2048) NULL)";

        string createMetadataTable = "CREATE TABLE IF NOT " +
            "EXISTS Metadata (rowid INTEGER PRIMARY KEY, " +
            "author NVARCHAR(2048) NULL," +
            "date_created INTEGER(2048) NULL," +
            "last_modified INTEGER(2048) NULL)";


        SqliteCommand createAccounts = new SqliteCommand(createAccountTable, db);
        SqliteCommand createTransactions = new SqliteCommand(createTransactionTable, db);
        SqliteCommand createPayees = new SqliteCommand(createPayeeTable, db);
        SqliteCommand createMetadata = new SqliteCommand(createMetadataTable, db);

        createAccounts.ExecuteReader();
        createTransactions.ExecuteReader();
        createPayees.ExecuteReader();
        createMetadata.ExecuteReader();
    }

    public static void AddData(string inputText)
    {
        string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
        using (SqliteConnection db =
          new SqliteConnection($"Filename={dbpath}"))
        {
            db.Open();

            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = db;

            // Use parameterized query to prevent SQL injection attacks
            insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @Entry);";
            insertCommand.Parameters.AddWithValue("@Entry", inputText);

            insertCommand.ExecuteReader();
        }

    }

    public static List<String> GetData()
    {
        List<String> entries = new List<string>();

        string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
        using (SqliteConnection db =
           new SqliteConnection($"Filename={dbpath}"))
        {
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand
                ("SELECT Text_Entry from MyTable", db);

            SqliteDataReader query = selectCommand.ExecuteReader();

            while (query.Read())
            {
                entries.Add(query.GetString(0));
            }
        }

        return entries;
    }
}