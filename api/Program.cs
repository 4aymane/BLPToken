using System.Numerics;
using System.Text;
using api.Data;
using api.DTOs;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var jwtKey = builder.Configuration["JwtKey"] ?? "hf8Ia7YNFGqDhf8Ia7YNFGqDhf8Ia7YNFGqDhf8Ia7YNFGqD";
var issuer = "BLPIssuer";
var audience = "BLPAudience";

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDatabase")
);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please insert JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        }
    );

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        }
    );
});

builder.Services.AddSingleton(new JwtService(jwtKey, issuer, audience)); // Register JwtService
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Define Minimal API Endpoints

app.MapPost(
    "/login",
    (UserLoginDto user, JwtService jwtService) =>
    {
        var token = jwtService.GenerateJwtToken();
        return Results.Ok(new { token });
    }
);

app.MapGet(
    "/token",
    (ApplicationDbContext context) =>
    {
        var token = context.Tokens.FirstOrDefault();
        if (token == null)
        {
            return Results.NotFound("Token data not found.");
        }

        return Results.Ok(
            new
            {
                id = token.Id,
                name = token.Name,
                totalSupply = token.TotalSupply.ToString(),
                circulatingSupply = token.CirculatingSupply.ToString()
            }
        );
    }
);

app.MapPost(
    "/token",
    [Authorize]
    async (ApplicationDbContext context) =>
    {
        try
        {
            var totalSupply = await TokenContractService.GetTotalSupplyAsync();
            var tokenName = await TokenContractService.GetTokenNameAsync();

            BigInteger nonCirculatingTokens = 0;
            var addresses = new List<string>
            {
                "0x000000000000000000000000000000000000dEaD",
                "0xe9e7CEA3DedcA5984780Bafc599bD69ADd087D56",
                "0xfE1d7f7a8f0bdA6E415593a2e4F82c64b446d404",
                "0x71F36803139caC2796Db65F373Fb7f3ee0bf3bF9",
                "0x62D6d26F86F2C1fBb65c0566Dd6545ae3F9A63F1",
                "0x83a7152317DCfd08Be0F673Ab614261b4D1e1622",
                "0x5A749B82a55f7d2aCEc1d71011442E221f55A537",
                "0x9eBbBE47def2F776D6d2244AcB093AB2fD1B2C2A",
                "0xcdD80c6F317898a8aAf0ec7A664655E25E4833a2",
                "0x456F20bb4d89d10A924CE81b7f0C89D5711CE05B"
            };

            foreach (var address in addresses)
            {
                var balance = await TokenContractService.GetTokenBalanceFromBlockchain(address);
                nonCirculatingTokens += balance;
            }

            var circulatingSupply = totalSupply - nonCirculatingTokens;

            var existingToken = context.Tokens.FirstOrDefault(t => t.Name == tokenName);
            if (existingToken != null)
            {
                existingToken.TotalSupply = totalSupply;
                existingToken.CirculatingSupply = circulatingSupply;
            }
            else
            {
                var newToken = new Token
                {
                    Name = tokenName,
                    TotalSupply = totalSupply,
                    CirculatingSupply = circulatingSupply
                };
                context.Tokens.Add(newToken);
            }

            await context.SaveChangesAsync();
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
);

app.Run();
