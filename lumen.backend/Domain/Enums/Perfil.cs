namespace Domain.Enums;

public enum Perfil
{
    [Description("Administrador")]
    Admin = 0,

    [Description("Usuário")]
    Usuario = 1,

    [Description("Convidado")]
    Convidado = 2
}
