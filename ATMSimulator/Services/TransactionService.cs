using ATMSimulator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using ATMSimulator.Entities;

namespace ATMSimulator.Services
{
    public class TransactionService : ITransactionService
    {
        private const decimal transactionFees = 10;
        private readonly Account _account;

        public TransactionService(Account account)
        {
            _account = account;
        }


        public decimal TranserFunds(decimal fromAccountBalance, decimal amount, decimal toAccountBalance)
        {
            if (fromAccountBalance <= amount) return 0;

            fromAccountBalance -= amount;
            return toAccountBalance + amount;
        }

        public Account WithdrawFunds(Account transactionAccount, decimal amount)
        {
            if (transactionAccount.AccountType == (int) AccountTypes.Savings)
            {
                transactionAccount.Balance -= (amount + transactionFees);
            }
            if (transactionAccount.AccountType == (int) AccountTypes.Cheque)
            {
                if (amount > transactionAccount.Balance)
                {
                    var newAmount = transactionAccount.Balance - amount;
                    if (newAmount > transactionAccount.NegativeBalance)
                    {
                        Console.WriteLine("Insufficient fund to withdraw.");
                        return null;
                    }
                    var @decimal = (newAmount < 0 && transactionAccount.Balance > 0) ? transactionAccount.NegativeBalance += newAmount : transactionAccount.NegativeBalance -= amount;


                    transactionAccount.Balance = transactionAccount.NegativeBalance - 500;
                }
                else
                {
                    transactionAccount.Balance -= amount;
                }
            }
            return transactionAccount;
        }

        public Account DepositFunds(Account transactionAccount, decimal amount)
        {
            try
            {
                transactionAccount.Balance += amount;                             
            }
            catch (Exception e)
            {
                Console.WriteLine("Transaction Incomplete. Please try later. \n" + e.Message);
            }
            return transactionAccount;
        }

        public decimal CheckBalance(string fromAccount)
        {
            throw new NotImplementedException();
        }

        public bool CheckIfValidTransaction(Account validateAccount, decimal amount, int action)
        {
            var validTransaction = false;
            var minimumBalance = action == (int)AccountActions.Withdraw ? 1 : 0;

            if (validateAccount.AccountType == (int)AccountTypes.Deposit && minimumBalance == 1)
            {
                Console.WriteLine("\n Sorry you can not withdraw from Deposit account.");
            }
            else if ((validateAccount.AccountType == (int)AccountTypes.Savings) && ((validateAccount.Balance - amount) >= minimumBalance))
            {
                validTransaction = true;
            }
            else if (validateAccount.AccountType == (int)AccountTypes.Cheque)
            {
                validTransaction = true;
                if (amount > validateAccount.Balance)
                {
                    var newBalance = amount - validateAccount.Balance;
                    if (newBalance > validateAccount.NegativeBalance)
                    {
                        Console.WriteLine("\n Sorry this Transaction may exceed the Minimum Balance for your account.");
                        validTransaction = false;
                    }
                }
            }
            else
            {
                Console.WriteLine("\n Sorry this Transaction may exceed the Minimum Balance for your account.");
            }
            return validTransaction;
        }

        public decimal ReadAndValidateInputAmount()
        {
            Console.Write("\n Please Enter an amount for transaction: ");
            var validAmount = Console.ReadLine();

            if (validAmount== null)
            {
                Console.Write("\n Please Enter Valid Amount: ");
                return 0;
            }

            return Convert.ToDecimal(validAmount);
        }

    }
}
