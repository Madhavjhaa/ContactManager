# ContactManager

Postgres was used as Database:
```
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
