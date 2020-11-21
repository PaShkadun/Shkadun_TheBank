
namespace Shkadun_TheBank
{
    abstract class Card
    {
        public int Balance { get; set; }
        public long CardNumber { get; protected set; }
        public ConsoleWriteAndRead CWAR { get; set; }   //Экземпляр класса консольного ввода-вывода сообщений

        abstract public void Transfer(CreditCard card);
        abstract public void Transfer(string NumberAccount, int howMany);
        //Положить средства на карту
        public bool AddCash(int howMany, int accountMoney)
        {
            if (howMany > accountMoney)  //Если средств недостаточно
            {
                CWAR.SendMessage(ConsoleWriteAndRead.INVALID_BALANCE);
                return false;
            }
            else
            {
                CWAR.SendMessage(ConsoleWriteAndRead.SUCCESSFUL);
                return true;
            }
        }

        abstract public void PullCash();
    }
}
