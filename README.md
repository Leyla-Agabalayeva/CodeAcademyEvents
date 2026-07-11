# CodeAcademy Events

A web app for managing events end-to-end: creating events, inviting guests, tracking RSVPs, checking people in at the door, and collecting feedback afterward. Built with ASP.NET Core MVC on .NET 8, split into three projects (UI / BLL / DAL) following a fairly standard layered architecture.

This started as a course project at CodeAcademy, so don't expect enterprise-grade polish everywhere — but the core flow works end to end, including real email delivery.

## What it does

- Admins create events, locations, event types, and organizers.
- Admins invite registered users to specific events. The invited person gets an email and can accept or decline.
- Once accepted, the guest gets a check-in number they can use at the event.
- After the event, guests can leave a rating and a comment.
- Each event page shows quick stats — how many were invited, how many accepted/rejected, how many checked in, and the average rating.
- Registration, login, password reset, and account confirmation all go through ASP.NET Core Identity, with actual emails sent via SMTP (not just fake confirmation screens).

## Stack

- ASP.NET Core MVC, .NET 8
- EF Core + SQL Server
- ASP.NET Core Identity for auth (roles: Admin / regular user)
- AutoMapper between entities and DTOs
- MailKit for sending mail (the built-in `System.Net.Mail.SmtpClient` kept failing against Gmail's TLS handshake, so this project uses MailKit instead)
- Bootstrap 5 + a custom CSS theme on top, applied everywhere including the built-in Identity pages

## Project layout

```
CodeAcademyEvents.UI/     controllers, Razor views, Identity pages
CodeAcademyEvents.BLL/    services, DTOs, AutoMapper profiles
CodeAcademyEvents.DAL/    EF Core DbContext, entities, repositories
```

Pretty standard split — DAL only knows about the database, BLL has the actual business rules and talks to DAL through a repository/unit-of-work pattern, UI only talks to BLL through interfaces.

## Running it locally

You'll need the .NET 8 SDK and a SQL Server instance (LocalDB is fine).

1. Clone the repo and restore packages:

   ```bash
   git clone https://github.com/<your-username>/CodeAcademyEvents.git
   cd CodeAcademyEvents
   dotnet restore
   ```

2. Set your connection string in `CodeAcademyEvents.UI/appsettings.Development.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=CodeAcademyEventsApp;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;"
   }
   ```

3. Apply migrations:

   ```bash
   dotnet ef database update --project CodeAcademyEvents.DAL --startup-project CodeAcademyEvents.UI
   ```

4. Set up SMTP so registration/invitation emails actually send. Same file, `SmtpSettings` section:

   ```json
   "SmtpSettings": {
     "Host": "smtp.gmail.com",
     "Port": 587,
     "Username": "your-email@gmail.com",
     "Password": "your-app-password",
     "From": "your-email@gmail.com"
   }
   ```

   If you're using Gmail: your normal account password won't work here anymore. You need 2-Step Verification turned on, then generate an App Password at myaccount.google.com/apppasswords and use that instead. Any other SMTP provider (Brevo, Mailtrap for testing, etc.) works too — just swap the host/port/credentials, nothing in the code needs to change.

5. Run it:

   ```bash
   dotnet run --project CodeAcademyEvents.UI
   ```

On first run the app seeds an admin account automatically:

- **Email:** admin@codeacademy.az
- **Password:** Admin123!

This is hard-coded in `DbInitializer.cs` for convenience while developing — change it (or move it out of source entirely) before deploying anywhere real.

## Who can do what

Regular visitors can browse events and register an account. Once logged in, a user can respond to invitations, check in, and leave feedback — but they can't create events or invite anyone. Only Admin accounts can manage events/locations/types/organizers and send invitations.

One implementation detail worth knowing: there's no separate "guest sign-up" step — the first time a logged-in user leaves feedback or gets invited, a matching `Person` record is created for them automatically behind the scenes.

## Typical flow, if you want to try it end to end

1. Log in as admin, create a location, an event type, an organizer, then an event.
2. Register a normal (non-admin) account and confirm the email.
3. Log back in as admin, open the event, invite that user from the dropdown.
4. Log in as the invited user, go to "My Invitations," accept it — you'll get a check-in number.
5. Use that number on the Check-in page to simulate arriving at the event.
6. Leave a rating/comment from the event page.
7. Check the event page again — the stats block updates with the new numbers.

## Known rough edges

- The seeded admin password is sitting in plain text in the source. Fine for a class project, not fine for anything public — rotate it or pull it out into config/secrets first.
- SMTP credentials in `appsettings.Development.json` are also plain text. Use user-secrets or environment variables if this ever goes further than local development.
- No automated tests yet.

## License

Whatever you want — add one here if you're publishing this.
