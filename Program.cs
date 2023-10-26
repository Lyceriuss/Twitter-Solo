using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


class Program{

    static void Main(string[] args)
    {
        MemberService memberService = new MemberService();
        MessageService messageService = new MessageService();
        DateTime Time = DateTime.Now;

        memberService.Register("A", "Abdulina Monaco", 25, "1");
        memberService.Register("B", "Bibidi Bob", 30, "2");
        memberService.Register("C", "Chai Tete", 28, "3");
        messageService.SendMessage(memberService.GetMemberByUsername("B"), memberService.GetMemberByUsername("A"), "Hey A! How's it going?", Time.AddHours(-5).AddMinutes(-32));
        messageService.SendMessage(memberService.GetMemberByUsername("C"), memberService.GetMemberByUsername("A"), "A, did you get my last email?", Time.AddHours(-4).AddMinutes(-15));

        messageService.SendMessage(memberService.GetMemberByUsername("C"), memberService.GetMemberByUsername("B"), "Let's catch up soon!", Time.AddHours(-2).AddMinutes(-23));

        messageService.SendMessage(memberService.GetMemberByUsername("B"), memberService.GetMemberByUsername("C"), "Hey B! All good. How about you?", Time.AddHours(-5).AddMinutes(-10));
        messageService.SendMessage(memberService.GetMemberByUsername("A"), memberService.GetMemberByUsername("B"), "B, we should discuss the new proposal.", Time.AddHours(-1).AddMinutes(-40));
        messageService.SendMessage(memberService.GetMemberByUsername("A"), memberService.GetMemberByUsername("C"), "Sure C! Let's schedule a call.", Time.AddMinutes(-30));

        ChatMember loggedInUser = null;



        while (true)
        {
            while (loggedInUser == null)
            {
                Console.WriteLine("Please login to access the chat features.");
                loggedInUser = ChatMember.Login(memberService);
            }

            Console.Clear();
            UI.MenuUI();

            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > 5)
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
                        Console.Write("Enter your message:");
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
                    Console.ReadKey();
                    break;
                case 3:  // This is a "pseudo-case" just to fit into the existing structure
                    var recentSendersWithMessages = messageService.GetThreeRecentSendersWithMessages(loggedInUser);
                    if (!recentSendersWithMessages.Any())
                    {
                        Console.WriteLine("No recent messages received.");
                        break;
                    }

                    Console.WriteLine("Most recent messages from up to 3 users:");
                    System.Console.WriteLine(" ");
                    int counter = 1;
                    foreach (var item in recentSendersWithMessages)
                    {
                        Console.WriteLine($"{counter}. {item.message.DisplayMessage()}");
                        counter++;
                    }
                    System.Console.WriteLine(" ");

                    Console.WriteLine("Reply to (enter number 1/2/3 or any other key to skip):");
                    if (int.TryParse(Console.ReadLine(), out int userChoice) && userChoice >= 1 && userChoice <= recentSendersWithMessages.Count)
                    {
                        Console.WriteLine($"Replying to {recentSendersWithMessages[userChoice - 1].sender.Username}. Enter your message:");
                        string replyContent = Console.ReadLine();
                        if (!string.IsNullOrEmpty(replyContent))
                        {
                            messageService.SendMessage(loggedInUser, recentSendersWithMessages[userChoice - 1].sender, replyContent);
                            Console.WriteLine("Reply sent successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Message content cannot be empty.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Reply skipped.");
                    }
                    break;

                case 4:
                    loggedInUser = null;
                    Console.WriteLine("Logged out successfully!");
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
