using ProductManagementSystem.Data;
using ProductManagementSystem.Models;
using ProductManagementSystem.Helpers;
using Microsoft.EntityFrameworkCore;

public class AccountService
{
    private readonly ApplicationDbContext _context;

    public AccountService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> RegisterNewUser(Account model)
    {
        if (await _context.Kunde.AnyAsync(u => u.Email == model.Email || u.Benutzername == model.Benutzername))
        {
            return new OperationResult(false, "E-Mail oder Benutzername existiert bereits.");
        }

        if (model.Passwort == null)
        {
            return new OperationResult(false, "Das Passwort darf nicht leer sein.");
        }

        model.Passwort = HashHelper.HashPassword(model.Passwort);
        await _context.Kunde.AddAsync(model);
        await _context.SaveChangesAsync();

        return new OperationResult(true, "Registrierung erfolgreich. Sie können sich jetzt anmelden.");
    }


    public async Task<OperationResult> VerifyUserLogin(string loginCredential, string passwort)
    {
        var user = await _context.Kunde
            .FirstOrDefaultAsync(u => u.Email == loginCredential || u.Benutzername == loginCredential);

        if (user != null && user.Passwort != null && HashHelper.VerifyPassword(user.Passwort, passwort))
        {
            return new OperationResult(true, "Login erfolgreich.");
        }

        return new OperationResult(false, "Login fehlgeschlagen. Bitte überprüfen Sie Ihre Anmeldedaten.");
    }

}
