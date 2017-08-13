using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSimulator.Entities;
using ATMSimulator.Interfaces;

namespace ATMSimulator.Repository
{
    public class BankBalanceRepository : IBankBalanceRepository
    {
        public List<Account> LoadBankBalanceToAccounts()
        {
            var accountList = new List<Account>
            {
                new Account() {AccountType = (int) AccountTypes.Savings, Balance = 5000, NegativeBalance = 0},
                new Account() {AccountType = (int) AccountTypes.Cheque, Balance = 3000, NegativeBalance = 500},
                new Account() {AccountType = (int) AccountTypes.Deposit, Balance = 2000, NegativeBalance = 0}
            };
            
            return accountList;
        }
    }
}
