namespace API.Extensions;

public static class BuilderExtensions
{
    public static void AddArchitecture(this WebApplicationBuilder builder)
    {
        builder.AddDatabase();
        builder.Services.AddHttpContextAccessor();
        
        var jwtKey = builder.Configuration["Jwt:Key"] 
                     ?? throw new ArgumentNullException("Jwt:Key");
        var keyBytes = Encoding.ASCII.GetBytes(jwtKey);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        builder.Services.AddAuthorization();
        builder.AddCorsPolicy();
        builder.AddRateLimiting();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API DevFlow", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Bearer {token}"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            });
        });
        
        builder.Services.AddHttpClient(); 
        builder.Services.AddHealthChecks();
    }

    public static void AddMyServices(this IServiceCollection services)
    {
        services.AddScoped<IPedidoService, PedidoService>();
        services.AddScoped<IFerramentasService, FerramentasService>();
        services.AddScoped<IColecaoService, ColecaoService>();
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<TokenService>();

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IColecaoRepository, ColecaoRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
    }

    private static void AddDatabase(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                b => b.MigrationsAssembly("API")
            ));
    }

    private static void AddCorsPolicy(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options => 
            options.AddPolicy("PermitirTudo", b =>
                b.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            ));
    }

    private static void AddRateLimiting(this WebApplicationBuilder builder)
    {
        builder.Services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("default", c =>
            {
                c.Window = TimeSpan.FromSeconds(10);
                c.PermitLimit = 30;
                c.QueueLimit = 2;
            });
        });
    }
}