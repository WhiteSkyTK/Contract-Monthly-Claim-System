
# Contract Monthly Claims System

## Overview
The Contract Monthly Claims System is a web application that allows lecturers to submit claims for hours worked, program coordinators and academic managers to verify and reject claims, and users to track the status of their claims. The application features user authentication, a user-friendly interface, and real-time updates on claim status.

## Table of Contents
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Running the Application](#running-the-application)
- [Usage](#usage)
- [Endpoints](#endpoints)
- [Contributing](#contributing)
- [License](#license)

## Prerequisites
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later
- [Visual Studio Code 2022](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads#express)

## Installation
1. Clone the repository to your local machine:
    ```bash
    git clone https://your-repository-url.git
    cd ContractMonthlyClaimsSystem
    ```

2. Restore the NuGet packages:
    ```bash
    dotnet restore
    ```

3. Create the database and apply migrations:
    ```bash
    dotnet ef database update
    ```

4. Execute the SQL query to import modules into the database:
    ```sql
    INSERT INTO Modules (ModuleCode, ModuleName, Description) VALUES
    ('PROG6212', 'Programming 6212', 'Advanced programming techniques.'),
    ('DATA6222', 'Data Analysis 6222', 'Data analysis and visualization.'),
    ('HCIN6221', 'Human-Computer Interaction 6221', 'Design and evaluation of user interfaces.'),
    ('IPMA6212', 'Information Project Management 6212', 'Managing information systems projects.'),
    ('SAND6221', 'Software Engineering 6221', 'Principles of software engineering.'),
    ('WEDE6021', 'Web Development 6021', 'Web application development fundamentals.'),
    ('ITPP5112', 'IT Project Planning 5112', 'IT project planning and execution.'),
    ('OPSY5121', 'Operating Systems 5121', 'Introduction to operating systems.');
    ```

## Running the Application
1. Open Visual Studio Code.
2. Open the terminal in VS Code (View > Terminal or `Ctrl + ``).
3. Navigate to the project directory:
    ```bash
    cd path/to/ContractMonthlyClaimsSystem
    ```
4. Run the application:
    ```bash
    dotnet run
    ```
5. Open a web browser and navigate to:
    ```plaintext
    https://localhost:7038
    ```

## Usage
- **Register a Lecturer:** Go to [Register Lecturer](https://localhost:7038/Home/Register) to register a new lecturer.
- **Register a Programme Coordinator:** Go to [Register Programme Coordinator](https://localhost:7038/Home/RegisterC) to register a new coordinator.
- **Register an Academic Manager:** Go to [Register Academic Manager](https://localhost:7038/Home/RegisterM) to register a new manager.
- **Login:** Use the following example credentials to log in:
    - **Lecturer:** Morenaletta@gmail.com / 12345678
    - **Programme Coordinator:** WarrenHani09@gmail.com / 12345678
    - **Academic Manager:** nogamenolifeshirosora1234@gmail.com / 12345678

- After logging in, you can submit claims, verify claims, and track their statuses.

## Endpoints
- **Register Lecturer:** `/Home/Register`
- **Register Programme Coordinator:** `/Home/RegisterC`
- **Register Academic Manager:** `/Home/RegisterM`
- **Track Claims:** `/Home/TrackClaims`
- **Verify Claims:** `/Home/VerifyClaims`

## Contributing
Contributions are welcome! Please feel free to submit a pull request or open an issue for any bugs or enhancements.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
