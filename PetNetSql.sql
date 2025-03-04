create database PetNet1;
use PetNet1;

CREATE TABLE Owners (   
	id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) UNIQUE NOT NULL,
    password NVARCHAR(255) NOT NULL,
	image NVARCHAR(255),
    phone NVARCHAR(20),
	location NVARCHAR(20),
);
CREATE TABLE Clinics (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NOT NULL,
    location NVARCHAR(255) NOT NULL,
    phone NVARCHAR(20),
    email NVARCHAR(255),
	image NVARCHAR(255),
    description TEXT,
    working_hours NVARCHAR(255)
);
CREATE TABLE Vets (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) UNIQUE NOT NULL,
    password NVARCHAR(255) NOT NULL,
    phone NVARCHAR(20),
    location NVARCHAR(20),
    specialization VARCHAR(255),
	image NVARCHAR(255),
    experience_years INT,
    clinic_id INT FOREIGN KEY REFERENCES Clinics(id) ON DELETE SET NULL
);

CREATE TABLE Admins (
   id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) UNIQUE NOT NULL,
    password NVARCHAR(255) NOT NULL,

   
);

CREATE TABLE Pets (
    id INT PRIMARY KEY IDENTITY(1,1),
    owner_id INT FOREIGN KEY REFERENCES Owners(id) ON DELETE CASCADE, 
    name VARCHAR(255),
    species VARCHAR(50), 
    breed VARCHAR(100),
    gender VARCHAR(100),
    spayed VARCHAR(100),
    birthdate DATE,
    weight FLOAT,
	image NVARCHAR(255),
);


CREATE TABLE Services (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NOT NULL,
    description TEXT,
   
);
CREATE TABLE Clinic_Services (
    id INT PRIMARY KEY IDENTITY(1,1),
    clinic_id INT FOREIGN KEY REFERENCES Clinics(id) ON DELETE CASCADE,
    service_id INT FOREIGN KEY REFERENCES Services(id) ON DELETE CASCADE,
  
);
CREATE TABLE Service_Types (
    id INT PRIMARY KEY IDENTITY(1,1),
    service_id INT FOREIGN KEY REFERENCES Services(id) ON DELETE CASCADE,
    name NVARCHAR(255) NOT NULL, -- مثال: فحص عادي، فحص متقدم
    price DECIMAL(10,2) NOT NULL,
   
);

CREATE TABLE Appointments (
    id INT PRIMARY KEY IDENTITY(1,1),
    owner_id INT FOREIGN KEY REFERENCES Owners(id) ON DELETE CASCADE, 
    clinic_id INT FOREIGN KEY REFERENCES Clinics(id) ON DELETE CASCADE,
    service_type_id INT FOREIGN KEY REFERENCES Service_Types(id) ON DELETE CASCADE,
    appointment_date DATETIME NOT NULL,
	appointment_time DATETIME NOT NULL,
    status NVARCHAR(20) CHECK (status IN ('Pending', 'Confirmed', 'Completed', 'Cancelled')),
	description TEXT,
   
);

CREATE TABLE Chat (
    id INT PRIMARY KEY IDENTITY(1,1),
    owner_id INT FOREIGN KEY REFERENCES Owners(id) ON DELETE CASCADE,
    vet_id INT FOREIGN KEY REFERENCES Vets(id) ON DELETE CASCADE,
    message TEXT NOT NULL,
   
);

CREATE TABLE Transport_Requests (
    id INT PRIMARY KEY IDENTITY(1,1),
    appointment_id INT FOREIGN KEY REFERENCES Appointments(id) ON DELETE CASCADE,
    pickup_address NVARCHAR(255) NOT NULL,
    dropoff_address NVARCHAR(255) NOT NULL,
    status NVARCHAR(20) CHECK (status IN ('Pending', 'On the way', 'Completed', 'Cancelled')),
   
);

CREATE TABLE Subscription_Packages (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    description TEXT
);

CREATE TABLE User_Subscriptions (
    id INT PRIMARY KEY IDENTITY(1,1),
    owner_id INT FOREIGN KEY REFERENCES Owners(id) ON DELETE CASCADE,
    package_id INT FOREIGN KEY REFERENCES Subscription_Packages(id) ON DELETE CASCADE,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    status NVARCHAR(20) CHECK (status IN ('Active', 'Expired'))
);

CREATE TABLE Blog (
    id INT PRIMARY KEY IDENTITY,
    title NVARCHAR(255) NOT NULL,
    content TEXT NOT NULL,
    image NVARCHAR(255),
    
);

CREATE TABLE Blog_Comments (
    id INT PRIMARY KEY IDENTITY,
    blog_id INT FOREIGN KEY REFERENCES Blog(id) ON DELETE CASCADE,
    owner_id INT FOREIGN KEY REFERENCES Owners(id) ON DELETE CASCADE,
    comment TEXT NOT NULL,
    
);

CREATE TABLE Reviews (
    id INT PRIMARY KEY IDENTITY,
    owner_id INT FOREIGN KEY REFERENCES Owners(id) ON DELETE CASCADE,
    clinic_id INT FOREIGN KEY REFERENCES Clinics(id) ON DELETE CASCADE,
    rating INT CHECK (rating BETWEEN 1 AND 5),
    comment TEXT,
    
);

CREATE TABLE Payments (
    id INT PRIMARY KEY IDENTITY(1,1),
    appointment_id INT FOREIGN KEY REFERENCES Appointments(id) ON DELETE CASCADE,
    owner_id INT FOREIGN KEY REFERENCES Owners(id) ON DELETE CASCADE,
    amount DECIMAL(10,2),
    payment_method NVARCHAR(255),
    payment_status NVARCHAR(255)
);