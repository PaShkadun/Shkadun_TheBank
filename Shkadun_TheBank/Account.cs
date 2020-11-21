using System;
using System.Collections.Generic;

namespace Shkadun_TheBank
{
    class Account
    {
        public List<Card> listCard;
        public string NumberAccount { get; private set; }
        public int Money { get; set; }
        public int CountCard { get; private set; }
        static ConsoleWriteAndRead CWAR;

        public static void PayCredit(List<Account> listAccount, int choose) //Оплата кредита
        {
            //Если нет карт, выходим
            if (listAccount[choose].listCard.Count == 0) { CWAR.SendMessage(ConsoleWriteAndRead.INVALID_CARD); return; }

            //Выбираем карту
            ListCard(listAccount, choose);
            int chooseCard = CWAR.ReadNumber(0, listAccount[choose].listCard.Count - 1);

            //Пробуем выполнить. Если не получается - карта не кредитная
            try
            {
                CreditCard creditCard = (CreditCard)listAccount[choose].listCard[chooseCard];

                //Если нет кредитов
                if (creditCard.creditList.Count == 0)
                {
                    CWAR.SendMessage(ConsoleWriteAndRead.NOT_HAVE_CREDIT);
                }
                else
                {
                    int i = 0;
                    //Вывод списка кредитов
                    foreach (Credit credit in creditCard.creditList)
                    {
                        Console.WriteLine($"{i} - {credit.Sum}");
                        i++;
                    }

                    int chooseCreditCard = CWAR.PayCredit(i);   //Выбор кредита

                    //Если недостаточно средств
                    if (creditCard.Balance < creditCard.creditList[chooseCreditCard].Sum)
                    {
                        CWAR.SendMessage(ConsoleWriteAndRead.INVALID_BALANCE);
                    }
                    else
                    {
                        creditCard.Balance -= creditCard.creditList[chooseCreditCard].Sum;
                        creditCard.creditList[chooseCreditCard].Sum = 0;
                        creditCard.creditList[chooseCreditCard].MonthsOfDebt = 0;

                        //Если кредит выплачен, удаляем
                        if (creditCard.creditList[chooseCreditCard].Months == 0 &&
                            creditCard.creditList[chooseCreditCard].Sum == 0)
                        {
                            creditCard.creditList.RemoveAt(chooseCreditCard);
                        }

                        CWAR.SendMessage(ConsoleWriteAndRead.SUCCESSFUL);
                    }
                }
            }
            catch { CWAR.SendMessage(ConsoleWriteAndRead.BLOCKING_OPERATION); }
        }

        //Проверка карт и начисления кредитов
        public static void CheckAllCards(List<Account> listAccount)
        {
            CreditCard creditCard;

            if (listAccount.Count == 0)  //Если карт нет
            {
                return;
            }

            foreach (Account account in listAccount)
            {
                if (account.listCard.Count == 0) ;  //Если карт на счёте нет.
                else
                {
                    foreach (Card card in account.listCard) //Проверяем карты
                    {
                        try
                        {
                            creditCard = (CreditCard)card;  //И, если это кредитная, начисляем кредиты

                            for (int i = 0; i < creditCard.creditList.Count; i++)    //...если они есть
                            {
                                if (creditCard.creditList[i].Months > 0)
                                {
                                    creditCard.creditList[i].Months--;
                                    creditCard.creditList[i].Sum += creditCard.creditList[i].CreditRate;
                                    creditCard.creditList[i].MonthsOfDebt++;
                                }
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        //Получение кредита
        public static void GetCredit(List<Account> listAccount, int choose)
        {
            if (listAccount[choose].listCard.Count == 0) { return; }
            try
            {
                ListCard(listAccount, choose);  //Выбор счёта
                int chooseCard = CWAR.ReadNumber(0, listAccount[choose].listCard.Count - 1);    //Выбор карты
                //Попытка приравнять карту к кредитной карте
                CreditCard creditCard = (CreditCard)listAccount[choose].listCard[chooseCard];
                CWAR.SendMessage(ConsoleWriteAndRead.CREDIT_INFO);

                if (!Credit.CheckCreditList(creditCard.creditList))  //Если нет задолженностей по кредитам
                {
                    int howMany = CWAR.ReadNumber();    //Сумма
                    int months = CWAR.ReadNumber();     //Количество месяцев
                    creditCard.creditList.Add(new Credit(howMany, months));
                    creditCard.Balance += howMany;
                    CWAR.SendMessage(ConsoleWriteAndRead.SUCCESSFUL);
                }
            }
            catch
            {
                CWAR.SendMessage(ConsoleWriteAndRead.BLOCKING_OPERATION);
            }
        }

        //Снятие(переходный метод)
        public static void PullMoney(List<Account> listAccount, int choose)
        {
            //Если карт нет
            if (listAccount[choose].listCard.Count == 0) { return; }

            //Если есть, выбираем карту и переходим к методу Card.PullCash();
            ListCard(listAccount, choose);
            int chooseCard = CWAR.ReadNumber(0, listAccount[choose].listCard.Count - 1);
            listAccount[choose].listCard[chooseCard].PullCash();
        }

        //Вывод счетов
        public static void ListAccount(List<Account> listAccount)
        {
            //Если их нет
            if (listAccount.Count == 0) { return; }

            int i = 0;

            foreach (Account account in listAccount)
            {
                Console.WriteLine($"{i} - {account.NumberAccount}");
            }
        }

        //Выбор карты для дальнейшего её пополнения
        public static void ChooseCard(List<Account> listAccount, int choose)
        {
            if (listAccount[choose].listCard.Count == 0) { return; }
            Account.ListCard(listAccount, choose);  //Выводим список карт на аккаунте, если они есть
            int chooseCard = CWAR.ReadNumber(0, listAccount[choose].listCard.Count - 1);
            int howMany = CWAR.HowMany("положить на");  //Спрашиваем, сколько ложить
            //Переходим к методу AddCash();
            if (listAccount[choose]
                .listCard[chooseCard]
                .AddCash(howMany, listAccount[choose].Money) == true)   //и пополняем карту, если всё ОК
            {
                listAccount[choose].Money -= howMany;
                listAccount[choose].listCard[chooseCard].Balance += howMany;
            }
        }

        //Переходный метод к Card.Transfer(Card);
        public static void TransferOnCard(List<Account> listAccount, int choose)
        {
            if (listAccount[choose].listCard.Count == 0) { return; }

            //Список карт на выбранном акке
            ListCard(listAccount, choose);
            int chooseCard = CWAR.ReadNumber(0, listAccount[choose].listCard.Count - 1);

            //Выбираем акк. куда переводим
            ListAccount(listAccount);
            int chooseTransferAccount = CWAR.ReadNumber(0, listAccount.Count - 1);

            //Если колво карт 0
            if (listAccount[chooseTransferAccount].listCard.Count == 0)
            {
                CWAR.SendMessage(ConsoleWriteAndRead.INVALID_CARD);
                return;
            }
            else
            {
                //Иначе Выбираем карту, куда переводим
                ListCard(listAccount, chooseTransferAccount);
                int chooseTransferCard = CWAR.ReadNumber(0, listAccount[chooseTransferAccount].listCard.Count - 1);

                //И пытаемся(не получится, если кредитная на дебет)
                try
                {
                    listAccount[choose].listCard[chooseCard].Transfer(
                            (CreditCard)listAccount[chooseTransferAccount].listCard[chooseTransferCard]
                        );
                }
                catch
                {
                    CWAR.SendMessage(ConsoleWriteAndRead.BLOCKING_OPERATION);
                }
            }
        }

        //Переходный этап к методу Card.Transfer(account)
        public static void TransferOnAccount(List<Account> listAccount, int choose)
        {
            ListCard(listAccount, choose);

            if (listAccount[choose].listCard.Count == 0) { return; }

            int chooseCard = CWAR.ReadNumber(0, listAccount[choose].listCard.Count - 1);
            listAccount[choose].listCard[chooseCard].Transfer(CWAR.WriteNameAccount(), CWAR.HowManyTransfer());
        }

        //Вывод списка карт
        public static void ListCard(List<Account> listAccount, int choose)
        {
            CreditCard creditCard = new CreditCard();
            string type;
            int i = 0;

            foreach (Card card in listAccount[choose].listCard)
            {
                if (card.GetType() == creditCard.GetType()) { type = "Credit"; }
                else { type = "Debet"; }

                Console.WriteLine($"{i} - {card.CardNumber} {type} {card.Balance}");
                i++;
            }
        }

        //Добавления карты
        public static void AddCard(List<Account> listAccount, int choose)
        {
            switch (CWAR.ChooseCardType())
            {
                case 0: listAccount[choose].listCard.Add(new CreditCard()); break;
                case 1: listAccount[choose].listCard.Add(new DebetCard()); break;
                default: break;
            }
        }

        public Account()
        {
            Money = 1000;
            NumberAccount = NewRandom.CreateNumberAccount();
            CWAR = new ConsoleWriteAndRead();
            listCard = new List<Card>();
        }
    }
}
