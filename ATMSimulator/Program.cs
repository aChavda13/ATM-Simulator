using System;
using System.Collections.Generic;
using ATMSimulator.Entities;
using ATMSimulator.Interfaces;
using ATMSimulator.Repository;
using Ninject;

namespace ATMSimulator
{
    class Program
    {
        private static IDisplayService _displayService;
        private static ITransactionService _transactionService;
        private static IBankBalanceRepository _bankBalanceRepository;
        private const int MainMenuChoices = 4; //Maximum allowed number in MainMenu
        private const int AccountMenuChoices = 3; //Maximum allowed number in AccountMenu

        private static List<Account> _accountsBalanceList = new List<Account>();
        static void Main(string[] args)
        {
            int menuChoice;
            RegisterServices();
            _accountsBalanceList = _bankBalanceRepository.LoadBankBalanceToAccounts();

            do
            {
                _displayService.DisplayMenu(0); // 0 is for Main menu

                menuChoice = _displayService.ReadAndValidateMenuOptions(MainMenuChoices);
                if (menuChoice != 0) // 0 is for Cancel
                {
                    PlayTransaction(menuChoice);
                }
            } while (menuChoice != 0);
            _displayService.Exit();
        }

        private static void PlayTransaction(int menuChoice)
        {
            var askToRepeat = 1;
            do
            {
                _displayService.PrintAtmWelcomeMessage();
                _displayService.DisplayMenu(menuChoice);
                var chosenAccount = _displayService.ReadAndValidateMenuOptions(AccountMenuChoices);
                if (chosenAccount < 1)
                {
                    _displayService.Exit();
                    break;
                }
                switch (menuChoice)
                {
                    case (int)AccountActions.BalanceCheck:
                        _displayService.PrintAtmWelcomeMessage();
                        DisplayBalance(chosenAccount);

                        Console.Write("\nPerform another Balance Check? (Y/N): ");
                        break;

                    case (int)AccountActions.Withdraw:
                        var trasactionAccount = GetAccountClass(chosenAccount);
                        var amount = _transactionService.ReadAndValidateInputAmount();

                        if (_transactionService.CheckIfValidTransaction(trasactionAccount, amount, (int)AccountActions.Withdraw))
                        {
                            trasactionAccount = _transactionService.WithdrawFunds(trasactionAccount, amount);
                            UpdateAndDisplayBalance(trasactionAccount, false);
                        }

                        Console.Write("\nPerform another Withdrawal? (Y/N): ");
                        break;

                    case (int)AccountActions.Deposit:
                        var accountToDeposit = GetAccountClass(chosenAccount);
                        var amountToDeposit = _transactionService.ReadAndValidateInputAmount();

                        accountToDeposit = _transactionService.DepositFunds(accountToDeposit, amountToDeposit);
                        UpdateAndDisplayBalance(accountToDeposit, true);

                        Console.Write("\nPerform another Deposit? (Y/N): ");
                        break;
                    default:
                        Console.WriteLine("Error: No transaction type found!");
                        break;
                }
            } while (_displayService.ReplayYesNo(askToRepeat));

        }

        static void UpdateAndDisplayBalance(Account trasactionAccount, bool deposite)
        {
            var account = "";
            var transactionFees = 0;
            try
            {
                if (trasactionAccount != null)
                {
                    // Updating Existing Account Balance
                    _accountsBalanceList[trasactionAccount.AccountType - 1].Balance = trasactionAccount.Balance;
                    _accountsBalanceList[trasactionAccount.AccountType - 1].NegativeBalance = trasactionAccount.NegativeBalance;

                    foreach (var acc in Enum.GetValues(typeof(AccountTypes)))
                    {
                        if ((int)acc == _accountsBalanceList[trasactionAccount.AccountType - 1].AccountType)
                        {
                            account = acc.ToString();
                        }
                    }

                    if (account == "Savings" && !deposite)
                        transactionFees = 10;

                    var output = "\n\t Account Balance "
                                    + "\n\t ========================================="
                                    + "\n\t| Time: " + DateTime.Now
                                    + "\n\t|\n\t| Account: " + account
                                    + "\n\t|\n\t| Balance: $" + trasactionAccount.Balance
                                    + "\n\t|\n\t| Transaction Fees: $" + transactionFees
                                 + "\n\t|\n\t| Remaining Negative Balance: $" + trasactionAccount.NegativeBalance
                                    + "\n\t|";
                    Console.WriteLine(output);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An Error occured during processing." + e.Message);
                throw;
            }
        }

        static void DisplayBalance(int accountType)
        {
            var account = "";
            foreach (var acc in Enum.GetValues(typeof(AccountTypes)))
            {
                if ((int)acc == _accountsBalanceList[accountType - 1].AccountType)
                {
                    account = acc.ToString();
                }
            }

            var output = "\n\t Account Balance "
                         + "\n\t ========================================="
                         + "\n\t| Time: " + DateTime.Now
                         + "\n\t|\n\t| Account: " + account
                         + "\n\t|\n\t| Balance: $" + _accountsBalanceList[accountType - 1].Balance
                         + "\n\t|";
            Console.WriteLine(output);
        }

        static Account GetAccountClass(int chosenAccount)
        {
            return new Account
            {
                Balance = _accountsBalanceList[chosenAccount - 1].Balance,
                AccountType = chosenAccount,
                NegativeBalance = _accountsBalanceList[chosenAccount - 1].NegativeBalance
            };
        }

        private static void RegisterServices()
        {
            var kernel = new StandardKernel();
            kernel.Load(new AtmSimulatorModule());

            _bankBalanceRepository = kernel.Get<IBankBalanceRepository>();
            _transactionService = kernel.Get<ITransactionService>();
            _displayService = kernel.Get<IDisplayService>();
        }

    }
}
