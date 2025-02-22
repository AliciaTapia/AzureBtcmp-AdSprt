using ContosoAdsSupport;
using ContosoSupport.Models;
using ContosoSupport.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Cloud.InstrumentationFramework;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContosoSupport.Controllers
{
    [Route("{subscriptionId}/{resourceGroup}/{resourceId}/cases")]
    [ApiController]
    public class SupportCasesController : ControllerBase
    {
        private const string idTemplate = "{id:length(24)}";
        private const string fail = "fail";

        private readonly ISupportService supportService;
        private readonly ILogger logger;

        public SupportCasesController(ISupportService supportService, ILogger<SupportCasesController> logger)
        {
            this.supportService = supportService;
            this.logger = logger;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<SupportCase>), 200)]
        public async Task<IActionResult> GetSupportCasesAsync(string subscriptionId, string resourceGroup, string resourceId, int? pageNumber = 1)
        {
            // Create extended operation with PartC data from schema
            using var operation = new ExtendedOperation<EntityAccess>("GetEntitiesByPageNumber")
            {
                ApiType = OperationApiType.InternalCall,
                ResourceType = "Contoso.Support/ticketingSystem",
                ResourceId = $"subscriptions/{subscriptionId}/resourceGroups/{resourceGroup}/providers/Contoso.Support/ticketingSystem/{resourceId}",
                PartC = new EntityAccess()
                {
                    entityType = typeof(SupportCase).Name,
                    filter = "null",
                    entityId = "N/A",
                    pageNumber = pageNumber.Value,
                    accessType = AccessType.read
                }
            };

            IEnumerable<SupportCase> supportCases = null;

            try
            {
                supportCases = await supportService.GetAsync(pageNumber).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Log error
                operation.PartC.response = 500;
                operation.SetResult(ex);
                throw;
            }

            if (null == supportCases)
            {
                // Set operation result of ClientError
                operation.PartC.response = 404;
                operation.SetResult(OperationResult.ClientError);
                return NotFound();
            }

            // Set operation result of Success
            operation.PartC.response = 200;
            operation.SetResult(OperationResult.Success);

            return Ok(supportCases);
        }

        [HttpGet(idTemplate)]
        [ProducesResponseType(typeof(SupportCase), 200)]
        public async Task<IActionResult> GetSupportCaseAsync(string subscriptionId, string resourceGroup, string resourceId, string id)
        {

            // Create extended operation with PartC data from schema
            using var operation = new ExtendedOperation<EntityAccess>("GetEntityById")
            {
                ApiType = OperationApiType.InternalCall,
                ResourceType = "Contoso.Support/ticketingSystem",
                ResourceId = $"subscriptions/{subscriptionId}/resourceGroups/{resourceGroup}/providers/Contoso.Support/ticketingSystem/{resourceId}",
                PartC = new EntityAccess()
                {
                    entityType = typeof(SupportCase).Name,
                    filter = "null",
                    entityId = id,
                    pageNumber = -1,
                    accessType = AccessType.read
                }
            };

            SupportCase supportCase = null;

            try
            {
                supportCase = await supportService.GetAsync(id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Log error
                operation.PartC.response = 500;
                operation.SetResult(ex);
                throw;
            }

            if (null == supportCase)
            {
                // Set operation result of ClientError
                operation.PartC.response = 404;
                operation.SetResult(OperationResult.ClientError);
                return NotFound();
            }

            // Set operation result of Success
            operation.PartC.response = 200;
            operation.SetResult(OperationResult.Success);
            return Ok(supportCase);

        }

        // TODO:  should provide a location header to the item
        [HttpPost()]
        public IActionResult PostSupportCaseAsync(string subscriptionId, string resourceGroup, string resourceId, [FromBody] SupportCase supportCase)
        {
            // Create extended operation with PartC data from schema
            using var operation = new ExtendedOperation<EntityAccess>("CreateEntity")
            {
                ApiType = OperationApiType.InternalCall,
                ResourceType = "Contoso.Support/ticketingSystem",
                ResourceId = $"subscriptions/{subscriptionId}/resourceGroups/{resourceGroup}/providers/Contoso.Support/ticketingSystem/{resourceId}",
                PartC = new EntityAccess()
                {
                    entityType = typeof(SupportCase).Name,
                    filter = "null",
                    entityId = "N/A",
                    pageNumber = -1,
                    accessType = AccessType.create
                }
            };

            try
            {
                supportService.CreateAsync(supportCase);
            }
            catch (Exception ex)
            {
                // Log error
                operation.PartC.response = 500;
                operation.SetResult(ex);
                throw;
            }

            // Set operation result of Success
            operation.PartC.response = 202;
            operation.SetResult(OperationResult.Success);

            return Accepted();
        }

        [HttpPut(idTemplate)]
        public async Task<IActionResult> UpdateSupportCaseAsync(string subscriptionId, string resourceGroup, string resourceId, string id, SupportCase supportCaseIn)
        {
            // Create extended operation with PartC data from schema
            using var operation = new ExtendedOperation<EntityAccess>("UpdateEntityById")
            {
                ApiType = OperationApiType.InternalCall,
                ResourceType = "Contoso.Support/ticketingSystem",
                ResourceId = $"subscriptions/{subscriptionId}/resourceGroups/{resourceGroup}/providers/Contoso.Support/ticketingSystem/{resourceId}",
                PartC = new EntityAccess()
                {
                    entityType = typeof(SupportCase).Name,
                    filter = "null",
                    entityId = id,
                    pageNumber = -1,
                    accessType = AccessType.update
                }
            };

            SupportCase supportCase = null;

            try
            {
                supportCase = await supportService.GetAsync(id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Log error
                operation.PartC.response = 500;
                operation.SetResult(ex);
                throw;
            }

            if (null == supportCase)
            {
                // Set operation result of ClientError
                operation.PartC.response = 404;
                operation.SetResult(OperationResult.ClientError);
                return NotFound();
            }

            try
            {
                supportService.UpdateAsync(id, supportCaseIn);
            }
            catch (Exception ex)
            {
                // Log error
                operation.PartC.response = 500;
                operation.SetResult(ex);
                throw;
            }

            // Set operation result of Success
            operation.PartC.response = 200;
            operation.SetResult(OperationResult.Success);
            return Accepted();
        }

        [HttpDelete(idTemplate)]
        public async Task<IActionResult> DeleteSupportCaseAsync(string subscriptionId, string resourceGroup, string resourceId, string id)
        {
            // Create extended operation with PartC data from schema
            using var operation = new ExtendedOperation<EntityAccess>("DeleteEntityById")
            {
                ApiType = OperationApiType.InternalCall,
                ResourceType = "Contoso.Support/ticketingSystem",
                ResourceId = $"subscriptions/{subscriptionId}/resourceGroups/{resourceGroup}/providers/Contoso.Support/ticketingSystem/{resourceId}",
                PartC = new EntityAccess()
                {
                    entityType = typeof(SupportCase).Name,
                    filter = "null",
                    entityId = id,
                    pageNumber = -1,
                    accessType = AccessType.delete
                }
            };

            SupportCase supportCase = null;

            try
            {
                supportCase = await supportService.GetAsync(id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Log error
                operation.PartC.response = 500;
                operation.SetResult(ex);
                throw;
            }

            if (null == supportCase)
            {
                // Set operation result of ClientError
                operation.PartC.response = 404;
                operation.SetResult(OperationResult.ClientError);
                return NotFound();
            }

            try
            {
                supportService.RemoveAsync(supportCase.Id);
            }
            catch (Exception ex)
            {
                // Log error
                operation.PartC.response = 500;
                operation.SetResult(ex);
                throw;
            }

            // Set operation result of Success
            operation.PartC.response = 200;
            operation.SetResult(OperationResult.Success);
            return Ok();
        }

        [HttpGet(fail)]
        public IActionResult GetFail500(string subscriptionId, string resourceGroup, string resourceId, string id)
        {
            Task.Delay(4000).Wait();
            // Create extended operation with PartC data from schema
            using var operation = new ExtendedOperation<EntityAccess>("FailonPurposeOOPS")
            {
                ApiType = OperationApiType.InternalCall,
                ResourceType = "Contoso.Support/ticketingSystem",
                ResourceId = $"subscriptions/{subscriptionId}/resourceGroups/{resourceGroup}/providers/Contoso.Support/ticketingSystem/{resourceId}",
                PartC = new EntityAccess()
                {
                    entityType = typeof(SupportCase).Name,
                    filter = "null",
                    entityId = id,
                    pageNumber = -1,
                    accessType = AccessType.read,
                    response = 500,
                }
            };
            operation.SetResult(OperationResult.Failure);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

    }
}