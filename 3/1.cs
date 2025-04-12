using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ContactManagementApp
{
    public enum ContactType
    {
        Work,
        School,
        Friend,
        Family,
        Other
    }

    public class Contact
    {
        public string firstname;
        public string lastname;
        public string phonenumber;
        public ContactType type;

        public Contact(string firstname, string lastname, string phonenumber, ContactType type)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.phonenumber = phonenumber;
            this.type = type;
        }
    }

    public class User
    {
        public string firstname;
        public string lastname;
        public string phonenumber;
        public List<Contact> contacts;

        public User(string firstname, string lastname, string phonenumber, List<Contact>? contacts = null)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.phonenumber = phonenumber;
            this.contacts = contacts ?? new List<Contact>();
        }
    }

    public class ContactManager
    {
        private const string filePath = "Users.txt";

        public ContactManager()
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
        }

        private static bool IsValidPhonenumber(string phonenumber)
        {
            if (phonenumber.Length != 11)
                return false;

            foreach (char c in phonenumber)
                if (!Char.IsDigit(c))
                    return false;

            return phonenumber.StartsWith("09");
        }

        private static bool IsValidName(string name, int minLen = 0, int maxLen = 256)
        {
            if (name.Length < minLen || name.Length > maxLen)
                return false;

            foreach (char c in name)
                if (!Char.IsLetter(c))
                    return false;

            return true;
        }

        private bool UserExists(string phonenumber)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("phonenumber"))
                    {
                        string phoneNumber = line.Substring("phonenumber: ".Length);
                        if (phonenumber == phoneNumber)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public User GetUserInfo()
        {
            string? input;
            string firstname, lastname, phonenumber;
            do
            {
                Console.Write("Enter user's first name: ");
                try
                {
                    input = Console.ReadLine();
                    if (input is null || !IsValidName(input, 2, 30))
                        throw new Exception("Invalid first name.");
                    firstname = input;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            } while (true);

            do
            {
                Console.Write("Enter user's last name: ");
                try
                {
                    input = Console.ReadLine();
                    if (input is null || !IsValidName(input, 2, 50))
                        throw new Exception("Invalid last name.");
                    lastname = input;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            } while (true);

            do
            {
                Console.Write("Enter user's phone number: ");
                try
                {
                    input = Console.ReadLine();
                    if (input is null || !IsValidPhonenumber(input))
                        throw new Exception("Invalid phone number.");
                    else if (UserExists(input))
                        throw new Exception("Phonenumber already exists");
                    phonenumber = input;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            } while (true);

            return new User(firstname, lastname, phonenumber);
        }

        public bool SaveUser(User user)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine($"firstname: {user.firstname}");
                    sw.WriteLine($"lastname: {user.lastname}");
                    sw.WriteLine($"phonenumber: {user.phonenumber}");
                    sw.WriteLine("contacts:");
                    foreach (var contact in user.contacts)
                    {
                        sw.WriteLine($"\tfirstname: {contact.firstname}");
                        sw.WriteLine($"\tlastname: {contact.lastname}");
                        sw.WriteLine($"\tphonenumber: {contact.phonenumber}");
                        sw.WriteLine($"\tcontact-type: {contact.type}");
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public User RetrieveUserInfo(string phoneNumber)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string? line;
                bool readingContacts = false;
                string firstname = "", lastname = "", phonenumber = "";
                string contactFirsname = "", contactLastname = "", contactPhonenumber = "", contactType;
                List<Contact> contacts = new List<Contact>();

                while ((line = sr.ReadLine()) != null)
                {
                    if (readingContacts)
                    {
                        if (!line.StartsWith("\t"))
                            return new User(firstname, lastname, phonenumber, contacts);
                        else
                        {
                            if (line.StartsWith("\tfirstname: "))
                            {
                                contactFirsname = line.Substring("\tfirstname: ".Length);
                            }
                            else if (line.StartsWith("\tlastname: "))
                            {
                                contactLastname = line.Substring("\tlastname: ".Length);
                            }
                            else if (line.StartsWith("\tphonenumber: "))
                            {
                                contactPhonenumber = line.Substring("\tphonenumber: ".Length);
                            }
                            else if (line.Contains("\tcontact-type: "))
                            {
                                contactType = line.Substring("\tcontact-type: ".Length);
                                Enum.TryParse(contactType, out ContactType type);
                                contacts.Add(new Contact(contactFirsname, contactLastname, contactPhonenumber, type));
                            }
                        }
                    }
                    else if (line.StartsWith("firstname: "))
                    {
                        firstname = line.Substring("firstname: ".Length);
                    }
                    else if (line.StartsWith("lastname: "))
                    {
                        lastname = line.Substring("lastname: ".Length);
                    }
                    else if (line.StartsWith("phonenumber: "))
                    {
                        phonenumber = line.Substring("phonenumber: ".Length);
                    }
                    else if (line.StartsWith("contacts:") && phonenumber == phoneNumber)
                    {
                        readingContacts = true;
                    }
                }
                if (readingContacts)
                {
                    return new User(firstname, lastname, phonenumber, contacts);
                }
            }
            // Just to remove warning, wasn't needed
            throw new Exception("User Doesn't exist");
        }

        public void PrintUserInfo(User user)
        {
            Console.WriteLine($"firstname: {user.firstname}");
            Console.WriteLine($"lastname: {user.lastname}");
            Console.WriteLine($"phonenumber: {user.phonenumber}");
            Console.WriteLine($"contacts: ");
            foreach (var contact in user.contacts)
            {
                Console.WriteLine($"\tfirstname: {contact.firstname}");
                Console.WriteLine($"\tlastname: {contact.lastname}");
                Console.WriteLine($"\tphonenumber: {contact.phonenumber}");
                Console.WriteLine($"\tcontact-type: {contact.type}");
            }
        }

        public Contact GetContactInfo()
        {
            string? input;
            string firstname, lastname, phonenumber;
            int typeNum;
            do
            {
                Console.Write("Enter contact's first name: ");
                try
                {
                    input = Console.ReadLine();
                    if (input is null || !IsValidName(input, 2, 30))
                        throw new Exception("Invalid first name.");
                    firstname = input;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            } while (true);

            do
            {
                Console.Write("Enter contact's last name: ");
                try
                {
                    input = Console.ReadLine();
                    if (input is null || !IsValidName(input, 2, 50))
                        throw new Exception("Invalid last name.");
                    lastname = input;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            } while (true);

            do
            {
                Console.Write("Enter contact's phone number: ");
                try
                {
                    input = Console.ReadLine();
                    if (input is null || !IsValidPhonenumber(input))
                        throw new Exception("Invalid phone number.");
                    else if (!UserExists(input))
                        throw new Exception("Phonenumber doesn't exist");
                    phonenumber = input;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            } while (true);

            Console.WriteLine("Contact types:");
            foreach (var item in Enum.GetValues(typeof(ContactType)))
            {
                Console.WriteLine($"{(int)item} - {item}");
            }

            do
            {
                Console.Write("Select contact type: ");
                input = Console.ReadLine();
                try
                {
                    if (!int.TryParse(input, out typeNum) || !Enum.IsDefined(typeof(ContactType), typeNum))
                        throw new Exception("Invalid contact type.");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (true);

            return new Contact(firstname, lastname, phonenumber, (ContactType)typeNum);
        }

        public bool AddContact(string userPhoneNumber, Contact contact)
        {
            var user = RetrieveUserInfo(userPhoneNumber);
            if (user == null)
                return false;

            user.contacts.Add(contact);
            OverwriteUser(user);
            return true;
        }

        public bool DeleteContact(string userPhoneNumber, string contactPhoneNumber)
        {
            var user = RetrieveUserInfo(userPhoneNumber);
            if (user == null)
                return false;

            var contact = user.contacts.FirstOrDefault(c => c.phonenumber == contactPhoneNumber);
            if (contact == null)
                return false;

            user.contacts.Remove(contact);
            OverwriteUser(user);
            return true;
        }

        public bool UpdateContact()
        {
            string? input;
            string userPhonenumber, contactPhonenumber;

            do
            {
                Console.Write("Enter user's phone number: ");
                try
                {
                    input = Console.ReadLine();
                    if (input is null || !IsValidPhonenumber(input))
                        throw new Exception("Invalid phone number.");
                    if (!UserExists(input))
                        throw new Exception("User not found.");
                    userPhonenumber = input;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (true);

            User user = RetrieveUserInfo(userPhonenumber);

            do
            {
                Console.Write("Enter contact's phone number: ");
                try
                {
                    input = Console.ReadLine();
                    if (input is null || !IsValidPhonenumber(input))
                        throw new Exception("Invalid phone number.");
                    if (!user.contacts.Any(c => c.phonenumber == input))
                        throw new Exception("Contact not found.");
                    contactPhonenumber = input;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (true);

            Contact contact = user.contacts.First(c => c.phonenumber == contactPhonenumber);

            do
            {
                Console.Write("Enter new first name (-1 to skip): ");
                try
                {
                    input = Console.ReadLine();
                    if (input == "-1")
                        break;
                    if (input is null || !IsValidName(input, 2, 30))
                        throw new Exception("Invalid first name.");
                    contact.firstname = input;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (true);

            do
            {
                Console.Write("Enter new last name (-1 to skip): ");
                try
                {
                    input = Console.ReadLine();
                    if (input == "-1")
                        break;
                    if (input is null || !IsValidName(input, 2, 50))
                        throw new Exception("Invalid last name.");
                    contact.lastname = input;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (true);

            Console.WriteLine("Contact types:");
            foreach (var item in Enum.GetValues(typeof(ContactType)))
            {
                Console.WriteLine($"{(int)item} - {item}");
            }

            do
            {
                Console.Write("Select new contact type (-1 to skip): ");
                input = Console.ReadLine();
                try
                {
                    if (!int.TryParse(input, out int typeNum) || !Enum.IsDefined(typeof(ContactType), typeNum))
                        throw new Exception("Invalid contact type.");
                    contact.type = (ContactType)typeNum;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (true);

            return UpdateContact(userPhonenumber, contact);
        }

        private bool UpdateContact(string userPhoneNumber, Contact contact)
        {
            var user = RetrieveUserInfo(userPhoneNumber);
            if (user == null)
                return false;

            var _contact = user.contacts.FirstOrDefault(c => c.phonenumber == contact.phonenumber);
            if (contact == null)
                return false;

            for (int i = 0; i < user.contacts.Count; ++i)
            {
                if (user.contacts[i].phonenumber == contact.phonenumber)
                {
                    user.contacts[i] = contact;
                    break;
                }
            }

            OverwriteUser(user);
            return true;
        }

        public bool OverwriteUser(User updatedUser)
        {
            List<string> allLines = new List<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                string? line;
                bool updated = false;
                while ((line = sr.ReadLine()) != null)
                {
                    if (updated)
                    {
                        allLines.Add(line);
                        continue;
                    }

                    if (line.StartsWith("firstname: "))
                    {
                        string firstname = line.Substring("firstname: ".Length);
                        allLines.Add(line);
                    }
                    else if (line.StartsWith("lastname: "))
                    {
                        string lastname = line.Substring("lastname: ".Length);
                        allLines.Add(line);
                    }
                    else if (line.StartsWith("phonenumber: "))
                    {
                        string phonenumber = line.Substring("phonenumber: ".Length);
                        allLines.Add(line);
                        if (phonenumber == updatedUser.phonenumber)
                        {
                            allLines.Add("contacts:");
                            foreach (var contact in updatedUser.contacts)
                            {
                                allLines.Add($"\tfirstname: {contact.firstname}");
                                allLines.Add($"\tlastname: {contact.lastname}");
                                allLines.Add($"\tphonenumber: {contact.phonenumber}");
                                allLines.Add($"\tcontact-type: {contact.type}");
                            }
                            while ((line = sr.ReadLine()) != null && !line.StartsWith("firstname: ")) ;
                            if (line != null)
                                allLines.Add(line);
                            updated = true;
                        }
                        else
                        {
                            allLines.Add(line);
                        }
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter(filePath, false))
            {
                foreach (var l in allLines)
                {
                    sw.WriteLine(l);
                }
            }

            return true;
        }
    }

    public class Program
    {
        public static void Main()
        {
            ContactManager cm = new ContactManager();
            User user1 = cm.GetUserInfo();
            User user2 = cm.GetUserInfo();
            User user3 = cm.GetUserInfo();

            cm.SaveUser(user1);
            cm.SaveUser(user2);
            cm.SaveUser(user3);

            Contact contact1 = cm.GetContactInfo();

            Contact contact2 = cm.GetContactInfo();

            cm.AddContact("09123456789", contact1);
            cm.AddContact("09123456789", contact2);

            cm.DeleteContact("09123456789", "09133456789");

            cm.UpdateContact();

            user1 = cm.RetrieveUserInfo("09123456789");
            user2 = cm.RetrieveUserInfo("09133456789");
            user3 = cm.RetrieveUserInfo("09143456789");

            cm.PrintUserInfo(user1);
            Console.WriteLine();
            cm.PrintUserInfo(user2);
            Console.WriteLine();
            cm.PrintUserInfo(user3);
        }
    }
}
