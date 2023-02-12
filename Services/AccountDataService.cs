using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Expense_Tracker_v1._0.Contracts.Services;

using Expense_Tracker_v1._0.Core.Models;

namespace Expense_Tracker_v1._0.Services;
public class AccountDataService : IAccountDataService
{
    private List<Account>? _allAccounts; //question mark denotes nullable

    private static IEnumerable<Account> AllAccounts()
    {
        return SqliteDataService.GetAccounts();
    }
    public static Account createAccount(string Name, double Balance, double splitVal)
    {
        return new Account(Name, Balance, splitVal);
    }

    public async Task<IEnumerable<Account>> GetGridDataAsync()
    {
        _allAccounts = new List<Account>(AllAccounts());

        await Task.CompletedTask;
        return _allAccounts;
    }
}
