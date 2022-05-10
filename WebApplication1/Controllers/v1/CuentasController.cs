using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.DTOs;
using WebApplication1.Servicios;

namespace WebApplication1.Controllers.v1
{
    [ApiController]
    [Route("api/v1/cuentas")]
    public class CuentasController:ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly HashService hashService;
        private readonly IDataProtector dataProtector;

        public CuentasController(UserManager<IdentityUser> userManager, IConfiguration configuration,
            SignInManager<IdentityUser> signInManager,IDataProtectionProvider dataProtectionProvider,
            HashService hashService)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.hashService = hashService;
            dataProtector =dataProtectionProvider.CreateProtector("valor_unico_secreto");
        }

        [HttpGet("hash/{textoplano}")]
        public ActionResult Hash(string textoplano)
        {
            var resultado1=hashService.Hash(textoplano);
            var resultado2 = hashService.Hash(textoplano);

            return Ok(new
            {
                textoplano=textoplano,
                Has1=resultado1,
                Has2=resultado2,
            });

        }

        [HttpGet("encritar")]
        public ActionResult Encriptar()
        {
            var textoplano = "Edgar Bryan";
            var textocyfrado=dataProtector.CreateProtector(textoplano);
         //   var textDesifrado = dataProtector.Unprotect("s");
            return Ok(new
            {
                textoplano = textoplano,
                textocyfrado = textocyfrado,
       //         textDesifrado = textDesifrado
            });
        }


        [HttpPost("registrar",Name ="Registar")]

        public async Task<ActionResult<Respuestalogin>> Registrar(CredencialesDTOcs credencialesDTOcs)
        {
            var usuario= new IdentityUser { UserName=credencialesDTOcs.email,
                Email=credencialesDTOcs.email};
            var resultado = await userManager.CreateAsync(usuario,credencialesDTOcs.password);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialesDTOcs);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        [HttpPost("login",Name ="Login")]
        public async Task<ActionResult<Respuestalogin>> Logeo(CredencialesDTOcs credencialesDTOcs)
        {
            var resultado= await signInManager.PasswordSignInAsync(credencialesDTOcs.email,
                credencialesDTOcs.password,isPersistent:false,lockoutOnFailure:false);


            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialesDTOcs);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }

        }


        [HttpGet("renovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public  async Task<ActionResult<Respuestalogin>> RenovarToken()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;


            var credencialesUsuario = new CredencialesDTOcs()
            {
                email = email
            };
            return await ConstruirToken(credencialesUsuario);
        }


        private async Task<Respuestalogin> ConstruirToken(CredencialesDTOcs credencialesDTOcs)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credencialesDTOcs.email),

            };

            var usuario = await userManager.FindByEmailAsync(credencialesDTOcs.email);
            var claimsDB= await userManager.GetClaimsAsync(usuario);
            claims.AddRange(claimsDB);
            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llaveJWT"]));
            var creds= new SigningCredentials(llave,SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            var securityToken = new JwtSecurityToken(issuer:null,audience:null,claims:claims,expires:expiracion,signingCredentials:creds);

            return new Respuestalogin()
            {
                token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion
            };
        }


        [HttpPost("HacerAdmin")]
        public async Task<IActionResult> HacerAdmin(EditarAdmin editarAdmin)
        {
            var usuario= await userManager.FindByEmailAsync(editarAdmin.Email);

            await userManager.AddClaimAsync(usuario, new Claim("esAdmin", "1"));

            return NoContent();
        }


        [HttpPost("RemoverAdmin")]
        public async Task<IActionResult> RemoverAdmin(EditarAdmin editarAdmin)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdmin.Email);

            await userManager.RemoveClaimAsync(usuario, new Claim("esAdmin", "1"));

            return NoContent();
        }
    }
}
