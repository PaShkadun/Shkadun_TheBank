using System;

namespace Shkadun_TheBank
{
    class ConsoleWriteAndRead
    {
        public const string INVALID_BALANCE = "Недостаточно средств";
        public const string INVALID_CARD = "Недостаточно карт";
        public const string BLOCKING_OPERATION = "Недоступная операция";
        public const string INVALID_INPUT = "Некорректный ввод";
        public const string NEGATIVE_CREDIT = "Отказано. Имеется задолженность по кредиту или баланс отрицателен.";
        public const string SUCCESSFUL = "Успешная операция";
        public const string CREDIT_INFO = "Введите сумму и кол-во месяцев кредита";
        public const string NOT_HAVE_CREDIT = "У вас нет кредитов по этой карте";

        public int ChooseCardType()
        {
            Console.WriteLine("0 - кредитная, 1 - дебетовая");
            return ReadNumber(0, 1);
        }

        public int HowManyTransfer()
        {
            Console.WriteLine("Сколько хотите перевести на счёт?");
            return ReadNumber();
        }

        public int HowMany(string str = "положить на")
        {
            Console.WriteLine($"Сколько хотите {str} карту?");
            return ReadNumber();
        }

        public int PayCredit(int i)
        {
            Console.WriteLine("Выберите карточку");
            return ReadNumber(0, i);
        }

        public void SendMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void Clear()
        {
            Console.Clear();
        }
        public int ReadNumber()
        {
            int read;

            while (!int.TryParse(Console.ReadLine(), out read)) ;
            return read;
        }
        public int ReadNumber(int min, int max)
        {
            int read;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out read))
                {
                    if (read >= min && read <= max)
                    {
                        Clear();
                        return read;
                    }
                    else
                    {
                        Console.WriteLine($"Недопустимое значение, диапазон {min}-{max}");
                    }
                }
                else
                {
                    Console.WriteLine("Некорректный ввод");
                }
            }
        }

        public int WhatDo()
        {
            Console.WriteLine("\nЧто дальше?\n0. Создать счёт\n1. Добавить карту\n2. Список карт" +
                              "\n3. Пополнить карту\n4. Снять с карты\n5. Перевести на карту\n" +
                              "6. Перевести на счёт\n7. Взять кредит\n8. Погасить кредит");
            return ReadNumber(0, 8);
        }

        public string WriteNameAccount()
        {
            Console.WriteLine("Введите ФИО получателя");
            Console.ReadLine();
            Console.WriteLine("Введите номер счёта получателя(буквы + цифры)");
            return Console.ReadLine();
        }
    }
}
