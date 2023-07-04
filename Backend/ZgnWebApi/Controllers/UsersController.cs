using Microsoft.AspNetCore.Mvc;
using ZgnWebApi.Controllers.Base;
using ZgnWebApi.Core.Utilities.Helpers;
using ZgnWebApi.Entities;

namespace ZgnWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : PageableController<User>
    {
        public UsersController()
        {
            base.AddRole("User.GetAll");
            base.AddRole("User.Get");
            base.AddRole("User.Add");
            base.AddRole("User.Update");
            base.AddRole("User.Delete");
            base.AddRole("User.SoftDelete");
        }
        public override IActionResult GetAll()
        {
            base.CheckRole("User.GetAll");
            return base.GetAll();
        }
        public override IActionResult GetById(int id)
        {
            base.CheckRole("User.Get");
            return base.GetById(id);
        }
        public override IActionResult Add(User entity)
        {
            base.CheckRole("User.Add");
            return base.Add(entity);
        }
        public override IActionResult Update(User entity)
        {
            base.CheckRole("User.Update");
            return base.Update(entity);
        }
        public override IActionResult Delete(User entity)
        {
            base.CheckRole("User.Delete");
            return base.Delete(entity);
        }
        public override IActionResult GetAllForUi(string? request)
        {
            base.CheckRole("User.GetAll");
            return base.GetAllForUi(request);
        }
        public override IActionResult GetForUi([FromForm] HandleRequestData data)
        {
            base.CheckRole("User.Get");
            return base.GetForUi(data);
        }
        public override IActionResult AddForUi([FromForm] HandleRequestData data)
        {
            base.CheckRole("User.Add");
            return base.AddForUi(data);
        }
        public override IActionResult UpdateForUi([FromForm] HandleRequestData data)
        {
            base.CheckRole("User.Update");
            return base.UpdateForUi(data);
        }
        public override IActionResult DeleteForUi(string request)
        {
            base.CheckRole("User.Delete");
            return base.DeleteForUi(request);
        }
        public override IActionResult SoftDeleteForUi(string request)
        {
            base.CheckRole("User.SoftDelete");
            return base.SoftDeleteForUi(request);
        }
    }

}
