using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSimulator.Entities;
using ATMSimulator.Interfaces;

namespace ATMSimulator.Services
{
    public class DisplayService : IDisplayService
    {
        private const int Mainmenu = 0;
        private const int Accountsmenu = 1;

        public void DisplayMenu(int actionNumber)
        {
            var newMenu = "";
            PrintAtmWelcomeMessage();
            if (actionNumber == Mainmenu)
            {
                //Main Menu
                newMenu = "\n\t\t Transaction Menu"
                          + "\n\t  -----------------------------------------"
                          + "\n\t\t" + (int)AccountActions.Deposit + "   " + AccountActions.Deposit
                          + "\n\t\t" + (int)AccountActions.Withdraw + "   " + AccountActions.Withdraw
                          + "\n\t\t" + (int)AccountActions.Transfer + "   " + AccountActions.Transfer
                          + "\n\t\t" + (int)AccountActions.BalanceCheck + "   " + AccountActions.BalanceCheck;
            }
            else if (actionNumber >= Accountsmenu)
            {
                //Accounts List
                newMenu = "\n\t\t  Account List"
                          + "\n\t  -----------------------------------------"
                          + "\n\t\t" + (int)AccountTypes.Savings + "   " + AccountTypes.Savings
                          + "\n\t\t" + (int)AccountTypes.Cheque + "   " + AccountTypes.Cheque
                          + "\n\t\t" + (int)AccountTypes.Deposit + "   " + AccountTypes.Deposit;
            }
            newMenu = newMenu + "\n\t\t0   Cancel";
            switch (actionNumber)
            {
                case (int)AccountActions.Deposit:
                    newMenu = newMenu + "\n\n Please confirm in which Account you want to deposit: ";
                    break;
                case (int)AccountActions.Withdraw:
                    newMenu = newMenu + "\n\n Please confirm from which Account you want to withdraw: ";
                    break;
                case (int)AccountActions.Transfer:
                    newMenu = newMenu + "\n\n Please confirm from which Account you want to trasfer: ";
                    break;
                case (int)AccountActions.BalanceCheck:
                    newMenu = newMenu + "\n\n Please confirm for which Account you want to check the balance: ";
                    break;
                default:
                    newMenu = newMenu + "\n\nSelect your Action: ";
                    break;
            }
            Console.Write(newMenu);
        }

        public void PrintAtmWelcomeMessage()
        {
            Console.Clear();
            Console.WriteLine("\n\t *****************************************"
                              + "\n\t     * Welcome to Personal Bank ATM *"
                              + "\n\t *****************************************");
        }

        public int ReadAndValidateMenuOptions(int menuOptions)
        {
            var vaildOption = false;
            var option = 0;

            try
            {
                do
                {
                    var input = Console.ReadLine();
                    if (input != null)
                    {
                        option = Convert.ToInt16(input);
                        if ((0 <= option) && (option <= menuOptions))
                        {
                            vaildOption = true;
                        }
                        else
                        {
                            Console.Write("\n You must Choose an option from 0 to {0}: ", menuOptions);
                        }
                    }

                } while (!vaildOption);

            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured during the transaction. Make sure You have enter currect input. \n" + e.Message);
                throw;
            }

            return (option);
        }

        public void Exit()
        {
            PrintAtmWelcomeMessage();
            Console.WriteLine("\n  Thank you for using Personal Bank ATM Serice!!");
            Console.WriteLine("\n\n  Press any key to exit.");
            Console.ReadKey();
        }

        public bool ReplayYesNo(int biPass = 1)
        {
            var validAnwser = false;
            if (biPass != Mainmenu)
            {
                do
                {
                    var response = Console.ReadLine();
                    if ((response == "y") || (response == "Y"))
                    {
                        validAnwser = true;
                    }
                    else if ((response == "n") || (response == "N"))
                    {
                        break;
                    }
                    else
                    {
                        Console.Write("\nY or N?");
                    }
                } while (!validAnwser);
            }
            else
            {
                Console.WriteLine("Error in RepeatYesNo Parameter");
            }
            return validAnwser;
        }
    }
}
