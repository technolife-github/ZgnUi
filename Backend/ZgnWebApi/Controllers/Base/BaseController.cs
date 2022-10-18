using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq.Expressions;
using ZgnWebApi.Core.Entities;

namespace ZgnWebApi.Controllers.Base
{
    public abstract class BaseController<T> : SecureController
        where T : class, IEntity<T>, new()
    {

        [HttpGet("GetAll")]
        public virtual IActionResult GetAll()
        {
            var result = new T().GetAll(null);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("GetById/{id}")]
        public virtual IActionResult GetById(int id)
        {
            var result = new T().Get(e => e.Id == id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("Add")]
        public virtual IActionResult Add(T entity)
        {
            var result = entity.Add();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut("Update")]
        public virtual IActionResult Update(T entity)
        {
            var result = entity.Update();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("Delete")]
        public virtual IActionResult Delete(T entity)
        {
            var result = entity.Delete();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
