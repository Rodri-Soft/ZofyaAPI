using Microsoft.AspNetCore.Mvc;
using ZofyaApi.Models;
using ZofyaApi.ModelValidations;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ZofyaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManagementController : ControllerBase
    {

        private ZofyaContext dbContext;
        private Log log = new Log();

        public ManagementController(ZofyaContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("/PostFindStaff")]
        public staff? PostFindStaff(AuxiliaryUser auxiliaryUser)
        {
            return dbContext.staff.Where(s => s.Email == auxiliaryUser.Email &&
                                         s.Password == auxiliaryUser.Password).FirstOrDefault();
        }    

        [HttpPost]
        [Route("/PostFindStaffEmail")]
        public staff? PostFindStaffEmail(IDResult idEmail)
        {
            return dbContext.staff.Where(s => s.Email == idEmail.ID).FirstOrDefault();
        }        
        



    }
}

