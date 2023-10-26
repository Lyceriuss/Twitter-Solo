using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


class Program{

    static void Main(string[] args)
    {
        MemberService memberService = new MemberService();
        MessageService messageService = new MessageService();

        memberService.Register("A", "Abdulina Monaco", 25, "1");
        memberService.Register("B", "Bibidi Bob", 30, "2");
        memberService.Register("C", "Chai Tete", 28, "3");

        ChatMember loggedInUser = null;



        while (true)
        {
            while (loggedInUser == null)
            {
                Console.WriteLine("Please login to access the chat features.");
                loggedInUser = ChatMember.Login(memberService);
            }

            Console.WriteLine("1. Send Message");
            Console.WriteLine("2. View Messages");
            Console.WriteLine("3. Logout");
            Console.WriteLine("4. Exit");

            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > 4)
            {
                Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    Console.Write("Enter recipient username:");
                    string recipientUsername = Console.ReadLine();
                    ChatMember recipient = memberService.GetMemberByUsername(recipientUsername);

                    if (recipient != null)
                    {
                        Console.WriteLine("Enter your message:");
                        string content = Console.ReadLine();
                        if (!string.IsNullOrEmpty(content))
                        {
                            messageService.SendMessage(loggedInUser, recipient, content);
                            Console.WriteLine("Message sent successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Message content cannot be empty.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Recipient not found.");
                    }
                    break;

                case 2:
                    List<PrivateMessage> userMessages = messageService.GetMessagesForUser(loggedInUser);
                    foreach (var message in userMessages)
                    {
                        Console.WriteLine(message.DisplayMessage());
                    }
                    break;

                case 3:
                    loggedInUser = null;
                    Console.WriteLine("Logged out successfully!");
                    break;

                case 4:
                    return;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}