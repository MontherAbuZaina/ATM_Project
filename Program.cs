using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
// cardNumber -> 8 digits
namespace ATM_Assignment
{
    [Serializable]
    public class Person
    {
        string firstName, lastName;

        public Person()
        {
            firstName = "Munther";
            lastName = "AbuZaina";
        }

        public Person(string firstName, string lastName)
        {
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
            }
        }
    }

    [Serializable]
    public class BankAccount
    {
        Person p = new Person();
        string email, cardNumber, pinCode;
        int accountBalance;

        public BankAccount(Person p, string email, string cardNumber, string pinCode, int accountBalance)
        {
            this.p = p;
            this.email = email;
            this.cardNumber = cardNumber;
            this.pinCode = pinCode;
            this.accountBalance = accountBalance;
        }

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }
        public string CardNumber
        {
            get
            {
                return cardNumber;
            }
            set
            {
                cardNumber = value;
            }
        }
        public string PinCode
        {
            get
            {
                return pinCode;
            }
            set
            {
                pinCode = value;
            }
        }
        public int AccountBalance
        {
            get
            {
                return accountBalance;
            }
            set
            {
                accountBalance = value;
            }
        }
    }


    [Serializable]
    public class Bank
    {
        int bankCapacity;
        int numberOfCustomers = 0;
        BankAccount[] accounts;
        public Bank(int bankCapacity = 100)
        {
            this.bankCapacity = bankCapacity;
            accounts = new BankAccount[bankCapacity];
        }

        public int NumberOfCustomers
        {
            get
            {
                return numberOfCustomers;
            }
            set
            {
                numberOfCustomers = value;
            }
        }

        public void Save()
        {
            FileStream fs = new FileStream("data.txt", FileMode.Create, FileAccess.Write);
            BinaryFormatter f = new BinaryFormatter();
            for (int i = 0; i < numberOfCustomers; ++i)
            {
                f.Serialize(fs, accounts[i]);
            }
            fs.Close();
        }

        public void Load()
        {
            FileStream fs = new FileStream("data.txt", FileMode.Open, FileAccess.Read);
            BinaryFormatter f = new BinaryFormatter();
            int index = 0;
            numberOfCustomers = 0; // maybe the file given and we don't know the number of customers
            while (fs.Position < fs.Length && index < bankCapacity)
            {
                BankAccount cur = (BankAccount)f.Deserialize(fs);
                accounts[index] = cur;
                numberOfCustomers++;
                index++;
            }
            fs.Close();
        }

        bool checkIdentity(string _cardNum, string _pinPass)
        {
          return IsBankUser(_cardNum, _pinPass);
        }

        public void AddNewAccount(BankAccount cur)
        {
            if (numberOfCustomers >= bankCapacity || cur.CardNumber.Length != 8 || cur.PinCode.Length != 4)
            {
                return;
            }
            accounts[numberOfCustomers++] = cur;
            Save();
        }

        public int CheckBalance(string cardNum, string pinPass)
        {
            if (!checkIdentity(cardNum, pinPass))
            {
                Console.WriteLine("Please Enter Your Info Correctly");
                return -1;
            }
            for (int i = 0; i < numberOfCustomers; ++i)
            {
                if (accounts[i].CardNumber == cardNum && accounts[i].PinCode == pinPass)
                {
                    return accounts[i].AccountBalance;
                }
            }
            return -1;
        }

        public bool IsBankUser(string cardNum, string pinPass)
        {
            for (int i = 0; i < numberOfCustomers; ++i)
            {
                if (accounts[i].CardNumber == cardNum && accounts[i].PinCode == pinPass)
                {
                    return true;
                }
            }
            return false;
        }

        public void Withdraw(BankAccount cur, int amount)
        {
            if (!checkIdentity(cur.CardNumber, cur.PinCode))
            {
                Console.WriteLine("Please Enter Your Info Correctly, Bank Account not found");
                return;
            }
            for (int i = 0; i < numberOfCustomers; ++i)
            {
                if (accounts[i] == cur)
                {
                    if (accounts[i].AccountBalance >= amount)
                    {
                        accounts[i].AccountBalance -= amount;
                        Save();
                        Console.WriteLine("Transaction done");  
                    }
                    else
                    {
                        Console.WriteLine("Not enough Money");
                    }
                    return;
                }
            }
        }

        public void Deposit(BankAccount cur, int amount)
        {
            if (!checkIdentity(cur.CardNumber, cur.PinCode))
            {
                Console.WriteLine("Please Enter Your Info Correctly, Bank Account not found");
                return;
            }
            for (int i = 0; i < numberOfCustomers; ++i)
            {
                if (accounts[i] == cur)
                {
                    accounts[i].AccountBalance += amount;
                    Save();
                    Console.WriteLine("Transaction done");
                    return;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}