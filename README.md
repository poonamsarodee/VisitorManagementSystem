# Visitor Management System

A professional Visitor Management System built with **ASP.NET MVC** and **ADO.NET**, allowing organizations to manage visitor entries with photo uploads, validations, and a clean, presentable UI for assignments and interviews.

---

## 🔥 Features

* ✅ Add new visitors with photo upload
* ✅ View detailed visitor information
* ✅ Edit visitor details (excluding photo)
* ✅ Delete visitor entries
* ✅ Strong form validations:

  * Email domain restriction
  * Valid 10-digit phone number (excluding 0–5 as starting digit)
  * Past or current visit date only

---

## 🧰 Tech Stack

| Technology           | Description                 |
| -------------------- | --------------------------- |
| ASP.NET MVC          | Web application framework   |
| ADO.NET              | Data access without ORM     |
| SQL Server           | Backend relational database |
| HTML, CSS, Bootstrap | Frontend styling and layout |

---

## 📂 Folder Structure

```
VisitorManagementSystem/
│
├── Controllers/
│   └── VisitorController.cs
│
├── Models/
│   └── Visitor.cs
│
├── Views/
│   └── Visitor/
│       ├── Create.cshtml
│       ├── Edit.cshtml
│       ├── Index.cshtml
│       ├── Details.cshtml
│
├── Uploads/         <-- Visitor photos saved here
├── Web.config        <-- Database connection string
```

---

## ⚙️ Setup Instructions

1. **Clone the repository:**

   ```bash
   git clone https://github.com/your-username/visitor-management-system.git
   ```

2. **Open the solution** in Visual Studio.

3. **Update Connection String** in `Web.config`:

   ```xml
   <connectionStrings>
     <add name="DefaultConnection" connectionString="Data Source=.;Initial Catalog=YourDB;Integrated Security=True" providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

4. **Create SQL Table:**

   ```sql
   CREATE TABLE Visitors (
       Id INT PRIMARY KEY IDENTITY,
       FullName NVARCHAR(100),
       Email NVARCHAR(100),
       ContactNumber NVARCHAR(15),
       VisitDate DATE,
       Purpose NVARCHAR(200),
       PhotoPath NVARCHAR(200)
   );
   ```

5. **Run the application** and test CRUD functionality.

---


## 🤝 Contribution

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

---

## 📄 License

This project is open-source and free to use for learning and demonstration purposes.

---

## ✍️ Developed by

**Poonam Sarode**

> Full-stack Developer | Passionate about clean, functional UI and efficient backend systems.
