using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Shkadun_TheBank
{
    class CreditCard : Card
    {
        public List<Credit> creditList;

        public override void PullCash()     //Снятие средств
        {
            if (!Credit.CheckCreditList(creditList) && Balance > 0) //Если неоплаченных кредитов нет и баланс положителен
            {
                int howMany = CWAR.HowMany();   //Запрос ввода желаемой суммы
                if (howMany <= Balance)          //Если недостаточно средств
                {
                    Balance -= howMany;
                    CWAR.SendMessage(ConsoleWriteAndRead.SUCCESSFUL);
                }
                else
                {
                    CWAR.SendMessage(ConsoleWriteAndRead.INVALID_BALANCE);
                }
            }
            else
            {
                CWAR.SendMessage(ConsoleWriteAndRead.NEGATIVE_CREDIT);
            }
        }

        public override void Transfer(CreditCard card)                  //Перевод на карту
        {
            if (!Credit.CheckCreditList(creditList) && Balance > 0)     //Если нет неоплаченных кредитов и баланс положителен
            {
                int howMany = CWAR.HowMany();   //Запрос суммы

                if (howMany <= Balance)         //Если средств достаточно
                {
                    Balance -= howMany;
                    card.Balance += howMany;
                    CWAR.SendMessage(ConsoleWriteAndRead.SUCCESSFUL);
                }
                else
                {
                    CWAR.SendMessage(ConsoleWriteAndRead.INVALID_BALANCE);
                }
            }
            else
            {
                CWAR.SendMessage(ConsoleWriteAndRead.NEGATIVE_CREDIT);
            }
        }

        public override void Transfer(string numberAccount, int howMany)    //Перевод на счёт
        {
            if (numberAccount.Length != 20)     //Если номер счёта короче 20 символов
            {
                CWAR.SendMessage(ConsoleWriteAndRead.INVALID_INPUT);
                return;
            }

            Regex regex = new Regex(@"\w");                         //Разрешаем использовтаь только цифры и буквы
            MatchCollection match = regex.Matches(numberAccount);   //Проверяем кол-во совпадений в строке

            if (match.Count != 20)  //Если их не 20, то строка некорректная
            {
                CWAR.SendMessage(ConsoleWriteAndRead.INVALID_INPUT);
            }
            else                    //Иначе
            {
                if (!Credit.CheckCreditList(creditList) && Balance > 0) //Если нет неоплаченных кредитов и баланс +
                {
                    if (howMany <= Balance) //И сумма меньше суммы на счёте
                    {
                        Balance -= howMany;
                        CWAR.SendMessage(ConsoleWriteAndRead.SUCCESSFUL);
                    }
                    else
                    {
                        CWAR.SendMessage(ConsoleWriteAndRead.INVALID_BALANCE);
                    }
                }
                else
                {
                    CWAR.SendMessage(ConsoleWriteAndRead.NEGATIVE_CREDIT);
                }
            }
        }

        public void AddCredit(int sum, int months)  //Добавить кредит
        {
            if (!Credit.CheckCreditList(creditList)) //Если нет неоплаченных кредитов
            {
                if (Balance >= 0)                    //И баланс > 0
                {
                    Balance += sum;
                    creditList.Add(new Credit(sum, months));
                    CWAR.SendMessage(ConsoleWriteAndRead.SUCCESSFUL);
                }
                else
                {
                    CWAR.SendMessage(ConsoleWriteAndRead.NEGATIVE_CREDIT);
                }
            }
            else
            {
                CWAR.SendMessage(ConsoleWriteAndRead.NEGATIVE_CREDIT);
            }
        }

        public CreditCard()
        {
            Balance = 0;
            CWAR = new ConsoleWriteAndRead();
            creditList = new List<Credit>();
            CardNumber = NewRandom.RandomCardNumber();
        }
    }
}
