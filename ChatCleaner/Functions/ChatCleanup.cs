using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ChatCleaner.Data.Contexts;

namespace ChatCleaner.Functions
{
    public class ChatCleanup
    {
        private readonly DataContext _context;
        private readonly ILogger<ChatCleanup> _logger;

        public ChatCleanup(DataContext context, ILogger<ChatCleanup> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Run every day at midnight (UTC)
        [Function("ChatCleanupFunction")]
        public async Task Run([TimerTrigger("0 0 0 * * *")] TimerInfo myTimer)
        {
            var currentUtcTime = DateTime.UtcNow;
            _logger.LogInformation($"Chat cleanup function executed at (UTC): {currentUtcTime}");

            // Delete messages older than 24 hours
            var cutoffDate = currentUtcTime.AddHours(-24);
            _logger.LogInformation($"Cutoff date for deletion (UTC): {cutoffDate}");

            var oldMessages = _context.ChatMessages.Where(m => m.Created < cutoffDate).ToList();
            if (oldMessages.Any())
            {
                _context.ChatMessages.RemoveRange(oldMessages);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Deleted {oldMessages.Count} old messages.");
            }
            else
            {
                _logger.LogInformation("No old messages found for deletion.");
            }
        }
    }
}
