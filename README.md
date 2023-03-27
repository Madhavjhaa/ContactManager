# Contact Manager Project

The Contact Manager Project is a RESTful web API built with ASP.NET Core that allows users to manage their contacts. Users can perform CRUD (Create, Read, Update, Delete) operations on their contacts using this API.

## Prerequisites

- .NET 6 SDK or later
- PostgreSQL

## Database Setup

The project uses a PostgreSQL database to store contact information. The database schema is defined in the `Contacts` table, which has the following columns:

- `Id` (integer, primary key): unique identifier for the contact
- `Salutation` (text, not null): title or form of address for the contact
- `FirstName` (text, not null): first name of the contact
- `LastName` (text, not null): last name of the contact
- `DisplayName` (text): name to display for the contact (defaults to concatenation of `Salutation`, `FirstName`, and `LastName`)
- `Birthdate` (date): date of birth for the contact
- `CreationTimestamp` (timestamp, not null): timestamp of when the contact was created
- `LastChangeTimestamp` (timestamp): timestamp of when the contact was last updated
- `NotifyHasBirthdaySoon` (boolean): whether or not to notify the user when the contact has a birthday soon
- `Email` (text, not null, unique): email address of the contact
- `PhoneNumber` (text): phone number of the contact

The project includes SQL scripts to create the `Contacts` table, as well as functions and triggers to update the `DisplayName`, `LastChangeTimestamp`, and `NotifyHasBirthdaySoon` columns automatically.

```sql
CREATE TABLE Contacts (
  Id SERIAL PRIMARY KEY,
  Salutation TEXT NOT NULL CHECK (length(Salutation) > 2),
  FirstName TEXT NOT NULL CHECK (length(FirstName) > 2),
  LastName TEXT NOT NULL CHECK (length(LastName) > 2),
  DisplayName TEXT DEFAULT '',
  Birthdate DATE,
  CreationTimestamp TIMESTAMP NOT NULL DEFAULT NOW(),
  LastChangeTimestamp TIMESTAMP,
  NotifyHasBirthdaySoon BOOLEAN DEFAULT FALSE,
  Email TEXT NOT NULL UNIQUE,
  PhoneNumber TEXT
);

CREATE OR REPLACE FUNCTION update_display_name()
RETURNS TRIGGER AS $$
BEGIN
  IF NEW.DisplayName = '' THEN
    NEW.DisplayName := NEW.Salutation || ' ' || NEW.FirstName || ' ' || NEW.LastName;
  END IF;
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_display_name
BEFORE INSERT ON Contacts
FOR EACH ROW
EXECUTE FUNCTION update_display_name();

CREATE OR REPLACE FUNCTION update_last_change_timestamp()
RETURNS TRIGGER AS $$
BEGIN
  NEW.LastChangeTimestamp := NOW();
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_last_change_timestamp
BEFORE UPDATE ON Contacts
FOR EACH ROW
EXECUTE FUNCTION update_last_change_timestamp();

CREATE OR REPLACE FUNCTION update_notify_birthday_soon()
RETURNS TRIGGER AS $$
BEGIN
  IF NEW.Birthdate IS NOT NULL THEN
    IF date_trunc('year', age(NEW.Birthdate)) != date_trunc('year', age(NEW.Birthdate - interval '14 days')) THEN
      NEW.NotifyHasBirthdaySoon := TRUE;
    ELSE
      NEW.NotifyHasBirthdaySoon := FALSE;
    END IF;
  ELSE
    NEW.NotifyHasBirthdaySoon := FALSE;
  END IF;
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_notify_birthday_soon
BEFORE INSERT OR UPDATE ON Contacts
FOR EACH ROW
EXECUTE FUNCTION update_notify_birthday_soon();

INSERT INTO Contacts (Id, Salutation, FirstName, LastName, Email, Birthdate)
VALUES
  (1, 'Herr', 'Niko', 'Laus', 'niko@laus.com', '2002-02-10'),
  (2, 'Frau', 'Martina', 'Martell', 'martina@martell.com', '1968-03-30'),
  (3, 'Dr.', 'Robert', 'Pfeiffer', 'robert@pfeiffer.com', '1989-03-18'),
  (4, 'Dr.', 'John', 'Schnee', 'john@schnee.com', CURRENT_DATE - interval '1 year' + interval '5 day'),
  (5, 'Herr', 'Michael', 'Henker', 'michael@henker.com', '1971-07-07');

SELECT * FROM Contacts;
```

## Project Structure

The project has been structured into the following folders:

- `ContactManager`: Contains the main project files.
- `DataAccessLibrary`: Contains the data access layer for the project.
- `ContactManager.DependencyInjection`: Contains the dependency injection configuration for the project.

## API Endpoints

The following API endpoints are available:

- `GET /api/contact`: Returns a list of all contacts.
- `GET /api/contact/{contactId}`: Returns a single contact by ID.
- `POST /api/contact`: Creates a new contact.
- `PUT /api/contact`: Updates an existing contact.
- `DELETE /api/contact/{contactId}`: Deletes a contact by ID.

## Dependencies

The following NuGet packages are used in this project:

- Microsoft.AspNetCore.App
- Npgsql
- Dapper
- Swashbuckle.AspNetCore

## How to Run the Project

To run the project, follow these steps:

1. Clone the project to your local machine.
2. Open the project in Visual Studio 2019 or later.
3. Ensure that you have PostgreSQL installed and running on your machine.
4. Open the `appsettings.json` file and update the connection string to point to your PostgreSQL instance.
5. Run the project. The API will be hosted on `http://localhost:5000`.

## API Documentation

The API documentation is generated using Swagger. To access the documentation, run the project and navigate to `http://localhost:5000/swagger/index.html` in your web browser. This will display the Swagger UI, which provides documentation for all the API endpoints.
