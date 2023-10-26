using System;
using System.Collections.Generic;
using System.Linq;



public class ChatMember
{
    public string Username { get; private set; }
    public string RealName { get; private set; }
    public int Age { get; private set; }
    public string MemberID { get; private set; }
    private string Password { get; set; }           // Private because of reasons

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
        return Password == password;
    }
    public static ChatMember Login(MemberService memberService)
    {

        Console.Write("Enter username:");
        string username = Console.ReadLine();

        Console.WriteLine("Enter password:");
        string password = Console.ReadLine();

        // Validate the user's credentials
        ChatMember member = memberService.GetMemberByUsername(username);
        if (member != null && member.Authenticate(password))
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

    public void SendMessage(ChatMember sender, ChatMember receiver, string content)
    {
        PrivateMessage newMessage = new PrivateMessage(sender, receiver, content);
        Messages.Add(newMessage);
    }

    public List<PrivateMessage> GetMessagesForUser(ChatMember member)
    {
        return Messages.Where(m => m.Receiver == member || m.Sender == member).ToList();
    }
}


public class PrivateMessage
{
    public ChatMember Sender { get; private set; }
    public ChatMember Receiver { get; private set; }
    public string Content { get; private set; }
    public DateTime SentTime { get; private set; }

    public PrivateMessage(ChatMember sender, ChatMember receiver, string content)
    {
        Sender = sender;
        Receiver = receiver;
        Content = content;
        SentTime = DateTime.Now; // Assigns the current DateTime when the message is created
    }

    public string DisplayMessage()
    {
        return $"{Sender.Username} to {Receiver.Username} at {SentTime:HH:mm}: {Content}";
    }

 
}
