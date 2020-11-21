using System.Text.RegularExpressions;

namespace Shkadun_TheBank
{
    class DebetCard : Card
    {
        public override void PullCash() //Снять с карты
        {
            int howMany = CWAR.HowMany("снять с");  //Запрос суммы

            if (howMany > Balance)  //Если средств недостаточно
            {
                CWAR.SendMessage(ConsoleWriteAndRead.INVALID_BALANCE);
            }
            else
            {
                Balance -= howMany;
                CWAR.SendMessage(ConsoleWriteAndRead.SUCCESSFUL);
            }
        }

        public void Transfer(DebetCard card)    //Перевод на дебетовую карту
        {
            int howMany = CWAR.HowMany("перевести на"); //Запрос суммы

            if (howMany > Balance)  //Если средств недостаточно
            {
                CWAR.SendMessage(ConsoleWriteAndRead.INVALID_BALANCE);
            }
            else
            {
                Balance -= howMany;
                card.Balance += howMany;
                CWAR.SendMessage(ConsoleWriteAndRead.SUCCESSFUL);
            }
        }

        public override void Transfer(string numberAccount, int howMany)    //Перевод на счёт
        {
            if (numberAccount.Length != 20)
            {
                CWAR.SendMessage(ConsoleWriteAndRead.INVALID_INPUT);
                return;
            }

            Regex regex = new Regex(@"\w");         //Разрешаем только цифры и буквы
            MatchCollection match = regex.Matches(numberAccount);   //Считаем кол-во совпадений

            if (match.Count != 20)   //Если не 20, то некорректный ввод
            {
                CWAR.SendMessage(ConsoleWriteAndRead.INVALID_INPUT);
            }
            else
            {
                if (howMany > Balance)
                {
                    CWAR.SendMessage(ConsoleWriteAndRead.INVALID_BALANCE);
                }
                else
                {
                    Balance -= howMany;
                    CWAR.SendMessage(ConsoleWriteAndRead.SUCCESSFUL);
                }
            }
        }

        public override void Transfer(CreditCard card)  //Перевод на кредитную карту
        {
            int howMany = CWAR.HowMany("перевести на");

            if (howMany > Balance)
            {
                CWAR.SendMessage(ConsoleWriteAndRead.INVALID_BALANCE);
            }
            else
            {
                Balance -= howMany;
                card.Balance += howMany;
                CWAR.SendMessage(ConsoleWriteAndRead.SUCCESSFUL);
            }
        }

        public DebetCard()
        {
            Balance = 0;
            CardNumber = NewRandom.RandomCardNumber();
            CWAR = new ConsoleWriteAndRead();
        }
    }
}
