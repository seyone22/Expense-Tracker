using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expense_Tracker_v1._0.Core.Contracts.Services;

using Expense_Tracker_v1._0.Core.Models;

namespace Expense_Tracker_v1._0.Services;
public class TransactionDataService : ITransactionDataService
{
    private List<Transaction>? _allTransactions; //question mark denotes nullable

    private static IEnumerable<Transaction> AllTransactions()
    {
        return SqliteDataService.GetTransactions();
    }
    public static Transaction createTransaction(string fromAccount, string toPayee, DateTime date, double value)
    {
        return new Transaction(fromAccount, toPayee, date, value);
    }

    public async Task<IEnumerable<Transaction>> GetGridDataAsync()
    {
        _allTransactions = new List<Transaction>(AllTransactions());

        await Task.CompletedTask;
        return _allTransactions;
    }

    public async Task<double> GetPoolTotal()
    {
        double x = SqliteDataService.CalculatePoolTotal();
        await Task.CompletedTask;
        return x;
    }
}
