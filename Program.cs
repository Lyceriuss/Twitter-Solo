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
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Send Message");
            Console.WriteLine("3. View Messages");
            Console.WriteLine("4. Logout");
            Console.WriteLine("5. Exit");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    loggedInUser = ChatMember.Login(memberService);
                    break;

                case 2:
                    if (loggedInUser != null)
                    {
                        Console.WriteLine("Enter recipient username:");
                        string recipientUsername = Console.ReadLine();
                        ChatMember recipient = memberService.GetMemberByUsername(recipientUsername);

                        if (recipient != null)
                        {
                            Console.WriteLine("Enter your message:");
                            string content = Console.ReadLine();
                            messageService.SendMessage(loggedInUser, recipient, content);
                        }
                        else
                        {
                            Console.WriteLine("Recipient not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You must be logged in to send a message.");
                    }
                    break;

                case 3:
                    if (loggedInUser != null)
                    {
                        List<PrivateMessage> userMessages = messageService.GetMessagesForUser(loggedInUser);
                        foreach (var message in userMessages)
                        {
                            Console.WriteLine(message.DisplayMessage());
                        }
                    }
                    else
                    {
                        Console.WriteLine("You must be logged in to view messages.");
                    }
                    break;

                case 4:
                    loggedInUser = null;
                    Console.WriteLine("Logged out successfully.");
                    break;

                case 5:
                    return; 

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}