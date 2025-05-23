﻿
using Ecommerce.Application.Interfaces.AdminInterfaces;
using Ecommerce.Domain.Enuns;
using Ecommerce.Domain.Models.AdminModels;
using Ecommerce.Domain.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;

namespace E_commerce_WEB_API___Teste_técnico_Rota.Controllers.AdminControllers
{
    [Authorize]
    [ApiController]
    [Route("api/admin")]
    public class AdminCommandsController : ControllerBase
    {
        //Injeção
        private readonly IAdminProductInterface _adminProductInterface;
        private readonly IAdminTransactionInterface _adminTransactionInterface;
        private readonly IAdminUserInterface _adminUserInterface;

        public AdminCommandsController(IAdminProductInterface adminProductInterface, IAdminTransactionInterface adminTransactionInterface, IAdminUserInterface adminUserInterface)
        {
            _adminProductInterface = adminProductInterface;
            _adminTransactionInterface = adminTransactionInterface;
            _adminUserInterface = adminUserInterface;
        }

        [Authorize(Roles = UsersRoles.Admin)]
        [HttpPut("user/InactivateStatus")]
        public async Task<IActionResult> PutUserStatusInactive([FromBody]string Usermail)
        {
            var (status, message) = await _adminUserInterface.PutUserStatusToInactive(Usermail);

            if(status==false)
            {
                return BadRequest(message);
            }
            return Ok(message);
        }

        [Authorize(Roles = UsersRoles.Admin)]
        [HttpPut("user/ActivateStatus")]
        public async Task<IActionResult> PutUserStatusActive([FromBody] string Usermail)
        {
            var (status, message) = await _adminUserInterface.PutUserStatusToActive(Usermail);

            if (status == false)
            {
                return BadRequest(message);
            }
            return Ok(message);
        }

        [Authorize(Roles = UsersRoles.Admin)]
        [HttpPost("NewProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] AdminCreateProductModel model)
        {
            var (status, message) =await _adminProductInterface.PostProductAdmin(model);

            if (status == false)
            {
                return BadRequest(message);
            }
            return Ok(message);
        }

        [Authorize(Roles = UsersRoles.Admin)]
        [HttpPut("product/ChangeData/{idproduct}")]
        public async Task<IActionResult> PutProduct([FromRoute] int idproduct, [FromBody] AdminUpdateProductModel model)//Valor, nome, descrição e quantidade no estoque
        {

            var (status, message)=await _adminProductInterface.PutProductAdmin(idproduct, model);

            if (status == false)
            {
                return BadRequest(message);
            }
            return Ok();
        }


        [Authorize(Roles = UsersRoles.Admin)]
        [HttpPut("product/status/{idproduct}")]
        public async Task<IActionResult> PutProductStatus([FromRoute]int idproduct, [FromBody] ProductStatusEnum newStatus)
        {

            if (newStatus == ProductStatusEnum.Active)
            {
                var (status, message) = await _adminProductInterface.PutProductStatusToAtiveAdmin(idproduct);
            }
            else if (newStatus == ProductStatusEnum.Inactive)
            {
                var (status, message) = await _adminProductInterface.PutProductStatusToInativeAdmin(idproduct);
            }
            else
            {
                return BadRequest("Status inválido.");
            }

            return Ok(new {Message="Status modificado com sucesso." });
        }

        [Authorize(Roles = UsersRoles.Admin)]
        [HttpPut("product/category/{idproduct}")]
        public async Task<IActionResult> PutProductCategory([FromRoute]int idproduct, [FromBody] AdminUpdateProductCategoryModel category)
        {
            var CategoryExtracted=category.Category; 
            var Response= await _adminProductInterface.PutProductCategoryAdmin(idproduct, CategoryExtracted);

            if (Response.Item1 == false)
            {
                return BadRequest(Response.Item2);
            }

            return Ok(Response.Item2);
        }

        [Authorize(Roles = UsersRoles.Admin)]
        [HttpPut("product/newStock/{idproduct}")]
        public async Task<IActionResult> PutProductStockTotal([FromRoute]int idproduct, [FromBody] int newStock)
        {

            var Response = await _adminProductInterface.PutProductStockTotalAdmin(idproduct, newStock);

            if (Response.Item1 is false)
            {
                return BadRequest(Response.Item2);
            }
            return Ok(Response.Item2);
        }

        //quando a transação for cancelada
        [Authorize(Roles = UsersRoles.Admin)]
        [HttpPut("transaction/Canceled/{TransactionID}")]
        public async Task<IActionResult> PutTransactionStatusToCanceled([FromBody] int TransactionID)
        {
            var Response = await _adminTransactionInterface.PutTransactionStatusToCanceledAdmin(TransactionID);

            if(Response.Item1 is false)
            {
                return BadRequest(Response.Item2);
            }
            return Ok(Response.Item2);
        }
    } 
}
