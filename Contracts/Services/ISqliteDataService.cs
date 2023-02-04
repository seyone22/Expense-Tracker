using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

using Expense_Tracker_v1._0.Core.Models;

namespace Expense_Tracker_v1._0.Contracts.Services;
internal interface ISqliteDataService
{
    Task<SqliteConnection> InitializeDatabaseAsync();
    Task createTablesAsync(SqliteConnection db);
}