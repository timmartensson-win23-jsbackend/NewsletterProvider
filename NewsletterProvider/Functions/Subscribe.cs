using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace NewsletterProvider.Functions
{
    public class Subscribe(ILogger<Subscribe> logger, DataContext context)
    {
        private readonly ILogger<Subscribe> _logger = logger;
        private readonly DataContext _context = context;

        [Function("Subscribe")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();

            _logger.LogInformation("Received request body: " + body);

            try
            {
                if (!string.IsNullOrEmpty(body))
                {
                    var subscribeEntity = JsonConvert.DeserializeObject<SubscribeEntity>(body);
                    if (subscribeEntity != null)
                    {
                        _logger.LogInformation("Deserialized subscribeEntity: " + JsonConvert.SerializeObject(subscribeEntity));
                        var existingSubscriber = await _context.Subscribe.FirstOrDefaultAsync(s => s.Email == subscribeEntity.OldEmail);
                        if (existingSubscriber != null)
                        {
                            _logger.LogInformation("Found existing subscriber with email: " + subscribeEntity.OldEmail);
                            _context.Subscribe.Remove(existingSubscriber);
                            await _context.SaveChangesAsync();

                            _context.Subscribe.Add(subscribeEntity);
                            await _context.SaveChangesAsync();
                            return new OkObjectResult(new { status = 200, message = "Subscriber was updated" });
                        }

                        _logger.LogInformation("No existing subscriber found, adding new subscriber");
                        _context.Subscribe.Add(subscribeEntity);
                        await _context.SaveChangesAsync();
                        return new OkResult();
                    }
                }
            }
            catch (Exception ex)
            { _logger.LogError($"ERROR : Subscribe.Run() :: {ex.Message}"); }
           
            return new BadRequestObjectResult(new { status = 400, message = "Unable to subscribe right now" });
        }

        [Function("GetAllEmails")]
        public async Task<IActionResult> GetAllEmailsAsync([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            try
            {
                var emails = await _context.Subscribe.Select(s => s.Email).ToListAsync();
                return new OkObjectResult(emails);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR : Subscribe.GetAllEmails() :: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
    }
}
