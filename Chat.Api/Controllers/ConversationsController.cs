using Chat.Core.Managers;
using Chat.Core.Models;
using Identity.Core.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConversationsController : ControllerBase
{
    private readonly ConversationManager _conversationManager;
    private readonly UserProvider _userProvider;

    public ConversationsController(ConversationManager conversationManager, UserProvider userProvider)
    {
        _conversationManager = conversationManager;
        _userProvider = userProvider;
    }

    [HttpGet]
    public async Task<List<ConversationModel>> GetConversations()
    {
        return await _conversationManager.GetConversations(_userProvider.UserId);
    }

    [HttpGet("{converId}")]
    public async Task<List<MessageModel>> GetConversationMessages(Guid converId)
    {
        return await _conversationManager.GetConversationMessages(converId);
    }

    [HttpPost]
    public async Task SaveMessages(NewMessageModel newMessageModel)
    {
        await _conversationManager.SaveMessages(_userProvider.UserId, newMessageModel); 
    }
}
