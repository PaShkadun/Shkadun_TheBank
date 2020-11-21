using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Shkadun_TheBank
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Account> listAccounts = new List<Account>();       //Список счетов
            ConsoleWriteAndRead cwar = new ConsoleWriteAndRead();   //Класс консольного ввода-вывода сообщений

            //Параллельная задача, начисляющая проценты по кредитам каждые 20 секунд.
            Task task = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(20000);
                    Account.CheckAllCards(listAccounts);    //Метод проверяющий и начисляющий кредиты
                }
            });

            while (true)
            {
                switch (cwar.WhatDo())
                {
                    case 0: //Создание счёта
                        listAccounts.Add(new Account());
                        break;
                    case 1: //Создание карты
                        Account.ListAccount(listAccounts);
                        Account.AddCard(listAccounts, cwar.ReadNumber(0, listAccounts.Count - 1));
                        break;
                    case 2: //Вывод списка карт
                        Account.ListAccount(listAccounts);
                        Account.ListCard(listAccounts, cwar.ReadNumber(0, listAccounts.Count - 1));
                        break;
                    case 3: //Пополнить карту
                        Account.ListAccount(listAccounts);
                        Account.ChooseCard(listAccounts, cwar.ReadNumber(0, listAccounts.Count - 1));
                        break;
                    case 4: //Снять с карты 
                        Account.ListAccount(listAccounts);
                        Account.PullMoney(listAccounts, cwar.ReadNumber(0, listAccounts.Count - 1));
                        break;
                    case 5: //Первод на карту
                        Account.ListAccount(listAccounts);
                        Account.TransferOnCard(listAccounts, cwar.ReadNumber(0, listAccounts.Count - 1));
                        break;
                    case 6: //Перевод на счёт
                        Account.ListAccount(listAccounts);
                        Account.TransferOnAccount(listAccounts, cwar.ReadNumber(0, listAccounts.Count - 1));
                        break;
                    case 7: //Взять кредит
                        Account.ListAccount(listAccounts);
                        Account.GetCredit(listAccounts, cwar.ReadNumber(0, listAccounts.Count - 1));
                        break;
                    case 8: //Погасить кредит
                        Account.ListAccount(listAccounts);
                        Account.PayCredit(listAccounts, cwar.ReadNumber(0, listAccounts.Count - 1));
                        break;
                    default: break;
                }
                Thread.Sleep(500);
            }
        }
    }
}
