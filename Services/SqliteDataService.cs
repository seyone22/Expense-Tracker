using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expense_Tracker_v1._0.Contracts.Services;
using Expense_Tracker_v1._0.Models;


using Expense_Tracker_v1._0.Core.Models;

using Microsoft.Data.Sqlite;
using System.IO;
using Windows.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Expense_Tracker_v1._0.Core.Services;

namespace Expense_Tracker_v1._0.Services;
internal class SqliteDataService : ISqliteDataService
{
    private const string defaultDBName = "pool.db";
    private static string currentDBName = "";

    public static string GetDBName() => defaultDBName;
    public static string GetCurrentDatabaseName() => currentDBName;

    public async Task<SqliteConnection> InitializeDatabaseAsync(string DBName, bool IsNewFile)
    {
        if(string.IsNullOrEmpty(DBName))
            DBName = defaultDBName;
        else
            currentDBName = DBName;
        //does not work in unpackaged apps.
        //DECIDE WHETHER TO USE PACKAGED ON UNPACKAGED BEFORE PROCEEDING TO AVOID PAIN
        await ApplicationData.Current.LocalFolder.CreateFileAsync(currentDBName, CreationCollisionOption.OpenIfExists);

        var dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, currentDBName);
        using SqliteConnection conn = new SqliteConnection($"Filename={dbpath}");
        conn.Open();
        await createTablesAsync(conn);
        if(!IsNewFile)
            await CreateMetadata();
        return conn;
    }

    public async Task CreateMetadata()
    {
        string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, GetCurrentDatabaseName());
        using (SqliteConnection db =
          new SqliteConnection($"Filename={dbpath}"))
        {
            db.Open();

            SqliteCommand insertCommand = new SqliteCommand
            {
                Connection = db,

                // Use parameterized query to prevent SQL injection attacks
                CommandText = "INSERT INTO Metadata VALUES (NULL, @PoolAuthor, @PoolName, @DateCreated, 0, @LastModified);"
            };

            insertCommand.Parameters.AddWithValue("@PoolAuthor", "SAMPLE AUTHOR");
            insertCommand.Parameters.AddWithValue("@PoolName", FileService.StripExtension(GetCurrentDatabaseName()) );
            insertCommand.Parameters.AddWithValue("@DateCreated", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@LastModified", DateTime.Now);

            await insertCommand.ExecuteReaderAsync();
        }
    }

    //public async Task LoadDatabaseAsync(File dbfile)
    //{
    //    SqliteConnection conn = new SqliteConnection(dbfile.);
    //}

    public static void InitializeDatabase()
    {
        //does not work in unpackaged apps.
        //DECIDE WHETHER TO USE PACKAGED ON UNPACKAGED BEFORE PROCEEDING TO AVOID PAIN
        ApplicationData.Current.LocalFolder.CreateFileAsync(GetCurrentDatabaseName(), CreationCollisionOption.OpenIfExists);

        string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, GetCurrentDatabaseName());
        using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
        {
            conn.Open();
            createTables(conn);
        }
    }

    public async Task createTablesAsync(SqliteConnection db)
    {
        string createAccountTable = "CREATE TABLE IF NOT " +
        "EXISTS Accounts (acc_id INTEGER PRIMARY KEY, " +
        "account_name NVARCHAR(2048) NULL," +
        "balance DOUBLE NULL," +
        "dues NVARCHAR(2048) NULL)";

        string createTransactionTable = "CREATE TABLE IF NOT " +
            "EXISTS Transactions (tx_id INTEGER PRIMARY KEY, " +
            "from_acc NVARCHAR(2048) NULL," +
            "to_payee NVARCHAR(2048) NULL," +
            "date_created NVARCHAR(2048) NULL," +
            "tx_value DOUBLE NULL)";

        string createPayeeTable = "CREATE TABLE IF NOT " +
            "EXISTS Payees (pay_id INTEGER PRIMARY KEY," +
            "payee_name NVARCHAR(2048) NULL)";

        string createMetadataTable = "CREATE TABLE IF NOT " +
            "EXISTS Metadata (rowid INTEGER PRIMARY KEY, " +
            "author NVARCHAR(2048) NULL," +
            "name NVARCHAR(2048) NULL," +
            "date_created NVARCHAR(2048) NULL," +
            "members INTEGER(2048) NULL," +
            "last_modified INTEGER(2048) NULL)";


        SqliteCommand createAccounts = new SqliteCommand(createAccountTable, db);
        SqliteCommand createTransactions = new SqliteCommand(createTransactionTable, db);
        SqliteCommand createPayees = new SqliteCommand(createPayeeTable, db);
        SqliteCommand createMetadata = new SqliteCommand(createMetadataTable, db);

        await createAccounts.ExecuteNonQueryAsync();
        await createTransactions.ExecuteNonQueryAsync();
        await createPayees.ExecuteNonQueryAsync();
        await createMetadata.ExecuteNonQueryAsync();
    }

    public static void createTables(SqliteConnection db)
    {
        string createAccountTable = "CREATE TABLE IF NOT " +
        "EXISTS Accounts (acc_id INTEGER PRIMARY KEY, " +
        "account_name NVARCHAR(2048) NULL," +
        "balance DOUBLE NULL," +
        "dues NVARCHAR(2048) NULL)";

        string createTransactionTable = "CREATE TABLE IF NOT " +
            "EXISTS Transactions (tx_id INTEGER PRIMARY KEY, " +
            "from_acc NVARCHAR(2048) NULL," +
            "to_payee NVARCHAR(2048) NULL," +
            "date_created NVARCHAR(2048) NULL," +
            "tx_value DOUBLE NULL)";

        string createPayeeTable = "CREATE TABLE IF NOT " +
            "EXISTS Payees (pay_id INTEGER PRIMARY KEY," +
            "payee_name NVARCHAR(2048) NULL)";

        string createMetadataTable = "CREATE TABLE IF NOT " +
            "EXISTS Metadata (rowid INTEGER PRIMARY KEY, " +
            "author NVARCHAR(2048) NULL," +
            "name NVARCHAR(2048) NULL," +
            "date_created INTEGER(2048) NULL," +
            "members INTEGER(2048) NULL," +
            "last_modified INTEGER(2048) NULL)";


        SqliteCommand createAccounts = new SqliteCommand(createAccountTable, db);
        SqliteCommand createTransactions = new SqliteCommand(createTransactionTable, db);
        SqliteCommand createPayees = new SqliteCommand(createPayeeTable, db);
        SqliteCommand createMetadata = new SqliteCommand(createMetadataTable, db);

        createAccounts.ExecuteNonQueryAsync();
        createTransactions.ExecuteNonQueryAsync();
        createPayees.ExecuteNonQueryAsync();
        createMetadata.ExecuteNonQueryAsync();
    }

    //TRANSACTION HANDLING
    public static List<Transaction> GetTransactions() //returns a List<Transaction> of all Transactions in the db
    {
        List<Transaction> entries = new List<Transaction>();

        string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, GetCurrentDatabaseName());
        using (SqliteConnection db =
           new SqliteConnection($"Filename={dbpath}"))
        {
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand
                ("SELECT * from Transactions", db);

            SqliteDataReader query = selectCommand.ExecuteReader();

            while (query.Read())
            {
                entries.Add(TransactionDataService.createTransaction(query.GetString(1), query.GetString(2), query.GetDateTime(3), query.GetDouble(4) ));
            }
        }
        return entries;
    }

    //ACCOUNT HANDLING
    public static List<Account> GetAccounts() //returns a List<Account> of all Accounts in the db
    {
        List<Account> entries = new List<Account>();

        string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, GetCurrentDatabaseName());
        using (SqliteConnection db =
           new SqliteConnection($"Filename={dbpath}"))
        {
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand
                ("SELECT * from Accounts", db);

            SqliteDataReader query = selectCommand.ExecuteReader();

            while (query.Read())
            {
                entries.Add(AccountDataService.createAccount(query.GetString(1),Convert.ToDouble(query.GetString(2)), CalculatePoolTotal() / GetCurrentPool().personCount));
            }
        }
        return entries;
    }


    //UNSORTED
    public static void PushTransaction(Transaction tx)
    {
        string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, GetCurrentDatabaseName());
        using (SqliteConnection db =
          new SqliteConnection($"Filename={dbpath}"))
        {
            db.Open();

            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = db;

            // Use parameterized query to prevent SQL injection attacks
            insertCommand.CommandText = "INSERT INTO Transactions VALUES (NULL, @AccountID, @Payee, @Date, @Value);";
            insertCommand.Parameters.AddWithValue("@AccountID", tx.AccountID);
            insertCommand.Parameters.AddWithValue("@Payee", tx.Payee);
            insertCommand.Parameters.AddWithValue("@Date", tx.Date);
            insertCommand.Parameters.AddWithValue("@Value", tx.Value);

            insertCommand.ExecuteReader();
        }

    }

    public static void PushAccount(Account ac)
    {
        string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, GetCurrentDatabaseName());
        using (SqliteConnection db =
          new SqliteConnection($"Filename={dbpath}"))
        {
            db.Open();

            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = db;

            // Use parameterized query to prevent SQL injection attacks
            insertCommand.CommandText = "INSERT INTO Accounts VALUES (NULL, @Name, @Balance, 0);";
            insertCommand.Parameters.AddWithValue("@Name", ac.Name);
            insertCommand.Parameters.AddWithValue("@Balance", ac.Balance);

            insertCommand.ExecuteReader();
        }
    }

    public static List<String> GetTransaction(string x) //returns a List<String> of all Transactions in the db
    {
        List<String> entries = new List<string>();

        string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, GetCurrentDatabaseName());
        using (SqliteConnection db =
           new SqliteConnection($"Filename={dbpath}"))
        {
            db.Open();

            string query = "SELECT * FROM Transactions WHERE tx_id = '" + x + "'";

            SqliteCommand selectCommand = new SqliteCommand
                (query, db);

            SqliteDataReader result = selectCommand.ExecuteReader();

            while (result.Read())
            {
                entries.Add(result.GetString(0));
            }
        }

        return entries;
    }

    public void RefreshTransaations()
    {
        
    }

    public static Pool GetCurrentPool()
    {
        Pool currentPool = new Pool();
        if (defaultDBName != "")
        {

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, GetCurrentDatabaseName());
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                string query = "SELECT * FROM Metadata WHERE rowid = 1";

                SqliteCommand selectCommand = new SqliteCommand
                    (query, db);

                SqliteDataReader result = selectCommand.ExecuteReader();

                while (result.Read())
                {
                    currentPool.author = result.GetString(1);
                    currentPool.name = result.GetString(2);
                    currentPool.dateCreated = Convert.ToDateTime(result.GetString(3));
                    currentPool.personCount = Convert.ToInt32(result.GetString(4));
                    currentPool.LastModified = Convert.ToDateTime(result.GetString(5));
                }    
            }
        }
        return currentPool;
    }

    public static double CalculatePoolTotal()
    {
        double poolTotal = 0;
        if (defaultDBName != "")
        {

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, GetCurrentDatabaseName());
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                string query = "SELECT * FROM Transactions";

                SqliteCommand selectCommand = new SqliteCommand
                    (query, db);

                SqliteDataReader result = selectCommand.ExecuteReader();

                while (result.Read())
                {
                    poolTotal += Convert.ToDouble(result.GetString(4));
                }
            }
        }
        return poolTotal;
    }

    public static int PoolCount()
    {
        Pool pool = new Pool();
        //pool = GetPool();
        return pool.personCount;
    }

    public static void UpdateAccount(string text, double value)
    {
        string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, GetCurrentDatabaseName());
        using (SqliteConnection db =
          new SqliteConnection($"Filename={dbpath}"))
        {
            db.Open();

            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = db;

            // Use parameterized query to prevent SQL injection attacks
            insertCommand.CommandText = "UPDATE Accounts SET balance = (balance + @value) WHERE account_name = @text";

            insertCommand.Parameters.AddWithValue("@value", value);
            insertCommand.Parameters.AddWithValue("@text", text);

            insertCommand.ExecuteReader();
        }
    }

    public static void UpdatePoolAddMember()
    {
        string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, GetCurrentDatabaseName());
        using (SqliteConnection db =
          new SqliteConnection($"Filename={dbpath}"))
        {
            db.Open();

            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = db;

            // Use parameterized query to prevent SQL injection attacks
            insertCommand.CommandText = "UPDATE Metadata SET members = (members + 1) WHERE rowid = 1";
            insertCommand.ExecuteReader();
        }
    }
}
