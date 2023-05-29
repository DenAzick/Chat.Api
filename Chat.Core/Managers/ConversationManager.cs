using Chat.Core.Context;
using Chat.Core.Entities;
using Chat.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.Core.Managers;

public class ConversationManager
{
    private readonly ChatDbContext _context;

    public ConversationManager(ChatDbContext context)
    {
        _context = context;
    }

    public async Task<List<ConversationModel>> GetConversations(Guid userId)
    {
        var conversations = await _context.Conversations
            .Where(c => c.UserIds.Contains(userId)).ToListAsync();

        return conversations.Select(c => new ConversationModel()
        {
            FromUserId = c.UserIds.First(u => u != userId),
            Id = c.Id,
        }).ToList();
    }

    public async Task<List<MessageModel>> GetConversationMessages(Guid conversationId)
    {
        var messages = await _context.Messages
            .Where(m => m.ConversationId == conversationId).ToListAsync();

        return messages.Select(message => new MessageModel()
        {
            FromUserId = message.FromUserId,
            Id = message.Id,
            Date = message.DateTime,
            Text = message.Text,
        }).ToList();
    }

    public async Task SaveMessages(Guid userId, NewMessageModel newMessageModel)
    {
        var conversation = await _context.Conversations
            .Where(c => c.UserIds
            .Contains(userId) &&
            c.UserIds.Contains(newMessageModel.ToUserId))
            .FirstOrDefaultAsync();


        if (conversation == null)
        {
            conversation = new Conversation()
            {
                UserIds = new List<Guid> { userId, newMessageModel.ToUserId }
            };
        }

        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync();


        var message = new Message()
        { 
            ConversationId = conversation.Id,
            DateTime = DateTime.Now,
            FromUserId = userId,
            Text = newMessageModel.Text,
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

    }

}
