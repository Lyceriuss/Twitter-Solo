using System;
using System.Collections.Generic;
using System.Linq;



public class ChatMember              // Private in case user info might be sensitive
{
    public string Username { get; private set; }          
    public string RealName { get; private set; } 
    public int Age { get; private set; }
    public string MemberID { get; private set; }        //An ID that will be used to identify the user in a myriad of different operations
    private string Password { get; set; }           

    public ChatMember(string username, string realName, int age, string password)
    {
        Username = username;
        RealName = realName;
        Age = age;
        MemberID = GenerateUniqueID(); 
        Password = password;
    }

    private string GenerateUniqueID()
    {
        
        return Guid.NewGuid().ToString();   //Might remove this to make sure they are predictable
    }

    public bool Authenticate(string password)
    {
        return Password == password;        //Authenticate password
    }
    public static ChatMember Login(MemberService memberService)         //Login function
    {

        Console.Write("Enter username:");                               //Promt for userName
        string username = Console.ReadLine();

        Console.Write("Enter password:");                               //Promt for password
        string password = Console.ReadLine();
        
        // Validate the user's credentials
        ChatMember member = memberService.GetMemberByUsername(username);
        if (member != null && member.Authenticate(password))                    //Check if there is a match
        {
            Console.WriteLine($"Logged in as {member.Username}.");
            return member;
        }
        else
        {
            Console.WriteLine("Invalid username or password.");
            return null;
        }
    }
}

public class MemberService
{
    private List<ChatMember> Members { get; set; } = new List<ChatMember>();

    public ChatMember Register(string username, string realName, int age, string password)
    {
        ChatMember newMember = new ChatMember(username, realName, age, password);
        Members.Add(newMember);
        return newMember;
    }

    public ChatMember GetMemberByUsername(string username)
    {
        return Members.FirstOrDefault(m => m.Username == username);
    }
}

public class MessageService
{
    private List<PrivateMessage> Messages { get; set; } = new List<PrivateMessage>();

    public void SendMessage(ChatMember sender, ChatMember receiver, string content, DateTime? constructor = null)
    {
        PrivateMessage newMessage = new PrivateMessage(sender, receiver, content, constructor);
        Messages.Add(newMessage);
    }

    public List<PrivateMessage> GetMessagesForUser(ChatMember member)
    {
        return Messages.Where(m => m.Receiver == member || m.Sender == member).ToList();
    }
    public List<(ChatMember sender, PrivateMessage message)> GetThreeRecentSendersWithMessages(ChatMember receiver)
    {
        // Fetch messages sent to the receiver
        var receivedMessages = Messages.Where(m => m.Receiver == receiver).ToList();

        // Group by sender and order by most recent message timestamp
        var groupedBySender = receivedMessages
            .GroupBy(m => m.Sender)
            .Select(g => new { Sender = g.Key, LatestMessage = g.OrderByDescending(msg => msg.SentTime).First() })
            .OrderByDescending(g => g.LatestMessage.SentTime)
            .Take(3)
            .Select(g => (g.Sender, g.LatestMessage))
            .ToList();

        return groupedBySender;
    }
}


public class PrivateMessage
{
    public ChatMember Sender { get; private set; }
    public ChatMember Receiver { get; private set; }
    public string Content { get; private set; }
    public DateTime SentTime { get; private set; }

    public PrivateMessage(ChatMember sender, ChatMember receiver, string content, DateTime? constructor = null)
    {
        Sender = sender;
        Receiver = receiver;
        Content = content;
        SentTime = constructor ?? DateTime.Now; //Uses Constructor first, otherwise it takes DateTime.
    }

    public string DisplayMessage()
    {
        return $"{Sender.Username} to {Receiver.Username} at {SentTime:HH:mm}: {Content}";
    }

 
}
